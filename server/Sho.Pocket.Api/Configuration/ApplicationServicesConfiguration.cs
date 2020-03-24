using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Currencies;
using Sho.Pocket.Application.ExchangeRates;
using Sho.Pocket.Application.Features.Balances;
using Sho.Pocket.Application.Features.BankAccounts;
using Sho.Pocket.Application.UserCurrencies;
using Sho.Pocket.Application.Utils.Csv;
using Sho.Pocket.Core.Features.Assets.Abstractions;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.BankAccounts;
using Sho.Pocket.Core.Features.Currencies.Abstractions;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.UserCurrencies.Abstractions;

namespace Sho.Pocket.Api.Configuration
{
    public static class ApplicationServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IBalanceService, BalanceService>();
            services.AddScoped<IBalancesTotalService, BalancesTotalService>();
            services.AddScoped<IBalanceNoteService, BalanceNoteService>();
            services.AddScoped<IBalanceExportService, BalanceExportService>();
            services.AddScoped<IBalanceImportService, BalanceImportService>();
            services.AddScoped<IBalanceTotalCalculator, BalanceTotalCalculator>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IUserCurrencyService, UserCurrencyService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IExchangeRateService, ExchangeRateService>();

            services.AddScoped<ICsvExporter, CsvExporter>();

        }
    }
}
