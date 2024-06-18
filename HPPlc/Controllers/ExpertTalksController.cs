using HPPlc.Model;
using HPPlc.Models;
//using OfficeOpenXml;
//using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;
using HPPlc.Models.ExpertTalkWebinar;

namespace HPPlc.Controllers
{
    public class ExpertTalksController : SurfaceController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;
        public ExpertTalksController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }
       
        [HttpPost]
        public ActionResult GetExpertTalkList(ExpertTalkInput input)
        {
            Responce responce = new Responce();
            try
            {
                ExpertTalkWebinarModel model = new ExpertTalkWebinarModel();
                model.ExpertWebinars = GetExpertTalkListData(input);
                //model.Videos = GetExpertTalkVideoListData(input);

                return PartialView("/Views/Partials/ExpertTalkWebinar/_expertWebinarList.cshtml", model);
            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();

                Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetExpertTalkList");
            }

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetExpertTalkVideoList(ExpertTalkInput input)
        {
            Responce responce = new Responce();
            try
            {
                ExpertTalkWebinarModel model = new ExpertTalkWebinarModel();
                model.Videos = GetExpertTalkVideoListData(input);
                return PartialView("/Views/Partials/ExpertTalkWebinar/_expertVideosList.cshtml", model);
            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();

                Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetExpertTalkList");
            }

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

		public List<WebinarItems> GetExpertTalkListData(ExpertTalkInput input)
		{
			ExpertTalkWebinarModel model = new ExpertTalkWebinarModel();
			List<WebinarItems> WebinarItemsList = new List<WebinarItems>();
			string referralCode = String.Empty;
			try
			{
				var commonContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
													  .Where(x => x.ContentType.Alias == "commonContent").OfType<CommonContent>().FirstOrDefault();
				referralCode = SessionManagement.GetCurrentSession<string>(SessionType.UserReferralCode);

				if (!String.IsNullOrWhiteSpace(referralCode))
				{
					if (!String.IsNullOrWhiteSpace(commonContent.ShareReferralText.ToString()) && !String.IsNullOrWhiteSpace(referralCode))
					{
						string referralText = commonContent.ShareReferralText.ToString().ToString().Replace("<p>", "").Replace("</p>", "");
						referralCode = referralText.Replace("{referralcode}", referralCode);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetExpertTalkListData - Referral");
			}

			try
			{
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				var expertTalkRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
													.Where(x => x.ContentType.Alias == "expertTalks")?.OfType<ExpertTalks>().FirstOrDefault();

				if (expertTalkRoot != null)
				{
					var expertWebinarsItems = expertTalkRoot?.Children?
						.Where(x => x.ContentType.Alias == "webinars")?.OfType<Webinars>()?.FirstOrDefault()?.Children?
						.Where(x => x.ContentType.Alias == "expertWebinar")?.OfType<ExpertWebinar>()?
						.Where(x => x.IsActive == true);//.Where(x => x.WebinarDate.ToShortDateString() == DateTime.Now.ToShortDateString());

					if (expertWebinarsItems != null && expertWebinarsItems.Count() > 0 && expertWebinarsItems.Any())
					{
						foreach (var Webinar in expertWebinarsItems)
						{
							WebinarItems webinarItem = new WebinarItems();
							webinarItem.ItemId = Convert.ToInt32(Webinar?.Id);
							webinarItem.Topic = Webinar?.Topic;
							var Image = Webinar?.ThumbnailImage;
							string altText = Image?.Value<string>("altText");
							var NextGenImage = Webinar?.ThumbnailImageWebp;

							var MobileImage = Webinar?.MobileImage;
							var MobileWebPImage = Webinar?.MobileImageWebP;
							if (Image != null)
							{
								webinarItem.AltText = altText;
								webinarItem.DesktopImage = Image.Url();
							}
							if (NextGenImage != null)
							{
								webinarItem.DesktopImageWebP = NextGenImage.Url();
							}
							if (MobileImage != null)
							{
								webinarItem.MobileImage = MobileImage.Url();
							}
							if (MobileWebPImage != null)
							{
								webinarItem.MobileImageWebP = MobileWebPImage.Url();
							}

							var Speakers = Webinar?.Speakers.ToList();
							if (Speakers != null && Speakers.Any())
							{
								List<Speakers> speakersLst = new List<Speakers>();

								foreach (var spkr in Speakers)
								{
									Speakers speakers = new Speakers();
									speakers.SpeakerName = spkr?.Title;
									speakers.SpeakerSubDetails = spkr?.Description;

									speakersLst.Add(speakers);
								}

								webinarItem.Speakers = speakersLst;
							}
							webinarItem.AppearRegisterNowInMinutes = Convert.ToInt32(Webinar?.AppearRegisterNowInMinutes);
							webinarItem.JoinNowDisplayInMinutes = Convert.ToInt32(Webinar?.JoinNowDisplayInMinutes);
							webinarItem.DisAppearJoinNowInMinutes = Convert.ToInt32(Webinar?.DisAppearJoinNowInMinutes);
							webinarItem.WebinarDate = Convert.ToDateTime(Webinar?.WebinarDate);
							webinarItem.WebinarLink = Webinar?.WebinarLink;
							webinarItem.WebinarStartTime = Convert.ToDateTime(Webinar?.WebinarStartTime);
							webinarItem.WebinarEndTime = Convert.ToDateTime(Webinar?.WebinarEndTime);

							webinarItem.IsDisplayShare = Webinar.UseShareContentFromHere;
							if (Webinar.UseShareContentFromHere)
							{
								webinarItem.facebookContent = Webinar.FacebookContent + "\n" + referralCode;
								webinarItem.whatsAppContent = Webinar.WhatsAppContent + "\n" + referralCode;
								//webinarItem.mailContent = Webinar.MailContent + "`" + Webinar?.Topic;

								if (!String.IsNullOrEmpty(referralCode))
								{
									string lastWord = Webinar.MailContent.Substring(Webinar.MailContent.LastIndexOf(' ') + 1);
									string newWord = string.Empty;
									if (lastWord.ToLower().Contains("thank") && Webinar.MailContent != null)
									{
										string[] data = lastWord.Split('\n');
										data[1] = "\n\n" + referralCode;
										data[2] = "\n\n" + data[2];
										newWord = string.Join("", data);

										webinarItem.mailContent = Webinar.MailContent.Replace(lastWord, newWord) + "`" + Webinar?.Topic;
									}
									else
										webinarItem.mailContent = Webinar.MailContent + "\n\n" + referralCode + "`" + Webinar?.Topic;
								}
								else
									webinarItem.mailContent = Webinar.MailContent + "`" + Webinar?.Topic;
							}
							else
							{
								webinarItem.facebookContent = String.Empty;
								webinarItem.whatsAppContent = String.Empty;
								webinarItem.mailContent = String.Empty;
							}

							WebinarItemsList.Add(webinarItem);
						}
					}
				}

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(ExpertTalksController), ex, message: "GetExpertTalkListData");
			}

			//try
			//{
			//	model.ExpertWebinars = WebinarItemsList;
			//}
			//catch (Exception ex)
			//{
			//	Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetExpertTalkListData");
			//}

			return WebinarItemsList;
		}

		public List<VideosItems> GetExpertTalkVideoListData(ExpertTalkInput input)
		{
			ExpertTalkWebinarModel model = new ExpertTalkWebinarModel();
			List<VideosItems> VideoItemsList = new List<VideosItems>();
			string bitlyLink = String.Empty;
			string referralCode = String.Empty;
			try
			{
				var commonContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
													  .Where(x => x.ContentType.Alias == "commonContent").OfType<CommonContent>().FirstOrDefault();
				referralCode = SessionManagement.GetCurrentSession<string>(SessionType.UserReferralCode);

				if (!String.IsNullOrWhiteSpace(referralCode))
				{
					if (!String.IsNullOrWhiteSpace(commonContent.ShareReferralText.ToString()) && !String.IsNullOrWhiteSpace(referralCode))
					{
						string referralText = commonContent.ShareReferralText.ToString().ToString().Replace("<p>", "").Replace("</p>", "");
						referralCode = referralText.Replace("{referralcode}", referralCode);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(ExpertTalksController), ex, message: "GetExpertTalkListData - Referral");
			}


			try
			{
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				var expertTalkRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
													.Where(x => x.ContentType.Alias == "expertTalks")?.OfType<ExpertTalks>().FirstOrDefault();

				if (expertTalkRoot != null)
				{
					var videoItems = expertTalkRoot?.Children?.Where(x => x.ContentType.Alias == "zoomMeeting")?.FirstOrDefault()?
						.Children.OfType<Video>()?.Where(x => x.IsActive);

					if (videoItems != null)
					{
						foreach (var Video in videoItems)
						{

							var AgeTitle = Umbraco?.Content(Video?.AgeTitle?.Udi)?
								.DescendantsOrSelf()?.OfType<NameListItem>()?.Where(x => x.IsActice)?.FirstOrDefault();

							string ItemName = AgeTitle?.ItemName;
							var volumeText = Umbraco?.Content(Video?.SelectVolume?.Udi)?
								.DescendantsOrSelf()?.OfType<NameListItem>()?.Where(x => x.IsActice)?.FirstOrDefault();
							var volumeCSS = Video?.VolumeBackgroungCss;

							string textFacebook = Video?.FacebookContent;
							string textWhatsApp = Video?.WhatsAppContent;
							string textMail = Video?.MailContent;
							string PDF_File = Video?.UploadPdf;

							bool IsGuestUserSheet = Video.IsGuestUserSheet;
							var Subscriptions = Video?.Subscriptions;
							bitlyLink = Video?.BitlyLink;
							//string videoDetailsUrl = "/videos/play-video?" + HttpUtility.UrlPathEncode("type=all&videoid=" + Video.VideoYouTubeId + "&video=" + Video.Id + "&filterid=" + "&age=" + "&name=" + ItemName);
							string videoDetailsUrl = string.Empty;

							if (!String.IsNullOrWhiteSpace(Video?.Url()))
							{
								videoDetailsUrl = Video?.Url().Substring(1);
							}

							string thumbUrl = String.Empty;
							if (Video?.ThumbnailImage != null)
							{
								thumbUrl = Video?.ThumbnailImage.Url().ToString();
							}

							VideosItems videosItem = new VideosItems();
							videosItem.ItemName = ItemName;
							videosItem.ImagesSrc = thumbUrl;
							videosItem.Title = Video?.Title;
							videosItem.videoDetailsUrl = videoDetailsUrl;
							videosItem.videoID = Video?.VideoYouTubeId;
							videosItem.IsDisplayShare = Video.UseShareContentFromHere;

							if (Video.UseShareContentFromHere)
							{
								videosItem.facebookContent = Video.FacebookContent + "\n" + referralCode;
								videosItem.whatsAppContent = Video.WhatsAppContent + "\n" + referralCode;
								//webinarItem.mailContent = Webinar.MailContent + "`" + Webinar?.Topic;

								if (!String.IsNullOrEmpty(referralCode))
								{
									string lastWord = Video.MailContent.Substring(Video.MailContent.LastIndexOf(' ') + 1);
									string newWord = string.Empty;
									if (lastWord.ToLower().Contains("thank") && Video.MailContent != null)
									{
										string[] data = lastWord.Split('\n');
										data[1] = "\n\n" + referralCode;
										data[2] = "\n\n" + data[2];
										newWord = string.Join("", data);

										videosItem.mailContent = Video.MailContent.Replace(lastWord, newWord) + "`" + Video?.Title;
									}
									else
										videosItem.mailContent = Video.MailContent + "\n\n" + referralCode + "`" + Video?.Title;
								}
								else
									videosItem.mailContent = Video.MailContent + "`" + Video?.Title;
							}
							else
							{
								videosItem.facebookContent = String.Empty;
								videosItem.whatsAppContent = String.Empty;
								videosItem.mailContent = String.Empty;
							}

							VideoItemsList.Add(videosItem);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetExpertTalkVideoListData");
			}

			//try
			//{
			//    model.Videos = VideoItemsList;
			//}
			//catch (Exception ex)
			//{
			//    Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetExpertTalkListData");
			//}

			return VideoItemsList;
		}
		public ActionResult JoinNowIsValid()
        {
            return Json(new
            {
                status = "Success",
                navigation = SessionManagement.GetCurrentSession<string>(SessionType.ExpertTalkUrl),
                message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsertExpertTalkHistory()
        {
            ReturnMessage returnMessage = new ReturnMessage();
            returnMessage.navigation = SessionManagement.GetCurrentSession<string>(SessionType.ExpertTalkUrl);
            if (SessionExpireAttribute.UserLoggedIn())
            {
                clsExpertHelper clsExpertHelper = new clsExpertHelper();
                GetStatus insertStatus = clsExpertHelper.insertExpertData();
                if (insertStatus.returnStatus == "Success")
                {
                    returnMessage.status = "Success";
                    if (SessionManagement.GetCurrentSession<string>(SessionType.ExpertTalkUrl) != null)
                        returnMessage.navigation = SessionManagement.GetCurrentSession<string>(SessionType.ExpertTalkUrl);
                    else
                        returnMessage.navigation = SessionManagement.GetCurrentSession<string>(SessionType.MettingName);
                    returnMessage.message = "";
                }

                SessionManagement.DeleteFromSession(SessionType.ExpertTalkUrl);
                SessionManagement.DeleteFromSession(SessionType.MettingName);
                SessionManagement.DeleteFromSession(SessionType.MeetingDate);
            }
            //return returnMessage;
            return Json(returnMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetExpertTaskList()
        {
            Responce responce = new Responce();
            try
            {

                List<ExpertTalk> List = new List<ExpertTalk>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
            {
                new SetParameters{ ParameterName = "@QType", Value = "1" },
            };
                List = _db.GetDataMultiple("GetExportTalk", List, sp);
                if (List != null && List.Count > 0)
                {

                    foreach (var item in List)
                    {
                        item.Name = clsCommon.Decrypt(item.Name);
                        item.Email = clsCommon.Decrypt(item.Email);
                        item.Contact = clsCommon.Decrypt(item.Contact);
                    }
                }
                responce.Result = List;
                responce.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            //return returnMessage;
            return Json(responce, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public virtual ActionResult ExportToExcel()
        {
            List<ExpertTalk> List = new List<ExpertTalk>();
            dbProxy _db = new dbProxy();

            List<SetParameters> sp = new List<SetParameters>()
            {
                new SetParameters{ ParameterName = "@QType", Value = "1" },
            };
            List = _db.GetDataMultiple("GetExportTalk", List, sp);
            if (List != null && List.Count > 0)
            {

                foreach (var item in List)
                {
                    item.Name = clsCommon.Decrypt(item.Name);
                    item.Email = clsCommon.Decrypt(item.Email);
                    item.Contact = clsCommon.Decrypt(item.Contact);
                }
            }
            clsExpertHelper clsExpertHelper = new clsExpertHelper();
            byte[] bytes = clsExpertHelper.ListToExcel(List, "Reprts");
            if (bytes != null && bytes.Count() > 0)
            {
                return File(bytes, "application/vnd.ms-excel", "Report.xlsx");
            }
            else
            {
                return Json(new
                {
                    status = "Fail",
                    navigation = "",
                    message = "Error occurs when create excel file.."
                }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}