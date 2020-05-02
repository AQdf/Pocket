using System;
using System.Collections.Generic;

namespace Sho.Pocket.Domain.ValueObjects
{
    public sealed class Money : ValueObject, IEquatable<Money>
    {
        public decimal Value { get; private set; }

        public string Currency { get; private set; }

        public Money(decimal value, string currency)
        {
            Value = value;
            Currency = currency;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Currency;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Money);
        }

        public bool Equals(Money other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Money left, Money right)
        {
            return EqualOperator(left, right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return NotEqualOperator(left, right);
        }
    }
}
