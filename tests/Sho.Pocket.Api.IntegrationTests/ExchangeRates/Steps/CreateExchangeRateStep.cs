using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.ExchangeRates.Steps
{
    [Binding]
    public class CreateExchangeRateStep
    {
        private readonly ExchangeRateFeatureContext _exchangeRateFeatureContext;

        public CreateExchangeRateStep(ExchangeRateFeatureContext exchangeRateFeatureContext)
        {
            _exchangeRateFeatureContext = exchangeRateFeatureContext;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"exchange rate (.*) to (.*) with value (.*), day shift (.*)")]
        public async Task GivenExchangeRateExistsForTodayWithValue(string baseCurrency, string counterCurrency, int value, int dayShift)
        {
            DateTime effectiveDate = DateTime.UtcNow.Date.AddDays(dayShift);

            await _exchangeRateFeatureContext.AddExchangeRate(effectiveDate, baseCurrency, counterCurrency, value);
        }

    }
}
