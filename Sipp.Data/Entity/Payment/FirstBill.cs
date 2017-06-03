using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.Payment
{
    [Table("FirstBill", Schema = "payment")]
    public class FirstBill
    {
        public string Evaluator { get; set; }
        public int YearOfCheckingPeriod { get; set; }//Periode Pemeriksaan
        public int YearOfBillingPeriod { get; set; }//Tahun Penagihan
        public string FirstBillingNo { get; set; }//No Surat Tagihan
        public Nullable<DateTime> FirstBillingDate { get; set; }//Tanggal Tagihan
        public string BillingType { get; set; }//IT, R, PHT//Tipe Tagihan
        public Nullable<double> Amount { get; set; }

        public string ObjectionInformation { get; set; }
        public string FileValidation { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
