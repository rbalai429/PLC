using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Reports.Models
{
	public class clsReferralDetailReport
	{
		public string u_name { get; set; }
		public string u_email { get; set; }
		public string u_whatsappno_prefix { get; set; }
		public string u_whatsappno { get; set; }
		public int IsActive { get; set; }
		public string ReferrerCode { get; set; }
		public DateTime DOC { get; set; }
		public string RefereeName { get; set; }
		public string RefereeEmail { get; set; }
		public string RefereeWPrefix { get; set; }
		public string RefereeWNumber { get; set; }
		public int RewardReferralInMonths { get; set; }
		public int RewardReferralInDays { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}