using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBalanceRepository
    {
        Task<IEnumerable<Balance>> GetAll(bool includeRelated = true);

        Task<IEnumerable<Balance>> GetByEffectiveDate(DateTime effectiveDate, bool includeRelated = true);

        Task<Balance> GetById(Guid id);

        Task<Balance> Add(Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId);

        Task<IEnumerable<Balance>> AddEffectiveBalances(DateTime currentEffectiveDate);

        Task<Balance> Update(Guid id, decimal value);

        Task Remove(Guid balanceId);

        Task<IEnumerable<DateTime>> GetOrderedEffectiveDates();

        Task ApplyExchangeRate(Guid exchangeRateId, Guid counterCurrencyId, DateTime effectiveDate);
    }
}
