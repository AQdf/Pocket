using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.ExchangeRates;
using Sho.Pocket.ExchangeRates.Providers;

namespace Sho.Pocket.Api.Configuration
{
    public static class ExchangeRateServicesConfiguration
    {
        public static void AddExchangeRatesIntegration(this IServiceCollection services)
        {
            services.AddScoped<IExchangeRateExternalService, ExchangeRateExternalService>();
            services.AddScoped<IExchangeRateProviderResolver, ExchangeRateProviderResolver>();
            services.AddScoped<IExchangeRateProvider, DefaultExchangeRateProvider>();
            services.AddScoped<IExchangeRateProvider, NBUExchangeRateProvider>();
            services.AddScoped<IExchangeRateProvider, MonobankExchangeRateProvider>();
            services.AddScoped<IExchangeRateProvider, PrivatbankExchangeRateProvider>();
        }
    }
}
