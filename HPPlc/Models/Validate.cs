using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HPPlc.Models
{
	public class Validate
	{
		public bool ValidateEmail(string email)
		{
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			Match match = regex.Match(email);
			if (match.Success)
				return true;
			else
				return false;
		}
		public bool ValidateMobile(string mobile)
		{
			Regex regex = new Regex(@"\+?[0-9]{10}");
			Match match = regex.Match(mobile);
			if (match.Success)
				return true;
			else
				return false;
		}
		public bool ValidatePassword(string password)
		{
			Regex regex = new Regex(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");
			Match match = regex.Match(password);
			if (match.Success)
				return true;
			else
				return false;
		}
	}
}