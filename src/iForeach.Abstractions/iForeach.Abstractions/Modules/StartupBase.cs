using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using org.iForeach.Runtime;

namespace org.iForeach.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class StartupBase : IStartup
    {
        /// <inheritdoc />
        public virtual int Order { get; } = 0;

        /// <inheritdoc />
        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        /// <inheritdoc />
        public virtual void Configure(IApplicationBuilder app, IServiceRouteBuilder routes, IServiceProvider serviceProvider)
        {
        }
    }
}
