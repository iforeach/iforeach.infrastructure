using Microsoft.AspNetCore.Builder;

using org.iForeach.Runtime;

namespace org.iForeach.Modules
{
    public interface IModularTenantRouteBuilder
    {
        IServiceRouteBuilder Build(IApplicationBuilder appBuilder);

        void Configure(IServiceRouteBuilder builder);
    }
}