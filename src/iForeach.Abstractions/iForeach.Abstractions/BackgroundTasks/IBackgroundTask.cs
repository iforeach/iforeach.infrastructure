using System;
using System.Threading;
using System.Threading.Tasks;

namespace org.iForeach.BackgroundTasks
{
    public interface IBackgroundTask
    {
        Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}
