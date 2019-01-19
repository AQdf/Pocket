using Sho.Pocket.Application.ExchangeRates.Models;
using System.Collections.Generic;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalancesViewModel
    {
        public BalancesViewModel(List<BalanceViewModel> items, int count, IEnumerable<BalanceTotalModel> totals, List<ExchangeRateModel> rates)
        {
            Items = items;
            Count = count;
            TotalBalance = totals;
            ExchangeRates = rates;
        }

        public IEnumerable<BalanceTotalModel> TotalBalance { get; set; }

        public int Count { get; set; }

        public List<ExchangeRateModel> ExchangeRates { get; set; }

        public List<BalanceViewModel> Items { get; set; }
    }
}
