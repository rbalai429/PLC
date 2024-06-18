using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Controllers
{
	public class CMSBindController : SurfaceController
	{
		private readonly ApplicationContext _context;

		public CMSBindController()
		{
		}

		public CMSBindController(ApplicationContext context)
		{
			_context = context;
		}
		public string BindCMSMode()
		{
			//var mediaData = Umbraco.Media(11114)?.DescendantsOrSelf();
			//if (mediaData != null)
			//{
			//	List<MediaReference> mediaReference = new List<MediaReference>();
			//	foreach (var media in mediaData)
			//	{
			//		mediaReference.Add(new MediaReference { Id= media.Id, Name= media?.Url(),Extention = media.ContentType.Alias});
			//	}
			//}


			//var image = Umbraco.Media(11114)?.DescendantsOrSelf().Where(x => x.ContentType.Alias == "Image").OfType<Image>();

			//string dff = "";

			var classRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>()?.Where(c => c.AgeGroup.Name == "4-5")?.FirstOrDefault()?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
									.Where(s => Umbraco?.Content(s?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == 1).FirstOrDefault().Id;

			//var subjectRoot = classRoot?.Where(x => Umbraco?.Content("umb://document/7d8b5c1bfa274c3f92d29433d2b19594")?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == 16929).FirstOrDefault();

			//var ddff = Umbraco?.Content("umb://document/7d8b5c1bfa274c3f92d29433d2b19594")?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue;

			var content = Services.ContentService.GetById(7247);

			content.SetValue("title", "Hello India", "en-US");
			content.SetValue("subTitle", "Hello India Sub", "en-US");
			content.SetValue("subTitle", "Hello India Sub", "en-US");
			//content.SetValue("selectAgeGroup", "/maters/age-master/4-5/", "en-US");

			// Get the content you want to assign to the footer links property 
			var contentPage = Umbraco.Content("e57ead40-c4f9-4ebd-81a8-58e4d0d4b52d");

			// Create an Udi of the Content
			var contentPageUdi = Udi.Create(Constants.UdiEntityType.Document, contentPage.Key);


			var externalLink = new List<Link>
			{
				// External Link
				new Link
				{
					Target = "_self",
					Name = contentPage.Name,
					Url = contentPage.Url,
					Type = LinkType.Content,
					Udi = contentPageUdi
				},
				// Media 
				//new Link
				//{
				//    Target = "_self",
				//    Name = media.Name,
				//    Url = media.MediaUrl(),
				//    Type = LinkType.Media,
				//    Udi = mediaUdi
				//}, 
				// Content 
				//new Link
				//{
				//    Target = "_self",
				//    Name = contentPage.Name,
				//    Url = contentPage.Url(),
				//    Type = LinkType.Content,
				//    Udi = contentPageUdi
				//}
			};

			// Serialize the list with links to JSON
			var links = JsonConvert.SerializeObject(externalLink);

			//SubscriptionBinding
			string subsLinks = "1,2,3";
			if (!String.IsNullOrWhiteSpace(subsLinks))
			{
				List<string> subscriptionId = subsLinks.Split(',').ToList();

				if (subscriptionId != null)
				{
					var subscriptionLink = new List<Link>();
					foreach (var slinks in subscriptionId)
					{
						if (slinks != null)
						{
							// Get the content you want to assign to the footer links property 
							var subscriptionDtls = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "subscriptionList")?.FirstOrDefault()?.DescendantsOrSelf()?.OfType<Subscriptions>()?
									.Where(x => x.IsActive == true && x.Ranking == slinks)?.FirstOrDefault();

							// Create an Udi of the Content
							var subscriptionDtlsUdi = Udi.Create(Constants.UdiEntityType.Document, subscriptionDtls.Key);

							subscriptionLink.Add(new Link { Target = "_self", Name = subscriptionDtls.Name, Url = subscriptionDtls.Url(), Type = LinkType.Content, Udi = subscriptionDtlsUdi });
						}
					}

					// Serialize the list with links to JSON
					var linksubs = JsonConvert.SerializeObject(subscriptionLink);
					// Set the value of the property with alias 'footerLinks'. 
					content.SetValue("subscription", linksubs, "en-US");
				}
			}

			//content.SetValue("desktopImage", 5828, "en-US");
			//content.SetValue("desktopNextGenImage", 12690, "en-US");
			//content.SetValue("uploadPDF", "/media/ozbkxsly/brain-games-4.pdf", "en-US");


			// Get the media you want to assign to the footer links property 
			//var media = Umbraco.Media("710fcc4a-861f-4eb0-8bad-82a132bd2961");

			// Create an Udi of the media
			//var mediaUdi = Udi.Create(Constants.UdiEntityType.Media, media.Key);

			//var mediaFile = new List<Media>
			//{
			//	new Media{

			//	}
			//};

			//var mediaFileUpdate = JsonConvert.SerializeObject(mediaFile);
			//content.SetValue("desktopImage", mediaFileUpdate, "en-US");

			//string fileName = "myfiles";
			//IMediaService _mediaService = Current.Services.MediaService;
			//var contentTypeBaseServiceProvider = Current.Services.ContentTypeBaseServices;
			//IMedia newFile = _mediaService.CreateMedia(fileName, -1, "Image");

			//string filePath = Server.MapPath("https://www.printlearncenter.com/media/xvhj12za/diwali-banner.jpeg");//"https://www.printlearncenter.com/media/xvhj12za/diwali-banner.jpeg";
			//using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open))
			//{
			//	content.SetValue(contentTypeBaseServiceProvider, "desktopImage", fileName, stream, "en-US");
			//}
			//_mediaService.Save(newFile);
			//content.SetValue("desktopImage", "https://www.printlearncenter.com/media/xvhj12za/diwali-banner.jpeg", "en-US");

			// Serialize the list with links to JSON
			//var mediaFileUpdate = JsonConvert.SerializeObject(mediaFile);


			// Set the value of the property with alias 'footerLinks'. 
			//content.SetValue("desktopImage", mediaFileUpdate, "en-US");

			Services.ContentService.SaveAndPublish(content, "en-US");

			return "";
		}
	}
}


public class MediaReference
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Extention { get; set; }
}