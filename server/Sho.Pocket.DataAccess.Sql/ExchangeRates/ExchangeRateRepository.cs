using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.DataAccess.Sql.ExchangeRates
{
    public class ExchangeRateRepository : BaseRepository<ExchangeRate>, IExchangeRateRepository
    {
        private const string SCRIPTS_DIR_NAME = "ExchangeRates.Scripts";

        public ExchangeRateRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public ExchangeRate Alter(DateTime effectiveDate, Guid baseCurrencyId, Guid counterCurrencyId, decimal rate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "AlterExchangeRate.sql");

            object queryParameters = new
            {
                effectiveDate,
                baseCurrencyId,
                counterCurrencyId,
                rate
            };

            ExchangeRate result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public ExchangeRate Update(Guid id, decimal rate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdateExchangeRate.sql");

            object queryParameters = new { id, rate };

            return base.UpdateEntity(queryText, queryParameters);
        }

        public ExchangeRate GetCurrencyExchangeRate(Guid baseCurrencyId, DateTime effectiveDate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetCurrencyExchangeRate.sql");

            object queryParameters = new { baseCurrencyId, effectiveDate };

            ExchangeRate result = base.GetEntity(queryText, queryParameters);

            return result;
        }

        public List<ExchangeRate> GetByEffectiveDate(DateTime effectiveDate)
        {
            string queryText = @"
                select	ExchangeRate.Id,
		                ExchangeRate.EffectiveDate,
		                ExchangeRate.BaseCurrencyId,
		                ExchangeRate.CounterCurrencyId,
                        ExchangeRate.Rate,
		                baseCurrency.[Name] as BaseCurrencyName,
		                counterCurrency.[Name] as CounterCurrencyName
                from ExchangeRate
                join Currency baseCurrency on ExchangeRate.BaseCurrencyId = baseCurrency.Id
                join Currency counterCurrency on ExchangeRate.CounterCurrencyId = counterCurrency.Id
                where EffectiveDate = @effectiveDate";

            object queryParameters = new { effectiveDate };

            List<ExchangeRate> result = base.GetAll(queryText, queryParameters);

            return result;
        }
    }
}
