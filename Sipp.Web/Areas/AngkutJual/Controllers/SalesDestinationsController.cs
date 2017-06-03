
using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Kendo.Mvc.Extensions;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class SalesDestinationsController : Controller
    {
        private ISalesDestinationRepository salesDestinationRepository = 
            new SalesDestinationRepository();

        public async Task<JsonResult> FindById(string id)
        {
            var data = await salesDestinationRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Destination = data.Destination,
                LetterNumber = data.Remark,
                FirstSkID = data.FirstSkID,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditService(SalesDestination salesDestination)
        {
            if (ModelState.IsValid)
            {
              
                await salesDestinationRepository.UpdateAsync(salesDestination);
                return salesDestination.ID;
            }

            return "0";
        }

        //// POST: AngkutJual/SalesDestinations/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> DeleteSerivice(string id)
        {
            SalesDestination salesDestination = await salesDestinationRepository.FindAsync(id);
            await salesDestinationRepository.RemoveAsync(salesDestination);

            return "OK";
        }
        // POST: AngkutJual/SalesDestinations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<string> CreateService(SalesDestination salesDestination)
        {
            if (ModelState.IsValid)
            {
                salesDestination.ID = Guid.NewGuid().ToString();
                var result = await salesDestinationRepository.AddAsync(salesDestination);
                return result.ID;
            }
            return "-1";
        }


        public JsonResult LoadSalesDestination(string id)
        {

            var data = salesDestinationRepository.GetAll().Where(s => s.FirstSkID == id);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             Destination = x.Destination,
                             Remark = x.Remark

                         };


            return Json(
                new
                {

                    data = result

                }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadSalesDestinationPerpanjangan(string id)
        {

            var data = salesDestinationRepository.GetAll().Where(s => s.ExtendedSkID == id);
            var result = from x in data
                             select new
                             {
                                 ID = x.ID,
                                 Destination = x.Destination,
                                 Remark =  x.Remark
                                
                             };


            return Json(
                new
                {

                    data = result

                }, JsonRequestBehavior.AllowGet);
        }

        // GET: AngkutJual/SalesDestinations
        //public ActionResult Index()
        //{
        //    var salesDestination = db.SalesDestination.Include(s => s.ExtendedSk).Include(s => s.FirstSk);
        //    return View(salesDestination.ToList());
        //}

        //// GET: AngkutJual/SalesDestinations/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SalesDestination salesDestination = db.SalesDestination.Find(id);
        //    if (salesDestination == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(salesDestination);
        //}

        //// GET: AngkutJual/SalesDestinations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ExtendedSkID = new SelectList(db.ExtendedSks, "ID", "LetterNumber");
        //    ViewBag.FirstSkID = new SelectList(db.FirstSks, "ID", "LetterNumber");
        //    return View();
        //}

        //// POST: AngkutJual/SalesDestinations/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Destination,Remark,FirstSkID,ExtendedSkID")] SalesDestination salesDestination)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SalesDestination.Add(salesDestination);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ExtendedSkID = new SelectList(db.ExtendedSks, "ID", "LetterNumber", salesDestination.ExtendedSkID);
        //    ViewBag.FirstSkID = new SelectList(db.FirstSks, "ID", "LetterNumber", salesDestination.FirstSkID);
        //    return View(salesDestination);
        //}

        //// GET: AngkutJual/SalesDestinations/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SalesDestination salesDestination = db.SalesDestination.Find(id);
        //    if (salesDestination == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ExtendedSkID = new SelectList(db.ExtendedSks, "ID", "LetterNumber", salesDestination.ExtendedSkID);
        //    ViewBag.FirstSkID = new SelectList(db.FirstSks, "ID", "LetterNumber", salesDestination.FirstSkID);
        //    return View(salesDestination);
        //}

        //// POST: AngkutJual/SalesDestinations/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Destination,Remark,FirstSkID,ExtendedSkID")] SalesDestination salesDestination)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(salesDestination).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ExtendedSkID = new SelectList(db.ExtendedSks, "ID", "LetterNumber", salesDestination.ExtendedSkID);
        //    ViewBag.FirstSkID = new SelectList(db.FirstSks, "ID", "LetterNumber", salesDestination.FirstSkID);
        //    return View(salesDestination);
        //}

        //// GET: AngkutJual/SalesDestinations/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SalesDestination salesDestination = db.SalesDestination.Find(id);
        //    if (salesDestination == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(salesDestination);
        //}

        //// POST: AngkutJual/SalesDestinations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    SalesDestination salesDestination = db.SalesDestination.Find(id);
        //    db.SalesDestination.Remove(salesDestination);
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
