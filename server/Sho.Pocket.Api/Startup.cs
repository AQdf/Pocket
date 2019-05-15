using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Common.Configuration;
using Sho.Pocket.Application.Common.Extensions;
using Sho.Pocket.Auth.IdentityServer;
using Sho.Pocket.Core;
using Sho.Pocket.Core.Auth;
using Sho.Pocket.Core.DataAccess;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

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
            services.AddCors(options => options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });

            GlobalSettings globalSettings = new GlobalSettings();
            ConfigurationBinder.Bind(Configuration.GetSection("GlobalSettings"), globalSettings);
            services.AddSingleton(s => globalSettings);

            services.AddApplicationServices();

            services.AddApplicationAuth(globalSettings);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Pocket API", Version = "v1" });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IDbConfiguration dbConfiguration,
            IAuthDbConfiguration authDbConfiguration)
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

            app.UseAuthentication();

            app.SeedApplicationData(dbConfiguration);
            app.SeedApplicationAuthData(authDbConfiguration);

            app.UseMvc();

            app.UseCors("AllowAll");

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
