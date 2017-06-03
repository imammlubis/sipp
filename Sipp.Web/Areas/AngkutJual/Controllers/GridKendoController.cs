using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using EduSpot.Entity.Tables.Organization;
using EduSpot.Core.Infrastructure;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class GridKendoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ICompanyRepository companyRepository = new CompanyRepository();
        public ActionResult Index()
        {
            var data = companyRepository.GetList();
            return View(data.ToList());
        }

        public ActionResult Company_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Company> company = companyRepository.GetAll();

            DataSourceResult result = company.ToDataSourceResult(request);

            //DataSourceResult result = company.ToDataSourceResult(request, company => new {
            //    ID = company.ID,
            //    Name = company.Name,
            //    NPWP = company.NPWP,
            //    StatusIzin = company.StatusIzin,
            //    TahapIup = company.TahapIup,
            //    NoUrutBerkas = company.NoUrutBerkas,
            //    CreatedBy = company.CreatedBy,
            //    CreatedDate = company.CreatedDate,
            //    ModifiedBy = company.ModifiedBy,
            //    ModifiedDate = company.ModifiedDate
            //});

            return Json(result);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    
        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
