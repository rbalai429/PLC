using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace HPPlc.Models.UrlProvider
{
	public class MyUrlProvider : IUrlProvider
	{
		//public UrlInfo GetUrl(UmbracoContext umbracoContext, IPublishedContent content, UrlProviderMode mode, string culture, Uri current)
		//{
		//	throw new NotImplementedException();
		//}

		public IEnumerable<UrlInfo> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
		{
			//return Enumerable.Empty<UrlInfo>();
			//throw new NotImplementedException();

			return null;
		}

		UrlInfo IUrlProvider.GetUrl(UmbracoContext umbracoContext, IPublishedContent content, UrlMode mode, string culture, Uri current)
		{
			try
			{
				var contentUrl = umbracoContext.Content.GetById(content.Id);
				if (contentUrl != null && content != null && content?.Parent?.ContentType?.Alias == "subjectsRoot" && content.Parent != null && content.Parent.Parent.Name == "Worksheets")
				{
					try
					{
						//this will remove the category name from the guide url
						var path = content.Parent.Url();
						var parts = path.Split(new[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

						return new UrlInfo("/" + parts[0] + "/" + content?.Url() + "/", true, "en-US");

					}
					catch { }


				}
			}
			catch { }

			return null;
			//throw new NotImplementedException();
		}
	}
}