using Sho.Pocket.Core;
using Sho.Pocket.Core.Abstractions;

namespace Sho.Pocket.Data
{
    public class DbConfiguration : IDbConfiguration
    {
        public string DbConnectionString { get; }

        public DbConfiguration(GlobalSettings globalSettings)
        {
            DbConnectionString = globalSettings.DbConnectionString;
        }
    }
}
