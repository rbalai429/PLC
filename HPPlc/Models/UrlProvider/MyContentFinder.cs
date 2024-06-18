using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Examine;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace HPPlc.Models.UrlProvider
{
	public class MyContentFinder : IContentLastChanceFinder
    {
        private readonly IDomainService _domainService;
        public MyContentFinder(IDomainService domainService)
        {
            _domainService = domainService;
        }
        public bool TryFindContent(PublishedRequest contentRequest)
        {
            //find the root node with a matching domain to the incoming request            

            var allDomains = _domainService.GetAll(true).ToList();
            var domain = allDomains.FirstOrDefault(f => f.DomainName == contentRequest.Uri.Authority
              || f.DomainName == "http://" + contentRequest.Uri.Authority
              || f.DomainName == "https://" + contentRequest.Uri.Authority);
            var siteId = domain != null ? domain.RootContentId : allDomains.Any() ? allDomains.FirstOrDefault()?.RootContentId : null;

            var siteRoot = contentRequest.UmbracoContext.Content.GetById(false, siteId ?? -1);
            if (siteRoot == null)
            {
                siteRoot = contentRequest.UmbracoContext.Content.GetAtRoot().FirstOrDefault();
            }

            if (siteRoot == null)
            {
                return false;
            }

            //assuming the 404 page is in the root of the language site with alias fourOhFourPageAlias
            var notFoundNode = siteRoot.DescendantsOrSelf().FirstOrDefault(f => f.ContentType.Alias == "subjectsRoot");

            if (notFoundNode != null)
            {
                contentRequest.PublishedContent = notFoundNode;
            }
            // return true or false depending on whether our custom 404 page was found
            return contentRequest.PublishedContent != null;
        }
    }
}
