using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Models.Internal;
using Sho.BankIntegration.Privatbank.Utils;

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
                HttpResponseMessage response = await _privatbankClient.GetPublicDataAsync($"pubinfo?json&exchange&coursid={type}");
                string json = await response.Content.ReadAsStringAsync();
                var ratesResponse = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateResponse>>(json);

                IEnumerable<PrivatbankExchangeRate> rates = ratesResponse
                    .Select(r => new PrivatbankExchangeRate(
                        ISO4217CurrencyCompatibility.GetActive(r.Ccy),
                        ISO4217CurrencyCompatibility.GetActive(r.Base_ccy),
                        r.Buy,
                        r.Sale));

                result.AddRange(rates);
            }

            return result;
        }
    }
}
