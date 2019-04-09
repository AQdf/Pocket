namespace Sho.Pocket.Core.DataAccess
{
    public interface IDbConfiguration
    {
        string DbConnectionString { get; }

        void SeedStorageData();
    }
}
