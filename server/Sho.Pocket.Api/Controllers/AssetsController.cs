using System;
using System.Collections.Generic;
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
        public IEnumerable<AssetViewModel> GetAll()
        {
            return _assetService.GetAll();
        }

        /// <summary>
        /// POST: api/assets
        /// </summary>
        /// <param name="newAsset"></param>
        /// <returns></returns>
        [HttpPost]
        public AssetViewModel Add([FromBody] AssetCreateModel createModel)
        {
            return _assetService.Add(createModel);
        }

        /// <summary>
        /// PUT: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedAsset"></param>
        [HttpPut("{id}")]
        public ActionResult<AssetViewModel> Update(Guid id, [FromBody] AssetUpdateModel updateModel)
        {
            AssetViewModel result = _assetService.Update(id, updateModel);

            return Ok(result);
        }

        /// <summary>
        /// DELETE: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(Guid id)
        {
            var result = _assetService.Delete(id);

            return Ok(result);
        }

        /// <summary>
        /// GET: api/assets/currencies
        /// </summary>
        /// <returns></returns>
        [HttpGet("currencies")]
        public IEnumerable<CurrencyViewModel> GetCurrencies()
        {
            return _assetService.GetCurrencies();
        }
    }
}
