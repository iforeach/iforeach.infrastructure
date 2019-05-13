using System.Collections.Generic;
using System.Threading.Tasks;

using org.iForeach.Environment.Extensions;
using org.iForeach.Environment.Extensions.Features;

namespace org.iForeach.Environment.Shell
{
    public interface IShellFeaturesManager
    {
        Task<IEnumerable<IFeatureInfo>> GetEnabledFeaturesAsync();
        Task<IEnumerable<IFeatureInfo>> GetAlwaysEnabledFeaturesAsync();
        Task<IEnumerable<IFeatureInfo>> GetDisabledFeaturesAsync();
        Task<(IEnumerable<IFeatureInfo> ToDisable, IEnumerable<IFeatureInfo> ToEnable)> UpdateFeaturesAsync(IEnumerable<IFeatureInfo> featuresToDisable,
                                                                                                            IEnumerable<IFeatureInfo> featuresToEnable,
                                                                                                            bool force);
        Task<IEnumerable<IExtensionInfo>> GetEnabledExtensionsAsync();
    }
}
