using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Coupon
{
    public class CreateCouponCodeModelTemp
    {
        public int CouponCodeId
        {
            get; set;
        }
        public string TransactionId
        {
            get; set;
        }
        public string CouponCodeName
        {
            get; set;
        }
        public string CouponType
        {
            get; set;
        }
        public string CouponUsagesType
        {
            get; set;
        }
        public int? NoOfUsages
        {
            get; set;
        }
        public DateTime? ValidityStartDate
        {
            get; set;
        }
        public DateTime? ValidityEndDate
        {
            get; set;
        }
        public string CouponSource
        {
            get; set;
        }
        public string DiscountType
        {
            get; set;
        }
        public decimal? DiscountValue
        {
            get; set;
        }
        public bool? IsAppliedForSubscription
        {
            get; set;
        }
        public string SelectedSubscription
        {
            get; set;
        }
        public bool? IsCouponAppliedForAgeGroup
        {
            get; set;
        }
        public string SelectedAgeGroup
        {
            get; set;
        }
        public bool? IsAppliedForMultipleUserType
        {
            get; set;
        }
        public string UserType
        {
            get; set;
        }
        public int UserId
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public string Reason
        {
            get; set;
        }

        public string DomainId
        {
            get; set;
        }
    }
    
}