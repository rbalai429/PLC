using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HPPlc
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			Session.Timeout = 500;
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			
			//FileSystemVirtualPathProvider.ConfigureMedia();
		}

		

	}
}
