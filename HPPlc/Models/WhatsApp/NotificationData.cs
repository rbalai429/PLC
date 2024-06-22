using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WhatsApp
{
	public class NotificationData
	{
		public int UserId { get; set; }
		public int NoOfWeek { get; set; }
		public string Name { get; set; }
		public string MobileNo { get; set; }
		public string NotificationName { get; set; }
		public int ResponseCode { get; set; }
		public string ResponseMessage { get; set; }
		public string MinAgegroup { get; set; }

		public string MaxLimit { get; set; }
		public string ComWithEmail { get; set; }
		public string ComWithWhatsApp { get; set; }
		public string UserUniqueCode { get; set; } = "";
		public int NoOfDays { get; set; } = 0;

		//Added by v
		public Int32 PlanRemainingDays { get; set; }
		public string NotificationTypeCode { get; set; }
		public string CustomerType { get; set; }
	}

	public class NotificationLog
	{
		public int UserId { get; set; }
		public int WeekId { get; set; }

		public string NotificationName { get; set; }
		public string NotificationType { get; set; }

		public int NotificationStatus { get; set; }
		public string NotificationJson { get; set; }

		public string SendStatus { get; set; }
		public string MobileNo { get; set; }
		public string AgeGroup { get; set; }
		public string Subject { get; set; }
		public string PdfUrl { get; set; }
		public string BannerUrl { get; set; }
		public string TypeOfNotif { get; set; }
		public string UserUniqueId { get; set; }
		public string TopicName { get; set; }
		public int WorksheetId { get; set; }
	}
	
	public class ScheduleTimer
	{
		public int StartTimeHour { get; set; }
		public int StartTimeMinute { get; set; }
		public int EndTimeHour { get; set; }
		public int EndTimeMinute { get; set; }
		public string Mode { get; set; }
	}


	public class WhatsAppDynamicValue
	{
		public string Name { get; set; }
		public string Domain { get; set; }
		public string Subjects { get; set; }
		public string PdfUrl { get; set; }
		public string HeaderIsBannerOrVideo { get; set; }
		public string BannerUrl { get; set; }
		public string VideoUrl { get; set; }
	}

	public class WhatsAppWelcomeTimeZone
	{
		public string MinAgeGroup { get; set; }
		public string TimeFormat { get; set; }

		public int WeekId { get; set; }
	}
}