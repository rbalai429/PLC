using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Vimeo
{
	public class VimeoRequest
	{
		[JsonProperty("hls")]
		public VimeoHls hls { get; set; }

		[JsonProperty("dash")]
		public VimeoHls dash { get; set; }
	}
}