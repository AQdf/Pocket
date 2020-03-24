using System;
using System.Threading.Tasks;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.ExchangeRates.Steps
{
    [Binding]
    public class CreateExchangeRateStep
    {
        private readonly ExchangeRateFeatureContext _context;

        public CreateExchangeRateStep(ExchangeRateFeatureContext exchangeRateFeatureContext)
        {
            _context = exchangeRateFeatureContext;
        }

        [Given(@"exchange rate (.*) to (.*) with value (.*), day shift (.*)")]
        public async Task GivenExchangeRateExistsForTodayWithValue(string baseCurrency, string counterCurrency, int value, int dayShift)
        {
            DateTime effectiveDate = DateTime.UtcNow.Date.AddDays(dayShift);

            await _context.ExchangeRateRepository.AlterAsync(effectiveDate, baseCurrency, counterCurrency, value, value);
        }

    }
}
