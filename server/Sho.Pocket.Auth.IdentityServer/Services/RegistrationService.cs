using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Utils;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserCreationResult> CreateSimpleUser(UserCreateModel model)
        {
            Guid userId = Guid.NewGuid();
            ApplicationUser userToCreate = new ApplicationUser
            {
                Id = userId,
                Email = model.Email,
                UserName = model.Email
            };

            UserCreationResult result = null;
            IdentityResult identityResult = await _userManager.CreateAsync(userToCreate, model.Password);

            if (identityResult.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());
                await _userManager.AddToRoleAsync(user, RoleConst.Simple);

                result = new UserCreationResult(identityResult, user);
            }

            return result;
        }
    }
}
