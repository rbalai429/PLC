using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using static HPPlc.Models.WhatsApp.NotificationData;

namespace HPPlc.Models.WhatsApp
{
	public class dbNotificationAccess
	{
		public dbNotificationAccess()
		{

		}

		dbProxy _db = new dbProxy();
		//WhatsAppNotification
		public List<NotificationData> GetNotificationData(string notificationType)
		{
			string QType = String.Empty;
			if (!String.IsNullOrWhiteSpace(notificationType) && notificationType.ToLower() == "freeuser")
				QType = "1";
			else if(!String.IsNullOrWhiteSpace(notificationType) && notificationType.ToLower() == "paid")
				QType = "2";
			else if (!String.IsNullOrWhiteSpace(notificationType) && notificationType.ToLower() == "bulk")
				QType = "4";
			else
				QType = "3";

			var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
			List<NotificationData> notificationDatas = new List<NotificationData>();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters { ParameterName = "@QType", Value =  QType}
			};

			notificationDatas = _db.GetDataMultiple<NotificationData>("USP_PLC_WhatsApp_Notification", notificationDatas, sp);

			return notificationDatas;
		}

		public List<NotificationData> Get365DaysNotificationData()
		{
			//var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
			List<NotificationData> notificationDatas = new List<NotificationData>();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters { ParameterName = "@QType", Value =  "3"}
			};

			notificationDatas = _db.GetDataMultiple<NotificationData>("USP_PLC_WhatsApp_Notification", notificationDatas, sp);

			return notificationDatas;
		}
		public async Task<GetStatus> NotificationLog(List<NotificationLog> notificationLog)
		{
			GetStatus status = new GetStatus();
			XDocument XmlDocGroup = new XDocument(new XDeclaration("1.0", "UTF - 8", "yes"),
				   new XElement("NewDataSet", from DataList in notificationLog
											  select new XElement("Notification",
													new XElement("UserId", DataList.UserId),
													new XElement("WeekId", DataList.WeekId),
													new XElement("NotificationName", DataList.NotificationName),
													new XElement("NotificationStatus", DataList.NotificationStatus),
													new XElement("NotificationJson", DataList.NotificationJson),
													new XElement("SendStatus", DataList.SendStatus),
													new XElement("MobileNo", DataList.MobileNo),
													new XElement("AgeGroup", DataList.AgeGroup),
													new XElement("Subject", DataList.Subject),
													new XElement("NotificationType", DataList.NotificationType),
													new XElement("BannerUrl", DataList.BannerUrl),
													new XElement("PdfUrl", DataList.PdfUrl),
													new XElement("TypeOfNotif", DataList.TypeOfNotif),
													new XElement("UserUniqueId", DataList.UserUniqueId),
													new XElement("WorksheetId", DataList.WorksheetId),
													new XElement("TopicName", DataList.TopicName)
											  )));

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters { ParameterName = "@XMLParameter", Value =  XmlDocGroup.ToString()}
			};

			status = await _db.StoreDataAsync("USP_PLC_WhatsApp_Notification_Log", sp);

			return status;
		}

		public async Task<GetStatus> WorksheetPageCnt(List<WorksheetFormat> pdfPageCnt,string source)
		{
			GetStatus status = new GetStatus();
			XDocument XmlDocGroup = new XDocument(new XDeclaration("1.0", "UTF - 8", "yes"),
				   new XElement("NewDataSet", from DataList in pdfPageCnt
											  select new XElement("PdfPageCnt",
													new XElement("WorksheetId", DataList.WorksheetId),
													new XElement("NoOfPage", DataList.CntOfPdfFilePage),
													new XElement("Source", source)
											  )));

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters { ParameterName = "@XMLParameter", Value =  XmlDocGroup.ToString()}
			};

			status = await _db.StoreDataAsync("USP_PLC_PdfPageCnt", sp);

			return status;
		}

		public WhatsAppWelcomeTimeZone GetNotificationWelcomeTimeZone()
		{
			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			WhatsAppWelcomeTimeZone whatsAppWelcomeTimeZone = new WhatsAppWelcomeTimeZone();
			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters { ParameterName = "@UserId", Value =  UserId.ToString()}
			};

			whatsAppWelcomeTimeZone = _db.GetData<WhatsAppWelcomeTimeZone>("USP_PLC_WhatsApp_Notification_Welcome_TimeFormat", whatsAppWelcomeTimeZone, sp);

			return whatsAppWelcomeTimeZone;
		}
	}
}