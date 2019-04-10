using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class AddEffectiveBalancesSteps
    {
        private List<BalanceViewModel> _balances;

        private BalanceFeatureContext _balanceFeatureContext;

        public AddEffectiveBalancesSteps(BalanceFeatureContext balanceFeatureContext)
        {
            _balanceFeatureContext = balanceFeatureContext;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [When(@"I add effective balances")]
        public void WhenIAddEffectiveBalances()
        {
            _balances = _balanceFeatureContext.AddEffectiveBalances();
        }

        [Then(@"balances for today exists")]
        public void ThenBalanceForTodayExists()
        {
            DateTime today = DateTime.UtcNow.Date;
            bool exists = _balances.Any(b => b.EffectiveDate == today);

            exists.Should().Be(true);
        }
    }
}
