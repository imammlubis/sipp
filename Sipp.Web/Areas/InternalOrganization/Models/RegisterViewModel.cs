using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sipp.Web.Areas.InternalOrganization.Models
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Provider { get; set; } //1 google 2 facebook
    }
}