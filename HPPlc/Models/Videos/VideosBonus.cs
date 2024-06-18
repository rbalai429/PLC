using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Videos
{
	public class VideosBonus
	{
		public int UserHaveSubscription { get; set; }
		public int HowManyVideosWatchedMoreThan15Sec { get; set; }
		public string WhichVideosWatchedMoreThan15Sec { get; set; }
		public int UserEligibleToWatchNewVideo { get; set; }
	}

	public class VideosList
	{
		public string VideoId { get; set; }
	}
}