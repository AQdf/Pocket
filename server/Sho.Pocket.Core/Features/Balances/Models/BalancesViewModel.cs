using System.Collections.Generic;

namespace Sho.Pocket.Core.Features.Balances.Models
{
    public class BalancesViewModel
    {
        public BalancesViewModel(IEnumerable<BalanceViewModel> items, int count, IEnumerable<BalanceTotalModel> totals)
        {
            Items = items;
            Count = count;
            TotalBalance = totals;
        }

        public IEnumerable<BalanceTotalModel> TotalBalance { get; set; }

        public int Count { get; set; }

        public IEnumerable<BalanceViewModel> Items { get; set; }
    }
}
