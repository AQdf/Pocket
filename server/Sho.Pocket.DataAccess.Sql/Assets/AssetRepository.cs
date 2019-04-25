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

        public async Task<IEnumerable<Asset>> GetAll()
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetAllAssets.sql");

            IEnumerable<Asset> result = await base.GetAll(queryText);

            return result;
        }

        public async Task<IEnumerable<Asset>> GetActiveAssets()
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

            IEnumerable<Asset> result = await base.GetAll(query);

            return result;
        }

        public async Task<Asset> GetById(Guid id)
        {
            string queryText = @"select top 1 * from Asset where Id = @id";

            object queryParams = new { id };

            Asset result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<Asset> Add(string name, Guid currencyId, bool isActive)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "InsertAsset.sql");

            object queryParameters = new { name, currencyId, isActive };

            Asset result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<Asset> Update(Guid id, string name, Guid currencyId, bool isActive)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "UpdateAsset.sql");

            object queryParameters = new { id, name, currencyId, isActive };

            Asset result = await base.UpdateEntity(queryText, queryParameters);

            return result;
        }

        public async Task Remove(Guid assetId)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "DeleteAsset.sql");

            object queryParameters = new
            {
                id = assetId
            };

            await base.RemoveEntity(queryText, queryParameters);
        }

        public async Task<bool> ExistsAssetBalance(Guid id)
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
