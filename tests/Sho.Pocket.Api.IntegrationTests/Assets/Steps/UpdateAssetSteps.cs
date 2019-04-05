﻿using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Assets.Managers;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Domain.Entities;
using System;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class UpdateAssetSteps
    {
        private AssetFeatureManager _assetFeatureManager;

        private AddAssetSteps _addAssetSteps;

        AssetUpdateModel _updateModel;

        Asset _updatedAsset;

        public UpdateAssetSteps(AssetFeatureManager assetFeatureManager, AddAssetSteps addAssetSteps)
        {
            _assetFeatureManager = assetFeatureManager;
            _addAssetSteps = addAssetSteps;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I set asset name to (.*), is active (.*)")]
        public void GivenSetAssetName(string assetName, bool isActive)
        {
            _updateModel = new AssetUpdateModel(assetName, isActive);
        }

        [When(@"I update asset")]
        public void WhenIUpdateAsset()
        {
            Guid assetId = _addAssetSteps.CreatedAsset.Id;

            _updatedAsset = _assetFeatureManager.UpdateAsset(assetId, _updateModel);
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