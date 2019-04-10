using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBalanceRepository
    {
        List<Balance> GetAll(bool includeRelated = true);

        List<Balance> GetByEffectiveDate(DateTime effectiveDate, bool includeRelated = true);

        Balance GetById(Guid id);

        Balance Add(Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId);

        List<Balance> AddEffectiveBalances(DateTime currentEffectiveDate);

        Balance Update(Guid id, decimal value);

        void Remove(Guid balanceId);

        List<DateTime> GetOrderedEffectiveDates();

        void ApplyExchangeRate(Guid exchangeRateId, Guid counterCurrencyId, DateTime effectiveDate);
    }
}
