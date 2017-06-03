using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdm.Web.Areas.PKP2B.Models
{
    public class TahapKegiatanViewModel
    {
        public string IDTahapKegiatan { get; set; }
        public string Tahap { get; set; }
        public int LuasDipertahankan { get; set; }
        public string CompanyID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}