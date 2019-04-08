using Sho.Pocket.Api.IntegrationTests.Assets.Managers;
using Sho.Pocket.Api.IntegrationTests.Balances.Managers;
using Sho.Pocket.Api.IntegrationTests.Common;
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

        private readonly BalanceFeatureManager _balanceFeatureManager;

        private readonly AssetFeatureManager _assetFeatureManager;

        public GetBalanceSteps(
            BalanceFeatureManager balanceFeatureManager,
            AssetFeatureManager assetFeatureManager,
            AddBalanceSteps addBalanceSteps)
        {
            _balanceFeatureManager = balanceFeatureManager;
            _assetFeatureManager = assetFeatureManager;
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
            Asset asset = _assetFeatureManager.Assets.Values.First(a => a.Name == assetName);

            Balance balance = _balanceFeatureManager.Balances.Values
                .First(b => b.AssetId == asset.Id && b.EffectiveDate == today);

            _balanceViewModel = _balanceFeatureManager.GetBalance(balance.Id);
        }
    }
}
