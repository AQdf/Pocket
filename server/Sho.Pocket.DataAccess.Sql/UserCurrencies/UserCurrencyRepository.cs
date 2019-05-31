using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.UserCurrencies
{
    public class UserCurrencyRepository : BaseRepository<UserCurrency>, IUserCurrencyRepository
    {
        public UserCurrencyRepository(IDbConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<UserCurrency>> GetByUserIdAsync(Guid userOpenId)
        {
            const string queryText =
                @"SELECT [UserOpenId], [Currency], [IsPrimary]
                FROM [dbo].[UserCurrency]
                WHERE [UserOpenId] = @userOpenId";
            object queryParams = new { userOpenId };

            IEnumerable<UserCurrency> result = await base.GetEntities(queryText, queryParams);

            return result;
        }

        public async Task<UserCurrency> GetCurrencyAsync(Guid userOpenId, string currency)
        {
            const string queryText =
                @"SELECT TOP 1 [UserOpenId], [Currency], [IsPrimary]
                FROM [dbo].[UserCurrency]
                WHERE [Currency] = @currency and [UserOpenId] = @userOpenId";
            object queryParams = new { userOpenId, currency };

            UserCurrency result = await base.GetEntity(queryText, queryParams);

            return result;
        }

        public async Task<UserCurrency> CreateAsync(Guid userOpenId, string currency, bool isPrimary)
        {
            const string queryText =
                @"INSERT INTO [dbo].[UserCurrency]
                ([Id], [UserOpenId], [Currency], [IsPrimary])
                VALUES (@userOpenId, @currency, @isPrimary)

                SELECT [UserOpenId], [Currency], [IsPrimary]
                FROM [dbo].[UserCurrency]
                WHERE [Currency] = @currency and [UserOpenId] = @userOpenId";
            object queryParams = new { userOpenId, currency, isPrimary };

            UserCurrency result = await base.InsertEntity(queryText, queryParams);

            if (isPrimary)
            {
                await SetPrimaryAsync(userOpenId, currency);

            }

            return result;
        }

        public async Task<UserCurrency> SetPrimaryAsync(Guid userOpenId, string currency)
        {
            const string queryText =
                @"UPDATE [dbo].[UserCurrency]
                SET [IsPrimary] = 0
                WHERE [UserOpenId] = @userOpenId

                UPDATE [dbo].[UserCurrency]
                SET [IsPrimary] = 1
                WHERE [Currency] = @currency AND [UserOpenId] = @userOpenId

                SELECT [UserOpenId], [Currency], [IsPrimary]
                FROM [dbo].[UserCurrency]
                WHERE [Currency] = @currency AND [UserOpenId] = @userOpenId";
            object queryParams = new { userOpenId, currency };

            UserCurrency result = await base.UpdateEntity(queryText, queryParams);

            return result;
        }

        public async Task<bool> DeleteAsync(Guid userOpenId, string currency)
        {
            string queryText =
                @"DELETE FROM [dbo].[UserCurrency]
                WHERE [Currency] = @currency AND [UserOpenId] = @userOpenId";
            object queryParameters = new { userOpenId, currency };

            await base.DeleteEntity(queryText, queryParameters);

            return true;
        }

        public async Task<bool> CheckIsPrimaryAsync(Guid userOpenId, string currency)
        {
            const string queryText =
                @"IF EXISTS (SELECT TOP 1 1 FROM [dbo].[UserCurrency] WHERE [Currency] = @currency AND [UserOpenId] = @userOpenId AND [IsPrimary] = 1)
                SELECT 1 ELSE SELECT 0";
            object queryParams = new { userOpenId, currency };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }
    }
}
