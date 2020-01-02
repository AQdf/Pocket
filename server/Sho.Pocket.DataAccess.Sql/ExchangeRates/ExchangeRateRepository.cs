using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.DataAccess.Sql.ExchangeRates
{
    public class ExchangeRateRepository : BaseRepository<ExchangeRate>, IExchangeRateRepository
    {
        public ExchangeRateRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<ExchangeRate> AlterAsync(DateTime effectiveDate, string baseCurrency, string counterCurrency, decimal rate, string provider)
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
                        INSERT INTO ExchangeRate([Id], [EffectiveDate], [BaseCurrency], [CounterCurrency], [Rate], [Provider]) VALUES (
                        @id, @effectiveDate, @baseCurrency, @counterCurrency, @rate, @provider)
                    END
                ELSE
                    BEGIN
                        UPDATE ExchangeRate SET Rate = @rate WHERE Id = @id
                    END

                SELECT * FROM ExchangeRate where Id = @id";

            object queryParameters = new
            {
                effectiveDate,
                baseCurrency,
                counterCurrency,
                rate,
                @provider
            };

            ExchangeRate result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<ExchangeRate> Update(Guid id, decimal rate)
        {
            string queryText = @"
                UPDATE ExchangeRate
                SET Rate = @rate
                WHERE Id = @id

                SELECT * FROM ExchangeRate WHERE Id = @id";

            object queryParameters = new { id, rate };

            ExchangeRate result = await base.UpdateEntity(queryText, queryParameters);

            return result;
        }

        public async Task<ExchangeRate> GetCurrencyExchangeRate(string baseCurrency, DateTime effectiveDate)
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
