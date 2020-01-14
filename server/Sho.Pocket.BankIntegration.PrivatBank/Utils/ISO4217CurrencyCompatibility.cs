using System.Collections.Generic;

namespace Sho.BankIntegration.Privatbank.Utils
{
    /// <summary>
    /// ISO4217 currency. Reference: <https://en.wikipedia.org/wiki/ISO_4217>
    /// </summary>
    internal static class ISO4217CurrencyCompatibility
    {
        public static string GetActive(string name)
        {
            return _compatibilityNames.ContainsKey(name) ? _compatibilityNames[name] : name;
        }

        private static readonly Dictionary<string, string> _compatibilityNames = new Dictionary<string, string>
        {
            { "PLZ", "PLN" },
            { "RUR", "RUB" }
        };
    }
}
