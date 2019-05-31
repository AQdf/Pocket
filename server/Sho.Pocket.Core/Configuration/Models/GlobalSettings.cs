namespace Sho.Pocket.Core.Configuration.Models
{
    public class GlobalSettings
    {
        public string DbConnectionString { get; set; }

        public string UsersDbConnectionString { get; set; }

        public string FreeCurrencyConverterApiKey { get; set; }

        public string JwtIssuer { get; set; }

        public string JwtAudience { get; set; }

        public string JwtKey { get; set; }

        public string SigningCredentials { get; set; }

        public string AdminEmail { get; set; }

        public string AdminPass { get; set; }
    }
}