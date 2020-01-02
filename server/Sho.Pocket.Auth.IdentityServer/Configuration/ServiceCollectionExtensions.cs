using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sho.Pocket.Auth.IdentityServer.Configuration.Models;
using Sho.Pocket.Auth.IdentityServer.DataAccess;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Auth.IdentityServer.Utils;
using Sho.Pocket.Core.Auth;
using System;
using System.Text;

namespace Sho.Pocket.Auth.IdentityServer.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationAuth(this IServiceCollection services, AuthSettings settings)
        {
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.JwtKey));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = settings.JwtIssuer;
                options.Audience = settings.JwtAudience;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = settings.JwtIssuer;
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.JwtIssuer,

                    ValidateAudience = false,
                    ValidAudience = settings.JwtAudience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _signingKey,

                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicyConst.SimpleUser, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(RoleConst.Simple, RoleConst.Admin);
                });
                options.AddPolicy(AuthPolicyConst.AdminUser, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(RoleConst.Admin);
                });
            });

            services.AddDbContext<ApplicationAuthDataContext>(options => options.UseSqlServer(settings.UsersDbConnectionString));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<ApplicationAuthDataContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IAuthDbInitializer, AuthDbInitializer>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
        }
    }
}
