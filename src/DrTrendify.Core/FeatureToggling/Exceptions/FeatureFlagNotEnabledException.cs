using System;

namespace DrTrendify.Core.FeatureToggling.Exceptions
{
    public class FeatureFlagNotEnabledException : Exception
    {
        public FeatureFlagNotEnabledException(string featureFlagName)
            : base($"Feature flag is not enabled: {featureFlagName}") {
            FeatureFlagName = featureFlagName;
        }

        public string FeatureFlagName { get; }
    }
}