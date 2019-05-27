using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Inventory
{
    public class InventoryRepository : BaseRepository<InventoryItem>, IInventoryRepository
    {
        public InventoryRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsAsync(Guid userOpenId)
        {
            const string queryText =
                @"SELECT [Id], [Name], [Description], [Category], [UserOpenId]
                FROM [dbo].[InventoryItem]
                WHERE [UserOpenId] = @userOpenId";
            object queryParams = new { userOpenId };

            IEnumerable<InventoryItem> result = await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<InventoryItem> GetItemAsync(Guid userOpenId, Guid id)
        {
            const string queryText =
                @"SELECT TOP 1 [Id], [Name], [Description], [Category], [UserOpenId]
                FROM [dbo].[InventoryItem]
                WHERE [Id] = @id and [UserOpenId] = @userOpenId";
            object queryParams = new { userOpenId, id, };

            InventoryItem result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<InventoryItem> CreateAsync(Guid userOpenId, string name, string description, string category)
        {
            const string queryText =
                @"DECLARE @id uniqueidentifier = NEWID()

                INSERT INTO [dbo].[InventoryItem]
                ([Id], [Name], [Description], [Category], [UserOpenId])
                VALUES (@id, @name, @description, @category, @userOpenId)

                SELECT [Id], [Name], [Description], [Category], [UserOpenId]
                FROM [dbo].[InventoryItem]
                WHERE [Id] = @id";
            object queryParams = new { userOpenId, name, description, category };

            InventoryItem result = await base.InsertEntity(queryText, queryParams);

            return result;
        }

        public async Task<InventoryItem> UpdateAsync(Guid userOpenId, Guid id, string name, string description, string category)
        {
            const string queryText =
                @"UPDATE [dbo].[InventoryItem]
                SET [Name] = @name, [Description] = @description, [Category] = @category
                WHERE [Id] = @id AND [UserOpenId] = @userOpenId

                SELECT [Id], [Name], [Description], [Category], [UserOpenId]
                FROM [dbo].[InventoryItem]
                WHERE [Id] = @id";
            object queryParams = new { userOpenId, id, name, description, category };

            InventoryItem result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }

        public async Task<bool> RemoveAsync(Guid userOpenId, Guid id)
        {
            string queryText = 
                @"DELETE FROM [dbo].[InventoryItem]
                WHERE [Id] = @id AND [UserOpenId] = @userOpenId";
            object queryParameters = new { userOpenId, id };

            await base.RemoveEntity(queryText, queryParameters);

            return true;
        }
    }
}
