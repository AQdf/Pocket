namespace Sho.Pocket.Auth.IdentityServer.Models
{
    public class LoginResult
    {
        private LoginResult(bool succeeded, string jwt)
        {
            Succeeded = succeeded;
            Jwt = jwt;
        }

        public string Jwt { get; set; }

        public bool Succeeded { get; set; }

        public static LoginResult Success(string jwt)
        {
            return new LoginResult(true, jwt);
        }

        public static LoginResult Failed()
        {
            return new LoginResult(false, null);
        }
    }
}
