//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Sho.Pocket.Core.DataAccess;
//using Sho.Pocket.Core.Features.Accounts;
//using Sho.Pocket.Core.Features.Accounts.Models;
//using Sho.Pocket.Core.Features.AccountsBalance;
//using Sho.Pocket.Core.Features.BankAccounts;
//using Sho.Pocket.Core.Features.BankAccounts.Models;
//using Sho.Pocket.Domain.Entities;

//namespace Sho.Pocket.Application.Features.Accounts
//{
//    public class AccountService : IAccountService
//    {
//        private readonly IAccountBalanceService _accountBalanceService;

//        private readonly IBankAccountService _bankAccountService;

//        private readonly IAccountRepository _accountRepository;

//        private readonly IUnitOfWork _unitOfWork;

//        public AccountService(
//            IAccountBalanceService accountBalanceService,
//            IBankAccountService bankAccountService,
//            IAccountRepository accountRepository,
//            IUnitOfWork unitOfWork)
//        {
//            _accountBalanceService = accountBalanceService;
//            _bankAccountService = bankAccountService;
//            _accountRepository = accountRepository;
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<AccountModel> AddAsync(Guid userId, AccountCreateModel createModel)
//        {
//            Account account = await _accountRepository.CreateAccountAsync(userId, createModel);
//            await _unitOfWork.SaveChangesAsync();
//            AccountModel accountModel = new AccountModel(account);

//            return accountModel;
//        }

//        public async Task<IReadOnlyCollection<AccountModel>> GetUserAccountsAsync(Guid userId, bool includeInactive = false)
//        {
//            IReadOnlyCollection<Account> accounts = await _accountRepository.GetAccountsAsync(userId, includeInactive);

//            if (accounts.Count == 0)
//            {
//                return new List<AccountModel>();
//            }

//            DateTime now = DateTime.UtcNow;
//            DateTime updatedOn = accounts.FirstOrDefault().UpdatedOn;

//            if (updatedOn.Date < now.Date)
//            {
//                await _accountBalanceService.AddAccountsBalanceAsync(userId, updatedOn.Date);
//            }

//            foreach (Account account in accounts)
//            {
//                bool isBankAccount = await _bankAccountService.CheckAccountIsBankAccount(userId, account.Id);

//                if (isBankAccount)
//                {
//                    BankAccountBalanceModel bankAccount = await _bankAccountService.GetBankAccountBalanceAsync(userId, account.Id);
//                    account.Balance = bankAccount.Balance;
//                    account.UpdatedOn = now;
//                }
//            }

//            await _unitOfWork.SaveChangesAsync();


//            List<AccountModel> result = accounts.Select(a => new AccountModel(a)).ToList();

//            return result;
//        }

//        public async Task<AccountModel> UpdateAsync(Guid userId, Guid id, AccountUpdateModel updateModel)
//        {
//            Account account = await _accountRepository.GetAccountAsync(userId, id);

//            if (account.Currency != updateModel.Currency)
//            {
//                bool balanceExists = await _accountBalanceService.ExistsAsync(userId, id);

//                if (balanceExists)
//                {
//                    throw new Exception("You cannot change currency if balance history exists.");
//                }
//            }

//            Account updatedAccount = await _accountRepository.UpdateAsync(userId, id, updateModel);
//            await _unitOfWork.SaveChangesAsync();

//            return new AccountModel(updatedAccount);
//        }

//        public async Task<bool> DeleteAsync(Guid userId, Guid id)
//        {
//            bool balanceExists = await _accountBalanceService.ExistsAsync(userId, id);

//            if (balanceExists)
//            {
//                await _accountRepository.DeactiveAccountAsync(userId, id);
//            }
//            else
//            {
//                await _accountRepository.DeleteAccountAsync(userId, id);
//            }

//            await _unitOfWork.SaveChangesAsync();

//            return true;
//        }
//    }
//}
