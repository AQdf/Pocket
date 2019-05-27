using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.DataAccess.Sql.Currencies
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        private const string SCRIPTS_DIR_NAME = "Currencies.Scripts";

        public CurrencyRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<Currency> CreateAsync(string name)
        {
            string queryText = @"
                if not exists (select top 1 1 from Currency where Name = @name)
                begin
                    insert into Currency (Name) values (@name);
                end
                select * from Currency where Name = @name";
            object queryParams = new { name };

            Currency result = await base.InsertEntity(queryText, queryParams);

            return result;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            string queryText = @"
                if exists (select top 1 1 from Currency where Name = @name)
                select 1 else select 0";
            object queryParams = new { name };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetAllCurrencies.sql");

            IEnumerable<Currency> result = await base.GetEntities(queryText);

            return result;
        }
    }
}
