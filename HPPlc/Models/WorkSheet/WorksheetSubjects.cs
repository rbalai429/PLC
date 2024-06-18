using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WorkSheet
{
	public class WorksheetSubjects
	{
		public string SubjectId { get; set; }
	}

	public class WorksheetSubjectsList
	{
		public string SubjectId { get; set; }
		public string SubjectName { get; set; }
	}
	public class WorksheetTopics
	{
		public string TopicsId { get; set; }
	}
	public class WorksheetClass
	{
		public string ClassId { get; set; }
	}
}