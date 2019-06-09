namespace Sho.Pocket.Domain.Entities
{
    public class Currency : BaseEntity
    {
        public Currency(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
