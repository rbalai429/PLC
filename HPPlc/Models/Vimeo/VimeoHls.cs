using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Vimeo
{
	public class VimeoHls
	{
		[JsonProperty("link_expiration_time")]
		public string link_expiration_time { get; set; }

		[JsonProperty("link")]
		public string link { get; set; }

		//[JsonProperty("cdn")]
		//public string Cdn { get; set; }

		//[JsonProperty("default_cdn")]
		//public string DefaultCdn { get; set; }

		//[JsonProperty("separate_av")]
		//public bool SeparateAv { get; set; }
	}
}