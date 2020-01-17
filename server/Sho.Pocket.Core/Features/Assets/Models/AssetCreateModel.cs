namespace Sho.Pocket.Core.Features.Assets.Models
{
    public class AssetCreateModel
    {
        public AssetCreateModel()
        {
        }

        public AssetCreateModel(string name, string currency, bool isActive)
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
