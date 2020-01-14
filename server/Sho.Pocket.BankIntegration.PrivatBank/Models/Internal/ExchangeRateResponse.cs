namespace Sho.BankIntegration.Privatbank.Models.Internal
{
    /// <summary>
    /// Privatbank exchange rate.
    /// Reference: <https://api.privatbank.ua/#p24/exchange>
    /// </summary>
    internal class ExchangeRateResponse
    {
        /// <summary>
        /// Base currency name according to <https://ru.wikipedia.org/wiki/Коды_валют>.
        /// </summary>
        public string Ccy { get; set; }

        /// <summary>
        /// Counter currency name according to <https://ru.wikipedia.org/wiki/Коды_валют>.
        /// </summary>
        public string Base_ccy { get; set; }

        /// <summary>
        /// Exchange rate to buy base currency.
        /// </summary>
        public decimal? Buy { get; set; }

        /// <summary>
        /// Exchange rate to sell base currency.
        /// </summary>
        public decimal? Sale { get; set; }
    }
}