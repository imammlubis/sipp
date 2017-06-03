using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.PKP2B.Controllers
{
    [Authorize(Roles = "PKP2BAdmin")]
    public class HomePKP2BController : Controller
    {
        // GET: PKP2B/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}