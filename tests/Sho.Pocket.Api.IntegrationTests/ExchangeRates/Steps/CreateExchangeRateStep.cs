using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.ExchangeRates.Managers;
using System;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.ExchangeRates.Steps
{
    [Binding]
    public class CreateExchangeRateStep
    {
        private ExchangeRateFeatureManager _exchangeRateFeatureManager;

        public CreateExchangeRateStep(ExchangeRateFeatureManager exchangeRateFeatureManager)
        {
            _exchangeRateFeatureManager = exchangeRateFeatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"exchange rate (.*) to (.*) with value (.*) for today")]
        public void GivenExchangeRateExistsForTodayWithValue(string baseCurrency, string counterCurrency, int value)
        {
            DateTime today = DateTime.UtcNow.Date;

            _exchangeRateFeatureManager.AddExchangeRate(today, baseCurrency, counterCurrency, value);
        }

    }
}
