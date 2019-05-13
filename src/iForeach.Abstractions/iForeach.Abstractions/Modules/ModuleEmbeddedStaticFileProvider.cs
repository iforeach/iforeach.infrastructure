using System.IO;
using System.Linq;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;
using org.iForeach.Environment.Extensions.Hosting;

namespace org.iForeach.Modules
{
    /// <summary>
    /// This custom <see cref="IFileProvider"/> implementation provides the file contents
    /// of embedded files in Module assemblies whose path is under a Module 'wwwroot' folder.
    /// </summary>
    public class ModuleEmbeddedStaticFileProvider : IFileProvider
    {
        private readonly IApplicationContext _applicationContext;
        private readonly HostEnvironmentProxyWrapper _environment;

        public ModuleEmbeddedStaticFileProvider(IApplicationContext applicationContext, HostEnvironmentProxyWrapper environment)
        {
            this._applicationContext = applicationContext;
            this._environment = environment;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return NotFoundDirectoryContents.Singleton;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (subpath == null)
            {
                return new NotFoundFileInfo(subpath);
            }

            var path = this.NormalizePath(subpath);

            var index = path.IndexOf('/');

            // "{ModuleId}/**/*.*".
            if (index != -1)
            {
                var application = this._applicationContext.Application;

                // Resolve the module id.
                var module = path.Substring(0, index);

                // Check if it is an existing module.
                if (application.Modules.Any(m=> m.Name == module))
                {
                    // Resolve the embedded file subpath: "wwwroot/**/*.*"
                    var fileSubPath = this._environment.ApplicationEntryPathRoot + path.Substring(index + 1);

                    if (module != application.Name)
                    {
                        // Get the embedded file info from the module assembly.
                        return application.GetModule(module).GetFileInfo(fileSubPath);
                    }

                    // Application static files can be still requested in a regular way "/**/*.*".
                    // Here, it's done through the Application's module "{ApplicationName}/**/*.*".
                    // But we still serve them from the same physical files "{WebRootPath}/**/*.*".
                    return new PhysicalFileInfo(new FileInfo(application.Root + fileSubPath));
                }
            }

            return new NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        private string NormalizePath(string path)
        {
            return path.Replace('\\', '/').Trim('/').Replace("//", "/");
        }
    }
}