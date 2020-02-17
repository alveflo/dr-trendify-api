using System.Threading.Tasks;

namespace DrTrendify.Core.FeatureToggling
{
    public interface IFeatureFlagProvider
    {
         Task<bool> IsFeatureEnabledAsync(string featureFlagName);
         bool IsFeatureEnabled(string featureFlagName);
         void EnsureFeatureEnabled(string featureFlagName);
         Task EnsureFeatureEnabledAsync(string featureFlagName);
         Task InvalidateCacheAsync();
         void InvalidateCache();

    }
}