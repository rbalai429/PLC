using HPPlc.Models.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WorkSheet
{
    public class WorkSheetModel
    {
        public List<WorkSheetItems> WorkSheets
        {
            get; set;
        }
		public int LoadMore
		{
			get; set;
		}
        public string Title
        {
            get; set;
        } = "";
        public IHtmlString Description
        {
            get; set;
        } = null;
        public string TrackTitle
        {
            get; set;
        } = null;

        public string Mode
        {
            get; set;
        } = "";

        public Paging Paging { get; set; } = new Paging();

        public List<VideosItems> Videos
        {
            get; set;
        } = new List<VideosItems>();
    }

    public class Paging
    {
        public int TotalItems { get; set; }
        public int NextPage { get; set; }
        public int DisplayItems { get; set; }
    }
    public class WorkSheetItems
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
        public string WorksheetTitle
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
        public string ViewAll
        {
            get; set;
        }
        public string ReadMore
        {
            get; set;
        }
        public int LoadMore
        {
            get; set;
        }
		public string PageTitle
		{
			get; set;
		}
        public string selectedSubjectsForSearch
        {
            get; set;
        }
        public string CbseContentCheckedForSearch
        {
            get; set;
        }
        
    }
    public class NestedItems
    {
		public int WorksheetId { get; set; }
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
        public string LockedDesktopImage
        {
            get; set;
        } = null;
        public string LockedDesktopNextGenImage
        {
            get; set;
        } = null;

        public string LockedMobileImage
        {
            get; set;
        } = null;

        public string LockedNextGenMobileImage
        {
            get; set;
        } = null;

        public string PreviewPdf
        {
            get; set;
        }
        public string MobileNextGenImage
        {
            get; set;
        }
        public string MobileAltText
        {
            get; set;
        }
        public string MobileImagesSrc
        {
            get; set;
        }
        public IHtmlString Description
		{
			get; set;
		}
        public IHtmlString WorksheetDetailsDescription
        {
            get; set;
        }
        public SocialItems socialItems
        {
            get; set;
        }
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
        public string Subject
        {
            get; set;
        }
        public string Topic
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
        public string SubTitle
        {
            get; set;
        }
        public string WorksheetDetailsUrl
        {
            get; set;
        }
        public List<string> SelectedSubjects
        {
            get; set;
        } = null;
        public List<string> SelectedTopics
        {
            get; set;
        } = null;
        public bool IsAppliedforLoggedInUserOnly
        {
            get; set;
        }
        public bool IsEnabledForDetailsPage
        {
            get; set;
        }
        public bool CBSEContentIncluded
        {
            get; set;
        }

        public bool IsQuizWorksheet
        {
            get; set;
        }
        public int NoOfDays
        {
            get; set;
        } = 0;

        public bool IsWorksheetLocked
        {
            get; set;
        } = false;

        public List<string> SelectedClasses { get; set; } = new List<string>();
        public bool IsPaid { get; set; }

		public bool RecentlyDownloaded { get; set; }
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