using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Application.ExchangeRates.Providers;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<ExchangeRateModel> AddDefaultExchangeRates(DateTime effectiveDate)
        {
            List<ExchangeRateModel> result = new List<ExchangeRateModel>();

            List<Currency> currencies = _currencyRepository.GetAll();
            Currency defaultCurrency = currencies.First(c => c.IsDefault);

            foreach (Currency currency in currencies)
            {
                bool exists = _exchangeRateRepository.Exists(currency.Id, effectiveDate);
                ExchangeRate exchangeRate;

                if (!exists)
                {
                    ExchangeRateProviderModel providerRate = _exchangeRateProvider.FetchRate(currency.Name, defaultCurrency.Name);
                    exchangeRate = _exchangeRateRepository.Alter(effectiveDate, currency.Id, defaultCurrency.Id, providerRate.Value);
                }
                else
                {
                    exchangeRate = _exchangeRateRepository.GetCurrencyExchangeRate(currency.Id, effectiveDate);
                }

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
    }
}
