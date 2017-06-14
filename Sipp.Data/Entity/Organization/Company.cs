using Sipp.Data.Entity.CoreIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.Organization
{
    [Table("Company", Schema = "organization")]
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string NPWP { get; set; }
        public string LegalType { get; set; } //IUPMINERAL, IUPBATUBARA, PKP2B, KK
        public string Address { get; set; }
        public string Province { get; set; }
        public Nullable<bool> IsVisible { get; set; }

        [ForeignKey("UserCompany")]
        public string UserCompanyId { get; set; }
        public virtual ApplicationUser UserCompany { get; set; }
    }
}
