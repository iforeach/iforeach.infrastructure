namespace org.iForeach.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        public Asset(string asset)
        {
            asset = asset?.Replace('\\', '/') ?? "";
            var index = asset.IndexOf('|');
            if(index > 0)
            {
                this.ModuleAssetPath = asset.Substring(0, index);
                this.ProjectAssetPath = asset.Substring(index + 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ModuleAssetPath { get; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string ProjectAssetPath { get; } = string.Empty;
    }
}