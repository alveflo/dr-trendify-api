using ServiceStack.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace DrTrendify.Persistance.Redis.Extensions
{
    public class RedisConfiguration 
    {
        public string ConnectionString { get; set; }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisDatabase(this IServiceCollection serviceCollection, RedisConfiguration redisConfiguration)
        {
            var manager = new RedisManagerPool(redisConfiguration.ConnectionString);

            serviceCollection.AddSingleton(manager);
            serviceCollection.AddTransient((serviceCollection) =>  manager.GetClient());

            return serviceCollection;
        }
    }
}