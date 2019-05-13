using System;

namespace org.iForeach.Environment.Shell.Descriptor.Models
{
    public class ShellFeature : IEquatable<ShellFeature>
    {
        public ShellFeature()
        {
        }

        public ShellFeature(string id, bool alwaysEnabled = false)
        {
            this.Id = id;
            this.AlwaysEnabled = alwaysEnabled;
        }

        public string Id { get; set; }
        public bool AlwaysEnabled { get; set; }

        public bool Equals(ShellFeature other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
