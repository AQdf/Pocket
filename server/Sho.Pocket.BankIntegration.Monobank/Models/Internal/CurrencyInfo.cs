namespace Sho.BankIntegration.Monobank.Models.Internal
{
    /// <summary>
    /// Example: <https://api.monobank.ua/docs/#/definitions/CurrencyInfo>
    /// </summary>
    internal class CurrencyInfo
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
        public long Date { get; set; }

        /// <summary>
        /// Exchange rate to buy base currency (float)
        /// Could be missing in response.
        /// </summary>
        public decimal? RateBuy { get; set; }

        /// <summary>
        /// Exchange rate to sell base currency (float)
        /// Could be missing in response.
        /// </summary>
        public decimal? RateSell { get; set; }

        /// <summary>
        /// Cross exchange rate (float)
        /// Could be missing in response.
        /// </summary>
        public decimal? RateCross { get; set; }
    }
}
