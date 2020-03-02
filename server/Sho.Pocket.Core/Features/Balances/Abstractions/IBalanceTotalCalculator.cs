using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalanceTotalCalculator
    {
        Task<BalanceTotalModel> CalculateAsync(IEnumerable<Balance> balances, string currency, string defaultCurrency, DateTime effectiveDate);
    }
}
