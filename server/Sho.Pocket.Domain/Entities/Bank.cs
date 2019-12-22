namespace Sho.Pocket.Domain.Entities
{
    public class Bank : BaseEntity
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public bool Active { get; set; }

        public string ApiUrl { get; set; }

        public int SyncFreqInSeconds { get; set; }
    }
}
