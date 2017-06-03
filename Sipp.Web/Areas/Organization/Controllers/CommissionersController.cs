using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EduSpot.Core.Infrastructure;
using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System.Threading.Tasks;

namespace Esdm.Web.Areas.Organization.Controllers
{
    public class CommissionersController : Controller
    {
        private ICommisionerRepository repo = new CommissionerRepository();
        public JsonResult LoadCommisioners(string id)
        {
            var data = repo.FindByCompany(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> CreateService(Commissioner model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Guid.NewGuid().ToString();
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;
                model.NPWP = model.NPWP;

                await repo.AddAsync(model);
                return model.ID;
            }
            return "0";
        }
        public async Task<JsonResult> FindById(string id)
        {
            var data = await repo.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Status = data.Status,
                CompanyID = data.CompanyID,
                Name = data.Name,
                NPWP = data.NPWP,
                Country = data.Country,
                AdditionalInformation = data.AdditionalInformation
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditService(Commissioner model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = User.Identity.Name;
                model.ModifiedDate = DateTime.Now;
                await repo.UpdateAsync(model);
                return model.ID;
            }
            return "0";
        }

        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var model = await repo.FindAsync(id);
            await repo.RemoveAsync(model);
            return "OK";
        }


        //private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Organization/Commissioners
        //public ActionResult Index()
        //{
        //    var commissioner = db.Commissioner.Include(c => c.Company);
        //    return View(commissioner.ToList());
        //}

        //// GET: Organization/Commissioners/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Commissioner commissioner = db.Commissioner.Find(id);
        //    if (commissioner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(commissioner);
        //}

        //// GET: Organization/Commissioners/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
        //    return View();
        //}

        //// POST: Organization/Commissioners/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Name,Description,Status,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Commissioner commissioner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Commissioner.Add(commissioner);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", commissioner.CompanyID);
        //    return View(commissioner);
        //}

        //// GET: Organization/Commissioners/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Commissioner commissioner = db.Commissioner.Find(id);
        //    if (commissioner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", commissioner.CompanyID);
        //    return View(commissioner);
        //}

        //// POST: Organization/Commissioners/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Name,Description,Status,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Commissioner commissioner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(commissioner).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", commissioner.CompanyID);
        //    return View(commissioner);
        //}

        //// GET: Organization/Commissioners/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Commissioner commissioner = db.Commissioner.Find(id);
        //    if (commissioner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(commissioner);
        //}

        //// POST: Organization/Commissioners/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Commissioner commissioner = db.Commissioner.Find(id);
        //    db.Commissioner.Remove(commissioner);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
