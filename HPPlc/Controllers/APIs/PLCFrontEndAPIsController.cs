using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;
using Umbraco.Web.PublishedModels;
using HPPlc.Controllers.APIs;
using Umbraco.Web;
using System.Configuration;
using System.Web.Http.Cors;
using Newtonsoft.Json;

namespace HPPlc.Controllers
{
	[RoutePrefix("api/frontendsource")]
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class PLCFrontEndAPIsController : UmbracoApiController
	{
		string mediaUrl = ConfigurationManager.AppSettings["BucketFileSystem:BucketHostname"].ToString();

		//[Route("headsection")]
		//[HttpPost]
		//public HttpResponseMessage HeadSection()
		//{
		//	//string culture = HPPlc.Models.CultureName.GetCultureName().Replace("/","");
		//	//string culture = "";
		//	HttpResponseMessage headerResponse = new HttpResponseMessage();
		//	HeaderData headerData = new HeaderData();
		//	List<HeaderData> ArrayheaderData = new List<HeaderData>();
		//	LinkProp linkProp = new LinkProp();
		//	MediaProp mediaProp = new MediaProp();
		//	MenusWithTitle contactMenu = new MenusWithTitle();
		//	MenusWithTitle postloginmenu = new MenusWithTitle();
		//	List<MenusWithTitle> postLoginMenuList = new List<MenusWithTitle>();
		//	DataNavigation dataNavigation = new DataNavigation();
		//	FilterationType filterationType = new FilterationType();
		//	List<NavigationData> navigationDatas = new List<NavigationData>();

		//	ClassMenu classMenu = new ClassMenu();
		//	SubjectsMenu subjectsMenu = new SubjectsMenu();
		//	Menus menus = new Menus();
		//	Response response = new Response();

		//	var header = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "commonRoot")?.FirstOrDefault()?
		//					   .Children?.Where(x => x.ContentType.Alias == "header")?.OfType<Header>().FirstOrDefault();

		//	var worksheetMenu = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
		//			.Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>().FirstOrDefault();

		//	if (worksheetMenu != null)
		//	{
		//		dataNavigation.Title = worksheetMenu.NavigationTitle;
		//		var fiterBy = worksheetMenu?.NavigationBy?.Where(x => x.Activation == true);
		//		if (fiterBy != null)
		//		{
		//			foreach (var item in fiterBy)
		//			{
		//				if (item != null)
		//				{
		//					filterationType.FilterTitle = item.FilterTitle;
		//					if (item?.FilterTag == "class")
		//					{
		//						var classes = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
		//							.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>();

		//						if (classes != null && classes.Any())
		//						{
		//							foreach (var cls in classes)
		//							{
		//								navigationDatas.Add(new NavigationData { Title = cls.Title, Navigation = cls.Url() });
		//							}
		//						}
		//					}
		//					else if (item?.FilterTag == "subject")
		//					{
		//						var subjects = className?.DescendantsOrSelf()?
		//										.Where(x => x.ContentType.Alias == "worksheetCategory")?
		//										.OfType<WorksheetCategory>();

		//						if (classes != null && classes.Any())
		//						{
		//							foreach (var cls in classes)
		//							{
		//								navigationDatas.Add(new NavigationData { Title = cls.Title, Navigation = cls.Url() });
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}

		//	if (header.Logo != null)
		//	{
		//		mediaProp = new MediaProp();

		//		mediaProp.AltText = header?.Logo?.Value<string>("altText");
		//		mediaProp.Target = "";
		//		mediaProp.Url = mediaUrl + header?.Logo?.Url();

		//		headerData.PLCLogo = mediaProp;
		//	}

		//	if (header.HPlogo != null)
		//	{
		//		mediaProp = new MediaProp();

		//		mediaProp.AltText = header?.HPlogo?.Value<string>("altText");
		//		mediaProp.Target = "";
		//		mediaProp.Url = mediaUrl + header?.HPlogo?.Url();

		//		headerData.HpLogo = mediaProp;
		//	}

		//	if (header.HeaderMenus != null)
		//	{
		//		List<LinkProp> headerMenus = new List<LinkProp>();
		//		foreach (var item in header.HeaderMenus)
		//		{
		//			linkProp = new LinkProp();
		//			linkProp.Name = item?.Name;
		//			linkProp.Target = item?.Target;
		//			linkProp.Url = item?.Url;
		//			linkProp.Udi = item?.Udi.ToString();

		//			headerMenus.Add(linkProp);
		//		}

		//		headerData.HeadMenus = headerMenus;
		//	}

		//	if (header.AfterLoggedInMenu != null)
		//	{
		//		List<LinkProp> headerMenus = new List<LinkProp>();
		//		foreach (var items in header.AfterLoggedInMenu)
		//		{
		//			postloginmenu = new MenusWithTitle();

		//			postloginmenu.title = items?.SelectSubscriptions.Name;
		//			postloginmenu.Navigation = items?.SelectSubscriptions.Url;
		//			postloginmenu.IconUrl = items?.BrowseDocument.Url();

		//			postLoginMenuList.Add(postloginmenu);
		//		}

		//		headerData.PostLoggedInMenus = postLoginMenuList;
		//	}

		//	response.StatusCode = 1;
		//	response.StatusMessage = "Done";
		//	response.Result = headerData;

		//	headerResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);

		//	return headerResponse;
		//}

		[Route("footsection")]
		[HttpPost]
		public HttpResponseMessage FootSection()
		{
			FooterData footerData = new FooterData();
			List<LinkProp> linkProp = new List<LinkProp>();
			List<MediaProp> mediaProp = new List<MediaProp>();
			Response response = new Response();
			HttpResponseMessage footerResponse = new HttpResponseMessage();

			var footer = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "commonRoot").FirstOrDefault()?
						   .Children?.Where(x => x.ContentType.Alias == "footer").OfType<Footer>().FirstOrDefault();

			if (footer != null && footer.FooterNavigation != null)
			{
				foreach (var item in footer.FooterNavigation.Where(x => x.IsActive == true))
				{
					linkProp.Add(new LinkProp { Name = item?.FooterNavigation?.Name, Url = item.FooterNavigation.Url, Target = item.FooterNavigation.Target });
				}

				footerData.FooterNavigation = linkProp;
			}

			if (footer != null && footer.PaymentSource != null)
			{
				foreach (var item in footer.PaymentSource.Where(x => x.IsActive == true))
				{
					mediaProp.Add(new MediaProp { Url = item?.MediaFile?.Url(), AltText = item.MediaFile.Value<string>("altText") });
				}

				footerData.PaymentSource = mediaProp;
			}

			if (footer != null && !String.IsNullOrWhiteSpace(footer.Copyright))
			{
				footerData.Copyright = footer.Copyright;
			}

			response.StatusCode = 1;
			response.StatusMessage = "Done";
			response.Result = footerData;


			footerResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);

			return footerResponse;
		}

		[Route("banner")]
		[HttpPost]
		public HttpResponseMessage BannerSection()
		{
			List<BannerItems> banner = new List<BannerItems>();
			BannerItems bannerItems = new BannerItems();
			MediaProp mediaProp = new MediaProp();
			LinkProp linkProp = new LinkProp();

			List<MediaProp> mediaProps = new List<MediaProp>();
			List<LinkProp> linkProps = new List<LinkProp>();
			//List<MultiTypeMediaProp> multiTypeMediaProps = new List<MultiTypeMediaProp>();

			Response response = new Response();
			HttpResponseMessage bannerResponse = new HttpResponseMessage();

			List<BannerElement> lstBanner = new List<BannerElement>();

			lstBanner = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
				.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "banner")?.OfType<Banner>()?.FirstOrDefault()?
				.BannerItems?.OfType<BannerElement>()?.Where(x => x.Activated == true).ToList();

			if (lstBanner != null && lstBanner.Any())
			{
				foreach (var BannerItem in lstBanner)
				{
					bannerItems = new BannerItems();
					if (BannerItem.DesktopImage != null)
					{
						mediaProp = new MediaProp();

						mediaProp.AltText = BannerItem.DesktopImage?.Value<string>("altText");
						mediaProp.Target = "";
						mediaProp.Url = mediaUrl + BannerItem.DesktopImage?.Url();

						bannerItems.DesktopImage = mediaProp;
					}
					else
					{
						mediaProp = new MediaProp();
						bannerItems.DesktopImage = mediaProp;
					}

					if (BannerItem.DesktopNextGenImage != null)
					{
						mediaProp = new MediaProp();

						mediaProp.AltText = BannerItem.DesktopNextGenImage?.Value<string>("altText");
						mediaProp.Target = "";
						mediaProp.Url = mediaUrl + BannerItem.DesktopNextGenImage?.Url();

						bannerItems.DesktopNextGenImage = mediaProp;
					}
					else
					{
						mediaProp = new MediaProp();
						bannerItems.DesktopNextGenImage = mediaProp;
					}

					if (BannerItem.MobileImage != null)
					{
						mediaProp = new MediaProp();

						mediaProp.AltText = BannerItem.MobileImage?.Value<string>("altText");
						mediaProp.Target = "";
						mediaProp.Url = mediaUrl + BannerItem.MobileImage?.Url();

						bannerItems.MobileImage = mediaProp;
					}
					else
					{
						mediaProp = new MediaProp();
						bannerItems.MobileImage = mediaProp;
					}

					if (BannerItem.MobileNextGenImage != null)
					{
						mediaProp = new MediaProp();

						mediaProp.AltText = BannerItem.MobileNextGenImage?.Value<string>("altText");
						mediaProp.Target = "";
						mediaProp.Url = mediaUrl + BannerItem.MobileNextGenImage?.Url();

						bannerItems.MobileNextGenImage = mediaProp;
					}
					else
					{
						mediaProp = new MediaProp();
						bannerItems.MobileNextGenImage = mediaProp;
					}

					if (BannerItem.BannerUrl != null)
					{
						linkProp = new LinkProp();

						linkProp.Name = BannerItem.BannerUrl?.Name;
						linkProp.Target = BannerItem.BannerUrl?.Target;
						linkProp.Url = BannerItem.BannerUrl?.Url;
						linkProp.Udi = BannerItem.BannerUrl?.Udi?.ToString();

						bannerItems.BannerUrl = linkProp;
					}
					else
					{
						linkProp = new LinkProp();
						bannerItems.BannerUrl = linkProp;
					}

					if (!String.IsNullOrWhiteSpace(BannerItem.IsVideo))
						bannerItems.IsVideo = BannerItem.IsVideo;
					else
						bannerItems.IsVideo = String.Empty;

					if (BannerItem.BannerAllowForGuestUser == true)
						bannerItems.BannerAllowForGuestUser = BannerItem.BannerAllowForGuestUser;
					else
						bannerItems.BannerAllowForGuestUser = false;

					banner.Add(bannerItems);
				}
			}

			response.StatusCode = 1;
			response.StatusMessage = "Done";
			response.Result = banner;
			bannerResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);

			return bannerResponse;
		}


		[Route("topsection")]
		[HttpPost]
		public HttpResponseMessage TopSection()
		{
			TopSection topSection = new TopSection();
			LinkProp linkProp = new LinkProp();
			Response response = new Response();
			HttpResponseMessage topSectionResponse = new HttpResponseMessage();

			var TopSection = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
				.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "homePageSignUpSection")?.OfType<HomePageSignUpSection>().FirstOrDefault();

			if (TopSection != null)
			{
				if (TopSection.Button1 != null)
				{
					linkProp = new LinkProp();
					linkProp.Name = TopSection?.Button1?.Name.ToString();
					linkProp.Target = TopSection?.Button1?.Target;
					linkProp.Url = TopSection?.Button1?.Url;
					linkProp.Udi = TopSection?.Button1?.Udi.ToString();

					topSection.SubscriptionButton = linkProp;

				}

				//if (TopSection.ReferralButton != null)
				//{
				//	linkProp = new LinkProp();
				//	linkProp.Name = TopSection?.ReferralButton?.Name.ToString();
				//	linkProp.Target = TopSection?.ReferralButton?.Target;
				//	linkProp.Url = TopSection?.ReferralButton?.Url;
				//	linkProp.Udi = TopSection?.ReferralButton?.Udi.ToString();

				//	topSection.ReferralButton = linkProp;
				//}

				topSection.Title = TopSection.Title.ToString();
				//topSection.ReferalTitle = TopSection.ReferralTitle;

				response.StatusCode = 1;
				response.StatusMessage = "Done";
				response.Result = topSection;
				topSectionResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
			}

			return topSectionResponse;
		}

		[Route("listofclass")]
		[HttpPost]
		public HttpResponseMessage ListOfClass()
		{
			List<ListOfClass> listOfClass = new List<ListOfClass>();
			LinkProp linkProp = new LinkProp();
			Response response = new Response();
			HttpResponseMessage listofclassResponse = new HttpResponseMessage();

			var ListofClass = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
				.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "ageMaster")?.OfType<AgeMaster>().FirstOrDefault()?
				.Children?.OfType<NameListItem>()?.Where(c => c.IsActice == true);

			if (ListofClass != null && ListofClass.Count() > 0)
			{
				foreach (var age in ListofClass)
				{
					listOfClass.Add(new APIs.ListOfClass { ItemValue = age.ItemValue, ItemName = age.ItemName });
				}

				response.StatusCode = 1;
				response.StatusMessage = "Done";
				response.Result = listOfClass;
				listofclassResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
			}

			return listofclassResponse;
		}


		[Route("listofsubject")]
		[HttpPost]
		public HttpResponseMessage ListOfSubject()
		{
			List<ListOfSubject> listOfSubjects = new List<ListOfSubject>();
			LinkProp linkProp = new LinkProp();
			Response response = new Response();
			HttpResponseMessage listofsubjectResponse = new HttpResponseMessage();

			var ListofSubjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
				.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "categoryMaster")?.OfType<CategoryMaster>()?.FirstOrDefault()?
				.Children?.OfType<NameListItem>()?.Where(c => c.IsActice == true);

			if (ListofSubjects != null && ListofSubjects.Count() > 0)
			{
				foreach (var subject in ListofSubjects)
				{
					listOfSubjects.Add(new APIs.ListOfSubject { ItemValue = subject.ItemValue, ItemName = subject.ItemName });
				}

				response.StatusCode = 1;
				response.StatusMessage = "Done";
				response.Result = listOfSubjects;
				listofsubjectResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
			}

			return listofsubjectResponse;
		}

		[Route("getworksheets")]
		[HttpPost]
		public HttpResponseMessage GetWorkSheets(string ClassValue = "", string SubjectValue = "")
		{
			var dataClass = JsonConvert.DeserializeObject<string>(ClassValue);
			var dataSubject = JsonConvert.DeserializeObject<string>(SubjectValue);

			List<WorksheetsFilterClass> worksheetsFilterClasses = new List<WorksheetsFilterClass>();
			List<WorksheetsFilterSubject> worksheetsFilterSubjects = new List<WorksheetsFilterSubject>();

			if (!String.IsNullOrWhiteSpace(dataClass))
			{
				foreach (string cls in dataClass.Split(','))
				{
					worksheetsFilterClasses.Add(new WorksheetsFilterClass { ClassValue = cls.ToString() });
				}
			}

			if (!String.IsNullOrWhiteSpace(dataSubject))
			{
				foreach (string sub in dataSubject.Split(','))
				{
					worksheetsFilterSubjects.Add(new WorksheetsFilterSubject { SubjectValue = sub.ToString() });
				}
			}
			WorksheetsClass worksheetsRoot = new WorksheetsClass();
			List<WorksheetsClass> worksheetsMain = new List<WorksheetsClass>();
			List<Worksheets> worksheets = new List<Worksheets>();

			MediaProp mediaProp = new MediaProp();
			LinkProp linkProp = new LinkProp();
			Response response = new Response();
			HttpResponseMessage worksheetResponse = new HttpResponseMessage();
			List<WorksheetListingAgeWise> ClassItems = null;
			List<WorksheetRoot> worksheetsData = null;

			var firstTimeDisplayWorksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
				.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()?.FirstOrDefault()?.FirstTimeDisplayWorksheet; ;

			if (worksheetsFilterClasses != null && worksheetsFilterClasses.Count > 0)
			{
				ClassItems = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
					.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
					.Where(s => worksheetsFilterClasses.Any(c => Umbraco?.Content(s?.AgeGroup?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue.ToString() == c.ClassValue.ToString())).ToList();
			}
			else
			{
				ClassItems = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
					.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>().ToList();
			}

			if (ClassItems != null && ClassItems.Count > 0)
			{
				foreach (var item in ClassItems)
				{
					worksheetsRoot = new WorksheetsClass();
					worksheetsRoot.ClassName = item.Name;
					worksheetsRoot.AgeGroup = item.AgeGroup.Name;

					worksheetsRoot.Title = item.Title;
					worksheetsRoot.Description = item.Description.ToString();

					//if (worksheetsFilterSubjects != null && worksheetsFilterSubjects.Count > 0)
					//{
					//	worksheetsData = item?.Children?.Where(x => x.ContentType.Alias == "worksheetRoot")?
					//	.OfType<WorksheetRoot>()?.Where(c => c.IsActive == true)?
					//	.Where(s => worksheetsFilterSubjects.Any(c => Umbraco?.Content(s?.Category?.FirstOrDefault()?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue.ToString() == c?.SubjectValue.ToString()))
					//	.Take(firstTimeDisplayWorksheet.Value).ToList();
					//}
					//else
					//{
					//	worksheetsData = item?.Children?.Where(x => x.ContentType.Alias == "worksheetRoot")?
					//	.OfType<WorksheetRoot>()?.Where(c => c.IsActive == true).Take(firstTimeDisplayWorksheet.Value).ToList();
					//}

					//if (worksheetsData != null)
					//{
					//	foreach (var wk in worksheetsData)
					//	{
					//		Worksheets wkSingle = new Worksheets();

					//		wkSingle.Title = wk?.Title;
					//		wkSingle.Description = wk?.Description.ToString();

					//		wkSingle.SubjectName = string.Join(",", wk?.Category?.Select(x => x.Name).ToList());
					//		wkSingle.WeekName = wk?.SelectVolume?.Name;

					//		wkSingle.IsGuestUser = wk.IsGuestUserSheet;
					//		wkSingle.DocumentUrl = wk?.UploadPdf;

					//		wkSingle.FacebookContent = wk?.FacebookContent;
					//		wkSingle.WhatsAppContent = wk?.WhatsAppContent;
					//		wkSingle.MailContent = wk?.MailContent;

					//		if (wk.UploadThumbnail != null)
					//		{
					//			mediaProp = new MediaProp();

					//			mediaProp.AltText = wk?.UploadThumbnail?.Value<string>("altText");
					//			mediaProp.Target = "";
					//			mediaProp.Url = mediaUrl + wk?.UploadThumbnail?.Url();

					//			wkSingle.DesktopImage = mediaProp;
					//		}
					//		else
					//		{
					//			mediaProp = new MediaProp();
					//			wkSingle.DesktopImage = mediaProp;
					//		}

					//		if (wk.NextGenImage != null)
					//		{
					//			mediaProp = new MediaProp();

					//			mediaProp.AltText = wk?.NextGenImage?.Value<string>("altText");
					//			mediaProp.Target = "";
					//			mediaProp.Url = mediaUrl + wk?.NextGenImage?.Url();

					//			wkSingle.DesktopNextGenImage = mediaProp;
					//		}
					//		else
					//		{
					//			mediaProp = new MediaProp();
					//			wkSingle.DesktopNextGenImage = mediaProp;
					//		}

					//		if (wk.UploadMobileThumbnail != null)
					//		{
					//			mediaProp = new MediaProp();

					//			mediaProp.AltText = wk.UploadMobileThumbnail?.Value<string>("altText");
					//			mediaProp.Target = "";
					//			mediaProp.Url = mediaUrl + wk.UploadMobileThumbnail?.Url();

					//			wkSingle.MobileImage = mediaProp;
					//		}
					//		else
					//		{
					//			mediaProp = new MediaProp();
					//			wkSingle.MobileImage = mediaProp;
					//		}

					//		if (wk.MobileNextGenImage != null)
					//		{
					//			mediaProp = new MediaProp();

					//			mediaProp.AltText = wk.MobileNextGenImage?.Value<string>("altText");
					//			mediaProp.Target = "";
					//			mediaProp.Url = mediaUrl + wk.MobileNextGenImage?.Url();

					//			wkSingle.MobileNextGenImage = mediaProp;
					//		}
					//		else
					//		{
					//			mediaProp = new MediaProp();
					//			wkSingle.MobileNextGenImage = mediaProp;
					//		}


					//		worksheets.Add(wkSingle);
					//	}

					//	worksheetsRoot.Worksheets = worksheets;

					//	worksheetsMain.Add(worksheetsRoot);

					//}
				}

				response.StatusCode = 1;
				response.StatusMessage = "Done";
				response.Result = worksheetsMain;
				worksheetResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
			}

			return worksheetResponse;
		}
	}
}