using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public static class CultureName
	{
		public static string GetCultureName()
		{
			string culture = String.Empty;
			culture = SessionManagement.GetCurrentSession<string>(SessionType.SelectedLanguageCulture);
			if (culture == "en-US")
				culture = String.Empty;
			else if (String.IsNullOrEmpty(culture))
				culture = String.Empty;
			else
				culture = "/" + culture;
			//string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;
			//string[] arrUrl = currentUrl.Split('/');
			//culture = arrUrl[1];
			return culture;
		}
	}
}