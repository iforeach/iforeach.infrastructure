using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using org.iForeach;
using org.iForeach.Environment.Extensions.Hosting;

namespace org.iForeach.Modules
{
    public interface IApplicationContext
    {
        Application Application { get; }
    }

    public class ModularApplicationContext : IApplicationContext
    {
        private readonly IHostEnvironmentProxy _environment;
        private readonly IEnumerable<IModuleNamesProvider> _moduleNamesProviders;
        private Application _application;
        private static readonly object s_initLock = new object();

        public ModularApplicationContext(IHostEnvironmentProxy environment, IEnumerable<IModuleNamesProvider> moduleNamesProviders)
        {
            this._environment = environment;
            this._moduleNamesProviders = moduleNamesProviders;
        }

        public Application Application
        {
            get
            {
                this.EnsureInitialized();
                return this._application;
            }
        }

        private void EnsureInitialized()
        {
            s_initLock.LockForCondition(() => this._application == null,
                                        () => this._application = new Application(this._environment, this.GetModules()));
        }

        private IEnumerable<Module> GetModules()
        {
            var modules = new ConcurrentBag<Module>
            {
                new Module(this._environment, this._environment.ApplicationName, true)
            };

            var names = this._moduleNamesProviders.SelectMany(p => p.GetModuleNames())
                                                  .Where(n => n != this._environment.ApplicationName)
                                                  .Distinct();

            Parallel.ForEach(names,
                             new ParallelOptions { MaxDegreeOfParallelism = 8 },
                             name => modules.Add(new Module(this._environment, name, false)));

            return modules;
        }
    }
}