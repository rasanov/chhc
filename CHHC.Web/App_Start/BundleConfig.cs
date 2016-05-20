using System.Web.Optimization;

namespace CHHC.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.1.js",
                        "~/Scripts/jquery.dataTables.js",
                        "~/Scripts/jquery.steps.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/main_20131004.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.11.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-confirmation").Include(
                        "~/Scripts/bootstrap-confirmation.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/pirobox").Include(
                        "~/Scripts/pirobox_extended-1.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/quickselect").Include(
                        "~/Scripts/quicksilver.js",
                        "~/Scripts/jquery.quickselect.js"));




            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                        "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap-override.css",
                        "~/Content/style.css",
                        "~/Content/fontello.css",
                        "~/Content/jquery.dataTables.css",
                        "~/Content/jquery.steps.css",
                        "~/Content/responsive.css"));

            bundles.Add(new StyleBundle("~/Content/home").Include(
                        "~/Content/home.css"));

            bundles.Add(new StyleBundle("~/Content/pirobox").Include(
                        "~/Content/pirobox/css.css"));

            bundles.Add(new StyleBundle("~/Content/quickselect").Include(
                        "~/Content/jquery.quickselect.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
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
        }
    }
}