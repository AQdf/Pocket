using System;
using System.Linq;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Assets.Models;
using Sho.Pocket.Core.Features.Balances.Models;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class GetBalanceSteps
    {
        private readonly BalanceFeatureContext _balanceFeatureContext;

        private readonly AssetFeatureContext _assetFeatureContext;

        public GetBalanceSteps(BalanceFeatureContext balanceFeatureContext, AssetFeatureContext assetFeatureManager)
        {
            _balanceFeatureContext = balanceFeatureContext;
            _assetFeatureContext = assetFeatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [When(@"I get balance for today of (.*)")]
        public void WhenIGetBalanceOfAsset(string assetName)
        {
            DateTime today = DateTime.UtcNow.Date;
            AssetViewModel asset = _assetFeatureContext.Assets.Values.First(a => a.Name == assetName);

            BalanceViewModel balance = _balanceFeatureContext.Balances.Values
                .First(b => b.AssetId == asset.Id && b.EffectiveDate == today);
        }
    }
}
