namespace Sho.Pocket.Core.Features.BankIntegration.Models
{
    public class BankModel
    {
        public BankModel(string name, string apiUri, int syncFreqInSeconds, bool isActive)
        {
            Name = name;
            ApiUri = apiUri;
            SyncFreqInSeconds = syncFreqInSeconds;
            IsActive = isActive;
        }

        public string Name { get; set; }

        public string ApiUri { get; set; }

        public int SyncFreqInSeconds { get; set; }

        public bool IsActive { get; set; }
    }
}
