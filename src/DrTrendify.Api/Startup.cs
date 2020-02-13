using DrTrendify.Core.Interfaces;
using DrTrendify.Core.Interfaces.Repositories;
using DrTrendify.Core.Services.GetStockdetailById;
using DrTrendify.Core.Services.GetStockdetails;
using DrTrendify.Core.Services.PopulateStockdetails;
using DrTrendify.NovemberScraper;
using DrTrendify.Persistance.Redis.Extensions;
using DrTrendify.Persistance.Redis.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.Configure<NovemberScraperConfig>(Configuration.GetSection("NovemberScraperConfig"));
            services.AddTransient<IStocklistFetcher, NovemberStocklistFetcher>();

            var redisConfig = new RedisConfiguration();
            Configuration.GetSection("RedisConfiguration").Bind(redisConfig);

            services.AddRedisDatabase(redisConfig);
            services.AddTransient<IStockdetailRepository, StockdetailRepository>();

            services.AddTransient<IPopulateStockdetailsService, PopulateStockdetailsService>();
            services.AddTransient<IGetStockdetailsService, GetStockdetailsService>();
            services.AddTransient<IGetStockdetailByIdService, GetStockdetailByIdService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
