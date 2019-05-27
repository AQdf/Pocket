﻿using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Currencies;
using Sho.Pocket.Application.DataExport;
using Sho.Pocket.Application.ExchangeRates;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Providers;
using Sho.Pocket.Application.Inventory;
using Sho.Pocket.Application.ItemCategories;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.DataAccess.Sql;
using Sho.Pocket.DataAccess.Sql.Assets;
using Sho.Pocket.DataAccess.Sql.Balances;
using Sho.Pocket.DataAccess.Sql.Currencies;
using Sho.Pocket.DataAccess.Sql.ExchangeRates;
using Sho.Pocket.DataAccess.Sql.Inventory;
using Sho.Pocket.DataAccess.Sql.ItemCategories;

namespace Sho.Pocket.Application.Common.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbConfiguration, DbConfiguration>();

            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            services.AddScoped<IExchangeRateProviderFactory, ExchangeRateProviderFactory>();

            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IBalanceService, BalanceService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IItemCategoryService, ItemCategoryService>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();

            services.AddScoped<ICsvExporter, CsvExporter>();
        }
    }
}
