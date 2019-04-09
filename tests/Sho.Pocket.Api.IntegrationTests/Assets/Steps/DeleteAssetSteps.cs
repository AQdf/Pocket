using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Domain.Entities;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class DeleteAssetSteps
    {
        private Asset _assetToDelete;

        private AssetFeatureContext _assetFeatureContext;

        private AddAssetSteps _addAssetSteps;

        public DeleteAssetSteps(AssetFeatureContext assetFeatureContext, AddAssetSteps addAssetSteps)
        {
            _assetFeatureContext = assetFeatureContext;
            _addAssetSteps = addAssetSteps;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I specified asset to delete (.*)")]
        public void GivenSpecifiedAssetToDelete(string assetName)
        {
            _assetToDelete = _addAssetSteps.CreatedAsset;
        }

        [When(@"I delete asset (.*)")]
        public void WhenIDeleteAsset(string assetName)
        {
            _assetFeatureContext.DeleteAsset(_assetToDelete.Id, assetName);
        }
        
        [Then(@"asset deleted")]
        public void ThenAssetDeleted()
        {
            bool exists = _assetFeatureContext.Assets.ContainsKey(_assetToDelete.Name);

            exists.Should().Be(false);
        }

        [Then(@"asset not deleted")]
        public void ThenAssetNotDeleted()
        {
            bool exists = _assetFeatureContext.Assets.ContainsKey(_assetToDelete.Name);

            exists.Should().Be(true);
        }
    }
}
