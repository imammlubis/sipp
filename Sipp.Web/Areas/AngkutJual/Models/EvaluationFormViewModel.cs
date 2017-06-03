using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Models
{
    public class EvaluationFormViewModel
    {
        private ICompanyRepository companyRepository = new CompanyRepository();
        public string ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }


        public string Address { get; set; }
        public string CurrencyType { get; set; }
        public string TelNumber { get; set; }
        public string AdditionalInformation { get; set; }//AdditionalInformation
        public string Email { get; set; }
        public string CoalSourceSk { get; set; }
        
        public string SkNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> SkDate { get; set; }
        public string SkDate2 { get; set; }
        public string CompanySource { get; set; }
        public string CompanySourceAddress { get; set; }
        public string Province { get; set; }
        public string CompanyId { get; set; }
        public string CompanyIds { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string NoIntroductoryLetter { get; set; }
        public string NoDisposition { get; set; }//No_Disposisi
        public string ReportSubmittedDate { get; set; }//Tgl_Pengumpulan_Laporan
        [Required]
        public string CoalOrigin { get; set; }//Sumber_Batubara
        [Required]
        public string EndUser { get; set; }
        [Required]
        public string Tonnage { get; set; }
        public string ActivityPlan { get; set; }//Rencana_kegiatan
        public string ActivityRealization { get; set; }//Realisasi_kegiatan
        public Nullable<bool> AcceptanceStatus { get; set; }//Status Penerimaan
        public string ReportType { get; set; }//Jenis Laporan
        public Nullable<int> Revenue { get; set; }
        public Nullable<double> BasicPrice { get; set; }//Harga_pokok
        public Nullable<double> ProfitBefore { get; set; } //Laba_sebelum
        public Nullable<double> OrganizationTax { get; set; } //Pajak_badan
        public Nullable<double> Pph { get; set; } //Pph
        public Nullable<double> Profit { get; set; } //Laba
        public Nullable<double> RevenueUSD { get; set; } //        Revenue_USD
        public Nullable<double> BasicPriceUSD { get; set; } //Harga_pokok_USD
        public Nullable<double> ProfitBeforeUSD { get; set; } //Laba_sebelum_USD
        public Nullable<double> OrganizationTaxUSD { get; set; } //Pajak_badan_USD
        public Nullable<double> PphUSD { get; set; } //Pph_USD
        public Nullable<double> ProfitUSD { get; set; } //Laba_USD

        private IEnumerable<SelectListItem> GetRoles()
        {
            var dbUserRoles = new Company();
            //var roles = (from a in dbUserRoles
            //            select new SelectListItem {
            //                Value = a.Name,
            //                Text = a.ID
            //            });
            var roles = dbUserRoles
                .ID
                .Select(x =>
                        new SelectListItem
                        {
                            Value = ID.ToString(),
                            Text = Name
                        });


            return new SelectList(roles, "Value", "Text");
        }
    }

    
}