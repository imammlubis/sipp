using System.Web.Mvc;

namespace Esdm.Web.Areas.ReportCompany
{
    public class ReportCompanyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ReportCompany";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ReportCompany_default",
                "ReportCompany/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}