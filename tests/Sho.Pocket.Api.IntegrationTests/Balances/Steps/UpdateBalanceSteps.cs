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
    public class UpdateBalanceSteps
    {
        private BalanceUpdateModel _balanceUpdateModel;

        private BalanceViewModel _updatedBalance;

        private readonly BalanceFeatureContext _balanceFeatureContext;

        private readonly AssetFeatureContext _assetFeatureContext;

        private readonly UserContext _userContext;

        public UpdateBalanceSteps(
            BalanceFeatureContext balanceFeatureContext,
            AssetFeatureContext assetFeatureManager,
            UserContext userContext)
        {
            _assetFeatureContext = assetFeatureManager;
            _balanceFeatureContext = balanceFeatureContext;
            _userContext = userContext;
        }

        [Given(@"I set balance of asset (.*) value to (.*)")]
        public async Task GivenISetBalanceValueTo(string assetName, decimal value)
        {
            AssetViewModel asset = await _assetFeatureContext.AssetService.GetAssetByNameAsync(_userContext.UserId, assetName);
            _balanceUpdateModel = new BalanceUpdateModel(asset.Id, value);
        }
        
        [When(@"I update balance for today of (.*)")]
        public async Task WhenIUpdateBalanceOfAsset(string assetName)
        {
            DateTime today = DateTime.UtcNow.Date;
            AssetViewModel asset = await _assetFeatureContext.AssetService.GetAssetByNameAsync(_userContext.UserId, assetName);

            BalancesViewModel balances = await _balanceFeatureContext.BalanceService
                .GetUserEffectiveBalancesAsync(_userContext.UserId, today);

            BalanceViewModel balance = balances.Items.Single(b => b.Asset.Name.Equals(assetName, StringComparison.OrdinalIgnoreCase));

            _updatedBalance = await _balanceFeatureContext.BalanceService
                .UpdateBalanceAsync(_userContext.UserId, balance.Id.Value, _balanceUpdateModel);
        }
        
        [Then(@"balance amount updated to (.*)")]
        public void ThenBalanceAmountUpdatedTo(decimal amount)
        {
            _updatedBalance.Value.Should().Be(amount);
        }
    }
}
