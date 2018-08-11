using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Api.ViewModels;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Services;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/asset-history")]
    [ApiController]
    public class AssetHistoryController : ControllerBase
    {
        private readonly IAssetHistoryService _assetHistoryService;

        public AssetHistoryController(IAssetHistoryService assetHistoryService)
        {
            _assetHistoryService = assetHistoryService;
        }

        /// <summary>
        /// GET: api/AssetHistory
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<AssetHistory> Get()
        {
            return _assetHistoryService.GetAll();
        }

        /// <summary>
        /// GET: api/AssetHistory/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// POST: api/AssetHistory
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public AssetHistory Post([FromBody] AssetHistoryViewModel viewModel)
        {
            AssetHistory asset = new AssetHistory
            {
                AssetName = viewModel.AssetName,
                EffectiveDate = viewModel.EffectiveDate,
                Balance = viewModel.Balance
            };

            AssetHistory result = _assetHistoryService.Add(asset);

            return result;
        }

        /// <summary>
        /// PUT: api/AssetHistory/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        [HttpPut("{id}")]
        public bool Put(Guid id, [FromBody] AssetHistoryViewModel viewModel)
        {
            AssetHistory assetHistory = new AssetHistory
            {
                Id = id,
                AssetName = viewModel.AssetName,
                EffectiveDate = viewModel.EffectiveDate,
                Balance = viewModel.Balance
            };

            _assetHistoryService.Update(assetHistory);

            return true;
        }

        /// <summary>
        /// DELETE: api/AssetHistory/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            _assetHistoryService.Delete(id);

            return true;
        }
    }
}
