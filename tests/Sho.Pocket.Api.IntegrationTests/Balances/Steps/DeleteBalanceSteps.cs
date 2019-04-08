using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Assets.Managers;
using Sho.Pocket.Api.IntegrationTests.Balances.Managers;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Domain.Entities;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class DeleteBalanceSteps
    {
        private Guid _balanceToDeleteId;

        private BalanceFeatureManager _balanceFeatureManager;

        private AssetFeatureManager _assetFeatureManager;

        public DeleteBalanceSteps(
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

        [Given(@"I specified balance to delete of asset (.*)")]
        public void GivenISpecifiedBalanceToDelete(string assetName)
        {
            DateTime today = DateTime.UtcNow.Date;
            Asset asset = _assetFeatureManager.Assets.Values.First(a => a.Name == assetName);

            Balance balance = _balanceFeatureManager.Balances.Values
                .First(b => b.AssetId == asset.Id && b.EffectiveDate == today);

            _balanceToDeleteId = balance.Id;
        }
        
        [When(@"I delete balance")]
        public void WhenIDeleteBalance()
        {
            _balanceFeatureManager.DeleteBalance(_balanceToDeleteId);
        }
        
        [Then(@"balance deleted")]
        public void ThenBalanceDeleted()
        {
            bool exists = _balanceFeatureManager.Balances.ContainsKey(_balanceToDeleteId);

            exists.Should().Be(false);
        }
    }
}
