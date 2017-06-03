using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class HomeAngkutJualController : Controller
    {
        // GET: AngkutJual/HomeAngkutJual
        public ActionResult Index()
        {
            return View();
        }
    }
}