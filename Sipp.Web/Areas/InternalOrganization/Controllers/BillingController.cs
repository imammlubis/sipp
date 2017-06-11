using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Sipp.Data.Entity.Payment;
using Sipp.Service.Organization;
using Sipp.Service.Payment;
using Sipp.Web.Areas.InternalOrganization.Models;
using Sipp.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sipp.Web.Areas.InternalOrganization.Controllers
{
    public class BillingController : Controller
    {
        private ApplicationUserManager _userManager;
        private EmailServices emailService = new EmailServices();
        private ICompanyRepository companyRepository = new CompanyRepository();
        private ICompanyEmailRepository companyEmailRepository = new CompanyEmailRepository();
        private IRegularBillRepository regularBillRepository = new RegularBillRepository();
        private IBillCreditRepository billCreditRepository = new BillCreditRepository();

        [Authorize(Roles = "administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "administrator")]
        public ActionResult DaftarTagihan()
        {
            return View();
        }
        [Authorize(Roles = "administrator")]
        public ActionResult Perusahaan()
        {
            return View();
        }
        [Authorize(Roles = "administrator")]
        public ActionResult DaftarPembayaran()
        {
            return View();
        }
        [Authorize(Roles = "administrator")]
        public JsonResult LoadListCompany()
        {
            var data = from a in companyRepository.Get().ToList()
                       select new
                       {
                           a.ID,
                           a.Name
                       };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "administrator")]
        public JsonResult LoadListAllCompany([DataSourceRequest] DataSourceRequest request)
        {
            var dataCompany = from a in companyRepository.Get().AsEnumerable()
                              join b in companyEmailRepository.Get().AsEnumerable()
                              on a.ID equals b.CompanyId into group1
                              from g1 in group1.DefaultIfEmpty()
                              select new CompanyViewModel
                              {
                                  Id = a.ID,
                                  Address = a.Address,
                                  Email = g1 == null ? String.Empty : g1.Email,
                                  LegalType = a.LegalType,
                                  Name = a.Name,
                                  NPWP = a.NPWP,
                                  Province = a.Province
                              };
            DataSourceResult result = dataCompany.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "administrator")]
        public async Task<string> CreateFirstBill(RegularBill regularBill)
        {
            var userid = User.Identity.GetUserId();
            var message = "";
            if (ModelState.IsValid)
            {
                regularBill.ID = Guid.NewGuid().ToString();
                regularBill.CreatedDate = DateTime.Now;
                regularBill.CreatedBy = userid;
                var result = await regularBillRepository.AddAsync(regularBill);
                message = result.ID;
            }
            return message;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [Authorize(Roles = "administrator")]
        public async Task<string> CreateCompany(CompanyViewModel companyModel)
        {
            var userid = User.Identity.GetUserId();
            var message = "";
            if (ModelState.IsValid)
            {
                var company = await companyRepository.AddAsync(new Data.Entity.Organization.Company
                {
                    ID = Guid.NewGuid().ToString(),
                    CreatedBy = userid,
                    CreatedDate = DateTime.Now,
                    Name = companyModel.Name,
                    Province = companyModel.Province,
                    LegalType = companyModel.LegalType
                });
                var user = new Data.Entity.CoreIdentity.ApplicationUser
                {
                    FirstName = companyModel.Email.Split('@')[0],
                    UserName = companyModel.Email,
                    Email = companyModel.Email,
                    EmailConfirmed = true
                };
                var result = await UserManager.CreateAsync(user, "Pass@123");
                var companyEmail = await companyEmailRepository.AddAsync(new Data.Entity.Organization.CompanyEmail
                {
                    ID = Guid.NewGuid().ToString(),
                    CreatedBy = userid,
                    CreatedDate = DateTime.Now,
                    Email = companyModel.Email,
                    UserCompanyId = user.Id,
                    CompanyId = company.ID
                });

               

                message = "sukses";
            }
            return message;
        }

        [Authorize(Roles = "administrator")]
        public JsonResult ListDaftarPembayaran([DataSourceRequest] DataSourceRequest request)
        {
            var data = from a in billCreditRepository.Get().AsEnumerable()
                       join b in regularBillRepository.Get().AsEnumerable()
                       on a.RegularBillId equals b.ID
                       join z in companyRepository.Get().AsEnumerable()
                       on b.CompanyId equals z.ID
                       select new DaftarPembayaranViewModel
                       {
                           Id = a.ID,
                           CompanyId = z.ID,
                           RegularBillId = a.RegularBillId,
                           Amount = a.Amount,
                           CompanyName = z.Name,
                           FileValidation = a.FileValidation,
                           IsApproved = a.IsApproved,
                           ObjectionInformation = a.ObjectionInformation
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "administrator")]
        public JsonResult ListDaftarTagihan([DataSourceRequest] DataSourceRequest request)
        {
            var data = from a in regularBillRepository.Get().AsEnumerable()
                       join b in companyRepository.Get().AsEnumerable()
                       on a.CompanyId equals b.ID
                       select new DaftarTagihanViewModel
                       {
                           Id = a.ID,
                           CompanyName = b.Name,
                           Province = b.Province,
                           Evaluator = a.Evaluator,
                           YearOfCheckingPeriod = a.YearOfCheckingPeriod,
                           YearOfBillingPeriod = a.YearOfBillingPeriod,
                           FirstBillingNo = a.FirstBillingNo,
                           FirstBillingDate = a.FirstBillingDate,
                           FirstAmount = a.FirstAmount,

                           SecondBillingNo = a.SecondBillingNo,
                           SecondBillingDate = a.SecondBillingDate,
                           SecondAmount = a.SecondAmount,

                           ThirdBillingNo = a.ThirdBillingNo,
                           ThirdBillingDate = a.ThirdBillingDate,
                           ThirdAmount = a.ThirdAmount,

                           FourthBillingNo = a.FourthBillingNo,
                           FourthBillingDate = a.FourthBillingDate,
                           FourthAmount = a.FourthAmount
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
    }
}