using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBalanceRepository
    {
        List<Balance> GetAll(bool includeRelated = true);

        Balance Add(Balance balance);

        void Update(Balance balance);

        void Remove(Guid balanceId);
    }
}
