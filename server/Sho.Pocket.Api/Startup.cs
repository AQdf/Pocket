using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Sho.Pocket.Api.Middlewares;
using Sho.Pocket.Api.Validation;
using Sho.Pocket.Application.Configuration;
using Sho.Pocket.Auth.IdentityServer.Configuration;
using Sho.Pocket.Auth.IdentityServer.Configuration.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.DataAccess.Sql;
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
            .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ContractResolver = new
                        CamelCasePropertyNamesContractResolver())
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

            // TODO: Refactor this
            DbSettings dbSettings = new DbSettings();
            ConfigurationBinder.Bind(Configuration.GetSection(nameof(DbSettings)), dbSettings);
            dbSettings.DbConnectionString = Configuration.GetConnectionString("DbConnectionString");
            services.AddSingleton(s => dbSettings);

            // TODO: Refactor this
            AuthSettings authSettings = new AuthSettings();
            ConfigurationBinder.Bind(Configuration.GetSection(nameof(AuthSettings)), authSettings);
            authSettings.UsersDbConnectionString = Configuration.GetConnectionString("UsersDbConnectionString");
            services.AddSingleton(s => authSettings);
            services.AddApplicationAuth(authSettings);

            // TODO: Refactor this
            ExchangeRateSettings exchangeRateSettings = new ExchangeRateSettings();
            ConfigurationBinder.Bind(Configuration.GetSection(nameof(ExchangeRateSettings)), exchangeRateSettings);
            services.AddSingleton(s => exchangeRateSettings);

            services.AddApplicationServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pocket API", Version = "v1" });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IDbConfiguration dbConfiguration,
            IAuthDbInitializer authDbConfiguration)
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

            app.SeedApplicationData(dbConfiguration);
            app.SeedApplicationAuthData(authDbConfiguration);

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
