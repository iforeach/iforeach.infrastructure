using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.FileProviders;

namespace org.iForeach
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class FileProviderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadAllLines(this IFileInfo fileInfo)
        {
            var lines = new List<string>();

            if (fileInfo?.Exists ?? false)
            {
                using (var stream = fileInfo.CreateReadStream())
                using (var reader = new StreamReader(stream))
                {
                    while(reader.ReadLine() is string line)
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// Use a collection of file paths to resolve files and subfolders directly under a given folder.
        /// Paths need to be normalized by using '/' for the directory separator and with no leading '/'.
        /// </summary>
        public static (IEnumerable<string> FilePaths, IEnumerable<string> FolderPaths) ResolveFolderContents(this string folder, IEnumerable<string> normalizedPaths)
        {
            var files = new HashSet<string>(StringComparer.Ordinal);
            var folders = new HashSet<string>(StringComparer.Ordinal);
            folder = folder.EnsureWithFolderSuffix();

            foreach (var path in normalizedPaths.Where(a => a.StartsWith(folder, StringComparison.Ordinal)))
            {
                // Resolve the subpath relative to the folder.
                var subPath = path.Substring(folder.Length);
                switch(subPath.IndexOf('/'))
                {
                    case -1:
                        // If no more slash, it's a file.
                        files.Add(path);
                        break;
                    case var index:
                        // Otherwise add the 1st subfolder path.
                        folders.Add(subPath.Substring(0, index));
                        break;
                }
            }

            return (files, folders);
        }

        /// <summary>
        /// Replace <c>\</c> -> <c>/</c>, <c>//</c> -> <c>/</c>, and then <see cref="string.Trim(char)"/> with <c>/</c> 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NormalizePath(this string path)
        {
            return path.Replace('\\', '/')
                       .Replace("//", "/")
                       .Trim('/');
        }

        /// <summary>
        /// 确认路径以 <c>/</c> 结尾
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string EnsureWithFolderSuffix(this string path)
        {
            if(path[^1] != '/')
            {
                return path + '/';
            }
            return path;
        }

    }
}
