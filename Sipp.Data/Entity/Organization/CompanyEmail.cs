using Sipp.Data.Entity.CoreIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.Organization
{
    [Table("CompanyEmail", Schema = "organization")]
    public class CompanyEmail : BaseEntity
    {
        public string Email { get; set; }
        [ForeignKey("UserCompany")]
        public string UserCompanyId { get; set; }
        public virtual ApplicationUser UserCompany { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
