using System;
using System.Collections.Generic;
using System.Linq;

namespace org.iForeach.Modules.Manifest
{
    /// <summary>
    /// Defines a Module which is composed of features.
    /// If the Module has only one default feature, it may be defined there.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class ModuleAttribute : FeatureAttribute
    {
        public ModuleAttribute()
        {
        }

        public virtual string Type => "Module";
        public new bool Exists => this.Id != null;

        /// <Summary>
        /// This identifier is overridden at runtime by the assembly name
        /// </Summary>
        public new string Id { get; internal set; }

        /// <Summary>The name of the developer.</Summary>
        public string Author { get; set; } = string.Empty;

        /// <Summary>The URL for the website of the developer.</Summary>
        public string Website { get; set; } = string.Empty;

        /// <Summary>The version number in SemVer format.</Summary>
        public string Version { get; set; } = "0.0";

        /// <Summary>A list of tags.</Summary>
        public string[] Tags { get; set; } = Enumerable.Empty<string>().ToArray();

        public List<FeatureAttribute> Features { get; } = new List<FeatureAttribute>();
    }
}