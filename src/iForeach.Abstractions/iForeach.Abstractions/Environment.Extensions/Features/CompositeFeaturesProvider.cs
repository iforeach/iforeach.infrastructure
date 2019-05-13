using System;
using System.Collections.Generic;
using System.Linq;

namespace org.iForeach.Environment.Extensions.Features
{
    public class CompositeFeaturesProvider : IFeaturesProvider
    {
        private readonly IFeaturesProvider[] _featuresProviders;
        public CompositeFeaturesProvider(params IFeaturesProvider[] featuresProviders)
        {
            this._featuresProviders = featuresProviders ?? new IFeaturesProvider[0];
        }
        
        public CompositeFeaturesProvider(IEnumerable<IFeaturesProvider> featuresProviders)
        {
            if (featuresProviders == null)
            {
                throw new ArgumentNullException(nameof(featuresProviders));
            }
            this._featuresProviders = featuresProviders.ToArray();
        }

        public IEnumerable<IFeatureInfo> GetFeatures(IExtensionInfo extensionInfo,
                                                     IManifestInfo manifestInfo)
        {
            List<IFeatureInfo> featureInfos = new List<IFeatureInfo>();

            foreach (var provider in this._featuresProviders)
            {
                featureInfos.AddRange(provider.GetFeatures(extensionInfo, manifestInfo));
            }

            return featureInfos;
        }
    }
}
