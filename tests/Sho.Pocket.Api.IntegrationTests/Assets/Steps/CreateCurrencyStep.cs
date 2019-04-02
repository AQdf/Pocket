using Sho.Pocket.Api.IntegrationTests.Common;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class CreateCurrencyStep
    {
        private AssetFeatureManager _assetFeatureManager;

        public CreateCurrencyStep(AssetFeatureManager assetFeatureManager)
        {
            _assetFeatureManager = assetFeatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"currency (.*) exists")]
        public void GivenCurrency(string currencyName)
        {
            _assetFeatureManager.AddCurrency(currencyName);
        }
    }
}
