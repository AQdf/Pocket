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
        public void Add([FromBody] AssetViewModel assetModel)
        {
            _assetService.Add(assetModel);
        }

        /// <summary>
        /// PUT: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updatedAsset"></param>
        [HttpPut("{Id}")]
        public bool Update(Guid Id, [FromBody] AssetViewModel assetModel)
        {
            _assetService.Update(assetModel);

            return true;
        }

        /// <summary>
        /// DELETE: api/assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        [HttpDelete("{Id}")]
        public bool Delete(Guid Id)
        {
            _assetService.Delete(Id);

            return true;
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
