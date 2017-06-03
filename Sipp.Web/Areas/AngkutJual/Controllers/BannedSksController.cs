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
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class BannedSksController : Controller
    {
        private IBannedSkRepository bannedSkRepository = new BannedSkRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();


        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var data = await bannedSkRepository.FindAsync(id);
            await bannedSkRepository.RemoveAsync(data);
            return "OK";
        }

        // GET: AngkutJual/BannedSks
        public ActionResult Index(string name)
        {
            //var data = bannedSkRepository.FindByName(name);
            //return View(data);
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<BannedSk> dataGrid = bannedSkRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: AngkutJual/BannedSks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BannedSk bannedSk = db.BannedSks.Find(id);
        //    if (bannedSk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bannedSk);
        //}

        // GET: AngkutJual/BannedSks/Create
        public ActionResult Create()
        {
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name");
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name");
            BannedSk bannedSk = new BannedSk()
            {
                ID = "1",

            };
            return View(bannedSk);
        }

        // POST: AngkutJual/BannedSks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SkNumber,SkDate,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] BannedSk bannedSk)
        {
            if (ModelState.IsValid)
            {
                bannedSk.ID = Guid.NewGuid().ToString();
                bannedSk.CreatedBy = User.Identity.Name;
                bannedSk.CreatedDate = DateTime.Now;
                bannedSkRepository.AddAsync(bannedSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", bannedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", bannedSk.CompanyID);
            return View(bannedSk);
        }

        // GET: AngkutJual/BannedSks/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannedSk bannedSk = await bannedSkRepository.FindAsync(id);
            if (bannedSk == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", bannedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", bannedSk.CompanyID);
            return View(bannedSk);
        }
        [HttpPost]
        public async Task<string> CreateService(BannedSk bannedSk)
        {
            if (ModelState.IsValid)
            {
                bannedSk.ID = Guid.NewGuid().ToString();
                bannedSk.CreatedBy = User.Identity.Name;
                bannedSk.CreatedDate = DateTime.Now;
                var result = await  bannedSkRepository.AddAsync(bannedSk);
                return result.ID;
            }
            return "-1";
        }   
        // POST: AngkutJual/BannedSks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SkNumber,SkDate,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] BannedSk bannedSk)
        {
            if (ModelState.IsValid)
            {
                bannedSk.ModifiedBy = User.Identity.Name;
                bannedSk.ModifiedDate = DateTime.Now;
                bannedSkRepository.UpdateAsync(bannedSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", bannedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", bannedSk.CompanyID);
            return View(bannedSk);
        }


        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            BannedSk bannedSk = await bannedSkRepository.FindAsync(p);
            await bannedSkRepository.RemoveAsync(bannedSk);
            return Json(p);
        }

    }
}
