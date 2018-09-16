using System.Collections.Generic;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalancesViewModel
    {
        public BalancesViewModel(List<BalanceViewModel> items, int count, decimal totalBalance)
        {
            Items = items;
            Count = count;
            TotalBalance = totalBalance;
        }

        public decimal TotalBalance { get; set; }

        public int Count { get; set; }

        public List<BalanceViewModel> Items { get; set; }
    }
}
