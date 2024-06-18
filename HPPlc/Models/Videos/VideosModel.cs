using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Videos
{
    public class VideosModel
    {
        public List<VideosItems> Videos
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
        public string VideoTitle
        {
            get; set;
        }
        public IHtmlString Description
        {
            get; set;
        }
        public string AgeValue
		{
			get; set;
		}
		public List<NestedItems> NestedItems
        {
            get; set;
        }
        public SeeMore SeeMore
        {
            get; set;
        }
        public int LoadMore
        {
            get; set;
        }
        public string ReadMore
        {
            get; set;
        }
    }
    
    public class NestedItems
    {
        public string NextGenImage
        {
            get; set;
        }
        public string AltText
        {
            get; set;
        }
        public string ImagesSrc
        {
            get; set;
        }
        public string MobileImagesSrc
        {
            get; set;
        } = "";
        public string MobileNextGenImagesSrc
        {
            get; set;
        } = "";
        public IHtmlString Description
        {
            get; set;
        }
        public SocialItems socialItems
        {
            get; set;
        } = new SocialItems();
        public SubscriptionStatus subscriptionStatus
        {
            get; set;
        }
        public string Volume
        {
            get; set;
        }
        public string Category
        {
            get; set;
        }
        public string Age
        {
            get; set;
        }
        public string Title
        {
            get; set;
        }
        public Boolean IsPlayVideos
        {
            get; set;
        }
        public string DataId
        {
            get; set;
        }
        public string VideoUrl
        {
            get; set;
        }

        public string VideoPreviewId
        {
            get; set;
        }
        public string VimeoURL
        {
            get; set;
        }
        public string SubscriptionUrl
        {
            get; set;
        }
        public bool ThumbUrl
        {
            get; set;
        }
		public string vrVideoId
		{
			get; set;
		}
        public string VideoSource
        {
            get; set;
        }
        public bool RecentlyDownloaded
        {
            get; set;
        } = false;
        public int? Id { get; set; } = 0;
        public bool IsWorkSheet { get; set; } = false;
        public bool IsPaid { get; set; } = true;
    }
    public class SocialItems
    {
        public string FBShare
        {
            get; set;
        }
        public string WhatAppShare
        {
            get; set;
        }
        public string EmailShare
        {
            get; set;
        }
    }
    public class SubscriptionStatus
    {
        public string DownloadUrl
        {
            get; set;
        }
        public string DownloadString
        {
            get; set;
        }
        public string SubscriptionUrl
        {
            get; set;
        }
        public Boolean Condition1
        {
            get; set;
        } = false;
        public Boolean Condition2
        {
            get; set;
        } = false;
        public Boolean Condition3
        {
            get; set;
        } = false;
        public Boolean Condition4
        {
            get; set;
        } = false;
        public Boolean Condition5
        {
            get; set;
        } = false;
        public Boolean Condition6
        {
            get; set;
        } = false;
        public Boolean Condition7
        {
            get; set;
        } = false;
        public Boolean Condition8
        {
            get; set;
        } = false;
        public Boolean Condition9
        {
            get; set;
        } = false;
    }
    public class SeeMore
    {
        public string VideoDetailsUrl
        {
            get; set;
        }
        public string NextGenMediaUrl
        {
            get; set;
        }
        public string MediaUrl
        {
            get; set;
        }
        public string MediaAltText
        {
            get; set;
        }
    }
}