using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Application.Assets.Models;
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

        private BalanceFeatureContext _balanceFeatureContext;

        private AssetFeatureContext _assetFeatureContext;

        private ExchangeRateFeatureContext _exchangeRateFeatureContext;

        public AddBalanceSteps(
            BalanceFeatureContext balanceFeatureContext,
            AssetFeatureContext assetFeatureContext,
            ExchangeRateFeatureContext exchangeRateFeatureContext)
        {
            _balanceFeatureContext = balanceFeatureContext;
            _assetFeatureContext = assetFeatureContext;
            _exchangeRateFeatureContext = exchangeRateFeatureContext;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [Given(@"I have balance of asset (.*), amount (.*), day shift (.*)")]
        public void GivenIHaveForTodayBalanceOfAssetActiveAssetAmount(string assetName, decimal amount, int dayShift)
        {
            GivenBalanceCreateModel(assetName, amount, dayShift);
            WhenIAddNewBalance();
        }

        [Given(@"I specified balance of asset (.*), amount (.*), day shift (.*)")]
        public void GivenBalanceCreateModel(string assetName, decimal amount, int dayShift)
        {
            DateTime effectiveDate = DateTime.UtcNow.Date.AddDays(dayShift);
            AssetViewModel asset = _assetFeatureContext.Assets[assetName];
            ExchangeRate exchangeRate = _exchangeRateFeatureContext.ExchangeRates.Values.First(r => r.EffectiveDate == effectiveDate);

            _balanceCreateModel = new BalanceCreateModel(asset.Id, effectiveDate, amount, exchangeRate.Id);
        }

        [When(@"I add new balance")]
        public void WhenIAddNewBalance()
        {
            _createdBalance = _balanceFeatureContext.AddBalance(_balanceCreateModel);
        }

        [Then(@"balance exists")]
        public void ThenBalanceExists()
        {
            _createdBalance.Should().NotBeNull();
        }

        [Then(@"balance asset is (.*)")]
        public void ThenBalanceAssetIs(string assetName)
        {
            AssetViewModel asset = _assetFeatureContext.Assets.Values.First(a => a.Name == assetName);

            _createdBalance.AssetId.Should().Be(asset.Id);
        }

        [Then(@"balance amount is (.*)")]
        public void ThenBalanceAmountIs(decimal amount)
        {
            _createdBalance.Value.Should().Be(amount);
        }

        [Then(@"balance of (.*) effective date is today")]
        public void ThenBalanceEffectiveDateIsToday(string assetName)
        {
            DateTime today = DateTime.UtcNow.Date;
            _createdBalance.EffectiveDate.Should().Be(today);
        }
    }
}
