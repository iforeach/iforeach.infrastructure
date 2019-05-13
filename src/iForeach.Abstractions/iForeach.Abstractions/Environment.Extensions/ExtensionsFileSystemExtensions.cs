using System.IO;

using Microsoft.Extensions.FileProviders;

using org.iForeach.Environment.Extensions.Hosting;

namespace org.iForeach.Environment.Extensions
{
    public static class ExtensionsFileSystemExtensions
    {
        public static IFileInfo GetExtensionFileInfo(this IHostEnvironmentProxy parentFileSystem,
                                                     IExtensionInfo extensionInfo,
                                                     string subPath)
        {
            return parentFileSystem.ContentRootFileProvider.GetFileInfo(Path.Combine(extensionInfo.SubPath,
                                                                                     subPath));
        }
    }
}
