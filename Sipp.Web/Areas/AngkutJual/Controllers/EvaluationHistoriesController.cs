using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EduSpot.Core.Infrastructure;
using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Kendo.Mvc.UI;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using Kendo.Mvc.Extensions;
using System.Threading.Tasks;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class EvaluationHistoriesController : Controller
    {
        private IEvaluationRepository evaluationRepository = new EvaluationRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();

        private IFirstSkRepository firstSkRepo = new FirstSkRepository();
        private IFirstSkSourceRepository firstSkSourceRepo = new FirstSkSourceRepository();
        private IExtendedSkRepository extendedSkRepository = new ExtendedSkRepository();
        private IExtendedSkSourceRepository extendedSkSourceRepository = new ExtendedSkSourceRepository();

        // GET: AngkutJual/EvaluationHistories
        public ActionResult Index()
        {
            //var dataGrid = from a in companyRepository.GetAll().AsEnumerable()
            //               join b in evaluationRepository.GetAll().AsEnumerable()
            //               on a.ID equals b.CompanyID
            //               select new EvaluationHistoryViewModel
            //               {
            //                   Name = a.Name,
            //                   ArrangementDate = DateTime.Parse(b.ReportSubmittedDate),
            //                   AcceptanceStatus = b.AcceptanceStatus == true ? "Diterima" : "Tidak Diterima",
            //                   ReportType = b.ReportType,
            //                   NoIntroductoryLetter = b.NoIntroductoryLetter,
            //                   Comment = b.AdditionalInformation,
            //                   FileDownload = b.SkFile
            //               };
            //return View(dataGrid);
            var evaluation = evaluationRepository.GetAll();
            return View(evaluation);
        }
        [HttpPost]
        public JsonResult ListKesesuaian([DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in firstSkRepo.GetAll().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable() on a.CompanyID equals b.ID
                           join c in firstSkSourceRepo.GetAll().AsEnumerable() on a.ID equals c.FirstSkID
                           where a.SkDate <= DateTime.Now && DateTime.Now <=a.SkEndDate           
                           select new KesesuaianViewModel
                           {
                               ID = a.ID,
                               CompanyName = b.Name,
                               SumberBatubara1 = c.CompanyName,
                               Volume1 = int.Parse(c.Volume),
                               SumberBatubara2 = "",
                               Volume2 = ""
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in companyRepository.GetAll().AsEnumerable()
                           join b in evaluationRepository.GetAll().AsEnumerable()
                           on a.ID equals b.CompanyID
                           select new EvaluationHistoryViewModel
                           {
                               ID = b.ID,
                               Name = a.Name,
                               ArrangementDate = b.ReportSubmittedDate,
                               CreatedBy = b.CreatedBy,
                               //AcceptanceStatus = b.AcceptanceStatus == true ? "Diterima" : "Tidak Diterima",
                               AcceptanceStatus = b.AcceptanceStatus.ToString(),
                               ReportType = b.ReportType,
                               NoIntroductoryLetter = b.NoIntroductoryLetter,
                               Comment = b.AdditionalInformation,
                               FileDownload = b.SkFile
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult List2([DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in companyRepository.GetAll().AsEnumerable()
                           join b in evaluationRepository.GetAll().AsEnumerable()
                           on a.ID equals b.CompanyID
                           where b.AcceptanceStatus == false
                           select new EvaluationHistoryViewModel
                           {
                               ID = b.ID,
                               Name = a.Name,
                               CreatedBy = b.CreatedBy,
                               ArrangementDate = b.ReportSubmittedDate,
                               //AcceptanceStatus = b.AcceptanceStatus == true ? "Diterima" : "Tidak Diterima",
                               AcceptanceStatus = b.AcceptanceStatus.ToString(),
                               ReportType = b.ReportType,
                               NoIntroductoryLetter = b.NoIntroductoryLetter,
                               Comment = b.AdditionalInformation,
                               FileDownload = b.SkFile
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            Evaluation evaluation = await evaluationRepository.FindAsync(p);
            await evaluationRepository.RemoveAsync(evaluation);
            return Json(p);
            //return RedirectToAction("Index");
        }

        //public ActionResult Pdf_Export_Read([DataSourceRequest]DataSourceRequest request)
        //{
        //    var dataGrid = from a in companyRepository.GetAll().AsEnumerable()
        //                   join b in evaluationRepository.GetAll().AsEnumerable()
        //                   on a.ID equals b.CompanyID
        //                   select new EvaluationHistoryViewModel
        //                   {
        //                       ID = b.ID,
        //                       Name = a.Name,
        //                       ArrangementDate = b.ReportSubmittedDate,
        //                       AcceptanceStatus = b.AcceptanceStatus == true ? "Diterima" : "Tidak Diterima",
        //                       ReportType = b.ReportType,
        //                       NoIntroductoryLetter = b.NoIntroductoryLetter,
        //                       Comment = b.AdditionalInformation,
        //                       FileDownload = b.SkFile
        //                   };
        //    DataSourceResult result = dataGrid.ToDataSourceResult(request);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        // GET: AngkutJual/EvaluationHistories/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Evaluation evaluation = db.Evaluation.Find(id);
        //    if (evaluation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(evaluation);
        //}

        // GET: AngkutJual/EvaluationHistories/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
        //    return View();
        //}

        //// POST: AngkutJual/EvaluationHistories/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,NoIntroductoryLetter,NoDisposition,ReportSubmittedDate,CoalOrigin,EndUser,Tonnage,ActivityPlan,ActivityRealization,Revenue,BasicPrice,ProfitBefore,OrganizationTax,Pph,Profit,RevenueUSD,BasicPriceUSD,ProfitBeforeUSD,OrganizationTaxUSD,PphUSD,ProfitUSD,SkFile,AdditionalInformation,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        //Evaluation evaluation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Evaluation.Add(evaluation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", evaluation.CompanyID);
        //    return View(evaluation);
        //}

        //// GET: AngkutJual/EvaluationHistories/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Evaluation evaluation = db.Evaluation.Find(id);
        //    if (evaluation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", evaluation.CompanyID);
        //    return View(evaluation);
        //}

        //// POST: AngkutJual/EvaluationHistories/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,NoIntroductoryLetter,NoDisposition,ReportSubmittedDate,CoalOrigin,EndUser,Tonnage,ActivityPlan,ActivityRealization,Revenue,BasicPrice,ProfitBefore,OrganizationTax,Pph,Profit,RevenueUSD,BasicPriceUSD,ProfitBeforeUSD,OrganizationTaxUSD,PphUSD,ProfitUSD,SkFile,AdditionalInformation,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Evaluation evaluation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(evaluation).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", evaluation.CompanyID);
        //    return View(evaluation);
        //}

        //// GET: AngkutJual/EvaluationHistories/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Evaluation evaluation = db.Evaluation.Find(id);
        //    if (evaluation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(evaluation);
        //}

        //// POST: AngkutJual/EvaluationHistories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Evaluation evaluation = db.Evaluation.Find(id);
        //    db.Evaluation.Remove(evaluation);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public class EvaluationHistoryViewModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public Nullable<DateTime> ArrangementDate { get; set; }
            public Boolean Status { get; set; }
            public string AcceptanceStatus { get; set; }

            public string ReportType { get; set; }
            public string NoIntroductoryLetter { get; set; }
            public string CreatedBy { get; set; }
            public string EvaluatorName { get; set; }
            public string Comment { get; set; }
            public string FileDownload { get; set; }
            public bool ConformityStatus { get; set; }
        }

        public class KesesuaianViewModel
        {
            public string ID { get; set; }
            public string CompanyId { get; set; }
            public string CompanyName { get; set; }
            public string SumberBatubara1 { get; set; }
            public int Volume1 { get; set; }
            public string SumberBatubara2 { get; set; }
            public string Volume2 { get; set; }
        }


    }

    
}
