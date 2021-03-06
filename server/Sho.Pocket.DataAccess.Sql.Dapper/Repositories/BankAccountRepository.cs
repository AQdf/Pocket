﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class BankAccountRepository : BaseRepository<AssetBankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(IOptionsMonitor<DbSettings> options) : base(options)
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
                    [AssetBankAccount].[BankAccountName] AS BankAccountName,
                    [AssetBankAccount].[Token] AS Token,
                    [AssetBankAccount].[BankClientId] AS BankClientId
                FROM [AssetBankAccount]
                LEFT JOIN [Asset] ON [Asset].[Id] = [AssetBankAccount].[AssetId]
                WHERE [Asset].[UserId] = @userId";

            object queryParams = new { userId };
            IEnumerable<AssetBankAccount> result = await base.GetEntities(queryText, queryParams);

            return result.ToList();
        }

        public async Task<AssetBankAccount> AlterAsync(Guid userId, Guid assetId, string bankName, string token, string bankClientId)
        {
            string queryText = @"
                DECLARE @id uniqueidentifier = (
	                SELECT TOP 1 [AssetBankAccount].[Id] FROM [AssetBankAccount]
                    LEFT JOIN [Asset] ON [Asset].[Id] = [AssetBankAccount].[AssetId]
	                WHERE [Asset].[UserId] = @userId AND [AssetBankAccount].[AssetId] = @assetId)

                IF @id IS NULL
	                BEGIN
		                SET @id = NEWID();
                        INSERT INTO [AssetBankAccount] ([Id], [AssetId], [BankName], [Token], [BankClientId])
                        VALUES (@id, @assetId, @bankName, @token, @bankClientId)
	                END
                ELSE
	                BEGIN
		                UPDATE [AssetBankAccount]
		                SET [AssetId] = @assetId, [BankName] = @bankName, [Token] = @token, [BankClientId] = @bankClientId
		                WHERE [Id] = @id
	                END

                SELECT * FROM [AssetBankAccount] WHERE [Id] = @id";

            object queryParameters = new
            {
                userId,
                assetId,
                bankName,
                token,
                bankClientId
            };

            AssetBankAccount result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<AssetBankAccount> UpdateAccountAsync(Guid userId, Guid assetId, string accountName, string bankAccountId)
        {
            const string queryText = @"
                DECLARE @id uniqueidentifier = (
	                SELECT TOP 1 [AssetBankAccount].[Id] FROM [AssetBankAccount]
                    LEFT JOIN [Asset] ON [Asset].[Id] = [AssetBankAccount].[AssetId]
	                WHERE [Asset].[UserId] = @userId AND [AssetBankAccount].[AssetId] = @assetId)

		        UPDATE [AssetBankAccount]
		        SET [BankAccountName] = @accountName, [BankAccountId] = @bankAccountId
		        WHERE [Id] = @id

                SELECT * FROM [AssetBankAccount] WHERE [Id] = @id";

            object queryParams = new { userId, assetId, accountName, bankAccountId };
            AssetBankAccount result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }

        public async Task<AssetBankAccount> GetByAssetIdAsync(Guid userId, Guid assetId)
        {
            const string queryText = @"
                DECLARE @id uniqueidentifier = (SELECT [Id] FROM [Asset] WHERE [UserId] = @userId AND [Id] = @assetId)

                SELECT * FROM [AssetBankAccount]
                WHERE [AssetId] = @id";

            object queryParams = new { userId, assetId };
            AssetBankAccount result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task DeleteAsync(Guid userId, Guid assetId)
        {
            const string queryText = @"
                DECLARE @id uniqueidentifier = (SELECT [Id] FROM [Asset] WHERE [UserId] = @userId AND [Id] = @assetId)

                DELETE FROM [AssetBankAccount]
                WHERE [AssetId] = @id";

            object queryParams = new { userId, assetId };
            await base.DeleteEntity(queryText, queryParams);
        }

        public async Task<AssetBankAccount> UpdateLastSyncAsync(Guid userId, Guid assetId, DateTime lastSyncDateTime, string bankAccountName)
        {
            const string queryText = @"
                DECLARE @assetBankAccountId UNIQUEIDENTIFIER = (
	                SELECT [AssetBankAccount].[Id] FROM [AssetBankAccount]
	                LEFT JOIN [Asset] ON [Asset].[Id] = [AssetBankAccount].[AssetId]
	                WHERE [Asset].[Id] = @assetId AND [Asset].[UserId] = @userId)

                IF (@assetBankAccountId IS NOT NULL)
                BEGIN
	                UPDATE [AssetBankAccount]
	                SET [LastSyncDateTime] = @lastSyncDateTime, [BankAccountName] = @bankAccountName
	                WHERE [AssetId] = @assetId

	                SELECT * FROM [AssetBankAccount]
	                WHERE [Id] = @assetBankAccountId
                END";

            object queryParams = new { userId, assetId, lastSyncDateTime, bankAccountName };

            AssetBankAccount result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }
    }
}
