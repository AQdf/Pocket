using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        private const string SCRIPTS_DIR_NAME = "Scripts";

        public CurrencyRepository(IOptionsMonitor<DbSettings> options) : base(options)
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
