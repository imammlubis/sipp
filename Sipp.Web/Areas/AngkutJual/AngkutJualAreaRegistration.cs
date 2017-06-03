using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual
{
    public class AngkutJualAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AngkutJual";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AngkutJual_default",
                "AngkutJual/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}