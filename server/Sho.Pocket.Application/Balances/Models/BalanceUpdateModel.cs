namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceUpdateModel
    {
        public BalanceUpdateModel()
        {
        }

        public BalanceUpdateModel(decimal value)
        {
            Amount = value;
        }

        public decimal Amount { get; set; }
    }
}
