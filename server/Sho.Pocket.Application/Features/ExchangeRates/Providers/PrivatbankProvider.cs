using Newtonsoft.Json;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Application.ExchangeRates.Providers;
using Sho.Pocket.Application.Features.ExchangeRates.Models;
using Sho.Pocket.Core.Configuration.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.Features.ExchangeRates.Providers
{
    public class PrivatbankProvider : IExchangeRateProvider
    {
        public string ProviderName { get { return ProviderConstants.PRIVATBANK_PROVIDER; } }

        private readonly string _uri;

        private readonly string[] courceIds = new string[] { "4", "5" }; // 4 - secondary currencies, 5 - main currencies

        public PrivatbankProvider(ExchangeRateProviderOption settings)
        {
            _uri = settings.Uri;
        }

        public async Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            List<ExchangeRateProviderModel> result = new List<ExchangeRateProviderModel>();

            foreach (string coursid in courceIds)
            {
                string requestUri = $"{_uri}?json&exchange&coursid={coursid}";
                string json;

                using (HttpClient client = new HttpClient())
                {
                    json = await client.GetStringAsync(requestUri);
                }

                List<PrivatbankExchangeRateModel> rates = JsonConvert.DeserializeObject<List<PrivatbankExchangeRateModel>>(json);
                result.AddRange(ParseResponse(rates));
            }

            return result;
        }

        public Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            throw new System.NotImplementedException();
        }

        private List<ExchangeRateProviderModel> ParseResponse(List<PrivatbankExchangeRateModel> providerRates)
        {
            List<ExchangeRateProviderModel> rates = new List<ExchangeRateProviderModel>();

            foreach (PrivatbankExchangeRateModel item in providerRates)
            {
                if (!string.IsNullOrWhiteSpace(item.Ccy)
                    && !string.IsNullOrWhiteSpace(item.Base_ccy)
                    && (!string.IsNullOrWhiteSpace(item.Sale)))
                {
                    var rate = new ExchangeRateProviderModel(
                        ProviderName,
                        item.Base_ccy,
                        item.Ccy,
                        decimal.Parse(item.Sale));

                    rates.Add(rate);
                }
            }

            return rates;
        }
    }
}
