using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceUpdateModel
    {
        public BalanceUpdateModel()
        {
        }

        public BalanceUpdateModel(Guid assetId, decimal value)
        {
            AssetId = assetId;
            Value = value;
        }

        public Guid AssetId { get; set; }

        public decimal Value { get; set; }
    }
}
