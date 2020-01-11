using System;

namespace Sho.BankIntegration.Monobank.Models
{
    /// <summary>
    /// Monobank exchange rate data.
    /// </summary>
    public class MonobankExchangeRate
    {
        public MonobankExchangeRate(
            int baseCurrencyCode,
            int counterCurrencyCode,
            DateTime date,
            decimal? rateBuy,
            decimal? rateSell,
            decimal? rateCross)
        {
            BaseCurrency = new MonobankCurrency(baseCurrencyCode);
            CounterCurrency = new MonobankCurrency(counterCurrencyCode);
            Date = date;
            RateBuy = rateBuy;
            RateSell = rateSell;
            RateCross = rateCross;
        }

        /// <summary>
        /// Exchange rate provider name. Defaults to Monobank.
        /// </summary>
        public string Provider => MonobankConfig.BANK_NAME;

        /// <summary>
        /// Exchange rate base currency according to ISO4217.
        /// </summary>
        public MonobankCurrency BaseCurrency { get; }

        /// <summary>
        /// Exchange rate counter currency according to ISO4217.
        /// </summary>
        public MonobankCurrency CounterCurrency { get; }

        /// <summary>
        /// Effective date of the exchange rate.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Exchange rate to buy base currency.
        /// Could be null.
        /// </summary>
        public decimal? RateBuy { get; set; }

        /// <summary>
        /// Exchange rate to sell base currency.
        /// Could be null.
        /// </summary>
        public decimal? RateSell { get; set; }

        /// <summary>
        /// Cross exchange rate.
        /// Could be null.
        /// </summary>
        public decimal? RateCross { get; set; }
    }
}
