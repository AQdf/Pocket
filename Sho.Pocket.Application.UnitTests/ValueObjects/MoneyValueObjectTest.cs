using Sho.Pocket.Domain.ValueObjects;
using System.Collections.Generic;
using Xunit;

namespace Sho.Pocket.Application.UnitTests.ValueObjects
{
    public class MoneyValueObjectTest
    {
        [Fact]
        public void MoneyValueObjects_HaveSameAttributes_ShouldBeEqual()
        {
            Money money1 = new Money(10.0M, "USD");
            Money money2 = new Money(10.0M, "USD");

            Assert.True(money1.Equals(money2));
        }

        [Theory]
        [InlineData(10, 10.5, "USD", "USD")]
        [InlineData(10, 10, "USD", "EUR")]
        [InlineData(10.5, 10, "EUR", "USD")]
        public void MoneyValueObjects_HaveDifferentAttributes_ShouldNotBeEqual(decimal value1, decimal value2, string currency1, string currency2)
        {
            Money money1 = new Money(value1, currency1);
            Money money2 = new Money(value2, currency2);

            Assert.False(money1.Equals(money2));
        }

        [Fact]
        public void MoneyValueObjects_HaveSameAttributes_UsingEqualOperator_ShouldBeEqual()
        {
            Money money1 = new Money(10.0M, "USD");
            Money money2 = new Money(10.0M, "USD");

            Assert.True(money1 == money2);
        }

        [Fact]
        public void MoneyValueObjects_HaveDifferentAttributes_UsingNotEqualOperator_ShouldNotBeEqual()
        {
            Money money1 = new Money(10.0M, "USD");
            Money money2 = new Money(10.5M, "USD");

            Assert.True(money1 != money2);
        }

        [Fact]
        public void MoneyValueObjects_HaveSameAttributes_HashesShouldBeEqual()
        {
            Money money1 = new Money(10.0M, "USD");
            Money money2 = new Money(10.0M, "USD");

            Assert.True(money1.GetHashCode() == money2.GetHashCode());
        }

        [Fact]
        public void MoneyValueObjects_HaveDifferentAttributes_HashesShouldNotBeEqual()
        {
            Money money1 = new Money(10.0M, "USD");
            Money money2 = new Money(10.5M, "USD");

            Assert.False(money1.GetHashCode() == money2.GetHashCode());
        }

        [Fact]
        public void MoneyValueObjects_AddSameMoneyValueObjectToHashset_ShouldNotBeAdded()
        {
            HashSet<Money> hashSet = new HashSet<Money>
            {
                new Money(10.0M, "USD")
            };

            bool addResult = hashSet.Add(new Money(10.0M, "USD"));

            Assert.False(addResult);
            Assert.Single(hashSet);
        }

        [Fact]
        public void MoneyValueObjects_AreNulls_ShouldBeEqual()
        {
            Money money1 = null;
            Money money2 = null;

            Assert.True(money1 == money2);
        }

        [Fact]
        public void MoneyValueObjects_OneObjectIsNull_ShouldNotBeEqual()
        {
            Money money1 = new Money(10.0M, "USD");
            Money money2 = null;

            Assert.False(money1 == money2);
        }
    }
}
