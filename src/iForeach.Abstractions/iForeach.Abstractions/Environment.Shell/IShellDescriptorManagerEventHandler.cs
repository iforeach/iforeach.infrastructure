using System.Threading.Tasks;

using org.iForeach.Environment.Shell.Descriptor.Models;

namespace org.iForeach.Environment.Shell
{
    /// <summary>
    /// Represent and event handler for shell descriptor.
    /// </summary>
    public interface IShellDescriptorManagerEventHandler
    {
        /// <summary>
        /// Triggered whenever a shell descriptor has changed.
        /// </summary>
        Task Changed(ShellDescriptor descriptor, string tenant);
    }
}
