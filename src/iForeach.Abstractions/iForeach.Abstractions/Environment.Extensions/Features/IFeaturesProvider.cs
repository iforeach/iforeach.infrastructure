using System.Collections.Generic;

namespace org.iForeach.Environment.Extensions.Features
{
    public interface IFeaturesProvider
    {
        IEnumerable<IFeatureInfo> GetFeatures(IExtensionInfo extensionInfo,
                                              IManifestInfo manifestInfo);
    }
}
