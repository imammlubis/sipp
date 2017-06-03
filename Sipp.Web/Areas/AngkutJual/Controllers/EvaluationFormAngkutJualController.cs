using AutoMapper;
using EduSpot.Entity.Tables.AngkutJual;
using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.Organization;
using Esdm.Web.Areas.AngkutJual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class EvaluationFormAngkutJualController : Controller
    {
        private IShareHolderRepository shareHolderRepository = new ShareHolderRepository();
        private ICompanyRepository companyRepository = new CompanyRepository();
        private ICompanyAddressRepository companyAddressRepository = new CompanyAddressRepository();
        private ICoalSourceSkRepository coalSourceSkRepository = new CoalSourceSkRepository();
        private IFirstSkRepository firstSkRepository = new FirstSkRepository();
        private IEvaluationRepository evaluationRepository = new EvaluationRepository();

        public ActionResult Index(EvaluationFormViewModel eval)
        {
            var data = from a in companyRepository.GetAll().AsEnumerable()
                       select new EvaluationFormViewModel {
                           ID = a.ID.ToString(),
                           Name = a.Name
                       };
            var x = data.ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.ID,

            }).ToList();

            ViewBag.CompanyList = x;

            GetShareHolder("");

            return View(eval);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EvaluationFormViewModel evaluationViewModel, FormCollection fc)
        {            
            if (!ModelState.IsValid || String.IsNullOrEmpty(evaluationViewModel.CompanyIds))
            {
                return View(evaluationViewModel);
            }
            try
            {
                Evaluation evaluation = new Evaluation()
                {
                    ID = Guid.NewGuid().ToString(),
                    CreatedBy = User.Identity.Name,
                    CreatedDate = DateTime.Now,
                    CompanyID = evaluationViewModel.CompanyIds,
                    ReportSubmittedDate = DateTime.Parse(evaluationViewModel.ReportSubmittedDate),
                    NoIntroductoryLetter = evaluationViewModel.NoIntroductoryLetter,
                    NoDisposition = evaluationViewModel.NoDisposition,
                    CoalOrigin = evaluationViewModel.CoalOrigin,
                    EndUser = evaluationViewModel.EndUser,
                    Tonnage = evaluationViewModel.Tonnage,
                    ActivityPlan = evaluationViewModel.ActivityPlan,
                    ActivityRealization = evaluationViewModel.ActivityRealization,

                    AcceptanceStatus = evaluationViewModel.CoalSourceSk == evaluationViewModel.CoalOrigin ? true : false,

                    ReportType = evaluationViewModel.ReportType,

                    Revenue = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.Revenue : null,
                    BasicPrice = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.BasicPrice : null,
                    ProfitBefore = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.ProfitBefore : null,
                    OrganizationTax = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.OrganizationTax : null,
                    Pph = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.Pph : null,
                    Profit = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.Profit : null,

                    RevenueUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.Revenue : null,
                    BasicPriceUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.BasicPrice : null,
                    ProfitBeforeUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.ProfitBefore : null,
                    OrganizationTaxUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.OrganizationTax : null,
                    PphUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.Pph : null,
                    ProfitUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.Profit : null,

                    AdditionalInformation = evaluationViewModel.AdditionalInformation
                };
                await evaluationRepository.AddAsync(evaluation);
            }
            catch { }
            //ShareHolder sh0 = new ShareHolder()
            //{
            //    ID = Guid.NewGuid().ToString(),
            //    Name = fc["shnama0"],
            //    TotalStock = fc["shjumlah0"],
            //    StatusWnBh = fc["shstatus0"],
            //    Status = true,
            //    CompanyID = evaluationViewModel.CompanyIds
            //};
            //await shareHolderRepository.AddAsync(sh0);

            //for (int i = 0; i < fc["shnama"].Split(',').Count(); i++)
            //{
            //    string nama, jumlah, nilai, wnbh;
            //    nama = fc["shnama"].Split(',')[i].ToString();
            //    jumlah = fc["shjumlah"].Split(',')[i].ToString();
            //    nilai = fc["shnilai"].Split(',')[i].ToString();
            //    wnbh = fc["shstatus"].Split(',')[i].ToString();
            //    ShareHolder sh = new ShareHolder()
            //    {
            //        ID= Guid.NewGuid().ToString(),
            //        Name = nama,
            //        TotalStock = jumlah,
            //        StatusWnBh = wnbh,
            //        Status = true,
            //        CompanyID = evaluationViewModel.CompanyIds
            //    };
            //    await shareHolderRepository.AddAsync(sh);
            //}
            return RedirectToAction("Index", "EvaluationHistories");
        }
        [HttpPost]
        public async Task<ActionResult> Update(EvaluationFormViewModel evaluationViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Evaluation evaluation = new Evaluation()
                    {
                        ID = evaluationViewModel.ID,
                        CreatedBy = User.Identity.Name,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now,
                        CompanyID = evaluationViewModel.CompanyId,
                        ReportSubmittedDate = DateTime.Parse(evaluationViewModel.ReportSubmittedDate),
                        NoIntroductoryLetter = evaluationViewModel.NoIntroductoryLetter,
                        NoDisposition = evaluationViewModel.NoDisposition,
                        CoalOrigin = evaluationViewModel.CoalOrigin,
                        EndUser = evaluationViewModel.EndUser,
                        Tonnage = evaluationViewModel.Tonnage,
                        ActivityPlan = evaluationViewModel.ActivityPlan,
                        ActivityRealization = evaluationViewModel.ActivityRealization,

                        AcceptanceStatus = evaluationViewModel.CoalSourceSk == evaluationViewModel.CoalOrigin ? true : false,

                        ReportType = evaluationViewModel.ReportType,

                        Revenue = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.Revenue : null,
                        BasicPrice = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.BasicPrice : null,
                        ProfitBefore = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.ProfitBefore : null,
                        OrganizationTax = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.OrganizationTax : null,
                        Pph = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.Pph : null,
                        Profit = evaluationViewModel.CurrencyType == "1" ? evaluationViewModel.Profit : null,

                        RevenueUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.Revenue : null,
                        BasicPriceUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.BasicPrice : null,
                        ProfitBeforeUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.ProfitBefore : null,
                        OrganizationTaxUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.OrganizationTax : null,
                        PphUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.Pph : null,
                        ProfitUSD = evaluationViewModel.CurrencyType == "2" ? evaluationViewModel.Profit : null,

                        AdditionalInformation = evaluationViewModel.AdditionalInformation
                    };
                    await evaluationRepository.UpdateAsync(evaluation);
                }
                catch { }
            }
            return RedirectToAction("Index", "EvaluationHistories");
        }

        public ActionResult Edit(string id)
        {
            var address = companyAddressRepository.GetAll().ToList();
            var firstSk = firstSkRepository.GetAll().ToList();
            var coalSource = coalSourceSkRepository.GetAll().ToList();

            var data = from a in companyRepository.GetAll().AsEnumerable()
                       select new EvaluationFormViewModel
                       {
                           ID = a.ID.ToString(),
                           Name = a.Name
                       };
            var x = data.ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.ID,
            }).ToList();
            ViewBag.CompanyList = x;
            
            var model = new EvaluationFormViewModel();
            var idData = evaluationRepository.GetById(id);
            Mapper.DynamicMap(idData, model);

            var datas = (from z in companyRepository.GetAll().AsEnumerable()
                         join a in address on z.ID equals a.CompanyID
                         join b in firstSk on a.CompanyID equals b.CompanyID
                         join c in coalSource on b.ID equals c.FirstSkID
                         where a.CompanyID == model.CompanyId
                         select new EvaluationFormViewModel
                         {
                             CompanyIds = id,
                             Name = z.Name,
                             Address = a.Address,
                             TelNumber = a.TelNumber,
                             Email = a.Email,
                             SkNumber = b.SkNumber,
                             SkDate = b.SkDate,
                             CompanySourceAddress = c.CompanySourceAddress,
                             Province = c.Province
                         }).ToList();
            model.Address = datas[0].Address;
            model.Name = datas[0].Name;
            model.TelNumber = datas[0].TelNumber;
            model.Email = datas[0].Email;
            model.SkNumber = datas[0].SkNumber;
            model.SkDate2 = datas[0].SkDate.Value.ToShortDateString();
            model.CoalSourceSk = datas[0].Province + "/" + datas[0].CompanySourceAddress;
            return View("Edit", model);
        }

        public JsonResult GetCompanyData(string id) {
            var companies = companyRepository.GetAll().Where(c=>c.ID == id).ToList();
            return Json(companies);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Name,NPWP,StatusIzin,TahapIup,NoUrutBerkas,IupTypeID,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")]
        //Company company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        company.ModifiedBy = User.Identity.Name;
        //        company.ModifiedDate = DateTime.Now;
        //        evaluationRepository.UpdateAsync(company);
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.IupTypeID = new SelectList(iupTypeRepository.GetAll(), "ID", "Name", company.IupTypeID);
        //    return View(company);
        //}
        [HttpGet]
        public ActionResult Create(EvaluationFormViewModel eval)
        {
            return View(eval);
        }

        [HttpGet]
        public JsonResult CompanyDetails(string CompanyId)
        {
            var address = companyAddressRepository.GetAll().ToList();
            var firstSk = firstSkRepository.GetAll().ToList();
            var coalSource = coalSourceSkRepository.GetAll().ToList();

            var datas = (from a in address
                         join b in firstSk on a.CompanyID equals b.CompanyID
                         join c in coalSource on b.ID equals c.FirstSkID
                         where a.CompanyID == CompanyId
                         select new EvaluationFormViewModel
                         {
                             CompanyIds = CompanyId,
                             Address = a.Address,
                             TelNumber = a.TelNumber,
                             Email = a.Email,
                             SkNumber = b.SkNumber,
                             SkDate2 = b.SkDate.Value.ToShortDateString(),
                             CompanySourceAddress = c.CompanySourceAddress,
                             Province = c.Province
                         }).ToList();

            GetShareHolder(datas[0].CompanyIds);

            return Json(datas[0], JsonRequestBehavior.AllowGet);
        }

        public List<ShareHolder> GetShareHolder(string id)
        {
            List<ShareHolder> sh = shareHolderRepository.FindByCompany(id).ToList();
            ViewData["ShareHolder"] = sh;
            return sh;
        }

        [HttpPost]
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
            return "0";
        }


    }
}