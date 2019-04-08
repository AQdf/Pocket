using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Balances.Managers;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Balances.Models;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class GetBalancesSteps
    {
        private List<BalanceViewModel> _balances;

        private readonly BalanceFeatureManager _balanceFeatureManager;

        public GetBalancesSteps(BalanceFeatureManager balanceFeatureManager)
        {
            _balanceFeatureManager = balanceFeatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [When(@"I get today balances")]
        public void WhenIGetTodayBalances()
        {
            DateTime today = DateTime.UtcNow.Date;

            _balances = _balanceFeatureManager.GetAllBalances(today);
        }
        
        [Then(@"my (.*) balances returned")]
        public void ThenMyBalancesReturned(int itemsNumber)
        {
            _balances.Count.Should().Be(itemsNumber);
        }
    }
}
