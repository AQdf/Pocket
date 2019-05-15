using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/assets")]
    public class AssetsController : AuthUserApiControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService, IAuthService authService) : base(authService)
        {
            _assetService = assetService;
        }

        /// <summary>
        /// GET: api/assets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<AssetViewModel>>> GetCurrentUserAssets()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<AssetViewModel> result = await _assetService.GetAssetsAsync(user.Id);

            return HandleResult(result);
        }

        /// <summary>
        /// GET: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetViewModel>> GetCurrentUserAsset(Guid id)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            AssetViewModel result = await _assetService.GetAssetAsync(user.Id, id);

            return HandleResult(result);
        }

        /// <summary>
        /// POST: api/assets
        /// </summary>
        /// <param name="newAsset"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<AssetViewModel>> AddAsset([FromBody] AssetCreateModel createModel)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            AssetViewModel result = await _assetService.AddAssetAsync(user.Id, createModel);

            return HandleResult(result);
        }

        /// <summary>
        /// PUT: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedAsset"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<AssetViewModel>> UpdateAsset(Guid id, [FromBody] AssetUpdateModel updateModel)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            AssetViewModel result = await _assetService.UpdateAsync(user.Id, id, updateModel);

            return HandleResult(result);
        }

        /// <summary>
        /// DELETE: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsset(Guid id)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            bool result = await _assetService.DeleteAsync(user.Id, id);

            return Ok(result);
        }
    }
}
