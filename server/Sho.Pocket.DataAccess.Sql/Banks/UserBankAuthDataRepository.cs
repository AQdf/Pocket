using System;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Banks
{
    public class UserBankAuthDataRepository : BaseRepository<UserBankAuthData>, IUserBankAuthDataRepository
    {
        public UserBankAuthDataRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<UserBankAuthData> AlterAsync(Guid userId, string bankName, string token, string bankClientId)
        {
            string queryText = @"
                DECLARE @id uniqueidentifier = (
	                SELECT TOP 1 Id FROM [dbo].[UserBankAuthData]
	                WHERE [UserId] = @userId AND [BankName]=@bankName)

                IF @id IS NULL
	                BEGIN
		                SET @id = NEWID();
                        INSERT INTO [dbo].[UserBankAuthData] ([Id], [UserId], [BankName], [Token], [BankClientId])
                        VALUES (@id, @userId, @bankName, @token, @bankClientId)
	                END
                ELSE
	                BEGIN
		                UPDATE [dbo].[UserBankAuthData]
		                SET [BankName] = @bankName, [Token] = @token, [BankClientId] = @bankClientId
		                WHERE [Id] = @id
	                END

                SELECT [Id], [UserId], [BankName], [Token]
                FROM [dbo].[UserBankAuthData] WHERE [Id] = @id";

            object queryParameters = new
            {
                userId,
                bankName,
                token,
                bankClientId
            };

            UserBankAuthData result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<UserBankAuthData> GetAsync(Guid userId, Guid id)
        {
            string queryText = @"
                SELECT * FROM [dbo].[UserBankAuthData]
                WHERE [Id] = @id AND [UserId] = @userId";

            object queryParameters = new { userId, id };

            UserBankAuthData result = await base.GetEntity(queryText, queryParameters);

            return result;
        }
    }
}
