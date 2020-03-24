using System;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.Features.Balances.Abstractions;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class BalanceFeatureContext : PocketFeatureBaseContext
    {
        public readonly IBalanceService BalanceService;

        public BalanceFeatureContext() : base()
        {
            BalanceService = Services.GetRequiredService<IBalanceService>();
        }
    }
}
