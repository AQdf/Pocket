using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Application.ExchangeRates.Providers;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Constants;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ExchangeRates
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateService(
            IExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository,
            IExchangeRateProviderFactory exchangeRateProviderFactory)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;

            _exchangeRateProvider = exchangeRateProviderFactory.GetProvider(ProviderConstants.DEFAULT_PROVIDER);
        }

        public async Task<IEnumerable<ExchangeRateModel>> AddDefaultExchangeRates(DateTime effectiveDate)
        {
            List<ExchangeRateModel> result = new List<ExchangeRateModel>();

            IEnumerable<Currency> currenciesEntities = await _currencyRepository.GetAllAsync();
            IEnumerable<string> currencies = currenciesEntities.Select(c => c.Name);

            foreach (string currency in currencies)
            {
                bool exists = await _exchangeRateRepository.Exists(currency, effectiveDate);
                ExchangeRate exchangeRate;

                if (!exists)
                {
                    ExchangeRateProviderModel providerRate = _exchangeRateProvider.FetchRate(currency, CurrencyConstants.UAH);
                    exchangeRate = await _exchangeRateRepository.Alter(effectiveDate, currency, CurrencyConstants.UAH, providerRate.Value);
                }
                else
                {
                    exchangeRate = await _exchangeRateRepository.GetCurrencyExchangeRate(currency, effectiveDate);
                }

                ExchangeRateModel model = new ExchangeRateModel(exchangeRate);
                result.Add(model);
            }

            return result;
        }

        public async Task<ExchangeRateModel> AlterExchangeRate(ExchangeRateModel model)
        {
            ExchangeRate exchangeRate = await _exchangeRateRepository.Alter(model.EffectiveDate, model.BaseCurrency, model.CounterCurrency, model.Value);
            ExchangeRateModel result = new ExchangeRateModel(exchangeRate);

            return result;
        }
    }
}
