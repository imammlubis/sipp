using System;
using System.Net;
using System.Web.Mvc;
using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using System.Linq;
using Kendo.Mvc.Extensions;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using EduSpot.Entity.Tables.Organization;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    //[Authorize(Roles = "AngkutJualAdmin")]
    public class FirstSksController : Controller
    {
        private IFirstSkRepository firstSkRepository = new FirstSkRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();

        // GET: AngkutJual/FirstSks
        public ActionResult Index(string name)
        {
            //var data = firstSkRepository.FindByName(name);
            //return View(data);
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<FirstSk> dataGrid = firstSkRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ImportDataFirstSks()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UploadExcel()
        {
            FirstSk firstSk = new FirstSk();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                HSSFWorkbook hssfwb = new HSSFWorkbook(file.InputStream);
                ISheet sheet = hssfwb.GetSheetAt(0);
                for (int row = 2; row <= sheet.LastRowNum; row++)
                {
                    if (sheet.GetRow(row) != null)
                    {
                        firstSk.ID = sheet.GetRow(row).GetCell(0).ToString();
                        firstSk.CreatedBy = User.Identity.Name;
                        firstSk.CreatedDate = DateTime.Now;
                        firstSk.LetterNumber = sheet.GetRow(row).GetCell(1).ToString();
                        //firstSk.LetterDate = DateTime.Parse(sheet.GetRow(row).GetCell(2));
                        firstSk.SkNumber = sheet.GetRow(row).GetCell(3).ToString();
                        firstSk.SkDate = DateTime.Parse(sheet.GetRow(row).GetCell(4).ToString());
                        firstSk.SkEndDate = DateTime.Parse(sheet.GetRow(row).GetCell(5).ToString());
                        firstSk.SkDuration = (firstSk.SkEndDate.Value.Year - firstSk.SkDate.Value.Year).ToString();
                        firstSk.AdditionalInfo = sheet.GetRow(row).GetCell(6).ToString();
                        firstSk.CompanyID = sheet.GetRow(row).GetCell(8).ToString();
                        await firstSkRepository.AddAsync(firstSk);
                    }
                }
            }
            return RedirectToAction("ImportDataCompany");
        }

        // GET: AngkutJual/FirstSks/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name");
            FirstSk firstSk = new FirstSk()
            {
                ID = "1"

            };
            return View(firstSk);
        }

        // POST: AngkutJual/FirstSks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LetterNumber,LetterDate,SkNumber,SkDate,SkEndDate,SkDuration,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] FirstSk firstSk)
        {
            if (ModelState.IsValid)
            {
                firstSk.ID = Guid.NewGuid().ToString();
                firstSk.CreatedBy = User.Identity.Name;
                firstSk.CreatedDate = DateTime.Now;
                firstSkRepository.AddAsync(firstSk);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", firstSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", firstSk.CompanyID);
            return View(firstSk);
        }
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<string> EditService(FirstSk firstSk)
        {
            if (ModelState.IsValid)
            {
                firstSk.ModifiedBy = User.Identity.Name;
                firstSk.ModifiedDate = DateTime.Now;
                await firstSkRepository.UpdateAsync(firstSk);
                return firstSk.ID;
            }
            return "0";
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<string> CreateService(FirstSk firstSk)
        {
            if (ModelState.IsValid)
            {
                firstSk.ID = Guid.NewGuid().ToString();
                firstSk.CreatedBy = User.Identity.Name;
                firstSk.CreatedDate = DateTime.Now;                
                var result = await firstSkRepository.AddAsync(firstSk);

                Company comp = companyRepository.GetAll().Where(c => c.ID == firstSk.CompanyID).SingleOrDefault();
                comp.ID = firstSk.CompanyID;                
                comp.IdSkAktif = firstSk.ID;
                comp.JenisSk = "Awal";
                await companyRepository.UpdateAsync(comp);
                return result.ID;
            }
            return "0";
        }

        public async Task<JsonResult> FindById(string id) {
            var data = await firstSkRepository.FindAsync(id);
            var result = new {
                ID = data.ID,
                SkNumber =   data.SkNumber,
                SertifikatCNC = data.SertifikatCNC,
                LetterNumber=   data.LetterNumber,
                LetterDate =    data.LetterDate != null ? data.LetterDate.Value.ToString("MM/dd/yyyy") : "",
                SkDuration =      data.SkDuration,
                SkDate = data.SkDate != null ? data.SkDate.Value.ToString("MM/dd/yyyy") : "",
                SkFile =   data.SkFile,
                SkEndDate = data.SkEndDate != null ? data.SkEndDate.Value.ToString("MM/dd/yyyy") : "",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // GET: AngkutJual/FirstSks/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FirstSk firstSk = await firstSkRepository.FindAsync(id);
            if (firstSk == null)
            {
                return HttpNotFound();
            }

            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", firstSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", firstSk.CompanyID);
            return View(firstSk);
        }

        // POST: AngkutJual/FirstSks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LetterNumber,LetterDate,SkNumber,SkDate,SkEndDate,SkDuration,AdditionalInfo,SkFile,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] FirstSk firstSk)
        {
            if (ModelState.IsValid)
            {
                firstSk.ModifiedBy = User.Identity.Name;
                firstSk.ModifiedDate = DateTime.Now;
                firstSkRepository.UpdateAsync(firstSk);
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", firstSk.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", firstSk.CompanyID);
            return View(firstSk);
        }


        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            FirstSk firstSk = await firstSkRepository.FindAsync(p);
            await firstSkRepository.RemoveAsync(firstSk);
            return Json(p);
        }
    }
}
