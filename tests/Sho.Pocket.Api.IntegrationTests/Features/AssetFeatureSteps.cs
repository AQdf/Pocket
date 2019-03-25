using Sho.Pocket.Application.Assets.Models;
using System;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Features
{
    [Binding]
    public class AssetFeatureSteps
    {
        private readonly AssetDriver _assetDriver;

        public AssetFeatureSteps(AssetDriver assetDriver)
        {
            _assetDriver = assetDriver;
        }

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            AssetCreateModel model = new AssetCreateModel
            {
                Name = "Bank account",
                CurrencyId = Guid.NewGuid(),
                IsActive = true
            };

            _assetDriver.InsertAssetToStorage(model);
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            _assetDriver.CheckResult(true);
        }
    }
}
