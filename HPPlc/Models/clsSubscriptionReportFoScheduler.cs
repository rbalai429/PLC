using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
    public class clsSubscriptionReportFoScheduler
    {
        public string UserUniqueId
        {
            get; set;
        }
        public string Existing_New
        {
            get; set;
        }
        public string DataSource
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
        public string u_whatsappno_prefix
        {
            get; set;
        }
        public string u_whatsappno
        {
            get; set;
        }
        public string ComWithEmail
        {
            get; set;
        }
        public string ComWithWhatsApp
        {
            get; set;
        }
        public string ComWithPhone
        {
            get; set;
        }
        public string CheckedTAndC
        {
            get; set;
        }
        public string AgeGroup
        {
            get; set;
        }

        public string Subscription_Plan_Opted
        {
            get; set;
        }
        public decimal Plan_Amount
        {
            get; set;
        }
        public decimal Existing_User_Discount
        {
            get; set;
        }
       
        public decimal Coupon_Redeemed_Amount_Item
        {
            get; set;
        }

		public string Coupon_Name_Item { get; set; }
		
        public decimal Coupon_Redeemed_Amount_Tran
        {
            get; set;
        }
        public string Coupon_Name_Tran { get; set; }

        public string Source
        {
            get; set;
        }
        public decimal Amount_Received
        {
            get; set;
        }
        public string RegistrationdatetimeStamp
        {
            get; set;
        }
        public string DOC
        {
            get; set;
        }
        public string ReferralCode
        {
            get; set;
        }
        public string Email_Consent_UTC_ts
        {
            get; set;
        }
        public string WhatsApp_Consent_UTC_ts
        {
            get; set;
        }
        public string Phone_Consent_UTC_ts
        {
            get; set;
        }

    }
}