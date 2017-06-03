
using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class BKPMsController : Controller
    {
        private IBKPMRepository bKPMRepository = new BKPMRepository();

        public async Task<JsonResult> FindById(string id)
        {
            var data = await bKPMRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                SkNumber = data.SkNumber,
                LetterNumber = data.LetterNumber,
                LetterDate = data.LetterDate != null ? data.LetterDate.Value.ToString("MM/dd/yyyy") : "",
                LetterType =   data.LetterType,
                BKPMAcceptanceDate = data.BKPMAcceptanceDate != null ? data.BKPMAcceptanceDate.Value.ToString("MM/dd/yyyy") : "",
                EvaluatorAcceptanceDate = data.EvaluatorAcceptanceDate != null ? data.EvaluatorAcceptanceDate.Value.ToString("MM/dd/yyyy") : "",
                AdditionalInformation =      data.AdditionalInformation,
                Status=   data.Status,
                SKFile =         data.SKFile,
                CompanyID=    data.CompanyID,

            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditService(BKPM bKPM)
        {
            if (ModelState.IsValid)
            {
                bKPM.ModifiedBy = User.Identity.Name;
                bKPM.ModifiedDate = DateTime.Now;
                await bKPMRepository.UpdateAsync(bKPM);
                return bKPM.ID;
            }
            return "0";
        }

        [HttpPost]
        public async Task<string> CreateService(BKPM bKPM)
        {
            if (ModelState.IsValid)
            {
                bKPM.ID = Guid.NewGuid().ToString();
                bKPM.CreatedBy = User.Identity.Name;
                bKPM.CreatedDate = DateTime.Now;

                var result = await bKPMRepository.AddAsync(bKPM);
                return result.ID;
                
            }
            return "-1";
        }

        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            BKPM bKPM = await bKPMRepository.FindAsync(id);
            await bKPMRepository.RemoveAsync(bKPM);
            return "OK";
        }


        //private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: AngkutJual/BKPMs
        //public ActionResult Index()
        //{
        //    var bKPM = db.BKPM.Include(b => b.Company);
        //    return View(bKPM.ToList());
        //}

        //// GET: AngkutJual/BKPMs/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BKPM bKPM = db.BKPM.Find(id);
        //    if (bKPM == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bKPM);
        //}

        //// GET: AngkutJual/BKPMs/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
        //    return View();
        //}

        //// POST: AngkutJual/BKPMs/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,SkNumber,LetterNumber,LetterDate,LetterType,BKPMAcceptanceDate,EvaluatorAcceptanceDate,AdditionalInformation,SKFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] BKPM bKPM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.BKPM.Add(bKPM);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", bKPM.CompanyID);
        //    return View(bKPM);
        //}

        //// GET: AngkutJual/BKPMs/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BKPM bKPM = db.BKPM.Find(id);
        //    if (bKPM == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", bKPM.CompanyID);
        //    return View(bKPM);
        //}

        //// POST: AngkutJual/BKPMs/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,SkNumber,LetterNumber,LetterDate,LetterType,BKPMAcceptanceDate,EvaluatorAcceptanceDate,AdditionalInformation,SKFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] BKPM bKPM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(bKPM).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", bKPM.CompanyID);
        //    return View(bKPM);
        //}

        //// GET: AngkutJual/BKPMs/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BKPM bKPM = db.BKPM.Find(id);
        //    if (bKPM == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bKPM);
        //}

        //// POST: AngkutJual/BKPMs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    BKPM bKPM = db.BKPM.Find(id);
        //    db.BKPM.Remove(bKPM);
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
