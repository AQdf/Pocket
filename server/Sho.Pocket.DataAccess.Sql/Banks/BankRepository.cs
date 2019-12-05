using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Banks
{
    public class BankRepository : BaseRepository<Bank>, IBankRepository
    {
        public BankRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
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
    }
}
