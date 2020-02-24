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

        public async Task<BalanceNote> GetByIdAsync(Guid userId, Guid id)
        {
            string queryText = @"SELECT * FROM [BalanceNote] WHERE [UserId] = @userId AND [Id] = @id";

            object queryParams = new { userId, id };

            BalanceNote result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<BalanceNote> GetByEffectiveDateAsync(Guid userId, DateTime effectiveDate)
        {
            string queryText = @"
                SELECT * FROM [BalanceNote]
                WHERE [UserId] = @userId AND [EffectiveDate] = @effectiveDate";

            object queryParams = new { userId, effectiveDate };

            BalanceNote result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<BalanceNote> CreateAsync(Guid userId, DateTime effectiveDate, string content)
        {
            string queryText = @"
                DECLARE @id uniqueidentifier = NEWID();

                INSERT INTO [BalanceNote] ([Id], [EffectiveDate], [Content], [UserId])
                VALUES (@id, @effectiveDate, @content, @userId)

                SELECT * FROM [BalanceNote] WHERE [Id] = @id";

            object queryParams = new { userId, effectiveDate, content };

            BalanceNote result = await base.InsertEntity(queryText, queryParams);

            return result;
        }

        public async Task<BalanceNote> UpdateAsync(Guid userId, Guid id, string content)
        {
            string queryText = @"
                UPDATE [BalanceNote]
                SET [Content] = @content
                WHERE [Id] = @id AND [UserId] = @userId

                SELECT * FROM [BalanceNote] WHERE [Id] = @id";

            object queryParams = new { userId, id, content };

            BalanceNote result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }
    }
}
