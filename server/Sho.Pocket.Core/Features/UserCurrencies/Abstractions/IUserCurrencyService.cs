using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.UserCurrencies.Models;

namespace Sho.Pocket.Core.Features.UserCurrencies.Abstractions
{
    public interface IUserCurrencyService
    {
        Task<List<UserCurrencyModel>> GetUserCurrenciesAsync(Guid userId);

        Task<UserCurrencyModel> GetUserCurrencyAsync(Guid userId, string currency);

        Task<UserCurrencyModel> AddUserCurrencyAsync(Guid userId, string currency, bool isPrimary);

        Task<UserCurrencyModel> SetUserPrimaryCurrencyAsync(Guid userId, string currency);

        Task<bool> DeleteUserCurrencyAsync(Guid userId, string currency);
    }
}
