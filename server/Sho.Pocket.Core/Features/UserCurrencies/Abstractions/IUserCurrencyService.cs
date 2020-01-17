using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.UserCurrencies.Models;

namespace Sho.Pocket.Core.Features.UserCurrencies.Abstractions
{
    public interface IUserCurrencyService
    {
        Task<List<UserCurrencyModel>> GetUserCurrenciesAsync(Guid userOpenId);

        Task<UserCurrencyModel> GetUserCurrencyAsync(Guid userOpenId, string currency);

        Task<UserCurrencyModel> AddUserCurrencyAsync(Guid userOpenId, string currency, bool isPrimary);

        Task<UserCurrencyModel> SetUserPrimaryCurrencyAsync(Guid userOpenId, string currency);

        Task<bool> DeleteUserCurrencyAsync(Guid userOpenId, string currency);
    }
}
