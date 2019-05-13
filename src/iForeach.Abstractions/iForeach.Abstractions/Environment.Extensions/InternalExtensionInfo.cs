using System.Collections.Generic;
using System.IO;
using System.Linq;

using org.iForeach.Environment.Extensions.Features;
using org.iForeach.Environment.Extensions.Manifests;

namespace org.iForeach.Environment.Extensions
{
    public class InternalExtensionInfo : IExtensionInfo
    {
        public InternalExtensionInfo(string subPath)
        {
            this.Id = Path.GetFileName(subPath);
            this.SubPath = subPath;

            this.Manifest = new NotFoundManifestInfo(subPath);
            this.Features = Enumerable.Empty<IFeatureInfo>();
        }

        public string Id { get; }
        public string SubPath { get; }
        public IManifestInfo Manifest { get; }
        public IEnumerable<IFeatureInfo> Features { get; }
        public bool Exists => this.Manifest.Exists;
    }
}