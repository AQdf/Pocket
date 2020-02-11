using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class BankRepository : BaseRepository<Bank>, IBankRepository
    {
        public BankRepository(IOptionsMonitor<DbSettings> options) : base(options)
        {
        }

        public async Task<IEnumerable<Bank>> GetSupportedBanksAsync()
        {
            string query = @"
                SELECT * FROM [dbo].[Bank]
                WHERE [Bank].[Active] = 1
                ORDER BY [Bank].[Name]";

            IEnumerable<Bank> result = await base.GetEntities(query);

            return result;
        }

        public async Task<Bank> GetBankAsync(string name)
        {
            const string queryText = @"SELECT * FROM [dbo].[Bank] WHERE [Name] = @name";
            object queryParams = new { name };
            Bank result = await base.GetEntity(queryText, queryParams);

            return result;
        }
    }
}
