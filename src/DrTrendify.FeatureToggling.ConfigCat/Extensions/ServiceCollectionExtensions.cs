using ConfigCat.Client;
using Microsoft.Extensions.DependencyInjection;

namespace DrTrendify.FeatureToggling.ConfigCat.Extensions
{
    public class ConfigCatConfig
    {
        public string Key { get; set; }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigCat(this IServiceCollection serviceCollection, ConfigCatConfig configCatConfig)
        {
            var client = new ConfigCatClient(configCatConfig.Key);
            
            serviceCollection.AddSingleton<IConfigCatClient>(client);
            return serviceCollection;
        }
    }
}