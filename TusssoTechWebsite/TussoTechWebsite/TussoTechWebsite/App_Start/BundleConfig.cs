using System.Web;
using System.Web.Optimization;

namespace TussoTechWebsite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
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
                      "~/Scripts/jquery.fitvids.js",
                      "~/Scripts/owl.carousel.min.js",
                      "~/Scripts/nivo-lightbox.min.js",
                      "~/Scripts/jquery.isotope.min.js",
                      "~/Scripts/jquery.appear.js",
                      "~/Scripts/count-to.js",
                      "~/Scripts/jquery.textillate.js",
                      "~/Scripts/jquery.lettering.js",
                      "~/Scripts/jquery.easypiechart.min.js",
                      "~/Scripts/jquery.nicescroll.min.js",
                      "~/Scripts/jquery.parallax.js",
                      "~/Scripts/mediaelement-and-player.js",
                      "~/Scripts/script.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/style.css",
                      "~/Content/responsive.css",
                      "~/Content/animate.css",
                      "~/Content/colors/green.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
