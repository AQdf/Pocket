using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        public Task<IEnumerable<Balance>> AddEffectiveBalances(DateTime currentEffectiveDate)
        {
            throw new NotImplementedException();
        }

        public Task<Balance> CreateAsync(Guid userOpenId, Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsEffectiveDateBalancesAsync(Guid userOpenId, DateTime effectiveDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Balance>> GetAllAsync(Guid userOpenId, bool includeRelated = true)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Balance>> GetByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate, bool includeRelated = true)
        {
            throw new NotImplementedException();
        }

        public Task<Balance> GetByIdAsync(Guid userOpenId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Balance>> GetLatestBalancesAsync(Guid userOpenId, bool includeRelated = true)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DateTime>> GetOrderedEffectiveDatesAsync(Guid userOpenId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(Guid userOpenId, Guid balanceId)
        {
            throw new NotImplementedException();
        }

        public Task<Balance> UpdateAsync(Guid userOpenId, Guid id, Guid assetId, decimal value)
        {
            throw new NotImplementedException();
        }
    }
}
