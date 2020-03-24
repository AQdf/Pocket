using System;
using System.Threading.Tasks;
using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using Sho.Pocket.Core.Features.Balances.Models;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Balances.Steps
{
    [Binding]
    public class AddEffectiveBalancesSteps
    {
        private readonly BalanceFeatureContext _context;

        private readonly UserContext _userContext;

        public AddEffectiveBalancesSteps(BalanceFeatureContext balanceFeatureContext, UserContext userContext)
        {
            _context = balanceFeatureContext;
            _userContext = userContext;
        }

        [When(@"I add effective balances")]
        public async Task WhenIAddEffectiveBalances()
        {
            await _context.BalanceService.AddEffectiveBalancesTemplate(_userContext.UserId);
        }

        [Then(@"balances for today exists")]
        public async Task ThenBalanceForTodayExists()
        {
            DateTime today = DateTime.UtcNow.Date;
            BalancesViewModel balances = await _context.BalanceService.GetUserEffectiveBalancesAsync(_userContext.UserId, today);
            bool exists = balances.Count > 0;

            exists.Should().Be(true);
        }
    }
}
