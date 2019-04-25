using Microsoft.AspNetCore.Identity;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Core.Auth;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer
{
    public class AuthDbConfiguration : IAuthDbConfiguration
    {
        private const string _adminRoleName = "admin";
        private readonly string _adminName;
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        private readonly string[] _defaultRoles = new string[] { _adminRoleName };

        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthDbConfiguration(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

            _adminName = Environment.GetEnvironmentVariable("POCKET_ADMIN_NAME");
            _adminEmail = Environment.GetEnvironmentVariable("POCKET_ADMIN_EMAIL");
            _adminPassword = Environment.GetEnvironmentVariable("POCKET_ADMIN_PASS");
        }

        public void SeedApplicationAuthData()
        {
            EnsureRoles();
        }

        private void EnsureRoles()
        {
            foreach (var role in _defaultRoles)
            {
                if (!_roleManager.RoleExistsAsync(role).Result)
                {
                    _roleManager.CreateAsync(new IdentityRole<Guid>(role)).RunSynchronously();
                }
            }
        }

        private void EnsureDefaultUser()
        {
            var adminUsers = _userManager.GetUsersInRoleAsync(_adminRoleName).Result;

            if (!adminUsers.Any())
            {
                var adminUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    Email = _adminEmail,
                    UserName = _adminName
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, _adminPassword).Result;
                _userManager.AddToRoleAsync(adminUser, _adminRoleName).RunSynchronously();
            }
        }
    }
}
