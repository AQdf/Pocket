namespace Sho.Pocket.Application.Assets.Models
{
    public class AssetUpdateModel
    {
        public AssetUpdateModel()
        {
        }

        public AssetUpdateModel(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
        }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
