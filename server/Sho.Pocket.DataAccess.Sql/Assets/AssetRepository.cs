using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.DataAccess.Sql.Assets
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        private const string SCRIPTS_DIR_NAME = "Assets.Scripts";

        public AssetRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userOpenId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetAllAssets.sql");
            object queryParams = new { userOpenId };

            IEnumerable<Asset> result = await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<Asset> GetByIdAsync(Guid userOpenId, Guid id)
        {
            const string queryText = @"select top 1 * from Asset where Id = @id and UserOpenId = @userOpenId";
            object queryParams = new { userOpenId, id, };

            Asset result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<Asset> CreateAsync(Guid userOpenId, string name, string currency, bool isActive)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "InsertAsset.sql");

            object queryParameters = new { userOpenId, name, currency, isActive, };

            Asset result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<Asset> UpdateAsync(Guid userOpenId, Guid id, string name, string currency, bool isActive)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "UpdateAsset.sql");

            object queryParameters = new { userOpenId, id, name, currency, isActive, };

            Asset result = await base.UpdateEntity(queryText, queryParameters);

            return result;
        }

        public async Task RemoveAsync(Guid userOpenId, Guid id)
        {
            string queryText = @"delete from Asset where Id = @id and UserOpenId = @userOpenId";

            object queryParameters = new { userOpenId, id };

            await base.DeleteEntity(queryText, queryParameters);
        }

        public async Task<IEnumerable<Asset>> GetActiveAssetsAsync()
        {
            string query = @"
                SELECT [Asset].[Id] AS [ID]
                      ,[Asset].[Name] AS [Name]
                      ,[Asset].[IsActive] AS [IsActive]
                      ,[Asset].[CurrencyId] AS [CurrencyId]
	                  ,[Currency].[Name] AS CurrencyName
                FROM [dbo].[Asset]
                JOIN [dbo].[Currency] ON [Currency].[Id] = [Asset].[CurrencyId]
                WHERE [Asset].[IsActive] = 1
                ORDER BY [Asset].[Name] ASC";

            IEnumerable<Asset> result = await base.GetEntities(query);

            return result;
        }

        public async Task<bool> ExistsAssetBalanceAsync(Guid id)
        {
            string queryText = @"
                if exists (select top 1 1 from Balance where AssetId = @id)
                select 1 else select 0";

            object queryParams = new { id };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }
    }
}
