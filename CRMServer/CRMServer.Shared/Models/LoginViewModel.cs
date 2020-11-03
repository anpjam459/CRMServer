using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRMServer.Shared.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(256)]
        public string UserName { set; get; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Password { set; get; }
    }
}
