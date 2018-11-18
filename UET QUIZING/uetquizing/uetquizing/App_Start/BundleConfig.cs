using System.Web;
using System.Web.Optimization;

namespace uetquizing
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
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/ContentTeacher/css").Include(
                      "~/assets/fonts/feather/feather.min.css",
                      "~/assets/libs/highlight/styles/vs2015.min.css",
                      "~/assets/libs/quill/dist/quill.core.css",
                      "~/assets/libs/select2/dist/css/select2.min.css",
                      "~/assets/libs/flatpickr/dist/flatpickr.min.css",
                      "~/assets/css/theme-dark.min.css",
                      "~/assets/css/theme.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/teacherScripts").Include(
                      "~/assets/libs/bootstrap/dist/js/bootstrap.bundle.min.js",
                      "~/assets/libs/chart.js/dist/Chart.min.js",
                      "~/assets/libs/chart.js/Chart.extension.min.js",
                      "~/assets/libs/highlight/highlight.pack.min.js",
                      "~/assets/libs/flatpickr/dist/flatpickr.min.js",
                      "~/assets/libs/jquery-mask-plugin/dist/jquery.mask.min.js",
                      "~/assets/libs/list.js/dist/list.min.js",
                      "~/assets/libs/quill/dist/quill.min.js",
                      "~/assets/libs/dropzone/dist/min/dropzone.min.js",
                      "~/assets/libs/select2/dist/js/select2.min.js",
                      "~/assets/js/theme.min.js"

                      ));
        }
    }
}
