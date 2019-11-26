using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.Features.ExchangeRates.Providers;
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
                case ProviderConstants.FREE_CURRENCY_PROVIDER:
                    result = new FreeCurrencyConverterProvider(providerSettings);
                    break;
                case ProviderConstants.MONOBANK_PROVIDER:
                    result = new MonobankProvider(providerSettings);
                    break;
                case ProviderConstants.PRIVATBANK_PROVIDER:
                    result = new MonobankProvider(providerSettings);
                    break;
                case ProviderConstants.DEFAULT_PROVIDER:
                    result = new DefaultExchangeRateProvider();
                    break;
                default:
                    result = new DefaultExchangeRateProvider();
                    break;
            }

            return result;
        }

        public IExchangeRateProvider GetProvider(string providerName)
        {
            IExchangeRateProvider result;

            ExchangeRateProviderOption providerSettings = GetActiveProvidersOptions()
                .FirstOrDefault(p => string.Equals(p.Name, providerName, StringComparison.InvariantCultureIgnoreCase));

            if (providerSettings != null)
            {
                switch (providerSettings.Name)
                {
                    case ProviderConstants.NBU_PROVIDER:
                        result = new NBUProvider(providerSettings);
                        break;
                    case ProviderConstants.FREE_CURRENCY_PROVIDER:
                        result = new FreeCurrencyConverterProvider(providerSettings);
                        break;
                    case ProviderConstants.MONOBANK_PROVIDER:
                        result = new MonobankProvider(providerSettings);
                        break;
                    case ProviderConstants.PRIVATBANK_PROVIDER:
                        result = new PrivatbankProvider(providerSettings);
                        break;
                    case ProviderConstants.DEFAULT_PROVIDER:
                        result = new DefaultExchangeRateProvider();
                        break;
                    default:
                        result = new DefaultExchangeRateProvider();
                        break;
                }
            }
            else
            {
                throw new Exception("Provider settings not found!");
            }

            return result;
        }
    }
}
