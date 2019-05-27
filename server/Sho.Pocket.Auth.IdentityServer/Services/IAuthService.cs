using Sho.Pocket.Auth.IdentityServer.Models;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public interface IAuthService
    {
        Task<UserViewModel> GetUserById(string id);

        Task<bool> VerifyAdminUserById(string id);
    }
}
