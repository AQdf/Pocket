namespace Sho.Pocket.Domain.Entities
{
    public class Currency : BaseEntity
    {
        public Currency() {}

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; }
    }
}