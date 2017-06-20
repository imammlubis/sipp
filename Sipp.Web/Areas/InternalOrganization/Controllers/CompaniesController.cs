using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Sipp.Data.Entity.Payment;
using Sipp.Service.Organization;
using Sipp.Service.Payment;
using Sipp.Web.Areas.InternalOrganization.Models;
using Sipp.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sipp.Web.Areas.InternalOrganization.Controllers
{
    public class CompaniesController : Controller
    {
        private EmailServices emailService = new EmailServices();
        private ICompanyRepository companyRepository = new CompanyRepository();
        private ICompanyEmailRepository companyEmailRepository = new CompanyEmailRepository();
        private IRegularBillRepository regularBillRepository = new RegularBillRepository();
        private IBillCreditRepository billCreditRepository = new BillCreditRepository();

        [Authorize(Roles = "company")]
        public ActionResult Index()
        {
            double? jumlahPiutang = 0;
            var userid = User.Identity.GetUserId();
            var userCompany = companyEmailRepository.Get().Where(c => c.UserCompanyId == userid).SingleOrDefault();
            var regularBill = regularBillRepository.Get().Where(c => c.CompanyId == userCompany.CompanyId).SingleOrDefault();
            var cicilan = billCreditRepository.Get().Where(c => c.CreatedBy == userid).Sum(c=>c.Amount);
            cicilan = cicilan == null ? 0 : cicilan;
            jumlahPiutang = regularBill.FirstAmount - cicilan;
            ViewBag.jumlahPiutang = jumlahPiutang;
            return View();
        }
        [Authorize(Roles = "company")]
        public ActionResult TransactionHistory()
        {
            double? jumlahPiutang = 0;
            var userid = User.Identity.GetUserId();
            var userCompany = companyEmailRepository.Get().Where(c => c.UserCompanyId == userid).SingleOrDefault();
            var regularBill = regularBillRepository.Get().Where(c => c.CompanyId == userCompany.CompanyId).SingleOrDefault();
            var cicilan = billCreditRepository.Get().Where(c => c.RegularBillId == regularBill.ID).Sum(c => c.Amount);
            cicilan = cicilan == null ? 0 : cicilan;
            jumlahPiutang = regularBill.FirstAmount - cicilan;
            ViewBag.jumlahPiutang = jumlahPiutang;
            return View();
        }
        public JsonResult ListHistoryTransaction([DataSourceRequest] DataSourceRequest request)
        {
            var userid = User.Identity.GetUserId();
            var data = from a in billCreditRepository.Get().AsEnumerable()
                       where a.CreatedBy == userid
                       //join b in regularBillRepository.Get().AsEnumerable()
                       //on a.RegularBillId equals b.ID into group1
                       //from g1 in group1.DefaultIfEmpty()
                       //join z in companyRepository.Get().AsEnumerable()
                       //on g1.CompanyId equals z.ID into group2
                       //from g2 in group2.DefaultIfEmpty()
                       //where g1.CreatedBy == userid
                       select new DaftarPembayaranViewModel
                       {
                           Id = a.ID,
                           //CompanyId = g2 == null ? String.Empty : g2.ID,
                           //RegularBillId = a.RegularBillId,
                           Amount = a.Amount,
                           //CompanyName = g2 == null ? String.Empty : g2.Name,
                           FileValidation = a.FileValidation,
                           //IsApproved = a.IsApproved,
                           CreatedDate = a.CreatedDate,
                           ObjectionInformation = a.ObjectionInformation
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "company")]
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file, BillCredit billCredit, FormCollection fc)
        {
            var userid = User.Identity.GetUserId();
            var message = "";
            var compId = companyEmailRepository.Get().Where(c => c.UserCompanyId == userid).SingleOrDefault();
            try
            {
                if (file.ContentLength > 0)
                {
                    billCredit.ID = Guid.NewGuid().ToString();
                    //string _FileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Documents/ImageApprovement"), billCredit.ID+extension);
                    file.SaveAs(_path);
                    
                    billCredit.Amount = double.Parse(fc["nominal"].ToString().Replace(".", ""));
                    billCredit.CreatedBy = userid;
                    billCredit.CreatedDate = DateTime.Now;
                    billCredit.FileValidation = billCredit.ID + extension;
                    billCredit.ObjectionInformation = fc["keterangan"];
                    //billCredit.IsApproved = null;
                    billCredit.CompanyId = compId.CompanyId;
                    var result = await billCreditRepository.AddAsync(billCredit);
                    message = "success";
                }
                ViewBag.Message = "File Uploaded Successfully!!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed!!";
                message = "failed | " + ex;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<string> CreateTransaction(HttpPostedFileBase file, BillCredit billCredit)
        {
            var userid = User.Identity.GetUserId();
            var message = "";
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Documents/ImageApprovement"), _FileName);
                    file.SaveAs(_path);

                    billCredit.ID = Guid.NewGuid().ToString();
                    billCredit.Amount = billCredit.Amount;
                    billCredit.CreatedBy = userid;
                    billCredit.CreatedDate = DateTime.Now;
                    billCredit.FileValidation = _FileName;
                    billCredit.ObjectionInformation = billCredit.ObjectionInformation;
                    billCredit.IsApproved = null;
                    var result = await billCreditRepository.AddAsync(billCredit);
                    message = "success";
                }
                ViewBag.Message = "File Uploaded Successfully!!";                
            }
            catch(Exception ex)
            {
                ViewBag.Message = "File upload failed!!";
                message = "failed | "+ex;
            }
            return message;
        }
    }
}