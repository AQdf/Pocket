using Sho.Pocket.Application.ExchangeRates.Models;
using System.Collections.Generic;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalancesViewModel
    {
        public BalancesViewModel(IEnumerable<BalanceViewModel> items, int count, IEnumerable<BalanceTotalModel> totals, IEnumerable<ExchangeRateModel> rates)
        {
            Items = items;
            Count = count;
            TotalBalance = totals;
            ExchangeRates = rates;
        }

        public IEnumerable<BalanceTotalModel> TotalBalance { get; set; }

        public int Count { get; set; }

        public IEnumerable<ExchangeRateModel> ExchangeRates { get; set; }

        public IEnumerable<BalanceViewModel> Items { get; set; }
    }
}
