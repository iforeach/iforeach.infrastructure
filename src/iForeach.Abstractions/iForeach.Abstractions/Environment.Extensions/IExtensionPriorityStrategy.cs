using org.iForeach.Environment.Extensions.Features;

namespace org.iForeach.Environment.Extensions
{
    public interface IExtensionPriorityStrategy
    {
        int GetPriority(IFeatureInfo feature);
    }
}
