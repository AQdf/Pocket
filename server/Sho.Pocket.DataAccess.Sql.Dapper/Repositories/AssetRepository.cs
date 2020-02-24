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

        public async Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userId, bool includeInactive)
        {
            string queryText = @"SELECT * FROM [dbo].[Asset] WHERE [UserOpenId] = @userId";

            if (!includeInactive)
            {
                queryText += " AND [IsActive] = 1";
            }

            object queryParams = new { userId };

            IEnumerable<Asset> result = await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<Asset> GetByIdAsync(Guid userId, Guid id)
        {
            const string queryText = @"select top 1 * from Asset where Id = @id and UserOpenId = @userId";
            object queryParams = new { userId, id, };

            Asset result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<Asset> CreateAsync(Guid userId, string name, string currency, bool isActive)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "InsertAsset.sql");

            object queryParameters = new { userId, name, currency, isActive, };

            Asset result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<Asset> UpdateAsync(Guid userId, Guid id, string name, string currency, bool isActive)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "UpdateAsset.sql");

            object queryParameters = new { userId, id, name, currency, isActive, };

            Asset result = await base.UpdateEntity(queryText, queryParameters);

            return result;
        }

        public async Task RemoveAsync(Guid userId, Guid id)
        {
            string queryText = @"delete from Asset where Id = @id and UserOpenId = @userId";

            object queryParameters = new { userId, id };

            await base.DeleteEntity(queryText, queryParameters);
        }

        public async Task<Asset> GetByNameAsync(Guid userId, string name)
        {
            string query = @"
                SELECT [Id], [Name], [IsActive], [Currency]
                FROM [dbo].[Asset]
                WHERE [UserOpenId] = @userId AND [Name] = @name";

            object queryParams = new { userId, name };

            Asset result = await base.GetEntity(query, queryParams);

            return result;
        }
    }
}
