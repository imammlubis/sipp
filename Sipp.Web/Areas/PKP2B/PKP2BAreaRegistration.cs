using System.Web.Mvc;

namespace Esdm.Web.Areas.PKP2B
{
    public class PKP2BAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PKP2B";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PKP2B_default",
                "PKP2B/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}