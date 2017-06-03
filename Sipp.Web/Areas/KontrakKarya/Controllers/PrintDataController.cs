using EduSpot.Entity.Tables.Organization;
using EduSpot.Entity.Tables.Pkp2b;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Abstraction.Entity.Pkp2b;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Pkp2b;
using Esdm.Web.Areas.PKP2B.Models;
using Ews.Repository.Abstraction.MiningService;
using Ews.Repository.Concrete.MiningService;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.KontrakKarya.Controllers
{
    public class PrintDataController : Controller
    {
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
        private ITahapKegiatanBlokRepository tahapKegiatanBlokRepository = new TahapKegiatanBlokRepository();
        private ISumberDayaBatubaraRepository sumberDayaBatubaraRepository = new SumberDayaBatubaraRepository();
        private ICompanyHistoryPkp2bRepository companyHistoryPkp2bRepository = new CompanyHistoryPkp2bRepository();
        private IReportRepository reportRepo = new ReportRepository();
        private IBiayaEksplorasiRepository biayaEksplorasiRepository = new BiayaEksplorasiRepository();
        private IDetailKronologiTahapanRepository detailKronologiTahapanRepository = new DetailKronologiTahapanRepository();

        // GET: KontrakKarya/PrintData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DownloadViewPDF(string id)
        {
            return new Rotativa.UrlAsPdf(Url.Action("Print/" + id, "PrintData", new { area = "KontrakKarya" }))
            {
                FileName = "KontrakKaryaCompany.pdf",
                PageSize = Size.A4,
                PageHeight = 600,
                PageWidth = 200
            };
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

            List<TahapKegiatanBlok> tahapKegiatanBlok = tahapKegiatanBlokRepository.Get().Where(c => c.CompanyID == id).ToList();
            ViewBag.tahapKegiatanBlok = tahapKegiatanBlok;

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
    }
}