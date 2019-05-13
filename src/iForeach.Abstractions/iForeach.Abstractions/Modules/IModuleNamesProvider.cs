using System.Collections.Generic;

namespace org.iForeach.Modules
{
    public interface IModuleNamesProvider
    {
        IEnumerable<string> GetModuleNames();
    }
}