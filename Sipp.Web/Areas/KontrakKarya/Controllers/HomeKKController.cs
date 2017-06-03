using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.KontrakKarya.Controllers
{
    [Authorize(Roles = "KKAdmin")]
    public class HomeKKController : Controller
    {
        // GET: KontrakKarya/HomeKK
        public ActionResult Index()
        {
            return View();
        }
    }
}