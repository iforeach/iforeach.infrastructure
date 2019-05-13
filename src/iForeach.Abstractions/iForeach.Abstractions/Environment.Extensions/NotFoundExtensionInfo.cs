using System.Collections.Generic;
using System.Linq;

using org.iForeach.Environment.Extensions.Features;
using org.iForeach.Environment.Extensions.Manifests;

namespace org.iForeach.Environment.Extensions
{
    public class NotFoundExtensionInfo : IExtensionInfo
    {
        public NotFoundExtensionInfo(string extensionId)
        {
            this.Features = Enumerable.Empty<IFeatureInfo>();
            this.Id = extensionId;
            this.Manifest = new NotFoundManifestInfo(extensionId);
        }

        public bool Exists => false;

        public IEnumerable<IFeatureInfo> Features { get; }

        public string Id { get; }

        public IManifestInfo Manifest { get; }

        public string SubPath => this.Id;
    }
}
