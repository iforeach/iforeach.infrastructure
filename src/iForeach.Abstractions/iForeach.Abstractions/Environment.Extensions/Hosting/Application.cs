using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using org.iForeach.Environment.Extensions.Hosting;

namespace org.iForeach.Modules
{
    public class Application
    {
        private readonly Dictionary<string, Module> _modulesByName;
        private readonly List<Module> _modules;
        private readonly HostEnvironmentProxyWrapper _environment;

        public Application(IHostEnvironmentProxy environment, IEnumerable<Module> modules)
            : this(environment as HostEnvironmentProxyWrapper ?? new HostEnvironmentProxyWrapper(environment), modules)
        {
        }

        private Application(HostEnvironmentProxyWrapper environment, IEnumerable<Module> modules)
        {
            this._environment = environment;
            this.Name = environment.ApplicationName;
            this.Path = environment.ContentRootPath;
            this.Root = this.Path.EnsureWithFolderSuffix();
            this.ModulePath = environment.ModulesPathRoot + this.Name;
            this.ModuleRoot = this.ModulePath.EnsureWithFolderSuffix();

            this.Assembly = Assembly.Load(new AssemblyName(this.Name));

            this._modules = new List<Module>(modules);
            this._modulesByName = this._modules.ToDictionary(m => m.Name, m => m);
        }

        public string Name { get; }
        public string Path { get; }
        public string Root { get; }
        public string ModulePath { get; }
        public string ModuleRoot { get; }
        public Assembly Assembly { get; }
        public IEnumerable<Module> Modules => this._modules;

        public Module GetModule(string name)
        {
            if (!this._modulesByName.TryGetValue(name, out var module))
            {
                return new Module(this._environment, string.Empty);
            }

            return module;
        }
    }
}