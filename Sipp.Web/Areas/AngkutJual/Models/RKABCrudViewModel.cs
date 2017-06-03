using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdm.Web.Areas.AngkutJual.Models
{
    public class RKABCrudViewModel
    {
        public string ID { get; set; }
        public string LetterNumber { get; set; }
        public DateTime LetterDate { get; set; }
        public int RKABYear { get; set; }
        public int MyProperty { get; set; }
    }
}