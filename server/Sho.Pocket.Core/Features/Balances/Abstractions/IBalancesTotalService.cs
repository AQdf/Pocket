using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalancesTotalService
    {
        Task<List<BalanceTotalModel>> GetLatestTotalBalanceAsync(Guid userOpenId);

        Task<List<BalanceTotalChangeModel>> GetUserBalanceChangesAsync(Guid userOpenId, int count);

        Task<List<BalanceTotalModel>> CalculateTotalsAsync(Guid userOpenId, IEnumerable<Balance> balances, DateTime effectiveDate);
    }
}
