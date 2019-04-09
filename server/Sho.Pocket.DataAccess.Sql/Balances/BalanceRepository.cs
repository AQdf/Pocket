using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Balances
{
    public class BalanceRepository : BaseRepository<Balance>, IBalanceRepository
    {
        private const string SCRIPTS_DIR_NAME = "Balances.Scripts";

        public BalanceRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public List<Balance> GetAll(bool includeRelated = true)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetAllBalances.sql");

            List<Balance> result = includeRelated
                ? GetAllWithRelatedEntities(queryText)
                : base.GetAll(queryText);

            return result;
        }

        public Balance GetById(Guid id)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetBalance.sql");

            object queryParameters = new
            {
                id
            };

            Balance result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.Query<Balance, Asset, ExchangeRate, Balance>(queryText, 
                    (balance, asset, rate) =>
                    {
                        balance.Asset = asset;
                        balance.ExchangeRate = rate;

                        return balance;
                    }, queryParameters).FirstOrDefault();
            }

            return result;
        }

        public Balance Add(Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertBalance.sql");

            object queryParameters = new { assetId, effectiveDate, value, exchangeRateId };

            Balance result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public List<Balance> AddEffectiveBalancesTemplate(DateTime currentEffectiveDate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertBalancesTemplate.sql");

            object queryParameters = new
            {
                effectiveDate = currentEffectiveDate,
            };

            return base.GetAll(queryText, queryParameters);
        }

        public Balance Update(Guid id, decimal value)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdateBalance.sql");

            object queryParameters = new { id, value };

            return base.UpdateEntity(queryText, queryParameters);
        }

        public void Remove(Guid balanceId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "DeleteBalance.sql");

            object queryParameters = new
            {
                id = balanceId
            };

            base.RemoveEntity(queryText, queryParameters);
        }

        public IEnumerable<DateTime> GetEffectiveDates()
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetBalancesEffectiveDates.sql");

            List<DateTime> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.Query<DateTime>(queryText).ToList();
            }

            return result;
        }

        public void ApplyExchangeRate(Guid exchangeRateId, Guid counterCurrencyId, DateTime effectiveDate)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "ApplyExchangeRate.sql");

            object queryParameters = new {
                exchangeRateId,
                currencyId = counterCurrencyId,
                effectiveDate
            };

            base.ExecuteScript(queryText, queryParameters);
        }

        private List<Balance> GetAllWithRelatedEntities(string queryText, string queryParams = null)
        {
            List<Balance> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.Query<Balance, Asset, ExchangeRate, Currency, Currency, Balance>(queryText,
                    (balance, asset, rate, baseCurrency, counterCurrency) =>
                    {
                        balance.Asset = asset;
                        balance.ExchangeRate = rate;

                        if (balance.ExchangeRate != null)
                        {
                            balance.ExchangeRate.BaseCurrency = baseCurrency;
                            balance.ExchangeRate.CounterCurrency = counterCurrency;
                        }

                        return balance;
                    }, queryParams).ToList();
            }

            return result;
        }
    }
}
