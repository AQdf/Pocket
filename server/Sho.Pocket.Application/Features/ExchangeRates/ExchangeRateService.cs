using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.ExchangeRates.Abstractions;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.ExchangeRates
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly IExchangeRateProviderResolver _exchangeRateProviderResolver;

        private readonly IUserCurrencyRepository _userCurrencyRepository;

        public ExchangeRateService(
            IExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository,
            IExchangeRateProviderResolver exchangeRateProviderResolver,
            IUserCurrencyRepository userCurrencyRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;
            _exchangeRateProviderResolver = exchangeRateProviderResolver;
            _userCurrencyRepository = userCurrencyRepository;
        }

        public async Task<List<ExchangeRateModel>> GetExchangeRatesAsync(DateTime effectiveDate)
        {
            IEnumerable<ExchangeRate> existingRates = await _exchangeRateRepository.GetByEffectiveDateAsync(effectiveDate);
            List<ExchangeRateModel> result = existingRates.Select(r => new ExchangeRateModel(r)).ToList();

            return result;
        }

        public async Task<List<ExchangeRateModel>> AddDefaultExchangeRates(Guid userOpenId, DateTime effectiveDate)
        {
            IEnumerable<Currency> currenciesEntities = await _currencyRepository.GetAllAsync();
            IEnumerable<string> currencies = currenciesEntities.Select(c => c.Name);
            IEnumerable<ExchangeRate> existingRates = await _exchangeRateRepository.GetByEffectiveDateAsync(effectiveDate);
            List<string> missingCurrencies = currencies.Except(existingRates.Select(r => r.BaseCurrency)).ToList();

            List<ExchangeRateModel> result = existingRates.Select(r => new ExchangeRateModel(r)).ToList();

            if (missingCurrencies.Any())
            {
                UserCurrency primaryCurrency = await _userCurrencyRepository.GetPrimaryCurrencyAsync(userOpenId);
                IReadOnlyCollection<ExchangeRateProviderModel> providerRates = await TryFetchRatesAsync(primaryCurrency.Currency, missingCurrencies);

                if (providerRates != null)
                {
                    foreach (ExchangeRateProviderModel rate in providerRates)
                    {
                        ExchangeRate exchangeRate = await _exchangeRateRepository.AlterAsync(effectiveDate, rate.BaseCurrency, primaryCurrency.Currency, rate.Buy, rate.Sell, rate.Provider);
                        result.Add(new ExchangeRateModel(exchangeRate));
                    }
                }
            }

            return result;
        }

        public async Task UpdateExchangeRateAsync(ExchangeRateModel model)
        {
            await _exchangeRateRepository.UpdateAsync(model.Id, model.Buy, model.Sell);
        }

        public async Task<IReadOnlyCollection<ExchangeRateProviderModel>> FetchProviderExchangeRateAsync(Guid userOpenId, string providerName)
        {
            IEnumerable<Currency> currenciesEntities = await _currencyRepository.GetAllAsync();
            List<string> currencies = currenciesEntities.Select(c => c.Name).ToList();
            UserCurrency primaryCurrency = await _userCurrencyRepository.GetPrimaryCurrencyAsync(userOpenId);

            IExchangeRateProvider provider = _exchangeRateProviderResolver.Resolve(providerName);
            IReadOnlyCollection<ExchangeRateProviderModel> result = await provider.FetchCurrencyRatesAsync(currencies, primaryCurrency.Currency);

            return result;
        }

        // TODO: Refactor this
        private async Task<IReadOnlyCollection<ExchangeRateProviderModel>> TryFetchRatesAsync(string primaryCurrency, List<string> currencies)
        {
            IReadOnlyCollection<IExchangeRateProvider> providers = _exchangeRateProviderResolver.GetActiveProviders();

            foreach (IExchangeRateProvider provider in providers)
            {
                try
                {
                    List<ExchangeRateProviderModel> result = await provider.FetchCurrencyRatesAsync(currencies, primaryCurrency);

                    // Workaround to populate UAH to UAH exchange rate
                    if (!result.Any(r => r.BaseCurrency == primaryCurrency && r.CounterCurrency == primaryCurrency))
                    {
                        result.Add(new ExchangeRateProviderModel(provider.ProviderName, primaryCurrency, primaryCurrency, 1.0M, 1.0M));
                    }

                    return result;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return null;
        }
    }
}
