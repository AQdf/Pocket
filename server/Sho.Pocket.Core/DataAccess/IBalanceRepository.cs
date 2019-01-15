using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBalanceRepository
    {
        List<Balance> GetAll(bool includeRelated = true);

        Balance GetById(Guid id);

        Balance Add(Balance balance);

        void AddEffectiveBalancesTemplate(DateTime currentEffectiveDate);

        void Update(Balance balance);

        void Remove(Guid balanceId);

        IEnumerable<DateTime> GetEffectiveDates();

        void ApplyExchangeRate(Guid exchangeRateId, Guid counterCurrencyId, DateTime effectiveDate);
    }
}
