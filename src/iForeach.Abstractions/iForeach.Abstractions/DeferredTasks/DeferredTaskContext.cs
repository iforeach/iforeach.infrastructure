using System;

namespace org.iForeach.DeferredTasks
{
    public class DeferredTaskContext
    {
        public DeferredTaskContext(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
