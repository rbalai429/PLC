using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public static class GetPageName
	{
		public static string pagename()
		{
			var pageName = HttpContext.Current.Request?.UrlReferrer?.AbsolutePath;

			if (!String.IsNullOrEmpty(pageName))
			{
				string cultureName = HPPlc.Models.CultureName.GetCultureName().Replace("/", "");
				string[] path = pageName?.TrimEnd('/')?.Split('/');
				pageName = path.LastOrDefault();

				if (String.IsNullOrEmpty(pageName) || (!String.IsNullOrEmpty(cultureName) && pageName == cultureName))
				{ pageName = "Home"; }
			}
			else
			{
				pageName = "Home";
			}

			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pageName.ToLower());
		}
	}
}