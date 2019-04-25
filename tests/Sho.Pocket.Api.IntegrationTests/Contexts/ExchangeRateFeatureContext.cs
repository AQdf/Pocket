using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class ExchangeRateFeatureContext : FeatureContextBase
    {
        public Dictionary<Guid, ExchangeRate> ExchangeRates { get; set; }

        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly CurrencyFeatureContext _currencyFeatureContext;

        public ExchangeRateFeatureContext(CurrencyFeatureContext currencyFeatureManager) : base()
        {
            ExchangeRates = new Dictionary<Guid, ExchangeRate>();

            _exchangeRateRepository = _serviceProvider.GetRequiredService<IExchangeRateRepository>();
            _currencyFeatureContext = currencyFeatureManager;
        }

        public async Task<ExchangeRate> AddExchangeRate(DateTime effectiveDate, string baseCurrency, string counterCurrency, int value)
        {
            Guid baseCurrencyId = _currencyFeatureContext.Currencies[baseCurrency].Id;
            Guid counterCurrencyId = _currencyFeatureContext.Currencies[counterCurrency].Id;

            ExchangeRate exchangeRate = await _exchangeRateRepository.Alter(effectiveDate, baseCurrencyId, counterCurrencyId, value);
            ExchangeRates.Add(exchangeRate.Id, exchangeRate);

            return exchangeRate;
        }
    }
}
