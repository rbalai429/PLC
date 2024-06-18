using HPPlc.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HPPlc.Models.HPUId
{
	public class CultureManagePostHPID
	{
		public static string SetCultureCookies()
		{
			string culture = CultureName.GetCultureName().Replace("/", "");
			if (!String.IsNullOrWhiteSpace(culture))
			{
				string cultureName = SessionManagement.GetCurrentSession<string>(SessionType.SelectedLanguage);

				HttpContext.Current.Response.Cookies["PreUrlRedirectionCulture"].Value = culture;
				HttpContext.Current.Response.Cookies["PreUrlRedirectionCultureName"].Value = cultureName;
			}

			return "";
		}
		public string CultureStorePostHpId()
		{
			System.Web.HttpCookie PreUrlRedirection = HttpContext.Current.Request?.Cookies["PreUrlRedirectionCulture"];
			System.Web.HttpCookie PreUrlRedirectionName = HttpContext.Current.Request?.Cookies["PreUrlRedirectionCultureName"];
			if (PreUrlRedirection != null && !String.IsNullOrWhiteSpace(PreUrlRedirection.Value))
			{
				string culture = PreUrlRedirection?.Value;
				string cultureName = PreUrlRedirectionName?.Value;

				CultureInfo cultureExists = CultureInfo.GetCultureInfo(culture);
				if (cultureExists.IsReadOnly)
				{
					HomeController homeController = new HomeController();
					homeController.LanguageStoreInSession(cultureName,culture);
					//SessionManagement.StoreInSession(SessionType.SelectedLanguageCulture, culture);

					HttpContext.Current.Response.Cookies["PreUrlRedirectionCulture"].Expires = DateTime.Now.AddDays(-1);
					HttpContext.Current.Response.Cookies["PreUrlRedirectionCultureName"].Expires = DateTime.Now.AddDays(-1);

					return culture;
				}
				//Uri uri = new Uri(Url);
				//string absoluteUri = uri.AbsolutePath.TrimStart('/');
				//if (!String.IsNullOrWhiteSpace(absoluteUri))
				//{
				//	string[] absoluteUriSplit = absoluteUri.Split('/');
				//	if (absoluteUriSplit.Count() > 0)
				//	{
				//		var cltName = absoluteUriSplit.First().Trim('/');
				//		if (!String.IsNullOrWhiteSpace(cltName))
				//		{
				//			CultureInfo cultureExists = CultureInfo.GetCultureInfo(cltName);
				//			if (cultureExists.IsReadOnly)
				//			{
				//				SessionManagement.StoreInSession(SessionType.SelectedLanguageCulture, cltName);

				//				//HttpContext.Current.Response.Cookies["PreUrlRedirection"].Expires = DateTime.Now.AddDays(-1);

				//				return "Valid";
				//			}
				//		}
				//	}
				//}
			}

			return "";
		}
	}
}