using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.ExchangeRates.Abstractions;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.ExchangeRates.Providers
{
    public class DefaultExchangeRateProvider : IExchangeRateProvider
    {
        private const decimal DEFAULT_VALUE = 1.0M;

        public string ProviderName => "Default";

        public Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            List<ExchangeRateProviderModel> result = baseCurrencies
                .Select(bc => new ExchangeRateProviderModel(ProviderName, bc, counterCurrency, DEFAULT_VALUE))
                .ToList();

            return Task.FromResult(result);
        }

        public Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            ExchangeRateProviderModel result = new ExchangeRateProviderModel(ProviderName, baseCurrency, counterCurrency, DEFAULT_VALUE);

            return Task.FromResult(result);
        }
    }
}
