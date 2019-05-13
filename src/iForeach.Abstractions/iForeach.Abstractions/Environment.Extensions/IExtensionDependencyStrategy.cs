using org.iForeach.Environment.Extensions.Features;

namespace org.iForeach.Environment.Extensions
{
    public interface IExtensionDependencyStrategy
    {
        bool HasDependency(IFeatureInfo observer, IFeatureInfo subject);
    }
}
