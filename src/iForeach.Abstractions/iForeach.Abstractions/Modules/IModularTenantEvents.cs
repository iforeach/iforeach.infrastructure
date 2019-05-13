using System.Threading.Tasks;

namespace org.iForeach.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModularTenantEvents
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ActivatingAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ActivatedAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task TerminatingAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task TerminatedAsync();
    }

    public class ModularTenantEvents : IModularTenantEvents
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Task ActivatedAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Task ActivatingAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Task TerminatedAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Task TerminatingAsync()
        {
            return Task.CompletedTask;
        }
    }
}