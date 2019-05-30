namespace Sho.Pocket.Application.ExchangeRates.Models
{
    internal class NBUExchangeRateModel
    {
        public NBUExchangeRateModel(string currency, string rate, string exchangeRate)
        {
            Currency = currency;
            Rate = decimal.Parse(rate);
            ExchangeDate = exchangeRate;
        }

        public string Currency { get; set; }

        public decimal Rate { get; set; }

        public string ExchangeDate { get; set; }
    }
}
