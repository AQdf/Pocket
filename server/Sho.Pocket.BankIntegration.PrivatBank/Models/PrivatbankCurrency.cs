using System.Collections.Generic;

namespace Sho.BankIntegration.Privatbank.Models
{
    internal class PrivatbankCurrency
    {
        private PrivatbankCurrency(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static PrivatbankCurrency GetCompatible(string name)
        {
            string compatibleName = _compatibilityNames.ContainsKey(name) ? _compatibilityNames[name] : name;

            return new PrivatbankCurrency(compatibleName);
        }

        // TODO: Move to configuration settings
        private static readonly Dictionary<string, string> _compatibilityNames = new Dictionary<string, string>
        {
            { "PLZ", "PLN" },
            { "RUR", "RUB" }
        };
    }
}
