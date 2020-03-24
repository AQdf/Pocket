using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Assets.Models;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class GetAssetsSteps
    {
        private readonly AssetFeatureContext _context;

        private readonly UserContext _userContext;

        private List<AssetViewModel> _assets;

        public GetAssetsSteps(AssetFeatureContext assetFeatureContext, UserContext userContext)
        {
            _context = assetFeatureContext;
            _userContext = userContext;
        }

        [When(@"I get assets")]
        public async Task WhenIGetAssets()
        {
            _assets = await _context.AssetService.GetAssetsAsync(_userContext.UserId, true);
        }
        
        [Then(@"my (.*) assets returned")]
        public void ThenMyAssetsReturned(int assetsCount)
        {
            _assets.Count.Should().Be(assetsCount);
        }
    }
}
