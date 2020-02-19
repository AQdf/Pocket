using Sho.Pocket.Core.Features.BankAccounts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.BankAccounts
{
    public interface IBankAccountService
    {
        Task<BankAccountModel> GetBankAccountAsync(Guid userId, Guid assetId);

        Task<List<ExternalBankAccountModel>> SubmitBankClientAuthDataAsync(Guid userId, Guid assetId, string bankName, string token, string bankClientId, string cardNumber);

        Task<bool> ConnectAssetWithBankAcountAsync(Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId);

        Task<bool> DisconnectAssetWithBankAcountAsync(Guid userId, Guid assetId);

        Task<BankAccountBalance> GetBankAccountBalanceAsync(Guid userId, Guid assetId);

        Task<List<BankAccountTransactionModel>> GetBankAccountTransactionsAsync(Guid userId, Guid assetId);

        List<string> GetSupportedBanks();
    }
}
