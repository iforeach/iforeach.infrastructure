using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace org.iForeach.Environment.Shell.State
{
    /// <summary>
    /// Represents the state of a feature in a tenant.
    /// </summary>
    public class ShellFeatureState
    {
        public string Id { get; set; }
        public State InstallState { get; set; }
        public State EnableState { get; set; }

        [JsonIgnore]
        public bool IsInstalled { get { return this.InstallState == State.Up; } }
        [JsonIgnore]
        public bool IsEnabled { get { return this.EnableState == State.Up; } }
        [JsonIgnore]
        public bool IsDisabled { get { return this.EnableState == State.Down || this.EnableState == State.Undefined; } }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum State
        {
            Undefined,
            Rising,
            Up,
            Falling,
            Down,
        }
    }
}
