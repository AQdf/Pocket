using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Auth.IdentityServer.DataAccess;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Core;
using Sho.Pocket.Core.Auth;
using System;

namespace Sho.Pocket.Auth.IdentityServer
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationIdentityServer(this IServiceCollection services, GlobalSettings settings)
        {
            services.AddScoped<IAuthDbConfiguration, AuthDbConfiguration>();

            services.AddDbContext<ApplicationAuthDataContext>(options => options.UseSqlServer(settings.DbConnectionString));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<ApplicationAuthDataContext>()
                    .AddDefaultTokenProviders();
        }
    }
}
