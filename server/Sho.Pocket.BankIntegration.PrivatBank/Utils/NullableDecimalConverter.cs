namespace Sho.BankIntegration.Privatbank.Utils
{
    internal static class NullableDecimalConverter
    {
        /// <summary>
        /// Converts the string representation of a number to its decimal equivalent.
        /// Returns null if conversion failed.
        /// </summary>
        /// <returns></returns>
        public static decimal? Convert(string s)
        {
            decimal? result = null;
            bool parsed = decimal.TryParse(s, out decimal value);

            if (parsed)
            {
                result = value;
            }

            return result;
        }
    }
}
