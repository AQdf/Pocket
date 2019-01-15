using AutoMapper;
using Newtonsoft.Json;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ExchangeRates
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IMapper _mapper;

        public ExchangeRateService(
            IExchangeRateRepository exchangeRateRepository,
            IMapper mapper)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _mapper = mapper;
        }

        public ExchangeRateModel AddExchangeRate(ExchangeRateModel model)
        {
            ExchangeRate rateToAdd = _mapper.Map<ExchangeRate>(model);
            ExchangeRate exchangeRate = _exchangeRateRepository.Add(rateToAdd);
            ExchangeRateModel result = _mapper.Map<ExchangeRateModel>(exchangeRate);

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
