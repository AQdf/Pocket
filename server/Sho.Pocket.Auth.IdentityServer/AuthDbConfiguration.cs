﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sho.Pocket.Auth.IdentityServer.DataAccess;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Core;
using Sho.Pocket.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer
{
    public class AuthDbConfiguration : IAuthDbConfiguration
    {
        private readonly string _adminRoleName;
        private readonly string[] _defaultRoles;
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthDbConfiguration(
            ApplicationAuthDataContext dbContext,
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager,
            GlobalSettings settings)
        {
            dbContext.Database.Migrate();

            _adminRoleName = settings.AdminRole;
            _defaultRoles = new string[] { _adminRoleName };
            _adminEmail = settings.AdminEmail;
            _adminPassword = settings.AdminPass;

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
            IList<ApplicationUser> adminUsers = await _userManager.GetUsersInRoleAsync(_adminRoleName);

            if (!adminUsers.Any())
            {
                var adminUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    Email = _adminEmail,
                    UserName = _adminEmail,
                    SecurityStamp = DateTime.UtcNow.ToString()
                };

                IdentityResult result = await _userManager.CreateAsync(adminUser, _adminPassword);

                await _userManager.AddToRoleAsync(adminUser, _adminRoleName);
            }
        }
    }
}
