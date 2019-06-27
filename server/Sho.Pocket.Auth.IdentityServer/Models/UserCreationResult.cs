using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Sho.Pocket.Auth.IdentityServer.Models
{
    public class UserCreationResult
    {
        public UserCreationResult(IdentityResult identityResult, ApplicationUser user)
        {
            Succeeded = identityResult.Succeeded;
            Errors = identityResult.Errors;
            User = new UserViewModel(user.Id, user.Email);
        }

        public bool Succeeded { get; protected set; }

        public IEnumerable<IdentityError> Errors { get; }

        public UserViewModel User { get; set; }
    }
}
