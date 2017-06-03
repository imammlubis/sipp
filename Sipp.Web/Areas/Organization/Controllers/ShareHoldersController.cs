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
    //[Authorize(Roles = "AngkutJualAdmin")]
    public class ShareHoldersController : Controller
    {
        private IShareHolderRepository shareHolderRepository = new ShareHolderRepository();
        private ICompanyAddressRepository companyAddressRepository = new CompanyAddressRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> CreateService(ShareHolder shareHolder)
        {
            if (ModelState.IsValid)
            {
                shareHolder.ID = Guid.NewGuid().ToString();
                shareHolder.CreatedBy = User.Identity.Name;
                shareHolder.CreatedDate = DateTime.Now;
                await shareHolderRepository.AddAsync(shareHolder);
                return shareHolder.ID;
            }
            return "OK";
        }

        public async Task<JsonResult> FindById(string id)
        {
            var data = await shareHolderRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Status = data.Status,
                StatusWnBh = data.StatusWnBh,
                CompanyID = data.CompanyID,
                Name = data.Name,
                TotalStock = data.TotalStock,
                ProsentaseSaham = data.ProsentaseSaham,
                Country = data.Country,
                Currency = data.Currency
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditService(ShareHolder model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = User.Identity.Name;
                model.ModifiedDate = DateTime.Now;
                await shareHolderRepository.UpdateAsync(model);
                return model.ID;
            }
            return "0";
        }
        
        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            var model = await shareHolderRepository.FindAsync(id);
            await shareHolderRepository.RemoveAsync(model);
            return "OK";
        }



        // GET: Organization/ShareHolders
        public ActionResult Index(string name)
        {
            //var data = shareHolderRepository.FindByName(name);
            //return View(data);
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<ShareHolder> dataGrid = shareHolderRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadShareHolders(string id) {
            var data = shareHolderRepository.FindByCompany(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ImportDataShareHolders()
        {
            return View();
        }
        public int GetDuplicateImportData(string id, string name)
        {
            var dataID = shareHolderRepository.FindByIdAndName(id, name);
            return dataID.Count();
        }
        [HttpPost]
        public async Task<ActionResult> UploadExcel()
        {
            ShareHolder shareHolder = new ShareHolder();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                HSSFWorkbook hssfwb = new HSSFWorkbook(file.InputStream);

                ISheet sheet = hssfwb.GetSheetAt(0);
                for (int row = 2; row <= sheet.LastRowNum; row++)
                {
                    try
                    {
                        int duplicate = GetDuplicateImportData(sheet.GetRow(row).GetCell(0).ToString(), sheet.GetRow(row).GetCell(1).ToString());
                        if (sheet.GetRow(row) != null && duplicate == 0)
                        {
                            shareHolder.ID = sheet.GetRow(row).GetCell(0).ToString();
                            shareHolder.CreatedBy = User.Identity.Name;
                            shareHolder.CreatedDate = DateTime.Now;
                            shareHolder.Name = sheet.GetRow(row).GetCell(1).ToString();
                            shareHolder.TotalStock = sheet.GetRow(row).GetCell(2).ToString();
                            shareHolder.Status = sheet.GetRow(row).GetCell(3).BooleanCellValue;
                            shareHolder.CompanyID = sheet.GetRow(row).GetCell(4).ToString();
                            await shareHolderRepository.AddAsync(shareHolder);
                        }
                    }
                    catch { }
                }
            }
            return RedirectToAction("index");
        }
        // GET: Organization/ShareHolders/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ShareHolder shareHolder = db.ShareHolders.Find(id);
        //    if (shareHolder == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(shareHolder);
        //}

        // GET: Organization/ShareHolders/Create
        public ActionResult Create()
        {
            ShareHolder shareHolder = new ShareHolder()
            {
                ID = "1"
            };
            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", shareHolder.CompanyID);

            return View(shareHolder);
        }

        // POST: Organization/ShareHolders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,TotalStock,StatusWnBh,Status,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] ShareHolder shareHolder)
        {
            if (ModelState.IsValid)
            {
                shareHolder.ID = Guid.NewGuid().ToString();
                shareHolder.CreatedBy = User.Identity.Name;
                shareHolder.CreatedDate = DateTime.Now;
                shareHolderRepository.AddAsync(shareHolder);
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(companyAddressRepository.GetAll(), "ID","Name", shareHolder.CompanyID);
            
            return View(shareHolder);
        }




        // GET: Organization/ShareHolders/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareHolder shareHolder = await shareHolderRepository.FindAsync(id);
            if (shareHolder == null)
            {
                return HttpNotFound();
            }

            ViewBag.CompanyID = new SelectList(companyRepository.GetAll(), "ID", "Name", shareHolder.CompanyID);

            return View(shareHolder);
        }

        // POST: Organization/ShareHolders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,TotalStock,StatusWnBh,Status,CompanyID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] ShareHolder shareHolder)
        {
            if (ModelState.IsValid)
            {
                shareHolder.ModifiedBy = User.Identity.Name;
                shareHolder.ModifiedDate = DateTime.Now;
                shareHolderRepository.UpdateAsync(shareHolder);
                return RedirectToAction("Index");
            }
            //ViewBag.CompanyID = new SelectList(db.Company, "ID", "Name", shareHolder.CompanyID);
            ViewBag.CompanyID = new SelectList(companyAddressRepository.GetAll(), "ID", "Name", shareHolder.CompanyID);

            return View(shareHolder);
        }

        // GET: Organization/ShareHolders/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ShareHolder shareHolder = await shareHolderRepository.FindAsync(id);
        //    if (shareHolder == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(shareHolder);
        //}

        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            ShareHolder shareHolder = await shareHolderRepository.FindAsync(p);
            await shareHolderRepository.RemoveAsync(shareHolder);
            return Json(p);
        }
    }
}
