using System.Collections.Generic;
using System.Threading.Tasks;

using org.iForeach.Environment.Extensions.Features;

namespace org.iForeach.Environment.Shell
{
    public static class ShellFeaturesManagerExtensions
    {
        public static Task<IEnumerable<IFeatureInfo>> EnableFeaturesAsync(this IShellFeaturesManager shellFeaturesManager,
                                                                          IEnumerable<IFeatureInfo> features)
        {
            return shellFeaturesManager.EnableFeaturesAsync(features, false);
        }

        public static async Task<IEnumerable<IFeatureInfo>> EnableFeaturesAsync(this IShellFeaturesManager shellFeaturesManager,
                                                                                IEnumerable<IFeatureInfo> features,
                                                                                bool force)
        {
            return (await shellFeaturesManager.UpdateFeaturesAsync(new IFeatureInfo[0], features, force)).ToEnable;
        }

        public static Task<IEnumerable<IFeatureInfo>> DisableFeaturesAsync(this IShellFeaturesManager shellFeaturesManager,
                                                                           IEnumerable<IFeatureInfo> features)
        {
            return shellFeaturesManager.DisableFeaturesAsync(features, false);
        }

        public static async Task<IEnumerable<IFeatureInfo>> DisableFeaturesAsync(this IShellFeaturesManager shellFeaturesManager,
                                                                                 IEnumerable<IFeatureInfo> features,
                                                                                 bool force)
        {
            return (await shellFeaturesManager.UpdateFeaturesAsync(features, new IFeatureInfo[0], force)).ToDisable;
        }
    }
}
