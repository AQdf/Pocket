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

        public ExchangeRateFeatureContext() : base()
        {
            ExchangeRates = new Dictionary<Guid, ExchangeRate>();

            _exchangeRateRepository = _serviceProvider.GetRequiredService<IExchangeRateRepository>();
        }

        public async Task<ExchangeRate> AddExchangeRate(DateTime effectiveDate, string baseCurrency, string counterCurrency, int value)
        {
            ExchangeRate exchangeRate = await _exchangeRateRepository.AlterAsync(effectiveDate, baseCurrency, counterCurrency, value);
            ExchangeRates.Add(exchangeRate.Id, exchangeRate);

            return exchangeRate;
        }
    }
}
