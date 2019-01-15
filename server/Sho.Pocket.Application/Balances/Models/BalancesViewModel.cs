using Sho.Pocket.Application.ExchangeRates.Models;
using System.Collections.Generic;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalancesViewModel
    {
        public BalancesViewModel(List<BalanceViewModel> items, int count, decimal totalBalance, List<ExchangeRateModel> rates)
        {
            Items = items;
            Count = count;
            TotalBalance = totalBalance;
            ExchangeRates = rates;
        }

        public decimal TotalBalance { get; set; }

        public int Count { get; set; }

        public List<ExchangeRateModel> ExchangeRates { get; set; }

        public List<BalanceViewModel> Items { get; set; }
    }
}
