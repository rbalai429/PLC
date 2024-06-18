using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.Routing;

namespace HPPlc.Models.UrlProvider
{
	public interface IContentFinder
	{
		bool TryFindContent(PublishedRequest contentRequest);
	}
}
