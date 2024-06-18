using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.FAQ
{
    public class FAQRequestModel
    {
        public int RequestId
        {
            get; set;
        }
        public string FullName
        {
            get; set;
        }
        public string Mobile
        {
            get; set;
        }
        public string SelectDate
        {
            get; set;
        }
        public string SelectTime
        {
            get; set;
        }
        public string FollowUp
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public string FollowUpBy
        {
            get; set;
        }
        public string Remark
        {
            get; set;
        }
        public string Consent
        {
            get; set;
        }

    }
    public class FAQRequestResultModel
    {
        public int Id
        {
            get; set;
        }
        public int RequestId
        {
            get; set;
        }
        public string FollowUp
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public DateTime DOC
        {
            get; set;
        }
        public string CreatedDate
        {
            get; set;
        }
        public string FollowUpBy
        {
            get; set;
        }
    }
    public class TimeList
    {
        public string Title
        {
            get; set;
        }
        public string Value
        {
            get; set;
        }
        public bool IsActive
        {
            get; set;
        }
        public int MaxCount
        {
            get; set;
        }
    }
}