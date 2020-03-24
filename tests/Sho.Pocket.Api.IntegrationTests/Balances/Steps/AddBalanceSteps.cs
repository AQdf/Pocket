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
    public class AddBalanceSteps
    {
        private readonly BalanceFeatureContext _balanceFeatureContext;

        private readonly AssetFeatureContext _assetFeatureContext;

        private readonly UserContext _userContext;

        private BalanceCreateModel _balanceCreateModel;

        private BalanceViewModel _createdBalance;

        public AddBalanceSteps(
            BalanceFeatureContext balanceFeatureContext,
            AssetFeatureContext assetFeatureContext,
            UserContext userContext)
        {
            _balanceFeatureContext = balanceFeatureContext;
            _assetFeatureContext = assetFeatureContext;
            _userContext = userContext;
        }

        [Given(@"I have balance of asset (.*), amount (.*), day shift (.*)")]
        public async Task GivenIHaveForTodayBalanceOfAssetActiveAssetAmount(string assetName, decimal amount, int dayShift)
        {
            await GivenBalanceCreateModel(assetName, amount, dayShift);
            await WhenIAddNewBalance();
        }

        [Given(@"I specified balance of asset (.*), amount (.*), day shift (.*)")]
        public async Task GivenBalanceCreateModel(string assetName, decimal amount, int dayShift)
        {
            DateTime effectiveDate = DateTime.UtcNow.Date.AddDays(dayShift);

            AssetViewModel asset = await _assetFeatureContext.AssetService
                .GetAssetByNameAsync(_userContext.UserId, assetName);

            _balanceCreateModel = new BalanceCreateModel(asset.Id, effectiveDate, asset.Currency, amount);
        }

        [When(@"I add new balance")]
        public async Task WhenIAddNewBalance()
        {
            _createdBalance = await _balanceFeatureContext.BalanceService
                .AddBalanceAsync(_userContext.UserId, _balanceCreateModel);
        }

        [Then(@"balance exists")]
        public void ThenBalanceExists()
        {
            _createdBalance.Should().NotBeNull();
        }

        [Then(@"balance asset is (.*)")]
        public async Task ThenBalanceAssetIs(string assetName)
        {
            AssetViewModel asset = await _assetFeatureContext.AssetService.GetAssetByNameAsync(_userContext.UserId, assetName);

            _createdBalance.AssetId.Should().Be(asset.Id);
        }

        [Then(@"balance amount is (.*)")]
        public void ThenBalanceAmountIs(decimal amount)
        {
            _createdBalance.Value.Should().Be(amount);
        }

        [Then(@"balance of (.*) effective date is today")]
        public async Task ThenBalanceEffectiveDateIsToday(string assetName)
        {
            DateTime today = DateTime.UtcNow.Date;
            AssetViewModel asset = await _assetFeatureContext.AssetService.GetAssetByNameAsync(_userContext.UserId, assetName);

            _createdBalance.AssetId.Should().Be(asset.Id);
            _createdBalance.EffectiveDate.Should().Be(today);
        }
    }
}
