using System;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Common.Configuration;
using Sho.Pocket.Core;

namespace Sho.Pocket.Api.IntegrationTests
{
    public abstract class TestDriverBase
    {
        protected ServiceProvider _serviceProvider;

        public TestDriverBase()
        {
            Configure();
        }

        private void Configure()
        {
            IServiceCollection services = new ServiceCollection();

            GlobalSettings globalSettings = new GlobalSettings
            {
                DbConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=PocketDb-Test;Integrated Security=true;"
            };
            services.AddSingleton(s => globalSettings);

            services.AddApplicationServices();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
