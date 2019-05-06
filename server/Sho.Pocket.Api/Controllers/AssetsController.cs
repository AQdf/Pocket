using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Currencies.Models;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        /// <summary>
        /// GET: api/assets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<AssetViewModel>> GetAll()
        {
            IEnumerable<AssetViewModel> result = await _assetService.GetAll();

            return result;
        }

        /// <summary>
        /// GET: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AssetViewModel> Get(Guid id)
        {
            AssetViewModel asset = await _assetService.GetById(id);

            return asset;
        }

        /// <summary>
        /// POST: api/assets
        /// </summary>
        /// <param name="newAsset"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AssetViewModel> Add([FromBody] AssetCreateModel createModel)
        {
            AssetViewModel result = await _assetService.Add(createModel);

            return result;
        }

        /// <summary>
        /// PUT: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedAsset"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<AssetViewModel>> Update(Guid id, [FromBody] AssetUpdateModel updateModel)
        {
            AssetViewModel result = await _assetService.Update(id, updateModel);

            return Ok(result);
        }

        /// <summary>
        /// DELETE: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            bool result = await _assetService.Delete(id);

            return Ok(result);
        }

        /// <summary>
        /// GET: api/assets/currencies
        /// </summary>
        /// <returns></returns>
        [HttpGet("currencies")]
        public async Task<IEnumerable<CurrencyViewModel>> GetCurrencies()
        {
            var result = await _assetService.GetCurrencies();

            return result;
        }
    }
}
