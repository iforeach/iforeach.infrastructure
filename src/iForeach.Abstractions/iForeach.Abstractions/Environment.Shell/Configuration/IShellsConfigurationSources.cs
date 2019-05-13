using Microsoft.Extensions.Configuration;

namespace org.iForeach.Environment.Shell.Configuration
{
    public interface IShellsConfigurationSources
    {
        IConfigurationBuilder AddSources(IConfigurationBuilder builder);
    }

    public static class ShellsConfigurationSourcesExtensions
    {
        public static IConfigurationBuilder AddSources(this IConfigurationBuilder builder, IShellsConfigurationSources sources)
        {
            sources.AddSources(builder);
            return builder;
        }
    }
}
