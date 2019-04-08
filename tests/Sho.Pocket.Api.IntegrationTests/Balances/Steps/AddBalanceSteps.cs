using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Assets.Managers;
using Sho.Pocket.Api.IntegrationTests.Balances.Managers;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.ExchangeRates.Managers;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class AddBalanceSteps
    {
        private BalanceCreateModel _balanceCreateModel;

        private Balance _createdBalance;

        private BalanceFeatureManager _balanceFeatureManager;

        private AssetFeatureManager _assetFeatureManager;

        private ExchangeRateFeatureManager _exchangeRateFeatureManager;

        public AddBalanceSteps(
            BalanceFeatureManager balanceFeatureManager,
            AssetFeatureManager assetFeatureManager,
            ExchangeRateFeatureManager exchangeRateFeatureManager)
        {
            _balanceFeatureManager = balanceFeatureManager;
            _assetFeatureManager = assetFeatureManager;
            _exchangeRateFeatureManager = exchangeRateFeatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I have balance of asset (.*), amount (.*) for today")]
        public void GivenIHaveForTodayBalanceOfAssetActiveAssetAmount(string assetName, decimal amount)
        {
            GivenBalanceCreateModel(assetName, amount);
            WhenIAddNewBalance();
        }

        [Given(@"I specified today balance of asset (.*), amount (.*)")]
        public void GivenBalanceCreateModel(string assetName, decimal amount)
        {
            Asset asset = _assetFeatureManager.Assets.Values.First(a => a.Name == assetName);
            ExchangeRate exchangeRate = _exchangeRateFeatureManager.ExchangeRates.Values.First();

            _balanceCreateModel = new BalanceCreateModel(asset.Id, exchangeRate.EffectiveDate, amount, exchangeRate.Id);
        }

        [When(@"I add new balance")]
        public void WhenIAddNewBalance()
        {
            _createdBalance = _balanceFeatureManager.AddBalance(_balanceCreateModel);
        }

        [Then(@"balance exists")]
        public void ThenBalanceExists()
        {
            _createdBalance.Should().NotBeNull();
        }

        [Then(@"balance asset is (.*)")]
        public void ThenBalanceAssetIs(string assetName)
        {
            Asset asset = _assetFeatureManager.Assets.Values.First(a => a.Name == assetName);

            _createdBalance.AssetId.Should().Be(asset.Id);
        }

        [Then(@"balance amount is (.*)")]
        public void ThenBalanceAmountIs(decimal amount)
        {
            _createdBalance.Value.Should().Be(amount);
        }

        [Then(@"balance effective date is today")]
        public void ThenBalanceEffectiveDateIsToday()
        {
            DateTime today = DateTime.UtcNow.Date;
            _createdBalance.EffectiveDate.Should().Be(today);
        }
    }
}
