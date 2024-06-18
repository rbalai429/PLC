using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class BreadCrumbSchema
	{
		public string type { get; set; }
		public int position { get; set; }
		public string item { get; set; }
		public string name { get; set; }
	}
}