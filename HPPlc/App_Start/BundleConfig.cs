using System.Web;
using System.Web.Optimization;

namespace HPPlc
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/bundle/common.css")
			.Include("~/common/css/slick.min.css"
			, "~/common/css/slick-theme.min.css"
			, "~/common/css/datepicker.min.css"
			, "~/common/fonts/fonts.css"
			, "~/common/css/Bootstrap.css"
			, "~/common/css/video-js.css"
			, "~/common/css/app.css"
			, "~/common/css/responsive.css"
			));


			bundles.Add(new ScriptBundle("~/bundles/all.js")
			.Include("~/common/js/jquery.min.js"
			, "~/common/js/slick.min.js"
			, "~/common/js/datepicker.min.js"
			, "~/common/js/lazyload.js"
			, "~/common/js/Multiselect.js"
			, "~/common/js/sweetalert.js"
			, "~/common/js/mediaCheck-min.js"
			, "~/Common/js/print.min.js"
			, "~/common/js/all-js.js"
			, "~/common/js/jquery.countdown360.js"
			, "~/MyScripts/payNow.js"
			, "~/CustumJs/Common.js"
			, "~/CustumJs/TrackingCode.js"
			//, "~/CustumJs/BannerVideo.js"
			));

			//bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
			//			"~/Scripts/jquery-{version}.js"));

			//bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
			//			"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			//bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
			//			"~/Scripts/modernizr-*"));

			//bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
			//		  "~/Scripts/bootstrap.js"));

			//bundles.Add(new StyleBundle("~/Content/css").Include(
			//		  "~/Content/bootstrap.css",
			//		  "~/Content/site.css"));


			//wrapup all css in a bundle  
			//bundles.Add(new StyleBundle("~/bundles/common")
			//.Include("~/common/css/plugincss.css")
			//.Include("~/common/css/jquery.fancybox.css")
			//.Include("~/common/css/jquery.fancybox-transitions.css")
			//.Include("~/common/css/app.css")
			//.Include("~/common/css/responsive.css"));

			//bundles.Add(new ScriptBundle("~/bundles/MyAppStartupJs")
			//.Include("/common/js/jquery.min.js",
			//"/common/js/jquery.fancybox.pack.js",
			//"/common/js/helpers/jquery.fancybox-media.js",
			//"/common/js/jquery.fancybox-transitions.js",
			//"/common/js/pluginjs.js",
			//"/common/js/lazyload.js",
			//"/common/js/mediaCheck-min.js",
			//"/common/js/all-js.js",
			//"/MyScripts/payNow.js",
			//"/CustumJs/Common.js",
			//"/CustumJs/TrackingCode.js"));

			////bundles.IgnoreList.Clear();
			BundleTable.EnableOptimizations = true;
		}
	}
}
