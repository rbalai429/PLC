using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Reports.Models
{
	public class clsSubscriptionReport
	{
		public string u_name { get; set; }
		public string u_email { get; set; }
		public string Ranking { get; set; }
		public string SubscriptionName { get; set; }
		public string SubscriptionPrice { get; set; }
		public string SubscriptionDuration { get; set; }
		public string AgeGroup { get; set; }
		public DateTime SubscriptionStartDate { get; set; }
		public DateTime SubscriptionEndDate { get; set; }
		public DateTime DOC { get; set; }
		public int IsActive { get; set; }
		public string PaymentStatus { get; set; }
		public DateTime PaymentDate { get; set; }
		public string PaymentId { get; set; }
	}
}