using System;

using org.iForeach.Environment.Shell.Builders.Models;

namespace org.iForeach.Environment.Shell.Builders
{
    public interface IShellContainerFactory
    {
        IServiceProvider CreateContainer(ShellSettings settings, ShellBlueprint blueprint);
    }
}