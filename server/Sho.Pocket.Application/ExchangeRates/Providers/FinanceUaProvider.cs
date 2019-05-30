using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.ExchangeRates;

namespace Sho.Pocket.Application.ExchangeRates.Providers
{
    public class FinanceUaProvider : IExchangeRateProvider
    {
        public string ProviderName { get { return ProviderConstants.FINANCE_UA_PROVIDER; } }

        private readonly string _uri;

        public FinanceUaProvider(ExchangeRateProviderOption settings)
        {
            _uri = settings.Uri;
        }

        public async Task<IEnumerable<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            //string requestUri = _uri;
            //string requestJson;

            //using (HttpClient client = new HttpClient())
            //{
            //    requestJson = client.GetStringAsync(requestUri).Result;
            //}

            //return new List<ExchangeRateProviderModel>();

            throw new System.NotImplementedException();
        }

        public async Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            throw new System.NotImplementedException();
        }
    }
}
