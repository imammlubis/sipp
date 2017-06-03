using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.KontrakKarya.Controllers
{
    public class DaftarLaporanController : Controller
    {
        // GET: KontrakKarya/DaftarLaporan
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Respond()
        {
            return View();
        }
    }
}