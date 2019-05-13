using System.Collections.Generic;
using System.Threading.Tasks;

using org.iForeach.Environment.Extensions.Features;
using org.iForeach.Environment.Shell.Descriptor.Models;

namespace org.iForeach.Environment.Shell
{
    public delegate void FeatureDependencyNotificationHandler(string messageFormat,
                                                              IFeatureInfo feature,
                                                              IEnumerable<IFeatureInfo> features);

    public interface IShellDescriptorFeaturesManager
    {
        Task<(IEnumerable<IFeatureInfo>, IEnumerable<IFeatureInfo>)> UpdateFeaturesAsync(ShellDescriptor shellDescriptor,
                                                                                         IEnumerable<IFeatureInfo> featuresToDisable,
                                                                                         IEnumerable<IFeatureInfo> featuresToEnable,
                                                                                         bool force);
    }
}
