namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Privatbank exchange rate.
    /// Reference: <https://api.privatbank.ua/#p24/exchange>
    /// </summary>
    public class PrivatbankExchangeRate
    {
        public PrivatbankExchangeRate(string baseCurrency, string counterCurrency, decimal? buy, decimal? sell)
        {
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
            Buy = buy;
            Sell = sell;
        }

        /// <summary>
        /// Provider name. Defaults to Privatbank name.
        /// </summary>
        public string Provider => PrivatbankConfig.BANK_NAME;

        /// <summary>
        /// Base currency name according to ISO 4217.
        /// </summary>
        public string BaseCurrency { get; }

        /// <summary>
        /// Counter currency name according to ISO 4217.
        /// </summary>
        public string CounterCurrency { get; }

        /// <summary>
        /// Exchange rate to buy base currency.
        /// </summary>
        public decimal? Buy { get; }

        /// <summary>
        /// Exchange rate to sell base currency.
        /// </summary>
        public decimal? Sell { get; }
    }
}
