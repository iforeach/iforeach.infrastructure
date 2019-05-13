using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;

using org.iForeach.Modules.Manifest;

using org.iForeach.Environment.Extensions.Hosting;

namespace org.iForeach.Modules
{
    public class Module
    {
        //public const string WebRootPath = "wwwroot";
        //public const string WebRoot = WebRootPath + "/";

        private readonly string _baseNamespace;
        private readonly DateTimeOffset _lastModified;
        private readonly IDictionary<string, IFileInfo> _fileInfos = new Dictionary<string, IFileInfo>();

        public Module(IHostEnvironmentProxy environment, string name, bool isApplication = false)
            : this(environment as HostEnvironmentProxyWrapper ?? new HostEnvironmentProxyWrapper(environment), name, isApplication)
        {

        }

        private Module(HostEnvironmentProxyWrapper environment, string name, bool isApplication = false)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                this.Name = name;
                this.SubPath = environment.ModulesPathRoot + this.Name;
                this.Root = this.SubPath + '/';

                this.Assembly = Assembly.Load(new AssemblyName(name));

                this.Assets = this.Assembly.GetCustomAttributes<ModuleAssetAttribute>()
                                           .Select(a => new Asset(a.Asset)).ToArray();

                this.AssetPaths = this.Assets.Select(a => a.ModuleAssetPath).ToArray();

                var moduleInfos = this.Assembly.GetCustomAttributes<ModuleAttribute>();

                this.ModuleInfo = moduleInfos.Where(f => !(f is ModuleMarkerAttribute)).FirstOrDefault()
                                  ?? moduleInfos.Where(f => f is ModuleMarkerAttribute).FirstOrDefault()
                                  ?? new ModuleAttribute { Name = Name };

                var features = this.Assembly.GetCustomAttributes<Manifest.FeatureAttribute>()
                                            .Where(f => !(f is ModuleAttribute))
                                            .ToList();

                this.ModuleInfo.Id = this.Name;

                if (isApplication)
                {
                    this.ModuleInfo.Name = environment.ApplicationModuleName;
                    this.ModuleInfo.Description = "Provides core features defined at the application level";
                    this.ModuleInfo.Priority = int.MinValue.ToString();
                    this.ModuleInfo.Category = "Application";
                    this.ModuleInfo.DefaultTenantOnly = true;

                    if (features.Any())
                    {
                        features.Insert(0, new Manifest.FeatureAttribute()
                        {
                            Id = this.ModuleInfo.Id,
                            Name = this.ModuleInfo.Name,
                            Description = this.ModuleInfo.Description,
                            Priority = this.ModuleInfo.Priority,
                            Category = this.ModuleInfo.Category
                        });
                    }
                }

                this.ModuleInfo.Features.AddRange(features);
            }
            else
            {
                this.Name = this.Root = this.SubPath = string.Empty;
                this.Assets = Enumerable.Empty<Asset>();
                this.AssetPaths = Enumerable.Empty<string>();
                this.ModuleInfo = new ModuleAttribute();
            }

            this._baseNamespace = this.Name + '.';
            this._lastModified = DateTimeOffset.UtcNow;

            if (!string.IsNullOrEmpty(this.Assembly?.Location))
            {
                try
                {
                    this._lastModified = File.GetLastWriteTimeUtc(this.Assembly.Location);
                }
                catch (PathTooLongException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        public string Name { get; }
        public string Root { get; }
        public string SubPath { get; }
        public Assembly Assembly { get; }
        public IEnumerable<Asset> Assets { get; }
        public IEnumerable<string> AssetPaths { get; }
        public ModuleAttribute ModuleInfo { get; }

        public IFileInfo GetFileInfo(string subpath)
        {
            lock(this._fileInfos)
            {
                switch (this._fileInfos.TryGetValue(subpath, out var fileInfo))
                {
                    case true:
                        return fileInfo;
                    case false when !this.AssetPaths.Contains(this.Root + subpath, StringComparer.Ordinal):
                        return new NotFoundFileInfo(subpath);
                    case false when this._fileInfos.TryGetValue(subpath, out fileInfo):
                        return fileInfo;
                    default:
                        var resourcePath = this._baseNamespace + subpath.Replace('/', '>');
                        var fileName = Path.GetFileName(subpath);

                        if (this.Assembly.GetManifestResourceInfo(resourcePath) == null)
                        {
                            return new NotFoundFileInfo(fileName);
                        }

                        return this._fileInfos[subpath] = new EmbeddedResourceFileInfo(this.Assembly,
                                                                                       resourcePath,
                                                                                       fileName,
                                                                                       this._lastModified);
                }
            }
        }
    }
}