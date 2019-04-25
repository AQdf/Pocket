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

        public async Task<ExchangeRate> Alter(DateTime effectiveDate, Guid baseCurrencyId, Guid counterCurrencyId, decimal rate)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "AlterExchangeRate.sql");

            object queryParameters = new
            {
                effectiveDate,
                baseCurrencyId,
                counterCurrencyId,
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

        public async Task<ExchangeRate> GetCurrencyExchangeRate(Guid baseCurrencyId, DateTime effectiveDate)
        {
            string queryText = await GetQueryText(SCRIPTS_DIR_NAME, "GetCurrencyExchangeRate.sql");

            object queryParameters = new { baseCurrencyId, effectiveDate };

            ExchangeRate result = await base.GetEntity(queryText, queryParameters);

            return result;
        }

        public async Task<bool> Exists(Guid baseCurrencyId, DateTime effectiveDate)
        {
            string queryText = @"
                if exists ( select top 1 1 from ExchangeRate
                            where BaseCurrencyId = @baseCurrencyId
                            and EffectiveDate = @effectiveDate )
                select 1 else select 0";

            object queryParams = new { baseCurrencyId, effectiveDate };

            bool result = await base.Exists(queryText, queryParams);

            return result;
        }

        public async Task<IEnumerable<ExchangeRate>> GetByEffectiveDate(DateTime effectiveDate)
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
                where EffectiveDate = @effectiveDate
                order by BaseCurrencyName";

            object queryParameters = new { effectiveDate };

            IEnumerable<ExchangeRate> result = await base.GetAll(queryText, queryParameters);

            return result;
        }
    }
}
