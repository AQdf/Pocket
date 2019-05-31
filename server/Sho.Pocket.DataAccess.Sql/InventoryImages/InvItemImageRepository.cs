using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sho.Pocket.DataAccess.Sql.InventoryImages
{
    public class InvItemImageRepository : BaseRepository<InvItemImage>, IInvItemImageRepository
    {
        //TODO: Move to config
        private const string _folderPath = "Path";

        public InvItemImageRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<IEnumerable<InvItemImage>> GetByItemIdAsync(Guid itemId)
        {
            const string queryText =
                @"SELECT [Id], [InvItemId], [FileName]
                FROM [dbo].[InvItemImage]
                WHERE [InvItemId] = @itemId";
            object queryParams = new { itemId };

            IEnumerable<InvItemImage> itemImages = await base.GetEntities(queryText, queryParams);

            foreach (InvItemImage itemImage in itemImages)
            {
                itemImage.Content = GetImageContent(itemImage);
            }

            return itemImages;
        }

        public async Task<InvItemImage> GetAsync(Guid id)
        {
            const string queryText =
                @"SELECT TOP 1 [Id], [InvItemId], [FileName]
                FROM [dbo].[InvItemImage]
                WHERE [Id] = @id";
            object queryParams = new { id };

            InvItemImage itemImage = await base.GetEntity(queryText, queryParams);
            itemImage.Content = GetImageContent(itemImage);

            return itemImage;
        }

        public async Task<InvItemImage> AddAsync(Guid itemId, string fileName, byte[] image)
        {
            SaveImageToFileSystem(itemId.ToString(), fileName, image);

            const string queryText =
                @"DECLARE @id uniqueidentifier = NEWID()

                INSERT INTO [dbo].[InvItemImage]
                ([Id], [InvItemId], [FileName])
                VALUES (@id, @itemId, @fileName)

                SELECT [Id], [InvItemId], [FileName]
                FROM [dbo].[InvItemImage]
                WHERE [Id] = @id";
            object queryParams = new { itemId, fileName };

            InvItemImage result = await base.InsertEntity(queryText, queryParams);

            return result;
        }

        public async Task<bool> RemoveAsync(Guid id, Guid itemId, string fileName)
        {
            RemoveImageFromFileSystem(itemId.ToString(), fileName);

            string queryText = @"DELETE FROM [dbo].[InvItemImage] WHERE [Id] = @id";
            object queryParameters = new { id };

            await base.DeleteEntity(queryText, queryParameters);

            return true;
        }

        private string GetImagePath(string itemId, string fileName)
        {
            string result = Path.Combine(_folderPath, itemId.ToString(), fileName);
            return result;
        }

        private byte[] GetImageContent(InvItemImage itemImage)
        {
            string fullPath = GetImagePath(itemImage.InvItemId.ToString(), itemImage.FileName);
            byte[] result = File.ReadAllBytes(fullPath);

            return result;
        }
        private void SaveImageToFileSystem(string itemId, string fileName, byte[] image)
        {
            string fullPath = GetImagePath(itemId.ToString(), fileName);
            File.WriteAllBytes(fullPath, image);
        }

        private void RemoveImageFromFileSystem(string itemId, string fileName)
        {
            string fullPath = GetImagePath(itemId.ToString(), fileName);
            File.Delete(fullPath);
        }
    }
}
