using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Controllers.APIs
{
	public class LinkProp
	{
		public string Name { get; set; }
		public string Target { get; set; } = "";
		public string Url { get; set; }
		public string Udi { get; set; } = "";
	}
	public class MediaProp
	{
		public string Url { get; set; }
		public string Target { get; set; } = "";
		public string AltText { get; set; } = "";
		public string Udi { get; set; } = "";
	}

	public class MultiTypeMediaProp
	{
		public string MobileImgUrl { get; set; }
		public string MobileImgTarget { get; set; } = "";
		public string MobileImgAltText { get; set; } = "";
		public string MobileImgUdi { get; set; } = "";

		public string MobileNextGenImgUrl { get; set; }
		public string MobileNextGenImgTarget { get; set; } = "";
		public string MobileNextGenImgAltText { get; set; } = "";
		public string MobileNextGenImgUdi { get; set; } = "";

		public string DesktopImgUrl { get; set; }
		public string DesktopImgTarget { get; set; } = "";
		public string DesktopImgAltText { get; set; } = "";
		public string DesktopImgUdi { get; set; } = "";

		public string DesktopNextGenImgUrl { get; set; }
		public string DesktopNextGenImgTarget { get; set; } = "";
		public string DesktopNextGenImgAltText { get; set; } = "";
		public string DesktopNextGenImgUdi { get; set; } = "";
	}
}