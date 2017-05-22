using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipp.Website.Areas.Payment.Controllers
{
    public class BillingController : Controller
    {
        // GET: Payment/Billing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BillingList()
        {
            return View();
        }
    }
}