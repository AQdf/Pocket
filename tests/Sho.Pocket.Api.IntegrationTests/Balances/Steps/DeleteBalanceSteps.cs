using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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

        private readonly DateTime _effectiveDate = DateTime.UtcNow.Date;

        private readonly BalanceFeatureContext _balanceFeatureContext;

        private readonly AssetFeatureContext _assetFeatureContext;

        private readonly UserContext _userContext;

        public DeleteBalanceSteps(
            BalanceFeatureContext balanceFeatureContext,
            AssetFeatureContext assetFeatureManager,
            UserContext userContext)
        {
            _assetFeatureContext = assetFeatureManager;
            _balanceFeatureContext = balanceFeatureContext;
            _userContext = userContext;
        }

        [Given(@"I specified balance to delete of asset (.*)")]
        public async Task GivenISpecifiedBalanceToDelete(string assetName)
        {
            AssetViewModel asset = await _assetFeatureContext.AssetService
                .GetAssetByNameAsync(_userContext.UserId, assetName);

            BalancesViewModel balances = await _balanceFeatureContext.BalanceService
                .GetUserEffectiveBalancesAsync(_userContext.UserId, _effectiveDate);

            BalanceViewModel balance = balances.Items.Single(b => b.Asset.Name == asset.Name && b.EffectiveDate == _effectiveDate);

            _balanceToDeleteId = balance.Id.Value;
        }
        
        [When(@"I delete balance")]
        public async Task WhenIDeleteBalance()
        {
            await _balanceFeatureContext.BalanceService.DeleteBalanceAsync(_userContext.UserId, _balanceToDeleteId);
        }
        
        [Then(@"balance deleted")]
        public async Task ThenBalanceDeleted()
        {
            BalancesViewModel balances = await _balanceFeatureContext.BalanceService
                .GetUserEffectiveBalancesAsync(_userContext.UserId, _effectiveDate);

            bool exists = balances.Items.Any(b => b.Id == _balanceToDeleteId);

            exists.Should().Be(false);
        }
    }
}
