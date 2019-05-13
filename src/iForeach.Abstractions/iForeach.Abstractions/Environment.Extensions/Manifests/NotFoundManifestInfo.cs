using System;
using System.Collections.Generic;
using System.Linq;

using org.iForeach.Modules.Manifest;

namespace org.iForeach.Environment.Extensions.Manifests
{
    public class NotFoundManifestInfo : IManifestInfo
    {
#pragma warning disable IDE0060 // 删除未使用的参数
        public NotFoundManifestInfo(string subPath)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
        }

        public bool Exists => false;
        public string Name => null;
        public string Description => null;
        public string Type => null;
        public string Author => null;
        public string Website => null;
        public Version Version => null;
        public IEnumerable<string> Tags => Enumerable.Empty<string>();
        public ModuleAttribute ModuleInfo => null;
    }
}
