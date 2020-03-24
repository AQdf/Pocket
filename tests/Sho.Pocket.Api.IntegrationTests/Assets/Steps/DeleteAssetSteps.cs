using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Assets.Models;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class DeleteAssetSteps
    {
        private AssetViewModel _assetToDelete;

        private readonly AssetFeatureContext _context;

        private readonly UserContext _userContext;

        public DeleteAssetSteps(AssetFeatureContext assetFeatureContext, UserContext userContext)
        {
            _context = assetFeatureContext;
            _userContext = userContext;
        }

        [Given(@"I specified asset to delete (.*)")]
        public async Task GivenSpecifiedAssetToDelete(string assetName)
        {
            _assetToDelete = await _context.AssetService.GetAssetByNameAsync(_userContext.UserId, assetName);
        }

        [When(@"I delete asset")]
        public async Task WhenIDeleteAsset()
        {
            await _context.AssetService.DeleteAsync(_userContext.UserId, _assetToDelete.Id);
        }
        
        [Then(@"asset deleted")]
        public async Task ThenAssetDeleted()
        {
            List<AssetViewModel> assets = await _context.AssetService.GetAssetsAsync(_userContext.UserId, false);
            bool exists = assets.Any(a => a.Id == _assetToDelete.Id);

            exists.Should().Be(false);
        }

        [Then(@"asset not deleted")]
        public async Task ThenAssetNotDeleted()
        {
            List<AssetViewModel> assets = await _context.AssetService.GetAssetsAsync(_userContext.UserId, false);
            bool exists = assets.Any(a => a.Id == _assetToDelete.Id);

            exists.Should().Be(true);
        }
    }
}
