using System;

namespace Sho.Pocket.Application.UserCurrencies.Models
{
    public class UserCurrencyModel
    {
        public UserCurrencyModel(string currency, bool isPrimary)
        {
            Currency = currency;
            IsPrimary = isPrimary;
        }

        public string Currency { get; set; }

        public bool IsPrimary { get; set; }
    }
}
