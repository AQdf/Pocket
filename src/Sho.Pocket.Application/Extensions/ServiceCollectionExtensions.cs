using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.DataAccess.Sql;
using Sho.Pocket.DataAccess.Sql.Assets;

namespace Sho.Pocket.Application.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbConfiguration, DbConfiguration>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IAssetService, AssetService>();
        }
    }
}
