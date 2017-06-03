using System;
using System.Net;
using System.Web.Mvc;
using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using System.Linq;
using Kendo.Mvc.Extensions;

namespace Esdm.Web.Areas.Organization.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class IupTypesController : Controller
    {
        private IIupTypeRepository iupTypeRepository = new IupTypeRepository();
        // GET: Administration/IupTypes
        public ActionResult Index(string name)
        {
            //var data = iupTypeRepository.FindByName(name);
            //return View(data);
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<IupType> dataGrid = iupTypeRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Administration/IupTypes/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    IupType iupType = db.IupTypes.Find(id);
        //    if (iupType == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(iupType);
        //}

        // GET: Administration/IupTypes/Create
        public ActionResult Create()
        {
            IupType iupType = new IupType() {
                ID="1",                

            };
            return View(iupType);
        }

        // POST: Administration/IupTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        IupType iupType)
        {
            if (ModelState.IsValid)
            {
                iupType.ID = Guid.NewGuid().ToString();
                iupType.CreatedBy = User.Identity.Name;
                iupType.CreatedDate = DateTime.Now;
                iupTypeRepository.AddAsync(iupType);

                //db.IupTypes.Add(iupType);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(iupType);
        }

        // GET: Administration/IupTypes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //IupType iupType = db.IupTypes.Find(id);
            IupType iupType = await iupTypeRepository.FindAsync(id) ;
            if (iupType == null)
            {
                return HttpNotFound();
            }
            return View(iupType);
        }

        // POST: Administration/IupTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] IupType iupType)
        {
            if (ModelState.IsValid)
            {
                iupType.ModifiedBy = User.Identity.Name;
                iupType.ModifiedDate = DateTime.Now;
                iupTypeRepository.UpdateAsync(iupType);
                //db.Entry(iupType).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iupType);
        }


        // POST: Administration/IupTypes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    IupType iupType = await iupTypeRepository.FindAsync(id);
        //    await   iupTypeRepository.RemoveAsync(iupType);
        //    //IupType iupType = db.IupTypes.Find(id);
        //    //db.IupTypes.Remove(iupType);
        //    //db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            IupType company = await iupTypeRepository.FindAsync(p);
            await iupTypeRepository.RemoveAsync(company);
            return Json(p);
        }
    }
}
