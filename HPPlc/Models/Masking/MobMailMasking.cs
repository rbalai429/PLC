using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HPPlc.Models.Masking
{
	public class MobMailMasking
	{
		public static String hidePhoneNum(String phone)
		{
			String result = "";
			if (phone != null && !"".Equals(phone))
			{
				if (phone.Length == 10)
				{
					string asterisks = new string('*', phone.Length - 4);

					// pick last 4 digits for showing
					string last = phone.Substring(phone.Length - 4, 4);

					// combine both asterisk mask and last digits
					result = asterisks + last;
				}
			}
			return result;
		}

		public static String hideEmailId(String email)
		{
			string pattern = @"(?<=[\w]{4})[\w-\._\+%]*(?=[\w]{0}@)";
			string result = Regex.Replace(email, pattern, m => new string('*', m.Length));

			return result;
		}
	}
}