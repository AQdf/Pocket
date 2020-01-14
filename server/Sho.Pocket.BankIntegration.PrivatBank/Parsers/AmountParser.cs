using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Parsers.Abstractions;
using Sho.BankIntegration.Privatbank.Utils;

namespace Sho.BankIntegration.Privatbank.Parsers
{
    internal class AmountParser : PrivatbankXmlResponseParser<PrivatbankAmount>
    {
        /// <summary>
        /// Parses amount string returned by Privatbank API.
        /// Returns null if failed to parse currency or value.
        /// </summary>
        /// <param name="amount">Amount string in format '-0.10 UAH'</param>
        /// <returns></returns>
        public override PrivatbankAmount Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            string[] amountParts = s.Split(' ');

            if (amountParts.Length != 2)
            {
                return null;
            }

            string currency = ISO4217CurrencyCompatibility.GetActive(amountParts[1]);
            decimal? amount = NullableDecimalConverter.Convert(amountParts[0]);

            if (string.IsNullOrWhiteSpace(currency) || !amount.HasValue)
            {
                return null;
            }

            return new PrivatbankAmount(currency, amount.Value);
        }
    }
}
