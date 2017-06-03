using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipp.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("administrator"))
                return RedirectToAction("Index", "Dashboard", new { area = "InternalOrganization" });

            else if (User.IsInRole("company"))
                return RedirectToAction("Index", "Companies", new { area = "InternalOrganization" });

            //else if (User.IsInRole("AngkutJualMineral"))
            //    return RedirectToAction("Index", "HomeAngkutJualMineral", new { area = "AngkutJualMineral" });

            //else if (User.IsInRole("PKP2BAdmin"))
            //    return RedirectToAction("Index", "HomePKP2B", new { area = "PKP2B" });

            //else if (User.IsInRole("KKAdmin"))
            //    return RedirectToAction("Index", "HomeKK", new { area = "KontrakKarya" });
            else
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}