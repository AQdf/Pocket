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

        public async Task<Currency> Add(string name)
        {
            string queryText = @"
                declare @currencyId uniqueidentifier = newid();
                insert into Currency (Id, Name, Description, IsDefault)
                values (@currencyId, @name, NULL, 1);
                select * from Currency where Id = @currencyId";

            object queryParameters = new { name };

            Currency result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<Currency> GetByName(string name)
        {
            string queryText = $"select * from Currency where Name = {name}";

            Currency result = await base.GetEntity(queryText);

            return result;
        }

        public async Task<IEnumerable<Currency>> GetAll()
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetAllCurrencies.sql");

            IEnumerable<Currency> result = await base.GetAll(queryText);

            return result;
        }
    }
}
