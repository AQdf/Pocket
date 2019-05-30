using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Common.Configuration;
using Sho.Pocket.Application.ExchangeRates.Providers;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Core;
using Sho.Pocket.Core.ExchangeRates;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public abstract class FeatureContextBase
    {
        // TODO: Remove this when UserContext will be implemented
        public readonly UserViewModel User = new UserViewModel(Guid.Parse("D6D726CD-DA50-490D-8605-A8E9125AC4B2"), "test.user@mail.com");

        protected ServiceProvider _serviceProvider;

        public FeatureContextBase()
        {
            Configure();
        }

        private void Configure()
        {
            IServiceCollection services = new ServiceCollection();

            GlobalSettings globalSettings = new GlobalSettings
            {
                DbConnectionString = ConfigurationConstants.DB_CONNECTION
            };

            services.AddSingleton(s => globalSettings);

            services.AddOptions();

            List<ExchangeRateProviderOption> exchangeRateProviders = new List<ExchangeRateProviderOption>
            {
                new ExchangeRateProviderOption{ Name = ProviderConstants.DEFAULT_PROVIDER }
            };

            services.Configure<ExchangeRateSettings>(o => { o.Providers = exchangeRateProviders; });

            services.AddApplicationServices();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
