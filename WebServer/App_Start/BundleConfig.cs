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
                         //"~/yonetim/plugins/datepicker/bootstrap-datepicker.js",
                         "~/assets/lib/datetimepicker/jquery.datetimepicker.full.min.js",
                         "~/yonetim/plugins/daterangepicker/daterangepicker.js",
                         //"~/yonetim/plugins/datepicker/locales/bootstrap-datepicker.tr.js",
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
                      //"~/yonetim/plugins/datepicker/datepicker3.css",
                      "~/assets/css/jquery-ui.css",
                      "~/yonetim/plugins/daterangepicker/daterangepicker.css",
                      "~/Content/bootstrap-datetimepicker-build.less"
                      ));

            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            
            bundles.Add(new StyleBundle("~/bundles/yon/css").Include(
                      "~/yonetim/bootstrap/css/bootstrap.min.css",
                      "~/yonetim/plugins/font-awesome/css/font-awesome.css",
                      "~/assets/css/ionicons.min.css",
                      "~/yonetim/dist/css/AdminLTE.min.css",
                      "~/yonetim/dist/css/skins/_all-skins.min.css",
                      "~/yonetim/plugins/iCheck/flat/blue.css",
                      "~/yonetim/plugins/morris/morris.css",
                      "~/yonetim/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
                      "~/yonetim/plugins/datepicker/datepicker3.css",
                      "~/yonetim/plugins/daterangepicker/daterangepicker.css",
                      "~/yonetim/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                      "~/yonetim/plugins/select2/select2.min.css",
                      "~/assets/css/buttons.dataTables.min.css"
                      ));
            try
            {
                bundles.Add(new ScriptBundle("~/bundles/yon/js").Include(
                      "~\\yonetim/plugins/jQuery/jquery-2.2.3.min.js",
                      "~\\Scripts/jquery-ui.min.js",
                      "~\\yonetim/lib/bootstrap/js/bootstrap.min.js",
                      "~\\yonetim/plugins/raphael/raphael-min.js",
                      "~\\yonetim/plugins/morris/morris.js",
                      "~\\yonetim/plugins/sparkline/jquery.sparkline.min.js",
                      "~\\yonetim/plugins/datatables/jquery.dataTables.min.js",
                      "~\\yonetim/plugins/datatables/dataTables.bootstrap.min.js",
                      "~\\yonetim/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~\\yonetim/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                      "~\\yonetim/plugins/knob/jquery.knob.js",
                      "~\\yonetim/plugins/moment/moment.min.js",
                      "~\\yonetim/plugins/daterangepicker/daterangepicker.js",
                      "~\\yonetim/plugins/datepicker/bootstrap-datepicker.js",
                      "~\\yonetim/plugins/datepicker/locales/bootstrap-datepicker.tr.js",
                      "~\\yonetim/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                      "~\\yonetim/plugins/slimScroll/jquery.slimscroll.min.js",
                      "~\\yonetim/plugins/fastclick/fastclick.js",
                      "~\\yonetim/dist/js/pages/dashboard2.js",
                      "~\\assets/js/jquery.pulsate.min.js",
                      "~\\yonetim/dist/js/app.min.js",
                      "~\\yonetim/dist/js/demo.js"
                      ));
            }
            catch
            {

            }
            
        }
    }
}
