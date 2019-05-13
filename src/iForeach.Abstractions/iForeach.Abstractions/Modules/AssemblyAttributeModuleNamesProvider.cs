using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using org.iForeach.Modules.Manifest;

using org.iForeach.Environment.Extensions.Hosting;

namespace org.iForeach.Modules
{
    public class AssemblyAttributeModuleNamesProvider : IModuleNamesProvider
    {
        private readonly List<string> _moduleNames;

        public AssemblyAttributeModuleNamesProvider(IHostEnvironmentProxy hostingEnvironment)
        {
            var assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
            this._moduleNames = assembly.GetCustomAttributes<ModuleNameAttribute>()
                                        .Select(m => m.Name)
                                        .ToList();
        }

        public IEnumerable<string> GetModuleNames()
        {
            return this._moduleNames;
        }
    }
}