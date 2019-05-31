using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.BalancesTotal
{
    public interface IBalancesTotalService
    {
        Task<List<BalanceTotalModel>> GetCurrentTotalBalance(Guid userOpenId);

        Task<List<BalanceTotalModel>> GetCurrencyTotals(Guid userOpenId, string currency, int count);

        Task<List<BalanceTotalModel>> CalculateTotalsAsync(IEnumerable<Balance> balances, DateTime effectiveDate);
    }
}
