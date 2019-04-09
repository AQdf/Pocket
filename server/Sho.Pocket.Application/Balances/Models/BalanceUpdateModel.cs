namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceUpdateModel
    {
        public BalanceUpdateModel()
        {
        }

        public BalanceUpdateModel(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; set; }
    }
}
