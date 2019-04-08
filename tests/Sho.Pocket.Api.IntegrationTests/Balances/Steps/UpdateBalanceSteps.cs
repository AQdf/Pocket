using FluentAssertions;
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
    public class UpdateBalanceSteps
    {
        private BalanceUpdateModel _balanceUpdateModel;

        private Balance _updatedBalance;

        private BalanceFeatureManager _balanceFeatureManager;

        private AssetFeatureManager _assetFeatureManager;

        public UpdateBalanceSteps(
            BalanceFeatureManager balanceFeatureManager,
            AssetFeatureManager assetFeatureManager)
        {
            _assetFeatureManager = assetFeatureManager;
            _balanceFeatureManager = balanceFeatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I set balance value to (.*)")]
        public void GivenISetBalanceValueTo(decimal value)
        {
            _balanceUpdateModel = new BalanceUpdateModel(value);
        }
        
        [When(@"I update balance for today of (.*)")]
        public void WhenIUpdateBalanceOfAsset(string assetName)
        {
            DateTime today = DateTime.UtcNow.Date;
            Asset asset = _assetFeatureManager.Assets.Values.First(a => a.Name == assetName);

            Balance balance = _balanceFeatureManager.Balances.Values
                .First(b => b.AssetId == asset.Id && b.EffectiveDate == today);

            _updatedBalance = _balanceFeatureManager.UpdateBalance(balance.Id, _balanceUpdateModel);
        }
        
        [Then(@"balance amount updated to (.*)")]
        public void ThenBalanceAmountUpdatedTo(decimal amount)
        {
            _updatedBalance.Value.Should().Be(amount);
        }
    }
}
