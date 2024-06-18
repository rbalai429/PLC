using HPPlc.Models.WorkSheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Models
{
	public class WorksheetVideosHelper
	{
		public WorksheetVideosHelper()
		{

		}
		#region WorkSheet start
		public NestedItems GetSocialItemsAndSubscriptionDetailsForWorkSheet(WorksheetRoot WorkSheet, WorksheetInput input, NestedItems nested, List<SelectedAgeGroup> myagegroup)
		{
			SocialItems social = new SocialItems();
			try
			{
				LoggedIn loggedin = new LoggedIn();
				loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

				//Get All subscription detais for user
				GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
				List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
				getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);


				var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
				var AgeTitleDesc = helper.Content(WorkSheet.AgeTitle?.Udi);
				var ItemName = AgeTitleDesc?.Value("itemName");
				var AgeGroup = AgeTitleDesc?.Value("itemValue");
				string cultureName = CultureName.GetCultureName();

				string bitlyLink = String.Empty;
				var PDF_File = WorkSheet?.UploadPdf;
				bitlyLink = WorkSheet?.BitlyLink;
				string DownloadString = String.Empty;

				if (loggedin != null)
				{
					//Get User subscription based on age group
					if (loggedin?.UserTransactionType == "free")
						UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.Ranking == "1" && x.IsActive == 1)?.SingleOrDefault();
					else
						UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString() && x.IsActive == 1)?.SingleOrDefault();


					//Check pdf is subscription wise or not
					if (getUserCurrentSubscription != null && getUserCurrentSubscription.Any())
					{
						var SubscriptionsWiseDocument = WorkSheet?.DocumentAndSubscriptions;
						bool subscriptionWiseDoc = WorkSheet.IsSubscriptionWiseDocument;

						if (SubscriptionsWiseDocument != null && subscriptionWiseDoc == true)
						{
							GetUserCurrentSubscription PrintCurrentSubscription = new GetUserCurrentSubscription();
							if (getUserCurrentSubscription.Count > 0 && getUserCurrentSubscription.Where(x => x.AgeGroup == AgeGroup.ToString()).Any())
								PrintCurrentSubscription = getUserCurrentSubscription?.Where(x => x.AgeGroup == AgeGroup.ToString() && x.IsActive == 1)?.SingleOrDefault();
							else
								PrintCurrentSubscription = getUserCurrentSubscription?.Where(x => x.Ranking == "1" && x.IsActive == 1)?.SingleOrDefault();

							if (PrintCurrentSubscription != null)
							{
								foreach (var documentNode in SubscriptionsWiseDocument)
								{
									if (documentNode.SelectSubscriptions != null)
									{
										string subscriptionNode = documentNode.SelectSubscriptions.Udi.ToString();
										if (!String.IsNullOrWhiteSpace(subscriptionNode))
										{
											bool SubscriptionDataExits = helper?.Content(subscriptionNode)?.DescendantsOrSelf().OfType<Subscriptions>().FirstOrDefault().Ranking == PrintCurrentSubscription.Ranking;
											if (SubscriptionDataExits)
												PDF_File = documentNode.BrowseDocument.Url();
										}
									}
								}
							}
						}
					}
				}

				if (loggedin == null && WorkSheet.IsGuestUserSheet == false)
					DownloadString = input.CultureInfo + "$" + ItemName + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";
				else if (loggedin == null && WorkSheet.IsGuestUserSheet == true)
					DownloadString = input.CultureInfo + "$" + "0" + "$" + AgeGroup + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";
				else
					DownloadString = input.CultureInfo + "$" + loggedin.UserId.ToString() + "$" + AgeGroup + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";

				string loginUrl = cultureName + "/my-account/login?ref=u";

				//Check if user have only free subscription and have referral
				int tobeDisplayNoOfVolumes = 0;
				int tobeStartVolumeDisplay = 0;
				int? currentVolume = 0;
				int startVolumeForReferral = WorkSheet.Parent.Parent.Value<int>("startVolumeForReferral");
				var volumeTextRef = WorkSheet?.SelectVolume;
				int? volumeItemValue = helper?.Content(volumeTextRef?.Udi)?.Value<int>("itemValue");
				if (loggedin != null && loggedin.UserTransactionType == "free" && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription?.DaysRemaining > 0)
				{
					tobeDisplayNoOfVolumes = UserCurrentSubscription.ReferralRewardMonth;
					tobeStartVolumeDisplay = startVolumeForReferral;
					currentVolume = volumeItemValue;
				}
				#region Social Share
				try
				{
					//Share Icons activation
					//if (WorkSheet.Subscription != null && WorkSheet.Subscription.Any())
					//{
					//	if ((loggedin == null && WorkSheet.IsGuestUserSheet == true) || //User not loggedIn and worksheet allowed for guest user
					//		((loggedin != null && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString()))) && helper.Content(WorkSheet.Subscription?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) ||
					//		((loggedin != null && UserCurrentSubscription?.DaysRemaining >= 1 && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString())) && helper.Content(WorkSheet.Subscription?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) == Convert.ToInt32(o.Ranking)))) || //  loggedIn age group and subscription matched with worksheet
					//		((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription != null && UserCurrentSubscription.DaysRemaining > 0 && helper.Content(WorkSheet.Subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking))) ||
					//		(loggedin != null && loggedin?.UserTransactionType == "free" && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
					//	{
					//		// || (loggedin != null && getUserCurrentSubscription != null)
					//		if (!String.IsNullOrEmpty(WorkSheet.FacebookContent) || !String.IsNullOrEmpty(WorkSheet.WhatsAppContent) || !String.IsNullOrEmpty(WorkSheet.MailContent))
					//		{
					//			if (!String.IsNullOrEmpty(WorkSheet.FacebookContent))
					//				social.FBShare = WorkSheet.FacebookContent;

					//			if (!String.IsNullOrEmpty(WorkSheet.WhatsAppContent))
					//				social.WhatAppShare = WorkSheet.WhatsAppContent;

					//			if (!String.IsNullOrEmpty(WorkSheet.MailContent))
					//				social.EmailShare = WorkSheet.MailContent + "`" + ItemName;
					//		}
					//	}
					//}

					// || (loggedin != null && getUserCurrentSubscription != null)
					if ((loggedin == null && WorkSheet.IsGuestUserSheet == true) || //User not loggedIn and worksheet allowed for guest user
						((loggedin != null && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString())))) ||
						((loggedin != null && UserCurrentSubscription?.DaysRemaining >= 1 && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString()) || //  loggedIn age group and subscription matched with worksheet
						(loggedin != null && loggedin?.UserTransactionType == "free")))))
					{
						if (!String.IsNullOrEmpty(WorkSheet.FacebookContent) || !String.IsNullOrEmpty(WorkSheet.WhatsAppContent) || !String.IsNullOrEmpty(WorkSheet.MailContent))
						{
							if (!String.IsNullOrEmpty(WorkSheet.FacebookContent))
								social.FBShare = WorkSheet.FacebookContent;

							if (!String.IsNullOrEmpty(WorkSheet.WhatsAppContent))
								social.WhatAppShare = WorkSheet.WhatsAppContent;

							if (!String.IsNullOrEmpty(WorkSheet.MailContent))
								social.EmailShare = WorkSheet.MailContent + "`" + ItemName;
						}
					}
				}
				catch (Exception ex)
				{

				}
				nested.socialItems = social;
				#endregion
				#region Subscription Button Test
				SubscriptionStatus status = new SubscriptionStatus();
				try
				{
					//if (WorkSheet.Subscription != null && WorkSheet.Subscription.Any())
					//{
						if (input.CultureInfo == "/")
							input.CultureInfo = String.Empty;

						string subscriptionRanking = "";
						//string subscriptionUrl = cultureName + "/subscription?subscptn=" + clsCommon.Encrypt(subscriptionRanking) + "&WID=" + clsCommon.encrypto(WorkSheet.Id.ToString().Trim()) + "&age=" + clsCommon.Encrypt(WorkSheet.AgeTitle?.Name.ToString().Trim());
						string subscriptionUrl = cultureName + "/my-account/add-age-group/";
						string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=Worksheet" + "&age=" + WorkSheet.AgeTitle?.Name + " Years" + "&week=" + WorkSheet?.SelectVolume?.Name + "&category=" + WorkSheet?.Category?.FirstOrDefault()?.Name;
						status.DownloadUrl = downloadUrl;
						status.DownloadString = DownloadString;

						if (loggedin == null)
							status.SubscriptionUrl = loginUrl;
						else
							status.SubscriptionUrl = subscriptionUrl;

						if (loggedin == null && WorkSheet.IsGuestUserSheet == true)
							status.Condition1 = true;
						//2. if user not loggedin and worksheet not mapped with isguestuser true
						else if (loggedin == null && WorkSheet.IsGuestUserSheet == false)
							status.Condition2 = true;
						// 3.1 if User have free subscription and have referral
						else if (loggedin != null && loggedin?.UserTransactionType == "free" && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString()) && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))//
							status.Condition3 = true;
						// 3.2 if User have loggedin and age group is same with worksheet and worksheet is free
						else if (loggedin != null && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString())))
							status.Condition4 = true;
						else if (loggedin != null && UserCurrentSubscription?.DaysRemaining >= 1 && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString()) && helper.Content(WorkSheet.Subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) == Convert.ToInt32(o.Ranking))))
							status.Condition5 = true;
						else if (loggedin != null && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString()) && helper.Content(WorkSheet.Subscription.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) < Convert.ToInt32(o.Ranking))))
							status.Condition6 = true;
						// 4.1 if User have loggedin and age group is not same with worksheet and user is free
						else if (loggedin != null && loggedin?.UserTransactionType == "free" && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && !(myagegroup != null && myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString())))
							status.Condition7 = true;
						else if (loggedin != null && loggedin?.UserTransactionType == "paid" && (UserCurrentSubscription != null && UserCurrentSubscription?.DaysRemaining < 1) && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()))
							status.Condition8 = true;
						//6. if User have loggedin and age group is not same with worksheet and worksheet is paid
						else if (loggedin != null && loggedin?.UserTransactionType == "paid" && !String.IsNullOrEmpty(WorkSheet.AgeTitle?.Name?.ToString()) && (myagegroup != null && !myagegroup.Any(x => x.AgeGroup == WorkSheet.AgeTitle?.Name?.ToString())))
							status.Condition9 = true;
						else
						{
						}
					//}
					nested.subscriptionStatus = status;

				}
				catch (Exception ex)
				{

				}
				#endregion Subscription Button Text End

			}
			catch (Exception ex)
			{
			}
			return nested;
		}
		#endregion

		#region Videos start
		public Videos.NestedItems GetSocialItemsAndSubscriptionDetailsForVideos(Video videos, Videos.VideosInput input, Videos.NestedItems nested)
		{
			Videos.SocialItems social = new Videos.SocialItems();
			try
			{
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
				string vUserSubscription = String.Empty;
				string playVideoUrl = String.Empty;
				string playVideoParam = String.Empty;
				string playVideoBitlyUrl = String.Empty;

				if (loggedin != null)
				{
					//Get User subscription based on age group
					if (loggedin.UserTransactionType == "free")
						UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.Ranking == "1" && x.IsActive == 1)?.SingleOrDefault();
					else
						UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.AgeGroup == videos.AgeTitle.Name && x.IsActive == 1)?.SingleOrDefault();
				}

				//Get Age GRoup details
				var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
				var AgeTitleDesc = helper.Content(videos.AgeTitle?.Udi);
				var AgeName = AgeTitleDesc?.Value("itemName");
				var AgeValue = AgeTitleDesc?.Value("itemValue");

				string culture = CultureName.GetCultureName();
				string subscribeUrl = culture + "/subscription/";
				string loginUrl = culture + "/my-account/login/";
				playVideoUrl = ConfigurationManager.AppSettings["SiteUrl"].ToString();
				var hepler = Umbraco.Web.Composing.Current.UmbracoHelper;
				string subscriptionRanking = hepler.Content(videos.Subscriptions.Select(x => x.Udi))?.ToList().OfType<Subscriptions>().First()?.Ranking.ToString().Trim();

				//Check if user have only free subscription and have referral
				//int tobeDisplayNoOfVolumes = 0;
				//int tobeStartVolumeDisplay = 0;
				//int? currentVolume = 0;

				//if (loggedin != null && loggedin.UserTransactionType == "free" && UserCurrentSubscription.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0)
				//{
				//	tobeDisplayNoOfVolumes = UserCurrentSubscription.ReferralRewardMonth;
				//	tobeStartVolumeDisplay = 4;
				//	currentVolume = 3;
				//}

				//playVideoParam = culture + "videos/play-video?" + clsCommon.Encrypt(HttpUtility.UrlPathEncode(input.FilterType + ":" + videos.VideoYouTubeId + ":" + videos.Id + ":" + input.FilterId));
				playVideoParam = culture + "videos/play-video?" + HttpUtility.UrlPathEncode("type=" + input.FilterType + "&videoid=" + videos.VideoYouTubeId + "&video=" + videos.Id + "&filterid=" + input.FilterId + "&age=" + videos?.AgeTitle?.Name + " Years" + "&name=" + videos.Title);

				// Generate bitly link for video link share
				//if (!String.IsNullOrEmpty(playVideoUrl) && !String.IsNullOrEmpty(playVideoParam))
				//{
				//	GenerateBitlyLink bitLink = new GenerateBitlyLink();
				//	playVideoBitlyUrl = bitLink.Shorten(playVideoUrl + playVideoParam);
				//}

				try
				{
					// Check User Sebcription - if subscribed then can video play other wise display subscribe button
					if (videos.Subscriptions != null && videos.Subscriptions.Any())
					{

						if ((loggedin == null && videos.IsGuestUserSheet == true) || //User not loggedIn and worksheet allowed for guest user
						   ((loggedin != null && !String.IsNullOrEmpty(videos.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name?.ToString()))) && hepler.Content(videos.Subscriptions?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) ||
							((loggedin != null && UserCurrentSubscription?.DaysRemaining >= 1 && !String.IsNullOrEmpty(videos.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name?.ToString())) && hepler.Content(videos.Subscriptions?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) == Convert.ToInt32(o.Ranking)))) || //loggedIn age group and subscription matched with worksheet
							((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription != null && UserCurrentSubscription.DaysRemaining > 0 && hepler.Content(videos.Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking))))
						//(loggedin != null && loggedin?.UserTransactionType == "free" && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
						{
							nested.IsPlayVideos = true;
							nested.DataId = videos.VideoYouTubeId;
							nested.VideoUrl = playVideoUrl + playVideoParam;
						}
						else
						{
							nested.IsPlayVideos = false;
							nested.SubscriptionUrl = culture + "/subscription?subscptn=" + clsCommon.Encrypt(subscriptionRanking) + "&age=" + clsCommon.Encrypt(videos.AgeTitle?.Name);
						}
					}
				}
				catch { }

				//Check - if image have in cms then display otherwise thumnail display from youTube
				string thumbUrl = String.Empty;
				if (videos?.ThumbnailImage != null)
				{
					thumbUrl = videos?.ThumbnailImage.Url().ToString();
				}
				if (String.IsNullOrEmpty(thumbUrl))
				{
					nested.ImagesSrc = "https://img.youtube.com/vi/" + videos.VideoYouTubeId;
					nested.ThumbUrl = true;
				}
				else
				{
					nested.ThumbUrl = false;
					nested.AltText = videos?.ThumbnailImage?.Value<string>("altText");
					nested.ImagesSrc = videos?.ThumbnailImage?.Url();
					nested.NextGenImage = videos?.NextGenImage?.Url();
				}



				try
				{
					// Share Icons activation according to subscription
					Videos.SocialItems socialItems = new Videos.SocialItems();
					if (videos.Subscriptions != null && videos.Subscriptions.Any())
					{
						//if ((loggedin == null && videos.IsGuestUserSheet == true) ||
						//	((loggedin != null && !String.IsNullOrEmpty(videos.AgeTitle?.Name) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name))) && hepler.Content(videos.Subscriptions?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) ||
						//	((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription != null && UserCurrentSubscription.DaysRemaining > 0 && hepler.Content(videos.Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking))) ||
						//	(loggedin != null && loggedin?.UserTransactionType == "free" && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
						//{
						if ((loggedin == null && videos.IsGuestUserSheet == true) || //User not loggedIn and worksheet allowed for guest user
							((loggedin != null && !String.IsNullOrEmpty(videos.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name?.ToString())) && hepler.Content(videos.Subscriptions?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1"))) ||
							((loggedin != null && UserCurrentSubscription?.DaysRemaining >= 1 && !String.IsNullOrEmpty(videos.AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name?.ToString())) && hepler.Content(videos.Subscriptions?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) == Convert.ToInt32(o.Ranking)))) || //loggedIn age group and subscription matched with worksheet
							((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription != null && UserCurrentSubscription.DaysRemaining > 0 && hepler.Content(videos.Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking))))
						//(loggedin != null && loggedin?.UserTransactionType == "free" && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
						{
							if (!String.IsNullOrEmpty(videos.FacebookContent) || !String.IsNullOrEmpty(videos.WhatsAppContent) || !String.IsNullOrEmpty(videos.MailContent))
							{

								if (!String.IsNullOrEmpty(videos.FacebookContent))
									socialItems.FBShare = videos.FacebookContent;
								if (!String.IsNullOrEmpty(videos.WhatsAppContent))
									socialItems.WhatAppShare = videos.WhatsAppContent + " " + playVideoBitlyUrl;

								if (!String.IsNullOrEmpty(videos.MailContent))
									socialItems.EmailShare = videos.MailContent + "\n\n" + playVideoBitlyUrl + " " + "\n\n" + "`" + videos?.Title;
								nested.socialItems = socialItems;
							}
						}
					}
				}
				catch { }

				var PDF_File = videos?.Value<string>("uploadPDF");
				string DownloadString = String.Empty;
				if (loggedin == null && videos.IsGuestUserSheet == false)
					DownloadString = input.CultureInfo + "$" + "0" + "$" + AgeValue + "$" + videos?.Id + "$" + PDF_File + "$" + "Video Print";
				else if (loggedin == null && videos.IsGuestUserSheet == true)
					DownloadString = input.CultureInfo + "$" + "0" + "$" + AgeValue + "$" + videos?.Id + "$" + PDF_File + "$" + "Video Print";
				else
					DownloadString = input.CultureInfo + "$" + loggedin.UserId.ToString() + "$" + AgeValue + "$" + videos?.Id + "$" + PDF_File + "$" + "Video Print";

				try
				{
					if (videos.Subscriptions != null && videos.Subscriptions.Any())
					{
						if (culture == "/")
							culture = String.Empty;


						int maxSubscriptionRanking = int.Parse(hepler?.Content(videos.Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Max(m => m.Ranking));
						string subscriptionUrl = culture + "/subscription?subscptn=" + clsCommon.encrypto(subscriptionRanking) + "&age=" + clsCommon.Encrypt(videos.AgeTitle?.Name);
						string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(videos.Id.ToString()) + "&source=Video";

						if (!String.IsNullOrEmpty(videos.AgeTitle?.Name))
						{
							Videos.SubscriptionStatus subscriptionStatus = new Videos.SubscriptionStatus();
							//1. if user not loggedin and worksheet mapped with isguestuser true
							if (loggedin == null && videos.IsGuestUserSheet == true)
							{
								subscriptionStatus.Condition1 = true;
								subscriptionStatus.DownloadUrl = downloadUrl;
								subscriptionStatus.DownloadString = DownloadString;
							}
							// 2. if user not loggedin and worksheet not mapped with isguestuser true
							else if (loggedin == null && videos.IsGuestUserSheet == false)
							{
								subscriptionStatus.Condition2 = true;
								subscriptionStatus.SubscriptionUrl = subscriptionUrl;
							}

							//3.1 if User have free subscription and have referral
							//else if (loggedin != null && loggedin?.UserTransactionType == "free" && !String.IsNullOrEmpty(videos.AgeTitle?.Name) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name) && UserCurrentSubscription.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
							//{
							//	subscriptionStatus.Condition3 = true;
							//	subscriptionStatus.DownloadUrl = downloadUrl;
							//	subscriptionStatus.DownloadString = DownloadString;
							//}

							//3.2 if User have loggedin and age group is same with worksheet and worksheet is free
							else if (loggedin != null && loggedin?.UserTransactionType == "paid" && !String.IsNullOrEmpty(videos.AgeTitle?.Name) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name) && hepler.Content(videos.Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")))
							{
								subscriptionStatus.Condition4 = true;
								subscriptionStatus.DownloadUrl = downloadUrl;
								subscriptionStatus.DownloadString = DownloadString;
							}
							else if (loggedin != null && UserCurrentSubscription?.DaysRemaining >= 1 && !String.IsNullOrEmpty(videos.AgeTitle?.Name) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name) && hepler.Content(videos.Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) == Convert.ToInt32(o.Ranking))))
							{
								subscriptionStatus.Condition5 = true;
								subscriptionStatus.DownloadUrl = downloadUrl;
								subscriptionStatus.DownloadString = DownloadString;
							}

							else if (loggedin != null && !String.IsNullOrEmpty(videos.AgeTitle?.Name) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name) && hepler.Content(videos.Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) < Convert.ToInt32(o.Ranking))))
							{
								subscriptionStatus.Condition6 = true;
								subscriptionStatus.SubscriptionUrl = subscriptionUrl;
							}
							// 4.1 if User have loggedin and age group is not same with worksheet and user is free
							else if (loggedin != null && loggedin?.UserTransactionType == "free" && !String.IsNullOrEmpty(videos.AgeTitle?.Name) && !(myagegroup != null && myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name)))
							{
								subscriptionStatus.Condition7 = true;
								subscriptionStatus.SubscriptionUrl = subscriptionUrl;
							}

							// 6. if User have loggedin and age group is not same with worksheet and worksheet is paid
							else if (loggedin != null && loggedin?.UserTransactionType == "paid" && !String.IsNullOrEmpty(videos.AgeTitle?.Name) && (myagegroup != null && !myagegroup.Any(x => x.AgeGroup == videos.AgeTitle?.Name)))
							{
								subscriptionStatus.Condition8 = true;
								subscriptionStatus.SubscriptionUrl = subscriptionUrl;
							}

							else
							{
								subscriptionStatus.Condition9 = true;
								subscriptionStatus.SubscriptionUrl = subscriptionUrl;
							}
							nested.subscriptionStatus = subscriptionStatus;

						}
					}
				}
				catch (Exception ex)
				{

				}
				#endregion Subscription Button Text End

			}
			catch (Exception ex)
			{
			}
			return nested;
		}
	}
}