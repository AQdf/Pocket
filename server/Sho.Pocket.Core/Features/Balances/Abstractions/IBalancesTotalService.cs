using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalancesTotalService
    {
        Task<List<BalanceTotalModel>> GetLatestTotalBalanceAsync(Guid userId);

        Task<List<BalanceTotalChangeModel>> GetUserBalanceChangesAsync(Guid userId, int count);

        Task<List<BalanceTotalModel>> CalculateTotalsAsync(Guid userId, IEnumerable<Balance> balances, DateTime effectiveDate);

        Task<List<BalancePrimaryCurrencyModel>> GetUserPrimaryCurrencyBalancesAsync(Guid userId);
    }
}
