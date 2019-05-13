using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;

using org.iForeach.Runtime;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class StartupActions
    {
        public StartupActions(int order)
        {
            this.Order = order;
        }

        public int Order { get; }

        public ICollection<Action<IServiceCollection, IServiceProvider>> ConfigureServicesActions { get; } = new List<Action<IServiceCollection, IServiceProvider>>();

        public ICollection<Action<IApplicationBuilder, IServiceRouteBuilder, IServiceProvider>> ConfigureActions { get; } = new List<Action<IApplicationBuilder, IServiceRouteBuilder, IServiceProvider>>();
    }
}
