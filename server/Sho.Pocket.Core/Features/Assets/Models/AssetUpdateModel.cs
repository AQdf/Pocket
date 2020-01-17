namespace Sho.Pocket.Core.Features.Assets.Models
{
    public class AssetUpdateModel
    {
        public AssetUpdateModel()
        {
        }

        public AssetUpdateModel(string name, string currency, bool isActive)
        {
            Name = name;
            Currency = currency;
            IsActive = isActive;
        }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool IsActive { get; set; }
    }
}
