using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdm.Web.Areas.PKP2B.Models
{
    public class HistoryViewModel
    {
        public string IdCompanyHistoryPkp2b { get; set; }
        public string Name { get; set; }
        public string NPWP { get; set; }
        public string Telphone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string CpName { get; set; }
        public string CpMobileNo { get; set; }
        public string CompanyID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}