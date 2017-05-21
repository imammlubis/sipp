using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.CoreIdentity
{
    public class ApplicationRole : IdentityRole
    {
        public string CustomRoleName { get; set; }
    }
}
