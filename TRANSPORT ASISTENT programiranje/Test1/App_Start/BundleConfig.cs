using System.Web;
using System.Web.Optimization;

namespace DDtrafic
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax*",
                        "~/Scripts/moment*",
                        "~/Content/bootstrap/js/jquery-ui.js",
                        "~/Content/daterange/jquery.daterangepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryDatePickerForm").Include(

                        "~/Content/bootstrap/js/jquery.ui.timepicker.js",
                        "~/Content/bootstrap/js/bootstrap-datepicker.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/tableConfig").Include(
                        "~/Content/bootstrap/js/tableCustomConfig.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
                  
            //          "~/Content/site.css"));



            bundles.Add(new ScriptBundle("~/Content/appJS").Include(
                       "~/Content/bootstrap/js/bootstrap.min.js",
                       "~/Content/bootstrap/js/bootstrap-table.js",
                       "~/Content/bootstrap/js/bootstrap-table-filter-control.js",
                       "~/Content/bootstrap/locale/bootstrap-table-sr-SR.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/appStyle").Include(
                      "~/Content/site.css",
                      "~/Content/bootstrap/css/bootstrap.css",
                      "~/Content/bootstrap/css/bootstrap-table.css",
                      "~/Content/bootstrap/css/bootstrap-datepicker.css",
                      "~/Content/jquery.ui.timepicker.css",
                       "~/Content/jquery-ui.css",
                       "~/Content/autocomplete.css",
                      "~/Content/daterangepicker.css"
                      ));
        }
    }
}
