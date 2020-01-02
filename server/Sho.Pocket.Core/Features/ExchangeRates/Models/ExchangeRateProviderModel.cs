namespace Sho.Pocket.Core.Features.ExchangeRates.Models
{
    public class ExchangeRateProviderModel
    {
        public ExchangeRateProviderModel(string provider, string baseCurrency, string counterCurrency, decimal value)
        {
            Provider = provider;
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
            Value = value;
        }

        public ExchangeRateProviderModel()
        {
        }

        public string Provider { get; set; }

        public string BaseCurrency { get; set; }

        public string CounterCurrency { get; set; }

        public decimal Value { get; set; }
    }
}
