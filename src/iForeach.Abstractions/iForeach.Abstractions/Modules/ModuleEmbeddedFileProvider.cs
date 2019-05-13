using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

using org.iForeach.Modules.FileProviders;
using org.iForeach;
using org.iForeach.Environment.Extensions.Hosting;

namespace org.iForeach.Modules
{
    /// <summary>
    /// This custom <see cref="IFileProvider"/> implementation provides the file contents
    /// of embedded files in Module assemblies.
    /// </summary>
    public class ModuleEmbeddedFileProvider : IFileProvider
    {
        private readonly IApplicationContext _applicationContext;
        private readonly HostEnvironmentProxyWrapper _environment;

        public ModuleEmbeddedFileProvider(IApplicationContext applicationContext, HostEnvironmentProxyWrapper environment)
        {
            this._applicationContext = applicationContext;
            this._environment = environment;
        }

        private Application Application => this._applicationContext.Application;

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            if (subpath == null)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            var folder = subpath.NormalizePath();

            var entries = new List<IFileInfo>();

            switch(folder)
            {
                case "":
                    // Under the root.
                    // Add the virtual folder "Areas" containing all modules.
                    entries.Add(new EmbeddedDirectoryInfo(this._environment.ModulesPath));
                    break;
                case var _ when folder == this.Application.ModulePath:
                    // Under "Areas".
                    // Add virtual folders for all modules by using their assembly names (module ids).
                    entries.AddRange(this.Application.Modules.Select(m => new EmbeddedDirectoryInfo(m.Name)));
                    break;
                case var _ when folder.StartsWith(this.Application.ModuleRoot, StringComparison.Ordinal):
                    // Under "Areas/{ModuleId}" or "Areas/{ModuleId}/**".
                    // Skip "Areas/" from the folder path.
                    var path = folder.Substring(this._environment.ModulesPathRoot.Length);
                    var index = path.IndexOf('/');

                    // Resolve the module id and get all its asset paths.
                    var name = index == -1 ? path : path.Substring(0, index);
                    var paths = this.Application.GetModule(name).AssetPaths;

                    // Resolve all files and folders directly under this given folder.
                    (var files, var folders) = folder.ResolveFolderContents(paths);

                    // And add them to the directory contents.
                    entries.AddRange(files.Select(p => this.GetFileInfo(p)));
                    entries.AddRange(folders.Select(n => new EmbeddedDirectoryInfo(n)));
                    break;
            }
            return new EmbeddedDirectoryContents(entries);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (subpath == null)
            {
                return new NotFoundFileInfo(subpath);
            }

            var path = subpath.NormalizePath();

            // "Areas/**/*.*".
            if (path.StartsWith(this._environment.ModulesPathRoot, StringComparison.Ordinal))
            {
                // Skip the "Areas/" root.
                path = path.Substring(this._environment.ModulesPathRoot.Length);
                var index = path.IndexOf('/');

                // "{ModuleId}/**/*.*".
                if (index != -1)
                {
                    // Resolve the module id.
                    var module = path.Substring(0, index);

                    // Skip the module id to resolve the subpath.
                    var fileSubPath = path.Substring(index + 1);

                    // If it is the app's module.
                    if (module == this.Application.Name)
                    {
                        // Serve the file from the physical application root folder.
                        return new PhysicalFileInfo(new FileInfo(this.Application.Root + fileSubPath));
                    }

                    // Get the embedded file info from the module assembly.
                    return this.Application.GetModule(module).GetFileInfo(fileSubPath);
                }
            }

            return new NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}