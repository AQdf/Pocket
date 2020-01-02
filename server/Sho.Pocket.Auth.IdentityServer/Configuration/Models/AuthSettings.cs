namespace Sho.Pocket.Auth.IdentityServer.Configuration.Models
{
    public class AuthSettings
    {
        public string UsersDbConnectionString { get; set; }

        public string JwtIssuer { get; set; }

        public string JwtAudience { get; set; }

        public string JwtKey { get; set; }

        public string SigningCredentials { get; set; }

        public string AdminEmail { get; set; }

        public string AdminPass { get; set; }
    }
}
