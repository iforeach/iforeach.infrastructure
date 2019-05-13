using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using org.iForeach.Runtime;

namespace org.iForeach.Modules
{
    /// <summary>
    /// Represents a fake Startup class that is composed of Configure and ConfigureServices lambdas.
    /// </summary>
    internal class StartupActionsStartup : StartupBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly StartupActions _actions;

        public StartupActionsStartup(IServiceProvider serviceProvider, StartupActions actions, int order)
        {
            this._serviceProvider = serviceProvider;
            this._actions = actions;
            this.Order = order;
        }

        public override int Order { get; }

        public override void ConfigureServices(IServiceCollection services)
        {
            foreach (var configureServices in this._actions.ConfigureServicesActions)
            {
                configureServices?.Invoke(services, this._serviceProvider);
            }
        }

        public override void Configure(IApplicationBuilder app, IServiceRouteBuilder routes, IServiceProvider serviceProvider)
        {
            foreach (var configure in this._actions.ConfigureActions)
            {
                configure?.Invoke(app, routes, serviceProvider);
            }
        }
    }
}
