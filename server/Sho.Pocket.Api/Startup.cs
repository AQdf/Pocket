using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Sho.Pocket.Api.Configuration;
using Sho.Pocket.Api.Middlewares;
using Sho.Pocket.Api.Validation;
using Sho.Pocket.Auth.IdentityServer.Configuration;
using Sho.Pocket.Auth.IdentityServer.Configuration.Models;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Core.Features.BankIntegration.Models;
using Sho.Pocket.ExchangeRates.Configuration.Models;

namespace Sho.Pocket.Api
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
            services.AddOptions();
            services.AddCors(options => options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

            DbSettings dbSettings = new DbSettings();
            Configuration.GetSection("DbSettings").Bind(dbSettings);
            dbSettings.DbConnectionString = Configuration.GetConnectionString("DbConnectionString");

            services.Configure<DbSettings>(settings =>
            {
                Configuration.GetSection("DbSettings").Bind(settings);
                settings.DbConnectionString = Configuration.GetConnectionString("DbConnectionString");
            });

            AuthSettings authSettings = new AuthSettings();
            Configuration.GetSection("AuthSettings").Bind(authSettings);
            authSettings.UsersDbConnectionString = Configuration.GetConnectionString("UsersDbConnectionString");

            services.Configure<AuthSettings>(settings =>
            {
                Configuration.GetSection("AuthSettings").Bind(settings);
                settings.UsersDbConnectionString = Configuration.GetConnectionString("UsersDbConnectionString");
            });

            BankIntegrationSettings banksSettings = new BankIntegrationSettings();
            Configuration.GetSection("BankIntegrationSettings").Bind(banksSettings);
            services.Configure<BankIntegrationSettings>(Configuration.GetSection("BankIntegrationSettings"));

            services.Configure<ExchangeRateSettings>(Configuration.GetSection("ExchangeRateSettings"));

            services.AddApplicationAuth(authSettings);
            services.AddApplicationServices();
            services.AddEntityFrameworkDataAccess(dbSettings);
            services.AddExchangeRatesIntegration();
            services.AddBankIntegration(banksSettings);

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IDbInitializer dbInitializer,
            IAuthDbInitializer authDbInitializer)
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

            app.UseRouting();
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            dbInitializer.EnsureCreated();
            dbInitializer.SeedStorageData();
            authDbInitializer.SeedApplicationAuthData();

            app.UseMiddleware<PocketExceptionMiddleware>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pocket v1");
            });
        }
    }
}
