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
    public class CompanyFilesController : Controller
    {

        private ICompanyFileRepository companyFileRepository = new CompanyFileRepository();

        [HttpPost]
        public async Task<string> CreateService(CompanyFile companyFile)
        {
            if (ModelState.IsValid)
            {
                companyFile.ID = Guid.NewGuid().ToString();
                companyFile.CreatedBy = User.Identity.Name;
                companyFile.CreatedDate = DateTime.Now;
                var result = await companyFileRepository.AddAsync(companyFile);
                return result.ID;
            }
            return "-1";
        }

        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var companyFile = await companyFileRepository.FindAsync(id);
            await companyFileRepository.RemoveAsync(companyFile);
            return "OK";
        }



        //private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Organization/CompanyFiles
        //public ActionResult Index()
        //{
        //    var companyFile = db.CompanyFile.Include(c => c.Company);
        //    return View(companyFile.ToList());
        //}

        //// GET: Organization/CompanyFiles/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CompanyFile companyFile = db.CompanyFile.Find(id);
        //    if (companyFile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(companyFile);
        //}

        //// GET: Organization/CompanyFiles/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
        //    return View();
        //}

        //// POST: Organization/CompanyFiles/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,FileName,NamaSurat,NomorSurat,TanggalSurat,Pengirim,Tujuan,Perihal,Description,Module,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] CompanyFile companyFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CompanyFile.Add(companyFile);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", companyFile.CompanyID);
        //    return View(companyFile);
        //}

        //// GET: Organization/CompanyFiles/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CompanyFile companyFile = db.CompanyFile.Find(id);
        //    if (companyFile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", companyFile.CompanyID);
        //    return View(companyFile);
        //}

        //// POST: Organization/CompanyFiles/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,FileName,NamaSurat,NomorSurat,TanggalSurat,Pengirim,Tujuan,Perihal,Description,Module,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] CompanyFile companyFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(companyFile).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", companyFile.CompanyID);
        //    return View(companyFile);
        //}

        //// GET: Organization/CompanyFiles/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CompanyFile companyFile = db.CompanyFile.Find(id);
        //    if (companyFile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(companyFile);
        //}

        //// POST: Organization/CompanyFiles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    CompanyFile companyFile = db.CompanyFile.Find(id);
        //    db.CompanyFile.Remove(companyFile);
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
