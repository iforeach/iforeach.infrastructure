namespace org.iForeach.Environment.Extensions.Features
{
    public class InternalFeatureInfo : IFeatureInfo
    {
        public InternalFeatureInfo(string id, IExtensionInfo extensionInfo)
        {
            this.Id = id;
            this.Name = id;
            this.Priority = 0;
            this.Category = null;
            this.Description = null;
            this.DefaultTenantOnly = false;
            this.Extension = extensionInfo;
            this.Dependencies = new string[0];
        }

        public string Id { get; }
        public string Name { get; }
        public int Priority { get; }
        public string Category { get; }
        public string Description { get; }
        public bool DefaultTenantOnly { get; }
        public IExtensionInfo Extension { get; }
        public string[] Dependencies { get; }
    }
}
