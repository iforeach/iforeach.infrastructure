using System;
using System.Collections.Generic;

using org.iForeach.Modules.Manifest;

namespace org.iForeach.Environment.Extensions
{
    public interface IManifestInfo
    {
        bool Exists { get; }
        string Name { get; }
        string Description { get; }
        string Type { get; }
        string Author { get; }
        string Website { get; }
        Version Version { get; }
        IEnumerable<string> Tags { get; }
        ModuleAttribute ModuleInfo { get; }
    }
}
