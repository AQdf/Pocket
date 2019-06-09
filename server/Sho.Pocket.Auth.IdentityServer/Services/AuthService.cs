using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Utils;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserViewModel> GetUserById(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            UserViewModel model = new UserViewModel(user.Id, user.Email);

            return model;
        }

        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            UserViewModel model = new UserViewModel(user.Id, user.Email);

            return model;
        }

        public async Task<bool> VerifyAdminUserById(string id)
        {
            bool result = false;
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                result = await _userManager.IsInRoleAsync(user, RoleConst.Admin);
            }

            return result;
        }
    }
}
