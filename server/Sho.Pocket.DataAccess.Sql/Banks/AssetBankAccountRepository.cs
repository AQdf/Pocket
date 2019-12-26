using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<AssetBankAccount>> GetByUserIdAsync(Guid userId)
        {
            const string queryText = @"
                SELECT [AssetBankAccount].[Id] AS Id,
                    [AssetBankAccount].[AssetId] AS AssetId,
                    [AssetBankAccount].[BankName] AS BankName,
                    [AssetBankAccount].[BankAccountId] AS BankAccountId,
                    [AssetBankAccount].[LastSyncDateTime] AS LastSyncDateTime,
                    [AssetBankAccount].[UserBankAuthDataId] AS UserBankAuthDataId,
                    [AssetBankAccount].[BankAccountName] AS BankAccountName
                FROM [dbo].[AssetBankAccount]
                LEFT JOIN [dbo].[Asset] ON [Asset].[Id] = [AssetBankAccount].[AssetId]
                WHERE [Asset].[UserOpenId] = @userId";

            object queryParams = new { userId };
            IEnumerable<AssetBankAccount> result = await base.GetEntities(queryText, queryParams);

            return result.ToList();
        }

        public async Task<AssetBankAccount> CreateAsync(Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId)
        {
            const string queryText = @"
                DECLARE @id uniqueidentifier = NEWID()
                DECLARE @authDataId uniqueidentifier = (SELECT [Id] FROM [dbo].[UserBankAuthData] WHERE [UserId] = @userId AND [BankName] = @bankName)

                INSERT INTO [dbo].[AssetBankAccount] ([Id], [AssetId], [BankName], [BankAccountId], [LastSyncDateTime], [UserBankAuthDataId], [BankAccountName])
                VALUES (@id, @assetId, @bankName, @bankAccountId, NULL, @authDataId, @accountName)

                SELECT [Id], [Id], [AssetId], [BankName], [BankAccountId], [LastSyncDateTime]
                FROM [dbo].[AssetBankAccount]
                WHERE [Id] = @id";

            object queryParams = new { userId, assetId, bankName, bankAccountId, accountName };
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

        public async Task<AssetBankAccount> UpdateAsync(Guid userId, Guid assetId, DateTime lastSyncDateTime, string bankAccountName)
        {
            const string queryText = @"
                DECLARE @assetBankAccountId UNIQUEIDENTIFIER = (
	                SELECT [AssetBankAccount].[Id] FROM [dbo].[AssetBankAccount]
	                LEFT JOIN [dbo].[Asset] ON [Asset].[Id] = [AssetBankAccount].[AssetId]
	                WHERE [Asset].[Id] = @assetId AND [Asset].[UserOpenId] = @userId)

                IF (@assetBankAccountId IS NOT NULL)
                BEGIN
	                UPDATE [dbo].[AssetBankAccount]
	                SET [LastSyncDateTime] = @lastSyncDateTime, [BankAccountName] = @bankAccountName
	                WHERE [AssetId] = @assetId

	                SELECT * FROM [dbo].[AssetBankAccount]
	                WHERE [Id] = @assetBankAccountId
                END";

            object queryParams = new { userId, assetId, lastSyncDateTime, bankAccountName };

            AssetBankAccount result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }
    }
}
