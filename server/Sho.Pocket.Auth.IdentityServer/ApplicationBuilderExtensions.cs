using Microsoft.AspNetCore.Builder;
using Sho.Pocket.Core.Auth;

namespace Sho.Pocket.Auth.IdentityServer
{
    public static class ApplicationBuilderExtensions
    {
        public static void SeedApplicationAuthData(this IApplicationBuilder app, IAuthDbConfiguration authDbConfiguration)
        {
            authDbConfiguration.SeedApplicationAuthData().Wait();
        }
    }
}
