using Sho.Pocket.Application.ExchangeRates.Models;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Application.Common.Comparers
{
    public class ExchangeRateComparer : IEqualityComparer<ExchangeRateModel>
    {
        public bool Equals(ExchangeRateModel x, ExchangeRateModel y)
        {
            return y != null &&
                   x.EffectiveDate == y.EffectiveDate &&
                   x.BaseCurrencyId.Equals(y.BaseCurrencyId) &&
                   x.CounterCurrencyId.Equals(y.CounterCurrencyId);
        }

        public int GetHashCode(ExchangeRateModel obj)
        {
            var hashCode = -810118591;
            hashCode = hashCode * -1521134295 + obj.EffectiveDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(obj.BaseCurrencyId);
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(obj.CounterCurrencyId);
            return hashCode;
        }
    }
}