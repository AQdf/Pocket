using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sho.Pocket.Auth.IdentityServer.Models;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUser(UserCreateModel model)
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                UserName = model.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            return result;
        }
    }
}
