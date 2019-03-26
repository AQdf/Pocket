using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    internal class UpdateAssetSteps : FeatureStepsBase
    {
        private Asset _asset;

        private AssetDriver _assetDriver;

        public UpdateAssetSteps(AssetDriver assetDriver)
        {
            _assetDriver = assetDriver;
        }

        [Given(@"asset with name (.*) exists in the storage")]
        public void GivenAssetWithParameters(string name)
        {
            Currency currency = _assetDriver.InsertCurrencyToStorage("USD");

            AssetCreateModel createModel = new AssetCreateModel
            {
                Name = name,
                CurrencyId = currency.Id,
                IsActive = true
            };

            _asset = _assetDriver.InsertAssetToStorage(createModel);
        }
        
        [When(@"I update asset name to (.*)")]
        public void WhenIUpdateAssetName(string assetName)
        {
            AssetViewModel viewModel = new AssetViewModel(_asset.Id, assetName, _asset.CurrencyId, _asset.IsActive);

            _assetDriver.UpdateAssetInStorage(viewModel);
        }
        
        [Then(@"asset name updated to (.*)")]
        public void ThenAssetNameUpdated(string assetName)
        {
            List<AssetViewModel> assets = _assetDriver.GetAssets();
            AssetViewModel updatedAsset = assets.FirstOrDefault(a => a.Id == _asset.Id);

            updatedAsset.Should().NotBeNull();
            updatedAsset.Name.Should().Be(assetName);
        }
    }
}
