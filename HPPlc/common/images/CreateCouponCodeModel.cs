using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Coupon
{
    public class CreateCouponCodeModel
    {
        public string TransactionId
        {
            get; set;
        }
        public int CouponCodeId
        {
            get; set;
        }
        public string CouponCodeName
        {
            get; set;
        }
        public List<CouponCodeList> CouponCodeList
        {
            get; set;
        }
        public List<CouponType> CouponTypeList
        {
            get; set;
        }
        public string CouponType
        {
            get; set;
        }
        public List<CouponUsagesType> CouponUsagesTypeList
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
        public List<DiscountTypes> DiscountTypesList
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
        public List<SubscriptionsItemForCouponCode> SubscriptionsItemForCouponCodeList
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
        public List<AgeGroupItemForCouponCode> AgeGroupItemForCouponCodeList
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
        public List<UserTypeMaster> UserTypeList
        {
            get; set;
        }
        public int UserId
        {
            get; set;
        }
        public List<DomainNameMaster> DomainNameMasterList
        {
            get; set;
        }
        public string DomainId
        {
            get; set;
        }

    }
    public class CouponType
    {
        public int CouponTypeId
        {
            get; set;
        }
        public string CouponTypeName
        {
            get; set;
        }
        public Boolean Selected
        {
            get; set;
        } = false;
    }
    public class CouponUsagesType
    {
        public int CouponUsagesTypeId
        {
            get; set;
        }
        public string CouponUsagesTypeName
        {
            get; set;
        }
        public Boolean Selected
        {
            get; set;
        } = false;
    }
    public class DiscountTypes
    {
        public int DiscountType
        {
            get; set;
        }
        public string DiscountTypeName
        {
            get; set;
        }
        public Boolean Selected
        {
            get; set;
        } = false;
    }
    public class SubscriptionsItemForCouponCode
    {
        public int SubscriptionId
        {
            get; set;
        }
        public string SubscriptionName
        {
            get; set;
        }
        public string Ranking
        {
            get; set;
        }
        public Boolean Selected
        {
            get; set;
        } = false;
    }
    public class AgeGroupItemForCouponCode
    {
        public int AgeGroupId
        {
            get; set;
        }
        public string AgeGroupName
        {
            get; set;
        }
        public Boolean Selected
        {
            get; set;
        } = false;
    }
    public class UserTypeMaster
    {
        public int UserTypeId
        {
            get; set;
        }
        public string UserType
        {
            get; set;
        }
        public string UserTypeName
        {
            get; set;
        }
        public bool Selected
        {
            get; set;
        } = false;
    }
    public class DomainNameMaster
    {
        public int DomainId
        {
            get; set;
        }
        public string DomainName
        {
            get; set;
        }
        public Boolean Selected
        {
            get; set;
        } = false;
    }
}