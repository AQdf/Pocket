using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sho.Pocket.Auth.IdentityServer.Models;
using System;

namespace Sho.Pocket.Auth.IdentityServer.DataAccess
{
    public class ApplicationAuthDataContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationAuthDataContext(DbContextOptions<ApplicationAuthDataContext> options) : base(options)
        {
        }
    }
}
