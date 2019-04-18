using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core;

namespace Sho.Pocket.Application.ExchangeRates.Providers
{
    public class FreeCurrencyConverterProvider : IExchangeRateProvider
    {
        public string ProviderName
        {
            get
            {
                return ProviderConstants.DEFAULT_PROVIDER;
            }
        }

        private readonly string _apiKey;

        public FreeCurrencyConverterProvider(GlobalSettings settings)
        {
            _apiKey = settings.FreeCurrencyConverterApiKey;
        }

        public List<ExchangeRateProviderModel> FetchCurrencyRates(List<string> baseCurrencies, string counterCurrency)
        {
            List<ExchangeRateProviderModel> result = baseCurrencies.Select(c => FetchRate(c, counterCurrency)).ToList();

            return result;
        }

        public ExchangeRateProviderModel FetchRate(string baseCurrency, string counterCurrency)
        {
            string apiKey = _apiKey;
            string code = $"{baseCurrency}_{counterCurrency}";
            string requestUri = $"https://free.currencyconverterapi.com/api/v6/convert?q={code}&compact=ultra&apiKey={apiKey}";
            string requestJson;

            using (HttpClient client = new HttpClient())
            {
                requestJson = client.GetStringAsync(requestUri).Result;
            }

            Dictionary<string, decimal> jsonObject = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(requestJson);
            decimal rate = jsonObject[code];

            ExchangeRateProviderModel result = new ExchangeRateProviderModel(ProviderName, baseCurrency, counterCurrency, rate);

            return result;
        }
    }
}
