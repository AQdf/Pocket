﻿using System.ComponentModel.DataAnnotations;

namespace Sho.Pocket.Auth.IdentityServer.Models
{
    public class UserCreateModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PrimaryCurrency { get; set; }
    }
}
