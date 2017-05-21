using System.Web;
using System.Web.Optimization;

namespace Sipp.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            //for kendo ui bundle
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
              "~/Scripts/kendo/2016.2.607/kendo.all.min.js",
              // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
              "~/Scripts/kendo/2016.2.607/kendo.aspnetmvc.min.js"));

            //bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
            //    "~/Content/kendo/2016.2.607kendo.common-bootstrap.min.css",
            //    "~/Content/kendo/2016.2.607kendo.bootstrap.min.css"));

        }
    }
}
