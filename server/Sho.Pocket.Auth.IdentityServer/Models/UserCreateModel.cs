﻿namespace Sho.Pocket.Auth.IdentityServer.Models
{
    public class UserCreateModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
