using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class BalanceRepository : BaseRepository<Balance>, IBalanceRepository
    {
        private const string SCRIPTS_DIR_NAME = "Scripts";

        public BalanceRepository(IOptionsMonitor<DbSettings> options) : base(options)
        {
        }

        public async Task<IEnumerable<Balance>> GetAllAsync(Guid userId, bool includeRelated = true)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetAllBalances.sql");
            object queryParams = new { userId };

            IEnumerable<Balance> result = includeRelated
                ? await GetAllWithRelatedEntities(queryText, queryParams)
                : await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<Balance>> GetByEffectiveDateAsync(Guid userId, DateTime effectiveDate, bool includeRelated = true)
        {
            string queryText = @"
                select * from Balance
                join Asset on Asset.Id = Balance.AssetId
                left join ExchangeRate on ExchangeRate.Id = Balance.ExchangeRateId
                where Balance.EffectiveDate = @effectiveDate and Balance.UserOpenId = @userId";

            object queryParams = new { userId, effectiveDate };

            IEnumerable<Balance> result = includeRelated
                ? await GetAllWithRelatedEntities(queryText, queryParams)
                : await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<Balance>> GetLatestBalancesAsync(Guid userId, bool includeRelated = true)
        {
            string queryText = @"
                declare @latestDate datetime2(7) = (select top 1 EffectiveDate from Balance order by EffectiveDate desc)

                select * from Balance
                join Asset on Asset.Id = Balance.AssetId
                left join ExchangeRate on ExchangeRate.Id = Balance.ExchangeRateId
                where Balance.EffectiveDate = @latestDate and Balance.UserOpenId = @userId";

            object queryParams = new { userId };

            IEnumerable<Balance> result = includeRelated
                ? await GetAllWithRelatedEntities(queryText, queryParams)
                : await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<Balance> GetByIdAsync(Guid userId, Guid id)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetBalance.sql");

            object queryParameters = new { userId, id };

            IEnumerable<Balance> resultItems;

            using (IDbConnection db = new SqlConnection(ConnectionString))
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

        public async Task<Balance> CreateAsync(Guid userId, Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "InsertBalance.sql");

            object queryParameters = new { userId, assetId, effectiveDate, value, exchangeRateId };

            Balance result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<Balance> UpdateAsync(Guid userId, Guid id, Guid assetId, decimal value)
        {
            string queryText = @"
                update Balance
                set [Value] = @value, [AssetId] = @assetId
                where Id = @id and UserOpenId = @userId

                select * from Balance
                where Id = @id";

            object queryParameters = new { userId, id, assetId, value };

            return await base.UpdateEntity(queryText, queryParameters);
        }

        public async Task<bool> RemoveAsync(Guid userId, Guid id)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "DeleteBalance.sql");
            object queryParams = new { userId, id };

            await base.DeleteEntity(queryText, queryParams);

            return true;
        }

        public async Task<IEnumerable<DateTime>> GetOrderedEffectiveDatesAsync(Guid userId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetBalancesEffectiveDates.sql");
            object queryParams = new { userId };

            IEnumerable<DateTime> result;

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                result = await db.QueryAsync<DateTime>(queryText, queryParams);
            }

            return result;
        }

        public async Task<bool> ExistsEffectiveDateBalancesAsync(Guid userId, DateTime effectiveDate)
        {
            string queryText = @"
                IF EXISTS (SELECT TOP 1 1 FROM [dbo].[Balance] WHERE [UserOpenId] = @userId AND [EffectiveDate] = @effectiveDate)
                SELECT 1 ELSE SELECT 0";

            object queryParams = new { userId, effectiveDate };
            bool result = await base.Exists(queryText, queryParams);

            return result;
        }

        private async Task<IEnumerable<Balance>> GetAllWithRelatedEntities(string queryText, object queryParams = null)
        {
            IEnumerable<Balance> result;

            using (IDbConnection db = new SqlConnection(ConnectionString))
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

        public async Task<bool> ExistsAssetBalanceAsync(Guid userId, Guid assetId)
        {
            string queryText = @"
                if exists (select top 1 1 from Balance where UserOpenId = @userId and AssetId = @assetId)
                select 1 else select 0";

            object queryParams = new { userId, assetId };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }
    }
}
