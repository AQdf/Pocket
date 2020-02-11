using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class BalanceNoteRepository : BaseRepository<BalanceNote>, IBalanceNoteRepository
    {
        public BalanceNoteRepository(IOptionsMonitor<DbSettings> options) : base(options)
        {
        }

        public async Task<BalanceNote> GetByIdAsync(Guid userOpenId, Guid id)
        {
            string queryText = @"
                SELECT [Id], [EffectiveDate], [Content], [UserOpenId]
                FROM [dbo].[BalanceNote]
                WHERE [UserOpenId] = @userOpenId AND [Id] = @id";

            object queryParams = new { userOpenId, id };

            BalanceNote result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<BalanceNote> GetByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate)
        {
            string queryText = @"
                SELECT [Id], [EffectiveDate], [Content], [UserOpenId]
                FROM [dbo].[BalanceNote]
                WHERE [UserOpenId] = @userOpenId AND [EffectiveDate] = @effectiveDate";

            object queryParams = new { userOpenId, effectiveDate };

            BalanceNote result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<BalanceNote> AlterAsync(Guid userOpenId, DateTime effectiveDate, string content)
        {
            string queryText = @"
                DECLARE @id uniqueidentifier = (
	                SELECT TOP 1 Id FROM [BalanceNote]
	                WHERE [EffectiveDate] = @effectiveDate)

                IF @id IS NULL
	                BEGIN
		                SET @id = NEWID();
                        INSERT INTO [dbo].[BalanceNote] ([Id], [EffectiveDate], [Content], [UserOpenId])
                        VALUES (@id, @effectiveDate, @content, @userOpenId)
	                END
                ELSE
	                BEGIN
		                UPDATE [dbo].[BalanceNote]
		                SET [Content] = @content
		                WHERE [Id] = @id AND [UserOpenId] = @userOpenId
	                END

                SELECT [Id], [EffectiveDate], [Content], [UserOpenId]
                FROM [dbo].[BalanceNote] WHERE [Id] = @id";

            object queryParameters = new
            {
                effectiveDate,
                userOpenId,
                content
            };

            BalanceNote result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<BalanceNote> CreateAsync(Guid userOpenId, DateTime effectiveDate, string content)
        {
            string queryText = @"
                DECLARE @id uniqueidentifier = NEWID();

                INSERT INTO [dbo].[BalanceNote] ([Id], [EffectiveDate], [Content], [UserOpenId])
                VALUES (@id, @effectiveDate, @content, @userOpenId)

                SELECT [Id], [EffectiveDate], [Content], [UserOpenId]
                FROM [dbo].[BalanceNote] WHERE [Id] = @id";

            object queryParams = new { userOpenId, effectiveDate, content };

            BalanceNote result = await base.InsertEntity(queryText, queryParams);

            return result;
        }

        public async Task<BalanceNote> UpdateAsync(Guid userOpenId, Guid id, string content)
        {
            string queryText = @"
                UPDATE [dbo].[BalanceNote]
                SET [Content] = @content
                WHERE [Id] = @id AND [UserOpenId] = @userOpenId

                SELECT [Id], [EffectiveDate], [Content], [UserOpenId]
                FROM [dbo].[BalanceNote] WHERE [Id] = @id";

            object queryParams = new { userOpenId, id, content };

            BalanceNote result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }
    }
}
