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
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class CoalSourceSksController : Controller
    {
        private ICoalSourceSkRepository coalSourceSkRepository = new CoalSourceSkRepository();
        private IFirstSkRepository firstSkRepository = new FirstSkRepository();
        private IExtendedSkRepository extendedSkRepository = new ExtendedSkRepository();
        private IBKPMRepository bkpmRepository = new BKPMRepository();


        // POST: AngkutJual/ETRecommendations/Delete/5
        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var data = await coalSourceSkRepository.FindAsync(id);
            await coalSourceSkRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<JsonResult> FindById(string id)
        {
            var data = await coalSourceSkRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                SkNumber = data.SkNumber,
                City = data.City,
                CompanySource =  data.CompanySource,
                CompanySourceAddress = data.CompanySourceAddress,
                Province=data.Province,
                Tonnage= data.Tonnage,
                SkFile = data.SkFile       ,
                SkDate = data.SkDate
                
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: AngkutJual/CoalSourceSks
        public ActionResult Index(string name)
        {
            //var data = coalSourceSkRepository.FindByName(name);
            //return View(data);
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            var dataResult = from a in coalSourceSkRepository.GetAll().AsEnumerable()
                             join b in firstSkRepository.GetAll().AsEnumerable()
                             on a.FirstSkID equals b.ID into group1
                             from g1 in group1.DefaultIfEmpty()
                             join c in bkpmRepository.GetAll().AsEnumerable()
                             on a.BKPMID equals c.ID into group2
                             from g2 in group2.DefaultIfEmpty()
                             join d in extendedSkRepository.GetAll().AsEnumerable()
                             on a.ExtendedSkID equals d.ID into group3
                             from g3 in group3.DefaultIfEmpty()
                             select new CoalSourceViewModel
                             {
                                 ID = a.ID,
                                 ExtendedSKNumber = g3 == null ? String.Empty : g3.LetterNumber,
                                 FirstSKNumber = g1 == null ? String.Empty : g1.LetterNumber,
                                 BkpmSKNumber = g2 == null ? String.Empty : g2.LetterNumber,
                                 SkNumber = a.SkNumber,
                                 SkDate = a.SkDate,
                                 SkFile = a.SkFile,
                                 CompanySource = a.CompanySource,
                                 CompanySourceAddress = a.CompanySourceAddress,
                                 Province = a.Province, 
                                 Tonnage = a.Tonnage,
                                 CreatedBy = a.CreatedBy,
                                 CreatedDate = a.CreatedDate,
                                 ModifiedBy = a.ModifiedBy,
                                 ModifiedDate = a.ModifiedDate
                             };
            DataSourceResult result = dataResult.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: AngkutJual/CoalSourceSks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CoalSourceSk coalSourceSk = db.CoalSourceSks.Find(id);
        //    if (coalSourceSk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(coalSourceSk);
        //}

        // GET: AngkutJual/CoalSourceSks/Create
        public ActionResult Create()
        {
            ViewBag.FirstSkID = new SelectList(firstSkRepository.GetAll(), "ID", "LetterNumber");
            ViewBag.ExtendedSkID = new SelectList(extendedSkRepository.GetAll(), "ID", "LetterNumber");
            ViewBag.BKPMID = new SelectList(bkpmRepository.GetAll(), "ID", "SkNumber");
            //ViewBag.ExtendedSkID = new SelectList(db.ExtendedSks, "ID", "LetterNumber");
            //ViewBag.FirstSkID = new SelectList(db.FirstSks, "ID", "LetterNumber");

            CoalSourceSk coalSourceSk = new CoalSourceSk()
            {
                ID = "1"
            };
            return View(coalSourceSk);
        }

        // POST: AngkutJual/CoalSourceSks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SkNumber,BKPMID,SkDate,SkFile,CompanySource,CompanySourceAddress,Province,Tonnage,FirstSkID,ExtendedSkID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] CoalSourceSk coalSourceSk)
        {
            if (ModelState.IsValid)
            {
                coalSourceSk.ID = Guid.NewGuid().ToString();
                coalSourceSk.CreatedBy = User.Identity.Name;
                coalSourceSk.CreatedDate = DateTime.Now;
                coalSourceSkRepository.AddAsync(coalSourceSk);
                return RedirectToAction("Index");
            }

            ViewBag.FirstSkID = new SelectList(firstSkRepository.GetAll(), "ID", "LetterNumber", coalSourceSk.FirstSkID);
            ViewBag.ExtendedSkID = new SelectList(extendedSkRepository.GetAll(), "ID", "LetterNumber", coalSourceSk.ExtendedSkID);
            ViewBag.BKPMID = new SelectList(bkpmRepository.GetAll(), "ID", "SkNumber", coalSourceSk.BKPMID);
            //ViewBag.ExtendedSkID = new SelectList(db.ExtendedSks, "ID", "LetterNumber", coalSourceSk.ExtendedSkID);
            //ViewBag.FirstSkID = new SelectList(db.FirstSks, "ID", "LetterNumber", coalSourceSk.FirstSkID);
            return View(coalSourceSk);
        }

        [HttpPost]
        public async Task<string>  CreateService(CoalSourceSk coalSourceSk)
        {
            if (ModelState.IsValid)
            {
                coalSourceSk.ID = Guid.NewGuid().ToString();
                coalSourceSk.CreatedBy = User.Identity.Name;
                coalSourceSk.CreatedDate = DateTime.Now;
                var result =  await  coalSourceSkRepository.AddAsync(coalSourceSk);
                return result.ID;
                
            }
            return "-1";
        }

        [HttpPost]
        public async Task<string>  EditService(CoalSourceSk coalSourceSk)
        {
            if (ModelState.IsValid)
            {
                coalSourceSk.ModifiedBy = User.Identity.Name;
                coalSourceSk.ModifiedDate = DateTime.Now;
                var result = await coalSourceSkRepository.UpdateAsync(coalSourceSk);
                return result.ID;
            }
            return "-1";
          
        }


        // GET: AngkutJual/CoalSourceSks/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoalSourceSk coalSourceSk = await coalSourceSkRepository.FindAsync(id);
            if (coalSourceSk == null)
            {
                return HttpNotFound();
            }

            ViewBag.FirstSkID = new SelectList(firstSkRepository.GetAll(), "ID", "LetterNumber", coalSourceSk.FirstSkID);
            ViewBag.ExtendedSkID = new SelectList(extendedSkRepository.GetAll(), "ID", "LetterNumber", coalSourceSk.ExtendedSkID);
            ViewBag.BKPMID = new SelectList(bkpmRepository.GetAll(), "ID", "SkNumber", coalSourceSk.BKPMID);
            return View(coalSourceSk);
        }

        // POST: AngkutJual/CoalSourceSks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SkNumber,BKPMID,SkDate,SkFile,CompanySource,CompanySourceAddress,Province,Tonnage,FirstSkID,ExtendedSkID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        CoalSourceSk coalSourceSk)
        {
            if (ModelState.IsValid)
            {
                coalSourceSk.ModifiedBy = User.Identity.Name;
                coalSourceSk.ModifiedDate = DateTime.Now;
                coalSourceSkRepository.UpdateAsync(coalSourceSk);
                return RedirectToAction("Index");
            }

            ViewBag.FirstSkID = new SelectList(firstSkRepository.GetAll(), "ID", "LetterNumber", coalSourceSk.FirstSkID);
            ViewBag.ExtendedSkID = new SelectList(extendedSkRepository.GetAll(), "ID", "LetterNumber", coalSourceSk.ExtendedSkID);
            ViewBag.BKPMID = new SelectList(bkpmRepository.GetAll(), "ID", "SkNumber", coalSourceSk.BKPMID);
            //ViewBag.ExtendedSkID = new SelectList(db.ExtendedSks, "ID", "LetterNumber", coalSourceSk.ExtendedSkID);
            //ViewBag.FirstSkID = new SelectList(db.FirstSks, "ID", "LetterNumber", coalSourceSk.FirstSkID);
            return View(coalSourceSk);
        }

        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            CoalSourceSk coalSourceSk = await coalSourceSkRepository.FindAsync(p);
            await coalSourceSkRepository.RemoveAsync(coalSourceSk);
            return Json(p);
        }


        public class CoalSourceViewModel
        {
            public string ID { get; set; }

            public string SkNumber { get; set; }
            public Nullable<DateTime> SkDate { get; set; }
            public string SkFile { get; set; }

            public string CompanySource { get; set; }
            public string CompanySourceAddress { get; set; }
            public string City { get; set; }
            public string Province { get; set; }
            public string SalesDestination { get; set; }
            public string Remark { get; set; }
            public string Tonnage { get; set; }



            public string FirstSkID { get; set; }
            public virtual FirstSk FirstSk { get; set; }
            public string ExtendedSkID { get; set; }

            public string ExtendedSKNumber { get; set; }
            public string FirstSKNumber { get; set; }
            public string BkpmSKNumber { get; set; }

            public virtual ExtendedSk ExtendedSk { get; set; }

            public string BKPMID { get; set; }
            public virtual BKPM BKPM { get; set; }


            public string CreatedBy { get; set; }
            public Nullable<DateTime> CreatedDate { get; set; }

            public string ModifiedBy { get; set; }
            public Nullable<DateTime> ModifiedDate { get; set; }


        }
    }
}
