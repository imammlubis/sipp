using System;
using System.Web.Mvc;
using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using System.Threading.Tasks;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class ETRecommendationsController : Controller
    {
        private IETRecommendationRepository eTRecommendationRepository = new ETRecommendationRepository();

        [HttpPost]
        public async Task<string> CreateService(ETRecommendation eTRecommendation)
        {
            if (ModelState.IsValid)
            {

                eTRecommendation.ID = Guid.NewGuid().ToString();
                eTRecommendation.CreatedBy = User.Identity.Name;
                eTRecommendation.CreatedDate = DateTime.Now;
                var result = await eTRecommendationRepository.AddAsync(eTRecommendation);
                return result.ID;

                
            }

            return "-1";

        }


        // POST: AngkutJual/ETRecommendations/Delete/5
        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var eTRecommendation = await eTRecommendationRepository.FindAsync(id);
            await eTRecommendationRepository.RemoveAsync(eTRecommendation);
            return "OK";
        }



        //private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: AngkutJual/ETRecommendations
        //public ActionResult Index()
        //{
        //    var eTRecommendation = db.ETRecommendation.Include(e => e.Company);
        //    return View(eTRecommendation.ToList());
        //}

        //// GET: AngkutJual/ETRecommendations/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ETRecommendation eTRecommendation = db.ETRecommendation.Find(id);
        //    if (eTRecommendation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(eTRecommendation);
        //}

        //// GET: AngkutJual/ETRecommendations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
        //    return View();
        //}

        //// POST: AngkutJual/ETRecommendations/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,SkNumber,SkDate,SkFile,AdditionalInformation,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] ETRecommendation eTRecommendation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ETRecommendation.Add(eTRecommendation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", eTRecommendation.CompanyID);
        //    return View(eTRecommendation);
        //}

        //// GET: AngkutJual/ETRecommendations/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ETRecommendation eTRecommendation = db.ETRecommendation.Find(id);
        //    if (eTRecommendation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", eTRecommendation.CompanyID);
        //    return View(eTRecommendation);
        //}

        //// POST: AngkutJual/ETRecommendations/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,SkNumber,SkDate,SkFile,AdditionalInformation,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] ETRecommendation eTRecommendation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(eTRecommendation).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", eTRecommendation.CompanyID);
        //    return View(eTRecommendation);
        //}

        //// GET: AngkutJual/ETRecommendations/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ETRecommendation eTRecommendation = db.ETRecommendation.Find(id);
        //    if (eTRecommendation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(eTRecommendation);
        //}

        //// POST: AngkutJual/ETRecommendations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    ETRecommendation eTRecommendation = db.ETRecommendation.Find(id);
        //    db.ETRecommendation.Remove(eTRecommendation);
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
