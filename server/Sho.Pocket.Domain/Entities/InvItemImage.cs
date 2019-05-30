using System;

namespace Sho.Pocket.Domain.Entities
{
    public class InvItemImage : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid InvItemId { get; set; }

        public string FileName { get; set; }

        public byte[] Content { get; set; }
    }
}
