using HPPlc.Models.Mailer;
using HPPlc.Models.SMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;		
using System.Web;
using System.Web.Mvc;

namespace HPPlc.Models.Mailer
{
    public class MailerContent
    {
        public string RegistrationOTP(string type,string custName,string Otp)
        {
            StringBuilder mailboday = new StringBuilder();
			if(!String.IsNullOrEmpty(custName))
				mailboday.Append("<p>Hello " + custName + ", </p>");
			else
				mailboday.Append("<p>Hello,</p>");

			mailboday.Append("<p></p>");
			if (type == "registration" || type == "updateprofile" || type == "login" || type == "Plan365Login" || type == "auth" || type == "Bonus")
			{
				mailboday.Append("<p><br>" + Otp + " - Here’s your OTP for signing in to your HP Print Learn Center account. Please DO NOT share it with anyone else.</p>");

				mailboday.Append("<p><br>Thanks,</p>");
				mailboday.Append("<p>HP Print Learn Center</p>");
				mailboday.Append("<p><br><br>Disclaimer: This email is sent from an account we use only for sending messages. Please don't reply to this email- we won't get your response.</p>");
			}
			else if(type == "forgot")
				mailboday.Append("<p>" + Otp + " is the OTP to re-set your HP Print Learn Center Password. </p>");

			mailboday.Append("<p></p>");
            mailboday.Append("<p>Thank you!</p>");
            mailboday.Append("<p>HP Print Learn Center</p>");
			mailboday.Append("<p><a href='www.printlearncenter.com'>www.printlearncenter.com</p>");
			//mailboday.Append("<p></p>");
			//mailboday.Append("<p></p>");
			return mailboday.ToString();
        }
		
		public string RegistrationVerifyEmail(string type, string custName, string verifyurl)
		{
			StringBuilder mailboday = new StringBuilder();
			if (!String.IsNullOrEmpty(custName))
				mailboday.Append("<p>Hello " + custName + ", </p>");
			else
				mailboday.Append("<p>Hello,</p>");

			mailboday.Append("<p></p>");
			if (type == "registration")
				mailboday.Append("<p>Please <a href=" + verifyurl + " target=_blank>Click Here</a> to set your password and complete your registration.</p>");
			else if (type == "forgotpassword")
				mailboday.Append("<p>" + verifyurl + " is the OTP to forgot your HP Print Learn Center Password. </p>");

			mailboday.Append("<p></p>");
			mailboday.Append("<p>Thank you!</p>");
			mailboday.Append("<p>HP Print Learn Center</p>");
			mailboday.Append("<p><a href='www.printlearncenter.com'>www.printlearncenter.com</p>");
			//mailboday.Append("<p></p>");
			//mailboday.Append("<p></p>");
			return mailboday.ToString();
		}
		
        public string CallCenterEmailerContent()
        {
            StringBuilder mailboday = new StringBuilder();
            mailboday.Append("<p>Hello,</p>");
            mailboday.Append("<p></p>");
            mailboday.Append("<p>Please find the attached detailed report of the leads on the Nissan online booking portal since yesterday.</p>");
            mailboday.Append("<p></p>");
            mailboday.Append("<p>Thank you!</p>");
            //mailboday.Append("<p></p>");
            //mailboday.Append("<p></p>");
            return mailboday.ToString();
        }

		public string EPrintEmail(string custName)
		{
			StringBuilder mailboday = new StringBuilder();
			if (!String.IsNullOrEmpty(custName))
				mailboday.Append("<p>Hello " + custName + ", </p>");
			else
				mailboday.Append("<p>Hello,</p>");

			mailboday.Append("<p></p>");
			mailboday.Append("<p>Thank you!</p>");
			mailboday.Append("<p>HP Print Learn Center</p>");
			mailboday.Append("<p><a href='www.printlearncenter.com'>www.printlearncenter.com</p>");
			//mailboday.Append("<p></p>");
			//mailboday.Append("<p></p>");
			return mailboday.ToString();
		}
	}
}