using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRMServer.Shared.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(256)]
        public string UserName { set; get; }

        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Password { set; get; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        [Compare("Password")]
        public string ConfirmPassword { set; get; }
    }
}
