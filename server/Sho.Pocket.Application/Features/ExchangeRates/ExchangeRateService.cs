using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.ExchangeRates
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateExternalService _exchangeRateExternalService;

        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly IUserCurrencyRepository _userCurrencyRepository;

        private readonly IUnitOfWork _unitOfWork;

        public ExchangeRateService(
            IExchangeRateExternalService exchangeRateExternalService,
            IExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository,
            IUserCurrencyRepository userCurrencyRepository,
            IUnitOfWork unitOfWork)
        {
            _exchangeRateExternalService = exchangeRateExternalService;
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;
            _userCurrencyRepository = userCurrencyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ExchangeRateModel>> GetExchangeRatesAsync(Guid userId, DateTime effectiveDate)
        {
            IEnumerable<ExchangeRate> existingRates = await _exchangeRateRepository.GetByEffectiveDateAsync(effectiveDate);

            List<ExchangeRateModel> result = existingRates
                .Select(r => new ExchangeRateModel(r))
                .OrderBy(r => r.BaseCurrency)
                .ToList();

            return result;
        }

        public async Task<List<ExchangeRateModel>> AddExchangeRatesAsync(Guid userId, DateTime effectiveDate)
        {
            IEnumerable<Currency> currenciesEntities = await _currencyRepository.GetAllAsync();
            IEnumerable<string> currencies = currenciesEntities.Select(c => c.Name);
            IEnumerable<ExchangeRate> existingRates = await _exchangeRateRepository.GetByEffectiveDateAsync(effectiveDate);
            List<string> missingCurrencies = currencies.Except(existingRates.Select(r => r.BaseCurrency)).ToList();

            List<ExchangeRateModel> result = existingRates.Select(r => new ExchangeRateModel(r)).ToList();

            if (missingCurrencies.Any())
            {
                UserCurrency primaryCurrency = await _userCurrencyRepository.GetPrimaryCurrencyAsync(userId);

                IReadOnlyCollection<ExchangeRateProviderModel> providerRates = await _exchangeRateExternalService
                    .TryFetchRatesAsync(primaryCurrency.Currency, missingCurrencies);

                if (providerRates != null)
                {
                    foreach (ExchangeRateProviderModel rate in providerRates)
                    {
                        ExchangeRate exchangeRate = await _exchangeRateRepository
                            .AlterAsync(
                                effectiveDate,
                                rate.BaseCurrency,
                                primaryCurrency.Currency,
                                rate.Buy,
                                rate.Sell,
                                rate.Provider);
                        
                        result.Add(new ExchangeRateModel(exchangeRate));
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task UpdateExchangeRateAsync(ExchangeRateModel model)
        {
            await _exchangeRateRepository.UpdateAsync(model.Id, model.Buy, model.Sell);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<ExchangeRateProviderModel>> FetchProviderExchangeRatesAsync(
            Guid userId,
            string provider)
        {
            IEnumerable<Currency> currenciesEntities = await _currencyRepository.GetAllAsync();
            List<string> currencies = currenciesEntities.Select(c => c.Name).ToList();
            UserCurrency primaryCurrency = await _userCurrencyRepository.GetPrimaryCurrencyAsync(userId);

            IReadOnlyCollection<ExchangeRateProviderModel> result = await _exchangeRateExternalService
                .FetchProviderExchangeRateAsync(provider, primaryCurrency.Currency, currencies);

            return result;
        }
    }
}
