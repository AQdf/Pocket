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
        public CurrencyRepository(IOptionsMonitor<DbSettings> options) : base(options)
        {
        }

        public async Task<Currency> CreateAsync(string name)
        {
            string queryText = @"
                IF NOT EXISTS (SELECT TOP 1 1 FROM [Currency] WHERE [Name] = @name)
                BEGIN
                    INSERT INTO [Currency] (Name) VALUES (@name);
                END
                SELECT * FROM [Currency] WHERE [Name] = @name";

            object queryParams = new { name };

            Currency result = await base.InsertEntity(queryText, queryParams);

            return result;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            string queryText = @"
                IF EXISTS (SELECT TOP 1 1 FROM [Currency] WHERE [Name] = @name)
                SELECT 1 ELSE SELECT 0";

            object queryParams = new { name };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            string queryText = @"SELECT * FROM [Currency] ORDER BY [Name] asc";

            IEnumerable<Currency> result = await base.GetEntities(queryText);

            return result;
        }
    }
}
