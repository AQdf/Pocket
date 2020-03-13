using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.ExchangeRates.Providers
{
    public class FinanceUaExchangeRateProvider : IExchangeRateProvider
    {
        public string ProviderName => "FinanceUa";

        public Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            throw new System.NotImplementedException();
        }

        public Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            throw new System.NotImplementedException();
        }
    }
}
