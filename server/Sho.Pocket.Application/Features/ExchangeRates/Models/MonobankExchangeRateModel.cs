namespace Sho.Pocket.Application.Features.ExchangeRates.Models
{
    public class MonobankExchangeRateModel
    {
        public string CurrencyCodeA { get; set; }

        public string CurrencyCodeB { get; set; }

        public string Date { get; set; }

        public string RateBuy { get; set; }

        public string RateSell { get; set; }

        public string RateCross { get; set; }
    }
}
