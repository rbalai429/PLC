using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Controllers.APIs
{
	public class WorksheetsClass
	{
		public string ClassName { get; set; }
		public string AgeGroup { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public List<Worksheets> Worksheets { get; set; }
	}
	public class Worksheets
	{
		public string Title { get; set; }
		public string Description { get; set; }

		public string SubjectName { get; set; }

		public string WeekName { get; set; }
		public bool IsGuestUser { get; set; }
		public MediaProp DesktopImage { get; set; }
		public MediaProp DesktopNextGenImage { get; set; }
		public MediaProp MobileImage { get; set; }
		public MediaProp MobileNextGenImage { get; set; }

		public string DocumentUrl { get; set; }

		public string FacebookContent { get; set; }
		public string WhatsAppContent { get; set; }
		public string MailContent { get; set; }
	}

	public class WorksheetsFilterClass
	{
		public string ClassValue { get; set; }
	}

	public class WorksheetsFilterSubject
	{
		public string SubjectValue { get; set; }
	}
}