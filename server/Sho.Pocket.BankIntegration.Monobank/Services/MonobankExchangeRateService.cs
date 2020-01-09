using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.BankIntegration.Monobank.Models;

namespace Sho.BankIntegration.Monobank.Services
{
    public class MonobankExchangeRateService
    {
        private readonly MonobankClient _monobankClient;

        public MonobankExchangeRateService(MonobankClient monobankClient)
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
            string json = await _monobankClient.GetPublicDataAsync("bank/currency");
            List<MonobankCurrencyInfo> currenciesInfo = JsonConvert.DeserializeObject<List<MonobankCurrencyInfo>>(json);

            IReadOnlyCollection<MonobankExchangeRate> rates = currenciesInfo
                .Select(i => ParseExchangeRate(i))
                .Where(rate => rate != null)
                .ToList();

            return rates;
        }

        private MonobankExchangeRate ParseExchangeRate(MonobankCurrencyInfo item)
        {
            if (MonobankCurrency.TryParse(item.CurrencyCodeA, out MonobankCurrency baseCurrency)
                && MonobankCurrency.TryParse(item.CurrencyCodeB, out MonobankCurrency counterCurrency))
            {
                decimal.TryParse(item.RateBuy, out decimal rateBuy);
                decimal.TryParse(item.RateSell, out decimal rateSell);
                decimal.TryParse(item.RateCross, out decimal rateCross);

                return new MonobankExchangeRate(baseCurrency.Name, counterCurrency.Name, rateBuy, rateSell, rateCross);
            }
            else
            {
                //TODO: Log warning: Exchange rate parsing failed!
                return null;
            }
        }
    }
}
