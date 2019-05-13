using System;

namespace org.iForeach.BackgroundTasks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class BackgroundTaskAttribute : Attribute
    {
        public bool Enable { get; set; } = true;
        public string Schedule { get; set; } = "*/5 * * * *";
        public string Description { get; set; } = string.Empty;
    }
}
