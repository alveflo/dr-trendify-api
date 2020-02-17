using System.Threading.Tasks;
using ConfigCat.Client;
using DrTrendify.Core.FeatureToggling;
using DrTrendify.Core.FeatureToggling.Exceptions;

namespace DrTrendify.FeatureToggling.ConfigCat
{
    public class FeatureFlagProvider : IFeatureFlagProvider
    {
        private readonly IConfigCatClient _configCatClient;

        public FeatureFlagProvider(IConfigCatClient configCatClient)
        {
            _configCatClient = configCatClient;
        }

        public Task<bool> IsFeatureEnabledAsync(string featureFlagName)
        {
            return _configCatClient.GetValueAsync(featureFlagName, false);
        }

        public bool IsFeatureEnabled(string featureFlagName)
        {
            return _configCatClient.GetValue(featureFlagName, false);
        }

        public void InvalidateCache()
        {
            _configCatClient.ForceRefresh();
        }

        public Task InvalidateCacheAsync()
        {
            return _configCatClient.ForceRefreshAsync();
        }

        public void EnsureFeatureEnabled(string featureFlagName)
        {
            if (!IsFeatureEnabled(featureFlagName))
                throw new FeatureFlagNotEnabledException(featureFlagName);
        }

        public async Task EnsureFeatureEnabledAsync(string featureFlagName)
        {
            if (!await IsFeatureEnabledAsync(featureFlagName))
                throw new FeatureFlagNotEnabledException(featureFlagName);
        }
    }
}
