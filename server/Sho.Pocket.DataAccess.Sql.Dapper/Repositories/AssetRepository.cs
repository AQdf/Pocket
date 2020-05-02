using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;
using Sho.Pocket.Domain.ValueObjects;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        public AssetRepository(IOptionsMonitor<DbSettings> options) : base(options)
        {
        }

        public async Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userId, bool includeInactive)
        {
            string queryText = @"SELECT * FROM [Asset] WHERE [UserId] = @userId";

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
            string queryText = @"SELECT TOP 1 * FROM [Asset] WHERE [Id] = @id AND [UserId] = @userId";
            object queryParams = new { userId, id, };

            Asset result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<Asset> CreateAsync(Guid userId, string name, Money balance, bool isActive, DateTime updatedOn)
        {
            string queryText = @"
                DECLARE @id UNIQUEIDENTIFIER = NEWID()
                INSERT INTO [Asset] ([Id], [Name], [Currency], [IsActive], [UserId], [Value], [UpdatedOn]) values
                    (@id, @name, @currency, @isActive, @userId, @value, @updatedOn)

                SELECT * FROM [Asset] WHERE [Asset].[Id] = @id";

            object queryParameters = new { userId, name, balance.Currency, isActive, balance.Value, updatedOn};

            Asset result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<Asset> UpdateAsync(Guid userId, Guid id, string name, Money balance, bool isActive, DateTime updatedOn)
        {
            string queryText = @"
                UPDATE [Asset]
                SET [Name] = @name,
	                [Currency] = @currency,
	                [IsActive] = @isActive,
                    [Value] = @value,
                    [UpdatedOn] = @updatedOn
                WHERE [Id] = @id AND [UserId] = @userId

                SELECT * FROM [Asset] WHERE [Asset].[Id] = @id";

            object queryParameters = new { userId, id, name, balance.Currency, isActive, balance.Value, updatedOn };

            Asset result = await base.UpdateEntity(queryText, queryParameters);

            return result;
        }

        public async Task RemoveAsync(Guid userId, Guid id)
        {
            string queryText = @"DELETE FROM [Asset] WHERE [Id] = @id AND [UserId] = @userId";

            object queryParameters = new { userId, id };

            await base.DeleteEntity(queryText, queryParameters);
        }

        public async Task<Asset> GetByNameAsync(Guid userId, string name)
        {
            string query = @"
                SELECT * FROM [Asset]
                WHERE [UserId] = @userId AND [Name] = @name";

            object queryParams = new { userId, name };

            Asset result = await base.GetEntity(query, queryParams);

            return result;
        }
    }
}
