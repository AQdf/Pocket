using System.Threading.Tasks;
using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Assets.Models;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class UpdateAssetSteps
    {
        private readonly AssetFeatureContext _context;

        private readonly UserContext _userContext;

        private AssetUpdateModel _updateModel;

        private AssetViewModel _updatedAsset;

        public UpdateAssetSteps(AssetFeatureContext assetFeatureContext, UserContext userContext)
        {
            _context = assetFeatureContext;
            _userContext = userContext;
        }

        [Given(@"I set asset name to (.*), currency (.*), is active (.*)")]
        public void GivenSetAssetName(string assetName, string currencyName, bool isActive)
        {
            _updateModel = new AssetUpdateModel(assetName, currencyName, isActive, 1.0M);
        }

        [When(@"I update asset (.*)")]
        public async Task WhenIUpdateAsset(string assetName)
        {
            AssetViewModel assetToUpdate = await _context.AssetService.GetAssetByNameAsync(_userContext.UserId, assetName);
            _updatedAsset = await _context.AssetService.UpdateAsync(_userContext.UserId, assetToUpdate.Id, _updateModel);
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
