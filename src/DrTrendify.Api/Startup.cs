using System;
using DrTrendify.AlfaScraper;
using DrTrendify.Core.FeatureToggling;
using DrTrendify.Core.Interfaces;
using DrTrendify.Core.Interfaces.Repositories;
using DrTrendify.Core.Services.GetStockdetailById;
using DrTrendify.Core.Services.GetStockdetails;
using DrTrendify.Core.Services.PopulateStockdetails;
using DrTrendify.FeatureToggling.ConfigCat;
using DrTrendify.FeatureToggling.ConfigCat.Extensions;
using DrTrendify.NovemberScraper;
using DrTrendify.Persistance.Redis.Extensions;
using DrTrendify.Persistance.Redis.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sentry;

namespace DrTrendify.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(o => o.AddPolicy("AllowAll", builder => 
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));

            services.Configure<NovemberScraperConfig>(Configuration.GetSection("NovemberScraperConfig"));
            services.Configure<AlfaScraperConfig>(Configuration.GetSection("AlfaScraperConfig"));

            var redisConfig = new RedisConfiguration();
            Configuration.GetSection("RedisConfiguration").Bind(redisConfig);
            services.AddRedisDatabase(redisConfig);

            var configCatConfig = new ConfigCatConfig();
            Configuration.GetSection("CatConfig").Bind(configCatConfig);

            services.AddConfigCat(configCatConfig);

            services.AddTransient<IStockdetailRepository, StockdetailRepository>();
            services.AddTransient<IStocklistFetcher, NovemberStocklistFetcher>();
            services.AddTransient<IPopulateStockdetailsService, PopulateStockdetailsService>();
            services.AddTransient<IGetStockdetailsService, GetStockdetailsService>();
            services.AddTransient<IGetStockdetailByIdService, GetStockdetailByIdService>();
            services.AddTransient<IBabyrageTrendAnalyzer, BabyrageTrendAnalyzer>();
            services.AddTransient<IFeatureFlagProvider, FeatureFlagProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
