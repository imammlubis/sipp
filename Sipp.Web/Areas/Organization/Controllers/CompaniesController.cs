using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System;
using System.Threading.Tasks;
using EduSpot.Core.Infrastructure;
using System.Web;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Esdm.Web.Areas.Organization.Controllers
{
    public class CompanyTestingViewModel {
        public string ID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
    }

    //[Authorize(Roles = "AngkutJualAdmin")]
    public class CompaniesController : Controller
    {
        private ICompanyRepository companyRepository = new CompanyRepository();
        private ICompanyAddressRepository companyAddressRepository = new CompanyAddressRepository();
        private IIupTypeRepository iupTypeRepository = new IupTypeRepository();
        private ICompanyHistoryRepository companyHistoryRepository = new CompanyHistoryRepository();
        private ICompanyAddressHistoryRepository companyAddressHistoryRepository = new CompanyAddressHistoryRepository();

        // GET: Organization/Companies
        //public async Task<ActionResult> Index(string name)
        //{
        //    var data = companyRepository.FindByName(name);
        //    return View(await data.ToListAsync());
        //}
        public async Task<string> SaveCompanyGeneralInfo(CompanyGeneralInfoViewModel model) {           
            var company = await companyRepository.FindAsync(model.ID);
            company.StatusIzin = model.StatusIzin == "1" ? true: false;            
            company.Name = model.Name;
            company.NPWP = model.NPWP;
            company.NoUrutBerkas = model.NoUrutBerkas;
            
            await companyRepository.UpdateAsync(company);

            //save history
            var companyHistory = new CompanyHistory() {
                CompanyID = company.ID,
                Name = company.Name,
                NPWP = company.NPWP,
                NoUrutBerkas = company.NoUrutBerkas,
                StatusIzin = company.StatusIzin,
                CreatedBy = User.Identity.Name,
                CreatedDate = DateTime.Now          
            };
            await companyHistoryRepository.AddAsync(companyHistory);

            var addresses = companyAddressRepository.GetAll().Where(s => s.CompanyID == model.ID);
            if (addresses.Count() > 0) {
                var address = await addresses.FirstOrDefaultAsync();
                address.MobileNumber = model.MobileNumber;
                address.Email = model.Email;
                address.TelNumber = model.TelNumber;
                address.Address = model.Address;
                address.AdditionalInfo = model.AdditionalInfo;
                address.Website = model.Website;
                address.Fax = model.Fax;
                address.CPName = model.CPName;
                await companyAddressRepository.UpdateAsync(address);

                var addressHistory = new CompanyAddressHistory() {
                    CompanyID = model.ID,
                    MobileNumber = address.MobileNumber,
                    Email = address.Email,
                    TelNumber = address.TelNumber,
                    Address = address.Address,
                    AdditionalInfo = address.AdditionalInfo,
                    CreatedDate = DateTime.Now,
                    CreatedBy = User.Identity.Name                                   
                };
                await companyAddressHistoryRepository.AddAsync(addressHistory);
            }
            return "1";
        }
        public ActionResult Index()
        {
            var data = companyRepository.GetList();
            return View(data.ToList());
        }
        public ActionResult Testing()
        {
            return View();
            //var data = companyRepository.GetList();
            //return View(data.ToList());
        }
        public JsonResult ListTesting([DataSourceRequest] DataSourceRequest request)
        {
            var companies = companyRepository.GetAll().AsEnumerable();
            var address = companyAddressRepository.GetAll().AsEnumerable();
            var data = from c in companies
                       join a in address on c.ID equals a.CompanyID
                       select new CompanyTestingViewModel
                       {
                           ID = a.ID,
                           CompanyName = c.Name,
                           Address = a.Address
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public int GetDuplicateImportData(string id, string name)
        {
            var dataID = companyRepository.FindByIdAndName(id, name);
            return dataID.Count();
        }
        [HttpPost]
        public async Task<ActionResult> UploadExcel()
        {
            Company company = new Company();

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                HSSFWorkbook hssfwb = new HSSFWorkbook(file.InputStream);

                ISheet sheet = hssfwb.GetSheetAt(0);
                for (int row = 2; row <= sheet.LastRowNum; row++)
                {
                    int duplicate = GetDuplicateImportData(sheet.GetRow(row).GetCell(0).ToString(), sheet.GetRow(row).GetCell(1).ToString());
                    if (sheet.GetRow(row) != null && duplicate == 0)
                    {try
                        {
                            company.ID = sheet.GetRow(row).GetCell(0).ToString();
                            company.CreatedBy = User.Identity.Name;
                            company.CreatedDate = DateTime.Now;
                            company.Name = sheet.GetRow(row).GetCell(1).ToString();
                            company.NPWP = sheet.GetRow(row).GetCell(2).ToString();
                            //company.StatusIzin = sheet.GetRow(row).GetCell(3).BooleanCellValue;
                            company.TahapIup = sheet.GetRow(row).GetCell(3).ToString();
                            company.NoUrutBerkas = sheet.GetRow(row).GetCell(4).ToString();
                            company.IupTypeID = sheet.GetRow(row).GetCell(5).ToString();
                            await companyRepository.AddAsync(company);
                        }
                        catch { }
                    }
                }
            }
            return RedirectToAction("ImportDataCompany");
        }

        public ActionResult ImportDataCompany()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<Company> dataGrid = companyRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListCompanyForTesting() {
            var companies = companyRepository.GetAll();
            return Json(new { data = companies }, JsonRequestBehavior.AllowGet);
        }
        // GET: Organization/Companies/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Company company = db.Company.Find(id);
        //    if (company == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(company);
        //}

        // GET: Organization/Companies/Create
        public ActionResult Create()
        {
            Company company = new Company()
            {
                ID = "1"
            };

            ViewBag.IupTypeID = new SelectList(iupTypeRepository.GetAll(), "ID", "Name", company.IupTypeID);
            //ViewBag.IupTypeID = new SelectList(db.IupTypes, "ID", "Name");
            return View(company);
        }

        // POST: Organization/Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,NPWP,StatusIzin,TahapIup,NoUrutBerkas,IupTypeID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        Company company)
        {
            if (ModelState.IsValid)
            {
                company.ID = Guid.NewGuid().ToString();
                company.CreatedBy = User.Identity.Name;
                company.CreatedDate = DateTime.Now;
                await companyRepository.AddAsync(company);
                return RedirectToAction("Index");
            }
            return View(company);
        }

        [HttpPost]
        public async Task<string> CreateService(
        Company company)
        {
            if (ModelState.IsValid)
            {
                company.ID = Guid.NewGuid().ToString();
                company.CreatedBy = User.Identity.Name;
                company.CreatedDate = DateTime.Now;
                await companyRepository.AddAsync(company);
                return company.ID;
            }
            return null;
        }


        // GET: Organization/Companies/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Company company = db.Company.Find(id);
            Company company = await companyRepository.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.IupTypeID = new SelectList(iupTypeRepository.GetAll(), "ID", "Name", company.IupTypeID);
            return View(company);
        }

        // POST: Organization/Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,NPWP,StatusIzin,TahapIup,NoUrutBerkas,IupTypeID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        Company company)
        {
            if (ModelState.IsValid)
            {
                company.ModifiedBy = User.Identity.Name;
                company.ModifiedDate = DateTime.Now;
                companyRepository.UpdateAsync(company);
                return RedirectToAction("Index");
            }
            ViewBag.IupTypeID = new SelectList(iupTypeRepository.GetAll(), "ID", "Name", company.IupTypeID);
            return View(company);
        }


        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            Company company = await companyRepository.FindAsync(p);
            await companyRepository.RemoveAsync(company);
            return Json(p);
        }

    }

    public class CompanyGeneralInfoViewModel {
        public string ID { get; set; }
        public string Name { get; set; }
        public string NPWP { get; set; }
        public string StatusIzin { get; set; }
        public string TahapIup { get; set; }
        public string NoUrutBerkas { get; set; }
        public string IupTypeID { get; set; }
        public string Address { get; set; }
        public string TelNumber { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Website { get; set; }
        public string AdditionalInfo { get; set; }
        public string Fax { get; set; }
        public string CPName { get; set; }
        /*
           $scope.companyModel.name = data.company[0].Name;
            $scope.companyModel.nourutberkas = data.company[0].NoUrutBerkas;
            $scope.companyModel.npwp = data.company[0].NPWP;
            $scope.companyModel.statusizin = data.company[0].StatusIzin;
            $scope.companyModel.tahapiup = data.company[0].TahapIup;
            $scope.companyModel.iuptypeid = data.company[0].IupTypeID;
            $scope.companyModel.address = data.company[0].Address;

         */
    }
}
