using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Application.Assets.Models;
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

        private BalanceFeatureContext _balanceFeatureContext;

        private AssetFeatureContext _assetFeatureContext;

        public DeleteBalanceSteps(
            BalanceFeatureContext balanceFeatureContext,
            AssetFeatureContext assetFeatureManager)
        {
            _assetFeatureContext = assetFeatureManager;
            _balanceFeatureContext = balanceFeatureContext;
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
            AssetViewModel asset = _assetFeatureContext.Assets.Values.First(a => a.Name == assetName);

            Balance balance = _balanceFeatureContext.Balances.Values
                .First(b => b.AssetId == asset.Id && b.EffectiveDate == today);

            _balanceToDeleteId = balance.Id;
        }
        
        [When(@"I delete balance")]
        public void WhenIDeleteBalance()
        {
            _balanceFeatureContext.DeleteBalance(_balanceToDeleteId);
        }
        
        [Then(@"balance deleted")]
        public void ThenBalanceDeleted()
        {
            bool exists = _balanceFeatureContext.Balances.ContainsKey(_balanceToDeleteId);

            exists.Should().Be(false);
        }
    }
}
