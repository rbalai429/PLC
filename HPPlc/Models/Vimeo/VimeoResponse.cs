using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Vimeo
{
	public class VimeoResponse
	{
		[JsonProperty("play")]
		public VimeoRequest play { get; set; }

		//[JsonProperty("request")]
		//public VimeoRequest Request { get; set; }

		//[JsonProperty("video")]
		//public VimeoVideo Video { get; set; }
	}
}