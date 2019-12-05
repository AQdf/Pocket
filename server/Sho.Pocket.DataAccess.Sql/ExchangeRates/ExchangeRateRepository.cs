using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.DataAccess.Sql.ExchangeRates
{
    public class ExchangeRateRepository : BaseRepository<ExchangeRate>, IExchangeRateRepository
    {
        private const string SCRIPTS_DIR_NAME = "ExchangeRates.Scripts";

        public ExchangeRateRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public async Task<ExchangeRate> AlterAsync(DateTime effectiveDate, string baseCurrency, string counterCurrency, decimal rate)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "AlterExchangeRate.sql");

            object queryParameters = new
            {
                effectiveDate,
                baseCurrency,
                counterCurrency,
                rate
            };

            ExchangeRate result = await base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public async Task<ExchangeRate> Update(Guid id, decimal rate)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "UpdateExchangeRate.sql");

            object queryParameters = new { id, rate };

            ExchangeRate result = await base.UpdateEntity(queryText, queryParameters);

            return result;
        }

        public async Task<ExchangeRate> GetCurrencyExchangeRate(string baseCurrency, DateTime effectiveDate)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetCurrencyExchangeRate.sql");

            object queryParameters = new { baseCurrency, effectiveDate };

            ExchangeRate result = await base.GetEntity(queryText, queryParameters);

            return result;
        }

        public async Task<bool> Exists(string baseCurrency, DateTime effectiveDate)
        {
            string queryText = @"
                if exists ( select top 1 1 from ExchangeRate
                            where BaseCurrency = @baseCurrency
                            and EffectiveDate = @effectiveDate )
                select 1 else select 0";

            object queryParams = new { baseCurrency, effectiveDate };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<ExchangeRate>> GetByEffectiveDateAsync(DateTime effectiveDate)
        {
            string queryText = @"
                select	ExchangeRate.Id,
		                ExchangeRate.EffectiveDate,
		                ExchangeRate.BaseCurrency,
		                ExchangeRate.CounterCurrency,
                        ExchangeRate.Rate
                from ExchangeRate
                where EffectiveDate = @effectiveDate
                order by BaseCurrency";

            object queryParameters = new { effectiveDate };

            IEnumerable<ExchangeRate> result = await base.GetEntities(queryText, queryParameters);

            return result;
        }
    }
}
