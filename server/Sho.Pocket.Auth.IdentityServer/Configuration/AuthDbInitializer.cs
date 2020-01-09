using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sho.Pocket.Auth.IdentityServer.Configuration.Models;
using Sho.Pocket.Auth.IdentityServer.DataAccess;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Configuration
{
    public class AuthDbInitializer : IAuthDbInitializer
    {
        private readonly string[] _defaultRoles;
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthDbInitializer(
            ApplicationAuthDataContext dbContext,
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<AuthSettings> options)
        {
            dbContext.Database.Migrate();

            _defaultRoles = new string[] { RoleConst.Admin, RoleConst.Simple };
            _adminEmail = options.CurrentValue.AdminEmail;
            _adminPassword = options.CurrentValue.AdminPass;

            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedApplicationAuthData()
        {
            await EnsureRoles();
            await EnsureDefaultUser();
        }

        private async Task EnsureRoles()
        {
            foreach (var role in _defaultRoles)
            {
                bool exists = await _roleManager.RoleExistsAsync(role);

                if (!exists)
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

        private async Task EnsureDefaultUser()
        {
            IList<ApplicationUser> adminUsers = await _userManager.GetUsersInRoleAsync(RoleConst.Admin);

            if (!adminUsers.Any())
            {
                var adminUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    Email = _adminEmail,
                    UserName = _adminEmail,
                    SecurityStamp = DateTime.UtcNow.ToString()
                };

                await _userManager.CreateAsync(adminUser, _adminPassword);
                await _userManager.AddToRoleAsync(adminUser, RoleConst.Admin);
            }
        }
    }
}
