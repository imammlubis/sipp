using System.Web.Mvc;

namespace Esdm.Web.Areas.KontrakKarya
{
    public class KontrakKaryaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "KontrakKarya";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KontrakKarya_default",
                "KontrakKarya/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}