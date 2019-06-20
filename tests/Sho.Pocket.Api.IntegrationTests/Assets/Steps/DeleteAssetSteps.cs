﻿using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Application.Assets.Models;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class DeleteAssetSteps
    {
        private AssetViewModel _assetToDelete;

        private readonly AssetFeatureContext _assetFeatureContext;

        private readonly AddAssetSteps _addAssetSteps;

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
            if (_addAssetSteps.CreatedAsset.Name == assetName)
            {
                _assetToDelete = _addAssetSteps.CreatedAsset;
            }
        }

        [When(@"I delete asset (.*)")]
        public async Task WhenIDeleteAsset(string assetName)
        {
            await _assetFeatureContext.DeleteAsset(_assetToDelete.Id, assetName);
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
