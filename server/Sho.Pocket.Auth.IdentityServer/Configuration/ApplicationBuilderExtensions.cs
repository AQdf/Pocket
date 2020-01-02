using Microsoft.AspNetCore.Builder;

namespace Sho.Pocket.Auth.IdentityServer.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void SeedApplicationAuthData(this IApplicationBuilder app, IAuthDbInitializer authDbConfiguration)
        {
            authDbConfiguration.SeedApplicationAuthData().Wait();
        }
    }
}
