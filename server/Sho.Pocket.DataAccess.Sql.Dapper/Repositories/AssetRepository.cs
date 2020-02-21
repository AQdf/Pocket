using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        private const string SCRIPTS_DIR_NAME = "Scripts";

        public AssetRepository(IOptionsMonitor<DbSettings> options) : base(options)
        {
        }

        public async Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userOpenId, bool includeInactive)
        {
            string queryText = @"SELECT * FROM [dbo].[Asset] WHERE [UserOpenId] = @userOpenId";

            if (!includeInactive)
            {
                queryText += " AND [IsActive] = 1";
            }

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

        public async Task<Asset> GetByNameAsync(Guid userOpenId, string name)
        {
            string query = @"
                SELECT [Id], [Name], [IsActive], [Currency]
                FROM [dbo].[Asset]
                WHERE [UserOpenId] = @userOpenId AND [Name] = @name";

            object queryParams = new { userOpenId, name };

            Asset result = await base.GetEntity(query, queryParams);

            return result;
        }
    }
}
