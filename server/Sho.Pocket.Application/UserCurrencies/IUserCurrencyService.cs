using Sho.Pocket.Application.UserCurrencies.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.UserCurrencies
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
