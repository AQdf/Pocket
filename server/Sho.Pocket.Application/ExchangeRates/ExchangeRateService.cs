using Newtonsoft.Json;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ExchangeRates
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly ICurrencyRepository _currencyRepository;

        public ExchangeRateService(
            IExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;
        }

        public List<ExchangeRateModel> AddDefaultExchangeRates(DateTime effectiveDate)
        {
            List<ExchangeRateModel> result = new List<ExchangeRateModel>();

            List<Currency> currencies = _currencyRepository.GetAll();
            Currency defaultCurrency = currencies.First(c => c.IsDefault);

            foreach (var currency in currencies)
            {
                ExchangeRate exchangeRate = _exchangeRateRepository.Alter(effectiveDate, currency.Id, defaultCurrency.Id, 1.0M);

                ExchangeRateModel model = new ExchangeRateModel(exchangeRate);
                result.Add(model);
            }

            return result;
        }

        public ExchangeRateModel AlterExchangeRate(ExchangeRateModel model)
        {
            ExchangeRate exchangeRate = _exchangeRateRepository.Alter(model.EffectiveDate, model.BaseCurrencyId, model.CounterCurrencyId, model.Value);
            ExchangeRateModel result = new ExchangeRateModel(exchangeRate);

            return result;
        }

        private async Task<Dictionary<string, decimal>> GetExchangeRates(IEnumerable<string> currencies)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            using (HttpClient client = new HttpClient())
            {
                foreach (string currency in currencies)
                {
                    string requestUri = $"http://free.currencyconverterapi.com/api/v5/convert?q={currency}_UAH&compact=y";

                    string json = await client.GetStringAsync(requestUri);

                    object rateObject = JsonConvert.DeserializeObject<object>(json);
                    string rateString = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)rateObject).First).Value).First).Value).Value.ToString();
                    decimal rate = decimal.Parse(rateString);

                    result.Add(currency, rate);
                }
            }

            return result;
        }
    }
}
