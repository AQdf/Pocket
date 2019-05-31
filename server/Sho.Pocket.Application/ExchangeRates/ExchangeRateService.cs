using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.Configuration.Models;
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

        private readonly IExchangeRateProviderFactory _exchangeRateProviderFactory;

        public ExchangeRateService(
            IExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository,
            IExchangeRateProviderFactory exchangeRateProviderFactory)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;
            _exchangeRateProviderFactory = exchangeRateProviderFactory;
        }

        public async Task<List<ExchangeRateModel>> AddDefaultExchangeRates(DateTime effectiveDate)
        {
            IEnumerable<Currency> currenciesEntities = await _currencyRepository.GetAllAsync();
            IEnumerable<string> currencies = currenciesEntities.Select(c => c.Name);
            IEnumerable<ExchangeRate> existingRates = await _exchangeRateRepository.GetByEffectiveDateAsync(effectiveDate);
            List<string> missingCurrencies = currencies.Except(existingRates.Select(r => r.BaseCurrency)).ToList();

            IEnumerable<ExchangeRateProviderModel> providerRates = await TryFetchRatesAsync(missingCurrencies);
            List<ExchangeRateModel> savedRates = await SaveRatesAsync(providerRates, effectiveDate);

            List<ExchangeRateModel> result = existingRates.Select(r => new ExchangeRateModel(r)).ToList();
            result.AddRange(savedRates);

            return result;
        }

        public async Task<ExchangeRateModel> AlterExchangeRateAsync(ExchangeRateModel model)
        {
            ExchangeRate exchangeRate = await _exchangeRateRepository.Alter(model.EffectiveDate, model.BaseCurrency, model.CounterCurrency, model.Value);
            ExchangeRateModel result = new ExchangeRateModel(exchangeRate);

            return result;
        }

        public async Task UpdateExchangeRateAsync(ExchangeRateModel model)
        {
            await _exchangeRateRepository.Update(model.Id, model.Value);
        }

        // TODO: Refactor this
        private async Task<IEnumerable<ExchangeRateProviderModel>> TryFetchRatesAsync(List<string> currencies)
        {
            List<ExchangeRateProviderOption> options = _exchangeRateProviderFactory.GetActiveProvidersOptions();

            foreach (ExchangeRateProviderOption option in options)
            {
                try
                {
                    IExchangeRateProvider provider = _exchangeRateProviderFactory.GetProvider(option);
                    IEnumerable<ExchangeRateProviderModel> result = await provider.FetchCurrencyRatesAsync(currencies, CurrencyConstants.UAH);

                    return result;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return null;
        }

        private async Task<List<ExchangeRateModel>> SaveRatesAsync(IEnumerable<ExchangeRateProviderModel> rates, DateTime effectiveDate)
        {
            List<ExchangeRateModel> result = new List<ExchangeRateModel>();

            foreach (ExchangeRateProviderModel rate in rates)
            {
                var exchangeRate = await _exchangeRateRepository.Alter(effectiveDate, rate.BaseCurrency, CurrencyConstants.UAH, rate.Value);
                var model = new ExchangeRateModel(exchangeRate);
                result.Add(model);
            }

            return result;
        }
    }
}
