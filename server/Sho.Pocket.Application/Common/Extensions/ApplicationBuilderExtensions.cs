using Microsoft.AspNetCore.Builder;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.Application.Common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void SeedData(this IApplicationBuilder app, IDbConfiguration dbConfiguration)
        {
            dbConfiguration.SeedStorageData();
        }
    }
}
