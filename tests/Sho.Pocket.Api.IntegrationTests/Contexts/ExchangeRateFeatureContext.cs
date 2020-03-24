using System;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class ExchangeRateFeatureContext : PocketFeatureBaseContext
    {
        public readonly IExchangeRateRepository ExchangeRateRepository;

        public ExchangeRateFeatureContext() : base()
        {
            ExchangeRateRepository = Services.GetRequiredService<IExchangeRateRepository>();
        }
    }
}
