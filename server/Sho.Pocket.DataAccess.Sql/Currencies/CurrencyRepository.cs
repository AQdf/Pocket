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

        public Currency Add(string name)
        {
            object queryParameters = new
            {
                name
            };

            string queryText = @"
                declare @currencyId uniqueidentifier = newid();
                insert into Currency (Id, Name, Description, IsDefault)
                values (@currencyId, @name, NULL, 1);
                select * from Currency where Id = @currencyId";

            return base.InsertEntity(queryText, queryParameters);
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
