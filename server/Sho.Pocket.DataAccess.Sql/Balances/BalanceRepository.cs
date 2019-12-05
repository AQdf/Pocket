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

        public async Task<IEnumerable<Balance>> GetAllAsync(Guid userOpenId, bool includeRelated = true)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetAllBalances.sql");
            object queryParams = new { userOpenId };

            IEnumerable<Balance> result = includeRelated
                ? await GetAllWithRelatedEntities(queryText, queryParams)
                : await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<Balance>> GetByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate, bool includeRelated = true)
        {
            string queryText = @"
                select * from Balance
                join Asset on Asset.Id = Balance.AssetId
                left join ExchangeRate on ExchangeRate.Id = Balance.ExchangeRateId
                where Balance.EffectiveDate = @effectiveDate and Balance.UserOpenId = @userOpenId";

            object queryParams = new { userOpenId, effectiveDate };

            IEnumerable<Balance> result = includeRelated
                ? await GetAllWithRelatedEntities(queryText, queryParams)
                : await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<Balance>> GetLatestBalancesAsync(Guid userOpenId, bool includeRelated = true)
        {
            string queryText = @"
                declare @latestDate datetime2(7) = (select top 1 EffectiveDate from Balance order by EffectiveDate desc)

                select * from Balance
                join Asset on Asset.Id = Balance.AssetId
                left join ExchangeRate on ExchangeRate.Id = Balance.ExchangeRateId
                where Balance.EffectiveDate = @latestDate and Balance.UserOpenId = @userOpenId";

            object queryParams = new { userOpenId };

            IEnumerable<Balance> result = includeRelated
                ? await GetAllWithRelatedEntities(queryText, queryParams)
                : await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<Balance> GetByIdAsync(Guid userOpenId, Guid id)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetBalance.sql");

            object queryParameters = new { userOpenId, id };

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

        public async Task<Balance> CreateAsync(Guid userOpenId, Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "InsertBalance.sql");

            object queryParameters = new { userOpenId, assetId, effectiveDate, value, exchangeRateId };

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

            IEnumerable<Balance> result = await base.GetEntities(queryText, queryParameters);

            return result;
        }

        public async Task<Balance> UpdateAsync(Guid userOpenId, Guid id, Guid assetId, decimal value)
        {
            string queryText = @"
                update Balance
                set [Value] = @value, [AssetId] = @assetId
                where Id = @id and UserOpenId = @userOpenId

                select * from Balance
                where Id = @id";

            object queryParameters = new { userOpenId, id, assetId, value };

            return await base.UpdateEntity(queryText, queryParameters);
        }

        public async Task<bool> RemoveAsync(Guid userOpenId, Guid id)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "DeleteBalance.sql");
            object queryParams = new { userOpenId, id };

            await base.DeleteEntity(queryText, queryParams);

            return true;
        }

        public async Task<IEnumerable<DateTime>> GetOrderedEffectiveDatesAsync(Guid userOpenId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetBalancesEffectiveDates.sql");
            object queryParams = new { userOpenId };

            IEnumerable<DateTime> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.QueryAsync<DateTime>(queryText, queryParams);
            }

            return result;
        }

        public async Task<bool> ExistsEffectiveDateBalancesAsync(Guid userOpenId, DateTime effectiveDate)
        {
            string queryText = @"
                IF EXISTS (SELECT TOP 1 1 FROM [dbo].[Balance] WHERE [UserOpenId] = @userOpenId AND [EffectiveDate] = @effectiveDate)
                SELECT 1 ELSE SELECT 0";

            object queryParams = new { userOpenId, effectiveDate };
            bool result = await base.Exists(queryText, queryParams);

            return result;
        }

        private async Task<IEnumerable<Balance>> GetAllWithRelatedEntities(string queryText, object queryParams = null)
        {
            IEnumerable<Balance> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.QueryAsync<Balance, Asset, ExchangeRate, Balance>(queryText,
                    (balance, asset, rate) =>
                    {
                        balance.Asset = asset;
                        balance.ExchangeRate = rate;

                        return balance;
                    }, queryParams);
            }

            return result;
        }
    }
}
