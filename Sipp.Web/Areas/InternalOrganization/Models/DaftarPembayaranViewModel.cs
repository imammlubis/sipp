using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sipp.Web.Areas.InternalOrganization.Models
{
    public class DaftarPembayaranViewModel
    {
        public string Id { get; set; }
        public Nullable<double> Amount { get; set; }
        public string FileValidation { get; set; }
        public string ObjectionInformation { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string RegularBillId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
    }
}