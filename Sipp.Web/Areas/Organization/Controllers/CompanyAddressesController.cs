using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EduSpot.Core.Infrastructure;
using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Esdm.Web.Areas.Organization.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class CompanyAddressesController : Controller
    {
        private ICompanyAddressRepository companyAddressRepository = new CompanyAddressRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();
        // GET: Organization/CompanyAddresses
        public ActionResult Index(string name)
        {
            //var data = companyAddressRepository.FindByName(name);
            //return View(data);
            return View();
        }
        public ActionResult ImportDataCompanyAddresses()
        {
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<CompanyAddress> dataGrid = companyAddressRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Organization/CompanyAddresses/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CompanyAddress companyAddress = db.CompanyAddress.Find(id);
        //    if (companyAddress == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(companyAddress);
        //}

        // GET: Organization/CompanyAddresses/Create
        public ActionResult Create()
        {
            CompanyAddress companyAddress = new CompanyAddress()
            {
                ID = "1",

            };
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", companyAddress.CompanyID);

            return View(companyAddress);
        }

        // POST: Organization/CompanyAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Address,TelNumber,Email,Status,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        CompanyAddress companyAddress)
        {
            if (ModelState.IsValid)
            {
                companyAddress.ID = Guid.NewGuid().ToString();
                companyAddress.CreatedBy = User.Identity.Name;
                companyAddress.CreatedDate = DateTime.Now;
                companyAddressRepository.AddAsync(companyAddress);
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", companyAddress.CompanyID);
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", companyAddress.CompanyID);
            return View(companyAddress);
        }
        public int GetDuplicateImportData(string id)
        {
            var dataID = companyAddressRepository.FindById(id);
            return dataID.Count();
        }
        [HttpPost]
        public async Task<ActionResult> UploadExcel()
        {
            CompanyAddress companyAddress = new CompanyAddress();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                HSSFWorkbook hssfwb = new HSSFWorkbook(file.InputStream);

                ISheet sheet = hssfwb.GetSheetAt(0);
                for (int row = 2; row <= sheet.LastRowNum; row++)
                {
                    try
                    {
                        int duplicate = GetDuplicateImportData(sheet.GetRow(row).GetCell(0).ToString());
                        if (sheet.GetRow(row) != null && duplicate == 0)
                        {
                            companyAddress.ID = sheet.GetRow(row).GetCell(0).ToString();
                            companyAddress.CreatedBy = User.Identity.Name;
                            companyAddress.CreatedDate = DateTime.Now;
                            companyAddress.Address = sheet.GetRow(row).GetCell(1).ToString();
                            companyAddress.TelNumber = sheet.GetRow(row).GetCell(2).ToString();
                            companyAddress.Email = sheet.GetRow(row).GetCell(3).ToString();
                            companyAddress.Status = sheet.GetRow(row).GetCell(4).BooleanCellValue;
                            companyAddress.CompanyID = sheet.GetRow(row).GetCell(5).ToString();
                            await companyAddressRepository.AddAsync(companyAddress);
                        }
                    }
                    catch { }
                }
            }
            return RedirectToAction("index");
        }

        // GET: Organization/CompanyAddresses/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyAddress companyAddress = await companyAddressRepository.FindAsync(id);
            if (companyAddress == null)
            {
                return HttpNotFound();
            }

            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", companyAddress.CompanyID);
            
            return View(companyAddress);
        }

        // POST: Organization/CompanyAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Address,TelNumber,Email,Status,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        CompanyAddress companyAddress)
        {
            if (ModelState.IsValid)
            {
                companyAddress.ModifiedBy = User.Identity.Name;
                companyAddress.ModifiedDate = DateTime.Now;
                companyAddressRepository.UpdateAsync(companyAddress);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", companyAddress.CompanyID);
            return View(companyAddress);
        }

       
        // POST: Organization/CompanyAddresses/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            CompanyAddress companyAddress = await companyAddressRepository.FindAsync(p);
            await companyAddressRepository.RemoveAsync(companyAddress);
            return Json(p);
        }

    }
}
