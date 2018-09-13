using System;

namespace Sho.Pocket.Application.AssetTypes.Models
{
    public class AssetTypeViewModel
    {
        public AssetTypeViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
