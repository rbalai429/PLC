using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.ExpertTalkWebinar
{
    public class ExpertTalkInput
    {
        public string CultureInfo
        {
            get; set;
        }
        public string CurrentNode
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
        public string selectedVolume
        {
            get; set;
        }
        public string selectedCategory
        {
            get; set;
        }

		public int DisplayAgeGroup
		{
			get; set;
		}
	}
    
}