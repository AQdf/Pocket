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

        public async Task<IEnumerable<Balance>> GetAllAsync(Guid userId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetUserBalances.sql");
            object queryParams = new { userId };

            IEnumerable<Balance> result = await GetAllWithRelatedEntities(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<Balance>> GetByEffectiveDateAsync(Guid userId, DateTime effectiveDate)
        {
            string queryText = @"
                SELECT * FROM [Balance]
                JOIN [Asset] ON [Asset].[Id] = [Balance].[AssetId]
                LEFT JOIN [ExchangeRate] ON [ExchangeRate].[Id] = [Balance].[ExchangeRateId]
                WHERE [Balance].[UserId] = @userId AND [Balance].[EffectiveDate] = @effectiveDate";

            object queryParams = new { userId, effectiveDate };

            IEnumerable<Balance> result = await GetAllWithRelatedEntities(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<Balance>> GetLatestBalancesAsync(Guid userId)
        {
            string queryText = @"
                DECLARE @latestDate datetime2(7) = (SELECT TOP 1 [EffectiveDate] FROM [Balance] ORDER BY [EffectiveDate] DESC)

                SELCET * FROM [Balance]
                JOIN [Asset] ON [Asset].[Id] = [Balance].[AssetId]
                LEFT JOIN [ExchangeRate] on [ExchangeRate].[Id] = [Balance].[ExchangeRateId]
                WHERE [Balance].[EffectiveDate] = @latestDate AND [Balance].[UserId] = @userId";

            object queryParams = new { userId };

            IEnumerable<Balance> result = await GetAllWithRelatedEntities(queryText, queryParams);

            return result;
        }

        public async Task<Balance> GetByIdAsync(Guid userId, Guid id)
        {
            string queryText = @"
                SELECT * FROM [Balance] b 
                JOIN [Asset] a ON a.[Id] = b.[AssetId]
                JOIN [ExchangeRate] r ON r.[Id] = b.[ExchangeRateId]
                WHERE b.[Id] = @id AND b.[UserId] = @userId";

            object queryParameters = new { userId, id };

            IEnumerable<Balance> resultItems;

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                resultItems = await db.QueryAsync<Balance, Asset, Balance>(queryText,
                    (balance, asset) =>
                    {
                        balance.Asset = asset;
                        return balance;
                    },
                    queryParameters);
            }

            return resultItems.FirstOrDefault();
        }

        public async Task<Balance> CreateAsync(Guid userId, Guid assetId, DateTime effectiveDate, decimal value)
        {
            string queryText = @"
                DECLARE @id UNIQUEIDENTIFIER = NEWID();

                INSERT INTO [Balance] ([Id], [AssetId], [Value], [EffectiveDate], [UserId]) values
                    (@id, @assetId, @value, @effectiveDate, @userId)

                SELECT * FROM [Balance] WHERE [Id] = @id";

            object queryParameters = new { userId, assetId, effectiveDate, value};

            Balance result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<Balance> UpdateAsync(Guid userId, Guid id, Guid assetId, decimal value)
        {
            string queryText = @"
                UPDATE [Balance]
                SET [Value] = @value, [AssetId] = @assetId
                WHERE [Id] = @id AND [UserId] = @userId

                SELECT * FROM [Balance] WHERE [Id] = @id";

            object queryParameters = new { userId, id, assetId, value };

            return await base.UpdateEntity(queryText, queryParameters);
        }

        public async Task<bool> RemoveAsync(Guid userId, Guid id)
        {
            string queryText = @"DELETE FROM [Balance] WHERE [Id] = @id AND [UserId] = @userId";
            object queryParams = new { userId, id };

            await base.DeleteEntity(queryText, queryParams);

            return true;
        }

        public async Task<IEnumerable<DateTime>> GetOrderedEffectiveDatesAsync(Guid userId)
        {
            string queryText = @"
                SELECT [EffectiveDate] FROM [Balance]
                WHERE [UserId] = @userId
                GROUP BY [EffectiveDate]
                ORDER BY [EffectiveDate] DESC";

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
                IF EXISTS (SELECT TOP 1 1 FROM [Balance] WHERE [UserId] = @userId AND [EffectiveDate] = @effectiveDate)
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
                result = await db.QueryAsync<Balance, Asset, Balance>(queryText,
                    (balance, asset) =>
                    {
                        balance.Asset = asset;
                        return balance;
                    },
                    queryParams);
            }

            return result;
        }

        public async Task<bool> ExistsAssetBalanceAsync(Guid userId, Guid assetId)
        {
            string queryText = @"
                IF EXISTS (SELECT TOP 1 1 FROM [Balance] WHERE [UserId] = @userId AND AssetId = @assetId)
                SELECT 1 ELSE SELECT 0";

            object queryParams = new { userId, assetId };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }
    }
}
