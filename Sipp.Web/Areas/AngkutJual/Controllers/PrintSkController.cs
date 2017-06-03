using EduSpot.Entity.Tables.AngkutJual;
using EduSpot.Entity.Tables.Organization;
using EduSpot.Entity.Tables.Pkp2b;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.Organization;
using Esdm.Web.Areas.AngkutJual.Models;
using Ews.Repository.Abstraction.MiningService;
using Ews.Repository.Concrete.MiningService;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class PrintSkController : Controller
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

        

        // GET: AngkutJual/PrintSk
        public ActionResult Index(string id)
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

            List<AdjustedSk> adjustedSk = adjustedSkRepository.FindByCompany(id).ToList();
            ViewData["AdjustedSk"] = adjustedSk;

            List<TempBannedSk> tempBannedSk = tempBannedSkRepository.FindByCompany(id).ToList();
            ViewData["TempBannedSk"] = tempBannedSk;

            List<BannedSk> bannedSk = bannedSkRepository.FindByCompany(id).ToList();
            ViewData["BannedSk"] = bannedSk;

            List<ReactivatedSk> reactivatedSk = reactivatedSkRepository.FindByCompany(id).ToList();
            ViewData["ReactivatedSk"] = reactivatedSk;

            List<ETRecommendation> eTRecommendation = eTRecommendationRepository.FindByCompany(id).ToList();
            ViewData["ETRecommendation"] = eTRecommendation;
            List<AdditionalCooperation> additionalCooperation = additionalCooperationRepository.FindByCompany(id).ToList();
            ViewData["AdditionalCooperation"] = additionalCooperation;
            List<CNCCertificate> cNCCertificate = cNCCertificateRepository.FindByCompany(id).ToList();
            ViewData["CNCCertificate"] = cNCCertificate;

            return View(data[0]);
        }


        public ActionResult DownloadViewPDF(string id)
        {
            return new Rotativa.UrlAsPdf(Url.Action("PrintTemplate/" + id, "PrintSk", new { area = "AngkutJual" }))
            {
                FileName = "PrintSk.pdf",
                PageSize = Size.A4,
                PageHeight = 600,
                PageWidth = 200
            };
        }

        public ActionResult DownloadViewPDFPeringatan(string id)
        {
            return new Rotativa.UrlAsPdf(Url.Action("Print/" + id, "PrintSk", new { area = "AngkutJual" }))
            {
                FileName = "PrintPeringatan.pdf",
                PageSize = Size.A4,
                PageHeight = 600,
                PageWidth = 200,
                PageOrientation = Orientation.Landscape
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

        public ActionResult Print(string id)
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
            List<WarningLetter> warningLetter = warningLetterRepository.GetAll().Where(c => c.CompanyID == id).ToList();
            ViewBag.WarningLetter = warningLetter;

            return View(data[0]);
        }
    }
}