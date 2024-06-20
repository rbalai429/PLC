using HPPlc.Models;
using HPPlc.Models.WhatsApp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Controllers
{
    public class NotificationController : SurfaceController
    {
        string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetWhatsAppEventTimingFreeSubscriber()
        {
            try
            {
                //Daily/Onetime
                NotificationItem notificationItem = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                        .Where(x => x.ContentType.Alias == "notificationItem")?
                        .OfType<NotificationItem>()?.Where(x => x.IsActive == true)?.FirstOrDefault();

                if (notificationItem != null)
                {
                    if (notificationItem.TypesOfNotification != null && notificationItem.IsActive == true && notificationItem.TypesOfNotification.ToLower().Equals("freeuser"))
                    {
                        TimeSpan start = new TimeSpan(notificationItem.StartTime.Hour, notificationItem.StartTime.Minute, 0); //0 o'clock like 11 PM to 11:10 AM
                        TimeSpan end = new TimeSpan(notificationItem.EndTime.Hour, notificationItem.EndTime.Minute, 0);
                        TimeSpan now = DateTime.Now.TimeOfDay;

                        if ((now > start) && (now < end))
                        {
                            await WhatsAppNotification(notificationItem);

                            return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(NotificationController), ex, message: "GetWhatsAppEventTiming-free user");
            }


            return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> WhatsAppNotification(NotificationItem notificationItem)
        {
            GetStatus responce = new GetStatus();

            try
            {
                dbNotificationAccess dbClass = new dbNotificationAccess();
                List<NotificationData> notificationDatas = new List<NotificationData>();
                List<NotificationLog> notificationLog = new List<NotificationLog>();

                if (notificationItem != null)
                {
                    notificationDatas = dbClass.GetNotificationData(notificationItem?.TypesOfNotification.ToString());
                    if (notificationDatas != null && notificationDatas.Count > 0)
                    {
                        foreach (var notif in notificationDatas)
                        {
                            //Free Notification for last week
                            if (!String.IsNullOrWhiteSpace(notif.MaxLimit) && notif.MaxLimit == "Y" && (!String.IsNullOrWhiteSpace(notif.ComWithWhatsApp) && notif.ComWithWhatsApp.ToLower() == "yes"))
                            {
                                string redirectURL = String.Empty;
                                string whatsappBannerr = String.Empty;
                                NotificationItems NotifData = notificationItem?.NotificationMapping?
                                        .Where(x => Umbraco?.Content(x.SelectWeek.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == notif.NoOfWeek.ToString())?.OfType<NotificationItems>()?.FirstOrDefault();

                                if (NotifData.BannerImage != null)
                                    whatsappBannerr = NotifData?.BannerImage?.Url();
                                else
                                {
                                    whatsappBannerr = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                                                                .Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()?.FirstOrDefault()?.FreeUserLastWeekBanner?.Url();
                                }

                                if (String.IsNullOrWhiteSpace(NotifData.RedirectUrl))
                                {
                                    redirectURL = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
                                                    .Where(c => Umbraco?.Content(c?.AgeGroup?.Udi).DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == notif.MinAgegroup)?.FirstOrDefault()?.Url();
                                }
                                else
                                {
                                    redirectURL = NotifData?.RedirectUrl;
                                }

                                if (!String.IsNullOrWhiteSpace(redirectURL))
                                {
                                    if (!redirectURL.Contains("https://"))
                                        redirectURL = domain + redirectURL;

                                    redirectURL = redirectURL + "?jumpid=mb_in_oc_mk_ot_cm017589_aw_cr&utm_source=mobile_apps";
                                }

                                notificationLog.Add(SendNotification(NotifData, notif, whatsappBannerr, redirectURL, "", 0, "", notificationItem?.TypesOfNotification, "I"));
                            }
                            else if (!String.IsNullOrWhiteSpace(notif.ComWithWhatsApp) && notif.ComWithWhatsApp.ToLower() == "yes") //Free Notification for before last week
                            {
                                //Get Subject using User Minimum Age group
                                var worksheet = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                                .Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
                                .Where(c => Umbraco?.Content(c?.AgeGroup?.Udi).DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == notif.MinAgegroup)?.FirstOrDefault()?
                                .DescendantsOrSelf()?.Where(b => b.ContentType.Alias == "worksheetRoot")?.OfType<WorksheetRoot>()?
                                .Where(v => Umbraco?.Content(v?.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == notif.NoOfWeek.ToString())?
                                .FirstOrDefault();

                                if (worksheet != null)
                                {
                                    NotificationItems NotifData = null;
                                    if (notif?.NoOfWeek > 0 && worksheet?.SelectSubject != null)
                                    {
                                        NotifData = notificationItem?.NotificationMapping?
                                            .Where(x => Umbraco?.Content(x?.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == notif?.NoOfWeek.ToString() &&
                                            Umbraco?.Content(x?.NotificationSubjects?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == Umbraco?.Content(worksheet?.SelectSubject?.Udi)?
                                            .DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue)?.OfType<NotificationItems>()?.FirstOrDefault();
                                    }

                                    if (NotifData != null)
                                    {
                                        string whatsappBanner = String.Empty;
                                        string downloadUrl = String.Empty;
                                        if (String.IsNullOrWhiteSpace(worksheet?.WhatsAppBanner?.Url()))
                                        {
                                            whatsappBanner = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                                                            .Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()?.FirstOrDefault()?.WhatsAppBannerDefault?.Url();
                                        }
                                        else
                                        {
                                            whatsappBanner = worksheet?.WhatsAppBanner?.Url();
                                        }

                                        downloadUrl = " " + domain + "worksheet-download?d=" + clsCommon.Encryptwithbase64Code("WID=" + worksheet?.Id.ToString() + "&UID=" + notif.UserId.ToString()) + "&jumpid=mb_in_oc_mk_ot_cm017589_aw_cr&utm_source=mobile_apps";

                                        if (String.IsNullOrWhiteSpace(downloadUrl))
                                            downloadUrl = domain;

                                        if (NotifData != null && notif != null && !String.IsNullOrWhiteSpace(whatsappBanner) && !String.IsNullOrWhiteSpace(downloadUrl) && !String.IsNullOrWhiteSpace(notificationItem.TypesOfNotification) && !String.IsNullOrWhiteSpace(worksheet.SelectSubject.Name))
                                        {
                                            notificationLog.Add(SendNotification(NotifData, notif, whatsappBanner, downloadUrl, worksheet?.Topic?.Name, worksheet.Id, worksheet?.SelectSubject?.Name, notificationItem?.TypesOfNotification, ""));
                                        }
                                        else
                                        {
                                            Logger.Error(reporting: typeof(NotificationController), null, message: "whatsappBanner or UploadPdf is empty");
                                        }
                                    }
                                    else
                                    {
                                        Logger.Error(reporting: typeof(NotificationController), null, message: "NotifData is empty");
                                    }
                                }
                            }

                        }

                        if (notificationLog != null && notificationLog.Count > 0)
                        {
                            await dbClass.NotificationLog(notificationLog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(NotificationController), ex, message: "WhatsAppNotification");
            }

            return "Done";
        }

        private NotificationLog SendNotification(NotificationItems NotifData, NotificationData notifypersonaldata, string BannerImage, string pdfUrl, string TopicName, int worksheetId, string SubjectName = "", string notificationtype = "", string TypeOfNotif = "")
        {
            string mobile = String.Empty;
            string name = String.Empty;

            WhatsAppDynamicValue lst = new WhatsAppDynamicValue();
            NotificationLog notificationLog = new NotificationLog();

            if (NotifData != null)
            {
                if (!String.IsNullOrWhiteSpace(notifypersonaldata.MobileNo) && !String.IsNullOrWhiteSpace(NotifData?.NotificationCode) && !String.IsNullOrWhiteSpace(notifypersonaldata.MinAgegroup) && notifypersonaldata.NoOfWeek > 0)
                {
                    mobile = clsCommon.Decrypt(notifypersonaldata.MobileNo);
                    if (!String.IsNullOrWhiteSpace(mobile) && (mobile.Substring(0, 3) == "+91" || mobile.Length == 10))
                        mobile = "91" + mobile;

                    //name
                    if (!String.IsNullOrWhiteSpace(notifypersonaldata.Name))
                    {
                        name = clsCommon.Decrypt(notifypersonaldata.Name);
                        lst.Name = name;
                    }

                    if (notifypersonaldata.MaxLimit != null && notifypersonaldata.MaxLimit == "Y")
                    {
                        //Banner url
                        if (!String.IsNullOrWhiteSpace(BannerImage))
                        {
                            if (BannerImage.Contains("https://"))
                                lst.BannerUrl = BannerImage;
                            else
                                lst.BannerUrl = domain + BannerImage;
                        }

                        //Domain
                        if (!String.IsNullOrWhiteSpace(pdfUrl))
                            lst.PdfUrl = pdfUrl;
                    }
                    else
                    {
                        //Banner url
                        if (!String.IsNullOrWhiteSpace(BannerImage) && BannerImage.Contains("https://"))
                            lst.BannerUrl = BannerImage;
                        else
                            lst.BannerUrl = domain + BannerImage;

                        //subject
                        if (!String.IsNullOrWhiteSpace(SubjectName) && !String.IsNullOrWhiteSpace(NotifData?.NotificationSubjects?.Name))
                            lst.Subjects = SubjectName;

                        //download url
                        if (!String.IsNullOrWhiteSpace(pdfUrl))
                            lst.PdfUrl = pdfUrl;
                    }

                    WhatsAppHelper whatsAppHelper = new WhatsAppHelper();
                    IRestResponse response = whatsAppHelper.CreateMessage(mobile, NotifData.NotificationCode, lst);

                    if (response != null)
                    {
                        notificationLog.UserId = notifypersonaldata.UserId;
                        notificationLog.WeekId = notifypersonaldata.NoOfWeek;
                        notificationLog.NotificationName = NotifData.NotificationCode;
                        notificationLog.NotificationStatus = 1;
                        notificationLog.NotificationJson = response.Content;
                        notificationLog.SendStatus = response.StatusCode.ToString();
                        notificationLog.MobileNo = notifypersonaldata.MobileNo;
                        notificationLog.AgeGroup = notifypersonaldata.MinAgegroup;
                        notificationLog.Subject = SubjectName;
                        notificationLog.NotificationType = notificationtype;
                        notificationLog.BannerUrl = BannerImage;
                        notificationLog.PdfUrl = pdfUrl;
                        notificationLog.TypeOfNotif = TypeOfNotif;
                        notificationLog.UserUniqueId = "";
                        notificationLog.TopicName = TopicName;
                        notificationLog.WorksheetId = worksheetId;
                    }
                }
            }

            return notificationLog;

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadDoc(string downpm)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(downpm))
                {
                    Uri myUri = new Uri(downpm);
                    string param = HttpUtility.ParseQueryString(myUri.Query).Get("d");

                    string dec = clsCommon.DecryptWithBase64Code(param);
                    if (!String.IsNullOrWhiteSpace(dec))
                    {
                        string[] queryvar = dec.Split('&');
                        string desType = "WhatsApp";

                        if (queryvar.Length >= 2)
                        {
                            foreach (var qs in queryvar)
                            {
                                if (qs == "type")
                                    desType = queryvar[1].Replace("type=", "");
                            }
                            //sp365d

                            desType = HttpUtility.ParseQueryString(myUri.Query).Get("type");
                            if (String.IsNullOrWhiteSpace(desType))
                                desType = "WhatsApp";

                            if (!String.IsNullOrWhiteSpace(desType) && desType == "sp365d")
                            {
                                string UserUniqueId = queryvar[1].Replace("UID=", "");
                                SessionManagement.StoreInSession(SessionType.UserUniqueId, UserUniqueId);
                                SessionManagement.StoreInSession(SessionType.UserId, 0);
                            }
                            else
                            {
                                int UserId = int.Parse(queryvar[1].Replace("UID=", ""));
                                SessionManagement.StoreInSession(SessionType.UserId, UserId);
                                SessionManagement.StoreInSession(SessionType.UserUniqueId, "");
                            }


                            string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(queryvar[0].ToString().Replace("WID=", "")) + "&source=" + desType;

                            return Json(new { status = "done", navigation = downloadUrl, message = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { status = "fail", navigation = "/", message = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = "fail", navigation = "/", message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(WorkSheetController), ex, message: "DownloadDoc");
            }

            return View();
        }

        [HttpPost]
        public JsonResult DownloadPDF_SFMC(string worksheetId, string pageId)
        {
            try
            {
                if (pageId == "Topic")
                {
                    var topicId = Umbraco?.Content(Umbraco?.Content(worksheetId)?.DescendantsOrSelf()?.OfType<TopicsName>()?.FirstOrDefault()?.TopicMapping.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue;
                    if (topicId != null)
                    {
                        var worksheet = Umbraco?.Content(worksheetId)?.Parent?.Children?.Where(x => x?.ContentType.Alias == "worksheetRoot")?
                            .OfType<WorksheetRoot>()?.Where(c => Umbraco?.Content(c?.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue.ToString() == topicId.ToString())?.FirstOrDefault();

                        if (worksheet != null)
                        {
                            string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(worksheet.Id.ToString()) + "&source=SFMC";

                            return Json(new { status = "done", navigation = downloadUrl, message = "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { status = "fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else if (pageId == "Free")
                {
                    string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(worksheetId.ToString()) + "&source=sfmcfreecontent";

                    return Json(new { status = "done", navigation = downloadUrl, message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(HomeController), ex, message: "Login - download pdf file");
            }

            return Json(new { status = "fail", navigation = "/", message = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DownloadPDF_SFMCPaid(string downpm)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(downpm))
                {
                    Uri myUri = new Uri(downpm);
                    string UserId = HttpUtility.ParseQueryString(myUri.Query).Get("user");
                    string WkstId = HttpUtility.ParseQueryString(myUri.Query).Get("worksheet");

                    if (!String.IsNullOrWhiteSpace(UserId) && !String.IsNullOrWhiteSpace(WkstId))
                    {
                        UserId = clsCommon.DecryptWithBase64Code(UserId);
                        WkstId = clsCommon.DecryptWithBase64Code(WkstId);

                        SessionManagement.StoreInSession(SessionType.UserId, int.Parse(UserId));

                        var worksheet = Umbraco?.Content(WkstId)?.DescendantsOrSelf()?.Where(x => x?.ContentType?.Alias == "worksheetRoot")?
                            .OfType<WorksheetRoot>()?.FirstOrDefault();

                        if (worksheet != null)
                        {
                            string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(worksheet.Id.ToString()) + "&source=sfmcpaid";

                            return Json(new { status = "done", navigation = downloadUrl, message = "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { status = "fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(HomeController), ex, message: "Login - SFMC paid download pdf file");
            }

            return Json(new { status = "fail", navigation = "/", message = "" }, JsonRequestBehavior.AllowGet);
        }
        //Paid Subscriber Start
        public async Task<ActionResult> GetWhatsAppEventTimingPaidSubscriber()
        {
            try
            {
                //Daily/Onetime
                NotificationForPaidUser notificationItem = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                        .Where(x => x.ContentType.Alias == "notificationForPaidUser")?
                        .OfType<NotificationForPaidUser>()?.Where(x => x.IsActive == true)?.FirstOrDefault();

                if (notificationItem != null)
                {
                    if (notificationItem.IsActive == true)
                    {
                        TimeSpan start = new TimeSpan(notificationItem.StartTime.Hour, notificationItem.StartTime.Minute, 0); //0 o'clock like 11 PM to 11:10 AM
                        TimeSpan end = new TimeSpan(notificationItem.EndTime.Hour, notificationItem.EndTime.Minute, 0);
                        TimeSpan now = DateTime.Now.TimeOfDay;

                        if ((now > start) && (now < end))
                        {
                            await WhatsAppNotificationForPaid(notificationItem);

                            return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(NotificationController), ex, message: "GetWhatsAppEventTiming-free user");
            }


            return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> WhatsAppNotificationForPaid(NotificationForPaidUser notificationItem)
        {
            GetStatus responce = new GetStatus();

            try
            {
                dbNotificationAccess dbClass = new dbNotificationAccess();
                List<NotificationData> notificationDatas = new List<NotificationData>();
                List<NotificationLog> notificationLog = new List<NotificationLog>();

                if (notificationItem != null)
                {
                    notificationDatas = dbClass.GetNotificationData("paid");
                    if (notificationDatas != null && notificationDatas.Count > 0)
                    {
                        foreach (var notif in notificationDatas)
                        {
                            //Get Subject using User Minimum Age group
                            var worksheet = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
                            .Where(c => Umbraco?.Content(c?.AgeGroup?.Udi).DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == notif.MinAgegroup)?.FirstOrDefault()?
                            .DescendantsOrSelf()?.Where(b => b.ContentType.Alias == "worksheetRoot")?.OfType<WorksheetRoot>()?
                            .Where(v => Umbraco?.Content(v?.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == notif.NoOfWeek.ToString())?
                            .FirstOrDefault();


                            if (worksheet != null)
                            {
                                //Check User Subscriptionenabled for this worksheet
                                List<GetYourSubscriptionDetails> subscriptionDetails = new List<GetYourSubscriptionDetails>();
                                GetMySubscription getMySubscription = new GetMySubscription();

                                subscriptionDetails = getMySubscription.mySubscriptions(notif.UserId);

                                if (subscriptionDetails != null && subscriptionDetails.Count > 0)
                                {
                                    //min. age group
                                    var subscriptiondata = subscriptionDetails.Where(x => x.AgeGroup == notif.MinAgegroup).FirstOrDefault();
                                    if (subscriptiondata != null && !String.IsNullOrWhiteSpace(subscriptiondata.Ranking))
                                    {
                                        //Check user have subscription for this worksheet.
                                        var worksheetSubscriptionExistsWithUser = worksheet?.Subscription?.Where(x => Umbraco.Content(x?.Udi).DescendantsOrSelf().OfType<Subscriptions>().Any(c => c.Ranking == subscriptiondata.Ranking));
                                        if (worksheetSubscriptionExistsWithUser != null)
                                        {
                                            NotificationItems1 NotifData = null;
                                            if (notif?.NoOfWeek > 0 && worksheet?.SelectSubject != null && worksheet?.AgeTitle != null && notif.MinAgegroup != null)
                                            {
                                                NotifData = notificationItem?.NotificationMapping?
                                                    .Where(x => Umbraco?.Content(x?.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == notif?.NoOfWeek.ToString() &&
                                                    Umbraco.Content(x?.ClassName?.Udi).DescendantsOrSelf().OfType<NameListItem>().ToList().Any(c => c?.ItemValue == notif.MinAgegroup) &&
                                                    Umbraco?.Content(x?.NotificationSubjects?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == Umbraco?.Content(worksheet?.SelectSubject?.Udi)?
                                                    .DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue)?.OfType<NotificationItems1>()?.FirstOrDefault();
                                            }

                                            if (NotifData != null)
                                            {
                                                string whatsappBanner = String.Empty;
                                                string downloadUrl = String.Empty;
                                                if (String.IsNullOrWhiteSpace(worksheet?.WhatsAppBanner?.Url()))
                                                {
                                                    whatsappBanner = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                                                                    .Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()?.FirstOrDefault()?.WhatsAppBannerDefault?.Url();
                                                }
                                                else
                                                {
                                                    whatsappBanner = worksheet?.WhatsAppBanner?.Url();
                                                }

                                                //downloadUrl = " " + domain + "worksheet-download?d=" + clsCommon.Encryptwithbase64Code("WID=" + worksheet?.Id.ToString() + "&UID=" + notif.UserId.ToString());
                                                if (NotifData?.IsRedirectUrl?.ToLower() == "no")
                                                    downloadUrl = " " + domain + "my-account/login?d=" + clsCommon.Encryptwithbase64Code("WID=" + worksheet?.Id.ToString() + "&UID=" + notif.UserId.ToString()) + "&jumpid=mb_in_oc_mk_ot_cm017589_aw_cr&utm_source=mobile_apps";
                                                else
                                                    downloadUrl = NotifData?.RedirectUrl;

                                                if (String.IsNullOrWhiteSpace(downloadUrl))
                                                    downloadUrl = domain;

                                                if (NotifData != null && notif != null && !String.IsNullOrWhiteSpace(whatsappBanner) && !String.IsNullOrWhiteSpace(downloadUrl) && !String.IsNullOrWhiteSpace(worksheet.SelectSubject.Name))
                                                {
                                                    notificationLog.Add(SendNotificationPaidUser(NotifData, notif, whatsappBanner, downloadUrl, worksheet?.Topic?.Name, worksheet.Id, worksheet?.SelectSubject?.Name, "paid"));
                                                }
                                                else
                                                {
                                                    Logger.Info(reporting: typeof(NotificationController), message: "whatsappBanner or UploadPdf is empty");
                                                }
                                            }
                                            else
                                            {
                                                Logger.Info(reporting: typeof(NotificationController), message: "NotifData is empty");
                                            }
                                        }
                                        else
                                        {
                                            Logger.Info(reporting: typeof(NotificationController), message: "Subscription Details with min age is empty");
                                        }
                                    }
                                    else
                                    {
                                        Logger.Info(reporting: typeof(NotificationController), message: "worksheetSubscriptionExistsWithUser is empty");
                                    }
                                }
                                else
                                {
                                    Logger.Info(reporting: typeof(NotificationController), message: "Subscription Details is empty");
                                }
                            }
                            else
                            {
                                Logger.Info(reporting: typeof(NotificationController), message: "Worksheet is empty");
                            }
                        }

                        if (notificationLog != null && notificationLog.Count > 0)
                        {
                            await dbClass.NotificationLog(notificationLog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(NotificationController), ex, message: "WhatsAppNotification - Paid");
            }

            return "Done";
        }

        private NotificationLog SendNotificationPaidUser(NotificationItems1 NotifData, NotificationData notifypersonaldata, string BannerImage, string pdfUrl, string TopicName, int WorksheetId, string SubjectName = "", string notificationtype = "")
        {
            string mobile = String.Empty;
            string name = String.Empty;

            WhatsAppDynamicValue lst = new WhatsAppDynamicValue();
            NotificationLog notificationLog = new NotificationLog();

            if (NotifData != null)
            {
                if (!String.IsNullOrWhiteSpace(notifypersonaldata.MobileNo) && !String.IsNullOrWhiteSpace(NotifData?.NotificationCode) && !String.IsNullOrWhiteSpace(notifypersonaldata.MinAgegroup) && notifypersonaldata.NoOfWeek > 0)
                {
                    mobile = clsCommon.Decrypt(notifypersonaldata.MobileNo);
                    if (!String.IsNullOrWhiteSpace(mobile) && (mobile.Substring(0, 3) == "+91" || mobile.Length == 10))
                        mobile = "91" + mobile;

                    //name
                    if (!String.IsNullOrWhiteSpace(notifypersonaldata.Name))
                    {
                        name = clsCommon.Decrypt(notifypersonaldata.Name);
                        lst.Name = name;
                    }

                    if (notifypersonaldata.MaxLimit != null && notifypersonaldata.MaxLimit == "Y")
                    {
                        //Banner url
                        if (!String.IsNullOrWhiteSpace(BannerImage))
                        {
                            if (BannerImage.Contains("https://"))
                                lst.BannerUrl = BannerImage;
                            else
                                lst.BannerUrl = domain + BannerImage;
                        }

                        //Domain
                        if (!String.IsNullOrWhiteSpace(domain))
                            lst.PdfUrl = domain + "subscription";
                    }
                    else
                    {
                        //Banner url
                        if (!String.IsNullOrWhiteSpace(BannerImage) && BannerImage.Contains("https://"))
                            lst.BannerUrl = BannerImage;
                        else
                            lst.BannerUrl = domain + BannerImage;

                        //subject
                        if (!String.IsNullOrWhiteSpace(SubjectName) && !String.IsNullOrWhiteSpace(NotifData?.NotificationSubjects?.Name))
                            lst.Subjects = SubjectName;

                        //download url
                        if (!String.IsNullOrWhiteSpace(pdfUrl))
                            lst.PdfUrl = pdfUrl;
                    }

                    WhatsAppHelper whatsAppHelper = new WhatsAppHelper();
                    IRestResponse response = whatsAppHelper.CreateMessage(mobile, NotifData.NotificationCode, lst);

                    if (response != null)
                    {
                        notificationLog.UserId = notifypersonaldata.UserId;
                        notificationLog.WeekId = notifypersonaldata.NoOfWeek;
                        notificationLog.NotificationName = NotifData.NotificationCode;
                        notificationLog.NotificationStatus = 1;
                        notificationLog.NotificationJson = response.Content;
                        notificationLog.SendStatus = response.StatusCode.ToString();
                        notificationLog.MobileNo = notifypersonaldata.MobileNo;
                        notificationLog.AgeGroup = notifypersonaldata.MinAgegroup;
                        notificationLog.Subject = SubjectName;
                        notificationLog.NotificationType = notificationtype;
                        notificationLog.BannerUrl = BannerImage;
                        notificationLog.PdfUrl = pdfUrl;
                        notificationLog.UserUniqueId = "";
                        notificationLog.TopicName = TopicName;
                        notificationLog.WorksheetId = WorksheetId;
                    }
                }
            }

            return notificationLog;

        }

        public ActionResult GetWhatsAppForPlan365Days()
        {
            try
            {
                List<NotificationData> notificationDatas = new List<NotificationData>();

                dbNotificationAccess dbClass = new dbNotificationAccess();
                notificationDatas = dbClass.Get365DaysNotificationData();
                if (notificationDatas != null && notificationDatas.Count > 0)
                {
                    foreach (var data in notificationDatas)
                    {
                        SendWhatsAppForSpecialPlan(data.NoOfDays, data.UserId, data.UserUniqueCode, data.MobileNo);
                    }

                    return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(NotificationController), ex, message: "GetWhatsAppForPlan365Days user");
            }


            return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
        }


        public string SendWhatsAppForSpecialPlan(int day, int UserId, string userUniqueId, string whatsAppNo)
        {
            var notificationItem = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                        .Where(x => x.ContentType.Alias == "specialPlanNotification")?
                            .OfType<SpecialPlanNotification>()?.Where(x => x.IsActive == true)?.FirstOrDefault();

            string notificationCode = notificationItem?.NotificationMapping?.Where(c => Umbraco.Content(c?.SelectDay?.Udi)?.DescendantsOrSelf()?
                .OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == day.ToString())?.FirstOrDefault()?.NotificationCode;

            if (!String.IsNullOrWhiteSpace(notificationCode))
            {
                SpecialDaysItems worksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
                                                            .DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "specialPlanRoot")?.OfType<SpecialPlanRoot>()?
                                                            .FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "planAgeGroup")?
                                                            .OfType<PlanAgeGroup>()?.Where(c => c.IsActive == true)?.FirstOrDefault()?
                                                            .Children?.Where(c => c.ContentType.Alias == "specialDaysItems")?.OfType<SpecialDaysItems>()?
                                                            .Where(v => v.IsActive == true && Umbraco.Content(v?.NoOfDays?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == day.ToString())?.FirstOrDefault();

                if (!String.IsNullOrWhiteSpace(whatsAppNo))
                {
                    whatsAppNo = clsCommon.Decrypt(whatsAppNo);
                    //mobile = "91" + mobile;
                    if (!String.IsNullOrWhiteSpace(whatsAppNo) && (whatsAppNo.Substring(0, 3) == "+91" || whatsAppNo.Length == 10))
                        whatsAppNo = "91" + whatsAppNo;

                    string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
                    WhatsAppDynamicValue lst = new WhatsAppDynamicValue();
                    WhatsAppHelper whatsAppHelper = new WhatsAppHelper();

                    if (notificationItem != null)
                    {
                        if (!String.IsNullOrWhiteSpace(notificationCode))
                        {
                            if (worksheet != null)
                            {
                                if (worksheet != null)
                                {
                                    if (worksheet?.WhatsAppShareBanner != null)
                                        lst.BannerUrl = worksheet.WhatsAppShareBanner.Url();
                                }

                                string downloadUrl = "worksheet-download?d=" + clsCommon.Encryptwithbase64Code("WID=" + worksheet?.Id.ToString() + "&UID=" + userUniqueId) + "&jumpid=mb_in_oc_mk_ot_cm017595_aw_ot&utm_source=mobile_apps&utm_medium=other_social&utm_campaign=sep_0709&type=sp365d";
                                lst.PdfUrl = downloadUrl;

                                if (!String.IsNullOrWhiteSpace(lst.PdfUrl) && !String.IsNullOrWhiteSpace(whatsAppNo) && !String.IsNullOrWhiteSpace(notificationCode))
                                {
                                    IRestResponse response = whatsAppHelper.CreateMessageForSpecialPlan(whatsAppNo, notificationCode, lst);

                                    if (response != null)
                                    {
                                        dbNotificationAccess dbClass = new dbNotificationAccess();
                                        List<NotificationLog> notificationLog = new List<NotificationLog>();

                                        notificationLog.Add(new NotificationLog { UserId = UserId, WeekId = day, NotificationName = notificationCode, NotificationStatus = 1, NotificationJson = response.Content, SendStatus = response.StatusCode.ToString(), TypeOfNotif = "S", UserUniqueId = userUniqueId, WorksheetId = worksheet.Id, TopicName = "", Subject = "", AgeGroup = worksheet?.AgeGroup?.Name });

                                        GetStatus status = Task.Run(() => dbClass.NotificationLog(notificationLog)).Result;
                                    }
                                }
                                else
                                {
                                    Logger.Info(reporting: typeof(HomeContainer), "WhatsApp SpecialPlan Message - parameter missing");
                                }
                            }
                            else
                            {
                                Logger.Info(reporting: typeof(HomeContainer), "WhatsApp SpecialPlan Message - worksheet missing");
                            }
                        }
                        else
                        {
                            Logger.Info(reporting: typeof(HomeContainer), "WhatsApp SpecialPlan Message - notificationcode missing");
                        }
                    }
                    else
                    {
                        Logger.Info(reporting: typeof(HomeContainer), "WhatsApp SpecialPlan Message - notification item");
                    }

                    return "ok";
                }
            }
            return "fail";
        }

        public async Task<string> BulkMessageNotification(string planName)
        {
            string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
            string whatsAppNo = String.Empty;
            var notificationItem = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                        .Where(x => x.ContentType.Alias == "bulkNotificationEventWise")?.OfType<BulkNotificationEventWise>()?.FirstOrDefault();

            if (notificationItem != null)
            {
                string notificationCode = notificationItem?.NotificationMapping.Where(c => c.NotificationEvent == planName)?.FirstOrDefault()?.NotificationCode;

                if (!String.IsNullOrWhiteSpace(notificationCode))
                {
                    dbNotificationAccess dbClass = new dbNotificationAccess();
                    List<NotificationData> notificationDatas = new List<NotificationData>();
                    List<NotificationLog> notificationLog = new List<NotificationLog>();

                    if (notificationItem != null)
                    {
                        notificationDatas = dbClass.GetNotificationData("bulk");

                        if (notificationDatas != null)
                        {
                            foreach (var items in notificationDatas)
                            {
                                if (!String.IsNullOrWhiteSpace(items.MobileNo))
                                {
                                    try
                                    {
                                        whatsAppNo = clsCommon.Decrypt(items.MobileNo);
                                        //mobile = "91" + mobile;
                                        if (!String.IsNullOrWhiteSpace(whatsAppNo) && (whatsAppNo.Substring(0, 3) == "+91" || whatsAppNo.Length == 10))
                                            whatsAppNo = "91" + whatsAppNo;

                                        WhatsAppDynamicValue lst = new WhatsAppDynamicValue();
                                        WhatsAppHelper whatsAppHelper = new WhatsAppHelper();

                                        var headerContent = notificationItem?.NotificationMapping.Where(c => c.NotificationEvent == planName)?.FirstOrDefault();

                                        if (headerContent != null)
                                        {
                                            var headerContentIsBannerOrVideo = headerContent?.IsBannerOrVideo;
                                            if (headerContentIsBannerOrVideo != null)
                                            {
                                                if (headerContentIsBannerOrVideo == "Banner")
                                                {
                                                    var bannerImage = headerContent?.BannerUrl;
                                                    if (bannerImage != null)
                                                    {
                                                        if (bannerImage.Url().Contains("https://"))
                                                            lst.BannerUrl = bannerImage.Url();
                                                        else
                                                            lst.BannerUrl = domain + bannerImage.Url();

                                                        lst.HeaderIsBannerOrVideo = headerContentIsBannerOrVideo;
                                                    }
                                                }
                                                else if (headerContentIsBannerOrVideo == "Video")
                                                {
                                                    var bannerVideo = headerContent?.VideoUrl;
                                                    if (bannerVideo != null)
                                                    {
                                                        lst.VideoUrl = bannerVideo;

                                                        lst.HeaderIsBannerOrVideo = headerContentIsBannerOrVideo;
                                                    }
                                                }
                                            }
                                        }

                                        //if (worksheet != null)
                                        //{
                                        //	if (worksheet?.WhatsAppShareBanner != null)
                                        //		lst.BannerUrl = worksheet.WhatsAppShareBanner.Url();

                                        //string downloadUrl = "worksheet-download?d=" + clsCommon.Encryptwithbase64Code("WID=" + worksheet?.Id.ToString() + "&UID=" + items.UserUniqueCode) + "&jumpid=mb_in_oc_mk_ot_cm017595_aw_ot&utm_source=mobile_apps&utm_medium=other_social&utm_campaign=sep_0709&type=sp365d";
                                        //lst.PdfUrl = downloadUrl;

                                        if (!String.IsNullOrWhiteSpace(whatsAppNo) && !String.IsNullOrWhiteSpace(notificationCode))
                                        {
                                            IRestResponse response = whatsAppHelper.CreateMessageForBulk(whatsAppNo, notificationCode, lst);

                                            if (response != null)
                                            {

                                                notificationLog.Add(new NotificationLog { UserId = items.UserId, WeekId = 0, NotificationType = planName, NotificationName = notificationCode, NotificationStatus = 1, NotificationJson = response.Content, SendStatus = response.StatusCode.ToString(), TypeOfNotif = "", UserUniqueId = items.UserUniqueCode, WorksheetId = 0, TopicName = "", Subject = "", AgeGroup = "" });
                                            }
                                        }
                                        else
                                        {
                                            Logger.Info(reporting: typeof(HomeContainer), "WhatsApp Bulk Message - parameter missing");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(reporting: typeof(NotificationController), ex, message: "BulkMessageNotification user");
                                    }
                                }
                            }
                        }


                        GetStatus status = Task.Run(() => dbClass.NotificationLog(notificationLog)).Result;

                        return "ok";
                    }

                    return "fail";
                }
            }

            return "";
        }

        #region whatsapp worksheets notifications

        public async Task SendWhatsappNotification()
        {
            var whatsappNotificationEvents = Umbraco.ContentAtRoot()?
                    .Where(x => x.ContentType.Alias == Home.ModelTypeAlias)?
                    .FirstOrDefault()?
                    .DescendantsOrSelf()?
                        .Where(x => x.ContentType.Alias == WhatsappNotifications.ModelTypeAlias)?
                        .OfType<WhatsappNotifications>();

            // TODO: call SP to get users list 
            // TODO: send whatsapp notifiaction to users

            await SendWhatsAppNotification(whatsappNotificationEvents, "1", "DLE");
        }

        public async Task<ActionResult> SendWhatsAppNotification
            (IEnumerable<WhatsappNotifications> whatsappNotificationEvents, string numberOfDays, string eventCodes)
        {
            try
            {
                var whatsappNotificationEvent = whatsappNotificationEvents?
                        .Select(x => x.WhatsappNotificationEvents.FirstOrDefault(y => y.NumberOfDays == numberOfDays))?
                        .FirstOrDefault();

                if (whatsappNotificationEvent != null)
                {
                    //var 
                    //if (notificationItem.TypesOfNotification != null && notificationItem.IsActive == true && notificationItem.TypesOfNotification.ToLower().Equals("freeuser"))
                    //{
                    //    TimeSpan start = new TimeSpan(notificationItem.StartTime.Hour, notificationItem.StartTime.Minute, 0); //0 o'clock like 11 PM to 11:10 AM
                    //    TimeSpan end = new TimeSpan(notificationItem.EndTime.Hour, notificationItem.EndTime.Minute, 0);
                    //    TimeSpan now = DateTime.Now.TimeOfDay;

                    //    if ((now > start) && (now < end))
                    //    {
                    //        await WhatsAppNotification(notificationItem);

                    //        return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(NotificationController), ex, message: "GetWhatsAppEventTiming-free user");
            }


            return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}