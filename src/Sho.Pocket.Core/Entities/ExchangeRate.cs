using System;

namespace Sho.Pocket.Core.Entities
{
    public class ExchangeRate
    {
        public Guid Id { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal USDtoUAH { get; set; }

        public decimal EURtoUAH { get; set; }
    }
}
