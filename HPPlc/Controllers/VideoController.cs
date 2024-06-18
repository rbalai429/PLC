using HPPlc.Model;
using HPPlc.Models;
using HPPlc.Models.PlayVideo;
using HPPlc.Models.Videos;
using HPPlc.Models.Vimeo;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Controllers
{
	public class VideoController : SurfaceController
	{
		bool isplayvideo = false;
		private readonly IVariationContextAccessor _variationContextAccessor;
		public VideoController(IVariationContextAccessor variationContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
		}

		//public string VideoTutorial(int vrNoreId, string CultureInfo, string filterType, string cultureDownloadText, string CultureSubscribeforDownload, string SeeMoreText, string upgradeToPremiumText, string BuyNewSubscription)
		//{
		//    string vUserSubscription = String.Empty;
		//    StringBuilder sb = new StringBuilder();
		//    sb.Clear();

		//    sb = VideoTutorialData(vrNoreId, CultureInfo, filterType, cultureDownloadText, CultureSubscribeforDownload, SeeMoreText, upgradeToPremiumText, BuyNewSubscription);

		//    return sb.ToString();
		//}

		//public StringBuilder VideoTutorialData(int vrNoreId, string CultureInfo, string filterType, string cultureDownloadText, string CultureSubscribeforDownload, string SeeMoreContent, string upgradeToPremiumText, string BuyNewSubscription)
		//{
		//    string vUserSubscription = String.Empty;
		//    string bitlyLink = String.Empty;
		//    StringBuilder sb = new StringBuilder();
		//    StringBuilder recommendVideos = new StringBuilder();
		//    sb.Clear();
		//    recommendVideos.Clear();

		//    //Check here user subscription
		//    LoggedIn loggedin = new LoggedIn();
		//    loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

		//    //Get All subscription detais for user
		//    GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
		//    List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
		//    getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

		//    //find registered age group
		//    HPPlc.Models.dbAccessClass db = new HPPlc.Models.dbAccessClass();
		//    List<HPPlc.Models.SelectedAgeGroup> myagegroup = new List<HPPlc.Models.SelectedAgeGroup>();
		//    myagegroup = db.GetUserSelectedUserGroup();

		//    try
		//    {
		//        //Check here user subscription
		//        //LoggedIn loggedin = new LoggedIn();
		//        //loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);
		//        //if (loggedin != null)
		//        //	vUserSubscription = loggedin.SubscriptionValidationText;

		//        string culture = CultureName.GetCultureName();
		//        string subscribeUrl = culture == "/" ? culture + "subscription/" : culture + "/subscription/";

		//        _variationContextAccessor.VariationContext = new VariationContext(CultureInfo);
		//        var VideoRoot = Umbraco?.Content(vrNoreId)?.DescendantsOrSelf()?.OfType<Videos>().FirstOrDefault();

		//        if (VideoRoot != null)
		//        {
		//            var videoFilter = VideoRoot?.FilterOptions.OfType<FilterOptions>();
		//            sb.Append("<div class='video-tutorials'>");

		//            if (videoFilter != null)
		//            {
		//                sb.Append("<div class='video-fltr'>");
		//                foreach (var item in videoFilter.Where(x => x.Activation))
		//                {
		//                    if (item.FilterTag == filterType)
		//                        sb.Append("<a href='javascript:void(0)' class='active' onclick=VideoFilter('" + item?.FilterTag + "')>" + item?.FilterTitle + "</a>");
		//                    else
		//                        sb.Append("<a href='javascript:void(0)' onclick=VideoFilter('" + item?.FilterTag + "')>" + item?.FilterTitle + "</a>");
		//                }
		//                sb.Append("</div>");
		//            }

		//            //string seeMoreContent = Umbraco.GetDictionaryValue("See More");

		//            var mediaUrl = VideoRoot?.Value<IPublishedContent>("seeMoreMedia");
		//            var nextGenMediaUrl = VideoRoot?.SeeMoreNextGen;
		//            int firstTimeDisplayVideos = VideoRoot.FirstTimeDisplayVideos;

		//            var filter = videoFilter?.Where(x => x.Activation && x.FilterTag == filterType).FirstOrDefault();

		//            try
		//            {
		//                //All
		//                if (filterType == "all" && filter.FilterTag == "all")
		//                {
		//                    var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault()?
		//                        .Children?.OfType<Video>()?.Where(x => x.IsActive)?.Take(firstTimeDisplayVideos);
		//                    //var filteredVideo = Videos?.Children.OfType<Video>().Where(x => x.IsActive);
		//                    if (Videos != null && Videos.Any())
		//                    {
		//                        //sb.Append("<h4>" + age.ItemName + "</h4>");
		//                        sb.Append("<div class='recommended-list'>");
		//                        sb.Append("<div class='list-items'>");
		//                        foreach (var videoItems in Videos)
		//                        {
		//                            var PDF_File = videoItems?.UploadPdf;
		//                            bitlyLink = videoItems?.BitlyLink;

		//                            //Generate Bitly Link
		//                            //bitlyLink = generateBitlyLink(PDF_File, bitlyLink, WorkSheet.Id, CultureInfo);

		//                            sb.Append("<div class='item-col'>");
		//                            sb.Append("<div class='card-box'>");

		//                            try
		//                            {
		//                                string thumbUrl = String.Empty;
		//                                if (videoItems?.ThumbnailImage != null)
		//                                {
		//                                    thumbUrl = videoItems?.ThumbnailImage.Url().ToString();
		//                                }

		//                                //social integration
		//                                StringBuilder social = new StringBuilder();
		//                                social = VideoSocialShareContent(loggedin, getUserCurrentSubscription, myagegroup, videoItems?.FacebookContent, videoItems?.WhatsAppContent, videoItems?.MailContent, videoItems?.VideoYouTubeId, videoItems?.NextGenImage?.Url(), PDF_File, bitlyLink, videoItems?.AgeTitle.Name == null ? "" : videoItems?.AgeTitle.Name, videoItems?.Title == null ? "" : videoItems?.Title, CultureInfo, thumbUrl, videoItems?.Description.ToString(), videoItems.IsGuestUserSheet, videoItems.Subscriptions, videoItems.Id, cultureDownloadText, CultureSubscribeforDownload, upgradeToPremiumText, BuyNewSubscription, videoItems?.ThumbnailImage?.Value<string>("altText"), "", "videolisting", filterType);
		//                                sb.Append(social);
		//                            }
		//                            catch (Exception ex)
		//                            {
		//                                Logger.Error(reporting: typeof(VideoController), ex, message: "VideoSocialShareContent");
		//                            }

		//                            sb.Append("</div>");
		//                            sb.Append("</div>");
		//                        }

		//                        //See More
		//                        sb.Append("<div class='item-col'>");
		//                        sb.Append("<div class='card-box'>");

		//                        sb.Append("<div class='card-video'>");
		//                        string videoDetailsUrl = culture + "/videos/video-details?type=all&typeid=" + clsCommon.encrypto("0");
		//                        sb.Append("<a href='" + videoDetailsUrl + "'><span class='see-more-btn'>" + SeeMoreContent + "</span>");
		//                        if (mediaUrl != null)
		//                        {
		//                            sb.Append("<picture>");
		//                            if (nextGenMediaUrl != null)
		//                            {
		//                                sb.Append("<source srcset = '" + nextGenMediaUrl.Url() + "' type='image/webp'>");
		//                            }
		//                            sb.Append("<img alt='" + mediaUrl.Value("altText") + "' src='" + mediaUrl.Url() + "'>");
		//                            sb.Append("</picture>");
		//                        }
		//                        sb.Append("</a> </div>");

		//                        sb.Append("</div>");
		//                        sb.Append("</div>");

		//                        sb.Append("</div> ");
		//                        sb.Append("</div> ");
		//                    }

		//                    try
		//                    {
		//                        recommendVideos = RecommendedVideos(filterType, CultureInfo, cultureDownloadText, CultureSubscribeforDownload, firstTimeDisplayVideos, SeeMoreContent, upgradeToPremiumText, BuyNewSubscription);
		//                        sb.Append(recommendVideos);
		//                    }
		//                    catch (Exception ex)
		//                    {
		//                        Logger.Error(reporting: typeof(VideoController), ex, message: "RecommendedVideos");
		//                    }
		//                }
		//            }
		//            catch (Exception ex)
		//            {
		//                Logger.Error(reporting: typeof(VideoController), ex, message: "Video All");
		//            }

		//            try
		//            {
		//                //Age Wise
		//                if (filterType == "age")
		//                {
		//                    var allAges = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home").FirstOrDefault()?
		//                    .Children?.Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?
		//                    .Children?.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>()?.Where(x => x.IsActice);

		//                    if (allAges != null)
		//                    {
		//                        foreach (var age in allAges)
		//                        {
		//                            //var Video = Videos?.Children.OfType<Video>().Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue).OfType<Video>();
		//                            var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault()?
		//                                    .Children?.OfType<Video>()?.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue).Take(firstTimeDisplayVideos);
		//                            //var filteredVideo = Videos?.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue);
		//                            if (Videos != null && Videos.Any())
		//                            {
		//                                //sb.Append("<div class='fit-hd'><h4>" + age.ItemName + "</h4><a href=video-details?type=age&typeid=" + clsCommon.encrypto(age.ItemValue) + " class='see-more'>"+ seeMoreContent+"</a></div>");
		//                                sb.Append("<div class='fit-hd'>");
		//                                sb.Append("<h4>" + age.ItemName + "</h4>");
		//                                //if (!String.IsNullOrEmpty(age.SubTitle))
		//                                //{
		//                                //	sb.Append("<h5>" + age.SubTitle + "</h5>");
		//                                //}
		//                                sb.Append("</div>");
		//                                sb.Append("<div class='video-thum-row'>");
		//                                sb.Append("<div class='video-thum-slider'>");
		//                                foreach (var videoItems in Videos)
		//                                {
		//                                    var PDF_File = videoItems?.Value<string>("uploadPDF");
		//                                    bitlyLink = videoItems?.Value<string>("bitlyLink");

		//                                    //Generate Bitly Link
		//                                    //bitlyLink = generateBitlyLink(PDF_File, bitlyLink, WorkSheet.Id, CultureInfo);

		//                                    sb.Append("<items>");
		//                                    sb.Append("<div class='item-col'>");
		//                                    sb.Append("<div class='card-box'>");

		//                                    try
		//                                    {
		//                                        //social integration
		//                                        StringBuilder social = new StringBuilder();
		//                                        social = VideoSocialShareContent(loggedin, getUserCurrentSubscription, myagegroup, videoItems.FacebookContent, videoItems.WhatsAppContent, videoItems.MailContent, videoItems?.VideoYouTubeId, videoItems?.NextGenImage?.Url(), PDF_File, bitlyLink, videoItems?.AgeTitle.Name == null ? "" : videoItems?.AgeTitle.Name, videoItems?.Title, CultureInfo, videoItems?.ThumbnailImage?.Url(), videoItems?.Description.ToString(), videoItems.IsGuestUserSheet, videoItems.Subscriptions, videoItems.Id, cultureDownloadText, CultureSubscribeforDownload, upgradeToPremiumText, BuyNewSubscription, videoItems?.ThumbnailImage?.Value<string>("altText"), "", "videolisting", filterType);
		//                                        sb.Append(social);
		//                                    }
		//                                    catch { }

		//                                    //sb.Append("<div class='card-btn-print'>");
		//                                    //sb.Append("<div class='btn-col'><a href='#' class='btn'>Download all Worksheets<span class='download-icon'></span></a></div>");
		//                                    //sb.Append("</div>");

		//                                    sb.Append("</div>");
		//                                    sb.Append("</div>");
		//                                    sb.Append("</items>");
		//                                }

		//                                //See More
		//                                sb.Append("<items>");
		//                                sb.Append("<div class='item-col'>");
		//                                sb.Append("<div class='card-box'>");
		//                                string videoDetailsUrl = culture == "/" ? String.Empty : culture + "videos/video-details?type=age&typeid=" + clsCommon.encrypto(age.ItemValue);
		//                                sb.Append("<a href='" + videoDetailsUrl + "'><span class='see-more-btn'>" + SeeMoreContent + "</span>");
		//                                if (mediaUrl != null)
		//                                {
		//                                    sb.Append("<picture>");
		//                                    if (nextGenMediaUrl != null)
		//                                    {
		//                                        sb.Append("<source srcset = '" + nextGenMediaUrl.Url() + "' type='image/webp'>");
		//                                    }
		//                                    sb.Append("<img alt='" + mediaUrl.Value("altText") + "' src='" + mediaUrl.Url() + "'>");
		//                                    sb.Append("</picture>");
		//                                }
		//                                sb.Append("</a> </div>");

		//                                sb.Append("</div>");
		//                                //sb.Append("</div>");
		//                                sb.Append("</items>");

		//                                sb.Append("</div> ");
		//                                sb.Append("</div> ");

		//                            }
		//                        }
		//                    }

		//                    try
		//                    {
		//                        recommendVideos = RecommendedVideos(filterType, CultureInfo, cultureDownloadText, CultureSubscribeforDownload, firstTimeDisplayVideos, SeeMoreContent, upgradeToPremiumText, BuyNewSubscription);
		//                        sb.Append(recommendVideos);
		//                    }
		//                    catch (Exception ex)
		//                    {
		//                        Logger.Error(reporting: typeof(VideoController), ex, message: "RecommendedVideos - Age");
		//                    }
		//                }
		//            }
		//            catch (Exception ex)
		//            {
		//                Logger.Error(reporting: typeof(VideoController), ex, message: "Video - Age");
		//            }

		//            try
		//            {
		//                //Pathway Wise
		//                if (filterType == "pathway")
		//                {
		//                    //get from master data
		//                    var allLearningPathways = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
		//                        .Children?.Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?
		//                        .Children?.Where(x => x.ContentType.Alias == "learningPathwaysMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>()?.Where(x => x.IsActice);

		//                    if (allLearningPathways != null)
		//                    {
		//                        foreach (var pathway in allLearningPathways)
		//                        {
		//                            var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault()?.Children?.OfType<Video>()?.Where(x => x.IsActive && Umbraco.Content(x.LearningPathway.Select(y => y.Udi)).ToList().OfType<NameListItem>().Any(o => o.ItemValue == pathway.ItemValue)).Take(firstTimeDisplayVideos);

		//                            //var filteredVideo = Videos?.Where(x => x.IsActive && Umbraco.Content(x.LearningPathway.Select(y => y.Udi)).ToList().OfType<NameListItem>().Any(o => o.ItemValue == pathway.ItemValue));
		//                            if (Videos != null && Videos.Any())
		//                            {
		//                                sb.Append("<div class='fit-hd'>");
		//                                sb.Append("<h4>" + pathway.ItemName + "</h4>");
		//                                //if (!String.IsNullOrEmpty(pathway.SubTitle))
		//                                //{
		//                                //	sb.Append("<h5>" + pathway.SubTitle + "</h5>");
		//                                //}
		//                                sb.Append("</div>");
		//                                sb.Append("<div class='video-thum-row'>");
		//                                sb.Append("<div class='video-thum-slider'>");
		//                                foreach (var videoItems in Videos)
		//                                {
		//                                    var PDF_File = videoItems?.Value<string>("uploadPDF");
		//                                    bitlyLink = videoItems?.Value<string>("bitlyLink");

		//                                    //Generate Bitly Link
		//                                    //bitlyLink = generateBitlyLink(PDF_File, bitlyLink, WorkSheet.Id, CultureInfo);

		//                                    sb.Append("<items>");
		//                                    sb.Append("<div class='item-col'>");
		//                                    sb.Append("<div class='card-box'>");

		//                                    try
		//                                    {
		//                                        //social integration
		//                                        StringBuilder social = new StringBuilder();
		//                                        social = VideoSocialShareContent(loggedin, getUserCurrentSubscription, myagegroup, videoItems.FacebookContent, videoItems.WhatsAppContent, videoItems.MailContent, videoItems?.VideoYouTubeId, videoItems?.NextGenImage?.Url(), PDF_File, bitlyLink, videoItems?.AgeTitle.Name == null ? "" : videoItems?.AgeTitle.Name, videoItems?.Title, CultureInfo, videoItems?.ThumbnailImage?.Url(), videoItems?.Description.ToString(), videoItems.IsGuestUserSheet, videoItems.Subscriptions, videoItems.Id, cultureDownloadText, CultureSubscribeforDownload, upgradeToPremiumText, BuyNewSubscription, videoItems?.ThumbnailImage?.Value<string>("altText"), pathway.ItemValue, "videolisting", filterType);
		//                                        sb.Append(social);
		//                                    }
		//                                    catch (Exception ex)
		//                                    {
		//                                        Logger.Error(reporting: typeof(VideoController), ex, message: "VideoSocialShareContent - Pathway");
		//                                    }

		//                                    //sb.Append("<div class='card-btn-print'>");
		//                                    //sb.Append("<div class='btn-col'><a href='#' class='btn'>Download all Worksheets<span class='download-icon'></span></a></div>");
		//                                    //sb.Append("</div>");

		//                                    sb.Append("</div>");
		//                                    sb.Append("</div>");
		//                                    sb.Append("</items>");
		//                                }

		//                                //See More
		//                                sb.Append("<items>");
		//                                sb.Append("<div class='item-col'>");
		//                                sb.Append("<div class='card-box'>");
		//                                string videoDetailsUrl = culture == "/" ? String.Empty : culture + "videos/video-details?type=pathway&typeid=" + clsCommon.encrypto(pathway.ItemValue);
		//                                sb.Append("<a href='" + videoDetailsUrl + "'><span class='see-more-btn'>" + SeeMoreContent + "</span>");
		//                                if (mediaUrl != null)
		//                                {
		//                                    sb.Append("<picture>");
		//                                    if (nextGenMediaUrl != null)
		//                                    {
		//                                        sb.Append("<source srcset = '" + nextGenMediaUrl.Url() + "' type='image/webp'>");
		//                                    }
		//                                    sb.Append("<img alt='" + mediaUrl.Value("altText") + "' src='" + mediaUrl.Url() + "'>");
		//                                    sb.Append("</picture>");
		//                                }
		//                                sb.Append("</a> </div>");

		//                                sb.Append("</div>");
		//                                //sb.Append("</div>");
		//                                sb.Append("</items>");

		//                                sb.Append("</div> ");
		//                                sb.Append("</div> ");
		//                            }
		//                        }
		//                    }

		//                    try
		//                    {
		//                        recommendVideos = RecommendedVideos(filterType, CultureInfo, cultureDownloadText, CultureSubscribeforDownload, firstTimeDisplayVideos, SeeMoreContent, upgradeToPremiumText, BuyNewSubscription);
		//                        sb.Append(recommendVideos);
		//                    }
		//                    catch (Exception ex)
		//                    {
		//                        Logger.Error(reporting: typeof(VideoController), ex, message: "RecommendedVideos - Video Pathway");
		//                    }
		//                }
		//            }
		//            catch (Exception ex)
		//            {
		//                Logger.Error(reporting: typeof(VideoController), ex, message: "Video - Pathway");
		//            }

		//            sb.Append("</div> ");
		//        }

		//        //get video with age filter
		//        //var filteredVideos = Videos.Children?
		//        //	.OfType<Video>()?.Where(x => x.IsPublished() && x.IsActive && Umbraco?.Content(x.AgeTitle.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue ==
		//        //	 Array.Find(agerange, element => element == Umbraco?.Content(x.AgeTitle.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue));

		//        //if (filteredVideos != null)
		//        //{
		//        //foreach (var Video in Videos?.Children.Where(x => x.IsPublished() && x.Value<Link>("ageTitle").Name == agerange[i]))
		//        //foreach (var Video in Videos?.Children.OfType<Video>().Where(x => x.IsActive))
		//        //{
		//        //	var Image = Video.ThumbnailImage;
		//        //	var AgeTitle = Video.AgeTitle;
		//        //	var AgeTitleDesc = Umbraco?.Content(AgeTitle?.Udi);
		//        //	var ItemName = AgeTitleDesc.Value("itemName");

		//        //	var volumeTextRef = Video.SelectVolume;
		//        //	var volumeText = Umbraco?.Content(volumeTextRef?.Udi)?.Value<string>("itemName");
		//        //	var volumeCSS = Video?.VolumeBackgroungCss;

		//        //	bool IsGuestUserSheet = Video.IsGuestUserSheet;
		//        //	var VideoId = Video?.VideoYouTubeId;

		//        //	var textMailSubject = Video?.Title;
		//        //	var Subscriptions = Video?.Subscriptions;
		//        //	//foreach (var videoCat in Video.Category)
		//        //	//{
		//        //	//	string catValue = Umbraco?.Content(videoCat?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.Where(x => x.IsActice).FirstOrDefault()?.ItemValue;
		//        //	//	if (catValue == Array.Find(categoryitem, element => element == catValue))
		//        //	//	{
		//        //	//		foreach (var pathways in Video?.LearningPathway)
		//        //	//		{
		//        //	//			string larningPathway = Umbraco?.Content(pathways?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.Where(x => x.IsActice).FirstOrDefault()?.ItemValue;
		//        //	//			if (larningPathway == Array.Find(pathwayitem, element => element == larningPathway))
		//        //	//			{



		//        //	//sb.Append("<div class='item-col'>");
		//        //	//sb.Append("<div class='card-box'>");
		//        //	//if (!String.IsNullOrEmpty(volumeText))
		//        //	//{
		//        //	//	sb.Append("<div class='card-tag " + volumeCSS + "'>" + volumeText + "</div>");
		//        //	//}







		//        //	//if (!String.IsNullOrEmpty(Video.VideoYouTubeId))
		//        //	//{
		//        //	//	StringBuilder social = new StringBuilder();
		//        //	//	social = VideoSocialShareContent(Video.VideoYouTubeId, ItemName.ToString(), culture, Image.Url(), Video?.Description.ToString(), IsGuestUserSheet, Subscriptions);
		//        //	//	sb.Append(social);
		//        //	//}
		//        //	//sb.Append("</div>");
		//        //	//sb.Append("</div>");
		//        //}

		//        //	break;
		//        //}
		//        //		}

		//        //		break;
		//        //	}
		//        //}
		//        //}


		//    }
		//    catch (Exception ex)
		//    {
		//        Logger.Error(reporting: typeof(VideoController), ex, message: "Video - VideoTutorialData");
		//    }


		//    return sb;
		//}

		//public string VideoPlay(string CultureInfo, string filterType, string cultureDownloadText, string CultureSubscribeforDownload, string upgradeToPremiumText, string BuyNewSubscription)
		//{
		//    string vUserSubscription = String.Empty;
		//    string bitlyLink = String.Empty;
		//    string videos = String.Empty;
		//    string filter = String.Empty;
		//    string vrVideoId = String.Empty;
		//    string vrNodeId = String.Empty;
		//    string vrPathwayValue = String.Empty;

		//    StringBuilder sb = new StringBuilder();
		//    sb.Clear();

		//    //Check here user subscription
		//    LoggedIn loggedin = new LoggedIn();
		//    loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

		//    //Get All subscription detais for user
		//    GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
		//    List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
		//    getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

		//    //find registered age group
		//    HPPlc.Models.dbAccessClass db = new HPPlc.Models.dbAccessClass();
		//    List<HPPlc.Models.SelectedAgeGroup> myagegroup = new List<HPPlc.Models.SelectedAgeGroup>();
		//    myagegroup = db.GetUserSelectedUserGroup();

		//    try
		//    {
		//        if (!String.IsNullOrEmpty(filterType))
		//        {
		//            string decrypredParameter = clsCommon.Decrypt(HttpUtility.UrlDecode(filterType));
		//            string[] qAll = decrypredParameter.Split(':');

		//            if (qAll != null && qAll.Length > 1)
		//            {
		//                if (!String.IsNullOrEmpty(qAll[0]))
		//                    filter = qAll[0];
		//                if (!String.IsNullOrEmpty(qAll[1]))
		//                    vrVideoId = qAll[1];
		//                if (!String.IsNullOrEmpty(qAll[2]))
		//                    vrNodeId = qAll[2];
		//                if (filter == "pathway")
		//                {
		//                    if (!String.IsNullOrEmpty(qAll[3]))
		//                        vrPathwayValue = qAll[3];
		//                }
		//            }


		//            if (!String.IsNullOrEmpty(filter) && !String.IsNullOrEmpty(vrVideoId) && !String.IsNullOrEmpty(vrNodeId))
		//            {
		//                try
		//                {
		//                    //Check here user subscription
		//                    //LoggedIn loggedin = new LoggedIn();
		//                    //loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);
		//                    //if (loggedin != null)
		//                    //	vUserSubscription = loggedin.SubscriptionValidationText;

		//                    string culture = CultureName.GetCultureName();
		//                    string subscribeUrl = culture + "/subscription/";

		//                    _variationContextAccessor.VariationContext = new VariationContext(CultureInfo);
		//                    var Videos = Umbraco?.Content(vrNodeId)?.DescendantsOrSelf()?.OfType<Video>()?.FirstOrDefault();
		//                    if (Videos != null)
		//                    {
		//                        var PDF_File = Videos?.Value<string>("uploadPDF");
		//                        bitlyLink = Videos?.Value<string>("bitlyLink");
		//                        string ageGroup = Umbraco?.Content(Videos?.AgeTitle.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue;

		//                        sb.Append("<div class='details-video'>");
		//                        sb.Append("<div class='video-play-cont'>");
		//                        sb.Append("<div class='video-play'>");
		//                        sb.Append("<div class='card-box'>");

		//                        try
		//                        {
		//                            //social integration
		//                            StringBuilder social = new StringBuilder();
		//                            social = VideoSocialShareContent(loggedin, getUserCurrentSubscription, myagegroup, Videos?.FacebookContent, Videos?.WhatsAppContent, Videos?.MailContent, Videos?.VideoYouTubeId, Videos?.NextGenImage?.Url(), PDF_File, bitlyLink, ageGroup, Videos?.Title, culture, Videos?.ThumbnailImage?.Url(), Videos?.Description.ToString(), Videos.IsGuestUserSheet, Videos.Subscriptions, Videos.Id, cultureDownloadText, CultureSubscribeforDownload, upgradeToPremiumText, BuyNewSubscription, Videos?.ThumbnailImage?.Value<string>("altText"), vrPathwayValue, "videodetails", filter);
		//                            sb.Append(social);
		//                        }
		//                        catch (Exception ex)
		//                        {
		//                            Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - VideoSocialShareContent");
		//                        }


		//                        sb.Append("</div>");
		//                        sb.Append("</div>");
		//                        sb.Append("</div>");
		//                        sb.Append("</div>");

		//                        //sb.Append("<div class='recommended-list'>");
		//                        //sb.Append("<div class='list-items'>");

		//                        //sb.Append("<div class='list-items'>");
		//                        sb.Append("<div class='video-tutorials'>");
		//                        sb.Append("<div class='video-thum-row'>");
		//                        sb.Append("<div class='video-thum-slider hltdvideo'>");

		//                        if (!String.IsNullOrEmpty(filter))
		//                        {
		//                            IEnumerable<Video> filteredVideo = null;
		//                            if (filter == "all")
		//                            {
		//                                filteredVideo = Umbraco?.Content(vrNodeId)?.Parent?.Children?.OfType<Video>()?.Where(x => x.IsActive);
		//                            }
		//                            else if (filter == "age")
		//                            {
		//                                filteredVideo = Umbraco?.Content(vrNodeId)?.Parent?.Children?.OfType<Video>()?
		//                                    .Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == ageGroup);//Umbraco?.Content(Videos?.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue
		//                            }
		//                            else if (filter == "pathway")
		//                            {

		//                                //filteredVideo = Umbraco?.Content(vrNodeId)?.Parent?.Children?.OfType<Video>()?
		//                                //	.Where(x => x.IsActive && Umbraco.Content(x.LearningPathway.Select(y => y.Udi)).ToList().OfType<NameListItem>().Any(o => o.ItemValue == Umbraco.Content(Videos?.LearningPathway.Select(y => y.Udi)).ToList().OfType<NameListItem>().FirstOrDefault().ItemValue));
		//                                if (!String.IsNullOrEmpty(vrPathwayValue))
		//                                {
		//                                    filteredVideo = Umbraco?.Content(vrNodeId)?.Parent?.Children?.OfType<Video>()?
		//                                        .Where(x => x.IsActive && Umbraco.Content(x.LearningPathway.Select(y => y.Udi)).ToList().OfType<NameListItem>().Any(o => o.ItemValue == vrPathwayValue));
		//                                }
		//                            }

		//                            try
		//                            {
		//                                if (filteredVideo != null && filteredVideo.Any())
		//                                {
		//                                    foreach (var video in filteredVideo)
		//                                    {
		//                                        string PDFFile = video?.UploadPdf;
		//                                        bitlyLink = video?.BitlyLink;
		//                                        string selAgeGroup = Umbraco?.Content(video?.AgeTitle.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue;

		//                                        sb.Append("<items>");
		//                                        sb.Append("<div class='item-col'>");
		//                                        if (video.VideoYouTubeId == vrVideoId)
		//                                            sb.Append("<div class='card-box active-video'>");
		//                                        else
		//                                            sb.Append("<div class='card-box'>");

		//                                        try
		//                                        {
		//                                            //social integration
		//                                            StringBuilder socialItems = new StringBuilder();
		//                                            socialItems = VideoSocialShareContent(loggedin, getUserCurrentSubscription, myagegroup, video?.FacebookContent, video?.WhatsAppContent, video?.MailContent, video?.VideoYouTubeId, video?.NextGenImage?.Url(), PDFFile, bitlyLink, selAgeGroup, video?.Title, culture, video?.ThumbnailImage?.Url(), video?.Description.ToString(), video.IsGuestUserSheet, video.Subscriptions, video.Id, cultureDownloadText, CultureSubscribeforDownload, upgradeToPremiumText, BuyNewSubscription, video?.ThumbnailImage?.Value<string>("altText"), "", "videolisting", filter);
		//                                            sb.Append(socialItems);

		//                                            if (isplayvideo)
		//                                                videos += video.VideoYouTubeId + ",";
		//                                        }
		//                                        catch (Exception ex)
		//                                        {
		//                                            Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - VideoSocialShareContent");
		//                                        }

		//                                        sb.Append("</div>");
		//                                        sb.Append("</div>");
		//                                        sb.Append("</items>");
		//                                    }
		//                                }
		//                            }
		//                            catch (Exception ex)
		//                            {
		//                                Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - Video Bind");
		//                            }

		//                            //Video save for recommended
		//                            try
		//                            {
		//                                dbProxy _db = new dbProxy();
		//                                GetStatus insertStatus = new GetStatus();
		//                                int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
		//                                string Videolength = Videos?.VideoTotalTimeInMin;
		//                                if (String.IsNullOrEmpty(Videolength))
		//                                    Videolength = "0";

		//                                List<SetParameters> sp = new List<SetParameters>()
		//                            {
		//                                new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
		//                                new SetParameters{ ParameterName = "@NodeId", Value = vrNodeId.ToString() },
		//                                new SetParameters { ParameterName = "@VideoId", Value = vrVideoId.ToString() },
		//                                new SetParameters { ParameterName = "@VideoType", Value = filter },
		//                                new SetParameters { ParameterName = "@VideoDuration", Value = Videolength }
		//                            };

		//                                insertStatus = _db.StoreData("USP_TrackVideo", sp);
		//                            }
		//                            catch (Exception ex)
		//                            {
		//                                Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - Video Save for Recommend");
		//                            }
		//                        }

		//                        sb.Append("</div>");
		//                        sb.Append("</div>");
		//                        sb.Append("</div>");
		//                        //sb.Append("</div>");

		//                        //sb.Append("<div class='video-tutorials'>");
		//                        //sb.Append("<div class='video-fltr'>");

		//                        //sb.Append("</div>");
		//                        //sb.Append("</div>");
		//                    }
		//                }
		//                catch (Exception ex)
		//                {
		//                    Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - Video Filter Bind");
		//                }
		//            }
		//        }

		//        videos = videos.TrimEnd(',');
		//    }
		//    catch (Exception ex)
		//    {
		//        Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - VideoPlay");
		//    }

		//    return sb.ToString() + "|" + videos.ToString();
		//}


		//public StringBuilder RecommendedVideos(string filterType, string CultureInfo, string cultureDownloadText, string CultureSubscribeforDownload, int firstTimeDisplayVideos, string SeeMoreContent, string upgradeToPremiumText, string BuyNewSubscription)
		//{
		//    StringBuilder sb = new StringBuilder();
		//    string bitlyLink = String.Empty;
		//    string culture = CultureName.GetCultureName();

		//    //Check here user subscription
		//    LoggedIn loggedin = new LoggedIn();
		//    loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

		//    //Get All subscription detais for user
		//    GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
		//    List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
		//    getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

		//    //find registered age group
		//    HPPlc.Models.dbAccessClass db = new HPPlc.Models.dbAccessClass();
		//    List<HPPlc.Models.SelectedAgeGroup> myagegroup = new List<HPPlc.Models.SelectedAgeGroup>();
		//    myagegroup = db.GetUserSelectedUserGroup();

		//    try
		//    {
		//        List<RecommendedVideos> recommendedVideos = new List<RecommendedVideos>();
		//        recommendedVideos = GetRecommendedVideosDB(1, filterType, firstTimeDisplayVideos);

		//        if (recommendedVideos != null && recommendedVideos.Any())
		//        {
		//            int[] nodes = new int[0];
		//            for (int i = 0; i < recommendedVideos.Count; i++)
		//            {
		//                nodes = nodes.Concat(new int[] { recommendedVideos[i].NodeId }).ToArray();
		//            }

		//            var videos = Umbraco?.Content(nodes);
		//            if (videos != null && videos.Any())
		//            {
		//                //string seeMoreContent = Umbraco.GetDictionaryValue("See More");
		//                var mediaUrl = videos?.First().Parent?.Value<IPublishedContent>("seeMoreMedia");
		//                var nextGenMediaUrl = videos?.First().Parent?.Value<IPublishedContent>("seeMoreNextGen");
		//                string recommendedVideoTitle = videos?.First()?.Parent?.Parent?.Value<string>("recommendedVideosTitle");

		//                sb.Append("<h4>" + recommendedVideoTitle + "</h4>");
		//                sb.Append("<div class='video-thum-row'>");
		//                sb.Append("<div class='recommended-videos'>");
		//                sb.Append("<div class='video-thum-slider'>");

		//                foreach (var videoItems in videos?.OfType<Video>()?.Where(x => x.IsActive))
		//                {
		//                    var PDF_File = videoItems?.UploadPdf;
		//                    bitlyLink = videoItems.BitlyLink;

		//                    //Generate Bitly Link
		//                    //bitlyLink = generateBitlyLink(PDF_File, bitlyLink, WorkSheet.Id, CultureInfo);
		//                    sb.Append("<items>");
		//                    sb.Append("<div class='item-col'>");
		//                    sb.Append("<div class='card-box'>");

		//                    try
		//                    {
		//                        //social integration
		//                        StringBuilder social = new StringBuilder();
		//                        social = VideoSocialShareContent(loggedin, getUserCurrentSubscription, myagegroup, videoItems?.FacebookContent, videoItems?.WhatsAppContent, videoItems?.MailContent, videoItems?.VideoYouTubeId, videoItems?.NextGenImage?.Url(), PDF_File, bitlyLink, videoItems?.AgeTitle.Name == null ? "" : videoItems?.AgeTitle.Name, videoItems?.Title == null ? "" : videoItems?.Title, CultureInfo, videoItems?.ThumbnailImage?.Url(), videoItems?.Description.ToString(), videoItems.IsGuestUserSheet, videoItems.Subscriptions, videoItems.Id, cultureDownloadText, CultureSubscribeforDownload, upgradeToPremiumText, BuyNewSubscription, videoItems?.ThumbnailImage?.Value<string>("altText"), "", "videolisting", filterType);
		//                        sb.Append(social);
		//                    }
		//                    catch (Exception ex)
		//                    {
		//                        Logger.Error(reporting: typeof(VideoController), ex, message: "Recommended Video - VideoSocialShareContent");
		//                    }

		//                    sb.Append("</div>");
		//                    sb.Append("</div>");
		//                    sb.Append("</items>");
		//                }

		//                //See More
		//                sb.Append("<items>");
		//                sb.Append("<div class='item-col'>");
		//                sb.Append("<div class='card-box'>");
		//                string videoDetailsUrl = culture == "/" ? String.Empty : culture + "videos/video-details?type=rcmd&typeid=" + clsCommon.encrypto(filterType);
		//                sb.Append("<a href='" + videoDetailsUrl + "'><span class='see-more-btn'>" + SeeMoreContent + "</span>");
		//                if (mediaUrl != null)
		//                {
		//                    sb.Append("<picture>");
		//                    if (nextGenMediaUrl != null)
		//                    {
		//                        sb.Append("<source srcset = '" + nextGenMediaUrl.Url() + "' type='image/webp'>");
		//                    }
		//                    sb.Append("<img alt='" + mediaUrl.Value("altText") + "' src='" + mediaUrl.Url() + "'>");
		//                    sb.Append("</picture>");
		//                }
		//                sb.Append("</a> </div>");

		//                sb.Append("</div>");
		//                sb.Append("</div>");
		//                sb.Append("</items>");

		//                sb.Append("</div> ");
		//                sb.Append("</div> ");
		//                sb.Append("</div> ");
		//            }
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        Logger.Error(reporting: typeof(VideoController), ex, message: "Recommended Video");
		//    }

		//    return sb;
		//}

		public List<RecommendedVideos> GetRecommendedVideosDB(int qtype, string filterType, int firstTimeDisplayVideos)
		{
			dbProxy _db = new dbProxy();
			List<RecommendedVideos> recommendedVideos = new List<RecommendedVideos>();

			List<SetParameters> sp = new List<SetParameters>()
			{
					new SetParameters{ ParameterName = "@QType", Value = qtype.ToString() },
					new SetParameters{ ParameterName = "@UniqueId", Value = "0" },
					new SetParameters{ ParameterName = "@UserId", Value = "0" },
					new SetParameters{ ParameterName = "@NodeId", Value = "0" },
					new SetParameters{ ParameterName = "@VideoPlayId", Value = "" },
					new SetParameters{ ParameterName = "@filterType", Value = filterType },
					new SetParameters{ ParameterName = "@filterRange", Value = firstTimeDisplayVideos.ToString() }
			};

			recommendedVideos = _db.GetDataMultiple<RecommendedVideos>("USP_ProcGetRecommendedVideos", recommendedVideos, sp);

			return recommendedVideos;
		}

		//public StringBuilder VideoDetailsPage(string vrFilterType, string vrFilterId, string CultureInfo, string cultureDownloadText, string CultureSubscribeforDownload, int vrDisplayOnPage, string upgradeToPremiumText, string BuyNewSubscription)
		//{
		//    string vUserSubscription = String.Empty;
		//    string recommendedTitle = String.Empty;
		//    string bitlyLink = String.Empty;
		//    int? countOfTotalVideos = 0;
		//    StringBuilder sb = new StringBuilder();
		//    sb.Clear();

		//    //Check here user subscription
		//    LoggedIn loggedin = new LoggedIn();
		//    loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

		//    //Get All subscription detais for user
		//    GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
		//    List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
		//    getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

		//    //find registered age group
		//    HPPlc.Models.dbAccessClass db = new HPPlc.Models.dbAccessClass();
		//    List<HPPlc.Models.SelectedAgeGroup> myagegroup = new List<HPPlc.Models.SelectedAgeGroup>();
		//    myagegroup = db.GetUserSelectedUserGroup();

		//    try
		//    {
		//        //Check here user subscription
		//        //LoggedIn loggedin = new LoggedIn();
		//        //loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);
		//        //if (loggedin != null)
		//        //	vUserSubscription = loggedin.SubscriptionValidationText;

		//        string culture = CultureName.GetCultureName();
		//        string subscribeUrl = culture == "/" ? culture + "subscription/" : culture + "/subscription/";

		//        if (!String.IsNullOrEmpty(vrFilterType) && !String.IsNullOrEmpty(vrFilterId))
		//        {
		//            _variationContextAccessor.VariationContext = new VariationContext(CultureInfo);
		//            var videoRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
		//                                        .Where(x => x.ContentType.Alias == "videos")?.FirstOrDefault()?.Children?
		//                                        .Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault();
		//            if (videoRoot != null)
		//            {
		//                //load more 
		//                int loadCount = videoRoot.Parent.Value<int>("firstTimeDisplayVideosOnDetails");
		//                if (vrDisplayOnPage == 0)
		//                    vrDisplayOnPage = loadCount;
		//                else
		//                {
		//                    vrDisplayOnPage += loadCount;
		//                }

		//                string FilterValue = clsCommon.Decrypto(vrFilterId);
		//                IEnumerable<Video> videos = null;
		//                int[] nodes = new int[0];


		//                //All
		//                if (vrFilterType == "all")
		//                {
		//                    countOfTotalVideos = videoRoot?.Children?.OfType<Video>()?
		//                        .Where(x => x.IsActive)?.Count();

		//                    videos = videoRoot?.Children?.OfType<Video>()?
		//                        .Where(x => x.IsActive).Take(vrDisplayOnPage);
		//                }
		//                else if (vrFilterType == "age")
		//                {
		//                    countOfTotalVideos = videoRoot?.Children?.OfType<Video>()?
		//                        .Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == FilterValue)?.Count();

		//                    videos = videoRoot?.Children?.OfType<Video>()?
		//                        .Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == FilterValue).Take(vrDisplayOnPage);
		//                }
		//                else if (vrFilterType == "pathway")
		//                {
		//                    countOfTotalVideos = videoRoot?.Children?.OfType<Video>()?
		//                        .Where(x => x.IsActive && Umbraco.Content(x.LearningPathway.Select(y => y.Udi)).ToList().OfType<NameListItem>().Any(o => o.ItemValue == FilterValue))?.Count();

		//                    videos = videoRoot?.Children?.OfType<Video>()?
		//                        .Where(x => x.IsActive && Umbraco.Content(x.LearningPathway.Select(y => y.Udi)).ToList().OfType<NameListItem>().Any(o => o.ItemValue == FilterValue)).Take(vrDisplayOnPage);
		//                }
		//                else if (vrFilterType == "rcmd")
		//                {
		//                    List<RecommendedVideos> recommendedVideos = new List<RecommendedVideos>();
		//                    recommendedVideos = GetRecommendedVideosDB(1, FilterValue, vrDisplayOnPage);

		//                    if (recommendedVideos != null && recommendedVideos.Any())
		//                    {
		//                        for (int i = 0; i < recommendedVideos.Count; i++)
		//                        {
		//                            nodes = nodes.Concat(new int[] { recommendedVideos[i].NodeId }).ToArray();
		//                        }

		//                        var rcmdVideos = Umbraco?.Content(nodes);
		//                        if (rcmdVideos != null && rcmdVideos.Any())
		//                        {
		//                            countOfTotalVideos = recommendedVideos?.First()?.VideoCount;
		//                            recommendedTitle = rcmdVideos?.First()?.Parent?.Value<string>("recommendedVideosTitle");
		//                            sb.Append("<div class='title-fltr'><div><h3> " + recommendedTitle + " </h3></div></div>");
		//                            sb.Append("<div class='video-tutorials'>");

		//                            try
		//                            {
		//                                if (rcmdVideos != null && rcmdVideos.Any())
		//                                {
		//                                    //sb.Append("<h4>" + age.ItemName + "</h4>");
		//                                    sb.Append("<div class='recommended-list'>");
		//                                    sb.Append("<div class='list-items'>");
		//                                    foreach (var videoItems in rcmdVideos.OfType<Video>())
		//                                    {
		//                                        var PDF_File = videoItems?.Value<string>("uploadPDF");
		//                                        bitlyLink = videoItems?.Value<string>("bitlyLink");

		//                                        //Generate Bitly Link
		//                                        //bitlyLink = generateBitlyLink(PDF_File, bitlyLink, WorkSheet.Id, CultureInfo);

		//                                        sb.Append("<div class='item-col'>");
		//                                        sb.Append("<div class='card-box'>");

		//                                        try
		//                                        {
		//                                            //social integration
		//                                            StringBuilder social = new StringBuilder();
		//                                            social = VideoSocialShareContent(loggedin, getUserCurrentSubscription, myagegroup, videoItems.FacebookContent, videoItems.WhatsAppContent, videoItems.MailContent, videoItems?.VideoYouTubeId, videoItems?.NextGenImage?.Url(), PDF_File, bitlyLink, videoItems?.AgeTitle.Name == null ? "" : videoItems?.AgeTitle.Name, videoItems?.Title == null ? "" : videoItems?.Title, CultureInfo, videoItems?.ThumbnailImage?.Url(), videoItems?.Description.ToString(), videoItems.IsGuestUserSheet, videoItems.Subscriptions, videoItems.Id, cultureDownloadText, CultureSubscribeforDownload, upgradeToPremiumText, BuyNewSubscription, videoItems?.ThumbnailImage?.Value<string>("altText"), "", "videolisting", vrFilterType);
		//                                            sb.Append(social);
		//                                        }
		//                                        catch (Exception ex)
		//                                        {
		//                                            Logger.Error(reporting: typeof(VideoController), ex, message: "Video Details page - VideoSocialShareContent");
		//                                        }

		//                                        sb.Append("</div>");
		//                                        sb.Append("</div>");
		//                                    }

		//                                    sb.Append("</div> ");
		//                                    sb.Append("</div> ");

		//                                    if (countOfTotalVideos > vrDisplayOnPage)
		//                                    {
		//                                        sb.Append("<div class='load-more'><a href='javascript:void(0);' onclick='loadMore(" + vrDisplayOnPage + ")'></a></div>");
		//                                    }
		//                                }
		//                            }
		//                            catch (Exception ex)
		//                            {
		//                                Logger.Error(reporting: typeof(VideoController), ex, message: "Video Details page - Recommended Video");
		//                            }


		//                            sb.Append("</div>");
		//                        }
		//                    }
		//                }

		//                if (vrFilterType == "all")
		//                    sb.Append("<div class='title-fltr'><div><h3> All Videos </h3></div></div>");
		//                else if (vrFilterType == "age")
		//                    sb.Append("<div class='title-fltr'><div><h3>" + Umbraco?.Content(videos?.First()?.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemName + "</h3></div></div>");
		//                else if (vrFilterType == "pathway")
		//                    sb.Append("<div class='title-fltr'><div><h3> " + Umbraco?.Content(videos?.First()?.LearningPathway.Select(y => y.Udi))?.ToList()?.OfType<NameListItem>()?.Where(x => x.ItemValue == FilterValue).FirstOrDefault().ItemName + " </h3></div></div>");


		//                sb.Append("<div class='video-tutorials'>");

		//                try
		//                {
		//                    if ((videos != null && videos.Any()))
		//                    {
		//                        //sb.Append("<h4>" + age.ItemName + "</h4>");
		//                        sb.Append("<div class='recommended-list'>");
		//                        sb.Append("<div class='list-items'>");
		//                        foreach (var videoItems in videos)
		//                        {
		//                            var PDF_File = videoItems?.Value<string>("uploadPDF");
		//                            bitlyLink = videoItems?.Value<string>("bitlyLink");

		//                            //Generate Bitly Link
		//                            //bitlyLink = generateBitlyLink(PDF_File, bitlyLink, WorkSheet.Id, CultureInfo);

		//                            sb.Append("<div class='item-col'>");
		//                            sb.Append("<div class='card-box'>");

		//                            try
		//                            {
		//                                //social integration
		//                                StringBuilder social = new StringBuilder();
		//                                social = VideoSocialShareContent(loggedin, getUserCurrentSubscription, myagegroup, videoItems.FacebookContent, videoItems.WhatsAppContent, videoItems.MailContent, videoItems?.VideoYouTubeId, videoItems?.NextGenImage?.Url(), PDF_File, bitlyLink, videoItems?.AgeTitle.Name == null ? "" : videoItems?.AgeTitle.Name, videoItems?.Title == null ? "" : videoItems?.Title, CultureInfo, videoItems?.ThumbnailImage?.Url(), videoItems?.Description.ToString(), videoItems.IsGuestUserSheet, videoItems.Subscriptions, videoItems.Id, cultureDownloadText, CultureSubscribeforDownload, upgradeToPremiumText, BuyNewSubscription, videoItems?.ThumbnailImage?.Value<string>("altText"), "", "videolisting", vrFilterType);
		//                                sb.Append(social);
		//                            }
		//                            catch (Exception ex)
		//                            {
		//                                Logger.Error(reporting: typeof(VideoController), ex, message: "Video Details page - VideoSocialShareContent");
		//                            }

		//                            sb.Append("</div>");
		//                            sb.Append("</div>");
		//                        }

		//                        sb.Append("</div> ");
		//                        sb.Append("</div> ");
		//                        //load more display till totalcount more than displayed on page
		//                        if (countOfTotalVideos > vrDisplayOnPage)
		//                        {
		//                            sb.Append("<div class='load-more'><a href='javascript:void(0);' onclick='loadMore(" + vrDisplayOnPage + ")'></a></div>");
		//                        }
		//                    }
		//                }
		//                catch (Exception ex)
		//                {
		//                    Logger.Error(reporting: typeof(VideoController), ex, message: "Video Details page - VideoSocialShareContent");
		//                }

		//                sb.Append("</div>");
		//            }

		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        Logger.Error(reporting: typeof(VideoController), ex, message: "Video Details page - VideoDetailsPage");
		//    }

		//    return sb;
		//}

		public JsonResult VideoTrackingView(string filtertype)
		{
			string filter = String.Empty;
			string vrVideoId = String.Empty;
			string vrNodeId = String.Empty;

			dbProxy _db = new dbProxy();
			WatchedVideos watchedVideos = new WatchedVideos();

			string culture = String.Empty;
			culture = CultureName.GetCultureName().Replace("/", "");

			if (!String.IsNullOrEmpty(filtertype) && filtertype != "s=bns")
			{
				string decrypredParameter = Server.UrlDecode(filtertype);
				string[] qAll = decrypredParameter.Split('&');
				if (qAll != null && qAll.Length >= 1)
				{
					if (!String.IsNullOrEmpty(qAll[0]))
					{
						string[] fltArr = qAll[0].Split('=');
						if (fltArr.Length == 2)
							filter = fltArr[1];
					}
					if (!String.IsNullOrEmpty(qAll[1]))
					{
						string[] videoArr = qAll[1].Split('=');
						if (videoArr.Length == 2)
							vrVideoId = videoArr[1];
					}
					if (!String.IsNullOrEmpty(qAll[2]))
					{
						string[] videoidArr = qAll[2].Split('=');
						if (videoidArr.Length == 2)
							vrNodeId = videoidArr[1];
					}
				}
				//if (qAll != null && qAll.Length > 1)
				//{
				//    if (!String.IsNullOrEmpty(qAll[0]))
				//        filter = qAll[0];
				//    if (!String.IsNullOrEmpty(qAll[1]))
				//        vrVideoId = qAll[1];
				//    if (!String.IsNullOrEmpty(qAll[2]))
				//        vrNodeId = qAll[2];
				//}
				
			
				try
				{
					if (!String.IsNullOrEmpty(vrNodeId) && !String.IsNullOrEmpty(vrVideoId))
					{
						int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
						_db = new dbProxy();
						List<SetParameters> sp = new List<SetParameters>()
			{
					new SetParameters{ ParameterName = "@QType", Value = "2" },
					new SetParameters{ ParameterName = "@UniqueId", Value = "0" },
					new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
					new SetParameters{ ParameterName = "@NodeId", Value = vrNodeId.ToString() },
					new SetParameters{ ParameterName = "@VideoPlayId", Value = vrVideoId }
			};

						watchedVideos = _db.GetData<WatchedVideos>("USP_ProcGetRecommendedVideos", watchedVideos, sp);
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(VideoController), ex, message: "Video - VideoTrackingView");
				}
			}

			return Json(watchedVideos, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult VideoTrackingUpdate(int vrUniqueId, string vrPlayDuration, int vrPlayEnd,string VideoSouce = "")
		{
			dbProxy _db = new dbProxy();
			GetStatus insertStatus = new GetStatus();

			try
			{
				if (vrUniqueId > 0)
				{
					List<SetParameters> sp = new List<SetParameters>()
			{
					new SetParameters{ ParameterName = "@UniqueId", Value = vrUniqueId.ToString() },
					new SetParameters{ ParameterName = "@VideosFinished", Value = vrPlayEnd.ToString() },
					new SetParameters{ ParameterName = "@VideoExecutionTimeInMin", Value = vrPlayDuration },
					new SetParameters{ ParameterName = "@VideoSource", Value = VideoSouce }
			};

					insertStatus = _db.GetData<GetStatus>("USP_UpdateVideosWatchedDuration", insertStatus, sp);
					if (insertStatus.returnStatus == "Success")
						return Json(new
						{
							status = insertStatus.returnStatus,
							navigation = "",
							message = vrPlayEnd.ToString()
						}, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "Video - VideoTrackingUpdate");
			}

			return Json(new
			{
				status = "Fail",
				navigation = "",
				message = ""
			}, JsonRequestBehavior.AllowGet);
		}

		//public StringBuilder VideoSocialShareContent(LoggedIn loggedin, List<GetUserCurrentSubscription> getUserCurrentSubscription, List<HPPlc.Models.SelectedAgeGroup> myagegroup, string textFacebook, string textWhatsApp, string textMail, string VideoId, string NextGenUrl, string PDF_File, string bitlyLink, string ageTitle, string ItemName, string CultureInfo, string ThumbnailImage, string Description, bool IsGuestUserSheet, IEnumerable<Link> subscription, int nodeId, string cultureDownloadText, string CultureSubscribeforDownload, string upgradeToPremiumText, string BuyNewSubscription, string altText, string filterValue = "", string sectionType = "", string filterType = "")
		//{
		//    StringBuilder sb = new StringBuilder();
		//    string vUserSubscription = String.Empty;
		//    string playVideoUrl = String.Empty;
		//    string playVideoParam = String.Empty;
		//    string playVideoBitlyUrl = String.Empty;


		//    ////Get All subscription detais for user
		//    GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();


		//    if (loggedin != null)
		//    {
		//        //Get User subscription based on age group
		//        if (loggedin.UserTransactionType == "free")
		//            UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.Ranking == "1" && x.IsActive == 1)?.SingleOrDefault();
		//        else
		//            UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.AgeGroup == ageTitle && x.IsActive == 1)?.SingleOrDefault();
		//    }

		//    string culture = CultureName.GetCultureName();
		//    string subscribeUrl = culture + "/subscription/";
		//    string loginUrl = culture + "/my-account/login/";
		//    playVideoUrl = ConfigurationManager.AppSettings["SiteUrl"].ToString();

		//    string subscriptionRanking = Umbraco.Content(subscription.Select(x => x.Udi))?.ToList().OfType<Subscriptions>().First()?.Ranking.ToString().Trim();

		//    //Check if user have only free subscription and have referral
		//    int tobeDisplayNoOfVolumes = 0;
		//    int tobeStartVolumeDisplay = 0;
		//    int? currentVolume = 0;
		//    if (loggedin != null && loggedin.UserTransactionType == "free" && UserCurrentSubscription.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0)
		//    {
		//        tobeDisplayNoOfVolumes = UserCurrentSubscription.ReferralRewardMonth;
		//        tobeStartVolumeDisplay = 4;
		//        currentVolume = 3;
		//    }

		//    //if video details page
		//    if (sectionType == "videodetails")
		//    {
		//        sb.Append("<div class='card-video'>");
		//        //sb.Append("<iframe src='https://www.youtube-nocookie.com/embed/" + VideoId + "?rel=1&fs=1&autoplay=1&controls=1&modestbranding=1&showinfo=0&title=0&ui-start-screen-info=0&wmode=transparent' title='' frameborder='0' allowfullscreen></iframe>");
		//        sb.Append("<div id='HpPlcvid'></div>");
		//        sb.Append("</div>");
		//    }
		//    else
		//    {
		//        if (filterType == "pathway")
		//            playVideoParam = culture + "videos/play-video?" + clsCommon.Encrypt(HttpUtility.UrlPathEncode(filterType + ":" + VideoId + ":" + nodeId + ":" + filterValue));
		//        else
		//            playVideoParam = culture + "videos/play-video?" + HttpUtility.UrlPathEncode("name=" + ItemName.Replace(" ", "-"));
		//        //playVideoParam = culture + "videos/play-video?" + HttpUtility.UrlPathEncode("type=" + filterType + "&videoid=" + VideoId + "&video=" + nodeId + "&filterid=" + "&age=" + ageTitle + "&name=" + ItemName);
		//        //playVideoParam = culture + "videos/play-video?" + clsCommon.Encrypt(HttpUtility.UrlPathEncode(filterType + ":" + VideoId + ":" + nodeId));

		//        //Generate bitly link for video link share
		//        if (!String.IsNullOrEmpty(playVideoUrl) && !String.IsNullOrEmpty(playVideoParam))
		//        {
		//            GenerateBitlyLink bitLink = new GenerateBitlyLink();
		//            playVideoBitlyUrl = bitLink.Shorten(playVideoUrl + playVideoParam);
		//        }

		//        sb.Append("<div class='card-video video-open'>");

		//        try
		//        {
		//            //Check User Sebcription - if subscribed then can video play other wise display subscribe button
		//            if (subscription != null && subscription.Any())
		//            {
		//                if ((loggedin == null && IsGuestUserSheet == true) ||
		//                //(filterType == "cat" || filterType == "pathway") ||
		//                ((loggedin != null && Umbraco.Content(subscription?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1"))) ||
		//                //((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription.DaysRemaining > 0 && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle))) && Umbraco.Content(subscription?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) ||
		//                ((loggedin != null && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle))) && Umbraco.Content(subscription?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) ||
		//                ((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription != null && UserCurrentSubscription.DaysRemaining > 0 && Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking))) ||
		//                (loggedin != null && loggedin?.UserTransactionType == "free" && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
		//                {
		//                    isplayvideo = true;
		//                    sb.Append("<a data-id='" + VideoId + "' href='" + playVideoUrl + playVideoParam + "'>");
		//                    sb.Append("<span class='play-btn'></span>");
		//                    sb.Append("</a>");
		//                }
		//                else
		//                {
		//                    isplayvideo = false;
		//                    //string subscriptionUrl = culture + "/subscription?subscptn=" + clsCommon.encrypto(Umbraco.Content(subscription.Select(x => x.Udi))?.ToList().OfType<Subscriptions>().First()?.Ranking.ToString().Trim()) + "&age=" + clsCommon.encrypto(ageTitle);
		//                    string subscriptionUrl = culture + "/subscription?subscptn=" + clsCommon.Encrypt(subscriptionRanking) + "&age=" + clsCommon.Encrypt(ageTitle);
		//                    sb.Append("<a href ='" + subscriptionUrl + "'><span class='play-btn'></span></a>");
		//                }
		//            }
		//        }
		//        catch { }


		//        //Check - if image have in cms then display otherwise thumnail display from youTube
		//        if (String.IsNullOrEmpty(ThumbnailImage))
		//        {
		//            sb.Append("<img src='https://img.youtube.com/vi/" + VideoId + "/hqdefault.jpg' alt=''>");
		//        }
		//        else
		//        {
		//            //sb.Append("<img class='b-lazy' alt='"+ altText + "' src='/common/images/img-loader.gif' data-src='" + ThumbnailImage + "' alt=''>");

		//            sb.Append("<picture>");
		//            if (!String.IsNullOrEmpty(NextGenUrl))
		//            {
		//                sb.Append("<source srcset = '" + NextGenUrl + "' type='image/webp'>");
		//            }
		//            sb.Append("<img alt='" + altText + "' src='" + ThumbnailImage + "'>");
		//            sb.Append("</picture>");
		//        }

		//        //sb.Append("</a>");
		//        sb.Append("</div>");
		//    }

		//    sb.Append("<div class='card-cont'>");
		//    sb.Append("<div class='card-dscptn'>");
		//    if (!String.IsNullOrEmpty(ItemName))
		//    {
		//        sb.Append("<div class='card-title'>" + ItemName + "</div>");
		//    }
		//    sb.Append(Description);
		//    sb.Append("</div>");



		//    try
		//    {
		//        //Share Icons activation according to subscription
		//        if (subscription != null && subscription.Any())
		//        {
		//            if ((loggedin == null && IsGuestUserSheet == true) ||
		//                //(filterType == "cat" || filterType == "pathway") ||
		//                //((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription.DaysRemaining > 0 && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle))) && Umbraco.Content(subscription?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) ||
		//                ((loggedin != null && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle))) && Umbraco.Content(subscription?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) ||
		//                ((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription != null && UserCurrentSubscription.DaysRemaining > 0 && Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking))) ||
		//                (loggedin != null && loggedin?.UserTransactionType == "free" && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
		//            {
		//                if (!String.IsNullOrEmpty(textFacebook) || !String.IsNullOrEmpty(textWhatsApp) || !String.IsNullOrEmpty(textMail))
		//                {
		//                    sb.Append("<div class='card-social-icon'>");
		//                    sb.Append("<div class='share-icon'>");
		//                    sb.Append("<div class='share-box'>");
		//                    sb.Append("<ul>");

		//                    if (!String.IsNullOrEmpty(textFacebook))
		//                        sb.Append(" <li><a href='javascript:void(0)' class='fb-icon aFBShare'><span style='display:none'>" + textFacebook + "</span></a></li>");

		//                    if (!String.IsNullOrEmpty(textWhatsApp))
		//                        sb.Append(" <li><a href='javascript:void(0)' class='whatsapp-icon aWHTAppSH'><span style='display:none'>" + textWhatsApp + "</span></a></li>");

		//                    if (!String.IsNullOrEmpty(textMail))
		//                        sb.Append(" <li><a href='javascript:void(0)' class='mail-icon aMailSh'><span style='display:none'>" + textMail + " " + "\n\n" + "`" + ItemName + "</span></a></li>");

		//                    sb.Append("</ul>");
		//                    sb.Append("</div>");
		//                    sb.Append("</div>");
		//                    sb.Append("</div>");

		//                    //sb.Append("<div class='eprint-icon'>");
		//                    //sb.Append("<a href='javascript:void(0)' class='aEPrint'><span style='display:none' id='spnfile'>" + PDF_File + "</span><span style='display:none' id='wid'>" + nodeId + "</span><span style='display:none' id='itemname'>" + ItemName + "</span></a>");
		//                    //sb.Append("</div>");
		//                }
		//                //}
		//            }
		//        }
		//    }
		//    catch { }


		//    sb.Append("</div>");

		//    if (sectionType == "videolisting" || sectionType == "videorecommended" || sectionType == "videodetails")
		//    {
		//        string DownloadString = String.Empty;
		//        if (loggedin == null && IsGuestUserSheet == false)
		//            DownloadString = CultureInfo + "$" + ItemName + "$" + nodeId + "$" + PDF_File + "$" + "Print";
		//        else if (loggedin == null && IsGuestUserSheet == true)
		//            DownloadString = CultureInfo + "$" + "0" + "$" + ItemName + "$" + nodeId + "$" + PDF_File + "$" + "Print";
		//        else
		//            DownloadString = CultureInfo + "$" + loggedin.UserId.ToString() + "$" + ItemName + "$" + nodeId + "$" + PDF_File + "$" + "Print";


		//        sb.Append("<div class='card-btn-print'>");
		//        //sb.Append("<div class='btn-col'><a href='#' class='btn'>Download all Worksheets<span class='download-icon'></span></a></div>");

		//        try
		//        {
		//            if (subscription != null && subscription.Any())
		//            {
		//                if (culture == "/")
		//                    culture = String.Empty;

		//                //string subscriptionRanking = Umbraco.Content(subscription.Select(x => x.Udi))?.ToList().OfType<Subscriptions>().First()?.Ranking.ToString().Trim();
		//                //if (subscriptionRanking == "1")
		//                //	subscriptionRanking = "3";
		//                int maxSubscriptionRanking = int.Parse(Umbraco?.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Max(m => m.Ranking));
		//                string subscriptionUrl = culture + "/subscription?subscptn=" + clsCommon.encrypto(subscriptionRanking) + "&age=" + clsCommon.encrypto(ageTitle);
		//                string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(nodeId.ToString()) + "&source=Video";

		//                if (!String.IsNullOrEmpty(ageTitle))
		//                {
		//                    //1. if user not loggedin and worksheet mapped with isguestuser true
		//                    if (loggedin == null && IsGuestUserSheet == true)
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + downloadUrl + "' class='btn downllink'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                        //sb.Append("<a href='javascript:void(0)' onclick=DownloadData('" + PDF_File + "','" + worksheetid + "','" + ageTitle + "','Worksheet') class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");

		//                        sb.Append("<div class='print-col'><a class='clsPrintDoc'><span style='display:none;'>" + DownloadString + "</span></a></div>");
		//                    }
		//                    //2. if user not loggedin and worksheet not mapped with isguestuser true
		//                    else if (loggedin == null && IsGuestUserSheet == false)
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + subscriptionUrl + "' class='btn subs-downllink'>" + CultureSubscribeforDownload + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");
		//                    }
		//                    // 3.1 if User have free subscription and have referral
		//                    else if (loggedin != null && loggedin?.UserTransactionType == "free" && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle) && UserCurrentSubscription.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + downloadUrl + "' class='btn downllink'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                        //sb.Append("<a href='javascript:void(0)' onclick=DownloadData('" + PDF_File + "','" + worksheetid + "','" + ageTitle + "','Worksheet') class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");

		//                        sb.Append("<div class='print-col'><a class='clsPrintDoc'><span style='display:none;'>" + DownloadString + "</span></a></div>");
		//                    }
		//                    // 3.2 if User have loggedin and age group is same with worksheet and worksheet is free
		//                    else if (loggedin != null && loggedin?.UserTransactionType == "paid" && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle) && Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")))
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + downloadUrl + "' class='btn downllink'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                        //sb.Append("<a href='javascript:void(0)' onclick=DownloadData('" + PDF_File + "','" + worksheetid + "','" + ageTitle + "','Worksheet') class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");

		//                        sb.Append("<div class='print-col'><a class='clsPrintDoc'><span style='display:none;'>" + DownloadString + "</span></a></div>");
		//                    }
		//                    else if (loggedin != null && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle) && Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) == Convert.ToInt32(o.Ranking))))
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + downloadUrl + "' class='btn downllink'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                        //sb.Append("<a href='javascript:void(0)' onclick=DownloadData('" + PDF_File + "','" + worksheetid + "','" + ageTitle + "','Worksheet') class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");

		//                        sb.Append("<div class='print-col'><a class='clsPrintDoc'><span style='display:none;'>" + DownloadString + "</span></a></div>");
		//                    }

		//                    else if (loggedin != null && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle) && Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) < Convert.ToInt32(o.Ranking))))
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + subscriptionUrl + "' class='btn subs-downllink'>" + upgradeToPremiumText + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");
		//                    }
		//                    // 4.1 if User have loggedin and age group is not same with worksheet and user is free
		//                    else if (loggedin != null && loggedin?.UserTransactionType == "free" && !String.IsNullOrEmpty(ageTitle) && !(myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle)))
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + subscriptionUrl + "' class='btn subs-downllink'>" + BuyNewSubscription + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");
		//                    }
		//                    // 4.2 if User have loggedin and age group is same with worksheet and user have paid
		//                    //else if (loggedin != null && loggedin?.UserTransactionType == "paid" && (UserCurrentSubscription != null && UserCurrentSubscription?.DaysRemaining >= 1) && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle)))
		//                    //{
		//                    //	sb.Append("<div class='btn-col btn-ful'>");
		//                    //	sb.Append("<a href='" + downloadUrl + "' class='btn downllink'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");

		//                    //	sb.Append("<div class='print-col'><a class='clsPrintDoc'><span style='display:none;'>" + DownloadString + "</span></a></div>");
		//                    //}
		//                    // 5. if User have loggedin and age group is not same with worksheet and user have paid
		//                    //else if (loggedin != null && loggedin?.UserTransactionType == "paid" && (UserCurrentSubscription != null && UserCurrentSubscription?.DaysRemaining == 0) && !String.IsNullOrEmpty(ageTitle))
		//                    //{
		//                    //	sb.Append("<div class='btn-col btn-ful'>");
		//                    //	sb.Append("<a href='" + subscribeUrl + "' class='btn subs-downllink'>" + upgradeToPremiumText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");

		//                    //}
		//                    //6. if User have loggedin and age group is not same with worksheet and worksheet is paid
		//                    else if (loggedin != null && loggedin?.UserTransactionType == "paid" && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && !myagegroup.Any(x => x.AgeGroup == ageTitle)))
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + subscribeUrl + "' class='btn subs-downllink'>" + upgradeToPremiumText + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");
		//                    }
		//                    //6. In case User purchased subscription for same age group and subscription type have same or upper
		//                    //else if (loggedin != null && (loggedin?.UserTransactionType == "paid" && UserCurrentSubscription != null && !String.IsNullOrWhiteSpace(UserCurrentSubscription.Ranking) && Convert.ToInt32(UserCurrentSubscription?.Ranking) > 1) && (!String.IsNullOrEmpty(ageTitle) && UserCurrentSubscription.AgeGroup == ageTitle) && (Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking) || Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) >= Convert.ToInt32(o.Ranking))) && UserCurrentSubscription.DaysRemaining >= 1)
		//                    //{
		//                    //	sb.Append("<div class='btn-col btn-ful'>");
		//                    //	sb.Append("<a href='" + downloadUrl + "' class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	//sb.Append("<a href='javascript:void(0)' onclick=DownloadData('" + PDF_File + "','" + worksheetid + "','" + ageTitle + "','Worksheet') class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");

		//                    //	sb.Append("<div class='print-col'><a class='clsPrintDoc'><span style='display:none;'>" + DownloadString + "</span></a></div>");
		//                    //}
		//                    //7. In case User purchased subscription for same age group and subscription type have lower
		//                    //else if ((loggedin != null && (UserCurrentSubscription != null && UserCurrentSubscription?.DaysRemaining >= 1) && loggedin?.UserTransactionType == "paid" && (!String.IsNullOrWhiteSpace(UserCurrentSubscription?.Ranking) && Convert.ToInt32(UserCurrentSubscription?.Ranking) > 1) && (!String.IsNullOrEmpty(ageTitle) && UserCurrentSubscription.AgeGroup == ageTitle) || Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) < Convert.ToInt32(o.Ranking))))
		//                    //{
		//                    //	sb.Append("<div class='btn-col btn-ful'>");
		//                    //	sb.Append("<a href='" + subscriptionUrl + "' class='btn'>" + upgradeToPremiumText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");
		//                    //}
		//                    else
		//                    {
		//                        sb.Append("<div class='btn-col btn-ful'>");
		//                        sb.Append("<a href='" + subscriptionUrl + "' class='btn subs-downllink'>" + BuyNewSubscription + "<span class='download-icon'></span></a>");
		//                        sb.Append("</div>");
		//                    }

		//                    //// if User have free subscription and have referral
		//                    //if (loggedin != null && loggedin.UserTransactionType == "free" && UserCurrentSubscription.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1)))
		//                    //{
		//                    //	sb.Append("<div class='btn-col'>");
		//                    //	//sb.Append("<a href='/umbraco/Surface/WorkSheet/DownloadData?url=" + PDF_File + "&UserId=0&WID=" + nodeId + "&CultrInfo=" + CultureInfo + "&Age=" + ItemName + "' class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("<a href='javascript:void(0)' onclick=DownloadData('" + PDF_File + "','" + nodeId + "','" + ageTitle + "','Video') class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");
		//                    //}
		//                    ////In case user have paid and age group matched display free subscription or have guest user 
		//                    //else if ((loggedin != null && !String.IsNullOrEmpty(ageTitle) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == ageTitle)) && Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) || (loggedin == null && IsGuestUserSheet == true))
		//                    //{
		//                    //	sb.Append("<div class='btn-col'>");
		//                    //	//sb.Append("<a href='/umbraco/Surface/WorkSheet/DownloadData?url=" + PDF_File + "&UserId=0&WID=" + nodeId + "&CultrInfo=" + CultureInfo + "&Age=" + ItemName + "' class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("<a href='javascript:void(0)' onclick=DownloadData('" + PDF_File + "','" + nodeId + "','" + ageTitle + "','Video') class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");
		//                    //}
		//                    ////In case user have loggedIn and subscription purchased
		//                    ////In case User purchased subscription for same age group and subscription type have same or upper
		//                    //else if (loggedin != null && (UserCurrentSubscription != null && !String.IsNullOrWhiteSpace(UserCurrentSubscription.Ranking) && Convert.ToInt32(UserCurrentSubscription?.Ranking) > 1) && (!String.IsNullOrEmpty(ageTitle) && UserCurrentSubscription.AgeGroup == ageTitle) && (Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking) || Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) >= Convert.ToInt32(o.Ranking))) && UserCurrentSubscription.DaysRemaining >= 1)
		//                    //{
		//                    //	sb.Append("<div class='btn-col'>");
		//                    //	//sb.Append("<a href='/umbraco/Surface/WorkSheet/DownloadData?url=" + PDF_File + "&UserId=0&WID=" + nodeId + "&CultrInfo=" + CultureInfo + "&Age=" + ItemName + "' class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("<a href='javascript:void(0)' onclick=DownloadData('" + PDF_File + "','" + nodeId + "','" + ageTitle + "','Video') class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");
		//                    //}
		//                    ////In case User purchased subscription for same age group and subscription type have lower
		//                    //else if ((loggedin != null && UserCurrentSubscription?.DaysRemaining >= 1) && ((UserCurrentSubscription != null && !String.IsNullOrWhiteSpace(UserCurrentSubscription?.Ranking) && Convert.ToInt32(UserCurrentSubscription?.Ranking) > 1) && (!String.IsNullOrEmpty(ageTitle) && UserCurrentSubscription.AgeGroup == ageTitle) || Umbraco.Content(subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) < Convert.ToInt32(o.Ranking))))
		//                    //{
		//                    //	sb.Append("<div class='btn-col'>");
		//                    //	sb.Append("<a href='" + subscriptionUrl + "' class='btn'>" + CultureSubscribeforDownload + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");
		//                    //}
		//                    ////In case User not purchased subscription for this age group
		//                    //else if (loggedin != null && (UserCurrentSubscription != null && !String.IsNullOrWhiteSpace(UserCurrentSubscription.Ranking) && Convert.ToInt32(UserCurrentSubscription?.Ranking) > 1) && !String.IsNullOrEmpty(ageTitle) && (UserCurrentSubscription.AgeGroup != ageTitle))
		//                    //{
		//                    //	sb.Append("<div class='btn-col'>");
		//                    //	sb.Append("<a href='" + subscriptionUrl + "' class='btn'>" + CultureSubscribeforDownload + "<span class='download-icon'></span></a>");
		//                    //	sb.Append("</div>");
		//                    //}
		//                }
		//                //else if (filterType == "pathway")
		//                //{
		//                //	sb.Append("<div class='btn-col'>");
		//                //	//sb.Append("<a href='/umbraco/Surface/WorkSheet/DownloadData?url=" + PDF_File + "&UserId=0" + "&WID=" + nodeId + "&CultrInfo=" + CultureInfo + "&Age=" + ItemName + "' class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                //	sb.Append("<a href='" + downloadUrl + "' class='btn'>" + cultureDownloadText + "<span class='download-icon'></span></a>");
		//                //	sb.Append("</div>");
		//                //}
		//            }
		//        }
		//        catch { }
		//        sb.Append("</div>");
		//    }
		//    else
		//    {
		//        sb.Append("<div class='card-btn-print'>");
		//        sb.Append("</div>");
		//    }

		//    return sb;
		//}

		#region Videos Start
		[HttpPost]
		public ActionResult GetVideosList(VideosInput input)
		{
			Responce responce = new Responce();
			try
			{
				//find registered age group
				dbAccessClass db = new dbAccessClass();
				List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
				myagegroup = db.GetUserSelectedUserGroup();

				VideosModel model = new VideosModel();
				model = GetVideosListData(input);

				if (myagegroup != null && myagegroup.Any())
				{
					model.Videos = model.Videos.OrderByDescending(lst => myagegroup.Any(y => y.AgeGroup == lst.AgeValue)).ToList();
				}

				return PartialView("/Views/Partials/Videos/_videosList.cshtml", model);
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosList");
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();
			}
			return Json(responce, JsonRequestBehavior.AllowGet);
		}
		public VideosModel GetVideosListData(VideosInput input)
		{
			VideosModel model = new VideosModel();
			List<VideosItems> VideosItems = new List<VideosItems>();
			try
			{
				string vUserSubscription = String.Empty;
				string bitlyLink = String.Empty;
				StringBuilder sb = new StringBuilder();
				StringBuilder recommendVideos = new StringBuilder();
				sb.Clear();
				recommendVideos.Clear();

				//Check here user subscription
				LoggedIn loggedin = new LoggedIn();
				loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

				//Get All subscription detais for user
				GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
				List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
				getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

				//find registered age group
				dbAccessClass db = new dbAccessClass();
				List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
				myagegroup = db.GetUserSelectedUserGroup();

				try
				{
					string culture = CultureName.GetCultureName();
					string subscribeUrl = culture == "/" ? culture + "subscription/" : culture + "/subscription/";

					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
					//var VideoRoot = Umbraco?.Content(input.CurrentNode)?.DescendantsOrSelf()?.OfType<Videos>().FirstOrDefault();

					var VideoRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
													.Where(x => x.ContentType.Alias == "videos")?.OfType<Videos>().FirstOrDefault();

					if (VideoRoot != null)
					{
						var mediaUrl = VideoRoot?.Value<IPublishedContent>("seeMoreMedia");
						var nextGenMediaUrl = VideoRoot?.SeeMoreNextGen;
						int firstTimeDisplayVideos = VideoRoot.FirstTimeDisplayVideos;

						try
						{
							//Age Wise
							var allAges = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home").FirstOrDefault()?
							.Children?.Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?
							.Children?.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>()?.Where(x => x.IsActice);

							if (allAges != null)
							{
								if (input.selectedAgeGroup != null && !string.IsNullOrWhiteSpace(input.selectedAgeGroup) && allAges != null)
								{
									allAges = allAges.Where(x => input.selectedAgeGroup.Contains(x.Name));
								}
								IEnumerable<Videos> videosRoot = new List<Videos>();
								foreach (var age in allAges)
								{
									VideosItems videosItems = new VideosItems();

									//var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault()?
									//        .Children?.OfType<Video>()?.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue).Take(firstTimeDisplayVideos);

									var VideosAgeGroup = VideoRoot?.DescendantsOrSelf()?
																		.Where(x => x.ContentType.Alias == "videoListingAgeWise")
																		.OfType<VideoListingAgeWise>().ToList();

									//if (input.selectedVolume != null && !string.IsNullOrWhiteSpace(input.selectedVolume) && Videos != null && Videos.Any())
									//{
									//    Videos = Videos.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue && ContainsInt(input.selectedVolume, Convert.ToInt32(Umbraco?.Content(x.SelectVolume?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue))).Take(firstTimeDisplayVideos);
									//    SessionManagement.StoreInSession(SessionType.UserSelectVolumeOnVidoe, input.selectedVolume);
									//}
									//if (input.selectedCategory != null && !string.IsNullOrWhiteSpace(input.selectedCategory) && Videos != null && Videos.Any())
									//{
									//    Videos = Videos.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue && (x.Category != null && x.Category.Count() > 0) ? x.Category.Where(y => y.Name.Contains(input.selectedCategory)).Count() > 0 : true).Take(firstTimeDisplayVideos);
									//}

									if (VideosAgeGroup != null)
									{
										var Videos = VideosAgeGroup?.Where(x => x.AgeGroup.Name == age.ItemValue).FirstOrDefault()
											?.Children<Video>().Where(x => x.IsActive);

										if (Videos != null && Videos.Any())
										{
											videosItems.VideoTitle = VideosAgeGroup?.Where(x => x.AgeGroup.Name == age.ItemValue)?.FirstOrDefault()?.Title;
											videosItems.AgeValue = age.ItemValue;

											List<NestedItems> nesteds = new List<NestedItems>();
											string videoPageURL = string.Empty;
											foreach (var videoItems in Videos)
											{
												NestedItems nested = new NestedItems();
												nested.Title = videoItems?.Title;
												nested.Description = videoItems?.Description;
												nested.Age = videoItems.AgeTitle?.Name;
												nested.Category = string.Join(",", videoItems.Category?.Select(x => x.Name).ToList());
												nested.Volume = videoItems.SelectVolume?.Name;
												nested.VideoPreviewId = videoItems?.VideoPreviewId;
												nested.VimeoURL = videoItems?.VimeoUrl;

												//VimeoResponse vimeoResponse = new VimeoResponse();
												//vimeoResponse = GetVideoUrl(videoItems.VideoPreviewId.ToString());
												//string url = task.Result;

												try
												{
													//social integration
													WorksheetVideosHelper videosHelper = new WorksheetVideosHelper();
													nested = videosHelper.GetSocialItemsAndSubscriptionDetailsForVideos(videoItems, input, nested);
												}
												catch { }

												nesteds.Add(nested);
												videoPageURL = videoItems.Parent.Url().ToString();
											}
											videosItems.NestedItems = nesteds;
											//See More
											SeeMore seeMore = new SeeMore();
											//seeMore.VideoDetailsUrl = culture + "/videos/video-details?type=age&typeid=" + clsCommon.encrypto(age.ItemValue.ToString());
											seeMore.VideoDetailsUrl = videoPageURL;
											if (mediaUrl != null)
											{
												if (nextGenMediaUrl != null)
													seeMore.NextGenMediaUrl = nextGenMediaUrl.Url();

												seeMore.MediaUrl = mediaUrl.Url();
											}

											videosItems.SeeMore = seeMore;
										}

									}

									VideosItems.Add(videosItems);
								}
								VideosItems.Add(GetRecommendedVideos(input, firstTimeDisplayVideos));
								model.Videos = VideosItems;

							}
						}
						catch (Exception ex)
						{
							Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosListData");
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosListData");
				}

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosListData");

			}
			return model;
		}
		public bool ContainsInt(string str, int value)
		{
			bool Result = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => int.Parse(x.Trim()))
				.Contains(value);
			return Result;
		}
		public VideosItems GetRecommendedVideos(VideosInput input, int firstTimeDisplayVideos)
		{
			VideosItems RecommendedVideos = new VideosItems();
			StringBuilder sb = new StringBuilder();
			string bitlyLink = String.Empty;
			string culture = CultureName.GetCultureName().Replace("/", "");

			//Check here user subscription
			LoggedIn loggedin = new LoggedIn();
			loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

			//Get All subscription detais for user
			GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
			List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
			getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

			//find registered age group
			dbAccessClass db = new dbAccessClass();
			List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
			myagegroup = db.GetUserSelectedUserGroup();

			try
			{
				List<RecommendedVideos> recommendedVideos = new List<RecommendedVideos>();
				recommendedVideos = GetRecommendedVideosDB(1, "age", firstTimeDisplayVideos);

				if (recommendedVideos != null && recommendedVideos.Any())
				{
					int[] nodes = new int[0];
					for (int i = 0; i < recommendedVideos.Count; i++)
					{
						nodes = nodes.Concat(new int[] { recommendedVideos[i].NodeId }).ToArray();
					}

					//string culture = CultureName.GetCultureName().Replace("/", "");
					if (String.IsNullOrWhiteSpace(culture))
						culture = "en-US";

					_variationContextAccessor.VariationContext = new VariationContext(culture);

					var videos = Umbraco?.Content(nodes).OfType<Video>();
					if (videos != null && videos.Any())
					{
						if (input != null && input.selectedAgeGroup != null && !string.IsNullOrWhiteSpace(input.selectedAgeGroup))
						{
							videos = videos.OfType<Video>()?.Where(x => input.selectedAgeGroup.Contains(x.AgeTitle?.Name));
						}
						if (input.selectedVolume != null && !string.IsNullOrWhiteSpace(input.selectedVolume) && videos != null && videos.Any())
						{
							videos = videos.OfType<Video>()?.Where(x => ContainsInt(input.selectedVolume, Convert.ToInt32(Umbraco?.Content(x.SelectVolume?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue)));

						}
						if (input.selectedCategory != null && !string.IsNullOrWhiteSpace(input.selectedCategory) && videos != null && videos.Any())
						{
							videos = videos.OfType<Video>()?.Where(x => ContainsInt(input.selectedCategory, Convert.ToInt32(Umbraco?.Content(x.Category.FirstOrDefault()?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue)));
						}
						//string seeMoreContent = Umbraco.GetDictionaryValue("See More");
						var mediaUrl = videos?.FirstOrDefault()?.Parent?.Parent?.Value<IPublishedContent>("seeMoreMedia");
						var nextGenMediaUrl = videos?.FirstOrDefault()?.Parent?.Parent?.Value<IPublishedContent>("seeMoreNextGen");
						string recommendedVideoTitle = videos?.FirstOrDefault()?.Parent?.Parent?.Value<string>("recommendedVideosTitle");

						RecommendedVideos.Title = recommendedVideoTitle;
						RecommendedVideos.VideoTitle = recommendedVideoTitle;
						List<NestedItems> nesteds = new List<NestedItems>();
						foreach (var videoItems in videos?.OfType<Video>().Where(x => x.AgeTitle != null).Take(firstTimeDisplayVideos))
						{
							NestedItems nested = new NestedItems();
							nested.Title = videoItems?.AgeTitle?.Name + " Years" + "-" + videoItems?.Title;
							RecommendedVideos.AgeValue = videoItems?.AgeTitle?.Name + " Years";
							//nested.Age = videoItems?.AgeTitle?.Name + " Years";
							nested.Description = videoItems?.Description;
							nested.VideoPreviewId = videoItems?.VideoPreviewId;
							nested.VimeoURL = videoItems?.VimeoUrl;

							try
							{
								//social integration
								WorksheetVideosHelper videosHelper = new WorksheetVideosHelper();
								nested = videosHelper.GetSocialItemsAndSubscriptionDetailsForVideos(videoItems, input, nested);

							}
							catch (Exception ex)
							{
								Logger.Error(reporting: typeof(VideoController), ex, message: "GetRecommendedVideos-GetSocialItemsAndSubscriptionDetailsForVideos");

							}
							nesteds.Add(nested);

						}
						//See More
						RecommendedVideos.NestedItems = nesteds;
						//See More
						SeeMore seeMore = new SeeMore();
						seeMore.VideoDetailsUrl = culture + "/videos/video-details?type=rcmd&typeid=" + clsCommon.encrypto("0");
						if (mediaUrl != null)
						{
							if (nextGenMediaUrl != null)
								seeMore.NextGenMediaUrl = nextGenMediaUrl.Url();
							seeMore.MediaUrl = mediaUrl.Url();
						}

						RecommendedVideos.SeeMore = seeMore;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "GetRecommendedVideos");
			}

			return RecommendedVideos;
		}
		[HttpPost]
		public ActionResult GetVideosById(VideosInput input)
		{

			Responce responce = new Responce();
			try
			{
				VideosItems model = new VideosItems();
				model = GetVideosData(input);
				return PartialView("/Views/Partials/Videos/_videosDetails.cshtml", model);
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosById");
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}
		public VideosItems GetVideosData(VideosInput input)
		{
			VideosItems videosItems = new VideosItems();
			string vUserSubscription = String.Empty;
			string recommendedTitle = String.Empty;
			string bitlyLink = String.Empty;
			int? countOfTotalVideos = 0;
			input.selectedVolume = SessionManagement.GetCurrentSession<string>(SessionType.UserSelectVolumeOnVidoe);
			input.selectedCategory = SessionManagement.GetCurrentSession<string>(SessionType.UserSelectCategoryOnWorksVidoe);

			//Check here user subscription
			LoggedIn loggedin = new LoggedIn();
			loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

			//Get All subscription detais for user
			GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
			List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
			getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

			//find registered age group
			dbAccessClass db = new dbAccessClass();
			List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
			myagegroup = db.GetUserSelectedUserGroup();

			try
			{
				string culture = CultureName.GetCultureName();
				string subscribeUrl = culture == "/" ? culture + "subscription/" : culture + "/subscription/";

				if (!String.IsNullOrEmpty(input.FilterType) && !String.IsNullOrEmpty(input.FilterType))
				{
					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
					//var videoRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
					//                            .Where(x => x.ContentType.Alias == "videos")?.FirstOrDefault()?.Children?
					//                            .Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault();
					var videoRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
													.Where(x => x.ContentType.Alias == "videos")?.OfType<Videos>().FirstOrDefault();
					if (videoRoot != null)
					{
						//load more 
						int loadCount = videoRoot.Value<int>("firstTimeDisplayVideosOnDetails");
						if (input.DisplayCount == 0)
							input.DisplayCount = loadCount;
						else
						{
							input.DisplayCount += loadCount;
						}

						//string FilterValue = clsCommon.Decrypto(input.FilterId);
						string FilterValue = input.FilterId;
						IEnumerable<Video> videos = null;
						int[] nodes = new int[0];


						//All
						if (input.FilterType == "age")
						{
							countOfTotalVideos = videoRoot?.Children?.OfType<Video>()?
								.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == FilterValue)?.Count();

							var VideosAgeGroup = videoRoot?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "videoListingAgeWise")
									.OfType<VideoListingAgeWise>().ToList();

							videos = VideosAgeGroup?.Where(x => x.AgeGroup.Name == FilterValue).FirstOrDefault()
											?.Children<Video>().Where(x => x.IsActive);

							//videos = videoRoot?.Children?.OfType<Video>()?
							//    .Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == FilterValue).Take(input.DisplayCount);
							//if (input.selectedVolume != null && !string.IsNullOrWhiteSpace(input.selectedVolume) && videos != null && videos.Any())
							//{
							//    countOfTotalVideos = videos.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == FilterValue && ContainsInt(input.selectedVolume, Convert.ToInt32(Umbraco?.Content(x.SelectVolume?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue))).Count();
							//    videos = videos.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == FilterValue && ContainsInt(input.selectedVolume, Convert.ToInt32(Umbraco?.Content(x.SelectVolume?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue))).Take(input.DisplayCount);
							//}
							//if (input.selectedCategory != null && !string.IsNullOrWhiteSpace(input.selectedCategory) && videos != null && videos.Any())
							//{
							//    countOfTotalVideos = videos.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == FilterValue && x.Category != null && x.Category.FirstOrDefault() != null && x.Category.FirstOrDefault().Udi != null && input.selectedCategory.Contains(Umbraco?.Content(x.Category?.FirstOrDefault().Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue)).Count();
							//    videos = videos.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == FilterValue && x.Category != null && x.Category.FirstOrDefault() != null && x.Category.FirstOrDefault().Udi != null && input.selectedCategory.Contains(Umbraco?.Content(x.Category?.FirstOrDefault().Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue)).Take(input.DisplayCount);
							//}

							try
							{
								if ((videos != null && videos.Any()))
								{
									videosItems.VideoTitle = VideosAgeGroup?.Where(x => x.AgeGroup?.Name == input?.FilterId)?.FirstOrDefault()?.Title;
									videosItems.Title = videosItems.VideoTitle;//VideosAgeGroup.FirstOrDefault().Title; //Umbraco?.Content(videos.FirstOrDefault().AgeTitle.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault(x => x.ItemValue == FilterValue)?.ItemName;
									videosItems.Description = VideosAgeGroup.FirstOrDefault().Description;
									videosItems.ReadMore = videoRoot.ReadMore;
									videosItems.AgeValue = VideosAgeGroup?.FirstOrDefault()?.AgeGroup.Name + " Years";
									List<NestedItems> nesteds = new List<NestedItems>();

									foreach (var videoItems in videos)
									{

										NestedItems nested = new NestedItems();
										nested.Title = videoItems.Title;
										nested.Description = videoItems.Description;
										nested.Category = string.Join(",", videoItems.Category?.Select(x => x.Name).ToList());

										nested.VideoPreviewId = videoItems?.VideoPreviewId;
										nested.VimeoURL = videoItems?.VimeoUrl;
										try
										{
											//social integration
											WorksheetVideosHelper videosHelper = new WorksheetVideosHelper();
											nested = videosHelper.GetSocialItemsAndSubscriptionDetailsForVideos(videoItems, input, nested);
										}
										catch { }
										nesteds.Add(nested);

									}
									videosItems.NestedItems = nesteds;
									if (countOfTotalVideos > input.DisplayCount)
									{
										videosItems.LoadMore = input.DisplayCount;
									}
								}
							}
							catch (Exception ex)
							{
								Logger.Error(reporting: typeof(VideoController), ex, message: "Video Details page -GetVideosData- GetSocialItemsAndSubscriptionDetailsForVideos");
							}
						}
						else if (input.FilterType == "rcmd")
						{
							List<RecommendedVideos> recommendedVideos = new List<RecommendedVideos>();
							recommendedVideos = GetRecommendedVideosDB(1, "age", input.DisplayCount);

							if (recommendedVideos != null && recommendedVideos.Any())
							{
								for (int i = 0; i < recommendedVideos.Count; i++)
								{
									nodes = nodes.Concat(new int[] { recommendedVideos[i].NodeId }).ToArray();
								}

								var rcmdVideos = Umbraco?.Content(nodes);
								if (rcmdVideos != null && rcmdVideos.Any())
								{
									countOfTotalVideos = recommendedVideos?.First()?.VideoCount;
									videosItems.Title = rcmdVideos?.First()?.Parent?.Parent?.Value<string>("recommendedVideosTitle");

									try
									{
										if (rcmdVideos != null && rcmdVideos.Any())
										{
											List<NestedItems> nesteds = new List<NestedItems>();
											foreach (var videoItems in rcmdVideos.OfType<Video>())
											{
												NestedItems nested = new NestedItems();
												//nested.Title = videoItems.Title;
												nested.Title = videoItems?.AgeTitle?.Name + " Years" + "-" + videoItems?.Title;
												nested.Description = videoItems.Description;
												nested.Category = string.Join(",", videoItems.Category?.Select(x => x.Name).ToList());
												try
												{
													//social integration
													WorksheetVideosHelper videosHelper = new WorksheetVideosHelper();
													nested = videosHelper.GetSocialItemsAndSubscriptionDetailsForVideos(videoItems, input, nested);
												}
												catch { }
												nesteds.Add(nested);
											}
											videosItems.NestedItems = nesteds;


											if (countOfTotalVideos > input.DisplayCount)
											{
												videosItems.LoadMore = input.DisplayCount;
											}
										}
									}
									catch (Exception ex)
									{
										Logger.Error(reporting: typeof(VideoController), ex, message: "Video Details page-GetVideosData - Recommended Video");
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "Video Details page - GetVideosData");
			}

			return videosItems;
		}

		[HttpGet]
		public ActionResult GetAgeGroupList(string CurrentNode)
		{
			Responce responce = new Responce();
			try
			{
				string culture = String.Empty;
				culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(culture);

				var vrAgrGroupe = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
					  .Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>().ToList().Where(c => c.IsActice == true);
				if (vrAgrGroupe != null && vrAgrGroupe.Any())
				{
					List<WorkSheetVideosFilter> List = new List<WorkSheetVideosFilter>();
					var VideoRoot = Umbraco?.Content(CurrentNode)?.DescendantsOrSelf()?.OfType<Videos>().FirstOrDefault();

					if (VideoRoot != null)
					{
						foreach (var age in vrAgrGroupe)
						{
							_variationContextAccessor.VariationContext = new VariationContext(culture);

							var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListingAgeWise")?.OfType<VideoListingAgeWise>()?
								.Where(x=> x.AgeGroup.Name == age?.ItemValue)?.Count();
							if (Videos > 0)
							{
								List.Add(new WorkSheetVideosFilter()
								{
									ItemValue = age.ItemValue,
									ItemName = age.AlternateItemName
								});
							}
						}
					}
					responce.Result = List;
					responce.StatusCode = HttpStatusCode.OK;
				}

			}
			catch (Exception ex)
			{
				responce.Message = ex.Message;
				responce.StatusCode = HttpStatusCode.OK;
			}
			return Json(responce, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public ActionResult GetVolumeList(string CurrentNode, string AgeItemValue)
		{
			Responce responce = new Responce();
			try
			{
				IEnumerable<NameListItem> AgeList;
				List<WorkSheetVideosFilter> List = new List<WorkSheetVideosFilter>();
				var vrWeekName = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
					   .Where(x => x.ContentType.Alias == "volumeMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>().ToList().Where(c => c.IsActice);
				if (vrWeekName != null && vrWeekName.Any())
				{
					var VideoRoot = Umbraco?.Content(CurrentNode)?.DescendantsOrSelf()?.OfType<Videos>().FirstOrDefault();
					if (AgeItemValue == null || string.IsNullOrWhiteSpace(AgeItemValue))
					{
						if (VideoRoot != null)
						{
							foreach (var week in vrWeekName)
							{
								var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault()?
											.Children?.OfType<Video>()?.Where(x => x.IsActive && x.AgeTitle != null && Umbraco?.Content(x.SelectVolume?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == week.ItemValue).ToList();
								if (Videos != null && Videos.Count() > 0)
								{
									List.Add(new WorkSheetVideosFilter()
									{
										ItemValue = week.ItemValue,
										ItemName = week.ItemName
									});
								}
							}
						}
					}
					else
					{
						AgeList = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
						 .Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>().ToList().Where(c => c.IsActice && AgeItemValue.Contains(c.Name));
						foreach (var age in AgeList)
						{
							var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault()?
											  .Children?.OfType<Video>()?.Where(x => x.IsActive && x.AgeTitle != null && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue).ToList();
							if (Videos != null && Videos.Count() > 0)
							{
								foreach (var week in vrWeekName)
								{
									var Data = Videos.Where(x => x.IsActive && x.AgeTitle != null && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue && Umbraco?.Content(x.SelectVolume?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == week.ItemValue).Count();
									if (Data > 0)
										List.Add(new WorkSheetVideosFilter()
										{
											ItemValue = week.ItemValue,
											ItemName = week.ItemName
										});
								}
							}
						}
					}
					List<WorkSheetVideosFilter> NewList = List.GroupBy(x => x.ItemValue).Select(y => y.First()).ToList();
					responce.Result = NewList;
					responce.StatusCode = HttpStatusCode.OK;
					return Json(responce, JsonRequestBehavior.AllowGet);
				}


			}
			catch (Exception ex)
			{
				responce.Message = ex.Message;
				responce.StatusCode = HttpStatusCode.OK;
			}
			return Json(responce, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public ActionResult GetCategoryList(string CurrentNode, string AgeItemValue, string VolumeItemValue)
		{
			Responce responce = new Responce();
			try
			{
				IEnumerable<NameListItem> AgeList;
				List<WorkSheetVideosFilter> List = new List<WorkSheetVideosFilter>();
				var vrCategories = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
					   .Where(x => x.ContentType.Alias == "categoryMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>().ToList().Where(c => c.IsActice);
				if (vrCategories != null && vrCategories.Any())
				{
					var VideoRoot = Umbraco?.Content(CurrentNode)?.DescendantsOrSelf()?.OfType<Videos>().FirstOrDefault();
					if ((AgeItemValue == null || string.IsNullOrWhiteSpace(AgeItemValue)) && (VolumeItemValue == null || string.IsNullOrWhiteSpace(VolumeItemValue)))
					{
						foreach (var cate in vrCategories)
						{
							var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault()?
										.Children?.OfType<Video>()?.Where(x => x.IsActive && x.AgeTitle != null && x.Category?.Count() > 0 && x.Category.Where(c => c.Name.Contains(cate.ItemValue)).Count() > 0).ToList();
							if (Videos != null && Videos.Count() > 0)
							{
								List.Add(new WorkSheetVideosFilter()
								{
									ItemValue = cate.ItemValue,
									ItemName = cate.ItemName
								});
							}
						}
					}
					else
					{
						AgeList = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
						 .Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>().ToList().Where(c => c.IsActice && !string.IsNullOrWhiteSpace(AgeItemValue) ? AgeItemValue.Contains(c.Name) : true);
						var Videos = VideoRoot?.Children?.Where(x => x.ContentType.Alias == "videoListing")?.FirstOrDefault()?
										.Children?.OfType<Video>()?.Where(x => x.IsActive && x.AgeTitle != null).ToList();
						foreach (var age in AgeList)
						{
							if (Videos.Where(x => x.IsActive && x.AgeTitle != null && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue).Count() > 0)
							{
								foreach (var cate in vrCategories)
								{
									var Data = Videos.Where(x => x.IsActive && x.AgeTitle != null && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue).ToList();
									Data = Data.Where(x => x.IsActive && (x.Category != null && x.Category.Count() > 0) ? x.Category.Where(y => y.Name == cate.Name)?.Count() > 0 : true).ToList();
									if (VolumeItemValue != null && !string.IsNullOrWhiteSpace(VolumeItemValue))
									{
										Data = Data.Where(x => x.AgeTitle != null && ContainsInt(VolumeItemValue, Convert.ToInt32(Umbraco?.Content(x.SelectVolume?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue))).ToList();
									}
									if (Data != null && Data.Count() > 0)
										List.Add(new WorkSheetVideosFilter()
										{
											ItemValue = cate.ItemValue,
											ItemName = cate.ItemName
										});
								}
							}
						}
					}
					List<WorkSheetVideosFilter> NewList = List.GroupBy(x => x.ItemValue).Select(y => y.First()).ToList();
					responce.Result = NewList;
					responce.StatusCode = HttpStatusCode.OK;
					return Json(responce, JsonRequestBehavior.AllowGet);
				}


			}
			catch (Exception ex)
			{
				responce.Message = ex.Message;
				responce.StatusCode = HttpStatusCode.OK;
			}
			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		#endregion

		[HttpPost]
		public ActionResult PlayVideos(PlayVideoInput input)
		{
			Responce responce = new Responce();
			try
			{
				VideosInput videosInput = new VideosInput()
				{
					CultureInfo = input.CultureInfo,
					FilterType = input.FilterType,
					Source = input.Source
				};
				PlayVideoModel model = new PlayVideoModel();
				model = GetPlayVideosDetails(videosInput);

				model.Source = input.Source;

				return PartialView("/Views/Partials/PlayVideo/_playVideo.cshtml", model);
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosById");
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();
			}
			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		public PlayVideoModel GetPlayVideosDetails(VideosInput input)
		{
			PlayVideoModel model = new PlayVideoModel();
			string vUserSubscription = String.Empty;
			string bitlyLink = String.Empty;
			string videos = String.Empty;
			string filter = String.Empty;
			string vrVideoId = String.Empty;
			string vrNodeId = String.Empty;
			string vrPathwayValue = String.Empty;
			string vrAgeValue = String.Empty;
			string ageGroup = String.Empty;
			string cultureName = String.Empty;
			string videoSource = String.Empty;
			
			StringBuilder sb = new StringBuilder();
			sb.Clear();

			string culture = CultureName.GetCultureName().Replace("/", "");

			//Check here user subscription
			LoggedIn loggedin = new LoggedIn();
			loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

			//Get All subscription detais for user
			GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
			List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
			getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

			//find registered age group
			dbAccessClass db = new dbAccessClass();
			List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
			myagegroup = db.GetUserSelectedUserGroup();

			try
			{
				if (!String.IsNullOrEmpty(input.FilterType))
				{
					string decrypredParameter = HttpUtility.UrlDecode(input.FilterType);
					string[] qAll = decrypredParameter.Split('&');

					if (qAll != null && qAll.Length >= 1)
					{
						if (!String.IsNullOrEmpty(qAll[0]))
						{
							string[] fltArr = qAll[0].Split('=');
							if (fltArr.Length == 2)
								filter = fltArr[1];
						}
						if (!String.IsNullOrEmpty(qAll[1]))
						{
							string[] videoArr = qAll[1].Split('=');
							if (videoArr.Length == 2)
								vrVideoId = videoArr[1];
						}
						if (!String.IsNullOrEmpty(qAll[2]))
						{
							string[] videoidArr = qAll[2].Split('=');
							if (videoidArr.Length == 2)
								vrNodeId = videoidArr[1];
						}
						if (!String.IsNullOrEmpty(qAll[4]))
						{
							string[] ageArr = qAll[4].Split('=');
							if (ageArr.Length == 2)
								vrAgeValue = ageArr[1];
						}
						if (filter == "pathway")
						{
							if (!String.IsNullOrEmpty(qAll[3]))
							{
								string[] pathwayArr = qAll[3].Split('=');
								if (pathwayArr.Length == 2)
									vrPathwayValue = pathwayArr[1];
							}
						}
					}


					if (!String.IsNullOrEmpty(filter) && !String.IsNullOrEmpty(vrVideoId) && !String.IsNullOrEmpty(vrNodeId))
					{
						try
						{
							string subscribeUrl = culture + "/subscription/";
							if (String.IsNullOrEmpty(culture))
								cultureName = "en-US";


							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
							var Videos = Umbraco?.Content(vrNodeId)?.DescendantsOrSelf()?.OfType<Video>()?.FirstOrDefault();
							if (Videos != null)
							{
								var PDF_File = Videos?.Value<string>("uploadPDF");
								bitlyLink = Videos?.Value<string>("bitlyLink");
								ageGroup = Umbraco?.Content(Videos?.AgeTitle.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue;

								NestedItems nested = new NestedItems();
								nested.Age = vrAgeValue;
								nested.Title = Videos.Title;
								nested.Description = Videos.Description;
								nested.Category = string.Join(",", Videos?.Category?.Select(x => x.Name).ToList());
								nested.VideoPreviewId = Videos?.VideoPreviewId;
								nested.VimeoURL = Videos?.VimeoUrl;
								nested.Id = Videos.Id;

								try
								{
									if (Videos?.Parent?.ContentType?.Alias == "zoomMeeting")
									{
										videoSource = "webinar";
										nested.VideoSource = "webinar";
									}

									//social integration
									WorksheetVideosHelper videosHelper = new WorksheetVideosHelper();
									
									nested = videosHelper.GetSocialItemsAndSubscriptionDetailsForVideos(Videos, input, nested);
									model.PlayVideo = nested;
									//model.VideoYouTubeId = Videos.VideoYouTubeId +",";
								}
								catch { }
								if (!String.IsNullOrEmpty(filter))
								{
									IEnumerable<Video> filteredVideo = null;
									if (filter == "all")
									{
										filteredVideo = Umbraco?.Content(vrNodeId)?.Parent?.Children?.OfType<Video>()?.Where(x => x.IsActive);
									}
									else if (filter == "age")
									{
										filteredVideo = Umbraco?.Content(vrNodeId)?.Parent?.Children?.OfType<Video>()?
											.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == ageGroup);
									}
									else if (filter == "pathway")
									{
										if (!String.IsNullOrEmpty(vrPathwayValue))
										{
											filteredVideo = Umbraco?.Content(vrNodeId)?.Parent?.Children?.OfType<Video>()?
												.Where(x => x.IsActive && Umbraco.Content(x.LearningPathway.Select(y => y.Udi)).ToList().OfType<NameListItem>().Any(o => o.ItemValue == vrPathwayValue));
										}
									}

									try
									{
										if (filteredVideo != null && filteredVideo.Any())
										{
											List<NestedItems> nesteds = new List<NestedItems>();
											foreach (var video in filteredVideo)
											{
												string PDFFile = video?.UploadPdf;
												bitlyLink = video?.BitlyLink;
												string selAgeGroup = Umbraco?.Content(video?.AgeTitle.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue;
												NestedItems nestedItems = new NestedItems();
												nestedItems.Age = vrAgeValue;
												nestedItems.Title = video?.Title;
												nestedItems.Description = video?.Description;
												nestedItems.Category = string.Join(",", video?.Category?.Select(x => x.Name).ToList());
												try
												{
													WorksheetVideosHelper videosHelper = new WorksheetVideosHelper();
													input.FilterType = filter;
													nestedItems = videosHelper.GetSocialItemsAndSubscriptionDetailsForVideos(video, input, nestedItems);
												}
												catch { }

												if (nestedItems.IsPlayVideos)
													model.VideoYouTubeId += nestedItems.DataId + ",";

												if (video.VideoYouTubeId == vrVideoId)
													nestedItems.vrVideoId = "active-video";
												else
													nested.vrVideoId = "";

												if (video?.Parent?.ContentType?.Alias == "zoomMeeting")
												{
													videoSource = "webinar";
													nested.VideoSource = "webinar";
												}

												nesteds.Add(nestedItems);

											}

											if (model.VideoYouTubeId != null)
											{
												model.VideoYouTubeId = model.VideoYouTubeId.Trim(',');
											}

											if (nesteds != null)
											{
												nesteds = nesteds.OrderByDescending(x => x.IsPlayVideos).ToList();
											}
											model.PlayVideoList = nesteds;
										}
									}
									catch (Exception ex)
									{
										Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - Video Bind");
									}
									//See More

									//Video save for recommended
									try
									{
										dbProxy _db = new dbProxy();
										GetStatus insertStatus = new GetStatus();
										int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
										string Videolength = Videos?.VideoTotalTimeInMin;
										if (String.IsNullOrEmpty(Videolength))
											Videolength = "0";

										List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
										new SetParameters{ ParameterName = "@NodeId", Value = vrNodeId.ToString() },
										new SetParameters { ParameterName = "@VideoId", Value = vrVideoId.ToString() },
										new SetParameters { ParameterName = "@VideoType", Value = filter },
										new SetParameters { ParameterName = "@VideoDuration", Value = Videolength },
										new SetParameters { ParameterName = "@Age", Value = ageGroup  },
										 new SetParameters { ParameterName = "@CultureInfo", Value = culture  },
										 new SetParameters { ParameterName = "@VideoSource", Value = videoSource  }
									};

										insertStatus = _db.StoreData("USP_TrackVideo", sp);
									}
									catch (Exception ex)
									{
										Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - Video Save for Recommend");
									}
								}
							}
						}
						catch (Exception ex)
						{
							Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - Video Filter Bind");
						}
					}
				}

				videos = videos.TrimEnd(',');
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "Video Play - VideoPlay");
			}

			return model;
		}


		public ActionResult GetVideoUrl(string videoId, string type)
		{
			VimeoResponseData vimeoResponseData = new VimeoResponseData();
			if (!String.IsNullOrWhiteSpace(videoId) && !String.IsNullOrWhiteSpace(type))
			{
				VimeoResponse vimeoResponse = new VimeoResponse();

				try
				{
					var client = new RestClient("https://api.vimeo.com/videos/" + videoId);
					client.Timeout = -1;
					var request = new RestRequest(Method.GET);
					request.AddHeader("Authorization", "Bearer a24d95c306298cd94afc7ef38ab2c374");
					IRestResponse response = client.Execute(request);
					vimeoResponse = JsonConvert.DeserializeObject<VimeoResponse>(response.Content);

					if (vimeoResponse != null && vimeoResponse.play.hls.link != null)
					{
						if (type == "hls")
							vimeoResponseData.VimeoUrl = vimeoResponse.play.hls.link;
						else if (type == "dash")
							vimeoResponseData.VimeoUrl = vimeoResponse.play.dash.link;
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideoUrl");
				}
			}

			return Json(vimeoResponseData, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GetStructuredProgramVideosList(VideosInput input)
		{
			Responce responce = new Responce();
			try
			{
				//find registered age group
				dbAccessClass db = new dbAccessClass();
				List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
				myagegroup = db.GetUserSelectedUserGroup();

				VideosModel model = new VideosModel();
				model = GetVideosListData(input);

				if (myagegroup != null && myagegroup.Any())
				{
					model.Videos = model.Videos.OrderByDescending(lst => myagegroup.Any(y => y.AgeGroup == lst.AgeValue)).ToList();
				}
				var random = new Random();
				int index = random.Next(model?.Videos?.Count ?? 1);
				int nesteditemcount = (model?.Videos?.ElementAt(index)?.NestedItems == null ? model?.Videos?.ElementAt(1)?.NestedItems?.Count : model?.Videos?.ElementAt(1)?.NestedItems?.Count) ?? 1;
				int nestedindex = random.Next(nesteditemcount);
				var outputvideos = model?.Videos?.ElementAt(0)?.NestedItems?.ElementAt(0);
				return Json(outputvideos, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosList");
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();
			}
			return Json(responce, JsonRequestBehavior.AllowGet);
		}
	}
}
