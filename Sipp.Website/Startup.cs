using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sipp.Website.Startup))]
namespace Sipp.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
