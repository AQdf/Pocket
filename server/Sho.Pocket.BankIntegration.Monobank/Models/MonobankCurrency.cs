using System.Collections.Generic;

namespace Sho.BankIntegration.Monobank.Models
{
    /// <summary>
    /// ISO4217 currency. Reference: <https://en.wikipedia.org/wiki/ISO_4217>
    /// </summary>
    internal class MonobankCurrency
    {
        private MonobankCurrency(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public int Code { get; set; }

        public string Name { get; set; }

        public static bool TryParse(int code, out MonobankCurrency result)
        {
            bool isSuccess = _currencyCodes.TryGetValue(code, out string name);
            result = new MonobankCurrency(code, name);

            return isSuccess;
        }

        // TODO: Move to configuration settings
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
