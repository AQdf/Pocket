using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class UserCurrencyRepository : BaseRepository<UserCurrency>, IUserCurrencyRepository
    {
        public UserCurrencyRepository(IOptionsMonitor<DbSettings> options) : base(options)
        {
        }

        public async Task<IEnumerable<UserCurrency>> GetByUserIdAsync(Guid userId)
        {
            const string queryText = @"SELECT * FROM [UserCurrency] WHERE [UserId] = @userId ORDER BY [Currency]";
            object queryParams = new { userId };

            IEnumerable<UserCurrency> result = await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<UserCurrency> GetCurrencyAsync(Guid userId, string currency)
        {
            const string queryText = @"
                SELECT TOP 1 * FROM [UserCurrency]
                WHERE [Currency] = @currency and [UserId] = @userId";

            object queryParams = new { userId, currency };

            UserCurrency result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<UserCurrency> CreateAsync(Guid userId, string currency, bool isPrimary)
        {
            const string queryText =@"
                INSERT INTO [UserCurrency]
                ([UserId], [Currency], [IsPrimary])
                VALUES (@userId, @currency, @isPrimary)

                SELECT * FROM [UserCurrency] WHERE [Currency] = @currency and [UserId] = @userId";
            object queryParams = new { userId, currency, isPrimary };

            UserCurrency result = await base.InsertEntity(queryText, queryParams);

            if (isPrimary)
            {
                await SetPrimaryAsync(userId, currency);

            }

            return result;
        }

        public async Task<UserCurrency> SetPrimaryAsync(Guid userId, string currency)
        {
            const string queryText = @"
                UPDATE [UserCurrency]
                SET [IsPrimary] = 0
                WHERE [UserId] = @userId

                UPDATE [UserCurrency]
                SET [IsPrimary] = 1
                WHERE [Currency] = @currency AND [UserId] = @userId

                SELECT * FROM [UserCurrency] WHERE [Currency] = @currency AND [UserId] = @userId";

            object queryParams = new { userId, currency };

            UserCurrency result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }

        public async Task<bool> DeleteAsync(Guid userId, string currency)
        {
            string queryText = @"DELETE FROM [UserCurrency] WHERE [Currency] = @currency AND [UserId] = @userId";
            object queryParameters = new { userId, currency };

            await base.DeleteEntity(queryText, queryParameters);

            return true;
        }

        public async Task<UserCurrency> GetPrimaryCurrencyAsync(Guid userId)
        {
            const string queryText = @"SELECT TOP 1 * FROM [UserCurrency] WHERE [UserId] = @userId AND [IsPrimary] = 1";
            object queryParams = new { userId };

            UserCurrency result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<bool> CheckIsPrimaryAsync(Guid userId, string currency)
        {
            const string queryText =@"
                IF EXISTS (SELECT TOP 1 1 FROM [UserCurrency] WHERE [Currency] = @currency AND [UserId] = @userId AND [IsPrimary] = 1)
                SELECT 1 ELSE SELECT 0";
            object queryParams = new { userId, currency };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }
    }
}
