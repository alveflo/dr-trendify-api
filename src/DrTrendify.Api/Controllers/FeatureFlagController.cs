using System.Threading.Tasks;
using DrTrendify.Core.FeatureToggling;
using Microsoft.AspNetCore.Mvc;

namespace DrTrendify.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeatureFlagController : ControllerBase
    {
        private readonly IFeatureFlagProvider _featureFlagProvider;

        public FeatureFlagController(IFeatureFlagProvider featureFlagProvider)
        {
            _featureFlagProvider = featureFlagProvider;
        }

        [HttpGet("{featureFlagName}")]
        public Task<bool> GetValueAsync(string featureFlagName)
        {
            return _featureFlagProvider.IsFeatureEnabledAsync(featureFlagName);
        }

        [HttpGet("invalidate")]
        public async Task<IActionResult> InvalidateCacheAsync()
        {
            await _featureFlagProvider.InvalidateCacheAsync();

            return Ok();
        }
    }
}