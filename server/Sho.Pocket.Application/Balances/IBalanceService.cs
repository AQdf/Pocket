using System;
using System.Collections.Generic;
using Sho.Pocket.Application.Balances.Models;

namespace Sho.Pocket.Application.Balances
{
    public interface IBalanceService
    {
        BalancesViewModel GetAll(DateTime? effectiveDate);

        void Add(BalanceViewModel balanceModel);

        void Update(BalanceViewModel balanceModel);

        void Delete(Guid Id);

        decimal GetTotalBalance();

        IEnumerable<DateTime> GetEffectiveDates();
    }
}
