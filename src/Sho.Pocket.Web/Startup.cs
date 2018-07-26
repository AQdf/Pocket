using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.BLL.Services;
using Sho.Pocket.Core;
using Sho.Pocket.Core.Configuration;
using Sho.Pocket.Core.Repositories;
using Sho.Pocket.Core.Services;
using Sho.Pocket.Data;
using Sho.Pocket.Data.Repositories;

namespace Sho.Pocket.Web
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
            string connectionString = Configuration.GetConnectionString("PocketLocalDbConnection");



            services.AddCors(options => options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });

            GlobalSettings globalSettings = new GlobalSettings();
            ConfigurationBinder.Bind(Configuration.GetSection("GlobalSettings"), globalSettings);
            services.AddSingleton(s => globalSettings);

            services.AddScoped<IDbConfiguration, DbConfiguration>();

            services.AddScoped<IPeriodSummaryRepository, PeriodSummaryRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();

            services.AddScoped<ISummaryService, SummaryService>();
            services.AddScoped<IAssetService, AssetService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseMvc();

            app.UseCors("AllowAll");
        }
    }
}
