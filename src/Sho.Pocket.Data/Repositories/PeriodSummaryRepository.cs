using Dapper;
using Sho.Pocket.Core.Abstractions;
using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Data.Repositories
{
    public class PeriodSummaryRepository : BaseRepository<PeriodSummary>, IPeriodSummaryRepository
    {
        public PeriodSummaryRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        private const string SCRIPTS_DIR_NAME = "PeriodSummary";

        public List<PeriodSummary> ReadAll()
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "ReadAllPeriodSummaries.sql");

            List<PeriodSummary> result = base.GetAll(queryText);
            
            return result;
        }

        public PeriodSummary GetPeriod(Guid id)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetPeriodSummary,sql");

            object queryParameters = new { id };

            PeriodSummary result = base.GetEntity(queryText, queryParameters);

            return result;
        }

        public PeriodSummary AddPeriod(PeriodSummary summary)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertPeriodSummary.sql");

            object queryParameters = new
            {
                reportedDate = summary.ReportedDate,
                xRateUSDtoUAH = summary.ExchangeRateUSDtoUAH,
                xRateEURtoUAH = summary.ExhangeRateEURtoUAH
            };

            PeriodSummary result = base.InsertEntity(queryText, queryParameters);
            
            return result;
        }

        public void DeletePeriod(Guid periodId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "DeletePeriod.sql");

            object queryParameters = new
            {
                id = periodId
            };

            base.RemoveEntity(queryText, queryParameters);
        }

        public void UpdatePeriodSummaryTotals(Guid periodId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdatePeriodSummaryTotals.sql");

            object queryParameters = new { periodId };

            base.ExecuteScript(queryText, queryParameters);
        }
    }
}
