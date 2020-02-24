using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IUserCurrencyRepository
    {
        Task<IEnumerable<UserCurrency>> GetByUserIdAsync(Guid userId);

        Task<UserCurrency> GetCurrencyAsync(Guid userId, string currency);

        Task<UserCurrency> CreateAsync(Guid userId, string currency, bool isPrimary);

        Task<bool> DeleteAsync(Guid userId, string currency);

        Task<UserCurrency> SetPrimaryAsync(Guid userId, string currency);

        Task<UserCurrency> GetPrimaryCurrencyAsync(Guid userId);

        Task<bool> CheckIsPrimaryAsync(Guid userId, string currency);
    }
}
