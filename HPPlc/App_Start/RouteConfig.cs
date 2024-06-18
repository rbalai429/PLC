using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HPPlc
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.Ignore("common/css/plc/{*catch}");
			//routes.MapMvcAttributeRoutes();	
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
			"download",
			"DownloadData/{action}/{id}",
			new
			{
				controller = "WorkSheet",
				action = "DownloadData",
				id = UrlParameter.Optional
			});
		}
	}
}
