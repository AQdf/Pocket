using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IUserCurrencyRepository
    {
        Task<IEnumerable<UserCurrency>> GetByUserIdAsync(Guid userOpenId);

        Task<UserCurrency> GetCurrencyAsync(Guid userOpenId, string currency);

        Task<UserCurrency> CreateAsync(Guid userOpenId, string currency, bool isPrimary);

        Task<bool> DeleteAsync(Guid userOpenId, string currency);

        Task<UserCurrency> SetPrimaryAsync(Guid userOpenId, string currency);

        Task<UserCurrency> GetPrimaryCurrencyAsync(Guid userOpenId);

        Task<bool> CheckIsPrimaryAsync(Guid userOpenId, string currency);
    }
}
