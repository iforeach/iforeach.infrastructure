using System.Collections.Generic;
using System.Threading.Tasks;

using org.iForeach.Environment.Extensions.Features;
using org.iForeach.Environment.Extensions.Loaders;

namespace org.iForeach.Environment.Extensions
{
    public interface IExtensionManager
    {
        IExtensionInfo GetExtension(string extensionId);
        IEnumerable<IExtensionInfo> GetExtensions();
        Task<ExtensionEntry> LoadExtensionAsync(IExtensionInfo extensionInfo);

        IEnumerable<IFeatureInfo> GetFeatures();
        IEnumerable<IFeatureInfo> GetFeatures(string[] featureIdsToLoad);
        IEnumerable<IFeatureInfo> GetFeatureDependencies(string featureId);
        IEnumerable<IFeatureInfo> GetDependentFeatures(string featureId);
        Task<IEnumerable<FeatureEntry>> LoadFeaturesAsync();
        Task<IEnumerable<FeatureEntry>> LoadFeaturesAsync(string[] featureIdsToLoad);
    }
}
