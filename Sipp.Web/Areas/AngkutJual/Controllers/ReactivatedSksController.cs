using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using System.Threading.Tasks;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class ReactivatedSksController : Controller
    {
        private IReactivatedSkRepository reactivatedRepository = new ReactivatedSkRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();
        // GET: AngkutJual/ReactivatedSks
        public ActionResult Index(string name)
        {
            //var data = reactivatedRepository.FindByName(name);
            //return View(data);
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<ReactivatedSk> dataGrid = reactivatedRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: AngkutJual/ReactivatedSks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ReactivatedSk reactivatedSk = db.ReactivatedSks.Find(id);
        //    if (reactivatedSk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(reactivatedSk);
        //}

        // GET: AngkutJual/ReactivatedSks/Create
        public ActionResult Create()
        {
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name");
            ReactivatedSk reactivatedSk = new ReactivatedSk()
            {
                ID = "1",
            };
            return View(reactivatedSk);
        }

        // POST: AngkutJual/ReactivatedSks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SkNumber,SkDate,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] ReactivatedSk reactivatedSk)
        {
            if (ModelState.IsValid)
            {
                reactivatedSk.ID = Guid.NewGuid().ToString();
                reactivatedSk.CreatedBy = User.Identity.Name;
                reactivatedSk.CreatedDate = DateTime.Now;
                reactivatedRepository.AddAsync(reactivatedSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", reactivatedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", reactivatedSk.CompanyID);
            return View(reactivatedSk);
        }

        [HttpPost]
        public async Task<string> CreateService( ReactivatedSk reactivatedSk)
        {
            if (ModelState.IsValid)
            {
                reactivatedSk.ID = Guid.NewGuid().ToString();
                reactivatedSk.CreatedBy = User.Identity.Name;
                reactivatedSk.CreatedDate = DateTime.Now;
                var result = await reactivatedRepository.AddAsync(reactivatedSk);
                return result.ID;
            }
            return "-1";
        }

        // GET: AngkutJual/ReactivatedSks/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReactivatedSk reactivatedSk = await reactivatedRepository.FindAsync(id);
            if (reactivatedSk == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", reactivatedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", reactivatedSk.CompanyID);
            return View(reactivatedSk);
        }

        // POST: AngkutJual/ReactivatedSks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SkNumber,SkDate,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] ReactivatedSk reactivatedSk)
        {
            if (ModelState.IsValid)
            {
                reactivatedSk.ModifiedBy = User.Identity.Name;
                reactivatedSk.ModifiedDate = DateTime.Now;
                reactivatedRepository.UpdateAsync(reactivatedSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", reactivatedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", reactivatedSk.CompanyID);
            return View(reactivatedSk);
        }

        

        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            ReactivatedSk reactivatedSk = await reactivatedRepository.FindAsync(p);
            await reactivatedRepository.RemoveAsync(reactivatedSk);
            return Json(p);
        }


        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            ReactivatedSk reactivatedSk = await reactivatedRepository.FindAsync(id);
            await reactivatedRepository.RemoveAsync(reactivatedSk);
            return "OK";
        }
    }
}
