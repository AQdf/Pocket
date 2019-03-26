using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    internal class DeleteAssetSteps : FeatureStepsBase
    {
        private Asset _asset;

        private AssetDriver _assetDriver;

        public DeleteAssetSteps(AssetDriver assetDriver)
        {
            _assetDriver = assetDriver;
        }

        [Given(@"asset to delete with name (.*) exists in the storage")]
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

        [Given(@"balance of asset exists in the storage")]
        public void GivenBalanceOfAssetExistsInTheStorage()
        {
            _assetDriver.InsertAssetBalance(_asset.Id, _asset.CurrencyId);
        }

        [When(@"I delete asset")]
        public void WhenIDeleteAsset()
        {
            _assetDriver.DeleteAssetFromStorage(_asset.Id);
        }
        
        [Then(@"asset deleted")]
        public void ThenAssetDeleted()
        {
            List<AssetViewModel> assets = _assetDriver.GetAssets();

            assets.Should().NotContain(a => a.Id == _asset.Id);
        }

        [Then(@"asset not deleted")]
        public void ThenAssetNotDeleted()
        {
            List<AssetViewModel> assets = _assetDriver.GetAssets();

            assets.Should().Contain(a => a.Id == _asset.Id);
        }
    }
}
