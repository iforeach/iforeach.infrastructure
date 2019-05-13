using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using org.iForeach.Environment.Shell;
using org.iForeach.Environment.Shell.Builders.Models;
using org.iForeach.Environment.Shell.Models;
using org.iForeach.Modules;

namespace org.iForeach.Hosting.ShellBuilders
{
    /// <summary>
    /// The shell context represents the shell's state that is kept alive
    /// for the whole life of the application
    /// </summary>
    public class ShellContext : IDisposable
    {
        private bool _disposed = false;
        private volatile int _refCount = 0;
        private List<WeakReference<ShellContext>> _dependents;
        private readonly object _synLock = new object();

        public ShellSettings Settings { get; set; }
        public ShellBlueprint Blueprint { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Whether the shell is activated. 
        /// </summary>
        public bool IsActivated { get; set; }

        /// <summary>
        /// The HTTP Request delegate built for this shell.
        /// </summary>
        public RequestDelegate Pipeline { get; set; }

        private bool _placeHolder;

        public class PlaceHolder : ShellContext
        {
            /// <summary>
            /// Used as a place holder for a shell that will be lazily created.
            /// </summary>
            public PlaceHolder()
            {
                this._placeHolder = true;
                this.Released = true;
                this._disposed = true;
            }
        }

        public IServiceScope CreateScope()
        {
            if (this._placeHolder)
            {
                return null;
            }

            var scope = new ServiceScopeWrapper(this);

            // A new scope can be only used on a non released shell.
            if (!this.Released)
            {
                return scope;
            }

            scope.Dispose();

            return null;
        }

        /// <summary>
        /// Whether the <see cref="ShellContext"/> instance is not yet built or has been released, for instance when a tenant is changed.
        /// </summary>
        public bool Released { get; private set; } = false;

        /// <summary>
        /// Returns the number of active scopes on this tenant.
        /// </summary>
        public int ActiveScopes => this._refCount;

        /// <summary>
        /// Mark the <see cref="ShellContext"/> has a candidate to be released.
        /// </summary>
        public void Release()
        {
            if (this.Released == true)
            {
                // Prevent infinite loops with circular dependencies
                return;
            }

            // When a tenant is changed and should be restarted, its shell context is replaced with a new one, 
            // so that new request can't use it anymore. However some existing request might still be running and try to 
            // resolve or use its services. We then call this method to count the remaining references and dispose it 
            // when the number reached zero.

            lock (this._synLock)
            {
                if (this.Released == true)
                {
                    return;
                }

                this.Released = true;

                if (this._dependents != null)
                {

                    foreach (var dependent in this._dependents)
                    {
                        if (dependent.TryGetTarget(out var shellContext))
                        {
                            shellContext.Release();
                        }
                    }
                }

                // A ShellContext is usually disposed when the last scope is disposed, but if there are no scopes
                // then we need to dispose it right away.
                if (this._refCount == 0)
                {
                    this.Dispose();
                }
            }
        }

        /// <summary>
        /// Registers the specified shellContext as a dependency such that they are also reloaded when the current shell context is reloaded.
        /// </summary>
        public void AddDependentShell(ShellContext shellContext)
        {
            // If the dependent is released, nothing to do.
            if (shellContext.Released)
            {
                return;
            }

            // If the dependency is already released.
            if (this.Released)
            {
                // The dependent is released immediately.
                shellContext.Release();
                return;
            }

            lock (this._synLock)
            {
                if (this._dependents == null)
                {
                    this._dependents = new List<WeakReference<ShellContext>>();
                }

                // Remove any previous instance that represent the same tenant in case it has been released (restarted).
                this._dependents.RemoveAll(x => !x.TryGetTarget(out var shell) || shell.Settings.Name == shellContext.Settings.Name);

                this._dependents.Add(new WeakReference<ShellContext>(shellContext));
            }
        }

        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }

        public void Close()
        {
            if (this._disposed)
            {
                return;
            }

            // Disposes all the services registered for this shell
            if (this.ServiceProvider != null)
            {
                (this.ServiceProvider as IDisposable)?.Dispose();
                this.ServiceProvider = null;
            }

            this.IsActivated = false;
            this.Blueprint = null;
            this.Pipeline = null;

            this._disposed = true;
        }

        ~ShellContext()
        {
            this.Close();
        }

        internal class ServiceScopeWrapper : IServiceScope
        {
            private readonly ShellContext _shellContext;
            private readonly IServiceScope _serviceScope;
            private readonly IServiceProvider _existingServices;
            private readonly HttpContext _httpContext;

            public ServiceScopeWrapper(ShellContext shellContext)
            {
                // Prevent the context from being disposed until the end of the scope
                Interlocked.Increment(ref shellContext._refCount);

                this._shellContext = shellContext;

                // The service provider is null if we try to create
                // a scope on a disabled shell or already disposed.
                if (this._shellContext.ServiceProvider == null)
                {
                    throw new ArgumentNullException(nameof(shellContext.ServiceProvider), $"Can't resolve a scope on tenant: {shellContext.Settings.Name}");
                }

                this._serviceScope = shellContext.ServiceProvider.CreateScope();
                this.ServiceProvider = this._serviceScope.ServiceProvider;

                var httpContextAccessor = this.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

                this._httpContext = httpContextAccessor.HttpContext;
                this._existingServices = this._httpContext.RequestServices;
                this._httpContext.RequestServices = this.ServiceProvider;
            }

            public IServiceProvider ServiceProvider { get; }

            /// <summary>
            /// Returns true if the shell context should be disposed consequently to this scope being released.
            /// </summary>
            private bool ScopeReleased()
            {
                // A disabled shell still in use is released by its last scope.
                if (this._shellContext.Settings.State == TenantState.Disabled)
                {
                    if (Interlocked.CompareExchange(ref this._shellContext._refCount, 1, 1) == 1)
                    {
                        this._shellContext.Release();
                    }
                }

                // If the context is still being released, it will be disposed if the ref counter is equal to 0.
                // To prevent this while executing the terminating events, the ref counter is not decremented here.
                if (this._shellContext.Released && Interlocked.CompareExchange(ref this._shellContext._refCount, 1, 1) == 1)
                {
                    var tenantEvents = this._serviceScope.ServiceProvider.GetServices<IModularTenantEvents>();

                    foreach (var tenantEvent in tenantEvents)
                    {
                        tenantEvent.TerminatingAsync().GetAwaiter().GetResult();
                    }

                    foreach (var tenantEvent in tenantEvents.Reverse())
                    {
                        tenantEvent.TerminatedAsync().GetAwaiter().GetResult();
                    }

                    return true;
                }

                return false;
            }

            public void Dispose()
            {
                var disposeShellContext = ScopeReleased();

                this._httpContext.RequestServices = this._existingServices;
                this._serviceScope.Dispose();

                if (disposeShellContext)
                {
                    this._shellContext.Dispose();
                }

                // Decrement the counter at the very end of the scope
                Interlocked.Decrement(ref this._shellContext._refCount);
            }
        }
    }
}
