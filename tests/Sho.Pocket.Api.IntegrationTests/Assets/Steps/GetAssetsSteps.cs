using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Assets.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class GetAssetsSteps
    {
        private List<AssetViewModel> _assets;

        private readonly AssetFeatureContext _assetFeatureContext;

        public GetAssetsSteps(AssetFeatureContext assetFeatureContext)
        {
            _assetFeatureContext = assetFeatureContext;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [When(@"I get assets")]
        public async Task WhenIGetAssets()
        {
            _assets = await _assetFeatureContext.GetAssets();
        }
        
        [Then(@"my (.*) assets returned")]
        public void ThenMyAssetsReturned(int assetsCount)
        {
            _assets.Count.Should().Be(assetsCount);
        }
    }
}
