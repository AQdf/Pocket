using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using System;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.ExchangeRates.Steps
{
    [Binding]
    public class CreateExchangeRateStep
    {
        private ExchangeRateFeatureContext _exchangeRateFeatureContext;

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
        public void GivenExchangeRateExistsForTodayWithValue(string baseCurrency, string counterCurrency, int value, int dayShift)
        {
            DateTime effectiveDate = DateTime.UtcNow.Date.AddDays(dayShift);

            _exchangeRateFeatureContext.AddExchangeRate(effectiveDate, baseCurrency, counterCurrency, value);
        }

    }
}
