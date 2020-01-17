using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sho.BankIntegration.Monobank;
using Sho.BankIntegration.Monobank.Services;
using Sho.BankIntegration.Privatbank;
using Sho.BankIntegration.Privatbank.Services;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Configuration.Models;
using Sho.Pocket.Application.Currencies;
using Sho.Pocket.Application.ExchangeRates;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.Features.Balances;
using Sho.Pocket.Application.Features.BankAccounts;
using Sho.Pocket.Application.Inventory;
using Sho.Pocket.Application.UserCurrencies;
using Sho.Pocket.Application.Utils.Csv;
using Sho.Pocket.BankIntegration;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Assets.Abstractions;
using Sho.Pocket.Core.Features.BankAccounts.Abstractions;
using Sho.Pocket.Core.Features.Currencies.Abstractions;
using Sho.Pocket.Core.Features.ExchangeRates.Abstractions;
using Sho.Pocket.Core.Features.Inventory.Abstractions;
using Sho.Pocket.Core.Features.UserCurrencies.Abstractions;
using Sho.Pocket.DataAccess.Sql;
using Sho.Pocket.DataAccess.Sql.Assets;
using Sho.Pocket.DataAccess.Sql.Balances;
using Sho.Pocket.DataAccess.Sql.Banks;
using Sho.Pocket.DataAccess.Sql.Currencies;
using Sho.Pocket.DataAccess.Sql.ExchangeRates;
using Sho.Pocket.DataAccess.Sql.Inventory;
using Sho.Pocket.DataAccess.Sql.ItemCategories;
using Sho.Pocket.DataAccess.Sql.UserCurrencies;
using Sho.Pocket.ExchangeRates.Providers;

namespace Sho.Pocket.Application.Configuration
{
    public static class ApplicationServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbConfiguration, DbConfiguration>();

            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IBalanceService, BalanceService>();
            services.AddScoped<IBalancesTotalService, BalancesTotalService>();
            services.AddScoped<IBalanceNoteService, BalanceNoteService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IItemCategoryService, ItemCategoryService>();
            services.AddScoped<IUserCurrencyService, UserCurrencyService>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IAssetBankAccountRepository, AssetBankAccountRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<IBalanceNoteRepository, BalanceNoteRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();
            services.AddScoped<ICsvExporter, CsvExporter>();

            services.AddScoped<IBankAccountSyncService, BankAccountSyncService>();
            services.AddScoped<IBankAccountServiceResolver, BankAccountServiceResolver>();

            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            services.AddScoped<IExchangeRateProvider, NBUExchangeRateProvider>();
            services.AddScoped<IExchangeRateProvider, DefaultExchangeRateProvider>();
            services.AddScoped<IExchangeRateProviderResolver, ExchangeRateProviderResolver>();
        }

        public static void AddBankIntegration(this IServiceCollection services, BankIntegrationSettings banksSettings)
        {
            services.AddMonobankIntegration(banksSettings);
            services.AddPrivatbankIntegration(banksSettings);
        }

        private static void AddMonobankIntegration(this IServiceCollection services, BankIntegrationSettings banksSettings)
        {
            BankSettings settings = banksSettings.Banks.FirstOrDefault(b => MonobankConfig.BANK_NAME.Equals(b.Name, StringComparison.OrdinalIgnoreCase));

            if (settings == null || string.IsNullOrWhiteSpace(settings.ApiUri))
            {
                throw new Exception($"Missing settings for {MonobankConfig.BANK_NAME} integration.");
            }

            services.AddHttpClient<MonobankHttpClient>(c =>
            {
                c.BaseAddress = new Uri(settings.ApiUri);
            });

            services.AddScoped<MonobankAccountService>();
            services.AddScoped<MonobankExchangeRateService>();

            services.AddScoped<IBankAccountService, MonobankAccountServiceAdapter>();
            services.AddScoped<IExchangeRateProvider, MonobankExchangeRateProvider>();
        }

        private static void AddPrivatbankIntegration(this IServiceCollection services, BankIntegrationSettings banksSettings)
        {
            BankSettings settings = banksSettings.Banks.FirstOrDefault(b => PrivatbankConfig.BANK_NAME.Equals(b.Name, StringComparison.OrdinalIgnoreCase));

            if (settings == null || string.IsNullOrWhiteSpace(settings.ApiUri))
            {
                throw new Exception($"Missing settings for {PrivatbankConfig.BANK_NAME} integration.");
            }

            services.AddHttpClient<PrivatbankClient>(c =>
            {
                c.BaseAddress = new Uri(settings.ApiUri);
            });

            services.AddScoped<PrivatbankAccountService>();
            services.AddScoped<PrivatbankExchangeRateService>();

            services.AddScoped<IBankAccountService, PrivatbankAccountServiceAdapter>();
            services.AddScoped<IExchangeRateProvider, PrivatbankExchangeRateProvider>();
        }
    }
}
