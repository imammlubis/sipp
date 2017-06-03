using Sipp.Service.Organization;
using Sipp.Service.Payment;
using Sipp.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipp.Web.Areas.InternalOrganization.Controllers
{
    public class DashboardController : Controller
    {
        private EmailServices emailService = new EmailServices();
        private ICompanyRepository companyRepository = new CompanyRepository();
        private IRegularBillRepository regularBillRepository = new RegularBillRepository();
        private IBillCreditRepository billCreditRepository = new BillCreditRepository();

        [Authorize(Roles = "administrator")]
        public ActionResult Index()
        {
            return View();
        }
    }
}