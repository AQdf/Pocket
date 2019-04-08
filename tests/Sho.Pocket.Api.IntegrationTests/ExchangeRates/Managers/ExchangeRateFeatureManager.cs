using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Currencies.Managers;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Api.IntegrationTests.ExchangeRates.Managers
{
    public class ExchangeRateFeatureManager : FeatureManagerBase
    {
        public Dictionary<Guid, ExchangeRate> ExchangeRates { get; set; }

        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly CurrencyFeatureManager _currencyFeatureManager;

        public ExchangeRateFeatureManager(CurrencyFeatureManager currencyFeatureManager) : base()
        {
            ExchangeRates = new Dictionary<Guid, ExchangeRate>();

            _exchangeRateRepository = _serviceProvider.GetRequiredService<IExchangeRateRepository>();
            _currencyFeatureManager = currencyFeatureManager;
        }

        public ExchangeRate AddExchangeRate(DateTime effectiveDate, string baseCurrency, string counterCurrency, int value)
        {
            Guid baseCurrencyId = _currencyFeatureManager.Currencies[baseCurrency].Id;
            Guid counterCurrencyId = _currencyFeatureManager.Currencies[counterCurrency].Id;

            ExchangeRate exchangeRate = _exchangeRateRepository.Add(effectiveDate, baseCurrencyId, counterCurrencyId, value);
            ExchangeRates.Add(exchangeRate.Id, exchangeRate);

            return exchangeRate;
        }
    }
}
