using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;

namespace Sho.Pocket.DataAccess.Sql.Currencies
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        private const string SCRIPTS_DIR_NAME = "Currencies.Scripts";

        public CurrencyRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public Currency GetByName(string name)
        {
            string queryText = $"select * from Currency where Name = {name}";

            return base.GetEntity(queryText);
        }

        public List<Currency> GetAll()
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetAllCurrencies.sql");

            return base.GetAll(queryText);
        }
    }
}
