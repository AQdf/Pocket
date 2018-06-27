using Dapper;
using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Data.Repositories
{
    public class PeriodSummaryRepository
    {
        private const string SCRIPTS_DIR_NAME = "PeriodSummary";

        public List<PeriodSummary> ReadAll()
        {
            string queryText = DbHelper.GetQueryText(SCRIPTS_DIR_NAME, "ReadAllPeriodSummaries.sql");

            List<PeriodSummary> result = DbHelper.GetAll<PeriodSummary>(queryText);
            
            return result;
        }

        public PeriodSummary AddPeriod(PeriodSummary summary)
        {
            string queryText = DbHelper.GetQueryText(SCRIPTS_DIR_NAME, "InsertPeriodSummary.sql");

            object queryParameters = new
            {
                reportedDate = summary.ReportedDate,
                xRateUSDtoUAH = summary.ExchangeRateUSDtoUAH,
                xRateEURtoUAH = summary.ExhangeRateEURtoUAH
            };

            PeriodSummary result = DbHelper.InsertEntity<PeriodSummary>(queryText, queryParameters);
            
            return result;
        }

        public void DeletePeriod(Guid periodId)
        {
            string queryText = DbHelper.GetQueryText(SCRIPTS_DIR_NAME, "DeletePeriod.sql");

            object queryParameters = new
            {
                id = periodId
            };

            DbHelper.RemoveEntity<PeriodSummary>(queryText, queryParameters);
        }

        public void UpdatePeriodSummaryTotals(Guid periodId)
        {
            string queryText = DbHelper.GetQueryText(SCRIPTS_DIR_NAME, "UpdatePeriodSummaryTotals.sql");

            object queryParameters = new { periodId };

            DbHelper.ExecuteScript(queryText, queryParameters);
        }
    }
}
