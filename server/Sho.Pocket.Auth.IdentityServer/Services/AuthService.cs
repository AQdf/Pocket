using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sho.Pocket.Auth.IdentityServer.Models;

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
    }
}
