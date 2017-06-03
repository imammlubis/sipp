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
using System.Threading.Tasks;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class CNCCertificatesController : Controller
    {
        private ICNCCertificateRepository cNCCertificateRepository = new CNCCertificateRepository();
        // POST: AngkutJual/CNCCertificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<string> CreateService(CNCCertificate cNCCertificate)
        {
            if (ModelState.IsValid)
            {
                cNCCertificate.ID = Guid.NewGuid().ToString();
                cNCCertificate.CreatedBy = User.Identity.Name;
                cNCCertificate.CreatedDate = DateTime.Now;
                var result = await cNCCertificateRepository.AddAsync(cNCCertificate);
                return result.ID;
            }
            return "-1";
        }


        // POST: AngkutJual/CNCCertificates/Delete/5
        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var model = await cNCCertificateRepository.FindAsync(id);
            await cNCCertificateRepository.RemoveAsync(model);
            return "OK";
        }


        //private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: AngkutJual/CNCCertificates
        //public ActionResult Index()
        //{
        //    var cNCCertificate = db.CNCCertificate.Include(c => c.Company);
        //    return View(cNCCertificate.ToList());
        //}

        //// GET: AngkutJual/CNCCertificates/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CNCCertificate cNCCertificate = db.CNCCertificate.Find(id);
        //    if (cNCCertificate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cNCCertificate);
        //}

        //// GET: AngkutJual/CNCCertificates/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
        //    return View();
        //}

        //// POST: AngkutJual/CNCCertificates/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,SkNumber,SkDate,SkFile,AdditionalInformation,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] CNCCertificate cNCCertificate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CNCCertificate.Add(cNCCertificate);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", cNCCertificate.CompanyID);
        //    return View(cNCCertificate);
        //}

        //// GET: AngkutJual/CNCCertificates/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CNCCertificate cNCCertificate = db.CNCCertificate.Find(id);
        //    if (cNCCertificate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", cNCCertificate.CompanyID);
        //    return View(cNCCertificate);
        //}

        //// POST: AngkutJual/CNCCertificates/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,SkNumber,SkDate,SkFile,AdditionalInformation,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] CNCCertificate cNCCertificate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cNCCertificate).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", cNCCertificate.CompanyID);
        //    return View(cNCCertificate);
        //}

        //// GET: AngkutJual/CNCCertificates/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CNCCertificate cNCCertificate = db.CNCCertificate.Find(id);
        //    if (cNCCertificate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cNCCertificate);
        //}

        //// POST: AngkutJual/CNCCertificates/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    CNCCertificate cNCCertificate = db.CNCCertificate.Find(id);
        //    db.CNCCertificate.Remove(cNCCertificate);
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
