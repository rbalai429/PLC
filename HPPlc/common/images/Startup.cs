using HPPlc.Models.Scheduler;
using Microsoft.AspNetCore.Builder;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Composing;
using Umbraco.Core.Services;
using Umbraco.Web;
using System;
using HPPlc.Models.Bot;
using System.Web.Http;
using HPPlc.App_Start;
using System.Web.Optimization;
using Microsoft.Extensions.DependencyInjection;
using WebEssentials.AspNetCore.Pwa;
using System.IO;

namespace HPPlc
{
	public class ApplicationComposer : ComponentComposer<ApplicationComponent>, IUserComposer
	{
		public void Configure(IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
				context.Response.Headers.Add("X-Frame-Options", "sameorigin");
				await next();
			});
		}

		public override void Compose(Composition composition)
		{
			// ApplicationStarting event in V7: add IContentFinders, register custom services and more here

			base.Compose(composition);
		}
	}
	public class ApplicationComponent : IComponent
	{
		// private readonly ProductCreatePageContract productCreatePageContract;
		private static ServiceContext services;
		private ScheduleViaStartUpFile scheduleViaStartUpFile;
		string LocalSaveFilePath = HttpContext.Current.Server.MapPath("~/ExcelFile/").ToString();
		public ApplicationComponent(IContentService contentService, IUmbracoContextFactory context)
		{
			scheduleViaStartUpFile = new ScheduleViaStartUpFile(context, HttpContext.Current);
			if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/SFMCExtractReport/")))
			{
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/SFMCExtractReport/"));
			}
			if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/ExcelFile/")))
			{
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/ExcelFile/"));
			}
		}
		public void Initialize()
		{
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			GlobalConfiguration.Configuration.MapHttpAttributeRoutes();
			GlobalConfiguration.Configuration.Initializer(GlobalConfiguration.Configuration);
			
			string IsSchedularActive = ConfigurationManager.AppSettings["IsSchedularActive"].ToString();

			if (IsSchedularActive == "Y")
			{
				Task.Run(() =>
				{
					
					scheduleViaStartUpFile.CallScheduler(LocalSaveFilePath);
				});
				//timer for 24 hours.
			}
		}

		
		public void Terminate()
		{
		}
	}
}