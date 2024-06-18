using HPPlc.Models.WorkSheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Models.FestivalOffers
{
    public class FestivalOffersHepler
    {
        public FestivalOffersHepler()
        {

        }

        public NestedItems GetSocialItemsAndSubscriptionDetailsForFetstivalWorkSheet(FreeDownloadsContent WorkSheet, WorksheetInput input, NestedItems nested)
        {
            SocialItems social = new SocialItems();
            try
            {
                LoggedIn loggedin = new LoggedIn();
                loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

                dbAccessClass db = new dbAccessClass();
                List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
                myagegroup = db.GetUserSelectedUserGroup();
                var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
                //var AgeTitleDesc = helper.Content(WorkSheet.AgeTitle?.Udi);
                //var ItemName = AgeTitleDesc?.Value("itemName");
                string cultureName = CultureName.GetCultureName().Replace("/", "");
                var PDF_File = WorkSheet?.UploadPdf;
                string DownloadString = String.Empty;

                DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "Print";

                //if (loggedin == null)
                //	DownloadString = input.CultureInfo + "$" + "0" + "$" + ItemName + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";
                //else
                //	DownloadString = input.CultureInfo + "$" + loggedin.UserId.ToString() + "$" + ItemName + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";

                string loginUrl = cultureName + "/my-account/login?ref=u";

                #region Social Share
                try
                {
                    //Share Icons activation
                    // || (loggedin != null && getUserCurrentSubscription != null)
                    if (!String.IsNullOrEmpty(WorkSheet.Facebook) || !String.IsNullOrEmpty(WorkSheet.WhatsApp) || !String.IsNullOrEmpty(WorkSheet.MailContent))
                    {
                        if (!String.IsNullOrEmpty(WorkSheet.Facebook))
                            social.FBShare = WorkSheet.Facebook;

                        if (!String.IsNullOrEmpty(WorkSheet.WhatsApp))
                            social.WhatAppShare = WorkSheet.WhatsApp;

                        if (!String.IsNullOrEmpty(WorkSheet.MailContent))
                            social.EmailShare = WorkSheet.MailContent;
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
                    if (input.CultureInfo == "/")
                        input.CultureInfo = String.Empty;

                    string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=WorksheetFestival";
                    status.DownloadUrl = downloadUrl;
                    status.DownloadString = DownloadString;
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

        public NestedItems GetSocialItemsAndSubscriptionDetailsForSpecialOfferWorkSheet(SpecialOfferItems WorkSheet, WorksheetInput input, NestedItems nested)
        {
            SocialItems social = new SocialItems();
            try
            {
                LoggedIn loggedin = new LoggedIn();
                loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

                dbAccessClass db = new dbAccessClass();
                List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
                myagegroup = db.GetUserSelectedUserGroup();
                var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
                //var AgeTitleDesc = helper.Content(WorkSheet.AgeTitle?.Udi);
                //var ItemName = AgeTitleDesc?.Value("itemName");
                string cultureName = CultureName.GetCultureName().Replace("/", "");
                var PDF_File = WorkSheet?.UploadPdf;
                string DownloadString = String.Empty;

                DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "Print";

                //if (loggedin == null)
                //	DownloadString = input.CultureInfo + "$" + "0" + "$" + ItemName + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";
                //else
                //	DownloadString = input.CultureInfo + "$" + loggedin.UserId.ToString() + "$" + ItemName + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";

                string loginUrl = cultureName + "/my-account/login?ref=u";

                //Check for get started
                int RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
                dbAccessClass dbObject = new dbAccessClass();
                string getStartedStatus = dbObject.CheckGetStaredClicked(RefUserId);

                try
                {

                    var commonContent = helper?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                                          .Where(x => x?.ContentType.Alias == "offerRoot")?.OfType<OfferRoot>()?.FirstOrDefault()?.DescendantsOrSelf()?
                                          .Where(x => x?.ContentType.Alias == "specialOffersRoot")?.OfType<SpecialOffersRoot>()?
                                          .Where(x => x.OfferName == input.Mode)?.FirstOrDefault();

					if (commonContent != null)
					{
						if ((!String.IsNullOrEmpty(commonContent.FacebookContent) || !String.IsNullOrEmpty(commonContent.WhatsAppContent) || !String.IsNullOrEmpty(commonContent.MailContent)))
						{

							string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
							string referralText = string.Empty;
							string lessonPlanUrl = domain + "summer-camp";
                            string ItemName = "Special Offer";
                            //if (loggedin != null)
                            //{
                            //	if (!String.IsNullOrWhiteSpace(commonContent.ReferralContent.ToString()) && !String.IsNullOrWhiteSpace(loggedin.ReferralCode))
                            //	{
                            //		referralText = commonContent.ReferralContent.ToString().ToString().Replace("<p>", "").Replace("</p>", "");
                            //		referralText = referralText.Replace("{referal}", loggedin.ReferralCode);

                            //		referralText = referralText.Replace("{loginurl}", domain + "my-account/login?referralcode=" + loggedin.ReferralCode);
                            //	}
                            //}

                            if (!String.IsNullOrEmpty(commonContent.FacebookContent))
							{
								string FacebookContent = commonContent.FacebookContent.Replace("{worksheeturl}", lessonPlanUrl);

								if (!String.IsNullOrEmpty(FacebookContent))
									social.FBShare = FacebookContent.Replace("{referal}", referralText);
								else
								{
									string facebookContent = FacebookContent.Replace("{referal}", "");
									social.FBShare = facebookContent;
								}
							}

							if (!String.IsNullOrEmpty(commonContent.WhatsAppContent))
							{
								string WhatsAppContent = commonContent.WhatsAppContent.Replace("{worksheeturl}", lessonPlanUrl);

								if (!String.IsNullOrEmpty(referralText))
									social.WhatAppShare = WhatsAppContent.Replace("{referal}", referralText);
								else
								{
									string whatsappContent = WhatsAppContent.Replace("{referal}", "");
									social.WhatAppShare = whatsappContent;
								}
							}

							if (!String.IsNullOrEmpty(commonContent.MailContent))
							{
								string MailContent = commonContent.MailContent.Replace("{worksheeturl}", lessonPlanUrl);

								if (!String.IsNullOrEmpty(referralText))
									social.EmailShare = MailContent.Replace("{referal}", referralText);
								else
								{
									string mailContent = MailContent.Replace("{referal}", "");
									social.EmailShare = mailContent + "`" + ItemName;
								}

							}
						}
					}
				}
                catch (Exception ex)
                {

                }

                nested.socialItems = social;

            }
            catch (Exception ex)
            {
            }
            return nested;
        }
    }
}