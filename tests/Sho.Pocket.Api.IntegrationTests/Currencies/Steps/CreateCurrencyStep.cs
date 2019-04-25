using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Currencies.Steps
{
    [Binding]
    public class CreateCurrencyStep
    {
        private readonly CurrencyFeatureContext _currencyFeatureContext;

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
        public async Task GivenCurrency(string currencyName)
        {
            await _currencyFeatureContext.AddCurrency(currencyName);
        }
    }
}
