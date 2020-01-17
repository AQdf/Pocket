using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Assets.Models;
using Sho.Pocket.Core.Features.Balances.Models;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class DeleteBalanceSteps
    {
        private Guid _balanceToDeleteId;

        private readonly BalanceFeatureContext _balanceFeatureContext;

        private readonly AssetFeatureContext _assetFeatureContext;

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

            BalanceViewModel balance = _balanceFeatureContext.Balances.Values
                .First(b => b.AssetId == asset.Id && b.EffectiveDate == today);

            _balanceToDeleteId = balance.Id.Value;
        }
        
        [When(@"I delete balance")]
        public async Task WhenIDeleteBalance()
        {
            await _balanceFeatureContext.DeleteBalance(_balanceToDeleteId);
        }
        
        [Then(@"balance deleted")]
        public void ThenBalanceDeleted()
        {
            bool exists = _balanceFeatureContext.Balances.ContainsKey(_balanceToDeleteId);

            exists.Should().Be(false);
        }
    }
}
