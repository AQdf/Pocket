namespace Sho.Pocket.Application.Configuration.Models
{
    public class BankSettings
    {
        public string Name { get; set; }

        public string ApiUri { get; set; }

        public int SyncFreqInSeconds { get; set; }

        public bool IsActive { get; set; }
    }
}
