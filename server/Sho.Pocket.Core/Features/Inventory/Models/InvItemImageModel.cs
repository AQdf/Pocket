using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Inventory.Models
{
    public class InvItemImageModel
    {
        public InvItemImageModel(InvItemImage itemImage)
        {
            Name = itemImage.FileName;
            Content = itemImage.Content;
        }

        public string Name { get; set; }

        public byte[] Content { get; set; }
    }
}
