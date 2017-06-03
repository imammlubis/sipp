using EduSpot.Core.Repository.AngkutJual;
using EduSpot.Entity.Tables.AngkutJual;
using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.Organization;
using Esdm.Web.Areas.AngkutJual.Models;
using Ews.Repository.Abstraction.MiningService;
using Ews.Repository.Concrete.MiningService;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class IupOpAngkutJualListController : Controller
    {
        private ICompanyRepository companyRepository = new CompanyRepository();
        private ICompanyAddressRepository companyAddressRepository = new CompanyAddressRepository();
        private IFirstSkRepository firstSkRepository = new Esdm.Repository.Concrete.Entity.AngkutJual.FirstSkRepository();
        private IBKPMRepository bkpmRepository = new BKPMRepository();
        private IBannedSkRepository bannedSkRepository = new BannedSkRepository();
        private IAdjustedSkRepository adjustedSkRepository = new AdjustedSkRepository();
        private IShareHolderRepository shareHolderRepository = new ShareHolderRepository();

        private ICommisionerRepository commissionerRepository = new CommissionerRepository();
        private IManagementRepository managementRepository = new ManagementRepository();
        private IWarningLetterRepository warningLetterRepository = new WarningLetterRepository();

        private ICoalSourceSkRepository coalSourceSkRepository = new CoalSourceSkRepository();
        private IExtendedSkRepository extendedSkRepository = new ExtendedSkRepository();
        private ITempBannedSkRepository tempBannedSkRepository = new TempBannedSkRepository();
        private IReactivatedSkRepository reactivatedSkRepository = new ReactivatedSkRepository();
        private IBKPMRepository bKPMRepository = new BKPMRepository();
        private IETRecommendationRepository eTRecommendationRepository = new ETRecommendationRepository();
        private IAdditionalCooperationRepository additionalCooperationRepository = new AdditionalCooperationRepository();
        private ICNCCertificateRepository cNCCertificateRepository = new CNCCertificateRepository();
        private ICompanyFileRepository companyFileRepository = new CompanyFileRepository();

        private IReportRepository reportRepository = new ReportRepository();
        private IFirstSkSourceRepository firstSkSourceRepo = new FirstSkSourceRepository();
        private ISourceChangesRepository sourceChangesRepository = new SourceChangesRepository();
        private ISourceChangesSkSourceRepository sourceChangesSkSourceRepository = new SourceChangesSkSourceRepository();
        private IExtendedSkSourceRepository extendedSkSourceRepository = new ExtendedSkSourceRepository();
        private INotificationLogAngkutJualRepository notifLogRepository = new NotificationLogAngkutJualRepository();
        private ICompanyMiningModulDetailRepository companyMiningModulDetailRepository = new CompanyMiningModulDetailRepository();

        // GET: AngkutJual/IupOpAngkutJualList
        public ActionResult Index()
        {
            return View();
            // var data = iupRepo.GetList();
            // return View(data);
        }
        //public IQueryable<Company> GetAll()
        //{

        //    var listData = (from a in companyRepository.GetAll()
        //                    join b in companyAddressRepository.GetAll() on a.ID equals b.CompanyID
        //                    join c in firstSkRepository.GetAll() on a.ID equals c.CompanyID
        //                    select new
        //                    {
        //                        a.Name,
        //                        b.Address,
        //                        a.NPWP,
        //                        a.StatusIzin,
        //                        c.LetterNumber
        //                    }).ToList();
        //    IQueryable<string> query = listData.AsQueryable();
        //    return query;
        //}

        //public IQueryable<Company> ListIupOpAngkutJualList()
        //{
        //    var dataGrid = (from a in companyRepository.GetAll()
        //                   join b in companyAddressRepository.GetAll() on a.ID equals b.CompanyID
        //                   join c in firstSkRepository.GetAll() on a.ID equals c.CompanyID
        //                   select new
        //                   {
        //                       a.Name,
        //                       b.Address,
        //                       a.NPWP,
        //                       a.StatusIzin,
        //                       c.LetterNumber
        //                   }).ToList();
        //    //var query = repo.Get().ExecuteQuery<DepartmentViewModel>(String.Format(
        //    //@"SELECT a.dept as department, b.description
        //    //FROM acc_tblcostacc a 
        //    //join sys_department b on a.dept = b.department
        //    //WHERE a.entity_cd = '" + CurrentEntityCode + @"' AND acc_no = '" + accNo + @"'"));

        //    return
        //        dataGrid.;
        //}

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }


        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            //var dataGrid = from a in companyRepository.GetAll().AsEnumerable()
            //               join b in companyAddressRepository.GetAll().AsEnumerable()
            //               on a.ID equals b.CompanyID into group1
            //               from g1 in group1.DefaultIfEmpty()
            //               join c in firstSkRepository.GetAll().AsEnumerable() on a.ID equals c.CompanyID
            //               into group2
            //               from g2 in group2.DefaultIfEmpty()
            //               select new IupOpAngkutJualListViewModel
            //               {
            //                   ID = a.ID,
            //                   Name = a.Name,
            //                   Address = g1 == null ? String.Empty : g1.Address,
            //                   NPWP = a.NPWP,
            //                   StatusIzin = a.StatusIzin == true ? "Aktif" : "Non Aktif",
            //                   LetterNumber = g2 == null ? String.Empty : g2.LetterNumber,
            //                   SkNumber = g2 == null ? String.Empty : g2.SkNumber
            //               };
            var dataGrid = from a in companyMiningModulDetailRepository.Get().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable()
                           on a.CompanyID equals b.ID
                           join c in companyAddressRepository.GetAll().AsEnumerable() on b.ID equals c.CompanyID
                           into group1
                           from g1 in group1.DefaultIfEmpty()
                           join d in firstSkRepository.GetAll().AsEnumerable() on b.ID equals d.CompanyID
                           into group2
                           from g2 in group2.DefaultIfEmpty()
                           where a.MiningModuleID == "2"
                           select new IupOpAngkutJualListViewModel
                           {
                               ID = b.ID,
                               Name = b.Name,
                               Address = g1 == null ? String.Empty : g1.Address,
                               NPWP = b.NPWP,
                               StatusIzin = b.StatusIzin == true ? "Aktif" : "Non Aktif",
                               LetterNumber = g2 == null ? String.Empty : g2.LetterNumber,
                               SkNumber = g2 == null ? String.Empty : g2.SkNumber
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListSkEnd([DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in companyRepository.GetAll().AsEnumerable()
                           join ca in companyAddressRepository.GetAll().AsEnumerable() on a.ID equals ca.CompanyID
                           join b in firstSkRepository.GetAll().AsEnumerable() on a.ID equals b.CompanyID
                           join xxx in companyMiningModulDetailRepository.Get().AsEnumerable() on a.ID equals xxx.CompanyID
                           where (DateTime.Parse(b.SkEndDate.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(b.SkEndDate.ToString()).Year - DateTime.Now.Year) <= 6
                           && (DateTime.Parse(b.SkEndDate.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(b.SkEndDate.ToString()).Year - DateTime.Now.Year) >= 0
                           && xxx.MiningModuleID == "2"
                           //di cek sk awal end date nya. if habis cek sk perpanjangan.. tampilkan no sk perpanjangan
                           select new IupOpAngkutJualListTab2ViewModel
                           {
                               ID = a.ID,
                               Name = a.Name,
                               SkNumber = b.SkNumber,
                               SkDate = b.SkDate,
                               SkEndDate = b.SkEndDate,
                               SkDuration = ((DateTime.Parse(b.SkEndDate.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(b.SkEndDate.ToString()).Year - DateTime.Now.Year)).ToString()
                               ///SkDuration = DateTime.Parse(b.SkEndDate.ToString()).Subtract(DateTime.Now).ToString()
                           };
            var dataGrid2 = from a in companyRepository.GetAll().AsEnumerable()
                           join ca in companyAddressRepository.GetAll().AsEnumerable() on a.ID equals ca.CompanyID
                           join b in extendedSkRepository.GetAll().AsEnumerable() on a.ID equals b.CompanyID
                           where(DateTime.Parse(b.SkEndDate.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(b.SkEndDate.ToString()).Year - DateTime.Now.Year) <= 6
                           && (DateTime.Parse(b.SkEndDate.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(b.SkEndDate.ToString()).Year - DateTime.Now.Year) >= 0                           
                           select new IupOpAngkutJualListTab2ViewModel
                           {
                               ID = a.ID,
                               Name = a.Name,
                               SkNumber = b.SkNumber,
                               SkDate = b.SkDate,
                               SkEndDate = b.SkEndDate,
                               SkDuration = ((DateTime.Parse(b.SkEndDate.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(b.SkEndDate.ToString()).Year - DateTime.Now.Year)).ToString()
                               
                           };
            DataSourceResult result = dataGrid.Union(dataGrid2).ToDataSourceResult(request);
            //DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListBKPM([DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in bkpmRepository.GetAll().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable() on a.CompanyID equals b.ID
                           join c in companyAddressRepository.GetAll().AsEnumerable() on b.ID equals c.CompanyID
                           join d in firstSkRepository.GetAll().AsEnumerable() on b.ID equals d.CompanyID
                           select new IupOpAngkutJualListBKPM
                           {
                               ID = a.ID,
                               CompanyID = b.ID,
                               Name = b.Name,
                               Address = c.Address,
                               SkNumber = d.SkNumber,
                               LetterNumber = a.LetterNumber,
                               LetterDate = a.LetterDate,
                               BKPMAcceptanceDate = a.BKPMAcceptanceDate,
                               EvaluatorAcceptanceDate = a.EvaluatorAcceptanceDate,
                               LetterType = a.LetterType,
                               Status = a.Status,
                               AdditionalInformation = a.AdditionalInformation
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListDetailPeringatan([DataSourceRequest] DataSourceRequest request)
        {
            //var dataGrid = from a in warningLetterRepository.GetAll().AsEnumerable()
            //               join b in companyRepository.GetAll().AsEnumerable()
            //               on a.CompanyID equals b.ID into group1
            //               from g1 in group1.DefaultIfEmpty()
            //               join c in companyAddressRepository.GetAll().AsEnumerable()
            //               on a.CompanyID equals c.CompanyID into group2
            //               from g2 in group2.DefaultIfEmpty()
            //               where a.WarningDuration == "6" || a.WarningDuration == "3" || a.WarningDuration == "1"
            //               select new IupOpAngkutJualListDetailPeringatan
            //               {
            //                   ID = a.ID,
            //                   Name = g1.Name,
            //                   CompanyID = g1.ID,
            //                   Address = g2 == null ? String.Empty : g2.Address,
            //                   SkNumber = String.IsNullOrEmpty(a.LetterNumber) ? "" : a.LetterNumber,
            //                   SkDate2 = a.LetterDate,
            //                   WarningDuration = a.WarningDuration
            //               };
            //string yearNow = DateTime.Now.Year.ToString();
            ////DateTime.Parse("2017-12-14").ToString("dd/MM/yyyy")
            //DateTime tw1 = DateTime.Parse(DateTime.Parse(yearNow + "-04-22").ToString("yyyy-MM-dd"));
            //DateTime tw2 = DateTime.Parse(DateTime.Parse(yearNow + "-07-22").ToString("yyyy-MM-dd"));
            //DateTime tw3 = DateTime.Parse(DateTime.Parse(yearNow + "-10-22").ToString("yyyy-MM-dd"));
            //DateTime tw4 = DateTime.Parse(DateTime.Parse(yearNow + "-01-22").ToString("yyyy-MM-dd"));
            //DateTime Annual =  DateTime.Parse(DateTime.Parse(yearNow + "-01-22").ToString("yyyy-MM-dd"));

            //var dataGrid = from a in reportRepository.GetAll().AsEnumerable()
            //               join b in companyRepository.GetAll().AsEnumerable() on a.CompanyID equals b.ID
            //               where a.Q1 == 0 && ((DateTime)tw1 - DateTime.Now).TotalDays > 0
            //               where a.Q2 == 0 && ((DateTime)tw2 - DateTime.Now).TotalDays > 0
            //               where a.Q3 == 0 && ((DateTime)tw3 - DateTime.Now).TotalDays > 0
            //               where a.Q4 == 0 && ((DateTime)tw4 - DateTime.Now).TotalDays > 0
            //               where String.IsNullOrEmpty(a.Annual.ToString()) || a.Annual == 0 && ((DateTime)Annual - DateTime.Now).TotalDays > 0
            //               where String.IsNullOrEmpty(a.Rkab.ToString()) || a.Rkab == 0 && ((DateTime)tw1 - DateTime.Now).TotalDays > 0
            //               select new IupOpAngkutJualListDetailPeringatan
            //               {
            //                   ID = a.ID,
            //                   Q1 = String.IsNullOrEmpty(a.Q1.ToString()) || a.Q1 == 0 && ((DateTime)tw1 - DateTime.Now).TotalDays > 0 ? "Belum Lapor" : "Sudah Lapor",
            //                   Q2 = String.IsNullOrEmpty(a.Q2.ToString()) || a.Q2 == 0 && ((DateTime)tw2 - DateTime.Now).TotalDays > 0 ? "Belum Lapor" : "Sudah Lapor",
            //                   Q3 = String.IsNullOrEmpty(a.Q3.ToString()) || a.Q3 == 0 && ((DateTime)tw3 - DateTime.Now).TotalDays > 0 ? "Belum Lapor" : "Sudah Lapor",
            //                   Q4 = String.IsNullOrEmpty(a.Q4.ToString()) || a.Q4 == 0 && ((DateTime)tw4 - DateTime.Now).TotalDays > 0 ? "Belum Lapor" : "Sudah Lapor",
            //                   Annual = String.IsNullOrEmpty(a.Annual.ToString()) || a.Annual == 0 && ((DateTime)Annual - DateTime.Now).TotalDays > 0 ? "Belum Lapor" : "Sudah Lapor",
            //                   Rkab = String.IsNullOrEmpty(a.Rkab.ToString()) || a.Rkab == 0 && ((DateTime)tw1 - DateTime.Now).TotalDays > 0 ? "Belum Lapor" : "Sudah Lapor",
            //                   Name = b.Name,
            //                   CompanyID = a.CompanyID                               
            //               };
            
            var dataGrid = from a in companyMiningModulDetailRepository.Get().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable()
                           on a.CompanyID equals b.ID
                           join c in companyAddressRepository.GetAll().AsEnumerable() on b.ID equals c.CompanyID
                           into group1
                           from g1 in group1.DefaultIfEmpty()
                           join d in firstSkRepository.GetAll().AsEnumerable() on b.ID equals d.CompanyID
                           into group2
                           from g2 in group2.DefaultIfEmpty()
                           where a.MiningModuleID == "2"
                           select new IupOpAngkutJualListViewModel
                           {
                               ID = b.ID,
                               Name = b.Name,
                               Address = g1 == null ? String.Empty : g1.Address,
                               Email = g1 == null ?String.Empty: g1.Email,
                               NoHp = g1 == null ? String.Empty: g1.MobileNumber
                           };

            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListDetail(string id, [DataSourceRequest] DataSourceRequest request)
        {
            //var dataGrid = tahapKegiatanRepository.Get().Where(c => c.CompanyID == id);
            var dataGrid = from a in warningLetterRepository.GetAll().AsEnumerable()
                           where a.CompanyID == id
                           select new
                           {
                               ID = a.ID,
                               AdditionalInfo = a.AdditionalInfo,
                               WarningType = a.WarningType,
                               WarningDuration = a.WarningDuration
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TestGridMasterDetail()
        {
            return View();
        }


        public ActionResult DownloadViewPDF(string id)
        {
            return new Rotativa.UrlAsPdf(Url.Action("PrintTemplate/" + id, "IupOpAngkutJualList", new { area = "AngkutJual" }))
            {
                FileName = "PrintSk.pdf",
                PageSize = Size.A4,
                PageHeight = 600,
                PageWidth = 200
            };
        }

        [HttpGet]
        public ActionResult PrintTemplate(string id)
        {
            var data = (from a in companyRepository.GetAll().AsEnumerable()
                        join b in companyAddressRepository.GetAll().AsEnumerable()
                        on a.ID equals b.CompanyID
                        where a.ID == id
                        select new IupOpPrintViewModel
                        {
                            NoUrutBerkas = a.NoUrutBerkas,
                            NamaPerusahaan = a.Name,
                            AlamatPerusahaan = b.Address,
                            NoTel = b.TelNumber,
                            Email = b.Email,
                            NPWP = a.NPWP,
                            Fax = b.Fax,
                            MobileNo = b.MobileNumber,
                            StatusIzin = b.Status == true ? "Aktif" : "Non Aktif",
                            Keterangan = b.AdditionalInfo
                        }).ToList();
            IupOpPrintViewModel model = new IupOpPrintViewModel();
            List<ShareHolder> sh = shareHolderRepository.FindByCompany(id).ToList();
            ViewData["ShareHolder"] = sh;

            List<Commissioner> commissioner = commissionerRepository.FindByCompany(id).ToList();
            ViewData["Commissioner"] = commissioner;

            List<Management> management = managementRepository.FindByCompany(id).ToList();
            ViewData["Management"] = management;

            List<FirstSk> firstSk = firstSkRepository.FindByCompanyId(id).ToList();
            ViewData["FirstSk"] = firstSk;

            var fss = (from a in firstSkSourceRepo.GetAll().AsEnumerable()
                       join b in firstSkRepository.GetAll().AsEnumerable()
                       on a.FirstSkID equals b.ID
                       where b.CompanyID == id
                       select new FirstSkSource
                       {
                           CompanyName = a.CompanyName,
                           Volume = a.Volume
                       }).ToList();

            ViewData["FirstSkSource"] = fss;

            var sc = (from a in sourceChangesRepository.GetAll().AsEnumerable()
                      where a.CompanyID == id
                      select new SourceChangesViewModel
                      {
                          LetterNumber = a.LetterNumber,
                          LetterDate = a.LetterDate.Value.ToString("dd/MM/yyyy"),
                          SkNumber = a.SkNumber,
                          SkDate = a.SkDate.Value.ToString("dd/MM/yyyy"),
                          Sumber = string.Join(", ", sourceChangesSkSourceRepository.GetAll()
                          .Where(c => c.SourceChangesID == a.ID)
                          .Select(c => c.CompanyName.ToString() + " (" + c.Volume + ")")
                          )
                      }).ToList();

            ViewData["SourceChanges"] = sc;

            var es = (from a in extendedSkRepository.GetAll().AsEnumerable()
                      where a.CompanyID == id
                      select new ExtendedSkViewModel
                      {
                          LetterNumber = a.LetterNumber,
                          LetterDate = a.LetterDate,
                          SkNumber = a.SkNumber,
                          SkDate = a.SkDate,
                          SkDuration = a.SkDuration,
                          SkEndDate = a.SkEndDate,
                          SertifikatCNC = a.SertifikatCNC,
                          Sumber = string.Join(", ", extendedSkSourceRepository.GetAll()
                          .Where(c => c.ExtendedSkID == a.ID)
                          .Select(c => c.CompanyName.ToString() + " (" + c.Volume + ")")
                          )
                      }).ToList();
            ViewData["ExtendedSk"] = es;
            
            List<BannedSk> bannedSk = bannedSkRepository.FindByCompany(id).ToList();
            ViewData["BannedSk"] = bannedSk;

            //List<Report> report = reportRepository.GetAll().Where(c => c.CompanyID == id).ToList();
            var report = (from a in reportRepository.GetAll().AsEnumerable()
                          join b in companyRepository.GetAll().AsEnumerable() on a.CompanyID equals b.ID
                          where b.ID == id
                          select new ReportViewModel
                          {
                              ID = a.ID,
                              CompanyName = b.Name,
                              Q1 = a.Q1,
                              PersenQ1 = (a.Q1 * 100) / a.Rkab,
                              Q2 = a.Q2,
                              PersenQ2 = (a.Q1 * 100) / a.Rkab,
                              Q3 = a.Q3,
                              PersenQ3 = (a.Q1 * 100) / a.Rkab,
                              Q4 = a.Q4,
                              PersenQ4 = (a.Q1 * 100) / a.Rkab,
                              Annual = a.Annual,
                              Rkab = a.Rkab,
                              Year = a.Year

                          }).ToList();
            ViewData["Report"] = report;


            //ViewData["report"] = reports;


            return View(data[0]);
        }

        // POST: Organization/Companies/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string p)
        {
            BKPM bkpm = await bkpmRepository.FindAsync(p);
            await bkpmRepository.RemoveAsync(bkpm);
            return Json(p);
        }

    }
}