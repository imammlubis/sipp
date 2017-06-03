using EduSpot.Entity.Tables.Organization;
using EduSpot.Entity.Tables.Pkp2b;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Kk;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Abstraction.Entity.Pkp2b;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.Kk;
using Esdm.Repository.Concrete.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Pkp2b;
using Esdm.Web.Areas.PKP2B.Models;
using Esdm.Web.Utils;
using Ews.Repository.Abstraction.MiningService;
using Ews.Repository.Concrete.MiningService;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.KontrakKarya.Controllers
{
    //[Authorize(Roles = "PKP2BAdmin")]
    public class MelihatDaftarKeseluruhanController : Controller
    {
        private EmailServices emailService = new EmailServices();
        private Esdm.Web.Utils.SmsService smsService = new Esdm.Web.Utils.SmsService();
        private ICompanyRepository companyRepository = new CompanyRepository();
        private ICompanyAddressRepository companyAddressRepository = new CompanyAddressRepository();
        private IShareHolderRepository shareHolderRepository = new ShareHolderRepository();
        private ICommisionerRepository commissionerRepository = new CommissionerRepository();
        private IManagementRepository managementRepository = new ManagementRepository();
        private IDataPkp2bRepository dataPkp2bRepository = new DataPkp2bRepository();
        private ICompanyMiningModulDetailRepository companyMiningModulDetailRepository = new CompanyMiningModulDetailRepository();
        private IAktaRepository aktaRepo = new AktaRepository();
        private ILingkunganIppkhDanLainnyaRepository lingkunganRepository = new LingkunganIppkhDanLainnyaRepository();
        private IKronologiTahapanRepository kronologiTahapanRepository = new KronologiTahapanRepository();
        private ICadanganBatubaraRepository cadanganBatubaraRepository = new CadanganBatubaraRepository();
        private IKualitasBatubaraRepository kualitasBatubaraRepository = new KualitasBatubaraRepository();
        private ITahapKegiatanRepository tahapKegiatanRepository = new TahapKegiatanRepository();
        private ISumberDayaBatubaraRepository sumberDayaBatubaraRepository = new SumberDayaBatubaraRepository();
        private ICompanyHistoryPkp2bRepository companyHistoryPkp2bRepository = new CompanyHistoryPkp2bRepository();
        private IReportRepository reportRepo = new ReportRepository();
        private IBiayaEksplorasiRepository biayaEksplorasiRepository = new BiayaEksplorasiRepository();
        private IDetailKronologiTahapanRepository detailKronologiTahapanRepository = new DetailKronologiTahapanRepository();
        private IDataKkRepository dataKkRepository = new DataKkRepository();

        // GET: KontrakKarya/MelihatDaftarKeseluruhan
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Print(string id)
        {
            var company = (from a in companyRepository.GetAll().AsEnumerable()
                           join b in companyAddressRepository.GetAll().AsEnumerable()
                           on a.ID equals b.CompanyID
                           where b.CompanyID == id
                           select new GeneralInformationViewModel
                           {
                               ID = a.ID,
                               Name = a.Name,
                               Npwp = a.NPWP,
                               Telp = b.TelNumber,
                               Fax = b.Fax,
                               Alamat = b.Address,
                               Email = b.Email,
                               Website = b.Website,
                               CpNama = b.CPName,
                               CpHp = b.MobileNumber == "+62" ? "" : b.MobileNumber
                           }).ToList();
            List<ShareHolder> sh = shareHolderRepository.FindByCompany(id).ToList();
            ViewData["ShareHolder"] = sh;

            List<Commissioner> commissioner = commissionerRepository.FindByCompany(id).ToList();
            ViewData["Commissioner"] = commissioner;

            List<Management> management = managementRepository.FindByCompany(id).ToList();
            ViewData["Management"] = management;

            List<DataPkp2b> datapkp2b = dataPkp2bRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.datapkp2b = datapkp2b;

            List<Akta> dataAkta = aktaRepo.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.dataAkta = dataAkta;

            List<TahapKegiatan> tahapKegiatan = tahapKegiatanRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.tahapKegiatan = tahapKegiatan;

            List<DetailKronologiTahapan> detailKronologiTahapan = detailKronologiTahapanRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.detailKronologiTahapan = detailKronologiTahapan;

            List<SumberDayaBatubara> sdb = sumberDayaBatubaraRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.sdb = sdb;

            List<KronologiTahapan> kronologiTahapan = kronologiTahapanRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.kronologiTahapan = kronologiTahapan;

            List<LingkunganIppkhDanLainnya> lingkungan = lingkunganRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.lingkungan = lingkungan;

            List<CadanganBatubara> cadanganBatubara = cadanganBatubaraRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.cadanganBatubara = cadanganBatubara;

            List<KualitasBatubara> kualitasBatubara = kualitasBatubaraRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.kualitasBatubara = kualitasBatubara;

            List<BiayaEksplorasi> biayaEksplorasi = biayaEksplorasiRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.biayaEksplorasi = biayaEksplorasi;

            return View(company[0]);
        }

        [HttpPost]
        public JsonResult ListHistory(string id, [DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in companyMiningModulDetailRepository.Get().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable()
                           on a.CompanyID equals b.ID
                           join c in companyHistoryPkp2bRepository.Get().AsEnumerable() on b.ID equals c.CompanyID
                           //into group1
                           //from g1 in group1.DefaultIfEmpty()
                           where c.CompanyID == id
                           select new HistoryViewModel
                           {
                               IdCompanyHistoryPkp2b = b.ID,
                               Name = b.Name,
                               Address = c == null ? String.Empty : c.Address,
                               NPWP = c == null ? String.Empty : c.NPWP,
                               Telphone = c == null ? String.Empty : c.Telphone,
                               Fax = c == null ? String.Empty : c.Fax,
                               Email = c == null ? String.Empty : c.Email,
                               Website = c == null ? String.Empty : c.Website,
                               CpName = c == null ? String.Empty : c.CpName,
                               CpMobileNo = c == null ? String.Empty : c.CpMobileNo,
                               CompanyID = id,
                               CreatedBy = c.CreatedBy,
                               CreatedDate = c.CreatedDate
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListReport([DataSourceRequest] DataSourceRequest request)
        {
            var companies = companyRepository.GetAll().AsEnumerable();
            var report = reportRepo.GetAll().AsEnumerable();
            var data = from r in report.AsEnumerable()
                       join c in companies.AsEnumerable() on r.CompanyID equals c.ID into group1
                       from g1 in group1
                       select new AngkutJual.Controllers.ReportViewModel
                       {
                           CompanyId = g1.ID.ToString(),
                           CompanyName = g1.Name,
                           Year = r.Year,
                           Rkab = r.Rkab,
                           Q1 = r.Q1,
                           Q2 = r.Q2,
                           Q3 = r.Q3,
                           Q4 = r.Q4,
                           Annual = r.Annual,
                           StatusRespond = r.StatusRespond
                       };
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in companyMiningModulDetailRepository.Get().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable()
                           on a.CompanyID equals b.ID
                           join c in companyAddressRepository.GetAll().AsEnumerable() on b.ID equals c.CompanyID
                           into group1
                           from g1 in group1.DefaultIfEmpty()
                           join d in dataKkRepository.Get().AsEnumerable() on b.ID equals d.CompanyID
                           into group2
                           from g2 in group2.DefaultIfEmpty()
                           where a.MiningModuleID == "5"
                           select new GeneralInformationViewModel
                           {
                               ID = b.ID,
                               Name = b.Name,
                               Alamat = g1 == null ? String.Empty : g1.Address,
                               Provinsi = g2 == null ? String.Empty : g2.Provinsi,
                               LuasWilayahAwal = g2 == null ? 0 : g2.LuasWilayahAwal,
                               TanggalKontrak = g2.TanggalKontrak,
                               TanggalBerakhir = g2.TanggalBerakhir
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListDetail(string id, [DataSourceRequest] DataSourceRequest request)
        {
            //var dataGrid = tahapKegiatanRepository.Get().Where(c => c.CompanyID == id);
            var dataGrid = from a in tahapKegiatanRepository.Get().AsEnumerable()
                           where a.CompanyID == id
                           select new TahapKegiatanViewModel
                           {
                               IDTahapKegiatan = a.ID,
                               Tahap = a.Tahap,
                               LuasDipertahankan = a.LuasDipertahankan
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TestGridMasterDetail()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ListSkEnd([DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in companyMiningModulDetailRepository.Get().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable()
                           on a.CompanyID equals b.ID
                           join c in companyAddressRepository.GetAll().AsEnumerable() on b.ID equals c.CompanyID
                           into group1
                           from g1 in group1.DefaultIfEmpty()
                           join d in dataKkRepository.Get().AsEnumerable() on b.ID equals d.CompanyID
                           into group2
                           from g2 in group2.DefaultIfEmpty()
                           where a.MiningModuleID == "5" &&
                           (DateTime.Parse(g2.TanggalBerakhir.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(g2.TanggalBerakhir.ToString()).Year - DateTime.Now.Year) <= 3
                           && g2.TanggalKontrak <= g2.TanggalBerakhir
                           && (DateTime.Parse(g2.TanggalBerakhir.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(g2.TanggalBerakhir.ToString()).Year - DateTime.Now.Year) >= 0
                           select new GeneralInformationViewModel
                           {
                               SkEndId = b.ID,
                               Name = b.Name,
                               Alamat = g1 == null ? String.Empty : g1.Address,
                               Provinsi = g2 == null ? String.Empty : g2.Provinsi,
                               LuasWilayahAwal = g2 == null ? 0 : g2.LuasWilayahAwal,
                               TanggalKontrak = g2.TanggalKontrak,
                               TanggalBerakhir = g2.TanggalBerakhir,
                               Email = g1.Email,
                               CpHp = g1.MobileNumber,
                               JangkaWaktu = ((DateTime.Parse(g2.TanggalBerakhir.ToString()).Month - DateTime.Now.Month) + 12 * (DateTime.Parse(g2.TanggalBerakhir.ToString()).Year - DateTime.Now.Year))
                           };

            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<string> SendAlert(string id, string type)
        {
            List<string> dests = new List<string>();
            List<string> ccs = new List<string>();
            var company = (from a in companyRepository.GetAll().AsEnumerable()
                           join b in companyAddressRepository.GetAll().AsEnumerable()
                           on a.ID equals b.CompanyID
                           join c in dataKkRepository.Get().AsEnumerable()
                           on a.ID equals c.CompanyID
                           where a.ID == id
                           select new GeneralInformationViewModel
                           {
                               Name = a.Name,
                               Email = b.Email,
                               CpHp = b.MobileNumber,
                               TanggalBerakhir = c.TanggalBerakhir

                           }).ToList();
            var message = "SK dari " + company[0].Name + " akan berakhir pada " + company[0].TanggalBerakhir.Value.ToString("dd/MM/yyyy") + ". Silahkan mengajukan permohonan perpanjangan. ESDM-MINERBA";
            var subject = "Masa Sk Akan Habis";
            var body = message;
            if (type == "SMS")
            {
                if (!String.IsNullOrEmpty(company[0].CpHp))
                {
                    smsService.SendSms(new SmsContent()
                    {
                        To = company[0].CpHp,
                        Body = body
                    });
                }
            }
            else if (type == "EMAIL")
            {
                if (!String.IsNullOrEmpty(company[0].Email))
                {
                    dests.Add(company[0].Email);
                }
                //try
                //{
                var result = emailService.SendAsyncDefault(new EmailContent
                {
                    Subject = subject,
                    Destination = dests,
                    CC = ccs,
                    Body = body
                });
                WriteLog(result.ToString());
            }
            else if (type == "EMAIL&SMS")
            {
                if (!String.IsNullOrEmpty(company[0].CpHp))
                {
                    smsService.SendSms(new SmsContent()
                    {
                        To = company[0].CpHp,
                        Body = body
                    });
                }
                if (!String.IsNullOrEmpty(company[0].Email))
                {
                    dests.Add(company[0].Email);
                }
                //try
                //{
                var result = emailService.SendAsyncDefault(new EmailContent
                {
                    Subject = subject,
                    Destination = dests,
                    CC = ccs,
                    Body = body
                });
                WriteLog(result.ToString());
            }
            //var notifModel = new NotificationLogAngkutJual
            //{
            //    IdNotificationLog = Guid.NewGuid().ToString(),
            //    NotificationLogDate = DateTime.Now,
            //    Email = model.DestinationEmail,
            //    MobileNo = model.DestinationMobileNo,
            //    TglSuratPeringatan = null,
            //    TglAkhirPeringatan = null,
            //    CompanyId = model.CompanyID,
            //    NotificationsContent = model.Keterangan,
            //    CreatedDate = DateTime.Now,
            //    CreatedBy = User.Identity.Name

            //};

            //var notifLog = await notifLogRepository.AddAsync(notifModel);
            //}
            //catch (Exception ex){
            //    WriteLog(ex.ToString());
            //}
            return "sent";
        }
        public void WriteLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + message.Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public ActionResult ViewHistory(string id)
        {
            ViewBag.idx = id;
            return View();
        }
    }
}