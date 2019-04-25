using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Balances
{
    public class BalanceRepository : BaseRepository<Balance>, IBalanceRepository
    {
        private const string SCRIPTS_DIR_NAME = "Balances.Scripts";

        public BalanceRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<IEnumerable<Balance>> GetAll(bool includeRelated = true)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetAllBalances.sql");

            IEnumerable<Balance> result = includeRelated
                ? await GetAllWithRelatedEntities(queryText)
                : await base.GetAll(queryText);

            return result;
        }

        public async Task<IEnumerable<Balance>> GetByEffectiveDate(DateTime effectiveDate, bool includeRelated = true)
        {
            string queryText = @"
                select * from Balance
                join Asset on Asset.Id = Balance.AssetId
                left join ExchangeRate on ExchangeRate.Id = Balance.ExchangeRateId
                left join Currency BaseCurrency on BaseCurrency.Id = ExchangeRate.BaseCurrencyId
                left join Currency CounterCurrency on CounterCurrency.Id = ExchangeRate.CounterCurrencyId
                where Balance.EffectiveDate = @effectiveDate";

            object queryParams = new { effectiveDate };

            IEnumerable<Balance> result = includeRelated
                ? await GetAllWithRelatedEntities(queryText, queryParams)
                : await base.GetAll(queryText, queryParams);

            return result;
        }

        public async Task<Balance> GetById(Guid id)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetBalance.sql");

            object queryParameters = new { id };

            IEnumerable<Balance> resultItems;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                resultItems = await db.QueryAsync<Balance, Asset, ExchangeRate, Balance>(queryText,
                    (balance, asset, rate) =>
                    {
                        balance.Asset = asset;
                        balance.ExchangeRate = rate;

                        return balance;
                    }, queryParameters);
            }

            return resultItems.FirstOrDefault();
        }

        public async Task<Balance> Add(Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "InsertBalance.sql");

            object queryParameters = new { assetId, effectiveDate, value, exchangeRateId };

            Balance result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<IEnumerable<Balance>> AddEffectiveBalances(DateTime currentEffectiveDate)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "InsertBalancesTemplate.sql");

            object queryParameters = new
            {
                effectiveDate = currentEffectiveDate,
            };

            IEnumerable<Balance> result = await base.GetAll(queryText, queryParameters);

            return result;
        }

        public async Task<Balance> Update(Guid id, decimal value)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "UpdateBalance.sql");

            object queryParameters = new { id, value };

            Balance result = await base.UpdateEntity(queryText, queryParameters);

            return result;
        }

        public async Task Remove(Guid balanceId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "DeleteBalance.sql");

            object queryParameters = new
            {
                id = balanceId
            };

            await base.RemoveEntity(queryText, queryParameters);
        }

        public async Task<IEnumerable<DateTime>> GetOrderedEffectiveDates()
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetBalancesEffectiveDates.sql");

            IEnumerable<DateTime> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.QueryAsync<DateTime>(queryText);
            }

            return result;
        }

        public async Task ApplyExchangeRate(Guid exchangeRateId, Guid counterCurrencyId, DateTime effectiveDate)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "ApplyExchangeRate.sql");

            object queryParameters = new {
                exchangeRateId,
                currencyId = counterCurrencyId,
                effectiveDate
            };

            await base.ExecuteScript(queryText, queryParameters);
        }

        private async Task<IEnumerable<Balance>> GetAllWithRelatedEntities(string queryText, object queryParams = null)
        {
            IEnumerable<Balance> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.QueryAsync<Balance, Asset, ExchangeRate, Currency, Currency, Balance>(queryText,
                    (balance, asset, rate, baseCurrency, counterCurrency) =>
                    {
                        balance.Asset = asset;
                        balance.ExchangeRate = rate;

                        if (balance.ExchangeRate != null)
                        {
                            balance.ExchangeRate.BaseCurrency = baseCurrency;
                            balance.ExchangeRate.CounterCurrency = counterCurrency;
                        }

                        return balance;
                    }, queryParams);
            }

            return result;
        }
    }
}
