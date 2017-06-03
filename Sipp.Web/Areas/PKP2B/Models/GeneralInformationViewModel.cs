using EduSpot.Entity.Tables.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdm.Web.Areas.PKP2B.Models
{
    public class GeneralInformationViewModel
    {
        public string ID { get; set; }
        public string SkEndId { get; set; }
        public string IdCompanyHistoryPkp2b { get; set; }
        public string Name { get; set; }
        public string Npwp { get; set; }
        public string Telp { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Alamat { get; set; }
        public string Website { get; set; }
        public string CpNama { get; set; }
        public string CpHp { get; set; }
        
        public string Komoditi { get; set; }
        public string Lainnya { get; set; }

        public string Generasi { get; set; }
        public string KodeWilayah { get; set; }
        public string NoKontrak { get; set; }
        public Nullable<DateTime> TanggalKontrak { get; set; }
        public String TanggalKontrak2 { get; set; }
        public int LuasWilayahAwal { get; set; }
        public int LuasWilayahDipertahankan { get; set; }
        public string Provinsi { get; set; }
        public string Kabupaten { get; set; }
        public string Perizinan { get; set; }
        public string TahapanAkhir { get; set; }
        public Nullable<DateTime> TanggalBerakhir { get; set; }
        public int JangkaWaktu { get; set; }
        public string JangkaWaktu2 { get; set; }
        public string CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}