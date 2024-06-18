using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Blog
{
	public class PopularBlog
	{
		public string BlogItemId { get; set; }
		public int TotalPopularBlog { get; set; }
		public int CountOfBlogItem { get; set; }
	}
}