using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sipp.Web.Areas.InternalOrganization.Models
{
    public class CompanyViewModel
    {
        public string Name { get; set; }
        public string NPWP { get; set; }
        public string LegalType { get; set; } //IUPMINERAL, IUPBATUBARA, PKP2B, KK
        public string Address { get; set; }
        public string Province { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
    }
}