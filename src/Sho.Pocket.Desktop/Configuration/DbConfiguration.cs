using Sho.Pocket.Core.Abstractions;

namespace Sho.Pocket.Desktop.Configuration
{
    public class DbConfiguration : IDbConfiguration
    {
        public string PocketDbConnectionString { get; set; }

        public DbConfiguration(string connectionString)
        {
            PocketDbConnectionString = connectionString;
        }
    }
}
