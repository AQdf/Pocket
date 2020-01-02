namespace Sho.Pocket.ExchangeRates.Configuration.Models
{
    public class ExchangeRateProviderOption
    {
        public string Name { get; set; }

        public string ApiKey { get; set; }

        public string Uri { get; set; }

        public bool IsActive { get; set; }

        public int Priority { get; set; }
    }
}
