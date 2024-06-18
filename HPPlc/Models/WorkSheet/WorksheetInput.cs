using HPPlc.Models.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WorkSheet
{
    public class WorksheetInput
    {
        public string Mode
        {
            get; set;
        }
        public string CultureInfo
        {
            get; set;
        }
        public string FilterType
        {
            get; set;
        }
        public string FilterId
        {
            get; set;
        }
        public int? DisplayCount
        {
            get; set;
        }
        public string selectedAgeGroup
        {
            get; set;
        }
        public string selectedTopics
        {
            get; set;
        }
        public string selectedSubject
        {
            get; set;
        }

        public string IsCbseContent
        {
            get; set;
        }
        public int DisplayAgeGroup
		{
			get; set;
		}
        public int worksheetId
        {
            get; set;
        }
        public string sortBy { get; set; } = "";
        public string selectedPaid { get; set; } = string.Empty;

        public int currentPage { get; set; } = 0;
        public string searchText { get; set; } = string.Empty;

        public VideosInput VideosInput { get; set; } = new VideosInput();

        public bool NoRecordFound { get; set; } = false;
    }
}