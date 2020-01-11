using System.Collections.Generic;

namespace Sho.BankIntegration.Monobank.Models
{
    /// <summary>
    /// ISO4217 currency. Reference: <https://en.wikipedia.org/wiki/ISO_4217>
    /// </summary>
    public class MonobankCurrency
    {
        public MonobankCurrency(int code)
        {
            Code = code;

            bool valueExists = _currencyCodes.TryGetValue(code, out string name);
            Name = valueExists ? name : null;
        }

        /// <summary>
        /// ISO4217 currency number code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// ISO4217 currency name.
        /// </summary>
        public string Name { get; set; }

        private static readonly Dictionary<int, string> _currencyCodes = new Dictionary<int, string>
        {
            { 840, "USD" },
            { 980, "UAH" },
            { 978, "EUR" },
            { 643, "RUB" },
            { 826, "GBP" },
            { 756, "CHF" },
            { 933, "BYN" },
            { 124, "CAD" },
            { 203, "CZK" },
            { 208, "DKK" },
            { 348, "HUF" },
            { 985, "PLN" },
            { 949, "TRY" }
        };
    }
}
