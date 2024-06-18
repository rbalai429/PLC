using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class clsSubscriptionReport
	{
		public string UserUniqueId { get; set; }
		public string u_name { get; set; }
		public string u_email { get; set; }
		public string AgeGroup { get; set; }
		
		public string SubscriptionName { get; set; }
		public string SubscriptionPrice { get; set; }
		public string Ranking { get; set; }
		public string SubscriptionDuration { get; set; }
		//public string DiscountAmt { get; set; }
		public string ExistingUserDiscountAmt { get; set; }
		public decimal CouponDiscountAmt { get; set; }
		public decimal ActualPayment { get; set; }
		public string CouponCode { get; set; }
		public string CouponSource { get; set; }
		public DateTime DateOfCreation { get; set; }
		public string PaymentId { get; set; }
		public string PaymentStatus { get; set; }
		public DateTime PaymentDate { get; set; }
		
		public string InvoiceNo { get; set; }
		public string PartCode { get; set; }
		public string UserType { get; set; }
	}
}