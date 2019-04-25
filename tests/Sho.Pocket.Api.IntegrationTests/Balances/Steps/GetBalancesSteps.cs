using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Application.Balances.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class GetBalancesSteps
    {
        private List<BalanceViewModel> _balances;

        private readonly BalanceFeatureContext _balanceFeatureContext;

        public GetBalancesSteps(BalanceFeatureContext balanceFeatureContext)
        {
            _balanceFeatureContext = balanceFeatureContext;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [When(@"I get today balances")]
        public async Task WhenIGetTodayBalances()
        {
            DateTime today = DateTime.UtcNow.Date;

            _balances = await _balanceFeatureContext.GetAllBalances(today);
        }
        
        [Then(@"my (.*) balances returned")]
        public void ThenMyBalancesReturned(int itemsNumber)
        {
            _balances.Count.Should().Be(itemsNumber);
        }
    }
}
