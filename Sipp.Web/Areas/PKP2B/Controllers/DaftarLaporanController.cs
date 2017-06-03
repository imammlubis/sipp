using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.PKP2B.Controllers
{
    [Authorize(Roles = "PKP2BAdmin")]
    public class DaftarLaporanController : Controller
    {
        // GET: PKP2B/DaftarLaporan
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Expand()
        {
            return View();
        }

        public ActionResult Respond()
        {
            return View();
        }
    }
}