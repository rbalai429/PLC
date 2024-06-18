using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Coupon
{
    public class CouponCode
    {
        public int CouponCodeId
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
        public bool? IsCouponAppliedForAgeGroup
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
        public bool Status
        {
            get; set;

        }
        public string SelectedAgeGroup
        {
            get; set;
        }
        public string SelectedSubscription
        {
            get; set;
        }
        public string DomainId
        {
            get; set;
        }

    }
    public class CouponCodeList
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
    }
    
}