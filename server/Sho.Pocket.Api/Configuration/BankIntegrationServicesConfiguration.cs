using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sho.BankIntegration.Monobank;
using Sho.BankIntegration.Monobank.Services;
using Sho.BankIntegration.Privatbank;
using Sho.BankIntegration.Privatbank.Services;
using Sho.Pocket.BankIntegration;
using Sho.Pocket.BankIntegration.Providers;
using Sho.Pocket.Core.Features.BankIntegration;
using Sho.Pocket.Core.Features.BankIntegration.Models;

namespace Sho.Pocket.Api.Configuration
{
    public static class BankIntegrationServicesConfiguration
    {
        public static void AddBankIntegration(this IServiceCollection services, BankIntegrationSettings banksSettings)
        {
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IBankIntegrationServiceResolver, BankIntegrationServiceResolver>();

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

            services.AddScoped<IBankIntegrationService, MonobankIntegrationService>();
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

            services.AddScoped<IBankIntegrationService, PrivatbankIntegrationService>();
        }
    }
}
