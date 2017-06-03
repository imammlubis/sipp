using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class CompanyHistoriesController : Controller
    {
        private ICompanyRepository companyRepository = new CompanyRepository();
        private ICompanyAddressRepository companyAddressRepository = new CompanyAddressRepository();

        private ICompanyHistoryRepository companyHistoryRepository = new CompanyHistoryRepository();
        private ICompanyAddressHistoryRepository companyAddressHistoryRepository = new CompanyAddressHistoryRepository();
        // GET: AngkutJual/CompanyHistories
        public ActionResult Index(string id)
        {
            return View();
        }
        public ActionResult ViewHistory(string id)
        {
            var companyName = companyRepository.GetAll().Where(s => s.ID.Equals(id));
            ViewBag.companyName = companyName.ToList()[0].Name;
            ViewBag.companyId = companyName.ToList()[0].ID;
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<CompanyAddress> dataGrid = companyAddressRepository.GetAll();
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListCompanyHistory([DataSourceRequest] DataSourceRequest request, string id)
        {
            var dataGrid = from a in companyHistoryRepository.GetAll().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable()
                           on a.CompanyID equals b.ID
                           where a.CompanyID.Equals(id)
                           select new EduSpot.Entity.Tables.Organization.CompanyHistory
                           {
                               //Name = b.Name,
                               CompanyID = a.CompanyID,
                               NPWP = a.NPWP,
                               StatusIzin = a.StatusIzin,
                               TahapIup = a.TahapIup,
                               NoUrutBerkas = a.NoUrutBerkas,
                               IupTypeID = a.IupTypeID,
                               CreatedBy = a.CreatedBy,
                               CreatedDate = a.CreatedDate
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListCompanyAddressHistory([DataSourceRequest] DataSourceRequest request, string id)
        {
            IQueryable<CompanyAddressHistory> dataGrid = companyAddressHistoryRepository.FindById(id);
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public class CompanyHistoryViewModel
        {
            public string ID { get; set; }

            public Nullable<int> Year { get; set; }//No_SK
            public Nullable<bool> Status { get; set; }//Tgl_SK

            public string CompanyID { get; set; }
            public string Name { get; set; }
        }
    }
}