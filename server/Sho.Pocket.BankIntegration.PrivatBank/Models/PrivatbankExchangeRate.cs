namespace Sho.BankIntegration.Privatbank.Models
{
    public class PrivatbankExchangeRate
    {
        public PrivatbankExchangeRate(string baseCurrency, string counterCurrency, decimal? buy, decimal? sell)
        {
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
            Buy = buy;
            Sell = sell;
        }

        public string Provider => PrivatbankDefaultConfig.BANK_NAME;

        public string BaseCurrency { get; }

        public string CounterCurrency { get; }

        public decimal? Buy { get; }

        public decimal? Sell { get; }
    }
}
