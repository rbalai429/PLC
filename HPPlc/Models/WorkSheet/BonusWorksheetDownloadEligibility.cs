using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WorkSheet
{
	public class BonusWorksheetDownloadEligibility
	{
		public int Result { get; set; }
		public int NoOfEligibleForDwnldWorksheet { get; set; }
		public int RemainingWorksheetForDwnld { get; set; }
		public int DownloadedWorksheet { get; set; }


		public int NoOfEligibleForVideoDwnldWorksheet { get; set; }
		public int RemainingVideoWorksheetForDwnld { get; set; }
		public int DownloadedVideo { get; set; }

		public int CurrentWorksheetDownloaded { get; set; }
		public int RemainingValidityInDays { get; set; }
	}

	public class BonusDownloadParam
	{
		public string Source { get; set; }
		public string NodeId { get; set; }
	}

	public class TeachersDownloadParam
	{
		public string AgeGroup { get; set; }
		public string Source { get; set; }
		public string NodeId { get; set; }
	}

	public class TeachersWorksheetDownloadEligibility
	{
		public int Result { get; set; }
		public int IsEligibleForDwnldWorksheet { get; set; }
		public int DownloadedWorksheet { get; set; }
		public int NoOfDaysRemainingForSubscription { get; set; }
		public int LimitOfDownloads { get; set; }
		public int CurrentWorksheetDownloaded { get; set; }
	}
}