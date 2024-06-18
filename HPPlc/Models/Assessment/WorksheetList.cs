using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Assessment
{
	public class WorksheetList
	{
		public int WorksheetId
		{
			get; set;
		}
		public string Title
		{
			get; set;
		}
		public string AgeGroup
		{
			get; set;
		}
		public string Subject
		{
			get; set;
		}
		public string Week
		{
			get; set;
		}
		public IHtmlString Description
		{
			get; set;
		}
		public bool has_quiz
		{
			get; set;
		}
		public string DesktopThumb
		{
			get; set;
		}
		public string MobileThumb
		{
			get; set;
		}

	}

	public class QuizAuthResponse
	{
		public int UserId { get; set; }
		public int WorksheetId { get; set; }
		public int QuizType { get; set; }
		public string Culture { get; set; }
		public int QuizForDemo { get; set; }
	}

	public class QuizRedirectionParameter
	{
		public string quizMode { get; set; }
		public string quizType { get; set; }
	}

	public class QuizValidate
	{
		public int status { get; set; }
		public string message { get; set; }
		public int QuizForDemo { get; set; }
		public int UserId { get; set; }
		public int WorksheetId { get; set; }
		public int QuizType { get; set; }
		public int WeekId { get; set; }
	}

	public class QuizValidateRoot
	{
		public List<QuizJSonResponse> items { get; set; }
	}
	public class QuizJSonResponse
	{
		public string id { get; set; }
		public string question_body { get; set; }
		public string explanation { get; set; }
		public List<options> options { get; set; }

		public question_attempt question_attempt { get; set; }
	}

	public class options
	{
		public string id { get; set; }
		public bool correct { get; set; }
		public int position { get; set; }
		public string option_body { get; set; }
	}

	public class question_attempt
	{
		public string id { get; set; }
		public List<string> submitted_options { get; set; }

		public string product_quiz_type { get; set; }
	}

	public class submitted_options
	{
		public string name { get; set; }
		public string value { get; set; }
	}

	public class QuizWebsiteResponce
	{
		public string UniqueCode { get; set; }
		public string QuizId { get; set; }
	}

	public class UserSubscriptionRanking
	{
		public int Ranking { get; set; }
	}

	public class WorksheetSubscriptionRanking
	{
		public int Ranking { get; set; }

	}

	public class RecomendateWorksheetId
	{
		public string WorksheetId { get; set; }
	}
	public class RecommendationWithPramotedWorksheet
	{
		public int ResponceCode { get; set; }
		public string RecommendationTitle { get; set; }
		public IHtmlString RecommendationDescription { get; set; }

		public string DownloadWorksheet { get; set; }
		public int RecommendedWorksheetId { get; set; }
	}

	public class Questions
	{
		public string Question_Id { get; set; }
		public string Question_Name { get; set; }
		public string Question_Explanation { get; set; }
	}

	public class Questions_Options
	{
		public string Question_Id { get; set; }
		public string Option_Id { get; set; }
		public bool Option_Correct { get; set; }
		public int Option_Position { get; set; }
		public string Option_Name { get; set; }
	}

	public class Questions_Submitted_Options
	{
		public string Question_Id { get; set; }
		public string Answer_Id { get; set; }
	}
}
