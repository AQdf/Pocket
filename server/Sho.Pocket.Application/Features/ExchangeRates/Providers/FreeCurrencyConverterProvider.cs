using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.Configuration.Models;

namespace Sho.Pocket.Application.ExchangeRates.Providers
{
    public class FreeCurrencyConverterProvider : IExchangeRateProvider
    {
        public string ProviderName
        {
            get
            {
                return ProviderConstants.FREE_CURRENCY_PROVIDER;
            }
        }

        private readonly string _apiKey;

        private readonly string _uri;

        public FreeCurrencyConverterProvider(ExchangeRateProviderOption settings)
        {
            _apiKey = settings.ApiKey;
            _uri = settings.Uri;
        }

        public async Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            IEnumerable<Task<ExchangeRateProviderModel>> tasks = baseCurrencies.Select(c => FetchRateAsync(c, counterCurrency));
            ExchangeRateProviderModel[] result = await Task.WhenAll(tasks);

            return result.ToList();
        }

        public async Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            string apiKey = _apiKey;
            string code = $"{baseCurrency}_{counterCurrency}";
            string requestUri = $"{_uri}?q={code}&compact=ultra&apiKey={apiKey}";
            string requestJson;

            using (HttpClient client = new HttpClient())
            {
                requestJson = await client.GetStringAsync(requestUri);
            }

            Dictionary<string, decimal> jsonObject = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(requestJson);
            decimal rate = jsonObject[code];

            ExchangeRateProviderModel result = new ExchangeRateProviderModel(ProviderName, baseCurrency, counterCurrency, rate);

            return result;
        }
    }
}
