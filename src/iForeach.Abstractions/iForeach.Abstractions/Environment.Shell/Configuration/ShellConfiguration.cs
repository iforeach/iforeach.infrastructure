using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using org.iForeach.Environment.Shell.Configuration.Internal;

namespace org.iForeach.Environment.Shell.Configuration
{
    /// <summary>
    /// Holds the tenant <see cref="IConfiguration"/> which is lazily built
    /// from the application configuration 'appsettings.json', the 'App_Data/appsettings.json'
    /// file and then the 'App_Data/Sites/{tenant}/appsettings.json' file.
    /// </summary>
    public class ShellConfiguration : IShellConfiguration
    {
        private IConfigurationRoot _configuration;
        private UpdatableDataProvider _updatableData;
        private readonly IEnumerable<KeyValuePair<string, string>> _initialData;

        private readonly string _name;
        private readonly Func<string, IConfigurationBuilder> _configBuilderFactory;
        private readonly IEnumerable<IConfigurationProvider> _configurationProviders;

        public ShellConfiguration() { }

        public ShellConfiguration(IConfiguration configuration)
        {
            this._configurationProviders = new ConfigurationBuilder().AddConfiguration(configuration)
                                                                     .Build()
                                                                     .Providers;
        }

        public ShellConfiguration(string name, Func<string, IConfigurationBuilder> factory)
        {
            this._name = name;
            this._configBuilderFactory = factory;
        }

        public ShellConfiguration(ShellConfiguration configuration) : this(null, configuration) { }

        public ShellConfiguration(string name, ShellConfiguration configuration)
        {
            this._name = name;

            if (configuration._configuration != null)
            {
                this._configurationProviders = configuration._configuration.Providers
                                                                           .Where(p => !(p is UpdatableDataProvider))
                                                                           .ToArray();

                this._initialData = configuration._updatableData.ToArray();

                return;
            }

            if (name == null)
            {
                this._configurationProviders = configuration._configurationProviders;
                this._initialData = configuration._initialData;
                return;
            }

            this._configBuilderFactory = configuration._configBuilderFactory;
        }

        private void EnsureConfiguration()
        {
            if (this._configuration == null)
            {
                lock (this)
                {
                    if (this._configuration == null)
                    {
                        var providers = new List<IConfigurationProvider>();

                        if (this._configBuilderFactory != null)
                        {
                            providers.AddRange(new ConfigurationBuilder().AddConfiguration(this._configBuilderFactory.Invoke(this._name).Build())
                                                                                               .Build()
                                                                                               .Providers
                                                                                           );
                        }

                        if (this._configurationProviders != null)
                        {
                            providers.AddRange(this._configurationProviders);
                        }

                        this._updatableData = new UpdatableDataProvider(this._initialData ?? Enumerable.Empty<KeyValuePair<string, string>>());

                        providers.Add(this._updatableData);

                        this._configuration = new ConfigurationRoot(providers);
                    }
                }
            }
        }

        /// <summary>
        /// The tenant lazily built <see cref="IConfiguration"/>.
        /// </summary>
        private IConfiguration Configuration
        {
            get
            {
                this.EnsureConfiguration();
                return this._configuration;
            }
        }

        public string this[string key]
        {
            get => this.Configuration[key];
            set
            {
                this.EnsureConfiguration();
                this._updatableData.Set(key, value);
            }
        }

        public IConfigurationSection GetSection(string key)
        {
            return this.Configuration.GetSection(key);
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return this.Configuration.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return this.Configuration.GetReloadToken();
        }
    }
}
