using System.Collections.Generic;

namespace Sho.Pocket.BankIntegration.Monobank.Utils
{
    internal class ISO4217CurrencyCodeConverter
    {
        private readonly Dictionary<string, string> _currencyCodes = new Dictionary<string, string>
        {
            { "840", "USD" },
            { "980", "UAH" },
            { "978", "EUR" },
            { "643", "RUB" },
            { "826", "GBP" },
            { "756", "CHF" },
            { "933", "BYN" },
            { "124", "CAD" },
            { "203", "CZK" },
            { "208", "DKK" },
            { "348", "HUF" },
            { "985", "PLN" },
            { "949", "TRY" }
        };

        internal string GetCurrencyName(string code)
        {
            if (!_currencyCodes.ContainsKey(code))
            {
                throw new System.Exception($"Can't find currency name by code {code}");
            }

            return _currencyCodes[code];
        }
    }
}
