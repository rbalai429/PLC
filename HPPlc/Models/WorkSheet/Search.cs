using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WorkSheet
{
	public class SimilarResponse
	{
		public string correctword { get; set; }
		public List<similarwords> similarword { get; set; }

	}

	public class similarwords
	{
		public string correctsimilarWord { get; set; }
	}
}