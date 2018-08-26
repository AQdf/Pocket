using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;

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
        public IEnumerable<AssetViewModel> GetAll()
        {
            return _assetService.GetAll();
        }


        /// <summary>
        /// GET: api/Assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// POST: api/Assets
        /// </summary>
        /// <param name="newAsset"></param>
        /// <returns></returns>
        [HttpPost]
        public void Post([FromBody] AssetViewModel assetModel)
        {
            _assetService.Add(assetModel);
        }

        /// <summary>
        /// PUT: api/Assets/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updatedAsset"></param>
        [HttpPut("{Id}")]
        public bool Put(Guid id, [FromBody] AssetViewModel assetModel)
        {
            _assetService.Update(assetModel);

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
