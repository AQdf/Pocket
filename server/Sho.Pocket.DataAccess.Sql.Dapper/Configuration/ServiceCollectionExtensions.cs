using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.DataAccess.Sql.Dapper.Repositories;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDapperDataAccess(this IServiceCollection services)
        {
            services.AddScoped<IDbInitializer, DapperDbInitializer>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<IBalanceNoteRepository, BalanceNoteRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();
        }
    }
}
