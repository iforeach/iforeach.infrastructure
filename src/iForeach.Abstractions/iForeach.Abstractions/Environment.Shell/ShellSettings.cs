using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using org.iForeach.Environment.Shell.Configuration;
using org.iForeach.Environment.Shell.Models;

namespace org.iForeach.Environment.Shell
{
    /// <summary>
    /// Represents the minimalistic set of fields stored for each tenant. This model
    /// is obtained from the 'IShellSettingsManager', which by default reads this
    /// from the 'App_Data/tenants.json' file.
    /// </summary>
    public class ShellSettings
    {
        private readonly ShellConfiguration _settings;
        private readonly ShellConfiguration _configuration;

        public ShellSettings()
        {
            this._settings = new ShellConfiguration();
            this._configuration = new ShellConfiguration();
        }

        public ShellSettings(ShellConfiguration settings, ShellConfiguration configuration)
        {
            this._settings = settings;
            this._configuration = configuration;
        }

        public ShellSettings(ShellSettings settings)
        {
            this._settings = new ShellConfiguration(settings._settings);
            this._configuration = new ShellConfiguration(settings.Name, settings._configuration);

            this.Name = settings.Name;
        }

        public string Name { get; set; }

        public string RequestUrlHost
        {
            get => this._settings["RequestUrlHost"];
            set => this._settings["RequestUrlHost"] = value;
        }

        public string RequestUrlPrefix
        {
            get => this._settings["RequestUrlPrefix"]?.Trim(' ', '/');
            set => this._settings["RequestUrlPrefix"] = value;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public TenantState State
        {
            get => this._settings.GetValue<TenantState>("State");
            set => this._settings["State"] = value.ToString();
        }

        [JsonIgnore]
        public IShellConfiguration ShellConfiguration => this._configuration;

        [JsonIgnore]
        public string this[string key]
        {
            get => this._configuration[key];
            set => this._configuration[key] = value;
        }
    }
}
