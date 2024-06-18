using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Controllers.APIs
{
	
	public class BannerItems
	{
		public MediaProp DesktopImage { get; set; }
		public MediaProp DesktopNextGenImage { get; set; }
		public MediaProp MobileImage { get; set; }
		public MediaProp MobileNextGenImage { get; set; }
		public LinkProp BannerUrl { get; set; }

		public bool BannerAllowForGuestUser { get; set; }
		public string IsVideo { get; set; }
	}
}