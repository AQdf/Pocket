using System;

namespace Sho.BankIntegration.Monobank.Utils
{
    internal static class DateTimeExtensions
    {
        public static long ToUnixTime(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        public static DateTime FromUnixTime(this long time)
        {
            return DateTimeOffset.FromUnixTimeSeconds(time).UtcDateTime;
        }
    }
}
