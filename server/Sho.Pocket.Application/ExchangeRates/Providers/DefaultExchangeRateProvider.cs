using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;

namespace Sho.Pocket.Application.ExchangeRates.Providers
{
    public class DefaultExchangeRateProvider : IExchangeRateProvider
    {
        private const decimal DEFAULT_VALUE = 1.0M;

        public string ProviderName
        {
            get
            {
                return ProviderConstants.DEFAULT_PROVIDER;
            }
        }

        public async Task<IEnumerable<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            List<ExchangeRateProviderModel> result = new List<ExchangeRateProviderModel>();

            foreach (string baseCurrency in baseCurrencies)
            {
                ExchangeRateProviderModel item = new ExchangeRateProviderModel(ProviderName, baseCurrency, counterCurrency, DEFAULT_VALUE);
                result.Add(item);
            }

            return result;
        }

        public async Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            ExchangeRateProviderModel result = new ExchangeRateProviderModel(ProviderName, baseCurrency, counterCurrency, DEFAULT_VALUE);

            return result;
        }
    }
}
