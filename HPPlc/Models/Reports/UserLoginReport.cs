using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Reports
{
    public class UserLoginReport
    {
		public int userId
		{
			get; set;
		}
		public string userUniqueId
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
		public DateTime DOC
		{
			get; set;
		}
		public string referralCode
		{
			get; set;
		}
		public int IsActive
		{
			get; set;
		}
		public string Mode
		{
			get; set;
		}
        public int LoginCount
		{
            get; set;
        }
        public DateTime? LastLoginDate
		{
            get; set;
        }
		public string UserType
		{
			get; set;
		}
	}
}