namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceTotalModel
    {
        public BalanceTotalModel(string currency, decimal value)
        {
            Currency = currency;
            Value = value;
        }

        public string Currency { get; set; }

        public decimal Value { get; set; }
    }
}
