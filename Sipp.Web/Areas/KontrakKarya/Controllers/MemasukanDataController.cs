using EduSpot.Entity.Tables.Kk;
using EduSpot.Entity.Tables.Organization;
using EduSpot.Entity.Tables.Pkp2b;
using Esdm.Repository.Abstraction.Entity.Kk;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Abstraction.Entity.Pkp2b;
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
    [Authorize(Roles = "KKAdmin")]
    public class MemasukanDataController : Controller
    {
        private EmailServices emailService = new EmailServices();
        private SmsService smsService = new SmsService();
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
        private IDetailKronologiTahapanRepository detailKronologiTahapanRepository = new DetailKronologiTahapanRepository();
        private ICadanganBatubaraRepository cadanganBatubaraRepository = new CadanganBatubaraRepository();
        private IKualitasBatubaraRepository kualitasBatubaraRepository = new KualitasBatubaraRepository();
        private ITahapKegiatanRepository tahapKegiatanRepository = new TahapKegiatanRepository();
        private ITahapKegiatanBlokRepository tahapKegiatanBlokRepository = new TahapKegiatanBlokRepository();
        private ISumberDayaBatubaraRepository sumberDayaBatubaraRepository = new SumberDayaBatubaraRepository();
        private IHistoryDataPkp2bRepository historyDataPkp2bRepository = new HistoryDataPkp2bRepository();
        private ICompanyHistoryPkp2bRepository companyHistoryPkp2bRepository = new CompanyHistoryPkp2bRepository();
        private IBiayaEksplorasiRepository biayaEksplorasiRepo = new BiayaEksplorasiRepository();
        private IDataKkRepository dataKkRepository = new DataKkRepository();
        private IKadarRepository kadarRepository = new KadarRepository();
        // GET: KontrakKarya/MemasukanData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            ViewBag.idx = id;
            return View();
        }

        [HttpPost]
        public JsonResult ListHistoryDataKk(string id, [DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = historyDataPkp2bRepository.Get().Where(c => c.CompanyID == id).OrderByDescending(c => c.CreatedDate);
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditServiceGeneralInformation(GeneralInformationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Company company = new Company();
                company.ID = model.ID;
                company.Name = model.Name;
                company.NPWP = model.Npwp;
                company.ModifiedDate = DateTime.Now;
                var resultCompany = await companyRepository.UpdateAsync(company);

                CompanyAddress ca = companyAddressRepository.GetAll().Where(c => c.CompanyID == model.ID).SingleOrDefault();
                ca.ID = ca.ID;
                ca.TelNumber = model.Telp;
                ca.Fax = model.Fax;
                ca.Address = model.Alamat;
                ca.Email = model.Email;
                ca.Website = model.Website;
                ca.CPName = model.CpNama;
                ca.MobileNumber = model.CpHp;
                ca.CompanyID = company.ID;
                ca.ModifiedDate = DateTime.Now;
                var resultCa = await companyAddressRepository.UpdateAsync(ca);

                DataKk dp = dataKkRepository.Get().Where(c => c.CompanyID == model.ID).SingleOrDefault();
                dp.ID = dp.ID;
                dp.Generasi = model.Generasi;
                dp.KodeWilayah = model.KodeWilayah;
                dp.NoKontrak = model.NoKontrak;
                dp.TanggalKontrak = model.TanggalKontrak;
                dp.LuasWilayahAwal = model.LuasWilayahAwal;
                dp.LuasWilayahDipertahankan = model.LuasWilayahDipertahankan;
                dp.Provinsi = model.Provinsi;
                dp.Kabupaten = model.Kabupaten;
                dp.Perizinan = model.Perizinan;
                dp.TahapanAkhir = model.TahapanAkhir;
                dp.TanggalBerakhir = model.TanggalBerakhir;
                dp.JangkaWaktu = model.JangkaWaktu;
                dp.CompanyID = company.ID;
                dp.ModifiedDate = DateTime.Now;
                dp.Komoditi = model.Komoditi;
                dp.Lainnya = model.Lainnya;
                var result = await dataKkRepository.UpdateAsync(dp);

                HistoryDataPkp2b hd = new HistoryDataPkp2b();
                hd.ID = Guid.NewGuid().ToString();
                hd.Generasi = model.Generasi;
                hd.KodeWilayah = model.KodeWilayah;
                hd.NoKontrak = model.NoKontrak;
                hd.TanggalKontrak = model.TanggalKontrak;
                hd.LuasWilayahAwal = model.LuasWilayahAwal;
                hd.LuasWilayahDipertahankan = model.LuasWilayahDipertahankan;
                hd.Provinsi = model.Provinsi;
                hd.Kabupaten = model.Kabupaten;
                hd.Perizinan = model.Perizinan;
                hd.TahapanAkhir = model.TahapanAkhir;
                hd.TanggalBerakhir = model.TanggalBerakhir;
                hd.JangkaWaktu = model.JangkaWaktu;
                hd.Komoditi = model.Komoditi;
                hd.Lainnya = model.Lainnya;
                hd.CompanyID = company.ID;
                hd.CreatedDate = DateTime.Now;
                var resultHd = await historyDataPkp2bRepository.AddAsync(hd);

                CompanyHistoryPkp2b ch = new CompanyHistoryPkp2b();
                ch.IdCompanyHistoryPkp2b = Guid.NewGuid().ToString();
                ch.Name = model.Name;
                ch.Address = model.Alamat;
                ch.NPWP = model.Npwp;
                ch.Telphone = model.Telp;
                ch.Fax = model.Fax;
                ch.Email = model.Email;
                ch.Website = model.Website;
                ch.CpName = model.CpNama;
                ch.CpMobileNo = model.CpHp;
                ch.CompanyID = model.ID;
                ch.CreatedDate = DateTime.Now;
                var resultCh = companyHistoryPkp2bRepository.AddAsync(ch);
                return resultCompany.ID;
            }
            return "0";
        }
        public async Task<JsonResult> FindDataPersetujuan(string id)
        {
            var data = await lingkunganRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                JenisPersetujuan = data.JenisPersetujuan,
                NoSk = data.NoSk,
                TanggalSk = data.TanggalSk.Value.ToString("dd/MM/yyyy"),
                File = data.File,
                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindDataAkta(string id)
        {
            var data = await aktaRepo.FindAsync(id);
            var result = new
            {
                ID = data.ID,


                JudulAkta = data.JudulAkta,
                NoAkta = data.NoAkta,
                TanggalAkta = data.TanggalAkta.Value.ToString("dd/MM/yyyy"),
                NoPengesahanKemenkumham = data.NoPengesahanKemenkumham,
                TanggalPengesahanKemenkumhan = data.TanggalPengesahanKemenkumhan.Value.ToString("dd/MM/yyyy"),
                File = data.File,

                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindTahapKegiatan(string id)
        {
            var data = await tahapKegiatanRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Tahap = data.Tahap,
                LuasDipertahankan = data.LuasDipertahankan,
                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindTahapKegiatanBlok(string id)
        {
            var data = await tahapKegiatanBlokRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Tahap = data.Tahap,
                LuasDipertahankan = data.LuasDipertahankan,
                TahapKegiatanId = data.TahapKegiatanId,
                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindSumberDayaBatubara(string id)
        {
            var data = await sumberDayaBatubaraRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Blok = data.Blok,
                Tereka = data.Tereka,
                Terunjuk = data.Terunjuk,
                Terukur = data.Terukur,
                CompanyID = data.CompanyID


            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindCadanganBatubara(string id)
        {
            var data = await cadanganBatubaraRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                CompanyID = data.CompanyID,
                Blok = data.Blok,
                Terkira = data.Terkira,
                Terbukti = data.Terbukti,
                Sr = data.Sr
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindKualitasBatubara(string id)
        {
            var data = await kualitasBatubaraRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                CompanyID = data.CompanyID,
                Parameter = data.Parameter,
                TM = data.TM,
                IM = data.IM,
                ASH = data.ASH,
                VM = data.VM,
                FC = data.FC,
                TS = data.TS,
                CVadb = data.CVadb,
                CVdaf = data.CVdaf,
                CVar = data.CVar
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> FindKadar(string id)
        {
            var data = await kadarRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                CompanyID = data.CompanyID,
                Komoditi = data.Komoditi,
                Blok = data.Blok,
                SampleCode = data.SampleCode,
                KadarKomoditi = data.KadarKomoditi
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindKronologiTahapan(string id)
        {
            var data = await kronologiTahapanRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Tahapan = data.Tahapan,
                NoSk = data.NoSk,
                TanggalSk = data.TanggalSk.Value.ToString("dd/MM/yyyy"),
                TanggalMulai = data.TanggalMulai.Value.ToString("dd/MM/yyyy"),
                TanggalAkhir = data.TanggalAkhir.Value.ToString("dd/MM/yyyy"),
                Keterangan = data.Keterangan,
                JangkaWaktu = data.JangkaWaktu,
                File = data.File,
                CompanyID = data.CompanyID


            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindDetailKronologiTahapan(string id)
        {
            var data = await detailKronologiTahapanRepository.FindAsync(id);
            var result = new
            {
                //    ID: '-',
                //CompanyID: $scope.idx,


                //Tahapan: $scope.detailKronologiTahapanModel.tahapan,
                //NoSk: $scope.detailKronologiTahapanModel.noSk,
                //TanggalSk: $('#tanggalSkKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.tanggalSk,
                //TanggalMulai: $('#mulaiKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.mulai,
                //TanggalAkhir: $('#sdKtDetail').data('datepicker').getFormattedDate('mm/dd/yyyy'),//$scope.kronologiTahapanModel.sampaiDengan,
                //Keterangan: $scope.detailKronologiTahapanModel.keterangan,
                //TahapanInduk: $scope.detailKronologiTahapanModel.tahapanInduk,
                //JangkaWaktu: $scope.detailKronologiTahapanModel.jangkaWaktu,
                //File: $scope.detailKronologiTahapanModel.file

                ID = data.ID,
                CompanyID = data.CompanyID,

                Tahapan = data.Tahapan,
                NoSk = data.NoSk,
                TanggalSk = data.TanggalSk.Value.ToString("dd/MM/yyyy"),
                TanggalMulai = data.TanggalMulai.Value.ToString("dd/MM/yyyy"),
                TanggalAkhir = data.TanggalAkhir.Value.ToString("dd/MM/yyyy"),
                Keterangan = data.Keterangan,
                TahapanInduk = data.TahapanInduk,
                JangkaWaktu = data.JangkaWaktu,
                File = data.File

            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<string> EditServiceDataAkta(Akta akta)
        {
            if (ModelState.IsValid)
            {
                akta.ModifiedBy = User.Identity.Name;
                akta.ModifiedDate = DateTime.Now;
                var result = await aktaRepo.UpdateAsync(akta);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceKadar(Kadar kadar)
        {
            if (ModelState.IsValid)
            {
                kadar.ModifiedBy = User.Identity.Name;
                kadar.ModifiedDate = DateTime.Now;
                var result = await kadarRepository.UpdateAsync(kadar);
                return result.ID;
            }
            return "0";
        }


        public async Task<string> EditServiceDataPersetujuan(LingkunganIppkhDanLainnya lingkunganIppkhDanLainnya)
        {
            if (ModelState.IsValid)
            {
                lingkunganIppkhDanLainnya.ModifiedBy = User.Identity.Name;
                lingkunganIppkhDanLainnya.ModifiedDate = DateTime.Now;
                var result = await lingkunganRepository.UpdateAsync(lingkunganIppkhDanLainnya);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceKronologiTahapan(KronologiTahapan kronologiTahapan)
        {
            if (ModelState.IsValid)
            {
                kronologiTahapan.ModifiedBy = User.Identity.Name;
                kronologiTahapan.ModifiedDate = DateTime.Now;
                var result = await kronologiTahapanRepository.UpdateAsync(kronologiTahapan);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceDetailKronologiTahapan(DetailKronologiTahapan detailKronologiTahapan)
        {
            if (ModelState.IsValid)
            {
                detailKronologiTahapan.ModifiedBy = User.Identity.Name;
                detailKronologiTahapan.ModifiedDate = DateTime.Now;
                var result = await detailKronologiTahapanRepository.UpdateAsync(detailKronologiTahapan);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceTahapKegiatan(TahapKegiatan tahapKegiatan)
        {
            if (ModelState.IsValid)
            {
                tahapKegiatan.ModifiedBy = User.Identity.Name;
                tahapKegiatan.ModifiedDate = DateTime.Now;
                var result = await tahapKegiatanRepository.UpdateAsync(tahapKegiatan);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceTahapKegiatanBlok(TahapKegiatanBlok tahapKegiatanBlok)
        {
            if (ModelState.IsValid)
            {
                tahapKegiatanBlok.ModifiedBy = User.Identity.Name;
                tahapKegiatanBlok.ModifiedDate = DateTime.Now;
                var result = await tahapKegiatanBlokRepository.UpdateAsync(tahapKegiatanBlok);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceSumberDayaBatubara(SumberDayaBatubara sumberBatubara)
        {
            if (ModelState.IsValid)
            {
                sumberBatubara.ModifiedBy = User.Identity.Name;
                sumberBatubara.ModifiedDate = DateTime.Now;
                var result = await sumberDayaBatubaraRepository.UpdateAsync(sumberBatubara);
                return result.ID;
            }
            return "0";
        }

        public async Task<ActionResult> Save(IEnumerable<HttpPostedFileBase> files, string id, string moduleType)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                var path = "";
                switch (moduleType)
                {
                    case "1":
                        path = @"~/DocumentKK/DataAkta/";
                        break;

                    case "2":
                        path = @"~/DocumentKK/KronologiTahapan/";
                        break;
                    case "3":
                        path = @"~/DocumentKK/DetailKronologiTahapan/";
                        break;
                    case "4":
                        path = @"~/DocumentKK/Lingkungan/";
                        break;
                }

                foreach (var file in files)
                {
                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(file.FileName);
                    var ext = Path.GetExtension(fileName);
                    var physicalPath = Path.Combine(Server.MapPath(path), id + ext);
                    // The files are not actually saved in this demo
                    try
                    {
                        file.SaveAs(physicalPath);
                        switch (moduleType)
                        {
                            case "1":
                                if (!String.IsNullOrEmpty(fileName))
                                {
                                    var akta = await aktaRepo.FindAsync(id);
                                    akta.File = id + ext;
                                    await aktaRepo.UpdateAsync(akta);
                                }
                                break;
                            case "2":
                                var kronologiTahapan = await kronologiTahapanRepository.FindAsync(id);
                                kronologiTahapan.File = id + ext;
                                await kronologiTahapanRepository.UpdateAsync(kronologiTahapan);
                                break;
                            case "3":
                                if (!String.IsNullOrEmpty(fileName))
                                {
                                    var dkt = await detailKronologiTahapanRepository.FindAsync(id);
                                    dkt.File = id + ext;
                                    await detailKronologiTahapanRepository.UpdateAsync(dkt);
                                }

                                break;
                            case "4":
                                var lingkungan = await lingkunganRepository.FindAsync(id);
                                lingkungan.File = id + ext;
                                await lingkunganRepository.UpdateAsync(lingkungan);
                                break;
                        }

                    }
                    catch (Exception ex) { }
                }
            }
            // Return an empty string to signify success
            return Content("OK");
        }

        public async Task<string> EditServiceCadanganBatubara(CadanganBatubara cadanganBatubara)
        {
            if (ModelState.IsValid)
            {
                cadanganBatubara.ModifiedBy = User.Identity.Name;
                cadanganBatubara.ModifiedDate = DateTime.Now;
                var result = await cadanganBatubaraRepository.UpdateAsync(cadanganBatubara);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceKualitasBatubara(KualitasBatubara kualitasBatubara)
        {
            if (ModelState.IsValid)
            {
                kualitasBatubara.ModifiedBy = User.Identity.Name;
                kualitasBatubara.ModifiedDate = DateTime.Now;
                var result = await kualitasBatubaraRepository.UpdateAsync(kualitasBatubara);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateServiceShareHolder(ShareHolder shareHolder)
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

        public async Task<string> SaveBiayaEksplorasi(BiayaEksplorasi biayaEksplorasi)
        {
            if (ModelState.IsValid)
            {
                biayaEksplorasi.ID = Guid.NewGuid().ToString();
                biayaEksplorasi.CreatedBy = User.Identity.Name;
                biayaEksplorasi.CreatedDate = DateTime.Now;
                await biayaEksplorasiRepo.AddAsync(biayaEksplorasi);

                //var check = biayaEksplorasiRepo.Get().Where(c => c.CompanyID == biayaEksplorasi.CompanyID);
                //if (String.IsNullOrEmpty(check.ToString()))
                //{
                //    biayaEksplorasi.ID = Guid.NewGuid().ToString();
                //    biayaEksplorasi.CreatedBy = User.Identity.Name;
                //    biayaEksplorasi.CreatedDate = DateTime.Now;
                //    await biayaEksplorasiRepo.AddAsync(biayaEksplorasi);
                //}
                //else
                //{
                //    biayaEksplorasi.ModifiedBy = User.Identity.Name;
                //    biayaEksplorasi.ModifiedDate = DateTime.Now;
                //    await biayaEksplorasiRepo.UpdateAsync(biayaEksplorasi);
                //}
                return biayaEksplorasi.ID;
            }
            return "OK";
        }
        public async Task<string> EditBiayaEksplorasi(BiayaEksplorasi biayaEksplorasi)
        {
            if (ModelState.IsValid)
            {
                biayaEksplorasi.ModifiedBy = User.Identity.Name;
                biayaEksplorasi.ModifiedDate = DateTime.Now;
                var result = await biayaEksplorasiRepo.UpdateAsync(biayaEksplorasi);
                return result.ID;
            }
            return "0";
        }

        public async Task<JsonResult> LoadDetails(string id)
        {
            var shareHolders = shareHolderRepository.FindByCompany(id);
            var datas = dataPkp2bRepository.Get().Where(c => c.CompanyID == id).SingleOrDefault();
            var datapkp2b = new
            {
                ID = datas.ID,
                Generasi = datas.Generasi,
                KodeWilayah = datas.KodeWilayah,
                NoKontrak = datas.NoKontrak,
                TanggalKontrak = datas.TanggalKontrak.Value.ToString("dd/MM/yyyy"),
                LuasWilayahAwal = datas.LuasWilayahAwal,
                LuasWilayahDipertahankan = datas.LuasWilayahDipertahankan,
                Provinsi = datas.Provinsi,
                Perizinan = datas.Perizinan,
                Kabupaten = datas.Kabupaten,
                TahapanAkhir = datas.TahapanAkhir,
                JangkaWaktu = datas.JangkaWaktu,
                TanggalBerakhir = datas.TanggalBerakhir.Value.ToString("dd/MM/yyyy")
            };

            var company = from a in companyRepository.GetAll().AsEnumerable()
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
                              CpHp = b.MobileNumber
                          };
            return Json(
                new
                {
                    datapkp2b = datapkp2b,
                    company = company,
                    shareHolders = shareHolders,
                }, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> LoadDetailsKk(string id)
        {
            var shareHolders = shareHolderRepository.FindByCompany(id);
            var datas = dataKkRepository.Get().Where(c => c.CompanyID == id).SingleOrDefault();
            var datapkp2b = new
            {
                ID = datas.ID,
                Generasi = datas.Generasi,
                KodeWilayah = datas.KodeWilayah,
                NoKontrak = datas.NoKontrak,
                TanggalKontrak = datas.TanggalKontrak.Value.ToString("dd/MM/yyyy"),
                LuasWilayahAwal = datas.LuasWilayahAwal,
                LuasWilayahDipertahankan = datas.LuasWilayahDipertahankan,
                Provinsi = datas.Provinsi,
                Perizinan = datas.Perizinan,
                Kabupaten = datas.Kabupaten,
                TahapanAkhir = datas.TahapanAkhir,
                JangkaWaktu = datas.JangkaWaktu,
                Komoditi = datas.Komoditi,
                Lainnya = datas.Lainnya,
                TanggalBerakhir = datas.TanggalBerakhir.Value.ToString("dd/MM/yyyy")
            };

            var company = from a in companyRepository.GetAll().AsEnumerable()
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
                              CpHp = b.MobileNumber
                          };
            return Json(
                new
                {
                    datapkp2b = datapkp2b,
                    company = company,
                    shareHolders = shareHolders,
                }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<string> CreateServiceGeneralInformation(GeneralInformationViewModel model)
        {
            //tabel pkp2b berlaku juga buat kk..... disamain aja yak tabel nyaaaa..... khaaaakkkk
            if (ModelState.IsValid)
            {
                Company company = new Company();
                company.ID = Guid.NewGuid().ToString();
                company.Name = model.Name;
                company.NPWP = model.Npwp;
                company.CreatedDate = DateTime.Now;
                var resultCompany = await companyRepository.AddAsync(company);

                CompanyAddress ca = new CompanyAddress();
                ca.ID = Guid.NewGuid().ToString();
                ca.TelNumber = model.Telp;
                ca.Fax = model.Fax;
                ca.Address = model.Alamat;
                ca.Email = model.Email;
                ca.Website = model.Website;
                ca.CPName = model.CpNama;
                ca.MobileNumber = model.CpHp;
                ca.CompanyID = company.ID;
                var resultCa = await companyAddressRepository.AddAsync(ca);

                DataKk dp = new DataKk();
                dp.ID = Guid.NewGuid().ToString();
                dp.Generasi = model.Generasi;
                dp.KodeWilayah = model.KodeWilayah;
                dp.NoKontrak = model.NoKontrak;
                dp.TanggalKontrak = model.TanggalKontrak;
                dp.LuasWilayahAwal = model.LuasWilayahAwal;
                dp.LuasWilayahDipertahankan = model.LuasWilayahDipertahankan;
                dp.Provinsi = model.Provinsi;
                dp.Kabupaten = model.Kabupaten;
                dp.TahapanAkhir = model.TahapanAkhir;
                dp.TanggalBerakhir = model.TanggalBerakhir;
                dp.Perizinan = model.Perizinan;
                dp.JangkaWaktu = model.JangkaWaktu;
                dp.Komoditi = model.Komoditi;
                dp.Lainnya = model.Lainnya;
                dp.CreatedBy = User.Identity.Name.ToString();
                dp.CreatedDate = DateTime.Now;
                dp.CompanyID = company.ID;
                var result = await dataKkRepository.AddAsync(dp);
                
                CompanyMiningModulDetail cmd = new CompanyMiningModulDetail()
                {
                    ID = Guid.NewGuid().ToString(),
                    CompanyID = company.ID,
                    MiningModuleID = "5"
                };
                await companyMiningModulDetailRepository.AddAsync(cmd);

                HistoryDataPkp2b hd = new HistoryDataPkp2b();
                hd.ID = Guid.NewGuid().ToString();
                hd.Generasi = model.Generasi;
                hd.KodeWilayah = model.KodeWilayah;
                hd.NoKontrak = model.NoKontrak;
                hd.TanggalKontrak = model.TanggalKontrak;
                hd.LuasWilayahAwal = model.LuasWilayahAwal;
                hd.LuasWilayahDipertahankan = model.LuasWilayahDipertahankan;
                hd.Provinsi = model.Provinsi;
                hd.Kabupaten = model.Kabupaten;
                hd.Perizinan = model.Perizinan;
                hd.TahapanAkhir = model.TahapanAkhir;
                hd.TanggalBerakhir = model.TanggalBerakhir;
                hd.JangkaWaktu = model.JangkaWaktu;
                hd.Komoditi = model.Komoditi;
                hd.Lainnya = model.Lainnya;
                hd.CompanyID = company.ID;
                hd.CreatedBy = User.Identity.Name.ToString();
                hd.CreatedDate = DateTime.Now;
                var resultHd = await historyDataPkp2bRepository.AddAsync(hd);

                CompanyHistoryPkp2b ch = new CompanyHistoryPkp2b();
                ch.IdCompanyHistoryPkp2b = Guid.NewGuid().ToString();
                ch.Name = model.Name;
                ch.Address = model.Alamat;
                ch.NPWP = model.Npwp;
                ch.Telphone = model.Telp;
                ch.Fax = model.Fax;
                ch.Email = model.Email;
                ch.Website = model.Website;
                ch.CpName = model.CpNama;
                ch.CpMobileNo = model.CpHp;
                ch.CompanyID = model.ID;
                ch.CreatedBy = User.Identity.Name.ToString();
                ch.CreatedDate = DateTime.Now;
                var resultCh = companyHistoryPkp2bRepository.AddAsync(ch);

                return resultCompany.ID;
            }
            return "0";
        }
        [HttpPost]
        public async Task<string> CreateDataAkta(Akta akta)
        {
            if (ModelState.IsValid)
            {
                akta.ID = Guid.NewGuid().ToString();
                akta.CreatedDate = DateTime.Now;
                var result = await aktaRepo.AddAsync(akta);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateKronologiTahapan(KronologiTahapan kronologiTahapan)
        {
            if (ModelState.IsValid)
            {
                kronologiTahapan.ID = Guid.NewGuid().ToString();
                kronologiTahapan.CreatedDate = DateTime.Now;
                var result = await kronologiTahapanRepository.AddAsync(kronologiTahapan);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateDetailKronologiTahapan(DetailKronologiTahapan detailKronologiTahapan)
        {
            if (ModelState.IsValid)
            {
                detailKronologiTahapan.ID = Guid.NewGuid().ToString();
                detailKronologiTahapan.CreatedDate = DateTime.Now;
                var result = await detailKronologiTahapanRepository.AddAsync(detailKronologiTahapan);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateKualitasBatubara(KualitasBatubara kualitasBatubara)
        {
            if (ModelState.IsValid)
            {
                kualitasBatubara.ID = Guid.NewGuid().ToString();
                kualitasBatubara.CreatedDate = DateTime.Now;
                var result = await kualitasBatubaraRepository.AddAsync(kualitasBatubara);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateKadar(Kadar kadar)
        {
            if (ModelState.IsValid)
            {
                kadar.ID = Guid.NewGuid().ToString();
                kadar.CreatedBy = User.Identity.Name.ToString();
                kadar.CreatedDate = DateTime.Now;
                var result = await kadarRepository.AddAsync(kadar);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateTahapKegiatan(TahapKegiatan tahapKegiatan)
        {
            if (ModelState.IsValid)
            {
                tahapKegiatan.ID = Guid.NewGuid().ToString();
                tahapKegiatan.CreatedDate = DateTime.Now;
                var result = await tahapKegiatanRepository.AddAsync(tahapKegiatan);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateTahapKegiatanBlok(TahapKegiatanBlok tahapKegiatanBlok)
        {
            if (ModelState.IsValid)
            {
                tahapKegiatanBlok.ID = Guid.NewGuid().ToString();
                tahapKegiatanBlok.CreatedDate = DateTime.Now;
                var result = await tahapKegiatanBlokRepository.AddAsync(tahapKegiatanBlok);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateSumberDayaBatubara(SumberDayaBatubara sumberDayaBatubara)
        {
            if (ModelState.IsValid)
            {
                sumberDayaBatubara.ID = Guid.NewGuid().ToString();
                sumberDayaBatubara.CreatedDate = DateTime.Now;
                var result = await sumberDayaBatubaraRepository.AddAsync(sumberDayaBatubara);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreateCadanganBatubara(CadanganBatubara cadanganBatubara)
        {
            if (ModelState.IsValid)
            {
                cadanganBatubara.ID = Guid.NewGuid().ToString();
                cadanganBatubara.CreatedDate = DateTime.Now;
                var result = await cadanganBatubaraRepository.AddAsync(cadanganBatubara);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> CreatePersetujuan(LingkunganIppkhDanLainnya persetujuan)
        {
            if (ModelState.IsValid)
            {
                persetujuan.ID = Guid.NewGuid().ToString();
                persetujuan.CreatedDate = DateTime.Now;
                var result = await lingkunganRepository.AddAsync(persetujuan);
                return result.ID;
            }
            return "0";
        }
        public JsonResult LoadPersetujuan(string id)
        {
            var result = lingkunganRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadKronologiTahapan(string id)
        {
            var result = kronologiTahapanRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDetailKronologiTahapan(string id)
        {
            var result = detailKronologiTahapanRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadCadanganBatubara(string id)
        {
            var result = cadanganBatubaraRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadKualitasBatubara(string id)
        {
            var result = kualitasBatubaraRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadKadar(string id)
        {
            var result = kadarRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadTahapKegiatan(string id)
        {
            var result = tahapKegiatanRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadTahapKegiatanBlok(string id)
        {
            var result = tahapKegiatanBlokRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSumberDayaBatubara(string id)
        {
            var result = sumberDayaBatubaraRepository.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> DeletePersetujuan(string id)
        {
            var data = await lingkunganRepository.FindAsync(id);
            await lingkunganRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteCadanganBatubara(string id)
        {
            var data = await cadanganBatubaraRepository.FindAsync(id);
            await cadanganBatubaraRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteKualitasBatubara(string id)
        {
            var data = await kualitasBatubaraRepository.FindAsync(id);
            await kualitasBatubaraRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteKadar(string id)
        {
            var data = await kadarRepository.FindAsync(id);
            await kadarRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteTahapKegiatan(string id)
        {
            var data = await tahapKegiatanRepository.FindAsync(id);
            await tahapKegiatanRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteTahapKegiatanBlok(string id)
        {
            var data = await tahapKegiatanBlokRepository.FindAsync(id);
            await tahapKegiatanBlokRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteSumberDayaBatubara(string id)
        {
            var data = await sumberDayaBatubaraRepository.FindAsync(id);
            await sumberDayaBatubaraRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteKronologiTahapan(string id)
        {
            var data = await kronologiTahapanRepository.FindAsync(id);
            await kronologiTahapanRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteDetailKronologiTahapan(string id)
        {
            var data = await detailKronologiTahapanRepository.FindAsync(id);
            await detailKronologiTahapanRepository.RemoveAsync(data);
            return "OK";
        }

        public JsonResult LoadAkta(string id)
        {
            var result = aktaRepo.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadBiayaEksplorasi(string id)
        {
            var result = biayaEksplorasiRepo.Get().Where(c => c.CompanyID == id);
            return Json(
                new
                {
                    data = result
                }, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> DeleteDataAkta(string id)
        {
            var data = await aktaRepo.FindAsync(id);
            await aktaRepo.RemoveAsync(data);
            return "OK";
        }
    }
}