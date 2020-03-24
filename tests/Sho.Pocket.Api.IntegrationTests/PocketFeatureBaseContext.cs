using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.Configuration;
using Sho.Pocket.BankIntegration;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Core.Features.BankIntegration;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.DataAccess.Sql.EntityFramework;
using Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories;
using Sho.Pocket.ExchangeRates;
using Sho.Pocket.ExchangeRates.Configuration.Models;
using Sho.Pocket.ExchangeRates.Providers;

namespace Sho.Pocket.Api.IntegrationTests
{
    public abstract class PocketFeatureBaseContext
    {
        public ServiceProvider Services;

        public PocketFeatureBaseContext()
        {
            Configure();
        }

        public void Configure()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOptions();
            services.AddMemoryCache();

            List<ExchangeRateProviderOption> exchangeRateProviders = new List<ExchangeRateProviderOption>
            {
                new ExchangeRateProviderOption{ Name = "Default" }
            };

            services.Configure<ExchangeRateSettings>(o => { o.Providers = exchangeRateProviders; });

            services.Configure<DbSettings>(o =>
            {
                o.SystemDefaultCurrency = "USD";
            });

            ConfigureTestInMemoryDb(services);
            services.AddApplicationServices();
            ConfigureExchangeRates(services);
            ConfigureBankIntegration(services);

            Services = services.BuildServiceProvider();

            IDbInitializer dbInitializer = Services.GetRequiredService<IDbInitializer>();
            dbInitializer.EnsureCreated();
        }

        private void ConfigureBankIntegration(IServiceCollection services)
        {
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IBankIntegrationServiceResolver, BankIntegrationServiceResolver>();
        }

        private void ConfigureExchangeRates(IServiceCollection services)
        {
            services.AddScoped<IExchangeRateExternalService, ExchangeRateExternalService>();
            services.AddScoped<IExchangeRateProviderResolver, ExchangeRateProviderResolver>();
            services.AddScoped<IExchangeRateProvider, DefaultExchangeRateProvider>();
        }

        private void ConfigureTestInMemoryDb(IServiceCollection services)
        {
            services.AddDbContext<PocketDbContext>(options => options.UseInMemoryDatabase(databaseName: "PocketDb-Test"));

            services.AddScoped<IDbInitializer, EntityFrameworkDbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<IBalanceNoteRepository, BalanceNoteRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();
        }
    }
}
