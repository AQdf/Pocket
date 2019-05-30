using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.ExchangeRates;

namespace Sho.Pocket.Application.ExchangeRates.Providers
{
    public class NBUProvider : IExchangeRateProvider
    {
        public string ProviderName { get { return ProviderConstants.NBU_PROVIDER; } }

        private const decimal DEFAULT_VALUE = 1.0M;

        private readonly string _uri;

        public NBUProvider(ExchangeRateProviderOption settings)
        {
            _uri = settings.Uri;
        }

        public async Task<IEnumerable<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            string requestUri = _uri;
            string xml;

            using (HttpClient client = new HttpClient())
            {
                xml = await client.GetStringAsync(requestUri);
            }

            IEnumerable<NBUExchangeRateModel> currencyModels = ParseXmlExchangeRatesResponse(xml, baseCurrencies);

            List<ExchangeRateProviderModel> result = currencyModels
                .Select(m => new ExchangeRateProviderModel(ProviderName, m.Currency, counterCurrency, m.Rate))
                .ToList();

            // Workaround to populate UAH to UAH exchange rate
            if (result.FirstOrDefault(r => r.BaseCurrency == counterCurrency) == null)
            {
                result.Add(new ExchangeRateProviderModel(ProviderName, counterCurrency, counterCurrency, DEFAULT_VALUE));
            }

            return result;
        }

        public Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<NBUExchangeRateModel> ParseXmlExchangeRatesResponse(string xml, List<string> baseCurrencies)
        {
            var xdoc = XDocument.Parse(xml);
            var exchange = xdoc.Element("exchange");
            var rates = exchange.Descendants("currency").ToList();

            List<NBUExchangeRateModel> result = rates
                .Where(i => baseCurrencies.Contains((string)i.Element("cc")))
                .Select(i => new NBUExchangeRateModel(
                    (string) i.Element("cc"),
                    (string) i.Element("rate"),
                    (string) i.Element("exchangedate")))
                .ToList();

            return result;
        }
    }
}
