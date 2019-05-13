using org.iForeach.Environment.Shell.Models;

namespace org.iForeach.Environment.Shell
{
    public static class ShellHelper
    {
        public const string DefaultShellName = "Default";

        public static ShellSettings BuildDefaultUninitializedShell = new ShellSettings {
            Name = DefaultShellName,
            State = TenantState.Uninitialized
        };
    }
}