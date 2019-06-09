using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Application.UserCurrencies.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.UserCurrencies
{
    public class UserCurrencyService : IUserCurrencyService
    {
        private readonly IUserCurrencyRepository _userCurrencyRepository;

        public UserCurrencyService(IUserCurrencyRepository userCurrencyRepository)
        {
            _userCurrencyRepository = userCurrencyRepository;
        }

        public async Task<List<UserCurrencyModel>> GetUserCurrenciesAsync(Guid userOpenId)
        {
            IEnumerable<UserCurrency> userCurrencies = await _userCurrencyRepository.GetByUserIdAsync(userOpenId);
            List<UserCurrencyModel> result = userCurrencies?.Select(uc => new UserCurrencyModel(uc.Currency, uc.IsPrimary)).ToList();

            return result;
        }

        public async Task<UserCurrencyModel> GetUserCurrencyAsync(Guid userOpenId, string currency)
        {
            UserCurrency userCurrency = await _userCurrencyRepository.GetCurrencyAsync(userOpenId, currency);
            UserCurrencyModel result = new UserCurrencyModel(userCurrency.Currency, userCurrency.IsPrimary);

            return result;
        }

        public async Task<UserCurrencyModel> AddUserCurrencyAsync(Guid userOpenId, string currency, bool isPrimary)
        {
            UserCurrency userCurrency = await _userCurrencyRepository.GetCurrencyAsync(userOpenId, currency);

            if (userCurrency == null)
            {
                userCurrency = await _userCurrencyRepository.CreateAsync(userOpenId, currency, isPrimary);
            }

            UserCurrencyModel result = new UserCurrencyModel(userCurrency.Currency, userCurrency.IsPrimary);

            return result;
        }

        public async Task<bool> DeleteUserCurrencyAsync(Guid userOpenId, string currency)
        {
            var isPrimaryCurrency = await _userCurrencyRepository.CheckIsPrimaryAsync(userOpenId, currency);

            if (!isPrimaryCurrency)
            {
                bool result = await _userCurrencyRepository.DeleteAsync(userOpenId, currency);

                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<UserCurrencyModel> SetUserPrimaryCurrencyAsync(Guid userOpenId, string currency)
        {
            UserCurrency userCurrency = await _userCurrencyRepository.SetPrimaryAsync(userOpenId, currency);
            UserCurrencyModel result = new UserCurrencyModel(userCurrency.Currency, userCurrency.IsPrimary);

            return result;
        }
    }
}
