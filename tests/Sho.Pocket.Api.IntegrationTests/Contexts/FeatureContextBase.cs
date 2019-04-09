using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Common.Configuration;
using Sho.Pocket.Core;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public abstract class FeatureContextBase
    {
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

            services.AddApplicationServices();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
