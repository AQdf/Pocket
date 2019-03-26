using FluentAssertions;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets
{
    [Binding]
    public class AddAssetFeatureSteps
    {
        private AssetCreateModel _assetCreateModel;

        private readonly AssetDriver _assetDriver;

        public AddAssetFeatureSteps(AssetDriver assetDriver)
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
            _assetDriver.InsertAssetToStorage(_assetCreateModel);
        }
        
        [Then(@"asset created with name (.*) and currency (.*)")]
        public void AssetAddedToStorage(string assetName, string currencyName)
        {
            List<AssetViewModel> assets = _assetDriver.GetAssets();

            assets.Should().Contain(a => a.Name == assetName && a.CurrencyName == currencyName);
        }

        [AfterScenario]
        public void CleanupStorage()
        {
            _assetDriver.Cleanup();
        }
    }
}
