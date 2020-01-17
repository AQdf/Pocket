using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Core.Features.Inventory.Abstractions;
using Sho.Pocket.Core.Features.Inventory.Models;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/inventory")]
    public class InventoryController : AuthUserControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService, IAuthService authService) : base(authService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// GET: api/inventory
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<InventoryItemModel>>> GetInventoryItems()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<InventoryItemModel> result = await _inventoryService.GetItemsAsync(user.Id);

            return Ok(result);
        }

        /// <summary>
        /// GET: api/inventory/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItemModel>> GetInventoryItems(Guid id)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            InventoryItemModel result = await _inventoryService.GetItemAsync(user.Id, id);

            return HandleResult(result);
        }

        /// <summary>
        /// POST: api/inventory
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<InventoryItemModel>> AddInventoryItem([FromBody] InventoryItemCreateModel createModel)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            InventoryItemModel result = await _inventoryService.AddItemAsync(user.Id, createModel);

            return HandleResult(result);
        }

        /// <summary>
        /// PUT: api/inventory/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateModel"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<InventoryItemModel>> UpdateInventoryItem(Guid id, [FromBody] InventoryItemUpdateModel updateModel)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            InventoryItemModel result = await _inventoryService.UpdateItemAsync(user.Id, id, updateModel);

            return HandleResult(result);
        }

        /// <summary>
        /// DELETE: api/inventory/0E056948-4014-4A2A-A132-5493A8499B9A
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

            bool result = await _inventoryService.DeleteItemAsync(user.Id, id);

            return Ok(result);
        }
    }
}
