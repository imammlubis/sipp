using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.Payment
{
    [Table("BillCredit", Schema = "payment")]
    public class BillCredit : BaseEntity
    {
        public Nullable<double> Amount { get; set; }
        public string FileValidation { get; set; }
        public string ObjectionInformation { get; set; }        
        public Nullable<bool> IsApproved { get; set; }
        //[ForeignKey("Company")]
        //public string CompanyId { get; set; }
        //public virtual Company Company { get; set; }

        [ForeignKey("RegularBill")]
        public string RegularBillId { get; set; }
        public virtual RegularBill RegularBill { get; set; }
        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
