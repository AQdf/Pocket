namespace Sho.BankIntegration.Monobank.Models
{
    public class MonobankExchangeRate
    {
        public MonobankExchangeRate(string baseCurrency, string counterCurrency, decimal? rateBuy, decimal? rateSell, decimal? rateCross)
        {
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
            RateBuy = rateBuy;
            RateSell = rateSell;
            RateCross = rateCross;
        }

        public string Provider => MonobankDefaultConfig.BANK_NAME;

        public string BaseCurrency { get; }

        public string CounterCurrency { get; }

        public decimal? RateBuy { get; set; }

        public decimal? RateSell { get; set; }

        public decimal? RateCross { get; set; }
    }
}
