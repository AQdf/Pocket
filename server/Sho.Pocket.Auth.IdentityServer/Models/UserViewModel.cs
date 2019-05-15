using System;

namespace Sho.Pocket.Auth.IdentityServer.Models
{
    public class UserViewModel
    {
        public UserViewModel(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public Guid Id { get; set; }

        public string Email { get; set; }
    }
}
