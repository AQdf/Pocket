using System.Threading.Tasks;
using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Assets.Models;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class AddAssetSteps
    {
        private readonly AssetFeatureContext _assetFeatureContext;

        private AssetCreateModel _assetCreateModel;

        public AssetViewModel CreatedAsset = null;

        public AddAssetSteps(AssetFeatureContext assetFeatureContext)
        {
            _assetFeatureContext = assetFeatureContext;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I have active asset (.*) with currency (.*)")]
        public async Task GivenAsset(string assetName, string currencyName)
        {
            GivenAssetCreateModel(assetName, currencyName, true);
            await WhenIAddNewAsset();
        }

        [Given(@"I specified asset name (.*), currency (.*), is active (.*)")]
        public void GivenAssetCreateModel(string assetName, string currencyName, bool isActive)
        {
            _assetCreateModel = new AssetCreateModel(assetName, currencyName, isActive);
        }

        [When(@"I add the asset")]
        public async Task WhenIAddNewAsset()
        {
            CreatedAsset = await _assetFeatureContext.AddAsset(_assetCreateModel);
        }
        
        [Then(@"asset created")]
        public void ThenAssetCreated()
        {
            CreatedAsset.Should().NotBeNull();
        }

        [Then(@"asset created with name (.*)")]
        public void ThenAssetName(string assetName)
        {
            CreatedAsset.Name.Should().Be(assetName);
        }

        [Then(@"asset created with currency (.*)")]
        public void ThenAssetCurrency(string currencyName)
        {
            CreatedAsset.Currency.Should().Be(currencyName);
        }

        [Then(@"asset created with is active (.*)")]
        public void ThenAssetIsActive(bool isActive)
        {
            CreatedAsset.IsActive.Should().Be(isActive);
        }
    }
}
