using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.BalancesTotal
{
    public interface IBalancesTotalService
    {
        Task<List<BalanceTotalModel>> GetCurrentTotalBalanceAsync(Guid userOpenId);

        Task<List<BalanceTotalChangeModel>> GetUserBalanceChangesAsync(Guid userOpenId, int count);

        Task<List<BalanceTotalModel>> CalculateTotalsAsync(Guid userOpenId, IEnumerable<Balance> balances, DateTime effectiveDate);
    }
}
