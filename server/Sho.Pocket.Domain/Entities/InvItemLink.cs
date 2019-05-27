using System;

namespace Sho.Pocket.Domain.Entities
{
    public class InvItemLink : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public Guid InvItemId { get; set; }
    }
}
