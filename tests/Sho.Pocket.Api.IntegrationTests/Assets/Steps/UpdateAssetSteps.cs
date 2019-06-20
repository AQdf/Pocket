using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class UpdateAssetSteps
    {
        AssetUpdateModel _updateModel;

        AssetViewModel _updatedAsset;

        private readonly AssetFeatureContext _assetFeatureContext;

        private readonly AddAssetSteps _addAssetSteps;

        public UpdateAssetSteps(AssetFeatureContext assetFeatureContext, AddAssetSteps addAssetSteps)
        {
            _assetFeatureContext = assetFeatureContext;
            _addAssetSteps = addAssetSteps;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I set asset name to (.*), currency (.*), is active (.*)")]
        public void GivenSetAssetName(string assetName, string currencyName, bool isActive)
        {
            _updateModel = new AssetUpdateModel(assetName, currencyName, isActive);
        }

        [When(@"I update asset")]
        public async Task WhenIUpdateAsset()
        {
            Guid assetId = _addAssetSteps.CreatedAsset.Id;

            _updatedAsset = await _assetFeatureContext.UpdateAsset(assetId, _updateModel);
        }

        [Then(@"asset name updated to (.*)")]
        public void ThenAssetNameUpdated(string assetName)
        {
            _updatedAsset.Name.Should().Be(assetName);
        }

        [Then(@"asset is active flag updated to (.*)")]
        public void ThenAssetBecomeNotActive(bool isActive)
        {
            _updatedAsset.IsActive.Should().Be(isActive);
        }
    }
}
