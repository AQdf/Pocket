namespace Sho.BankIntegration.Monobank.Models
{
    /// <summary>
    /// Example: <https://api.monobank.ua/docs/#/definitions/CurrencyInfo>
    /// </summary>
    internal class MonobankCurrencyInfo
    {
        /// <summary>
        /// Currency code of base currency according to ISO 4217 (int32)
        /// </summary>
        public int CurrencyCodeA { get; set; }

        /// <summary>
        /// Currency code of counter currency according to ISO 4217 (int32)
        /// </summary>
        public int CurrencyCodeB { get; set; }

        /// <summary>
        /// Effective time of exchange rate in seconds in Unix time format (int64)
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Exchange rate to buy base currency (float)
        /// </summary>
        public string RateBuy { get; set; }

        /// <summary>
        /// Exchange rate to sell base currency (float)
        /// </summary>
        public string RateSell { get; set; }

        /// <summary>
        /// Cross exchange rate (float)
        /// </summary>
        public string RateCross { get; set; }
    }
}
