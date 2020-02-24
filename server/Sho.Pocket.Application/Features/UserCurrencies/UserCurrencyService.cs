using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.UserCurrencies.Abstractions;
using Sho.Pocket.Core.Features.UserCurrencies.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.UserCurrencies
{
    public class UserCurrencyService : IUserCurrencyService
    {
        private readonly IUserCurrencyRepository _userCurrencyRepository;

        private readonly IUnitOfWork _unitOfWork;

        public UserCurrencyService(IUserCurrencyRepository userCurrencyRepository, IUnitOfWork unitOfWork)
        {
            _userCurrencyRepository = userCurrencyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserCurrencyModel>> GetUserCurrenciesAsync(Guid userId)
        {
            IEnumerable<UserCurrency> userCurrencies = await _userCurrencyRepository.GetByUserIdAsync(userId);
            List<UserCurrencyModel> result = userCurrencies?.Select(uc => new UserCurrencyModel(uc.Currency, uc.IsPrimary)).ToList();

            return result;
        }

        public async Task<UserCurrencyModel> GetUserCurrencyAsync(Guid userId, string currency)
        {
            UserCurrency userCurrency = await _userCurrencyRepository.GetCurrencyAsync(userId, currency);
            UserCurrencyModel result = new UserCurrencyModel(userCurrency.Currency, userCurrency.IsPrimary);

            return result;
        }

        public async Task<UserCurrencyModel> AddUserCurrencyAsync(Guid userId, string currency, bool isPrimary)
        {
            UserCurrency userCurrency = await _userCurrencyRepository.GetCurrencyAsync(userId, currency);

            if (userCurrency == null)
            {
                userCurrency = await _userCurrencyRepository.CreateAsync(userId, currency, isPrimary);
                await _unitOfWork.SaveChangesAsync();
            }

            UserCurrencyModel result = new UserCurrencyModel(userCurrency.Currency, userCurrency.IsPrimary);

            return result;
        }

        public async Task<bool> DeleteUserCurrencyAsync(Guid userId, string currency)
        {
            bool isPrimaryCurrency = await _userCurrencyRepository.CheckIsPrimaryAsync(userId, currency);

            if (isPrimaryCurrency)
            {
                throw new Exception($"You cannot delete primary currency {currency}");
            }

            bool result = await _userCurrencyRepository.DeleteAsync(userId, currency);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<UserCurrencyModel> SetUserPrimaryCurrencyAsync(Guid userId, string currency)
        {
            UserCurrency userCurrency = await _userCurrencyRepository.SetPrimaryAsync(userId, currency);
            await _unitOfWork.SaveChangesAsync();

            UserCurrencyModel result = new UserCurrencyModel(userCurrency.Currency, userCurrency.IsPrimary);

            return result;
        }
    }
}
