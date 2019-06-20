using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;

namespace Sho.Pocket.Application.ExchangeRates.Providers
{
    public class FinanceUaProvider : IExchangeRateProvider
    {
        public string ProviderName { get { return ProviderConstants.FINANCE_UA_PROVIDER; } }

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
