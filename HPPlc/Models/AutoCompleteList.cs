using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
    public class AutoCompleteList
    {
        public string WorkSheetTitle { get; set; }
        public string SubjectTitle { get; set; }

		public int WorkSheetId { get; set; }

        public string ClassTitle { get; set; } = "";
	}
}