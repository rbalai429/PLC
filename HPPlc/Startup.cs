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
using System.Web.Routing;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using HPPlc.Models;
using Umbraco.Core;
using Umbraco.Web.Routing;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using HPPlc.Models.UrlProvider;
using Umbraco.Core.Events;
using Umbraco.Core.Services.Implement;
using Umbraco.Core.Models;
using Amazon.S3;
using Amazon.Runtime;
using System.Text;
using Amazon.S3.Transfer;
using System.Linq;
using Amazon.S3.Model;
using HPPlc.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Umbraco.Web.Editors;
using Umbraco.Web.Models.ContentEditing;
using System.Web.Http.Filters;

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

			
			//app.UseCors();
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
			//scheduleViaStartUpFile = new ScheduleViaStartUpFile(context);
			scheduleViaStartUpFile = new ScheduleViaStartUpFile(context, HttpContext.Current);

			if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/ExcelFile/")))
			{
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/ExcelFile/"));
			}
			if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/ExcelFile/SFMCExtractReport/")))
			{
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/ExcelFile/SFMCExtractReport/"));
			}
		}

		public void Initialize()
		{
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			GlobalConfiguration.Configuration.MapHttpAttributeRoutes();
			GlobalConfiguration.Configuration.Initializer(GlobalConfiguration.Configuration);

			EditorModelEventManager.SendingContentModel += EditorModelEventManager_SendingContentModel;

			string IsSchedularActive = ConfigurationManager.AppSettings["IsSchedularActive"].ToString();

			if (IsSchedularActive == "Y")
			{
				Task.Run(() =>
				{
					scheduleViaStartUpFile.CallScheduler(LocalSaveFilePath);
				});
				//timer for 24 hours.
			}
			
			//HomeController home = new HomeController();
			//home.BindCMS();
			//scheduleViaStartUpFile.BindCMS();

			//ContentService c = new ContentService();
			//Umbraco.Core.Models.IContent myContent = IContentService().GetById(1054);
			//UmbracoApplicationBase.ApplicationInit += UmbracoApplication_ApplicationInit;

			// subscribe to content service published event
			//ContentService.Saving += this.ContentService_Saving;
		}

		//private void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> e)
		//{
		//	// the custom code to fire everytime content is published goes here!
		//	foreach (var content in e.SavedEntities
		//		.Where(c => (c.ContentType.Alias.InvariantEquals("worksheetRoot") || c.ContentType.Alias.InvariantEquals("structureProgramItems"))))
		//	{
		//		if (content != null)//Check if it is a new item
		//		{
		//			var WorksheetId = content.Id;
		//			var ClassName = content.GetValue("selectAgeGroup", "en-US");
		//			//if (content.HasProperty("uploadPDF"))
		//			//{
		//			//	var filename = content.GetValue("uploadPDF", "en-us");

		//			//	if (filename != null)
		//			//	{
		//			//		string fileExtention = Path.GetExtension(filename.ToString());

		//			//		if (fileExtention.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase))
		//			//		{
		//			//			string[] lengthOfExt = filename.ToString().Split('.');
		//			//			string mimeType = MimeMapping.GetMimeMapping(filename.ToString());
		//			//			if (String.IsNullOrWhiteSpace(mimeType))
		//			//			{
		//			//				e.Cancel = true;
		//			//				e.Messages.Add(new EventMessage("Cancel", "File is not in correct format.", EventMessageType.Error));
		//			//			}
		//			//			else if (mimeType.ToLower() != "application/pdf")
		//			//			{
		//			//				e.Cancel = true;
		//			//				e.Messages.Add(new EventMessage("Cancel", "File is not in correct format.", EventMessageType.Error));
		//			//			}
		//			//			else if (lengthOfExt.Length <= 1 || lengthOfExt.Length > 2)
		//			//			{
		//			//				e.Cancel = true;
		//			//				e.Messages.Add(new EventMessage("Cancel", "File is not in correct format.", EventMessageType.Error));
		//			//			}
		//			//			else
		//			//			{
		//			//				try
		//			//				{
		//			//					string CreateFolderUsingCurrentClassSubject = "media/Nursary/Maths/";
		//			//					string fullFileName = String.Empty;
		//			//					string[] temp = filename.ToString().Split('/');
		//			//					string fileName = temp[temp.Length - 1];
		//			//					//fileName = AntiXssEncoder.HtmlEncode(fileName, true);
		//			//					if (temp.Length >= 5)
		//			//						fullFileName = temp[3] + "/" + temp[4] + "/" + fileName;
		//			//					else if (temp.Length >= 3)
		//			//						fullFileName = "media/Nursary" + "/" + "Maths" + "/" + fileName;//fullFileName = temp[1] + "/" + temp[2] + "/" + fileName;
		//			//					else
		//			//						fullFileName = fileName;

		//			//					string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
		//			//					string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
		//			//					string BucketName = ConfigurationManager.AppSettings["BucketFileSystem:BucketName"].ToString();

		//			//					var credentials = new BasicAWSCredentials(AWSAccessKey, AWSSecretKey);
		//			//					var config = new AmazonS3Config
		//			//					{
		//			//						RegionEndpoint = Amazon.RegionEndpoint.APSouth1
		//			//					};


		//			//					var client = new AmazonS3Client(credentials, config);
		//			//					var fileTransferUtility = new TransferUtility(client);

		//			//					ListObjectsRequest listObject = new ListObjectsRequest
		//			//					{
		//			//						BucketName = BucketName,
		//			//						Prefix = fullFileName
		//			//					};
		//			//					var folderExists = client.ListObjects(listObject);
		//			//					foreach (S3Object obj in folderExists.S3Objects)
		//			//					{
		//			//						Console.WriteLine(obj.Key);
		//			//					}
		//			//					if (folderExists != null && folderExists.S3Objects != null && folderExists.S3Objects.Count == 0)
		//			//					{
		//			//						PutObjectRequest putRequest = new PutObjectRequest
		//			//						{
		//			//							BucketName = BucketName,
		//			//							Key = fullFileName
		//			//						};

		//			//						//sender.SaveAndPublish(putRequest);
		//			//						PutObjectResponse response1 = client.PutObject(putRequest);


		//			//						//content.SetValue("uploadPreviewPDF", fullFileName);

		//			//					}
		//			//					//var objectResponse = fileTransferUtility.S3Client.GetObject(new GetObjectRequest()
		//			//					//{
		//			//					//	BucketName = BucketName,
		//			//					//	Key = fullFileName
		//			//					//});

		//			//					//string bucketFileContentType = objectResponse.Headers.ContentType;
		//			//					//if (bucketFileContentType.ToLower() != "application/pdf")
		//			//					//{
		//			//					//	e.Cancel = true;
		//			//					//	e.Messages.Add(new EventMessage("Cancel", "File is not in correct format.", EventMessageType.Error));
		//			//					//}


		//			//					//byte[] buffer = null;
		//			//					//Stream fs = objectResponse.ResponseStream;
		//			//					//BinaryReader br = new BinaryReader(fs);
		//			//					//buffer = br.ReadBytes(5);

		//			//					//var enc = new ASCIIEncoding();
		//			//					//var header = enc.GetString(buffer);

		//			//					//if (buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46)
		//			//					//{
		//			//					//if (!header.StartsWith("%PDF-"))
		//			//					//{
		//			//					//	e.Cancel = true;
		//			//					//	e.Messages.Add(new EventMessage("Cancel", "File is not in correct format.", EventMessageType.Error));
		//			//					//}
		//			//					//}
		//			//				}
		//			//				catch (Exception ex)
		//			//				{
		//			//					e.Cancel = true;
		//			//					e.Messages.Add(new EventMessage("Cancel", "File is not in correct format.", EventMessageType.Error));
		//			//				}
		//			//			}
		//			//		}
		//			//		else
		//			//		{
		//			//			e.Cancel = true;
		//			//			e.Messages.Add(new EventMessage("Cancel", "File is not in correct format.", EventMessageType.Error));
		//			//		}
		//			//	}
		//			//}
		//		}
		//	}
		//}

		//private void UmbracoApplication_ApplicationInit(object sender, EventArgs e)
		//{
		//	string myMainCulture = "en-US";
		//	//var x = Current.Services.ContentTypeService.GetAllContentTypeAliases();
		//	//scheduleViaStartUpFile.BindCMS();
		//	//System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
		//	//System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
		//	//var content = Current.Services.ContentService.GetById(1067);
		//	//content.SetValue("title", "Hello India", myMainCulture);
		//	//Current.Services.ContentService.SaveAndPublish(content, myMainCulture);
		//}

		private void EditorModelEventManager_SendingContentModel(HttpActionExecutedContext sender, EditorModelEventArgs<ContentItemDisplay> e)
		{
			//Restrict visibility here
			string[] tabAlias = { "SEO","FOOTER CONTENT DATA" };
			string userGroups = "WRITERS";

			IList<string> _currentUserGroupsAliasses =
								 e.UmbracoContext.Security.CurrentUser?.Groups?.Select(userGroup => userGroup.Alias).ToList();

			if (_currentUserGroupsAliasses != null && _currentUserGroupsAliasses.Any(cug => cug.ToUpper() == userGroups))
			{
				foreach (var variant in e.Model.Variants)
				{
					var tabsToHide = variant.Tabs.Where(t => !t.Alias.Equals(tabAlias));

					foreach (var tab in tabsToHide)
					{
						if (!tabAlias.Any(x => x.ToUpper() == tab.Alias.ToString().ToUpper()))
						{
							foreach (var propety in tab.Properties)
							{
								propety.HideLabel = true;
								propety.View = "/App_Plugins/CustomPropertyEditorViews/PropertyWithoutAccess.html";
							}
						}
					}
				}
			}

			//foreach (var tab in contentItemDisplay?.Variants?.FirstOrDefault()?.Tabs?.ToList())
			//{
			//    if (tab.Alias != tabAlias)
			//    {
			//        //tab.Properties = tab.Properties.Where(x => x.Alias != tabAlias);
			//        tab.Label = "";
			//    }
			//}

			//         if (_currentUserGroupsAliasses != null && !_currentUserGroupsAliasses.Any(cug => cug == userGroups))
			//{
			//	if ((contentItemDisplay?.Variants?.FirstOrDefault()?.Tabs?.Any()).GetValueOrDefault(false))
			//	{
			//contentItemDisplay.Variants.FirstOrDefault().Tabs =
			//	contentItemDisplay.Variants.FirstOrDefault().Tabs.Where(x => x.Alias.Equals(tabAlias));
			//	}
			//}

			//ContentItemDisplay contentItemDisplay = e.Model;
			//string tabAliass = "SEO";
			//foreach (var variant in e.Model.Variants)
			//{
			//	var tabsToHide = variant.Tabs.Where(t => !t.Alias.Contains(tabAliass));
			//	foreach (var tab in tabsToHide)
			//	{
			//		tab.Label = "";

			//		//foreach (var propety in tab.Properties)
			//		//{
			//		//	propety.HideLabel = true;
			//		//}
			//	}
			//}
		}

		public void Terminate()
		{
			EditorModelEventManager.SendingContentModel -= EditorModelEventManager_SendingContentModel;
			//ContentService.Saving -= this.ContentService_Saving;
		}
	}
}