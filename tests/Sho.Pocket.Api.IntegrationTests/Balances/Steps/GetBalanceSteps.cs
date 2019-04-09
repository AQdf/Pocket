using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class GetBalanceSteps
    {
        private BalanceViewModel _balanceViewModel;

        private readonly BalanceFeatureContext _balanceFeatureContext;

        private readonly AssetFeatureContext _assetFeatureContext;

        public GetBalanceSteps(
            BalanceFeatureContext balanceFeatureContext,
            AssetFeatureContext assetFeatureManager,
            AddBalanceSteps addBalanceSteps)
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
            Asset asset = _assetFeatureContext.Assets.Values.First(a => a.Name == assetName);

            Balance balance = _balanceFeatureContext.Balances.Values
                .First(b => b.AssetId == asset.Id && b.EffectiveDate == today);

            _balanceViewModel = _balanceFeatureContext.GetBalance(balance.Id);
        }
    }
}
