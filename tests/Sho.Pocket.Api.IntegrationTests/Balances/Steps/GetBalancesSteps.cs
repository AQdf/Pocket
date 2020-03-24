using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Balances.Models;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class GetBalancesSteps
    {
        private List<BalanceViewModel> _balances;

        private readonly BalanceFeatureContext _context;

        private readonly UserContext _userContext;

        public GetBalancesSteps(BalanceFeatureContext balanceFeatureContext, UserContext userContext)
        {
            _context = balanceFeatureContext;
            _userContext = userContext;
        }

        [When(@"I get today balances")]
        public async Task WhenIGetTodayBalances()
        {
            DateTime today = DateTime.UtcNow.Date;

            BalancesViewModel balances = await _context.BalanceService.GetUserEffectiveBalancesAsync(_userContext.UserId, today);

            _balances = balances.Items.ToList();
        }
        
        [Then(@"my (.*) balances returned")]
        public void ThenMyBalancesReturned(int itemsNumber)
        {
            _balances.Count.Should().Be(itemsNumber);
        }
    }
}
