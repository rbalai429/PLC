using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.ExpertTalkWebinar
{
    public class ExpertTalkWebinarModel
    {
        public List<WebinarItems> ExpertWebinars
        {
            get; set;
        }

        public List<VideosItems> Videos
        {
            get; set;
        }

        public int LoadMore
		{
			get; set;
		}
	}

    public class WebinarItems
    {
        public int ItemId
        {
            get; set;
        }
        public List<Speakers> Speakers
        {
            get; set;
        }
        public string Topic
        {
            get; set;
        }
        public int JoinNowDisplayInMinutes
        {
            get; set;
        }
        public int DisAppearJoinNowInMinutes
        {
            get; set;
        }
        public bool IsActive
        {
            get; set;
        }
        public DateTime WebinarDate
        {
            get; set;
        }
        public string DesktopImageWebP
        {
            get; set;
        }
        public string AltText
        {
            get; set;
        }
        public string DesktopImage
        {
            get; set;
        }
        public string MobileImage
        {
            get; set;
        }
        public string MobileImageWebP
        {
            get; set;
        }
        public string WebinarLink
        {
            get; set;
        }
        public DateTime WebinarEndTime
        {
            get; set;
        }
        public DateTime WebinarStartTime
        {
            get; set;
        }
        public int AppearRegisterNowInMinutes
        {
            get; set;
        }

        public bool IsDisplayShare
        {
            get; set;
        }
        public string facebookContent
        {
            get; set;
        }

        public string whatsAppContent
        {
            get; set;
        }

        public string mailContent
        {
            get; set;
        }
    }

    public class VideosItems
    {
        public int ItemId
        {
            get; set;
        }
        public string ImagesSrc
        {
            get; set;
        }
        public string Title
        {
            get; set;
        }
        public string AgeValue
        {
            get; set;
        }
        public int LoadMore
        {
            get; set;
        }
        public string ItemName
        {
            get; set;
        }
       
        public string videoDetailsUrl
        {
            get; set;
        }
        public string videoID
        {
            get; set;
        }

        public bool IsDisplayShare
        {
            get; set;
        }
        public string facebookContent
        {
            get; set;
        }

        public string whatsAppContent
        {
            get; set;
        }

        public string mailContent
        {
            get; set;
        }
    }

    public class Speakers
    {
        public string SpeakerName
        {
            get; set;
        }
        public IHtmlString SpeakerSubDetails
        {
            get; set;
        }
    }
}