using HPPlc.Models;
using HPPlc.Models.Blog;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Controllers
{
    public class BlogController : SurfaceController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;

        public BlogController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }

        [HttpPost]
        public ActionResult GetBlogsListing(BlogInput input)
        {
            int toBeDisplay = 0;
            int blogBalanceCnt = 0;

            try
            {
                string CultureInfo = CultureName.GetCultureName();
                //_variationContextAccessor.VariationContext = new VariationContext(CultureInfo);
                //List<BlogContent> blogContent;

                if (input.TotalCountOfBlogs > 0 && input.BindedOnPage < input.TotalCountOfBlogs)
                {
                    toBeDisplay = input.BindedOnPage + input.HowManyBlogsToBeDisplay;
                    blogBalanceCnt = (input.TotalCountOfBlogs - toBeDisplay) <= 0 ? 0 : (input.TotalCountOfBlogs - toBeDisplay);
                }

                var blogContent = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
                                                        .Where(x => x.ContentType.Alias == "blogRoute").FirstOrDefault()?
                                                        .Children?.Where(x => x.ContentType.Alias == "blogListing").FirstOrDefault()?.Children?
                                                        .OfType<BlogContent>()?.Where(x => x.IsPrimaryPost == false && x.IsActive == true)?
                                                        .OrderByDescending(x => x.Id)?.Take(toBeDisplay);

                ViewData["TotalCountOfBlogs"] = input.TotalCountOfBlogs;
                ViewData["blogTobeDisplay"] = toBeDisplay;
                ViewData["blogOlderPostTitle"] = input.OlderPostTitle;
                
                return PartialView("/Views/Partials/Blog/_BlogListing.cshtml", blogContent);
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(BlogController), ex, message: "GetBlogsListing - Main block");
            }

            return PartialView("/Views/Partials/Blog/_BlogListing.cshtml");
        }
    }
}