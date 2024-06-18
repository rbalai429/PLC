using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Controllers.APIs
{
	public class FooterData
	{
		public List<LinkProp> FooterNavigation { get; set; }
		public string Copyright { get; set; }

		public List<MediaProp> PaymentSource { get; set; }
	}
}