using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets
{
    [Binding]
    internal class AddAssetSteps : FeatureStepsBase
    {
        private AssetCreateModel _assetCreateModel;

        private Asset _createdAsset;

        private AssetDriver _assetDriver;

        public AddAssetSteps(AssetDriver assetDriver)
        {
            _assetDriver = assetDriver;
        }

        [Given(@"asset with name (.*) and currency (.*)")]
        public void GivenAssetWithParameters(string assetName, string currencyName)
        {
            Currency currency = _assetDriver.InsertCurrencyToStorage(currencyName);

            _assetCreateModel = new AssetCreateModel
            {
                Name = assetName,
                CurrencyId = currency.Id,
                IsActive = true
            };
        }

        [When(@"I add the asset")]
        public void WhenIAddNewAsset()
        {
            _createdAsset = _assetDriver.InsertAssetToStorage(_assetCreateModel);
        }
        
        [Then(@"asset created with name (.*) and currency (.*)")]
        public void AssetAddedToStorage(string assetName, string currencyName)
        {
            List<AssetViewModel> assets = _assetDriver.GetAssets();
            AssetViewModel newAsset = assets.FirstOrDefault(a => a.Id == _createdAsset.Id);

            newAsset.Should().NotBeNull();
            newAsset.Should().Match<AssetViewModel>(a => a.Name == assetName && a.CurrencyName == currencyName);
        }
    }
}
