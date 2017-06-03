using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.Payment
{
    [Table("RegularBill", Schema = "payment")]
    public class RegularBill : BaseEntity
    {
        public string Evaluator { get; set; }//Pemeriksa
        public int YearOfCheckingPeriod { get; set; }//Periode Pemeriksaan
        public int YearOfBillingPeriod { get; set; }//Tahun Penagihan
        public string FirstBillingNo { get; set; }//No Surat Tagihan I
        public Nullable<DateTime> FirstBillingDate { get; set; }//Tanggal Tagihan I
        public Nullable<double> FirstAmount { get; set; }//Nominal Tagihan I
        public string SecondBillingNo { get; set; }//No Surat Tagihan II
        public Nullable<DateTime> SecondBillingDate { get; set; }//Tanggal Tagihan II
        public Nullable<double> SecondAmount { get; set; }//Nominal Tagihan I
        public string ThirdBillingNo { get; set; }//No Surat Tagihan III
        public Nullable<DateTime> ThirdBillingDate { get; set; }//Tanggal Tagihan III
        public Nullable<double> ThirdAmount { get; set; }//Nominal Tagihan III
        public string FourthBillingNo { get; set; }//No Surat Tagihan IV
        public Nullable<DateTime> FourthBillingDate { get; set; }//Tanggal Tagihan IV
        public Nullable<double> FourthAmount { get; set; }//Nominal Tagihan IV
        public string BillingType { get; set; }//IT, R, PHT//Tipe Tagihan

        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
