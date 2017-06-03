using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdm.Web.Areas.AngkutJual.Models
{
    public class IupOpPrintViewModel
    {
        public string NoUrutBerkas { get; set; }
        public string NamaPerusahaan { get; set; }
        public string AlamatPerusahaan { get; set; }
        public string NoTel { get; set; }
        public string Email { get; set; }
        public string NPWP { get; set; }
        public string StatusIzin { get; set; }
        public string Keterangan { get; set; }
        public string Fax { get; set; }
        public string MobileNo { get; set; }
    }
}