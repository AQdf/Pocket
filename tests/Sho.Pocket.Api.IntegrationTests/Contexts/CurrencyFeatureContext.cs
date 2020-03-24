using System;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class CurrencyFeatureContext : PocketFeatureBaseContext
    {
        public readonly ICurrencyRepository CurrencyRepository;

        public CurrencyFeatureContext() : base()
        {
            CurrencyRepository = Services.GetRequiredService<ICurrencyRepository>();
        }
    }
}
