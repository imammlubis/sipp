using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.ReportCompany.Controllers
{
    [Authorize(Roles = "AngkutJualCompany")]
    public class ReportFormController : Controller
    {
        // GET: ReportCompany/ReportForm
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Rkab()
        {
            return View();
        }

        public ActionResult Triwulan()
        {
            return View();
        }

        public ActionResult Tahunan()
        {
            return View();
        }

    }
}