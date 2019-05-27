using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.ItemCategories;
using Sho.Pocket.Auth.IdentityServer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.AdminControllers
{
    [Route("api/admin/item-categories")]
    public class ItemCategoryAdminController : AuthAdminControllerBase
    {
        private readonly IItemCategoryService _itemCategoryService;

        public ItemCategoryAdminController(IItemCategoryService itemCategoryService, IAuthService authService) : base(authService)
        {
            _itemCategoryService = itemCategoryService;
        }

        /// <summary>
        /// GET: api/admin/item-categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetDefaultCategories()
        {
            bool isAdmin = await VerifyCurrentAdminUserAsync();

            if (!isAdmin)
            {
                return HandleUserNotAdminResult();
            }

            IEnumerable<string> result = await _itemCategoryService.GetDefaultCategoriesAsync();

            return Ok(result);
        }

        /// <summary>
        /// GET: api/admin/item-categories/exists/smartphone
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("exists/{name}")]
        public async Task<ActionResult<bool>> ExistsDefaultCategory(string name)
        {
            bool isAdmin = await VerifyCurrentAdminUserAsync();

            if (!isAdmin)
            {
                return HandleUserNotAdminResult();
            }

            bool result = await _itemCategoryService.ExistsAsync(name);

            return result;
        }

        /// <summary>
        /// POST: api/item-categories
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> AddDefaultCategory([FromBody] string name)
        {
            bool isAdmin = await VerifyCurrentAdminUserAsync();

            if (!isAdmin)
            {
                return HandleUserNotAdminResult();
            }

            string result = await _itemCategoryService.AddDefaultCategoryAsync(name);

            return Ok(result);
        }

        /// <summary>
        /// PUT: api/admin/item-categories/laptop
        /// </summary>
        /// <param name="oldName"></param>
        [HttpPut("{oldName}")]
        public async Task<ActionResult<string>> UpdateDefaultCategory(string oldName, [FromBody] string newName)
        {
            bool isAdmin = await VerifyCurrentAdminUserAsync();

            if (!isAdmin)
            {
                return HandleUserNotAdminResult();
            }

            string result = await _itemCategoryService.UpdateDefaultCategoryAsync(oldName, newName);

            return HandleResult(result);
        }

        /// <summary>
        /// DELETE: api/admin/item-categories/laptop
        /// </summary>
        /// <param name="name"></param>
        [HttpDelete("{name}")]
        public async Task<ActionResult<bool>> DeleteDefaultCategory(string name)
        {
            bool isAdmin = await VerifyCurrentAdminUserAsync();

            if (!isAdmin)
            {
                return HandleUserNotAdminResult();
            }

            bool result = await _itemCategoryService.DeleteDefaultCategoryAsync(name);

            return result ? (ActionResult<bool>)Ok(result) : (ActionResult<bool>)BadRequest(result);
        }
    }
}
