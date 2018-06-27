using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.Entities
{
    public class PeriodSummary : BaseEntity
    {
        public PeriodSummary()
        {

        }

        public PeriodSummary(DateTime reportedDate, decimal rateUSDtoUAH, decimal rateEURtoUAH)
        {
            ReportedDate = reportedDate;
            ExchangeRateUSDtoUAH = rateUSDtoUAH;
            ExhangeRateEURtoUAH = rateEURtoUAH;
        }

        public List<Asset> Assets { get; set; }

        public DateTime ReportedDate { get; set; }
        
        public decimal ExchangeRateUSDtoUAH { get; set; }

        public decimal ExhangeRateEURtoUAH { get; set; }

        public decimal TotalBalanceUAH { get; set; }

        public decimal TotalBalanceUSD { get; set; }

        public decimal TotalBalanceEUR { get; set; }

        #region Object overrides

        public override bool Equals(object obj)
        {
            return this.ToString() == obj.ToString();
        }

        public override string ToString()
        {
            return $"{ReportedDate:dd MMM yyyy}: {TotalBalanceUAH} UAH, {TotalBalanceUSD} USD, {TotalBalanceEUR} EUR";
        }
        
        #endregion Object overrides
    }
}
