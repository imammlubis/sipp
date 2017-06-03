using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.AngkutJual;
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
    [Authorize(Roles = "AngkutJualAdmin")]
    public class NotificationLogController : Controller
    {
        private ICompanyRepository companyRepository = new CompanyRepository();
        private INotificationLogAngkutJualRepository notifLogRepo = new NotificationLogAngkutJualRepository();
        // GET: AngkutJual/NotificationLog
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            var dataGrid = from a in notifLogRepo.GetAll().AsEnumerable()
                           join b in companyRepository.GetAll().AsEnumerable()
                           on a.CompanyId equals b.ID
                           select new NotificationLogViewModel
                           {
                               IdNotificationLog = a.IdNotificationLog,
                               NotificationLogDate = a.NotificationLogDate,
                               Email = a.Email,
                               MobileNo = a.MobileNo,
                               TglSuratPeringatan = a.TglSuratPeringatan,
                               TglAkhirPeringatan = a.TglAkhirPeringatan,
                               CompanyId = a.CompanyId,
                               NotificationsContent = a.NotificationsContent,
                               CompanyName = b.Name
                           };
            DataSourceResult result = dataGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public class NotificationLogViewModel
        {
            public string IdNotificationLog { get; set; }
            public string CompanyName { get; set; }
            public Nullable<DateTime> NotificationLogDate { get; set; }
            public string Email { get; set; }
            public string MobileNo { get; set; }
            public Nullable<DateTime> TglSuratPeringatan { get; set; }
            public Nullable<DateTime> TglAkhirPeringatan { get; set; }
            public string CompanyId { get; set; }
            public string NotificationsContent { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public string ModifiedDate { get; set; }
        }

    }
}