using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Application.ExchangeRates.Providers;
using Sho.Pocket.Application.Features.ExchangeRates.Models;
using Sho.Pocket.Core.Configuration.Models;

namespace Sho.Pocket.Application.Features.ExchangeRates.Providers
{
    public class MonobankProvider : IExchangeRateProvider
    {
        public string ProviderName { get { return ProviderConstants.MONOBANK_PROVIDER; } }

        private const decimal DEFAULT_VALUE = 0.0M;

        private readonly string _uri;

        public MonobankProvider(ExchangeRateProviderOption settings)
        {
            _uri = settings.Uri;
        }

        public async Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            string requestUri = _uri;
            string json;

            using (HttpClient client = new HttpClient())
            {
                json = await client.GetStringAsync(requestUri);
            }

            List<MonobankExchangeRateModel> rates = JsonConvert.DeserializeObject<List<MonobankExchangeRateModel>>(json);

            List<ExchangeRateProviderModel> result = ParseResponse(rates);

            return result;
        }

        public Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            throw new System.NotImplementedException();
        }

        private List<ExchangeRateProviderModel> ParseResponse(List<MonobankExchangeRateModel> providerRates)
        {
            List<ExchangeRateProviderModel> rates = new List<ExchangeRateProviderModel>();

            foreach (MonobankExchangeRateModel item in providerRates)
            {
                if (_currencyCodes.ContainsKey(item.CurrencyCodeA) 
                    && _currencyCodes.ContainsKey(item.CurrencyCodeB)
                    && (!string.IsNullOrWhiteSpace(item.RateSell) || !string.IsNullOrWhiteSpace(item.RateCross)))
                {
                    var rate = new ExchangeRateProviderModel(
                        ProviderName,
                        _currencyCodes[item.CurrencyCodeB],
                        _currencyCodes[item.CurrencyCodeA],
                        ParseExchangeRateValue(item));

                    rates.Add(rate);
                }
            }

            return rates;
        }

        private decimal ParseExchangeRateValue(MonobankExchangeRateModel rate)
        {
            if (!string.IsNullOrWhiteSpace(rate.RateSell))
            {
                return decimal.Parse(rate.RateSell);
            }

            if (!string.IsNullOrWhiteSpace(rate.RateCross))
            {
                return decimal.Parse(rate.RateCross);
            }

            return DEFAULT_VALUE;
        }

        private readonly Dictionary<string, string> _currencyCodes = new Dictionary<string, string>
        {
            { "840", "USD" },
            { "980", "UAH" },
            { "978", "EUR" },
            { "643", "RUB" },
            { "826", "GBP" },
            { "756", "CHF" },
            { "933", "BYN" },
            { "124", "CAD" },
            { "203", "CZK" },
            { "208", "DKK" },
            { "348", "HUF" },
            { "985", "PLN" },
            { "949", "TRY" }
        };
    }
}
