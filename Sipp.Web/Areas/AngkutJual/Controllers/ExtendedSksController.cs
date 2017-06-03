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
using EduSpot.Entity.Tables.Organization;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class ExtendedSksController : Controller
    {
        private IExtendedSkRepository extendedSkRepository = new ExtendedSkRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();
        // GET: AngkutJual/ExtendedSks
        public ActionResult Index(string name)
        {
            //var data = extendedSkRepository.FindByName(name);
            //return View(data);
            return View();
        }


        public async Task<JsonResult> FindById(string id)
        {
            var data = await extendedSkRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                SkNumber = data.SkNumber,
                LetterNumber = data.LetterNumber,
                LetterDate = data.LetterDate != null ? data.LetterDate.Value.ToString("MM/dd/yyyy") : "",
                SkDuration = data.SkDuration,
                SkDate = data.SkDate != null ? data.SkDate.Value.ToString("MM/dd/yyyy") : "",
                SkFile = data.SkFile,
                SkEndDate = data.SkEndDate != null ? data.SkEndDate.Value.ToString("MM/dd/yyyy") : "",
                RpiitDate = data.RpiitDate != null ? data.RpiitDate.Value.ToString("MM/dd/yyyy") : "",
                RpiitNumber = data.RpiitNumber
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


       

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditService(ExtendedSk extendedSk)
        {
            if (ModelState.IsValid)
            {
                extendedSk.ModifiedBy = User.Identity.Name;
                extendedSk.ModifiedDate = DateTime.Now;
                await extendedSkRepository.UpdateAsync(extendedSk);
                return extendedSk.ID;
            }

            return "0";
        }

        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<ExtendedSk> dataGrid = extendedSkRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<string> CreateService(ExtendedSk extendedSk)
        {
            if (ModelState.IsValid)
            {
                extendedSk.ID = Guid.NewGuid().ToString();
                extendedSk.CreatedBy = User.Identity.Name;
                extendedSk.CreatedDate = DateTime.Now;
                var result =  await  extendedSkRepository.AddAsync(extendedSk);

                Company comp = new Company();
                comp.IdSkAktif = extendedSk.ID;
                comp.JenisSk = "Perpanjangan";
                await companyRepository.AddAsync(comp);

                return result.ID;
            }
            return "-1";
        }
        // GET: AngkutJual/ExtendedSks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ExtendedSk extendedSk = db.ExtendedSks.Find(id);
        //    if (extendedSk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(extendedSk);
        //}

        // GET: AngkutJual/ExtendedSks/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name");
            ExtendedSk extendedSk = new ExtendedSk()
            {
                ID = "1"
            };
            return View(extendedSk);
        }

        // POST: AngkutJual/ExtendedSks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LetterNumber,LetterDate,RpiitNumber,RpiitDate,SkNumber,SkDate,SkEndDate,SkDuration,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] ExtendedSk extendedSk)
        {
            if (ModelState.IsValid)
            {
                extendedSk.ID = Guid.NewGuid().ToString();
                extendedSk.ModifiedBy = User.Identity.Name;
                extendedSk.ModifiedDate = DateTime.Now;
                extendedSkRepository.AddAsync(extendedSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", extendedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", extendedSk.CompanyID);
            return View(extendedSk);
        }

        // GET: AngkutJual/ExtendedSks/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtendedSk extendedSk = await extendedSkRepository.FindAsync(id);
            if (extendedSk == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", extendedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", extendedSk.CompanyID);
            return View(extendedSk);
        }
        // POST: AngkutJual/ExtendedSks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LetterNumber,LetterDate,RpiitNumber,RpiitDate,SkNumber,SkDate,SkEndDate,SkDuration,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] ExtendedSk extendedSk)
        {
            if (ModelState.IsValid)
            {
                extendedSk.ModifiedBy = User.Identity.Name;
                extendedSk.ModifiedDate = DateTime.Now;
                extendedSkRepository.UpdateAsync(extendedSk);

                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", extendedSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", extendedSk.CompanyID);
            return View(extendedSk);
        }

        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            ExtendedSk extendedSk = await extendedSkRepository.FindAsync(p);
            await extendedSkRepository.RemoveAsync(extendedSk);
            return Json(p);
        }
    }
}
