using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Reports
{
    public class WorkSheetDownloadReport
    {
		
		public string userUniqueId
		{
			get; set;
		}
		public string u_name
		{
			get; set;
		}
		public string u_email
		{
			get; set;
		}
		
		public string u_whatsappno
		{
			get; set;
		}
		
		public string Mode
		{
			get; set;
		}
		public string WorkshhetPDFUrl
		{
			get; set;
		}
		public string UserType
		{
			get; set;
		}
		public DateTime? InsertedOn
		{
			get;set;
		}
		public string AgeGroup
		{
			get; set;
		}
	}
}