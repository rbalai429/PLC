using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Coupon
{
	public class CouponCodeResponse
	{
		public int ResponseCode { get; set; }
		public string ResponseMessage { get; set; }
		public int CouponCodeId { get; set; }
		public string CouponCodeName { get; set; }
		public string CouponType { get; set; }
		public string CouponSource { get; set; }
		public int CouponRemainingToUse { get; set; }
		public int ValidityRemainingDate { get; set; }
		public int DiscountType	{ get; set; }
		public decimal DiscountValue { get; set; }
		public decimal ActualDiscountValue { get; set; }
		public string DiscountMessage { get; set; }
		public int IsAppliedForSubscription { get; set; }
		public string SubscriptionRanking { get; set; }
		public int IsCouponAppliedForAgeGroup { get; set; }
		public string UserTypeAppliedForCoupon { get; set; }
		public string AgeGroupAppliedForCoupon { get; set; }
		public string CouponAvailedUniqueId { get; set; }
		public int BenefitRestrict { get; set; }
	}
}