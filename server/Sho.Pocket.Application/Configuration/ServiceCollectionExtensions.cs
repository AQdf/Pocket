using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Currencies;
using Sho.Pocket.Application.DataExport;
using Sho.Pocket.Application.ExchangeRates;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Providers;
using Sho.Pocket.Application.Features.Balances;
using Sho.Pocket.Application.Features.BankSync;
using Sho.Pocket.Application.Inventory;
using Sho.Pocket.Application.UserCurrencies;
using Sho.Pocket.BankIntegration.Monobank;
using Sho.Pocket.BankIntegration.Monobank.Abstractions;
using Sho.Pocket.BankIntegration.Privatbank.Abstractions;
using Sho.Pocket.BankIntegration.Privatbank.Services;
using Sho.Pocket.Core.BankIntegration;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.BankAccounts.Abstractions;
using Sho.Pocket.DataAccess.Sql;
using Sho.Pocket.DataAccess.Sql.Assets;
using Sho.Pocket.DataAccess.Sql.Balances;
using Sho.Pocket.DataAccess.Sql.Banks;
using Sho.Pocket.DataAccess.Sql.Currencies;
using Sho.Pocket.DataAccess.Sql.ExchangeRates;
using Sho.Pocket.DataAccess.Sql.Inventory;
using Sho.Pocket.DataAccess.Sql.ItemCategories;
using Sho.Pocket.DataAccess.Sql.UserCurrencies;

namespace Sho.Pocket.Application.Configuration
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
            services.AddScoped<IAccountBankSyncService, AccountBankSyncService>();
            services.AddScoped<IBankAccountServiceResolver, BankAccountServiceResolver>();
            services.AddScoped<ICsvExporter, CsvExporter>();

            services.AddScoped<IMonobankAccountService, MonobankAccountService>();
            services.AddScoped<IPrivatbankAccountService, PrivatbankAccountService>();
        }
    }
}
