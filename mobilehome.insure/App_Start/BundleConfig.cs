using System.Web;
using System.Web.Optimization;

namespace MobileHome.Insure.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Content/Plugins/pluginscripts").Include(
                "~/Content/Plugins/owl-carousel/*.js",
                "~/Content/Plugins/easytabs/easyResponsiveTabs.js",
                "~/Content/Plugins/jflickrfeed/*.js",
                "~/Scripts/jquery.easing.min.js",
                "~/Scripts/jCProgress-1.0.3.js",
                "~/Content/Plugins/flex-slider/jquery.flexslider.js",
                "~/Scripts/jquery.appear.js",
                "~/Scripts/jquery.inview.js",
                "~/Scripts/jquery.prettyphoto.js",
                "~/Scripts/jquery.nicescroll.js"
                //,"~/Scripts/gmaps.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Custom/customScripts").Include(
                "~/Scripts/Custom/contact.js",
                "~/Scripts/Custom/main.js"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrapscript").Include(
                "~/Scripts/bootstrap*"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrapStyle").Include(
                "~/Content/css/bootstrap*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/stylecss").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/Plugins/owl-carousel/carouselStyles").Include(
                "~/Content/Plugins/owl-carousel/owl.carousel.css",
                "~/Content/Plugins/owl-carousel/owl.theme.css",
                "~/Content/Plugins/owl-carousel/owl.transitions.css"));

            bundles.Add(new StyleBundle("~/Content/Plugins/easytabs/cssStyles").Include(
                "~/Content/Plugins/easytabs/easy-responsive-tabs.css"));

            bundles.Add(new StyleBundle("~/Content/Plugins/flex-slider/sliderStyle").Include(
                "~/Content/Plugins/flex-slider/flexslider.css"));

            bundles.Add(new StyleBundle("~/Content/css/prettyPhoto").Include(
                "~/Content/css/prettyphoto.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome/css/fontstyle").Include(
            "~/Content/font-awesome/css/*.css"));

            bundles.Add(new StyleBundle("~/Content/css/CustomStyle").Include(
                "~/Content/css/style.css",
                "~/Content/css/style-responsive.css",
               "~/Content/css/custom.css" ));
        }
    }
}