using Microsoft.AspNetCore.Identity;
using Sho.Pocket.Auth.IdentityServer.Models;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public interface IRegistrationService
    {
        Task<UserCreationResult> CreateSimpleUser(UserCreateModel model);
    }
}
