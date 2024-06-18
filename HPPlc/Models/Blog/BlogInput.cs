using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Blog
{
	public class BlogInput
	{
		public int BindedOnPage { get; set; }
		public int HowManyBlogsToBeDisplay { get; set; }
		public int TotalCountOfBlogs { get; set; }
		public string OlderPostTitle { get; set; }
	}
}