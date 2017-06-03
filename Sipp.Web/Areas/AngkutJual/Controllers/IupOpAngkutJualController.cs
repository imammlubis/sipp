using AutoMapper;
using EduSpot.Entity.Tables.AngkutJual;
using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.Organization;
using Esdm.Web.Areas.AngkutJual.Models;
using Esdm.Web.Utils;
using Ews.Repository.Abstraction.MiningService;
using Ews.Repository.Concrete.MiningService;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    [Authorize(Roles = "AngkutJualAdmin")]
    public class IupOpAngkutJualController : Controller
    {
        private IWarningLetterRepository warningLetterRepository = new WarningLetterRepository();
        private EmailServices emailService = new EmailServices();
        private Esdm.Web.Utils.SmsService smsService = new Esdm.Web.Utils.SmsService();
        private IBannedSkRepository bannedSkRepository = new BannedSkRepository();
        private IAdjustedSkRepository adjustedSkRepository = new AdjustedSkRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();
        private ICompanyAddressRepository companyAddressRepository = new CompanyAddressRepository();
        private IShareHolderRepository shareHolderRepository = new ShareHolderRepository();

        private ICommisionerRepository commissionerRepository = new CommissionerRepository();
        private IManagementRepository managementRepository = new ManagementRepository();
        private IRKABRepository rkabRepository = new RKABRepository();
        private IReportRepository reportRepository = new ReportRepository();

        private IQuarterlyRepository quarterlyRepository = new QuarterlyRepository();
        private IYearlyRepository yearlyRepository = new YearlyRepository();

        private IFirstSkRepository firstSkRepository = new FirstSkRepository();
        private ICoalSourceSkRepository coalSourceSkRepository = new CoalSourceSkRepository();
        private IExtendedSkRepository extendedSkRepository = new ExtendedSkRepository();
        private ITempBannedSkRepository tempBannedSkRepository = new TempBannedSkRepository();
        private IReactivatedSkRepository reactivatedSkRepository = new ReactivatedSkRepository();
        private IBKPMRepository bKPMRepository = new BKPMRepository();
        private IETRecommendationRepository eTRecommendationRepository = new ETRecommendationRepository();
        private IAdditionalCooperationRepository additionalCooperationRepository = new AdditionalCooperationRepository();
        private ICNCCertificateRepository cNCCertificateRepository = new CNCCertificateRepository();
        private ICompanyFileRepository companyFileRepository = new CompanyFileRepository();
        private IFirstSkSourceRepository firstSkSourceRepo = new FirstSkSourceRepository();
        private ISourceChangesRepository sourceChangesRepository = new SourceChangesRepository();
        private ISourceChangesSkSourceRepository sourceChangesSkSourceRepository = new SourceChangesSkSourceRepository();
        private IExtendedSkSourceRepository extendedSkSourceRepository = new ExtendedSkSourceRepository();
        private INotificationLogAngkutJualRepository notifLogRepository = new NotificationLogAngkutJualRepository();

        private ICompanyMiningModulDetailRepository companyMiningModulDetailRepository = new CompanyMiningModulDetailRepository();

        // GET: AngkutJual/IupOpAngkutJual
        public async Task<ActionResult> Index()
        {
            return View();
        }

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
                      where a.CompanyID==id
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

            //var scs = (from a in sourceChangesSkSourceRepository.GetAll().AsEnumerable()
            //           join b in sourceChangesRepository.GetAll().AsEnumerable()
            //           on a.SourceChangesID equals b.ID
            //           where b.CompanyID == id
            //           select new SourceChangesSkSourceViewModel
            //           {
            //               CompanyName = a.CompanyName,
            //               Volume = a.Volume,
            //               Sumber = a
            //           }).ToList();

            //ViewData["SourceChangesSource"] = scs;

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

        [HttpPost]
        public async Task<string> CreateServiceFirstSkSource(FirstSkSource firstSkSource)
        {
            if (ModelState.IsValid)
            {
                firstSkSource.ID = Guid.NewGuid().ToString();
                var result = await firstSkSourceRepo.AddAsync(firstSkSource);
                return result.ID;
            }
            return "-1";
        }
        //CreateServiceSimpanSkPerpanjangan2
        [HttpPost]
        public async Task<string> CreateServiceSimpanSkPerpanjangan2(ExtendedSk extendedSk)
        {
            if (ModelState.IsValid)
            {
                extendedSk.ID = Guid.NewGuid().ToString();
                extendedSk.CreatedDate = DateTime.Now;
                var result = await extendedSkRepository.AddAsync(extendedSk);
                return result.ID;
            }
            return "-1";
        }

        [HttpPost]
        public async Task<string> CreateServiceSimpanSkPengakhiran(BannedSk bannedSk)
        {
            if (ModelState.IsValid)
            {
                bannedSk.ID = Guid.NewGuid().ToString();
                bannedSk.CreatedDate = DateTime.Now;
                var result = await bannedSkRepository.AddAsync(bannedSk);
                return result.ID;
            }
            return "-1";
        }

        [HttpPost]
        public async Task<string> CreateServiceSumberPerubahanKerjasama(SourceChangesSkSource sourceChangesSkSource)
        {
            if (ModelState.IsValid)
            {
                sourceChangesSkSource.ID = Guid.NewGuid().ToString();
                var result = await sourceChangesSkSourceRepository.AddAsync(sourceChangesSkSource);
                return result.ID;
            }
            return "-1";
        }
        [HttpPost]
        public async Task<string> CreateServiceSumberSkPerpanjangan(ExtendedSkSource extendedSkSource)
        {
            if (ModelState.IsValid)
            {
                extendedSkSource.ID = Guid.NewGuid().ToString();
                var result = await extendedSkSourceRepository.AddAsync(extendedSkSource);
                return result.ID;
            }
            return "-1";
        }
        public async Task<string> CreateServicePerubahanKerjasama(SourceChanges sourceChanges)
        {
            if (ModelState.IsValid)
            {
                sourceChanges.ID = Guid.NewGuid().ToString();
                sourceChanges.CreatedBy = User.Identity.Name;
                //peopleInvolved.CreateBy = User.Identity.Name;
                sourceChanges.CreatedDate = DateTime.Now;
                var result = await sourceChangesRepository.AddAsync(sourceChanges);
                return result.ID;
            }
            return "-1";
        }
        //EditServicePerubahanKerjasama
        public async Task<string> EditServicePerubahanKerjasama(SourceChanges sourceChanges)
        {
            if (ModelState.IsValid)
            {
                sourceChanges.CreatedBy = User.Identity.Name;
                sourceChanges.ModifiedDate = DateTime.Now;
                var result = await sourceChangesRepository.UpdateAsync(sourceChanges);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceSkPerpanjangan2(ExtendedSk extendedSk)
        {
            if (ModelState.IsValid)
            {
                extendedSk.ModifiedBy = User.Identity.Name;
                extendedSk.ModifiedDate = DateTime.Now;
                var result = await extendedSkRepository.UpdateAsync(extendedSk);
                return result.ID;
            }
            return "0";
        }
        public async Task<string> EditServiceSkPengakhiran(BannedSk bannedSk)
        {
            if (ModelState.IsValid)
            {
                //peopleInvolved.CreateBy = User.Identity.Name;
                bannedSk.ModifiedDate = DateTime.Now;
                var result = await bannedSkRepository.UpdateAsync(bannedSk);
                return result.ID;
            }
            return "0";
        }

        public async Task<string> EditServiceSumberPerubahanKerjasama(SourceChangesSkSource sourceChanges)
        {
            if (ModelState.IsValid)
            {
                var result = await sourceChangesSkSourceRepository.UpdateAsync(sourceChanges);
                return result.ID;
            }
            return "0";
        }
        public async Task<string> EditServiceSumberSkPerpanjangan(ExtendedSkSource extendedSkSourceChanges)
        {
            if (ModelState.IsValid)
            {
                var result = await extendedSkSourceRepository.UpdateAsync(extendedSkSourceChanges);
                return result.ID;
            }
            return "0";
        }

        //DeletePerubahanKerjasama
        public async Task<string> DeletePerubahanKerjasama(string id)
        {
            var data = await sourceChangesRepository.FindAsync(id);
            await sourceChangesRepository.RemoveAsync(data);
            return "OK";
        }
        public async Task<string> DeleteSkPerpanjangan2(string id)
        {
            var data = await extendedSkRepository.FindAsync(id);
            await extendedSkRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteSkPengakhiran(string id)
        {
            var data = await bannedSkRepository.FindAsync(id);
            await bannedSkRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteSumberPerubahanKerjasama(string id)
        {
            var data = await sourceChangesSkSourceRepository.FindAsync(id);
            await sourceChangesSkSourceRepository.RemoveAsync(data);
            return "OK";
        }

        public async Task<string> DeleteSumberSkPerpanjangan(string id)
        {
            var data = await extendedSkSourceRepository.FindAsync(id);
            await extendedSkSourceRepository.RemoveAsync(data);
            return "OK";
        }

        //FindPerubahanKerjasama
        public async Task<JsonResult> FindPerubahanKerjasama(string id)
        {
            var data = await sourceChangesRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                LetterNumber = data.LetterNumber,
                LetterDate = data.LetterDate.Value.ToString("dd/MM/yyyy"),
                SkNumber = data.SkNumber,
                SkDate = data.SkDate.Value.ToString("dd/MM/yyyy"),
                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> FindSkPerpanjangan2(string id)
        {
            var data = await extendedSkRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                LetterNumber = data.LetterNumber,
                LetterDate = data.LetterDate.Value.ToString("dd/MM/yyyy"),
                SkNumber = data.SkNumber,
                SkDate = data.SkDate.Value.ToString("dd/MM/yyyy"),
                SkDuration = data.SkDuration,
                SkEndDate = data.SkEndDate.Value.ToString("dd/MM/yyyy"),
                SertifikatCNC = data.SertifikatCNC,
                SkFile = data.SkFile,
                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindSkPengakhiran(string id)
        {
            var data = await bannedSkRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                LetterNumber = data.LetterNumber,
                LetterDate = data.LetterDate.Value.ToString("dd/MM/yyyy"),
                SkNumber = data.SkNumber,
                SkDate = data.SkDate.Value.ToString("dd/MM/yyyy"),
                SkFile = data.SkFile,
                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //FindSumberPerubahanKerjasama
        public async Task<JsonResult> FindSumberPerubahanKerjasama(string id)
        {
            var data = await sourceChangesSkSourceRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Volume = data.Volume,
                CompanyName = data.CompanyName,
                SourceChangesID = data.SourceChangesID,
                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> FindSumberSkPerpanjangan(string id)
        {
            var data = await extendedSkSourceRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Volume = data.Volume,
                CompanyName = data.CompanyName,
                ExtendedSkID = data.ExtendedSkID,
                CompanyID = data.CompanyID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSumberPerubahanKerjasama(string id)
        {
            var result = from y in sourceChangesSkSourceRepository.GetAll().AsEnumerable()
                         where y.CompanyID == id
                         select new SourceChangesSkSourceViewModel
                         {
                             ID = y.ID,
                             CompanyName = y.CompanyName,
                             Volume = y.Volume,
                             SourceChangesID = y.SourceChangesID,
                             CompanyID = y.CompanyID
                         };
            return Json(
                new
                {
                    data = result

                }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSumberSkPerpanjangan(string id)
        {
            var result = from y in extendedSkSourceRepository.GetAll().AsEnumerable()
                         where y.CompanyID == id
                         select new ExtendedSkSourceViewModel
                         {
                             ID = y.ID,
                             CompanyName = y.CompanyName,
                             Volume = y.Volume,
                             ExtendedSkID = y.ExtendedSkID,
                             CompanyID = y.CompanyID
                         };
            return Json(
                new
                {
                    data = result

                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSkPengakhiran(string id)
        {
            var result = from y in bannedSkRepository.GetAll().AsEnumerable()
                         where y.CompanyID == id
                         select new SkPengakhiranViewModel
                         {
                             ID = y.ID,
                             LetterNumber = y.LetterNumber,
                             LetterDate = y.LetterDate.Value.ToString("dd/MM/yyyy") == null ? "" : y.LetterDate.Value.ToString("dd/MM/yyyy"),
                             SkNumber = y.SkNumber,
                             SkDate = y.SkDate.Value.ToString("dd/MM/yyyy") == null ? "" : y.SkDate.Value.ToString("dd/MM/yyyy"),                             
                             SkFile = y.SkFile,
                             CompanyID = y.CompanyID
                         };
            return Json(
                new
                {
                    data = result

                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSkPerpanjangan2(string id)
        {
            var result = from y in extendedSkRepository.GetAll().AsEnumerable()
                         where y.CompanyID == id
                         select new SkPerpanjanganViewModel
                         {
                             ID = y.ID,
                             LetterNumber = y.LetterNumber,
                             LetterDate = y.LetterDate.Value.ToString("dd/MM/yyyy") == null? "": y.LetterDate.Value.ToString("dd/MM/yyyy"),
                             SkNumber = y.SkNumber,
                             SkDate = y.SkDate.Value.ToString("dd/MM/yyyy") == null ? "" : y.SkDate.Value.ToString("dd/MM/yyyy"),
                             SkDuration = y.SkDuration,
                             SkEndDate = y.SkEndDate.Value.ToString("dd/MM/yyyy") == null ? "" : y.SkEndDate.Value.ToString("dd/MM/yyyy"),
                             SertifikatCNC = y.SertifikatCNC,
                             SkFile = y.SkFile,
                             CompanyID = y.CompanyID
                         };
            return Json(
                new
                {
                    data = result

                }, JsonRequestBehavior.AllowGet);
        }

        //LoadPerubahanKerjasama
        public JsonResult LoadPerubahanKerjasama(string id)
        {
            //var data = sourceChangesRepository.GetAll()
            //    .Where(s => s.CompanyID == id)
            //    .OrderByDescending(s => s.CreatedDate);

            //var result = from x in data
            //             select new
            //             {
            //                 ID = x.ID,
            //                 LetterNumber = x.LetterNumber,
            //                 LetterDate = DateTime.Parse(x.LetterDate.ToString()).ToString(),
            //                 SkNumber = x.SkNumber,
            //                 SkDate = x.SkDate.Value.ToString()
            //             };
            var result = from y in sourceChangesRepository.GetAll().AsEnumerable()
                         where y.CompanyID == id
                         orderby y.CreatedDate descending
                         select new SourceChangesViewModel
                         {
                             ID = y.ID,
                             LetterNumber = y.LetterNumber,
                             LetterDate = y.LetterDate.Value.ToString("dd/MM/yyyy") == null ? "": y.LetterDate.Value.ToString("dd/MM/yyyy"),
                             SkNumber = y.SkNumber,
                             SkDate = y.SkDate.Value.ToString("dd/MM/yyyy") == null ? "": y.SkDate.Value.ToString("dd/MM/yyyy")
                         };
            return Json(
                new
                {
                    data = result

                }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AlertData(string id)
        {
            var company = await companyRepository.FindAsync(id);
            var address = companyAddressRepository.GetAll().Where(s => s.CompanyID == company.ID).FirstOrDefault();
            var companyAddress = new List<CompanyAddress>();
            companyAddress.Add(address);
            company.CompanyAddresses = companyAddress;
            return View(company);
        }

        public async Task<ActionResult> AlertPeringatan(string id)
        {
            var company = await companyRepository.FindAsync(id);
            var address = companyAddressRepository.GetAll().Where(s => s.CompanyID == company.ID).FirstOrDefault();
            var companyAddress = new List<CompanyAddress>();
            companyAddress.Add(address);
            company.CompanyAddresses = companyAddress;
            return View(company);
        }

        public JsonResult findWarning(string id)
        {
            var data = companyAddressRepository.GetAll().Where(c => c.CompanyID == id).SingleOrDefault();
            var result = new
            {
                email = data.Email,
                mobile = data.MobileNumber,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> SendSkHabis(WarningLetterViewModel model)
        {
            List<string> dests = new List<string>();
            List<string> ccs = new List<string>();
            //var companyAddress = companyAddressRepository.GetAll().Where(c => c.CompanyID == model.CompanyID).SingleOrDefault();
            var message = model.Keterangan;
            var subject = "SK dari " + model.CompanyName + " akan berakhir";
            var body = message;
            if (!String.IsNullOrEmpty(model.DestinationMobileNo))
            {
                smsService.SendSms(new SmsContent()
                {
                    To = model.DestinationMobileNo,
                    Body = body
                });
            }
            if (!String.IsNullOrEmpty(model.DestinationEmail))
            {
                dests.Add(model.DestinationEmail);
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
            var notifModel = new NotificationLogAngkutJual
            {
                IdNotificationLog = Guid.NewGuid().ToString(),
                NotificationLogDate = DateTime.Now,
                Email = model.DestinationEmail,
                MobileNo = model.DestinationMobileNo,
                TglSuratPeringatan = null,
                TglAkhirPeringatan = null,
                CompanyId = model.CompanyID,
                NotificationsContent = model.Keterangan,
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name

            };

            var notifLog = await notifLogRepository.AddAsync(notifModel);
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

        public async Task<string> SaveWarningLetter(WarningLetterViewModel model)
        {
            //save to db
            var data = new WarningLetter
            {
                ID = Guid.NewGuid().ToString(),
                LetterNumber = model.LetterNumber,
                LetterDate = model.LetterDate,
                SkEndDate = model.SkEndDate,
                WarningDuration = model.WarningDuration,
                ObligationsAlready = model.ObligationsAlready,
                ObligationsShould = model.ObligationsShould,
                ObligationsYet = model.ObligationsYet,
                AdditionalInfo = model.AdditionalInfo,
                WarningType = model.WarningType,
                CompanyID = model.CompanyID,
                DestinationEmail = model.DestinationEmail,
                DestinationMobileNo = model.DestinationMobileNo
            };
            var notifModel = new NotificationLogAngkutJual
            {
                IdNotificationLog = Guid.NewGuid().ToString(),
                NotificationLogDate = DateTime.Now,
                Email = model.DestinationEmail,
                MobileNo = model.DestinationMobileNo,
                TglSuratPeringatan = model.LetterDate,
                TglAkhirPeringatan = model.SkEndDate,
                CompanyId = model.CompanyID,
                NotificationsContent = model.AdditionalInfo,
                CreatedDate = DateTime.Now

            };

            var notifLog = await notifLogRepository.AddAsync(notifModel);

            var result = await warningLetterRepository.AddAsync(data);
            var company = await companyRepository.FindAsync(model.CompanyID);
            //send sms
            var smsresult = smsService.SendSms(new SmsContent()
            {
                To = data.DestinationMobileNo,
                Body = data.AdditionalInfo
            });
            //send email
            var emailbody = System.IO.File.ReadAllText(Server.MapPath("~/Utils/htmls/templateemailwarningletter.html"));
            emailbody = emailbody.Replace("[NamaPerusahaan]", company.Name)
                .Replace("[JenisPeringatan]", model.WarningType)
                .Replace("[JangkaWaktu]", model.WarningDuration + " Hari.")
                .Replace("[NomorSurat]", model.LetterNumber)
                .Replace("[TanggalSurat]", model.LetterDate.Value.ToString("dd/MM/yyyy"))
                .Replace("[SkEndDate]", model.SkEndDate.Value.ToString("dd/MM/yyyy"))
                .Replace("[Email]", model.DestinationEmail)
                .Replace("[NoHandphone]", String.IsNullOrEmpty(model.DestinationMobileNo) ? "" : model.DestinationMobileNo)
                .Replace("[KewajibanHarus]", String.IsNullOrEmpty(model.ObligationsShould) ? "" : model.ObligationsShould)
                .Replace("[KewajibanBelum]", String.IsNullOrEmpty(model.ObligationsYet) ? "" : model.ObligationsYet)
                .Replace("[KewajibanSudah]", String.IsNullOrEmpty(model.ObligationsAlready) ? "" : model.ObligationsAlready)
                .Replace("[Keterangan]", String.IsNullOrEmpty(model.AdditionalInfo) ? "" : model.AdditionalInfo);
            List<string> dests = new List<string>();

            List<string> ccs = new List<string>();
            var subject = model.WarningType + " untuk " + company.Name;
            dests.Add(model.DestinationEmail);
            //ccs.Add("mahendra@eduspot.co.id");
            var emailresult = emailService.SendAsyncDefault(new EmailContent
            {
                Subject = subject,
                Destination = dests,
                CC = ccs,
                Body = emailbody
            });



            return result.ID;
        }

        public JsonResult SendSms()
        {
            //laksdlaskdlaskd


            //send sms
            var result = smsService.SendSms(new SmsContent()
            {
                To = "+6287880445131",
                Body = "sms testing notif"
            });

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult SendEmail()
        {
            List<string> dests = new List<string>();

            List<string> ccs = new List<string>();
            ccs.Add("sdsujminerba@gmail.com");
            ccs.Add("ricky_rinaldy@rocketmail.com");
            ccs.Add("anindiaprimasari@gmail.com");


            var subject = "Alert Email";


            var body = "test body";



            dests.Add("mahendra@eduspot.co.id");

            var result = emailService.SendAsyncDefault(new EmailContent
            {
                Subject = subject,
                Destination = dests,
                CC = ccs,
                Body = body
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //SK PERPANJANGAN
        public JsonResult LoadSkPerpanjangan(string id)
        {
            var skPerpanjangan = extendedSkRepository.GetAll().Where(s => s.CompanyID == id)
                .OrderByDescending(s => s.CreatedDate);
            var dataSkPerpanjangan = from x in skPerpanjangan
                                     select new
                                     {
                                         ID = x.ID,
                                         RpiitNumber = x.RpiitNumber,
                                         RpiitDate = x.RpiitDate,
                                         LetterNumber = x.LetterNumber,
                                         LetterDate = x.LetterDate,
                                         SkNumber = x.SkNumber,
                                         SkDate = x.SkDate,
                                         SkEndDate = x.SkEndDate,
                                         SkDuration = x.SkDuration,
                                         AdditionalInfo = x.AdditionalInfo,
                                         SkFile = x.SkFile,
                                         CompanyID = x.CompanyID,
                                         CreatedBy = x.CreatedBy,
                                         CreatedDate = x.CreatedDate,
                                         ModifiedBy = x.ModifiedBy,
                                         ModifiedDate = x.ModifiedDate,
                                         SertifikatCNC = x.SertifikatCNC
                                     };
            return Json(
                new
                {
                    skPerpanjangan = dataSkPerpanjangan
                }, JsonRequestBehavior.AllowGet);
        }
        //END OF SK PERPANJANGAN

        //SK PENYESUAIAN
        public JsonResult LoadSkPenyesuaian(string id)
        {

            var data = adjustedSkRepository.GetAll().Where(s => s.CompanyID == id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             RpiitNumber = x.RpiitNumber,
                             RpiitDate = x.RpiitDate,
                             LetterNumber = x.LetterNumber,
                             LetterDate = x.LetterDate,
                             SkNumber = x.SkNumber,
                             SkDate = x.SkDate,
                             //SkEndDate = x.SkEndDate,
                             //SkDuration = x.SkDuration,
                             AdditionalInfo = x.AdditionalInfo,
                             SkFile = x.SkFile,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,

                         };


            return Json(
                new
                {

                    skPenyesuaian = result

                }, JsonRequestBehavior.AllowGet);
        }
        //END OF SK PEYESUAIAN\


        //SK PENGHENTIAN SEMENTARA
        public JsonResult LoadSkPenghentianSementara(string id)
        {

            var data = tempBannedSkRepository.GetAll().Where(s => s.CompanyID == id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             SkDate = x.SkDate,
                             AdditionalInfo = x.AdditionalInfo,
                             SkFile = x.SkFile,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,

                         };


            return Json(
                new
                {

                    skPenghentianSementara = result

                }, JsonRequestBehavior.AllowGet);
        }
        //END OF SK PENGHENTIAN SEMENTARA

        //SK PENCABUTAN IJIN
        public JsonResult LoadSkPencabutanIjin(string id)
        {

            var data = bannedSkRepository.GetAll().Where(s => s.CompanyID == id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             SkDate = x.SkDate,
                             AdditionalInfo = x.AdditionalInfo,
                             SkFile = x.SkFile,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,

                         };


            return Json(
                new
                {

                    skPencabutanIjin = result

                }, JsonRequestBehavior.AllowGet);
        }
        //END OF SK PENGHENTIAN SEMENTARA

        //SK PENGAKTIFAN KEMBALI
        public JsonResult LoadSkPengaktifan(string id)
        {

            var data = reactivatedSkRepository.GetAll().Where(s => s.CompanyID == id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             SkDate = x.SkDate,
                             AdditionalInfo = x.AdditionalInfo,
                             SkFile = x.SkFile,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,

                         };


            return Json(
                new
                {
                    skPengaktifan = result
                }, JsonRequestBehavior.AllowGet);
        }
        //END OF SK PENGAKTIFAN KEMBALI

        //SK BKPM
        public JsonResult LoadSkBkpm(string id)
        {

            var data = bKPMRepository.GetByCompany(id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             LetterNumber = x.LetterNumber,
                             LetterDate = x.LetterDate,
                             LetterType = x.LetterType,
                             BKPMAcceptanceDate = x.BKPMAcceptanceDate,
                             EvaluatorAcceptanceDate = x.EvaluatorAcceptanceDate,
                             AdditionalInformation = x.AdditionalInformation,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,
                             SKFile = x.SKFile

                         };


            return Json(
                new
                {

                    skBkpm = result

                }, JsonRequestBehavior.AllowGet);
        }
        //END OF SK BKPM

        //ETRecommendation
        public JsonResult LoadETRecommendation(string id)
        {

            var data = eTRecommendationRepository.GetByCompany(id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             SkDate = x.SkDate,
                             AdditionalInfo = x.AdditionalInformation,
                             SkFile = x.SkFile,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,

                         };


            return Json(
                new
                {

                    ETRecommendation = result

                }, JsonRequestBehavior.AllowGet);
        }
        //END OF ETRecommendation


        //Additional COoperation
        public JsonResult LoadAdditionalCooperation(string id)
        {

            var data = additionalCooperationRepository.GetByCompany(id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             SkDate = x.SkDate,
                             AdditionalInfo = x.AdditionalInformation,
                             SkFile = x.SkFile,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,

                         };


            return Json(
                new
                {

                    AdditionalCooperation = result

                }, JsonRequestBehavior.AllowGet);
        }
        //Additional COoperation

        //CNCCertificate
        public JsonResult LoadCNCCertificate(string id)
        {

            var data = cNCCertificateRepository.GetByCompany(id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             SkDate = x.SkDate,
                             AdditionalInfo = x.AdditionalInformation,
                             SkFile = x.SkFile,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,

                         };


            return Json(
                new
                {

                    CNCCertificate = result

                }, JsonRequestBehavior.AllowGet);
        }
        //CNCCertificate

        //COmpanyFiles
        public JsonResult LoadCompanyFile(string id)
        {
            var data = companyFileRepository.GetByCompany(id)
                .OrderByDescending(s => s.CreatedDate);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             NomorSurat = x.NomorSurat,
                             NamaSurat = x.NamaSurat,
                             Pengirim = x.Pengirim,
                             TanggalSurat = x.TanggalSurat,
                             Tujuan = x.Tujuan,
                             Perihal = x.Perihal,
                             Description = x.Description,
                             CompanyID = x.CompanyID,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedDate = x.ModifiedDate,
                             FileName = x.FileName
                         };


            return Json(
                new
                {

                    CompanyFile = result

                }, JsonRequestBehavior.AllowGet);
        }
        //end of COmpanyFiles

        public async Task<ActionResult> Save(IEnumerable<HttpPostedFileBase> files, string id, string moduleType)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                var path = "";
                switch (moduleType)
                {
                    case "1":
                        path = @"~/Documents/SkAwal/";
                        break;

                    case "2":
                        path = @"~/Documents/SkAwal/IupOp/";
                        break;
                    case "3":
                        path = @"~/Documents/SkPerpanjangan/";
                        break;
                    case "4":
                        path = @"~/Documents/SkPerpanjangan/IupOp/";
                        break;
                    case "5":
                        path = @"~/Documents/SkPenyesuaian/";
                        break;
                    case "6":
                        path = @"~/Documents/SkPenghentianSementara/";
                        break;
                    case "7":
                        path = @"~/Documents/SKPencabutanIjin/";
                        break;
                    case "8":
                        path = @"~/Documents/SKPengaktifan/";
                        break;
                    case "9":
                        path = @"~/Documents/SKBkpm/";
                        break;
                    case "10":
                        path = @"~/Documents/SKBkpm/IupOp/";
                        break;
                    case "11":
                        path = @"~/Documents/ETRecommendation/";
                        break;
                    case "12":
                        path = @"~/Documents/AdditionalCooperation/";
                        break;
                    case "13":
                        path = @"~/Documents/CNCCertificate/";
                        break;
                    case "14":
                        path = @"~/Documents/CompanyFiles/";
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
                                //path = @"~/Documents/SkAwal/IupOp/";


                                if (!String.IsNullOrEmpty(fileName))
                                {
                                    var skawal = await firstSkRepository.FindAsync(id);
                                    skawal.SkFile = id + ext;
                                    await firstSkRepository.UpdateAsync(skawal);
                                }

                                break;
                            case "2":
                                //path = @"~/Documents/SkAwal/IupOp/";
                                var iupopskawal = await coalSourceSkRepository.FindAsync(id);
                                iupopskawal.SkFile = id + ext;
                                await coalSourceSkRepository.UpdateAsync(iupopskawal);
                                break;
                            case "3":
                                //path = @"~/Documents/SkPerpanjangan/";
                                if (!String.IsNullOrEmpty(fileName))
                                {
                                    var skperpanjangan = await extendedSkRepository.FindAsync(id);
                                    skperpanjangan.SkFile = id + ext;
                                    await extendedSkRepository.UpdateAsync(skperpanjangan);
                                }

                                break;
                            case "4":
                                //path = @"~/Documents/SkPerpanjangan/IupOp";
                                var skperpanjanganiupop = await coalSourceSkRepository.FindAsync(id);
                                skperpanjanganiupop.SkFile = id + ext;
                                await coalSourceSkRepository.UpdateAsync(skperpanjanganiupop);
                                break;
                            case "5":
                                //path = @"~/Documents/SkPenyesuaian/";
                                var skpenyesuaian = await adjustedSkRepository.FindAsync(id);
                                skpenyesuaian.SkFile = id + ext;
                                await adjustedSkRepository.UpdateAsync(skpenyesuaian);
                                break;
                            case "6":
                                //path = @"~/Documents/SkPenghentianSementara/";
                                var skpenghentianSementara = await tempBannedSkRepository.FindAsync(id);
                                skpenghentianSementara.SkFile = id + ext;
                                await tempBannedSkRepository.UpdateAsync(skpenghentianSementara);
                                break;
                            case "7":
                                //path = @"~/Documents/SKPencabutanIjin/";
                                var skpencabutanijin = await bannedSkRepository.FindAsync(id);
                                skpencabutanijin.SkFile = id + ext;
                                await bannedSkRepository.UpdateAsync(skpencabutanijin);
                                break;
                            case "8":
                                //  path = @"~/Documents/SKPengaktifan/";
                                var skpengaktifan = await reactivatedSkRepository.FindAsync(id);
                                skpengaktifan.SkFile = id + ext;
                                await reactivatedSkRepository.UpdateAsync(skpengaktifan);
                                break;
                            case "9":
                                //  path = @"~/Documents/SKBkpm/";
                                if (!String.IsNullOrEmpty(fileName))
                                {
                                    var skbkpm = await bKPMRepository.FindAsync(id);
                                    skbkpm.SKFile = id + ext;
                                    await bKPMRepository.UpdateAsync(skbkpm);
                                }

                                break;
                            case "10":
                                //  path = @"~/Documents/SKBkpm/IupOp";
                                var skbkpmiupop = await coalSourceSkRepository.FindAsync(id);
                                skbkpmiupop.SkFile = id + ext;
                                await coalSourceSkRepository.UpdateAsync(skbkpmiupop);
                                break;
                            case "11":
                                //   path = @"~/Documents/SKBkpm/ETRecommendation";
                                var etRecommendation = await eTRecommendationRepository.FindAsync(id);
                                etRecommendation.SkFile = id + ext;
                                await eTRecommendationRepository.UpdateAsync(etRecommendation);
                                break;
                            case "12":
                                //   path = @"~/Documents/SKBkpm/AdditionalCooperation";
                                var additionalCoop = await additionalCooperationRepository.FindAsync(id);
                                additionalCoop.SkFile = id + ext;
                                await additionalCooperationRepository.UpdateAsync(additionalCoop);
                                break;
                            case "13":
                                //   path = @"~/Documents/SKBkpm/CNCCertificate";
                                var cncCert = await cNCCertificateRepository.FindAsync(id);
                                cncCert.SkFile = id + ext;
                                await cNCCertificateRepository.UpdateAsync(cncCert);
                                break;
                            case "14":
                                //   path = @"~/Documents/SKBkpm/CompanyFiles";
                                var compFile = await companyFileRepository.FindAsync(id);
                                compFile.FileName = id + ext;
                                await companyFileRepository.UpdateAsync(compFile);
                                break;
                        }

                    }
                    catch (Exception ex) { }
                }
            }

            // Return an empty string to signify success
            return Content("OK");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult PreCreation()
        {
            return View();
        }
        public ActionResult DownloadViewPDF(string id)
        {
            //var model = new IupOpPrintViewModel();
            //return new Rotativa.ActionAsPdf("PrintTemplate"+"/"+id, model)
            //{
            //    FileName = "PrintSk.pdf",
            //    PageHeight = 630,
            //    PageWidth = 1200
            //};
            //return new Rotativa.UrlAsPdf("google.com");
            //return new Rotativa.UrlAsPdf(Url.Action("PrintTemplate" + "/" + id, "IupOpAngkutJual", new { area = "AngkutJual"}))
            //{ PageOrientation = Orientation.Landscape,
            //    PageHeight = 1200,
            //    PageWidth = 1000,
            //    CustomSwitches = "--viewport-size 1000x1000" };
            //return new Rotativa.ActionAsPdf("PrintTemplate" + "/" + id)
            return new Rotativa.UrlAsPdf(Url.Action("PrintTemplate/" + id, "IupOpAngkutJualList", new {area="AngkutJual"} ))
            {
                FileName = "PrintSk.pdf",
                PageSize = Size.A4,
                PageHeight = 600,
                PageWidth = 200
            };
        }
        public ActionResult testprint()
        {
            return View();
        }

        public async Task<JsonResult> LoadDetails(string id)
        {
            //var company = await companyRepository.FindAsync(id);
            var shareHolders = shareHolderRepository.FindByCompany(id);
            var company = from a in companyRepository.GetAll().AsEnumerable()
                          join b in companyAddressRepository.GetAll().AsEnumerable()
                          on a.ID equals b.CompanyID
                          where b.CompanyID == id
                          select new PreCreationViewModel
                          {
                              ID = a.ID,
                              Name = a.Name,
                              NoUrutBerkas = a.NoUrutBerkas,
                              NoTelp = b.TelNumber,
                              NoHand = b.MobileNumber,
                              Email = b.Email,
                              NPWP = a.NPWP,
                              StatusIzin = a.StatusIzin,
                              Address = b.Address,
                              Keterangan = b.AdditionalInfo,
                              Website = b.Website,
                              Fax = b.Fax,
                              CPName = b.CPName
                          };
            return Json(
                new
                {
                    company = company,
                    shareHolders = shareHolders,

                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSkAwal(string id)
        {
            var skAwal = firstSkRepository.GetAll().Where(s => s.CompanyID == id);
            var dataSkAwal = from x in skAwal
                             select new
                             {
                                 ID = x.ID,
                                 LetterNumber = x.LetterNumber,
                                 LetterDate = x.LetterDate,
                                 SkNumber = x.SkNumber,
                                 SkDate = x.SkDate,
                                 SkEndDate = x.SkEndDate,
                                 SkDuration = x.SkDuration,
                                 AdditionalInfo = x.AdditionalInfo,
                                 SkFile = x.SkFile,
                                 CompanyID = x.CompanyID,
                                 CreatedBy = x.CreatedBy,
                                 CreatedDate = x.CreatedDate,
                                 ModifiedBy = x.ModifiedBy,
                                 ModifiedDate = x.ModifiedDate,

                                 //CoalSourceSks = x.CoalSourceSks.Select(y => new
                                 //{
                                 //    ID = y.ID,
                                 //    SkNumber = y.SkNumber,
                                 //    SkDate = y.SkDate,
                                 //    SkFile = y.SkFile,
                                 //    CompanySource = y.CompanySource,
                                 //    CompanySourceAddress = y.CompanySourceAddress,
                                 //    Province = y.Province,
                                 //    Tonnage = y.Tonnage,
                                 //    FirstSkID = y.FirstSkID,
                                 //    ExtendedSkID = y.ExtendedSkID,
                                 //    CreatedBy = y.CreatedBy,
                                 //    CreatedDate = y.CreatedDate,
                                 //    ModifiedBy = y.ModifiedBy,
                                 //    ModifiedDate = y.ModifiedDate,

                                 //}).ToList()
                             };


            return Json(
                new
                {
                    skAwal = dataSkAwal.OrderByDescending(s => s.CreatedDate)

                }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindFirstSkSource(string id)
        {
            var data = await firstSkSourceRepo.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                CompanyName = data.CompanyName,
                Volume = data.Volume,
                FirstSkID = data.FirstSkID
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<string> EditServiceFirstSkSource(FirstSkSource firstSkSource)
        {
            if (ModelState.IsValid)
            {
                var result = await firstSkSourceRepo.UpdateAsync(firstSkSource);
                return result.ID;
            }
            return "-1";

        }
        [HttpPost]
        public async Task<string> DeleteServiceFirstSkSource(string id)
        {
            var data = await firstSkSourceRepo.FindAsync(id);
            await firstSkSourceRepo.RemoveAsync(data);
            return "OK";
        }

        public JsonResult LoadIupOpSkAwal(string id)
        {
            var data = firstSkSourceRepo.GetAll().Where(c => c.FirstSkID == id);
            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             CompanyName = x.CompanyName,
                             Volume = x.Volume,
                             FirstSkID = x.FirstSkID
                         };

            return Json(
                new
                {

                    data = result

                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadIupOpSkPerpanjangan(string id)
        {

            var data = coalSourceSkRepository.GetAll()
                .Where(s => s.ExtendedSkID == id)
                .OrderByDescending(s => s.CreatedDate);

            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             SkFile = x.SkFile,
                             CompanySource = x.CompanySource,
                             CompanySourceAddress = x.CompanySourceAddress,
                             City = x.City,
                             Province = x.Province,
                             SalesDestination = x.SalesDestination,
                             Tonnage = x.Tonnage,
                         };


            return Json(
                new
                {

                    data = result

                }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadIupOpSkBkpm(string id)
        {

            var data = coalSourceSkRepository.GetAll()
                .Where(s => s.BKPMID == id)
                .OrderByDescending(s => s.CreatedDate);

            var result = from x in data
                         select new
                         {
                             ID = x.ID,
                             SkNumber = x.SkNumber,
                             SkFile = x.SkFile,
                             CompanySource = x.CompanySource,
                             CompanySourceAddress = x.CompanySourceAddress,
                             City = x.City,
                             Province = x.Province,
                             SalesDestination = x.SalesDestination,
                             Tonnage = x.Tonnage,
                         };


            return Json(
                new
                {

                    data = result

                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadRkab(string id)
        {
            var rkab = rkabRepository.GetAll().Where(s => s.CompanyID == id);
            var dataRkab = from x in rkab.OrderByDescending(s => s.CreatedDate)
                           select new
                           {
                               ID = x.ID,
                               RkabYear = x.RkabYear,
                               Status = x.Status,
                               CompanyID = x.CompanyID,
                           };


            return Json(
                new
                {

                    rkab = dataRkab

                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<string> CreateReportService(Report model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Guid.NewGuid().ToString();
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;
                model.StatusTegur = null;

                var result = await reportRepository.AddAsync(model);
                return result.ID;
            }
            return "-1";
        }


        [HttpPost]
        public async Task<string> DeleteReport(string id)
        {
            Report model = await reportRepository.FindAsync(id);
            await reportRepository.RemoveAsync(model);
            return "OK";
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditReportService(Report model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = User.Identity.Name;
                model.ModifiedDate = DateTime.Now;
                await reportRepository.UpdateAsync(model);
                return model.ID;
            }
            return "0";
        }

        public async Task<JsonResult> findReport(string id)
        {
            var data = await reportRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Rkab = data.Rkab,
                Q1 = data.Q1,
                Q2 = data.Q2,
                Q3 = data.Q3,
                Q4 = data.Q4,
                Annual = data.Annual,
                Year = data.Year,
                CompanyID = data.CompanyID

            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadReport(string id)
        {
            var report = reportRepository.GetAll().Where(c => c.CompanyID == id);
            var dataReport = from c in report.OrderByDescending(c => c.Year)
                             select new ReportViewModel2
                             {
                                 ID = c.ID,
                                 Year = c.Year,
                                 Rkab = c.Rkab,
                                 Q1 = c.Q1,
                                 PersenQ1 = Math.Round(((decimal)c.Q1 * 100 / (decimal)c.Rkab), 2), //((c.Q1 * 100) / c.Rkab).ToString("0.00"),
                                 Q2 = c.Q2,
                                 PersenQ2 = Math.Round(((decimal)c.Q2 * 100 / (decimal)c.Rkab), 2),
                                 Q3 = c.Q3,
                                 PersenQ3 = Math.Round(((decimal)c.Q3 * 100 / (decimal)c.Rkab), 2),
                                 Q4 = c.Q4,
                                 PersenQ4 = Math.Round(((decimal)c.Q4 * 100 / (decimal)c.Rkab), 2),
                                 Annual = c.Annual,
                                 PersenAnnual = Math.Round(((decimal)c.Annual * 100 / (decimal)c.Rkab), 2),
                                 CompanyID = c.CompanyID
                             };
            return Json(
                new
                {
                    report = dataReport
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadQuarter(string id)
        {
            var quarter = quarterlyRepository.GetAll().Where(s => s.CompanyID == id);
            var dataquarter = from x in quarter.OrderByDescending(s => s.CreatedDate)
                              select new
                              {
                                  ID = x.ID,
                                  Year = x.Year,
                                  Status = x.Status,
                                  CompanyID = x.CompanyID,
                                  Period = x.Period

                              };


            return Json(
                new
                {

                    quarter = dataquarter

                }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadAnnual(string id)
        {
            var rkab = yearlyRepository.GetAll().Where(s => s.CompanyID == id);
            var dataRkab = from x in rkab.OrderByDescending(s => s.CreatedDate)
                           select new
                           {
                               ID = x.ID,
                               Year = x.Year,
                               Status = x.Status,
                               CompanyID = x.CompanyID,

                           };


            return Json(
                new
                {

                    annual = dataRkab

                }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PreCreation(PreCreationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Company company = new Company()
            {
                ID = Guid.NewGuid().ToString(),
                Name = model.Name,
                NPWP = model.NPWP,
                NoUrutBerkas = model.NoUrutBerkas,
                CreatedBy = User.Identity.Name,
                CreatedDate = DateTime.Now
            };

            CompanyAddress ca = new CompanyAddress()
            {
                ID = Guid.NewGuid().ToString(),
                Address = model.Address,
                CompanyID = company.ID,
                CreatedBy = User.Identity.Name,
                CreatedDate = DateTime.Now
            };

            await companyRepository.AddAsync(company);
            await companyAddressRepository.AddAsync(ca);

            CompanyMiningModulDetail cmd = new CompanyMiningModulDetail()
            {
                ID = Guid.NewGuid().ToString(),
                CompanyID = company.ID,
                MiningModuleID = "2"
            };
            await companyMiningModulDetailRepository.AddAsync(cmd);
            return RedirectToAction("details", new { id = company.ID });
        }

        public ActionResult Details(string id)
        {
            ViewBag.idx = id;
            return View();
        }
    }

    public class PreCreationViewModel
    {
        public string ID { get; set; }
        [Required]
        [RegularExpression("^[A-Z0-9 ]*$", ErrorMessage ="Company doesn't allowed '.' character")]
        public string Name { get; set; }
        [Required]
        public string NoUrutBerkas { get; set; }
        [Required]
        public string NPWP { get; set; }
        [Required]
        public string Address { get; set; }
        public string Keterangan { get; set; }
        public string NoTelp { get; set; }
        public string NoHand { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string CPName { get; set; }
        public Nullable<bool> StatusIzin { get; set; }
    }


    public class WarningLetterViewModel
    {

        public string ID { get; set; }

        public string LetterNumber { get; set; }
        public Nullable<DateTime> LetterDate { get; set; }
        public Nullable<DateTime> SkEndDate { get; set; }

        public string SkFile { get; set; }
        public string WarningDuration { get; set; }
        public string ObligationsShould { get; set; }
        public string ObligationsYet { get; set; }
        public string ObligationsAlready { get; set; }
        public string AdditionalInfo { get; set; }
        public string Keterangan { get; set; }
        public string WarningType { get; set; } //SP1 SP2 SP3
        public string CompanyName { get; set; }
        public string DestinationEmail { get; set; }
        public string DestinationMobileNo { get; set; }
        public string CompanyID { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    

    public class RKABViewModel2
    {
        public string ID { get; set; }

        public Nullable<int> RkabYear { get; set; }//No_SK
        public Nullable<bool> Status { get; set; }//Tgl_SK

        public string CompanyID { get; set; }
        public string Name { get; set; }
        public virtual Company Company { get; set; }
    }
    public class YearAndQuarterly
    {
        public string ID { get; set; }

        public Nullable<int> Year { get; set; }//No_SK
        public Nullable<bool> Status { get; set; }//Tgl_SK

        public string CompanyID { get; set; }
        public string Name { get; set; }
        public virtual Company Company { get; set; }
    }
    public class SourceChangesViewModel
    {
        public string ID { get; set; }
        public string LetterNumber { get; set; }
        public string LetterDate { get; set; }
        public string SkNumber { get; set; }
        public string Sumber { get; set; }
        public string SkDate { get; set; }
        public string CompanyID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
    public class ExtendedSkViewModel
    {
        public string ID { get; set; }
        public string Sumber { get; set; }
        public string SertifikatCNC { get; set; }
        public string LetterNumber { get; set; }
        public Nullable<DateTime> LetterDate { get; set; }
        public string RpiitNumber { get; set; }
        public Nullable<DateTime> RpiitDate { get; set; }
        public string SkNumber { get; set; }
        public Nullable<DateTime> SkDate { get; set; }
        public Nullable<DateTime> SkEndDate { get; set; }
        public string SkDuration { get; set; }
        public string AdditionalInfo { get; set; }
        public string SkFile { get; set; }
        public string CompanyID { get; set; }
    }
    public class ExtendedSkSourceViewModel
    {
        public string ID { get; set; }
        public string CompanyName { get; set; }
        public string Volume { get; set; }
        public string ExtendedSkID { get; set; }
        public string CompanyID { get; set; }
    }

    public class SourceChangesSkSourceViewModel
    {
        public string ID { get; set; }
        public string CompanyName { get; set; }
        public string LetterNumber { get; set; }

        public string Volume { get; set; }
        public string SourceChangesID { get; set; }
        public string CompanyID { get; set; }
        public string Sumber { get; set; }
        public string Volume2 { get; set; }
    }
    public class SkPengakhiranViewModel
    {
        public string ID { get; set; }
        public string SkNumber { get; set; }
        public string LetterNumber { get; set; }
        public string LetterDate { get; set; }
        public string SkDate { get; set; }
        public string AdditionalInfo { get; set; }
        public string SkFile { get; set; }

        public string CompanyID { get; set; }

        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }

        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> SkEndDate { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
    }
    

    public class SkPerpanjanganViewModel
    {
        public string ID { get; set; }
        public string LetterNumber { get; set; }
        public string LetterDate { get; set; } //tanggal permohonan
        public string RpiitNumber { get; set; }
        public Nullable<DateTime> RpiitDate { get; set; }
        public string SkNumber { get; set; }
        public string SkDate { get; set; }
        public string SkEndDate { get; set; }
        public string SkDuration { get; set; }
        public string AdditionalInfo { get; set; }
        public string SkFile { get; set; }
        public string CompanyID { get; set; }
        public string SertifikatCNC { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}