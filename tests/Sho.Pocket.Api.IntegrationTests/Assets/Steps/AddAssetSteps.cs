using System.Threading.Tasks;
using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Assets.Models;
using TechTalk.SpecFlow;
using Xunit;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class AddAssetSteps
    {
        private readonly AssetFeatureContext _context;

        private readonly UserContext _userContext;

        private AssetCreateModel _assetCreateModel;

        private AssetViewModel CreatedAsset;

        public AddAssetSteps(AssetFeatureContext assetFeatureContext, UserContext userContext)
        {
            _context = assetFeatureContext;
            _userContext = userContext;
        }

        [Given(@"I have active asset (.*) with currency (.*)")]
        public async Task GivenAsset(string assetName, string currencyName)
        {
            GivenAssetCreateModel(assetName, currencyName, true);
            await WhenIAddNewAsset();
        }

        [Given(@"I specified asset name (.*), currency (.*), is active (.*)")]
        public void GivenAssetCreateModel(string assetName, string currencyName, bool isActive)
        {
            _assetCreateModel = new AssetCreateModel(assetName, currencyName, isActive, 1.0M);
        }

        [When(@"I add the asset")]
        public async Task WhenIAddNewAsset()
        {
            CreatedAsset = await _context.AssetService.AddAssetAsync(_userContext.UserId, _assetCreateModel);
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
            CreatedAsset.Currency.Should().Be(currencyName);
        }

        [Then(@"asset created with is active (.*)")]
        public void ThenAssetIsActive(bool isActive)
        {
            CreatedAsset.IsActive.Should().Be(isActive);
        }
    }
}
