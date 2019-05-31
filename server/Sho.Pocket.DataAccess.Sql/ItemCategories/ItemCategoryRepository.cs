using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.ItemCategories
{
    public class ItemCategoryRepository : BaseRepository<ItemCategory>, IItemCategoryRepository
    {
        public ItemCategoryRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<IEnumerable<ItemCategory>> GetAllAsync()
        {
            const string queryText = @"SELECT [Name] FROM [dbo].[ItemCategory]";

            IEnumerable<ItemCategory> result = await base.GetEntities(queryText);

            return result;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            const string queryText =
                @"IF EXISTS (SELECT TOP 1 1 FROM [dbo].[ItemCategory] WHERE [Name] = @name)
                SELECT 1 ELSE SELECT 0";
            object queryParams = new { name };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }

        public async Task<ItemCategory> CreateAsync(string name)
        {
            const string queryText =
                @"INSERT INTO [dbo].[ItemCategory]([Name]) VALUES (@name)

                SELECT [Name] FROM [dbo].[ItemCategory] WHERE [Name] = @name";
            object queryParams = new { name };

            ItemCategory result = await base.InsertEntity(queryText, queryParams);

            return result;
        }

        public async Task<ItemCategory> UpdateAsync(string oldName, string newName)
        {
            const string queryText =
                @"UPDATE [dbo].[ItemCategory]
                SET [Name] = @newName
                WHERE [Name] = @oldName

                SELECT [Name] FROM [dbo].[ItemCategory] WHERE [Name] = @newName";
            object queryParams = new { oldName, newName };

            ItemCategory result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }

        public async Task<bool> DeleteAsync(string name)
        {
            const string queryText = @"DELETE FROM [dbo].[ItemCategory] WHERE [Name] = @name";
            object queryParams = new { name };

            await base.DeleteEntity(queryText, queryParams);

            return true;
        }
    }
}
