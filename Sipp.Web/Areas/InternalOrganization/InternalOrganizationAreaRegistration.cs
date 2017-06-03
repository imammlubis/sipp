using System.Web.Mvc;

namespace Sipp.Web.Areas.InternalOrganization
{
    public class InternalOrganizationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InternalOrganization";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InternalOrganization_default",
                "InternalOrganization/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}