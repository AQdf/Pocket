using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.BankIntegration.Privatbank.Models;

namespace Sho.BankIntegration.Privatbank.Services
{
    public class PrivatbankExchangeRateService
    {
        private readonly string[] currencyTypes = new string[] { "4", "5" }; // 4 - secondary currencies, 5 - main currencies

        private readonly PrivatbankClient _privatbankClient;

        public PrivatbankExchangeRateService(PrivatbankClient privatbankClient)
        {
            _privatbankClient = privatbankClient;
        }

        /// <summary>
        /// Gets exchange rates of Privatbank.
        /// Reference: <https://api.privatbank.ua/#p24/exchange>
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<PrivatbankExchangeRate>> GetBankExchangeRatesAsync()
        {
            List<PrivatbankExchangeRate> result = new List<PrivatbankExchangeRate>();

            foreach (string type in currencyTypes)
            {
                string json = await _privatbankClient.GetPublicDataAsync($"pubinfo?json&exchange&coursid={type}");

                List<PrivatbankExchangeRateResponse> rates = JsonConvert.DeserializeObject<List<PrivatbankExchangeRateResponse>>(json);
                result.AddRange(ParseExchangeRates(rates));
            }

            return result;
        }

        private List<PrivatbankExchangeRate> ParseExchangeRates(List<PrivatbankExchangeRateResponse> providerRates)
        {
            List<PrivatbankExchangeRate> rates = new List<PrivatbankExchangeRate>();

            foreach (PrivatbankExchangeRateResponse item in providerRates)
            {
                if (!string.IsNullOrWhiteSpace(item.Ccy) && !string.IsNullOrWhiteSpace(item.Base_ccy))
                {
                    decimal.TryParse(item.Buy, out decimal buy);
                    decimal.TryParse(item.Sale, out decimal sell);
                    PrivatbankCurrency baseCurrency = PrivatbankCurrency.GetCompatible(item.Ccy);
                    PrivatbankCurrency counterCurrency = PrivatbankCurrency.GetCompatible(item.Base_ccy);

                    rates.Add(new PrivatbankExchangeRate(baseCurrency.Name, counterCurrency.Name, buy, sell));
                }
            }

            return rates;
        }
    }
}
