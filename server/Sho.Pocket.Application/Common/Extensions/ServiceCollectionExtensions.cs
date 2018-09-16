﻿using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.DataAccess.Sql;
using Sho.Pocket.DataAccess.Sql.Assets;
using Sho.Pocket.DataAccess.Sql.AssetTypes;
using Sho.Pocket.DataAccess.Sql.Balances;
using Sho.Pocket.DataAccess.Sql.Currencies;

namespace Sho.Pocket.Application.Common.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbConfiguration, DbConfiguration>();

            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IBalanceService, BalanceService>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<IAssetTypeRepository, AssetTypeRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        }
    }
}