using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Models.Internal;
using Sho.BankIntegration.Monobank.Utils;

namespace Sho.BankIntegration.Monobank.Services
{
    public class MonobankExchangeRateService
    {
        private readonly MonobankHttpClient _monobankClient;

        public MonobankExchangeRateService(MonobankHttpClient monobankClient)
        {
            _monobankClient = monobankClient;
        }

        /// <summary>
        /// Gets exchange rates of Monobank. Data is cached and updated no more than 1 time for 5 minutes.
        /// Reference: <https://api.monobank.ua/docs/#tag-------------->
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<MonobankExchangeRate>> GetBankExchangeRatesAsync()
        {
            HttpResponseMessage response = await _monobankClient.GetPublicDataAsync("bank/currency");
            string json = await response.Content.ReadAsStringAsync();
            var currenciesInfo = JsonConvert.DeserializeObject<IEnumerable<CurrencyInfo>>(json);

            var rates = currenciesInfo
                .Select(i => new MonobankExchangeRate(i.CurrencyCodeA, i.CurrencyCodeB, i.Date.FromUnixTime(), i.RateBuy, i.RateSell, i.RateCross))
                .ToList();

            return rates;
        }
    }
}
