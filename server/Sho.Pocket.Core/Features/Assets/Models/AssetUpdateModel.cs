namespace Sho.Pocket.Core.Features.Assets.Models
{
    public class AssetUpdateModel
    {
        public AssetUpdateModel()
        {
        }

        public AssetUpdateModel(string name, string currency, bool isActive, decimal value)
        {
            Name = name;
            Currency = currency;
            IsActive = isActive;
            Value = value;
        }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool IsActive { get; set; }

        public decimal Value { get; set; }
    }
}
