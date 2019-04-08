using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Assets.Managers;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Domain.Entities;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class DeleteAssetSteps
    {
        private Asset _assetToDelete;

        private AssetFeatureManager _assetFeatureManager;

        private AddAssetSteps _addAssetSteps;

        public DeleteAssetSteps(AssetFeatureManager assetFeatureManager, AddAssetSteps addAssetSteps)
        {
            _assetFeatureManager = assetFeatureManager;
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
            _assetFeatureManager.DeleteAsset(_assetToDelete.Id);
        }
        
        [Then(@"asset deleted")]
        public void ThenAssetDeleted()
        {
            bool exists = _assetFeatureManager.Assets.ContainsKey(_assetToDelete.Id);

            exists.Should().Be(false);
        }

        [Then(@"asset not deleted")]
        public void ThenAssetNotDeleted()
        {
            bool exists = _assetFeatureManager.Assets.ContainsKey(_assetToDelete.Id);

            exists.Should().Be(true);
        }
    }
}
