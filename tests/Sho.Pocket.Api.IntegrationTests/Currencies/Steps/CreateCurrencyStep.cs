using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Currencies.Managers;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Currencies.Steps
{
    [Binding]
    public class CreateCurrencyStep
    {
        private CurrencyFeatureManager _currencyFeatureManager;

        public CreateCurrencyStep(CurrencyFeatureManager currencyFeatureManager)
        {
            _currencyFeatureManager = currencyFeatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"currency (.*) exists")]
        public void GivenCurrency(string currencyName)
        {
            _currencyFeatureManager.AddCurrency(currencyName);
        }
    }
}
