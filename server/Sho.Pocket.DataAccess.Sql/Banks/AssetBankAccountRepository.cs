using System;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Banks
{
    public class AssetBankAccountRepository : BaseRepository<AssetBankAccount>, IAssetBankAccountRepository
    {
        public AssetBankAccountRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<AssetBankAccount> CreateAsync(Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId)
        {
            const string queryText = @"
                DECLARE @id uniqueidentifier = NEWID()
                DECLARE @authDataId uniqueidentifier = (SELECT [Id] FROM [dbo].[UserBankAuthData] WHERE [UserId] = @userId AND [BankName] = @bankName)

                INSERT INTO [dbo].[AssetBankAccount] ([Id], [AssetId], [BankName], [BankAccountId], [LastSyncDateTime], [UserBankAuthDataId], [BankAccountName])
                VALUES (@id, @assetId, @bankName, @bankAccountId, @currentDate, @authDataId, @accountName)

                SELECT [Id], [Id], [AssetId], [BankName], [BankAccountId], [LastSyncDateTime]
                FROM [dbo].[AssetBankAccount]
                WHERE [Id] = @id";

            DateTime currentDate = DateTime.UtcNow;
            object queryParams = new { userId, assetId, bankName, bankAccountId, accountName, currentDate };
            AssetBankAccount result = await base.InsertEntity(queryText, queryParams);

            return result;
        }

        public async Task<AssetBankAccount> GetAsync(Guid userId, Guid assetId)
        {
            const string queryText = @"
                DECLARE @id uniqueidentifier = (SELECT [Id] FROM [Asset] WHERE [UserOpenId] = @userId AND [Id] = @assetId)

                SELECT * FROM [dbo].[AssetBankAccount]
                WHERE [AssetId] = @id";

            object queryParams = new { userId, assetId };
            AssetBankAccount result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task DeleteAsync(Guid userId, Guid assetId)
        {
            const string queryText = @"
                DECLARE @id uniqueidentifier = (SELECT [Id] FROM [Asset] WHERE [UserOpenId] = @userId AND [Id] = @assetId)

                DELETE FROM [dbo].[AssetBankAccount]
                WHERE [AssetId] = @id";

            object queryParams = new { userId, assetId };
            await base.DeleteEntity(queryText, queryParams);
        }
    }
}
