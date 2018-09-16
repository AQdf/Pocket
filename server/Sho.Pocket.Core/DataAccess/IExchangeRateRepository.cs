using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IExchangeRateRepository
    {
        ExchangeRate Alter(DateTime effectiveDate, Guid assetId, decimal rate);
    }
}