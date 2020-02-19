using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper.Repositories
{
    public class ExchangeRateRepository : BaseRepository<ExchangeRate>, IExchangeRateRepository
    {
        public ExchangeRateRepository(IOptionsMonitor<DbSettings> options) : base(options)
        {
        }

        public async Task<ExchangeRate> AlterAsync(DateTime effectiveDate, string baseCurrency, string counterCurrency, decimal buy, decimal sell, string provider)
        {
            string queryText = @"
                DECLARE @id uniqueidentifier = (
                    SELECT TOP 1 Id FROM ExchangeRate
                    WHERE BaseCurrency = @baseCurrency
                    AND CounterCurrency = @counterCurrency
                    AND EffectiveDate = @effectiveDate)

                IF @id IS NULL
                    BEGIN
                        SET @id = NEWID();
                        INSERT INTO ExchangeRate([Id], [EffectiveDate], [BaseCurrency], [CounterCurrency], [BuyRate], [SellRate], [Provider]) VALUES (
                        @id, @effectiveDate, @baseCurrency, @counterCurrency, @buy, @sell, @provider)
                    END
                ELSE
                    BEGIN
                        UPDATE ExchangeRate SET BuyRate = @buy, SellRate = @sell WHERE Id = @id
                    END

                SELECT * FROM ExchangeRate where Id = @id";

            object queryParameters = new
            {
                effectiveDate,
                baseCurrency,
                counterCurrency,
                buy,
                sell,
                provider
            };

            ExchangeRate result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<ExchangeRate> UpdateAsync(Guid id, decimal buy, decimal sell)
        {
            string queryText = @"
                UPDATE ExchangeRate
                SET BuyRate = @buy, SellRate = @sell
                WHERE Id = @id

                SELECT * FROM ExchangeRate WHERE Id = @id";

            object queryParameters = new { id, buy, sell };

            ExchangeRate result = await base.UpdateEntity(queryText, queryParameters);

            return result;
        }

        public async Task<ExchangeRate> GetCurrencyExchangeRateAsync(string baseCurrency, DateTime effectiveDate)
        {
            string queryText = @"SELECT * FROM ExchangeRate WHERE BaseCurrency = @baseCurrency AND EffectiveDate = @effectiveDate";
            object queryParameters = new { baseCurrency, effectiveDate };

            ExchangeRate result = await base.GetEntity(queryText, queryParameters);

            return result;
        }

        public async Task<bool> Exists(string baseCurrency, DateTime effectiveDate)
        {
            string queryText = @"
                IF EXISTS ( SELECT TOP 1 1 FROM ExchangeRate
                            WHERE BaseCurrency = @baseCurrency
                            AND EffectiveDate = @effectiveDate )
                SELECT 1 ELSE SELECT 0";

            object queryParams = new { baseCurrency, effectiveDate };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<ExchangeRate>> GetByEffectiveDateAsync(DateTime effectiveDate)
        {
            string queryText = @"SELECT * FROM ExchangeRate WHERE EffectiveDate = @effectiveDate ORDER BY BaseCurrency";
            object queryParameters = new { effectiveDate };

            IEnumerable<ExchangeRate> result = await base.GetEntities(queryText, queryParameters);

            return result;
        }
    }
}
