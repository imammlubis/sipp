using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.Payment
{
    [Table("RegularPayment", Schema = "payment")]
    public class RegularPayment : BaseEntity
    {
        public string Province { get; set; }
        public string FirstBillingNo { get; set; }
        public int YearOfCheckingPeriod { get; set; }
        public int YearOfBillingPeriod { get; set; }
        public Nullable<DateTime> FirstBillingDate { get; set; }
        public string LegalType { get; set; } //IUPMINERAL, IUPBATUBARA, PKP2B, KK
        public string BillingType { get; set; }//IT, R, PHT
        public Nullable<bool> IsFirstBill { get; set; }
        public string ObjectionInformation { get; set; }
        public Nullable<double> Amount { get; set; }
        public string FileValidation { get; set; }
        public Nullable<bool> IsApproved { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
