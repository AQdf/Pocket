using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.ExchangeRates
{
    public class ExchangeRateExternalService : IExchangeRateExternalService
    {
        private readonly IExchangeRateProviderResolver _exchangeRateProviderResolver;

        public ExchangeRateExternalService(IExchangeRateProviderResolver exchangeRateProviderResolver)
        {
            _exchangeRateProviderResolver = exchangeRateProviderResolver;
        }

        public async Task<IReadOnlyCollection<ExchangeRateProviderModel>> FetchProviderExchangeRateAsync(
            string providerName,
            string counterCurrency,
            List<string> baseCurrencies)
        {
            IExchangeRateProvider provider = _exchangeRateProviderResolver.Resolve(providerName);

            IReadOnlyCollection<ExchangeRateProviderModel> result = await provider
                .FetchCurrencyRatesAsync(baseCurrencies, counterCurrency);

            return result;
        }

        public async Task<IReadOnlyCollection<ExchangeRateProviderModel>> TryFetchRatesAsync(
            string counterCurrecy,
            List<string> baseCurrencies)
        {
            IReadOnlyCollection<IExchangeRateProvider> providers = _exchangeRateProviderResolver.GetActiveProviders();

            foreach (IExchangeRateProvider provider in providers)
            {
                try
                {
                    List<ExchangeRateProviderModel> result = await provider.FetchCurrencyRatesAsync(baseCurrencies, counterCurrecy);

                    // Workaround to populate same currency exchange rate
                    if (!result.Any(r => r.BaseCurrency == counterCurrecy && r.CounterCurrency == counterCurrecy))
                    {
                        result.Add(new ExchangeRateProviderModel(provider.ProviderName, counterCurrecy, counterCurrecy, 1.0M, 1.0M));
                    }

                    return result;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return null;
        }
    }
}
