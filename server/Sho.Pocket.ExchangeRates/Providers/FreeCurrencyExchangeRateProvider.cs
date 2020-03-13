using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.ExchangeRates.Configuration.Models;

namespace Sho.Pocket.ExchangeRates.Providers
{
    public class FreeCurrencyExchangeRateProvider : IExchangeRateProvider
    {
        public string ProviderName => "FreeCurrencyConverter";

        private readonly string _apiKey;

        private readonly string _uri;

        public FreeCurrencyExchangeRateProvider(IOptionsMonitor<ExchangeRateSettings> options)
        {
            ExchangeRateProviderOption option = options.CurrentValue.Providers.FirstOrDefault(o => o.Name.Equals(ProviderName, StringComparison.OrdinalIgnoreCase));

            if (option == null)
            {
                throw new Exception($"{ProviderName} exchange rate provider is not configured");
            }

            _apiKey = option.ApiKey;
            _uri = option.Uri;
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
            string json;

            using (HttpClient client = new HttpClient())
            {
                json = await client.GetStringAsync(requestUri);
            }

            Dictionary<string, decimal> jsonObject = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(json);
            decimal rate = jsonObject[code];

            ExchangeRateProviderModel result = new ExchangeRateProviderModel(ProviderName, baseCurrency, counterCurrency, rate, rate);

            return result;
        }
    }
}
