﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.DataAccess.Sql.EntityFramework;
using Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories;

namespace Sho.Pocket.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityFrameworkDataAccess(this IServiceCollection services, DbSettings dbSettings)
        {
            services.AddDbContext<PocketDbContext>(options => options.UseSqlServer(dbSettings.DbConnectionString));

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
