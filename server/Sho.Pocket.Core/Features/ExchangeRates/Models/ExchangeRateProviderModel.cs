namespace Sho.Pocket.Core.Features.ExchangeRates.Models
{
    public class ExchangeRateProviderModel
    {
        public ExchangeRateProviderModel(string provider, string baseCurrency, string counterCurrency, decimal buy, decimal sell)
        {
            Provider = provider;
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
            Buy = buy;
            Sell = sell;
        }

        public ExchangeRateProviderModel()
        {
        }

        public string Provider { get; set; }

        public string BaseCurrency { get; set; }

        public string CounterCurrency { get; set; }

        public decimal Buy { get; set; }

        public decimal Sell { get; set; }
    }
}
