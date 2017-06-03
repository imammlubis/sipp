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
        public string Evaluator { get; set; }//Pemeriksa
        public int YearOfCheckingPeriod { get; set; }//Periode Pemeriksaan
        public int YearOfBillingPeriod { get; set; }//Tahun Penagihan
        public int BiilingSeq { get; set; }//Penagihan Ke
        public string BillingNo { get; set; }//No Surat Tagihan
        public Nullable<DateTime> BillingDate { get; set; }//Tanggal Tagihan
        public string BillingType { get; set; }//IT, R, PHT//Tipe Tagihan
        public Nullable<double> Amount { get; set; }//Nominal Tagihan

        public Nullable<bool> IsFirstBill { get; set; }
        public string ObjectionInformation { get; set; }
        public string FileValidation { get; set; }
        public Nullable<bool> IsApproved { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
