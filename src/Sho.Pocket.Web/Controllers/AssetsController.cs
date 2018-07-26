using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Api.ViewModels;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Services;

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
        /// GET: api/Assets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Asset> GetAll()
        {
            return _assetService.GetAll();
        }


        /// <summary>
        /// GET: api/Assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public string Get(Guid Id)
        {
            return "value";
        }

        /// <summary>
        /// POST: api/Assets
        /// </summary>
        /// <param name="newAsset"></param>
        /// <returns></returns>
        [HttpPost]
        public Asset Post([FromBody] AssetViewModel newAsset)
        {
            Asset asset = new Asset
            {
                Name = newAsset.Name,
                TypeName = newAsset.TypeName,
                CurrencyName = newAsset.CurrencyName,
                Balance = newAsset.Balance
            };

            Asset result = _assetService.Add(asset);

            return result;
        }

        /// <summary>
        /// PUT: api/Assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updatedAsset"></param>
        [HttpPut("{Id}")]
        public bool Put(Guid Id, [FromBody] AssetViewModel updatedAsset)
        {
            Asset asset = new Asset
            {
                Id = Id,
                Name = updatedAsset.Name,
                TypeName = updatedAsset.TypeName,
                CurrencyName = updatedAsset.CurrencyName,
                Balance = updatedAsset.Balance
            };

            _assetService.Update(asset);

            return true;
        }

        /// <summary>
        /// DELETE: api/ApiWithActions/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        [HttpDelete("{Id}")]
        public bool Delete(Guid Id)
        {
            _assetService.Delete(Id);

            return true;
        }
    }
}
