using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Currencies.Steps
{
    [Binding]
    public class CreateCurrencyStep
    {
        private CurrencyFeatureContext _currencyFeatureContext;

        public CreateCurrencyStep(CurrencyFeatureContext currencyFeatureContext)
        {
            _currencyFeatureContext = currencyFeatureContext;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"currency (.*) exists")]
        public void GivenCurrency(string currencyName)
        {
            _currencyFeatureContext.AddCurrency(currencyName);
        }
    }
}
