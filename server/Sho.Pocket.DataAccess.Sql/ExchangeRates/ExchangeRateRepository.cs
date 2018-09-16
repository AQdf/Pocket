using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.DataAccess.Sql.ExchangeRates
{
    public class ExchangeRateRepository : BaseRepository<ExchangeRate>, IExchangeRateRepository
    {
        private const string SCRIPTS_DIR_NAME = "ExchangeRates.Scripts";

        // TODO: Remove hard-coded default currency
        private const string DEFAULT_CURRENCY_NAME = "UAH";

        public ExchangeRateRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public ExchangeRate Alter(DateTime effectiveDate, Guid assetId, decimal rate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "AlterExchangeRate.sql");

            object queryParameters = new
            {
                effectiveDate,
                assetId,
                counterCurrencyName = DEFAULT_CURRENCY_NAME,
                rate
            };

            ExchangeRate result = base.InsertEntity(queryText, queryParameters);

            return result;
        }
    }
}
