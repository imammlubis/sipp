using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Microsoft.Reporting.WebForms;
using System.IO;
using EduSpot.Entity.Tables.AngkutJual;
using System.Threading.Tasks;
using Esdm.Web.Utils;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    
    public class NotificationViewModel
    {
        public Nullable<int> resp { get; set; }
        public string CompanyName { get; set; }
        public string DestinationEmail { get; set; }
        public string DestinationMobileNo { get; set; }
        public string AdditionalInfo { get; set; }
    }
    [Authorize(Roles = "AngkutJualAdmin")]

    public class RKABRealizationsController : Controller
    {
        private ICompanyRepository companyRepository = new CompanyRepository();
        private IRKABRepository rkabRepository = new RKABRepository();
        private IYearlyRepository yearlyRepository = new YearlyRepository();
        private IQuarterlyRepository quarterlyRepository = new QuarterlyRepository();
        private IReportRepository reportRepo = new ReportRepository();

        private EmailServices emailService = new EmailServices();
        private Esdm.Web.Utils.SmsService smsService = new Esdm.Web.Utils.SmsService();


        // GET: AngkutJual/RKABRealizations
        public ActionResult Index()
        {
            return View();
        }



        public async Task<string> SendNotification(NotificationViewModel model)
        {

            //send sms
            var smsresult = smsService.SendSms(new SmsContent()
            {
                To = model.DestinationMobileNo,
                Body = model.resp == 1 ? model.CompanyName + ": Form Diterima. Catatan:" + model.AdditionalInfo :
                model.CompanyName + ": Form Ditolak." + " Catatan:" + model.AdditionalInfo
            });


            //send email

            var emailbody = System.IO.File.ReadAllText(Server.MapPath("~/Utils/htmls/templateemailresponse.html"));
            emailbody = emailbody.Replace("[NamaPerusahaan]", model.CompanyName)
                .Replace("[Email]", model.DestinationEmail)
                .Replace("[NoHandphone]", model.DestinationMobileNo)
                 .Replace("[Keterangan]", model.AdditionalInfo);

            List<string> dests = new List<string>();

            List<string> ccs = new List<string>();


            var subject = "Respon" + " untuk " + model.CompanyName;


            dests.Add(model.DestinationEmail);

            ccs.Add("mahendra@eduspot.co.id");

            var emailresult = emailService.SendAsyncDefault(new EmailContent
            {
                Subject = subject,
                Destination = dests,
                CC = ccs,
                Body = emailbody
            });


            return "OK";
        }


        public ActionResult ListAll()
        {

            //var data = from a in 
            return View();
        }

        public JsonResult ListRKAB([DataSourceRequest] DataSourceRequest request)
        {

            var companies = companyRepository.GetAll().AsEnumerable();
            var rkab = rkabRepository.GetAll().AsEnumerable();

            var data = from a in companies.AsEnumerable()
                       join b in rkab.AsEnumerable() on a.ID equals b.CompanyID
                       select new RKABViewModel
                       {
                           ID = b.ID,
                           CompanyName = a.Name,
                           Status = Convert.ToBoolean(b.Status) == false ? "Belum Lapor" : "Sudah Lapor",
                           Year = Convert.ToInt16(b.RkabYear)
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListTahunan([DataSourceRequest] DataSourceRequest request)
        {
            var companies = companyRepository.GetAll().AsEnumerable();
            var yearly = yearlyRepository.GetAll().AsEnumerable();
            var data = from a in companies.AsEnumerable()
                       join b in yearly.AsEnumerable() on a.ID equals b.CompanyID
                       select new YearlyViewModel
                       {
                           ID = b.ID,
                           CompanyName = a.Name,
                           Status = Convert.ToBoolean(b.Status) == false ? "No" : "Yes",
                           Year = Convert.ToInt16(b.Year)
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListReport([DataSourceRequest] DataSourceRequest request)
        {
            var companies = companyRepository.GetAll().AsEnumerable();
            var report = reportRepo.GetAll().AsEnumerable();
            var data = from r in report.AsEnumerable()
                       join c in companies.AsEnumerable() on r.CompanyID equals c.ID into group1
                       from g1 in group1
                       select new ReportViewModel
                       {
                           CompanyId = g1.ID.ToString(),
                           CompanyName = g1.Name,
                           Year = r.Year,
                           Rkab = r.Rkab,
                           StatusRkab = String.IsNullOrEmpty(r.StatusRkab) ? "Not Confirmed" : r.StatusRkab,
                           Q1 = r.Q1,
                           StatusQ1 = String.IsNullOrEmpty(r.StatusQ1) ? "Not Confirmed" : r.StatusQ1,
                           PersenQ1 = Math.Round(((decimal)r.Q1 * 100 / (decimal)r.Rkab), 2), //((c.Q1 * 100) / c.Rkab).ToString("0.00"),
                           Q2 = r.Q2,
                           StatusQ2 = String.IsNullOrEmpty(r.StatusQ2) ? "Not Confirmed" : r.StatusQ2,
                           PersenQ2 = Math.Round(((decimal)r.Q2 * 100 / (decimal)r.Rkab), 2),
                           Q3 = r.Q3,
                           StatusQ3 = String.IsNullOrEmpty(r.StatusQ3) ? "Not Confirmed" : r.StatusQ3,
                           PersenQ3 = Math.Round(((decimal)r.Q3 * 100 / (decimal)r.Rkab), 2),
                           Q4 = r.Q4,
                           StatusQ4 = String.IsNullOrEmpty(r.StatusQ4) ? "Not Confirmed" : r.StatusQ4,
                           PersenQ4 = Math.Round(((decimal)r.Q4 * 100 / (decimal)r.Rkab), 2),
                           Annual = r.Annual,
                           StatusAnnual = String.IsNullOrEmpty(r.StatusAnnual) ? "Not Confirmed" : r.StatusAnnual,
                           PersenAnnual = Math.Round(((decimal)r.Annual * 100 / (decimal)r.Rkab), 2),
                           StatusRespond = r.StatusRespond
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListTriwulan([DataSourceRequest] DataSourceRequest request)
        {
            var companies = companyRepository.GetAll().AsEnumerable();
            var quarterly = quarterlyRepository.GetAll().AsEnumerable();

            var data = from a in companies.AsEnumerable()
                       join b in quarterly.AsEnumerable() on a.ID equals b.CompanyID
                       orderby b.Period ascending
                       select new QuarterlyViewModel
                       {
                           ID = b.ID,
                           CompanyName = a.Name,
                           Status = Convert.ToBoolean(b.Status) == false ? "Belum Lapor" : "Sudah Lapor",
                           Period = Convert.ToInt16(b.Period),
                           Year = Convert.ToInt16(b.Year)
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: AngkutJual/RKABRealizations/Details/5
        public ActionResult DetailRkab(string id)
        {
            var list = new SelectList(new[]
            {
                new { ID = "1", Name = ".pdf" },
                new { ID = "2", Name = ".xls" }
            },
            "ID", "Name", 1);

            ViewData["listPrint"] = list;
            return View();
        }

        public ActionResult TestPrintRDLC()
        {
            return View();
        }

        public ActionResult Report(string id)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "ReportRKAB.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            List<RKAB> cm = new List<RKAB>();
            cm = rkabRepository.GetAll().ToList();
            ReportDataSource rd = new ReportDataSource("DataSet1", cm);
            lr.DataSources.Add(rd);
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }

        // GET: AngkutJual/RKABRealizations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AngkutJual/RKABRealizations/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AngkutJual/RKABRealizations/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AngkutJual/RKABRealizations/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AngkutJual/RKABRealizations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AngkutJual/RKABRealizations/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
    public class RKABViewModel
    {
        public string ID { get; set; }
        public string CompanyName { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
    }
    public class ReportViewModel2
    {
        public string ID { get; set; }
        public string CompanyName { get; set; }
        public decimal PersenQ1 { get; set; }
        public decimal PersenQ2 { get; set; }
        public decimal PersenQ3 { get; set; }
        public decimal PersenQ4 { get; set; }
        public decimal PersenAnnual { get; set; }
        public string CompanyID { get; set; }
        public int Year { get; set; }
        public string StatusTegur { get; set; }
        public int Rkab { get; set; }
        public int Q1 { get; set; }
        public int Q2 { get; set; }
        public int Q3 { get; set; }
        public int Q4 { get; set; }
        public int Annual { get; set; }
    }
    public class ReportViewModel
    {
        public string ID { get; set; }
        public string CompanyName { get; set; }

        public decimal PersenQ1 { get; set; }
        public decimal PersenQ2 { get; set; }
        public decimal PersenQ3 { get; set; }
        public decimal PersenQ4 { get; set; }
        public decimal PersenAnnual { get; set; }

        public string CompanyId { get; set; }
        public int Year { get; set; }
        public string StatusTegur { get; set; }
        public string StatusRespond { get; set; }
        public int Rkab { get; set; }
        public string StatusRkab { get; set; }
        public int Q1 { get; set; }
        public string StatusQ1 { get; set; }
        public int Q2 { get; set; }
        public string StatusQ2 { get; set; }
        public int Q3 { get; set; }
        public string StatusQ3 { get; set; }
        public int Q4 { get; set; }
        public string StatusQ4 { get; set; }
        public int Annual { get; set; }
        public string StatusAnnual { get; set; }
    }
    public class YearlyViewModel
    {
        public string ID { get; set; }
        public string CompanyName { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
    }
    public class QuarterlyViewModel
    {
        public string ID { get; set; }
        public string CompanyName { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public int Period { get; set; }
    }

}
