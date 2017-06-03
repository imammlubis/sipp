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
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class AdjustedSksController : Controller
    {
        private IAdjustedSkRepository adjustedSkRepository = new AdjustedSkRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();


        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var data = await adjustedSkRepository.FindAsync(id);
            await adjustedSkRepository.RemoveAsync(data);
            return "OK";
        }
        // GET: AngkutJual/AdjustedSks
        public ActionResult Index(string name)
        {
            //var data = adjustedSkRepository.FindByName(name);
            //return View(data);
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<AdjustedSk> dataGrid = adjustedSkRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: AngkutJual/AdjustedSks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AdjustedSk adjustedSk = db.AdjustedSk.Find(id);
        //    if (adjustedSk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(adjustedSk);
        //}

        // GET: AngkutJual/AdjustedSks/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name");
            return View();
        }

        // POST: AngkutJual/AdjustedSks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "ID,LetterNumber,LetterDate,RpiitNumber,RpiitDate,SkNumber,SkDate,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
            AdjustedSk adjustedSk)
        {
            if (ModelState.IsValid)
            {
                adjustedSk.ID = Guid.NewGuid().ToString();
                adjustedSk.CreatedBy = User.Identity.Name;
                adjustedSk.CreatedDate = DateTime.Now;
                adjustedSk.SkFile = adjustedSk.ID + System.IO.Path.GetExtension(file.FileName).ToLower();
                adjustedSkRepository.AddAsync(adjustedSk);
                if (file != null && file.ContentLength > 0)
                {
                    try {
                        string path = Path.Combine(Server.MapPath("~"), "Documents/SkPenyesuaian", 
                            adjustedSk.ID + System.IO.Path.GetExtension(file.FileName).ToLower());
                        file.SaveAs(path);
                    }
                    catch(Exception ex) {
                        ViewBag.Message = "Error" + ex.Message.ToString();
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", adjustedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", adjustedSk.CompanyID);
            return View(adjustedSk);
        }

        [HttpPost]
        public async Task<string> CreateService(
            AdjustedSk adjustedSk)
        {
            if (ModelState.IsValid)
            {
                adjustedSk.ID = Guid.NewGuid().ToString();
                adjustedSk.CreatedBy = User.Identity.Name;
                adjustedSk.CreatedDate = DateTime.Now;
               var result = await adjustedSkRepository.AddAsync(adjustedSk);
                return result.ID;
            }
            return "-1";
        }

        // GET: AngkutJual/AdjustedSks/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustedSk adjustedSk = await adjustedSkRepository.FindAsync(id);
            if (adjustedSk == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", adjustedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", adjustedSk.CompanyID);
            return View(adjustedSk);
        }

        // POST: AngkutJual/AdjustedSks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, [Bind(Include = "ID,LetterNumber,LetterDate,RpiitNumber,RpiitDate,SkNumber,SkDate,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] AdjustedSk adjustedSk)
        {
            if (ModelState.IsValid)
            {
                adjustedSk.ModifiedBy = User.Identity.Name;
                adjustedSk.ModifiedDate = DateTime.Now;

                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~"), "Documents/SkPenyesuaian",
                            adjustedSk.ID + System.IO.Path.GetExtension(file.FileName).ToLower());
                        file.SaveAs(path);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Error" + ex.Message.ToString();
                    }
                }
                adjustedSk.SkFile = adjustedSk.ID + System.IO.Path.GetExtension(file.FileName).ToLower();
                adjustedSkRepository.UpdateAsync(adjustedSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", adjustedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", adjustedSk.CompanyID);
            return View(adjustedSk);
        }


        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            AdjustedSk adjustedSk = await adjustedSkRepository.FindAsync(p);
            await adjustedSkRepository.RemoveAsync(adjustedSk);
            return Json(p);
        }
    }
}
