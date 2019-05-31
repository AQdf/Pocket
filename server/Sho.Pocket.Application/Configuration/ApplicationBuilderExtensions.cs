using Microsoft.AspNetCore.Builder;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.Application.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void SeedApplicationData(this IApplicationBuilder app, IDbConfiguration dbConfiguration)
        {
            dbConfiguration.SeedStorageData();
        }
    }
}
