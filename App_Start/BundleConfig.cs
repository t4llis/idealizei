using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            /*
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/vendors/jquery/dist/jquery.min.js",
                      "~/Content/vendors/bootstrap/dist/js/bootstrap.bundle.min.js",
                      "~/Content/vendors/fastclick/lib/fastclick.js",
                      "~/Content/vendors/nprogress/nprogress.js",
                      "~/Content/vendors/Chart.js/dist/Chart.min.js",
                      "~/Content/vendors/gauge.js/dist/gauge.min.js",
                      "~/Content/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js",
                      "~/Content/vendors/iCheck/icheck.min.js",
                      "~/Content/vendors/skycons/skycons.js",
                      "~/Content/vendors/Flot/jquery.flot.js",
                      "~/Content/vendors/Flot/jquery.flot.pie.js",
                      "~/Content/vendors/Flot/jquery.flot.time.js",
                      "~/Content/vendors/Flot/jquery.flot.stack.js",
                      "~/Content/vendors/Flot/jquery.flot.resize.js",
                      "~/Content/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                      "~/Content/vendors/flot-spline/js/jquery.flot.spline.min.js",
                      "~/Content/vendors/flot.curvedlines/curvedLines.js",
                      "~/Content/vendors/DateJS/build/date.js",
                      "~/Content/vendors/jqvmap/dist/jquery.vmap.js",
                      "~/Content/vendors/jqvmap/dist/maps/jquery.vmap.world.js",
                      "~/Content/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js",
                      "~/Content/vendors/moment/min/moment.min.js",
                      "~/Content/vendors/bootstrap-daterangepicker/daterangepicker.js",
                      "~/Content/js/grow/jquery.growl.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/vendors/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/vendors/font-awesome/css/font-awesome.min.css",
                      "~/Content/vendors/nprogress/nprogress.css",
                      "~/Content/vendors/iCheck/skins/flat/green.css",
                      "~/Content/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
                      "~/Content/vendors/jqvmap/dist/jqvmap.min.css",
                      "~/Content/vendors/bootstrap-daterangepicker/daterangepicker.css",
                      "~/Content/css/grow/jquery.growl.css",
                      "~/Content/build/css/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/Custom-js").Include(
                      "~/Content/build/js/custom.js",
                      "~/Content/custom/js/Facebook.js",
                      "~/Content/custom/js/Ideia.js"));

            bundles.Add(new StyleBundle("~/Content/custom-css").Include(
                      "~/Content/custom/css/Ideia.css"));
            */

            BundleTable.EnableOptimizations = true;
        }
    }
}
