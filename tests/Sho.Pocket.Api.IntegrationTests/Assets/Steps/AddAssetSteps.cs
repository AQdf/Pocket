using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Assets.Managers;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Currencies.Managers;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Domain.Entities;
using System;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class AddAssetSteps
    {
        private AssetFeatureManager _assetFeatureManager;

        private readonly CurrencyFeatureManager _currencyFatureManager;

        private AssetCreateModel _assetCreateModel;

        public Asset CreatedAsset = null;

        public AddAssetSteps(
            AssetFeatureManager assetFeatureManager,
            CurrencyFeatureManager currencyFatureManager)
        {
            _assetFeatureManager = assetFeatureManager;
            _currencyFatureManager = currencyFatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I have active asset (.*) with currency (.*)")]
        public void GivenAsset(string assetName, string currencyName)
        {
            GivenAssetCreateModel(assetName, currencyName, true);
            WhenIAddNewAsset();
        }

        [Given(@"I specified asset name (.*), currency (.*), is active (.*)")]
        public void GivenAssetCreateModel(string assetName, string currencyName, bool isActive)
        {
            Guid currencyId = _currencyFatureManager.Currencies[currencyName].Id;

            _assetCreateModel = new AssetCreateModel(assetName, currencyId, isActive);
        }

        [When(@"I add the asset")]
        public void WhenIAddNewAsset()
        {
            CreatedAsset = _assetFeatureManager.AddAsset(_assetCreateModel);
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
            Currency currency = _currencyFatureManager.Currencies[currencyName];

            CreatedAsset.CurrencyId.Should().Be(currency.Id);
            currency.Name.Should().Be(currencyName);
        }

        [Then(@"asset created with is active (.*)")]
        public void ThenAssetIsActive(bool isActive)
        {
            CreatedAsset.IsActive.Should().Be(isActive);
        }
    }
}
