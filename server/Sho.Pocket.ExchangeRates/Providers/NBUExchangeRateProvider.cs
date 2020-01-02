using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.Features.ExchangeRates.Abstractions;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.ExchangeRates.Configuration.Models;

namespace Sho.Pocket.ExchangeRates.Providers
{
    public class NBUExchangeRateProvider : IExchangeRateProvider
    {
        public string ProviderName => "NBU";

        private const decimal DEFAULT_VALUE = 1.0M;

        private readonly string _uri;

        public NBUExchangeRateProvider(ExchangeRateSettings settings)
        {
            ExchangeRateProviderOption option = settings.Providers.FirstOrDefault(o => o.Name.Equals(ProviderName, StringComparison.OrdinalIgnoreCase));

            if (option == null)
            {
                throw new Exception($"{ProviderName} exchange rate provider is not configured");
            }

            _uri = option.Uri;
        }

        public async Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
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

            return result;
        }

        public Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            throw new NotImplementedException();
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
