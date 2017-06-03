
using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class AdditionalCooperationsController : Controller
    {

        private IAdditionalCooperationRepository additionalCooperationRepository = new  AdditionalCooperationRepository();

        [HttpPost]
        public async Task<string> CreateService(AdditionalCooperation additionalCooperation)
        {
            if (ModelState.IsValid)
            {
                additionalCooperation.ID = Guid.NewGuid().ToString();
                additionalCooperation.CreatedBy = User.Identity.Name;
                additionalCooperation.CreatedDate = DateTime.Now;
                var result = await additionalCooperationRepository.AddAsync(additionalCooperation);
                return result.ID;
            }
            return "-1";
        }


        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var additionalCooperation = await additionalCooperationRepository.FindAsync(id);
            await additionalCooperationRepository.RemoveAsync(additionalCooperation);
            return "OK";
        }



        //private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: AngkutJual/AdditionalCooperations
        //public ActionResult Index()
        //{
        //    var additionalCooperation = db.AdditionalCooperation.Include(a => a.Company);
        //    return View(additionalCooperation.ToList());
        //}

        //// GET: AngkutJual/AdditionalCooperations/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AdditionalCooperation additionalCooperation = db.AdditionalCooperation.Find(id);
        //    if (additionalCooperation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(additionalCooperation);
        //}

        //// GET: AngkutJual/AdditionalCooperations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
        //    return View();
        //}

        //// POST: AngkutJual/AdditionalCooperations/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,SkNumber,SkDate,SkFile,AdditionalInformation,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] AdditionalCooperation additionalCooperation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.AdditionalCooperation.Add(additionalCooperation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", additionalCooperation.CompanyID);
        //    return View(additionalCooperation);
        //}

        //// GET: AngkutJual/AdditionalCooperations/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AdditionalCooperation additionalCooperation = db.AdditionalCooperation.Find(id);
        //    if (additionalCooperation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", additionalCooperation.CompanyID);
        //    return View(additionalCooperation);
        //}

        //// POST: AngkutJual/AdditionalCooperations/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,SkNumber,SkDate,SkFile,AdditionalInformation,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] AdditionalCooperation additionalCooperation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(additionalCooperation).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", additionalCooperation.CompanyID);
        //    return View(additionalCooperation);
        //}

        //// GET: AngkutJual/AdditionalCooperations/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AdditionalCooperation additionalCooperation = db.AdditionalCooperation.Find(id);
        //    if (additionalCooperation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(additionalCooperation);
        //}

        //// POST: AngkutJual/AdditionalCooperations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    AdditionalCooperation additionalCooperation = db.AdditionalCooperation.Find(id);
        //    db.AdditionalCooperation.Remove(additionalCooperation);
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
