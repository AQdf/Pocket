using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Api.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Core.Features.BankAccounts.Abstractions;
using Sho.Pocket.Core.Features.BankAccounts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/bank-sync")]
    public class AssetBankSyncController : AuthUserControllerBase
    {
        private readonly IBankAccountSyncService _accountBankSyncService;

        public AssetBankSyncController(IBankAccountSyncService accountBankSyncService, IAuthService authService) : base(authService)
        {
            _accountBankSyncService = accountBankSyncService;
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

            List<string> result = await _accountBankSyncService.GetSupportedBanksAsync();

            return HandleResult(result);
        }

        /// <summary>
        /// GET: api/assets/bank-sync/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <returns></returns>
        [HttpGet("{assetId}")]
        public async Task<ActionResult<AssetBankAccountViewModel>> GetBankAccountSyncData(Guid assetId)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            AssetBankAccountViewModel result = await _accountBankSyncService.GetAssetBankAccountAsync(user.Id, assetId);

            return new ActionResult<AssetBankAccountViewModel>(result);
        }

        /// <summary>
        /// POST: api/assets/bank-sync/auth
        /// </summary>
        /// <returns></returns>
        [HttpPost("auth")]
        public async Task<ActionResult<List<BankAccount>>> SubmitBankClientAuthData([FromBody] AssetBankAccountAuthDataRequest request)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<BankAccount> result = await _accountBankSyncService.SubmitBankClientAuthDataAsync(user.Id, request.AssetId, request.BankName, request.Token, request.BankClientId, request.CardNumber);

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

            bool result = await _accountBankSyncService
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

            bool result = await _accountBankSyncService.DisconnectAssetWithBankAcountAsync(user.Id, assetId);

            return Ok(result);
        }
    }
}
