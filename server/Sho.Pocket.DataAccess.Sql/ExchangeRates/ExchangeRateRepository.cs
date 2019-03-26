using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Constants;
using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.DataAccess.Sql.ExchangeRates
{
    public class ExchangeRateRepository : BaseRepository<ExchangeRate>, IExchangeRateRepository
    {
        private const string SCRIPTS_DIR_NAME = "ExchangeRates.Scripts";

        public ExchangeRateRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public ExchangeRate Add(DateTime effectiveDate, Guid baseCurrencyId, Guid counterCurrencyId, decimal rate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertExchangeRate.sql");

            object queryParameters = new { effectiveDate, baseCurrencyId, counterCurrencyId, rate };

            ExchangeRate result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public ExchangeRate Alter(DateTime effectiveDate, Guid baseCurrencyId, decimal rate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "AlterExchangeRate.sql");

            object queryParameters = new
            {
                effectiveDate,
                baseCurrencyId,
                counterCurrencyName = CurrencyConstants.UAH,
                rate
            };

            ExchangeRate result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public void Update(Guid id, decimal rate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdateExchangeRate.sql");

            object queryParameters = new { id, rate };

            base.UpdateEntity(queryText, queryParameters);
        }

        public ExchangeRate GetCurrencyExchangeRate(Guid baseCurrencyId, DateTime effectiveDate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetCurrencyExchangeRate.sql");

            object queryParameters = new { baseCurrencyId, effectiveDate };

            ExchangeRate result = base.GetEntity(queryText, queryParameters);

            return result;
        }
    }
}
