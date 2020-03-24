namespace Sho.Pocket.Core.Features.BankIntegration.Models
{
    public class BankSettings
    {
        public string Name { get; set; }

        public string ApiUri { get; set; }

        public int SyncFreqInSeconds { get; set; }

        public bool IsActive { get; set; }
    }
}
