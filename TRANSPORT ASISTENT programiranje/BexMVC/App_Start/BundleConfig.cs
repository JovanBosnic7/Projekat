using System.Web;
using System.Web.Optimization;

namespace BexMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //ovo se ucitava pri _Layout-u
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(

                        "~/Scripts/jquery-{version}.js"
                      
                        )); //,"~/Scripts/jquery-ui-1.12.1*"

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax*",
                        "~/Scripts/moment*",
                        //"~/Scripts/fancy-box/jquery.fancybox.js",
                        "~/Content/bootstrap/js/jquery-ui.js",
                        "~/Content/daterange/jquery.daterangepicker.min.js"

                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryDatePickerForm").Include(
                       
                        "~/Content/bootstrap/js/jquery.ui.timepicker.js",
                        "~/Content/bootstrap/js/bootstrap-datepicker.js"
                        ));


            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                        "~/Content/morrisjs/morris.min.js", //chart
                       "~/Content/morrisjs/chartEvidentiraneCene.js",
                       "~/Content/morrisjs/chartObrisane.js",
                       "~/Content/morrisjs/chartEvidentiranePoreklo.js"

                        ));

            bundles.Add(new ScriptBundle("~/bundles/chartVozniPark").Include(
                        "~/Content/morrisjs/morris.min.js", //chart
                       "~/Content/morrisjs/chartVozniPark.js"

                        ));

            bundles.Add(new ScriptBundle("~/bundles/tableConfig").Include(
                        "~/Content/bootstrap/js/tableCustomConfig.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Content/appJS").Include(
                       "~/Content/bootstrap/js/bootstrap.min.js",


                       "~/Content/bootstrap/js/bootstrap-table.js",
                       "~/Content/bootstrap/js/bootstrap-table-export.js",
                       //"~/Content/bootstrap/js/bootstrap-datepicker.js",
                       "~/Content/bootstrap/js/bootstrap-table-filter-control.js",
                       "~/Content/bootstrap/locale/bootstrap-table-sr-SR.js",

                       //"~/Content/bootstrap/js/jquery-ui.js",


                       "~/Content/material-dashboard/js/plugins/perfect-scrollbar.jquery.min.js",

                       //"~/Content/bootstrap/js/jquery.dragtable.js",
                       //"~/Content/bootstrap/js/colResizable-1.6*",
                       "~/Content/bootstrap/js/FileSaver.min.js",
                       "~/Content/bootstrap/js/xlsx.core.min.js",
                       "~/Content/bootstrap/js/jspdf.min.js",
                       "~/Content/bootstrap/js/jspdf.plugin.autotable.js",
                       "~/Content/bootstrap/js/es6-promise.auto.min.js",
                       "~/Content/bootstrap/js/html2canvas.min.js",
                       "~/Content/bootstrap/js/tableExport.js",

                       "~/Content/metisMenu/metisMenu.min.js",
                       "~/Content/raphael/raphael.min.js",

                       "~/Content/dist/js/sb-admin-2.js",
                       "~/Scripts/fancy-box/jquery.fancybox.js"
                       //,
                       //"~/Content/material-dashboard/js/material-dashboard.js"
                       //,
                       //"~/Content/material-dashboard/js/plugins/fullcalendar.js",
                       //"~/Content/material-dashboard/js/plugins/fullcalendar-locales-all.js"
                       //,
                       //"~/Content/dist/vendor/bootstrap/js/bootstrap.bundle.min.js"


                       ));

            bundles.Add(new StyleBundle("~/Content/appStyle").Include(
                     
                      "~/Content/bootstrap/css/bootstrap.css",
                      "~/Content/bootstrap/css/bootstrap-table.css",
                       "~/Content/material-dashboard/css/material-dashboard.css", // ovo je osnovni stil app
                      "~/Content/bootstrap/css/bootstrap-datepicker.css",
                      "~/Content/jquery.ui.timepicker.css",
                      //"~/Content/bootstrap/css/bootstrap-table-filter-control.css",
                      //"~/Content/bootstrap/css/dragtable.css",
                      //"~/Content/metisMenu/metisMenu.min.css",
                      "~/Scripts/fancy-box/jquery.fancybox.css",
                      "~/Content/morrisjs/morris.css", //chart
                                                       //"~/Content/dist/vendor/fontawesome-free/css/all.min.css",
                                                       //"~/Content/dist/css/font-nunito.css",
                                                       //"~/Content/dist/css/sb-admin-2.css",
                      

                      "~/Content/font-awesome/css/font-awesome.min.css",
                      "~/Content/jquery-ui.css",
                      
                      "~/Content/autocomplete.css", //typeahead
                      "~/Content/daterangepicker.css"

                      )


                      );
        }
    }
}
