using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Api.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Core.Features.BankAccounts;
using Sho.Pocket.Core.Features.BankAccounts.Models;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/bank-sync")]
    public class BankAccountController : AuthUserControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(
            IBankAccountService bankAccountService,
            IAuthService authService)
            : base(authService)
        {
            _bankAccountService = bankAccountService;
        }

        /// <summary>
        /// GET: api/assets/bank-sync/banks-lookup
        /// </summary>
        /// <returns></returns>
        [HttpGet("banks-lookup")]
        public async Task<ActionResult<List<string>>> GetSupportedBanks()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<string> result = _bankAccountService.GetSupportedBanks();

            return HandleResult(result);
        }

        /// <summary>
        /// GET: api/assets/bank-sync/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <returns></returns>
        [HttpGet("{assetId}")]
        public async Task<ActionResult<BankAccountModel>> GetBankAccountSyncData(Guid assetId)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            BankAccountModel result = await _bankAccountService.GetBankAccountAsync(user.Id, assetId);

            return new ActionResult<BankAccountModel>(result);
        }

        /// <summary>
        /// GET: api/assets/bank-sync/transactions/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <returns></returns>
        [HttpGet("transactions/{assetId}")]
        public async Task<ActionResult<List<BankAccountModel>>> GetAssetBankAccountTransactions(Guid assetId)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<BankAccountTransactionModel> result = await _bankAccountService.GetBankAccountTransactionsAsync(user.Id, assetId);

            return Ok(result);
        }

        /// <summary>
        /// POST: api/assets/bank-sync/auth
        /// </summary>
        /// <returns></returns>
        [HttpPost("auth")]
        public async Task<ActionResult<List<ExternalBankAccountModel>>> SubmitBankClientAuthData([FromBody] AssetBankAccountAuthDataRequest request)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<ExternalBankAccountModel> result = await _bankAccountService.SubmitBankClientAuthDataAsync(user.Id, request.AssetId, request.BankName, request.Token, request.BankClientId, request.CardNumber);

            return Ok(result);
        }

        /// <summary>
        /// POST: api/assets/bank-sync/connect-account
        /// </summary>
        /// <returns></returns>
        [HttpPost("connect-account")]
        public async Task<ActionResult<bool>> ConnectAssetWithBankAcount([FromBody] AssetBankAccountUpdateRequest createRequest)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            bool result = await _bankAccountService
                .ConnectAssetWithBankAcountAsync(user.Id, createRequest.AssetId, createRequest.BankName, createRequest.AccountName, createRequest.BankAccountId);

            return Ok(result);
        }

        /// <summary>
        /// POST: api/assets/bank-sync/disconnect-account/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <returns></returns>
        [HttpPost("disconnect-account/{assetId}")]
        public async Task<ActionResult<bool>> DisconnectAssetWithBankAcount(Guid assetId)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            bool result = await _bankAccountService.DisconnectAssetWithBankAcountAsync(user.Id, assetId);

            return Ok(result);
        }
    }
}
