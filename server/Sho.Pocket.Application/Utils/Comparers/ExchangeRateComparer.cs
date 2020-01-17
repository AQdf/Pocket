using System;
using System.Collections.Generic;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.Application.Utils.Comparers
{
    public class ExchangeRateComparer : IEqualityComparer<ExchangeRateModel>
    {
        public bool Equals(ExchangeRateModel x, ExchangeRateModel y)
        {
            return y != null &&
                   x.EffectiveDate == y.EffectiveDate &&
                   string.Equals(x.BaseCurrency, y.BaseCurrency, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(x.CounterCurrency, y.CounterCurrency, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(ExchangeRateModel obj)
        {
            var hashCode = -810118591;
            hashCode = hashCode * -1521134295 + obj.EffectiveDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.BaseCurrency);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.CounterCurrency);
            return hashCode;
        }
    }
}