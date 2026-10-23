using System.Web;
using System.Web.Optimization;

namespace iRestaurant
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            try //~/bundles/as/js
            {
                bundles.Add(new ScriptBundle("~/bundles/as/js").Include(
                         "~/assets/lib/jquery.js",
                         "~/Scripts/jquery-ui.min.js",
                         "~/assets/lib/bootstrap/js/bootstrap.min.js",
                         //"~/assets/lib/bootstrap/js/npm.js",
                         "~/assets/lib/metismenu/metisMenu.js",
                         "~/assets/lib/onoffcanvas/onoffcanvas.js",
                         "~/assets/lib/screenfull/screenfull.js",
                         "~/assets/lib/input-mask/jquery.inputmask.js",
                         "~/assets/lib/input-mask/jquery.inputmask.date.extensions.js",
                         "~/assets/lib/input-mask/jquery.inputmask.extensions.js",
                         "~/assets/lib/moment/moment-with-locales.min.js",
                         //"~/adminlte/plugins/datepicker/bootstrap-datepicker.js",
                         "~/assets/lib/datetimepicker/jquery.datetimepicker.full.min.js",
                         "~/adminlte/plugins/daterangepicker/daterangepicker.js",
                         //"~/adminlte/plugins/datepicker/locales/bootstrap-datepicker.tr.js",
                         "~/assets/js/core.js",
                         "~/assets/js/app.js",
                         "~/assets/js/jquery.pulsate.min.js",
                         "~/Scripts/less.js"
                         ));

            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
            }

            try //~/bundles/as/css
            {
                bundles.Add(new StyleBundle("~/bundles/as/css").Include(
                      //"~/assets/lib/bootstrap/css/bootstrap.css",
                      "~/assets/lib/font-awesome/css/font-awesome.css",
                      //"~/assets/css/main.css",
                      "~/assets/lib/metismenu/metisMenu.css",
                      "~/assets/lib/onoffcanvas/onoffcanvas.css",
                      "~/assets/lib/animate.css/animate.css",
                      "~/assets/less/theme.less",
                      "~/assets/lib/datetimepicker/jquery.datetimepicker.min.css",
                      //"~/adminlte/plugins/datepicker/datepicker3.css",
                      "~/assets/css/jquery-ui.css",
                      "~/adminlte/plugins/daterangepicker/daterangepicker.css",
                      "~/Content/bootstrap-datetimepicker-build.less"
                      ));

            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            
            bundles.Add(new StyleBundle("~/bundles/yon/css").Include(
                      "~/adminlte/bootstrap/css/bootstrap.min.css",
                      "~/adminlte/plugins/font-awesome/css/font-awesome.css",
                      "~/assets/css/ionicons.min.css",
                      "~/adminlte/dist/css/AdminLTE.min.css",
                      "~/adminlte/dist/css/skins/_all-skins.min.css",
                      "~/adminlte/plugins/iCheck/flat/blue.css",
                      "~/adminlte/plugins/morris/morris.css",
                      "~/adminlte/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
                      "~/adminlte/plugins/datepicker/datepicker3.css",
                      "~/adminlte/plugins/daterangepicker/daterangepicker.css",
                      "~/adminlte/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                      "~/adminlte/plugins/select2/select2.min.css",
                      "~/assets/css/buttons.dataTables.min.css"
                      ));
            try
            {
                bundles.Add(new ScriptBundle("~/bundles/yon/js").Include(
                      "~\\adminlte/plugins/jQuery/jquery-2.2.3.min.js",
                      "~\\Scripts/jquery-ui.min.js",
                      "~\\adminlte/lib/bootstrap/js/bootstrap.min.js",
                      "~\\adminlte/plugins/raphael/raphael-min.js",
                      "~\\adminlte/plugins/morris/morris.js",
                      "~\\adminlte/plugins/sparkline/jquery.sparkline.min.js",
                      "~\\adminlte/plugins/datatables/jquery.dataTables.min.js",
                      "~\\adminlte/plugins/datatables/dataTables.bootstrap.min.js",
                      "~\\adminlte/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~\\adminlte/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                      "~\\adminlte/plugins/knob/jquery.knob.js",
                      "~\\adminlte/plugins/moment/moment.min.js",
                      "~\\adminlte/plugins/daterangepicker/daterangepicker.js",
                      "~\\adminlte/plugins/datepicker/bootstrap-datepicker.js",
                      "~\\adminlte/plugins/datepicker/locales/bootstrap-datepicker.tr.js",
                      "~\\adminlte/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                      "~\\adminlte/plugins/slimScroll/jquery.slimscroll.min.js",
                      "~\\adminlte/plugins/fastclick/fastclick.js",
                      "~\\adminlte/dist/js/pages/dashboard2.js",
                      "~\\assets/js/jquery.pulsate.min.js",
                      "~\\adminlte/dist/js/app.min.js",
                      "~\\adminlte/dist/js/demo.js"
                      ));
            }
            catch
            {

            }
            
        }
    }
}
