using System;

namespace Sho.Pocket.Data
{
    public static class DbConfiguration
    {
        public static string GetConnectionString()
        {
            return @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=PocketDb;Integrated Security=SSPI;AttachDBFilename=E:\Code\Projects\Pocket\database\PocketDb.mdf";
        }
    }
}
