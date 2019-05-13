using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;

using org.iForeach;
using org.iForeach.Modules;
using org.iForeach.Runtime;

namespace Microsoft.Extensions.DependencyInjection
{
    public class IforeachCoreBuilder
    {
        private readonly Dictionary<int, StartupActions> _actions = new Dictionary<int, StartupActions>();

        public IforeachCoreBuilder(IServiceCollection services)
        {
            this.ApplicationServices = services;
        }

        public IServiceCollection ApplicationServices { get; }

        public IforeachCoreBuilder RegisterStartup<T>() where T : class, IStartup
        {
            this.ApplicationServices.AddTransient<IStartup, T>();
            return this;
        }

        /// <summary>
        /// This method gets called for each tenant. Use this method to add services to the container.
        /// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="configure">The action to execute when configuring the services for a tenant.</param>
        /// <param name="order">The order of the action to execute. Lower values will be executed first.</param>
        public IforeachCoreBuilder ConfigureServices(Action<IServiceCollection, IServiceProvider> configure, int order = 0)
        {
            this._actions.GetOrAdd(order,
                                   order => new StartupActions(order),
                                   (order, actions) => {
                                       this.ApplicationServices.AddTransient<IStartup>(sp => new StartupActionsStartup(sp.GetRequiredService<IServiceProvider>(),
                                                                                                                       actions,
                                                                                                                       order));
                                   })
                         .ConfigureServicesActions
                         .Add(configure);
            return this;
        }

        /// <summary>
        /// This method gets called for each tenant. Use this method to add services to the container.
        /// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="configure">The action to execute when configuring the services for a tenant.</param>
        /// <param name="order">The order of the action to execute. Lower values will be executed first.</param>
        public IforeachCoreBuilder ConfigureServices(Action<IServiceCollection> configure, int order = 0)
        {
            return this.ConfigureServices((s, sp) => configure(s), order);
        }
        
        /// <summary>
        /// This method gets called for each tenant. Use this method to configure the request's pipeline.
        /// </summary>
        /// <param name="configure">The action to execute when configuring the request's pipeling for a tenant.</param>
        /// <param name="order">The order of the action to execute. Lower values will be executed first.</param>
        public IforeachCoreBuilder Configure(Action<IApplicationBuilder, IServiceRouteBuilder, IServiceProvider> configure, int order = 0)
        {
            if (!this._actions.TryGetValue(order, out var actions))
            {
                actions = this._actions[order] = new StartupActions(order);

                this.ApplicationServices.AddTransient<IStartup>(sp => new StartupActionsStartup(
                    sp.GetRequiredService<IServiceProvider>(), actions, order));
            }

            actions.ConfigureActions.Add(configure);

            return this;
        }

        /// <summary>
        /// This method gets called for each tenant. Use this method to configure the request's pipeline.
        /// </summary>
        /// <param name="configure">The action to execute when configuring the request's pipeling for a tenant.</param>
        /// <param name="order">The order of the action to execute. Lower values will be executed first.</param>
        public IforeachCoreBuilder Configure(Action<IApplicationBuilder, IServiceRouteBuilder> configure, int order = 0)
        {
            return this.Configure((app, routes, sp) => configure(app, routes), order);
        }

        /// <summary>
        /// This method gets called for each tenant. Use this method to configure the request's pipeline.
        /// </summary>
        /// <param name="configure">The action to execute when configuring the request's pipeling for a tenant.</param>
        /// <param name="order">The order of the action to execute. Lower values will be executed first.</param>
        public IforeachCoreBuilder Configure(Action<IApplicationBuilder> configure, int order = 0)
        {
            return this.Configure((app, routes, sp) => configure(app), order);
        }
    }
}
