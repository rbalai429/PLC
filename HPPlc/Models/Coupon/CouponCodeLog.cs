using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Coupon
{
    public class CouponCodeLog
    {
        public int CouponCodeId
        {
            get; set;
        }
        public string CouponCodeName
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
        public string UploadStatus
        {
            get; set;
        }
        public string Reason
        {
            get; set;
        }
        public string CouponSource
        {
            get; set;
        }
    }
}