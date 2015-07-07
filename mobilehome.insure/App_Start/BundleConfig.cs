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
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new StyleBundle("~/bundles/jqueryui/css")
            .Include("~/Content/css/jquery-ui*"));
                

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


            bundles.Add(new StyleBundle("~/Content/assets/global/plugins/TableToolsv2.2.4/css/style").Include(
                "~/Content/assets/global/plugins/TableToolsv2.2.4/css/*.css"
                ));

            bundles.Add(new ScriptBundle("~/Content/assets/global/plugins/TableToolsv2.2.4/css/script")
                .Include("~/Content/assets/global/plugins/TableToolsv2.2.4/js/*.js")
                );

            bundles.Add(new ScriptBundle("~/Content/Assets/Global/Plugins/select2/scripts").Include(
                "~/Content/Assets/Global/Plugins/select2/*.js"
                ));

            bundles.Add(new StyleBundle("~/Content/Assets/Global/Plugins/select2/css").Include(
                 "~/Content/Assets/Global/Plugins/select2/*.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Custom/customScripts").Include(
                "~/Scripts/Custom/contact.js",
                "~/Scripts/Custom/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/bootbox")
                .Include("~/Scripts/bootbox.js"));

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
            bundles.Add(new StyleBundle("~/Content/assets/global").Include(
                "~/Content/assets/global/plugins/font-awesome/css/font-awesome.min.css",
                "~/Content/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
                "~/Content/assets/global/plugins/bootstrap/css/bootstrap.min.css",
                "~/Content/assets/global/plugins/uniform/css/uniform.default.css",
                "~/Content/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css",
                "~/Content/assets/global/plugins/bootstrap-daterangepicker/daterangepicker-bs3.css",
                "~/Content/assets/global/plugins/fullcalendar/fullcalendar.min.css",
                "~/Content/assets/global/plugins/jqvmap/jqvmap/jqvmap.css",
                "~/Content/assets/admin/pages/css/tasks.css",
                "~/Content/assets/global/css/components.css",
                "~/Content/assets/global/css/plugins.css",
                "~/Content/assets/admin/layout/css/layout.css",
                "~/Content/assets/admin/layout/css/themes/darkblue.css",
                "~/Content/assets/admin/layout/css/custom.css"
                ));

            bundles.Add(new StyleBundle("~/Content/assets/admin").Include(
                "~/Content/assets/admin/layout/css/layout.css",
                "~/Content/assets/admin/layout/css/themes/darkblue.css",
                "~/Content/assets/admin/layout/css/custom.css"
                ));
        }
    }
}