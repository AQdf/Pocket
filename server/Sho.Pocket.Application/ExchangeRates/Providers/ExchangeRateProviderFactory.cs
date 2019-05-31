using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Core.Configuration.Models;

namespace Sho.Pocket.Application.ExchangeRates.Providers
{
    public class ExchangeRateProviderFactory : IExchangeRateProviderFactory
    {
        private readonly ExchangeRateSettings _settings;

        public ExchangeRateProviderFactory(IOptions<ExchangeRateSettings> settings)
        {
            _settings = settings.Value;
        }

        public List<ExchangeRateProviderOption> GetActiveProvidersOptions()
        {
            List<ExchangeRateProviderOption> result = _settings.Providers.Where(p => p.IsActive).OrderBy(p => p.Priority).ToList();

            return result;
        }

        public IExchangeRateProvider GetProvider(ExchangeRateProviderOption providerSettings)
        {
            IExchangeRateProvider result;

            switch (providerSettings.Name)
            {
                case ProviderConstants.NBU_PROVIDER:
                    result = new NBUProvider(providerSettings);
                    break;
                case ProviderConstants.DEFAULT_PROVIDER:
                    result = new DefaultExchangeRateProvider();
                    break;
                case ProviderConstants.FREE_CURRENCY_PROVIDER:
                    result = new FreeCurrencyConverterProvider(providerSettings);
                    break;
                case ProviderConstants.FINANCE_UA_PROVIDER:
                    result = new FinanceUaProvider(providerSettings);
                    break;
                default:
                    result = new DefaultExchangeRateProvider();
                    break;
            }

            return result;
        }
    }
}
