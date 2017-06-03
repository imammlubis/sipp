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
using System.Threading.Tasks;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class TempBannedSksController : Controller
    {
        private ITempBannedSkRepository tempBannedSkRepository = new TempBannedSkRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();
        // GET: AngkutJual/TempBannedSks
        public ActionResult Index(string name)
        {
            //var data = tempBannedSkRepository.FindByName(name);
            //return View(data);
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<TempBannedSk> dataGrid = tempBannedSkRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: AngkutJual/TempBannedSks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TempBannedSk tempBannedSk = db.TempBannedSks.Find(id);
        //    if (tempBannedSk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tempBannedSk);
        //}

        // GET: AngkutJual/TempBannedSks/Create
        public ActionResult Create()
        {
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name");
            TempBannedSk tempBannedSk = new TempBannedSk()
            {
                ID = "1"
            };
            return View(tempBannedSk);
        }

        // POST: AngkutJual/TempBannedSks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SkNumber,SkDate,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        TempBannedSk tempBannedSk)
        {
            if (ModelState.IsValid)
            {
                tempBannedSk.ID = Guid.NewGuid().ToString();
                tempBannedSk.CreatedBy = User.Identity.Name;
                tempBannedSk.CreatedDate = DateTime.Now;
                tempBannedSkRepository.AddAsync(tempBannedSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", tempBannedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", tempBannedSk.CompanyID);
            return View(tempBannedSk);
        }

        [HttpPost]
        public async Task<string> CreateService(TempBannedSk tempBannedSk)
        {
            if (ModelState.IsValid)
            {
                tempBannedSk.ID = Guid.NewGuid().ToString();
                tempBannedSk.CreatedBy = User.Identity.Name;
                tempBannedSk.CreatedDate = DateTime.Now;
                var result = await tempBannedSkRepository.AddAsync(tempBannedSk);
                return result.ID;
            }
            return "-1";
        }
        // GET: AngkutJual/TempBannedSks/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempBannedSk tempBannedSk = await tempBannedSkRepository.FindAsync(id);

            if (tempBannedSk == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", tempBannedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", tempBannedSk.CompanyID);
            return View(tempBannedSk);
        }

        // POST: AngkutJual/TempBannedSks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SkNumber,SkDate,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        TempBannedSk tempBannedSk)
        {
            if (ModelState.IsValid)
            {
                tempBannedSk.ModifiedBy = User.Identity.Name;
                tempBannedSk.ModifiedDate = DateTime.Now;
                tempBannedSkRepository.UpdateAsync(tempBannedSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", tempBannedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", tempBannedSk.CompanyID);
            return View(tempBannedSk);
        }


        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            TempBannedSk tempBannedSk = await tempBannedSkRepository.FindAsync(p);
            await tempBannedSkRepository.RemoveAsync(tempBannedSk);
            return Json(p);
        }


        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            TempBannedSk tempBannedSk = await tempBannedSkRepository.FindAsync(id);
            await tempBannedSkRepository.RemoveAsync(tempBannedSk);
            return "OK";
        }

    }
}
