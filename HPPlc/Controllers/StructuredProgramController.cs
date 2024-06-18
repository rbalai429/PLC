using HPPlc.Models.WorkSheet;
using HPPlc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;
using Umbraco.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using Umbraco.Web.Models;
using System.Web;
using Umbraco.Core;
using Examine.Search;
using Examine;
using Lucene.Net.Search;
using Umbraco.Examine;
using static Lucene.Net.Index.SegmentReader;
using DocumentFormat.OpenXml.EMMA;
using VideosNestedItems = HPPlc.Models.Videos.NestedItems;
using VideosSeeMore = HPPlc.Models.Videos.SeeMore;
using NestedItems = HPPlc.Models.WorkSheet.NestedItems;
using HPPlc.Models.Videos;
using SocialItems = HPPlc.Models.WorkSheet.SocialItems;
using SubscriptionStatus = HPPlc.Models.WorkSheet.SubscriptionStatus;
using System.Threading.Tasks;
using StackExchange.Profiling.Internal;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace HPPlc.Controllers
{
	public class StructuredProgramController : SurfaceController
	{
		private readonly IVariationContextAccessor _variationContextAccessor;
		public StructuredProgramController(IVariationContextAccessor variationContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
		}

		[HttpPost]
		public async Task<ActionResult> GetStructuredProgramList(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetVideoModel model = new WorkSheetVideoModel();
				model = await GetStructuredProgramListData(input);

				if (input != null && !String.IsNullOrWhiteSpace(input.Mode) && input.Mode.ToLower() == "quiz")
				{
					model.Mode = input.Mode;
				}

				return PartialView("/Views/Partials/Worksheets/_structuredProgramList.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetStructuredProgramList");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		//[HttpPost]
		//public async Task<ActionResult> GetRelatedStructuredProgramDetails(WorksheetInput input)
		//{
		//	Responce responce = new Responce();
		//	try
		//	{
		//		WorkSheetVideoModel model = new WorkSheetVideoModel();
		//		model = await GetStructuredProgramListData(input);

		//		model.Mode = input?.Mode;

		//		return PartialView("/Views/Partials/Worksheets/_structuredProgramList.cshtml", model);
		//	}
		//	catch (Exception ex)
		//	{
		//		responce.StatusCode = HttpStatusCode.InternalServerError;
		//		responce.Message = ex.ToString();

		//		Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetWorksheetList");
		//	}

		//	return Json(responce, JsonRequestBehavior.AllowGet);
		//}

		private async Task<WorkSheetVideoModel> GetStructuredProgramListData(WorksheetInput input)
		{
			WorkSheetVideoModel model = new WorkSheetVideoModel();
			try
			{
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				var worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
									.Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();

				var videoRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
									.Where(x => x.ContentType.Alias == "videos")?.OfType<Videos>().FirstOrDefault();

				if (worksheetRoot != null)
				{
					try
					{
						var pagingInfo = new Paging();
						var WorkSheets = await GetFilterData(worksheetRoot, videoRoot, input);
						int nextPage = input.currentPage + 1;
						pagingInfo.NextPage = nextPage;
						pagingInfo.DisplayItems = worksheetRoot.NoOfDisplayWorksheet;
						pagingInfo.TotalItems = WorkSheets?.FirstOrDefault()?.totalItems ?? 0;
						model.WorkSheets = WorkSheets;
						model.Paging = pagingInfo;
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetStructuredProgramListData - sub block");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetStructuredProgramListData");
			}

			return model;
		}

		private async Task<VideosNestedItems> GetVideos(Video videoItems, VideosInput input)
		{
			var SelectedAgeValues = videoItems.AgeTitle?.Udi;
			var SelectedSubject = videoItems.Category.Select(x => x.Udi).ToList();
			var selectedAgeContent = Umbraco.Content(SelectedAgeValues);
			var SelectedClasssValues = selectedAgeContent.Value<string>("alternateClassName");
			var selectedSubjectContent = Umbraco.Content(SelectedSubject);
			var SelectedSubjectValues = selectedSubjectContent.Select(x => x.Value<string>("itemName")).ToList();

			VideosNestedItems nested = new VideosNestedItems();

			nested.Id = videoItems?.Id;
			nested.Title = videoItems?.Title;
			nested.Description = videoItems?.Description;
			nested.Age = SelectedClasssValues;
			nested.Category = string.Join(",", SelectedSubjectValues);
			nested.Volume = videoItems.SelectVolume?.Name;
			nested.VideoPreviewId = videoItems?.VideoPreviewId;
			nested.VimeoURL = videoItems?.VimeoUrl;
			nested.VideoUrl = videoItems?.Url();


			nested.IsPaid = videoItems.IsGuestUserSheet == true ? false : true;
			if (videoItems?.ThumbnailImage != null)
				nested.ImagesSrc = videoItems?.ThumbnailImage.Url();
			if (videoItems?.NextGenImageThumb != null)
				nested.NextGenImage = videoItems?.NextGenImageThumb.Url();
			if (videoItems?.UploadMobileThumbnail != null)
				nested.MobileImagesSrc = videoItems?.UploadMobileThumbnail.Url();
			if (videoItems?.MobileNextGenImage != null)
				nested.MobileNextGenImagesSrc = videoItems?.MobileNextGenImage.Url();

			try
			{
				//social integration
				WorksheetVideosHelper videosHelper = new WorksheetVideosHelper();
				//nested.socialItems  = new VideosNestedItems.SocialItems();
				//nested = videosHelper.GetSocialItemsAndSubscriptionDetailsForVideos(videoItems, input, nested);
				if (!String.IsNullOrEmpty(videoItems.FacebookContent) || !String.IsNullOrEmpty(videoItems.WhatsAppContent) || !String.IsNullOrEmpty(videoItems.MailContent))
				{
					if (!String.IsNullOrEmpty(videoItems.FacebookContent))
						nested.socialItems.FBShare = videoItems.FacebookContent;

					if (!String.IsNullOrEmpty(videoItems.WhatsAppContent))
						nested.socialItems.WhatAppShare = videoItems.WhatsAppContent;

					if (!String.IsNullOrEmpty(videoItems.MailContent))
						nested.socialItems.EmailShare = videoItems.MailContent + "\n\n" + "`" + videoItems?.Title;
				}

				string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(videoItems.Id.ToString()) + "&source=BonusVideoWorkSheet" + "&paid=" + ((bool)(videoItems.IsGuestUserSheet == false) ? "Paid" : "Free");
				nested.subscriptionStatus = new Models.Videos.SubscriptionStatus();
				string DownloadString = clsCommon.encrypto(videoItems.Id.ToString()) + "$" + "bonusVideoworksheetPrint" + "$" + ((bool)(videoItems.IsGuestUserSheet == false) ? "Paid" : "Free");
				nested.subscriptionStatus.DownloadString = DownloadString;
				nested.subscriptionStatus.DownloadUrl = downloadUrl;

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetVideos");
			}
			return nested;
		}

		public List<WorksheetRoot> GetWorksheetsBySubject(List<WorksheetCategory> subjects, int firstTimeDisplayWorksheet, WorksheetInput input)
		{
			//int _firstTimeDisplayWorksheetRemaining = firstTimeDisplayWorksheet;
			List<WorksheetRoot> worksheetGetSubjectWise = new List<WorksheetRoot>();
			List<WorksheetRoot> worksheetAllSubjectWise = new List<WorksheetRoot>();
			List<WorksheetTopics> worksheetTopics = new List<WorksheetTopics>();

			if (!String.IsNullOrWhiteSpace(input.selectedTopics))
			{
				string[] allTopics = input.selectedTopics.Split(',');
				if (allTopics != null && allTopics.Length > 0)
				{
					for (int i = 0; i < allTopics.Length; i++)
					{
						worksheetTopics.Add(new WorksheetTopics { TopicsId = allTopics[i].ToString().Trim() });
					}
				}
			}

			foreach (var subject in subjects)
			{
				var SubjectId = Umbraco?.Content(subject?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue;
				//_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				if ((input.selectedSubject != null && !string.IsNullOrWhiteSpace(input.selectedSubject)) || input.selectedTopics != null && !string.IsNullOrWhiteSpace(input.selectedTopics))
				{
					//List<WorksheetTopics> topics = new List<WorksheetTopics>();
					//if (!String.IsNullOrWhiteSpace(input.selectedTopics))
					//{
					//	for (int i =0; i< input.selectedTopics.Split(',').Length;i++)
					//	{
					//		topics.Add(new WorksheetTopics { TopicsId = input.selectedTopics[i].ToString() });
					//	}
					//}

					if (SubjectId != null && !String.IsNullOrWhiteSpace(SubjectId.ToString()))// && input.selectedSubject.Contains(SubjectId.ToString())
					{
						if (worksheetTopics != null && worksheetTopics.Count > 0)
						{
							worksheetGetSubjectWise = subject?.Children<WorksheetRoot>()?
								.Where(x => x.IsActive == true && x.SelectSubject != null
								&& (Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == SubjectId)
								&& (worksheetTopics != null && worksheetTopics.Any(c => Umbraco?.Content(x.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue.ToString() == c.TopicsId))).ToList();
						}
						else if (!String.IsNullOrWhiteSpace(input.Mode) && input.Mode == "related")
						{
							worksheetGetSubjectWise = subject?.Children<WorksheetRoot>()?
								.Where(x => x.IsActive == true && x.Id != input.worksheetId && (x.SelectSubject != null
								&& Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?
								.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == SubjectId)).ToList();
						}
						else if (!String.IsNullOrWhiteSpace(input.Mode) && input.Mode.ToLower() == "quiz")
						{
							worksheetGetSubjectWise = subject?.Children<WorksheetRoot>()?
								.Where(x => x.IsActive == true && x.IsQuizWorksheet == true && x.SelectSubject != null
								&& Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == SubjectId).ToList();
						}
						else
						{
							worksheetGetSubjectWise = subject?.Children<WorksheetRoot>()?
								.Where(x => x.IsActive == true && (x.SelectSubject != null
								&& Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == SubjectId)).ToList();
						}

						//worksheetGetSubjectWise = subject?.Children<WorksheetRoot>()?
						//	.Where(x => x.IsActive == true && (x.SelectSubject != null
						//	&& Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == SubjectId) || 
						//	(x.Topic != null && topics.Any(t => t.TopicsId ==
						//	Umbraco?.Content(x?.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue.ToString()))).ToList();

						if (worksheetGetSubjectWise != null)
						{
							SessionManagement.StoreInSession(SessionType.UserSelectCategoryOnWorksSheet, input.selectedSubject);

							if (input.IsCbseContent == "1")
								worksheetAllSubjectWise.AddRange(worksheetGetSubjectWise.Where(x => x.CBsecontentIncluded == true));
							else
								worksheetAllSubjectWise.AddRange(worksheetGetSubjectWise);
						}
					}
				}
				else
				{
					if (worksheetTopics != null && worksheetTopics.Count > 0)
					{
						worksheetGetSubjectWise = subject.Children<WorksheetRoot>()?
						.Where(x => x.IsActive == true
						&& Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == SubjectId
						&& (worksheetTopics != null && worksheetTopics.Any(c => Umbraco?.Content(x.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue.ToString() == c.TopicsId)))
						.ToList();
					}
					else if (!String.IsNullOrWhiteSpace(input.Mode) && input.Mode.ToLower() == "quiz")
					{
						worksheetGetSubjectWise = subject.Children<WorksheetRoot>()?
						.Where(x => x.IsActive == true && x.IsQuizWorksheet == true
						&& Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == SubjectId)?
						.ToList();
					}
					else
					{
						worksheetGetSubjectWise = subject.Children<WorksheetRoot>()?
						.Where(x => x.IsActive == true
						&& (Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == SubjectId))?
						.ToList();
					}

					if (worksheetGetSubjectWise != null)
					{
						if (input.IsCbseContent == "1")
							worksheetAllSubjectWise.AddRange(worksheetGetSubjectWise.Where(x => x.CBsecontentIncluded == true));
						else
							worksheetAllSubjectWise.AddRange(worksheetGetSubjectWise);


					}
				}
			}

			return worksheetAllSubjectWise;
		}

		public List<WorksheetRoot> GetWorksheetsByTopic(List<TopicsName> topics, int firstTimeDisplayWorksheet, WorksheetInput input)
		{
			//int _firstTimeDisplayWorksheetRemaining = firstTimeDisplayWorksheet;
			List<WorksheetRoot> worksheetGetTopicWise = new List<WorksheetRoot>();
			List<WorksheetRoot> worksheetAllTopicWise = new List<WorksheetRoot>();

			foreach (var topic in topics)
			{
				var TopicId = Umbraco?.Content(topic?.TopicMapping?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue.ToString();
				//_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				if (input.selectedTopics != null && !string.IsNullOrWhiteSpace(input.selectedTopics))
				{
					if (TopicId != null && !String.IsNullOrWhiteSpace(TopicId.ToString()))// && input.selectedSubject.Contains(SubjectId.ToString())
					{
						worksheetGetTopicWise = topic?.Parent?.Children<WorksheetRoot>()?
							.Where(x => x.IsActive == true && x.Topic != null
							&& Umbraco?.Content(x.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue.ToString() == TopicId).ToList();

						if (worksheetGetTopicWise != null)
						{
							//SessionManagement.StoreInSession(SessionType.UserSelectCategoryOnWorksSheet, input.selectedSubject);

							if (input.IsCbseContent == "1")
								worksheetAllTopicWise.AddRange(worksheetGetTopicWise.Where(x => x.CBsecontentIncluded == true));
							else
								worksheetAllTopicWise.AddRange(worksheetGetTopicWise);
						}
					}
				}
				else
				{
					worksheetGetTopicWise = topic?.Parent?.Children<WorksheetRoot>()?
						.Where(x => x.IsActive == true
						&& Umbraco?.Content(x.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue.ToString() == TopicId)?
						.ToList();

					if (worksheetGetTopicWise != null)
					{
						if (input.IsCbseContent == "1")
							worksheetAllTopicWise.AddRange(worksheetGetTopicWise.Where(x => x.CBsecontentIncluded == true));
						else
							worksheetAllTopicWise.AddRange(worksheetGetTopicWise);
					}
				}
			}

			return worksheetAllTopicWise;
		}

		public List<StructureProgramItems> GetStructuredProgramSingle(WorksheetInput input)
		{
			//int _firstTimeDisplayWorksheetRemaining = firstTimeDisplayWorksheet;
			List<StructureProgramItems> worksheetSingle = new List<StructureProgramItems>();
			//_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
			if (input.worksheetId > 0)
			{
				worksheetSingle = Umbraco?.Content(input.worksheetId)?.DescendantsOrSelf()?.OfType<StructureProgramItems>()?
					.Where(x => x.IsActive == true).ToList();

				//if (worksheetSingle != null)
				//{
				//	//SessionManagement.StoreInSession(SessionType.UserSelectCategoryOnWorksSheet, input.selectedSubject);

				//	if (input.IsCbseContent == "1")
				//		worksheetAllTopicWise.AddRange(worksheetGetTopicWise.Where(x => x.CBsecontentIncluded == true));
				//	else
				//		worksheetAllTopicWise.AddRange(worksheetGetTopicWise);
				//}

			}

			return worksheetSingle;
		}

		[HttpPost]
		public ActionResult GetStructuredProgramById(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetItems model = new WorkSheetItems();
				model = GetStructuredProgramDetailsById(input);

				return PartialView("/Views/Partials/Worksheets/_workSheetsDetails.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();
			}
			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		private WorkSheetItems GetStructuredProgramDetailsById(WorksheetInput input)
		{
			WorkSheetItems model = new WorkSheetItems();
			try
			{
				//if (String.IsNullOrWhiteSpace(input.selectedSubject))
				//	input.selectedSubject = SessionManagement.GetCurrentSession<string>(SessionType.UserSelectCategoryOnWorksSheet);
				//else
				//	SessionManagement.StoreInSession(SessionType.UserSelectCategoryOnWorksSheet, input.selectedSubject);

				//if (String.IsNullOrWhiteSpace(input.IsCbseContent))
				//	input.IsCbseContent = SessionManagement.GetCurrentSession<string>(SessionType.CbseContentChecked);
				//else
				//	SessionManagement.StoreInSession(SessionType.CbseContentChecked, input.IsCbseContent);

				if (!String.IsNullOrWhiteSpace(input.selectedSubject))
					model.selectedSubjectsForSearch = string.Join(",", input.selectedSubject);
				if (!String.IsNullOrWhiteSpace(input.IsCbseContent) && !input.IsCbseContent.Equals("0"))
					model.CbseContentCheckedForSearch = input.IsCbseContent;

				string vUserSubscription = String.Empty;
				string recommendedTitle = String.Empty;
				string bitlyLink = String.Empty;
				int? countOfTotalWorksheets = 0;

				try
				{
					//Check here user subscription
					LoggedIn loggedin = new LoggedIn();
					loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);
					string culture = CultureName.GetCultureName().Replace("/", "");
					string subscribeUrl = culture + "subscription/";

					if (input.DisplayCount == 0)
						input.DisplayCount = input.DisplayCount;
					else
						input.DisplayCount += input.DisplayCount;

					if (!String.IsNullOrEmpty(input.FilterType))
					{
						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
						//var worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
						//							.Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault()?.Children?
						//							.Where(x => x.ContentType.Alias == "worksheet")?.FirstOrDefault();

						//load more 
						if (input.DisplayCount == 0)
						{
							int? DisplayCount = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?
							   .FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "worksheetNode")?
							   .FirstOrDefault()?.Value<int>("firstTimeDisplayWorksheetOnDetails");

							input.DisplayCount = DisplayCount;
						}

						List<WorksheetCategory> subjects = new List<WorksheetCategory>();
						if (String.IsNullOrWhiteSpace(input.selectedSubject))
						{
							subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?
								.FirstOrDefault()?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
								.Where(x => x?.Url().ToString().Trim().ToLower().Split('/')?.Where(y => !string.IsNullOrWhiteSpace(y)).LastOrDefault() == input.FilterType.Trim().ToLower())?.FirstOrDefault()?
								.Children<WorksheetCategory>()?.Where(c => c.IsActive == true).ToList();
						}
						else
						{
							List<WorksheetSubjects> subj = new List<WorksheetSubjects>();
							if (!String.IsNullOrWhiteSpace(input.selectedSubject))
							{
								string[] subjectArray = input.selectedSubject.Split(',');
								if (subjectArray != null)
								{
									for (int i = 0; i < subjectArray.Length; i++)
									{
										subj.Add(new WorksheetSubjects { SubjectId = subjectArray[i].ToString().Trim() });
									}

									if (subj != null)
									{
										subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?
													.FirstOrDefault()?.DescendantsOrSelf()?
													.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
													.Where(x => x?.Url().ToString().Trim().ToLower().Split('/')?.Where(y => !string.IsNullOrWhiteSpace(y)).LastOrDefault() == input.FilterType.Trim().ToLower())?.FirstOrDefault()?
													.Children<WorksheetCategory>()?.Where(c => c.IsActive == true
													&& subj.Any(s => Umbraco?.Content(c.CategoryName.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue.ToString() == s.SubjectId)).ToList();
									}
								}
							}
						}

						if (subjects != null && subjects.Any())
						{
							List<WorksheetRoot> worksheetAllSubjectWise = new List<WorksheetRoot>();
							worksheetAllSubjectWise = GetWorksheetsBySubject(subjects, 0, input);

							if (worksheetAllSubjectWise != null && worksheetAllSubjectWise.Count > 0)
							{
								countOfTotalWorksheets = worksheetAllSubjectWise.Count();
								//Sorting
								try
								{
									if (worksheetAllSubjectWise != null && worksheetAllSubjectWise.Any() && worksheetAllSubjectWise.Count > 0)
									{
										worksheetAllSubjectWise = worksheetAllSubjectWise?.
											OrderBy(x => Convert.ToInt32(Umbraco?.Content(x.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue)).Take(input.DisplayCount.Value).ToList();
									}
								}
								catch { }

								model.ReadMore = subjects?.FirstOrDefault()?.Parent?.Parent?.Value<string>("readMore");
								int startVolumeForReferral = subjects.FirstOrDefault().Parent.Parent.Value<int>("startVolumeForReferral");


								model.Title = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<string>("title");
								model.Description = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<IHtmlString>("description");
								//SessionManagement.StoreInSession(SessionType.UserSelectAgeOnWorksSheet, worksheetAllSubjectWise?.FirstOrDefault()?.AgeTitle.Name);

								try
								{
									if (worksheetAllSubjectWise != null && worksheetAllSubjectWise.Any())
									{
										//find registered age group
										dbAccessClass db = new dbAccessClass();
										List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
										myagegroup = db.GetUserSelectedUserGroup();

										List<NestedItems> List = new List<NestedItems>();
										foreach (var worksheetItems in worksheetAllSubjectWise)
										{
											NestedItems nested = new NestedItems();
											string worksheetType = String.Empty;
											var PDF_File = worksheetItems?.UploadPdf;
											bitlyLink = worksheetItems?.BitlyLink;

											var desktopmedia = worksheetItems?.UploadThumbnail;
											var desktopmediaNextgen = worksheetItems?.NextGenImage;
											var mobilemedia = worksheetItems?.UploadMobileThumbnail;
											var mobilemediaNextgen = worksheetItems?.MobileNextGenImage;

											var AgeTitle = worksheetItems?.AgeTitle;
											var AgeTitleDesc = Umbraco?.Content(AgeTitle?.Udi);
											var ItemName = AgeTitleDesc?.Value("itemName");
											if (!String.IsNullOrEmpty(worksheetItems?.AgeTitle.Name))
												worksheetType = "age";

											if (desktopmediaNextgen != null)
												nested.NextGenImage = desktopmediaNextgen.Url();
											if (desktopmedia != null)
											{
												nested.AltText = desktopmedia.Value("altText").ToString();
												nested.ImagesSrc = desktopmedia.Url().ToString();
											}

											if (mobilemediaNextgen != null)
												nested.MobileNextGenImage = mobilemediaNextgen.Url();
											if (mobilemedia != null)
											{
												nested.MobileAltText = mobilemedia.Value("altText").ToString();
												nested.MobileImagesSrc = mobilemedia.Url().ToString();
											}

											if (!String.IsNullOrEmpty(ItemName.ToString()))
												nested.Title = ItemName.ToString();

											nested.Description = worksheetItems?.Description;
											nested.WorksheetDetailsDescription = worksheetItems?.DescriptionPageContent;
											nested.Volume = worksheetItems.SelectWeek?.Name.ToString();
											//nested.Category = string.Join(",", worksheetItems.Category?.Select(x => x.Name).ToList());
											//nested.Category = string.Join(",", worksheetItems?.Name);

											nested.IsQuizWorksheet = worksheetItems.IsQuizWorksheet;
											nested.CBSEContentIncluded = worksheetItems.CBsecontentIncluded;
											nested.PreviewPdf = worksheetItems?.UploadPreviewPdf;
											nested.IsEnabledForDetailsPage = worksheetItems.IsEnableForDetailsPage;

											if (!String.IsNullOrEmpty(worksheetItems.Value<string>("umbracoUrlAlias")))
												nested.WorksheetDetailsUrl = worksheetItems.Value<string>("umbracoUrlAlias");
											else
												nested.WorksheetDetailsUrl = worksheetItems?.Url();

											if (worksheetItems.SelectWeek != null)
											{
												nested.Category = worksheetItems?.Name;
											}

											nested.Subject = worksheetItems?.SelectSubject?.Name.ToString();
											nested.Topic = worksheetItems?.Topic?.Name.ToString();

											nested.Age = worksheetItems?.AgeTitle?.Name.ToString();
											WorksheetVideosHelper worksheetVideos = new WorksheetVideosHelper();
											nested = worksheetVideos.GetSocialItemsAndSubscriptionDetailsForWorkSheet(worksheetItems, input, nested, myagegroup);

											List.Add(nested);

										}

										//load more display till totalcount more than displayed on page
										model.NestedItems = List;
									}
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "Worksheet - WorksheetDetailsData");
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "Worksheet - WorksheetDetailsData");
				}
				if (countOfTotalWorksheets > input.DisplayCount)
					model.LoadMore = input.DisplayCount.Value;

				model.PageTitle = input?.FilterType;

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "Worksheet - WorksheetDetailsData");
			}

			return model;
		}

		[HttpPost]
		public async Task<ActionResult> GetSingleStructuredProgram(WorksheetInput InputWorksheet)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetVideoModel model = new WorkSheetVideoModel();
				model = await GetStructuredProgramListData(InputWorksheet);

				return PartialView("/Views/Partials/Worksheets/_structuredProgramSingle.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetSingleWorkSheet");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		public async Task<List<DownloadBonusWorkSheet>> GetMostRecommendedBonusWorkSheet()
		{
			dbProxy _db = new dbProxy();
			List<DownloadBonusWorkSheet> downloadBonusWorkSheet = new List<DownloadBonusWorkSheet>();

			string culture = CultureName.GetCultureName();
			if (String.IsNullOrEmpty(culture))
				culture = "en-US";
			downloadBonusWorkSheet = _db.GetDataMultiple("GetMostRecommendedBonusWorkSheet", downloadBonusWorkSheet, null);

			return downloadBonusWorkSheet;
		}


		//public List<int> GetRecentlyDownloadedBonusWorkSheet(int userId)
		//{
		//	dbProxy _db = new dbProxy();
		//	List<int> recentlyDownloadBonusWorkSheet = new List<int>();

		//	try
		//	{
		//		string culture = CultureName.GetCultureName();
		//		if (String.IsNullOrEmpty(culture))
		//			culture = "en-US";

		//		List<SetParameters> sp = new List<SetParameters>()
		//			{
		//				new SetParameters { ParameterName = "@UserId", Value = userId.ToString()}
		//			};
		//		recentlyDownloadBonusWorkSheet = _db.GetDataMultiple("GetRecentlyDownloadedBonusWorkSheet", recentlyDownloadBonusWorkSheet, sp);
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetRecentlyDownloadedBonusWorkSheet");
		//	}

		//	return recentlyDownloadBonusWorkSheet;
		//}

		public async Task<List<int>> GetRecentlyDownloadedBonusWorkSheet(int userId, string Doctype)
		{
			dbProxy _db = new dbProxy();
			List<int> recentlyDownloadBonusWorkSheet = new List<int>();

			try
			{
				string culture = CultureName.GetCultureName();
				if (String.IsNullOrEmpty(culture))
					culture = "en-US";

				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@UserId", Value = userId.ToString()},
						new SetParameters{ParameterName="@DocType",Value=Doctype}
					};
				recentlyDownloadBonusWorkSheet = _db.GetDataMultiple("GetRecentlyDownloadedBonusWorkSheet", recentlyDownloadBonusWorkSheet, sp);
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetRecentlyDownloadedBonusWorkSheet");
			}

			return recentlyDownloadBonusWorkSheet;
		}

		[HttpPost]
		public ActionResult GetStructuredProgramSearchAutoComplete(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				List<AutoCompleteList> model = new List<AutoCompleteList>();
				model = GetStructuredProgramAutoCompleteListData(input);



				return Json(model, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();



				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetStructuredProgramSearchAutoComplete");
			}



			return Json(responce, JsonRequestBehavior.AllowGet);
		}



		public async Task<List<WorkSheetVideoItems>> GetFilterData(StructureProgramRoot worksheetRoot, Videos VideoRoot, WorksheetInput input)
		{
			List<WorkSheetVideoItems> ReturnList = new List<WorkSheetVideoItems>();

			SocialItems social = new SocialItems();

			LoggedIn loggedin = new LoggedIn();

			loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

			int RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

			SessionManagement.StoreInSession(SessionType.BonusWorkSheetCurrentPage, input.currentPage);

			bool UserLoggedInOrNot = HPPlc.Models.SessionExpireAttribute.UserLoggedIn();

			try
			{

				string bitlyLink = String.Empty;

				string culture = CultureName.GetCultureName().Replace("/", "");

				if (String.IsNullOrEmpty(culture))

					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

				var nextGenMediaUrl = worksheetRoot?.Value<IPublishedContent>("seeMoreNextGen");

				if (worksheetRoot != null)

				{

					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

					IEnumerable<StructureProgramItems> WorksheetItem = new List<StructureProgramItems>();

					List<NameListItem> agemaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
													   .Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?.Children?
													   .Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?
													   .OfType<NameListItem>().Where(x => x.IsActice && x.DisplayInBonusWorksheet).ToList();

					List<Subjects> subjectmaster = worksheetRoot?.Children?
													   .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?
													   .OfType<Subjects>().ToList();

					List<Topics> topicmaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>()?.Where(x => x.IsActive == true)?.OrderBy(x => x.TopicName).ToList();

					
					var WorkSheets = worksheetRoot?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "structureProgramItems")
									.OfType<StructureProgramItems>().ToList();

					var FilteredWorkSheets = WorkSheets;

					if (WorkSheets != null)

					{

						List<string> subjectsfilterAllId = null;

						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

						WorkSheetVideoItems items = new WorkSheetVideoItems();

						try

						{

							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

							List<WorksheetCategory> subjects = new List<WorksheetCategory>();

							List<TopicsName> topics = new List<TopicsName>();

							if (input.Mode != null && input.Mode == "single")

							{

								FilteredWorkSheets = GetStructuredProgramSingle(input);

							}

							List<string> seletedAgeGroups = new List<string>();

							List<string> seletedSubjects = new List<string>();

							List<string> seletedTopics = new List<string>();

							if (!string.IsNullOrEmpty(input.selectedAgeGroup) && agemaster != null && agemaster.Any())

							{

								seletedAgeGroups = agemaster.Where(x => input.selectedAgeGroup.Split(',').Contains(x.ItemValue)).Select(x => x.Name).ToList();

								if (seletedAgeGroups != null && seletedAgeGroups.Any())

								{

									FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectAgeGroup.Any(x => seletedAgeGroups.Contains(x.Name))).ToList();

								}

							}

							if (!string.IsNullOrEmpty(input.selectedSubject) && subjectmaster != null && subjectmaster.Any())

							{

								//seletedSubjects = subjectmaster.Where(x => input.selectedSubject.Split(',').Contains(x.SubjectName)).Select(x => x.Name).ToList();

								seletedSubjects = subjectmaster.Where(x => input.selectedSubject.Split(',').Contains(x.SubjectValue.ToString())).Select(x => x.Name).ToList();

								if (seletedSubjects != null && seletedSubjects.Any())

								{

									FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectSubject.Any(x => seletedSubjects.Contains(x.Name))).ToList();

								}

							}

							if (!string.IsNullOrEmpty(input.selectedTopics) && topicmaster != null && topicmaster.Any())

							{

								seletedTopics = topicmaster.Where(x => input.selectedTopics.Split(',').Contains(x.TopicValue.ToString())).Select(x => x.Name).ToList();

								if (seletedTopics != null && seletedTopics.Any())

								{

									FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectTopic.Any(x => seletedTopics.Contains(x.Name))).ToList();

								}

							}

							if (!string.IsNullOrEmpty(input.selectedPaid))

							{

								FilteredWorkSheets = FilteredWorkSheets.Where(x => input.selectedPaid.Split(',').Contains(Convert.ToString(Convert.ToInt16(x.IsPaid)))).ToList();

							}

							if (!string.IsNullOrEmpty(input.searchText) && input.searchText.All(char.IsDigit))
							{
								FilteredWorkSheets = WorkSheets.Where(x => x.Id.ToString().ToLower() == input.searchText.ToLower()).ToList();
							}
							else if (!String.IsNullOrEmpty(input.searchText))
							{
								//get Node Id based on search text
								List<AdvanceSearch> advanceSearches = new List<AdvanceSearch>();
								dbProxy _db = new dbProxy();
								List<SetParameters> sp = new List<SetParameters>()
								{
									new SetParameters { ParameterName = "@Searchtext", Value = input.searchText}
								};
								advanceSearches = _db.GetDataMultiple("USP_Proc_AdvanceSearch", advanceSearches, sp);

								if (advanceSearches == null || advanceSearches.Count == 0)
								{
									advanceSearches = new List<AdvanceSearch>();
								}

								FilteredWorkSheets = FilteredWorkSheets.Where(x => advanceSearches.Any(z => z.NodeId == x.Id)).ToList();
								//else
								//{
								//	var WorkSheetsEmpty = null;
								//	FilteredWorkSheets = WorkSheetsEmpty;
								//}
							}



							//var selectedclass = string.IsNullOrEmpty(input.searchText) ? null : agemaster?.Where(x => input.searchText.ToLower().Contains(x.AlternateClassName.ToLower()))?.FirstOrDefault();
							//var classsearchstr = selectedclass == null ? "" : selectedclass?.AlternateClassName;
							//int placement = string.IsNullOrEmpty(input.searchText) ? -1 : input.searchText.IndexOf(classsearchstr, StringComparison.OrdinalIgnoreCase);

							////var classsearchstr = placement > -1 ? (placement == 0 ? input.searchText.Substring(placement, 8) : input.searchText.Substring(placement - 1, 8)) : "";
							//var searchstr = !string.IsNullOrEmpty(input.searchText) ? (input.searchText.Contains(classsearchstr.ToLower()) && !string.IsNullOrEmpty(classsearchstr) ? input.searchText.ToLower().Replace(classsearchstr.ToLower(), string.Empty) : input.searchText) : "";
							//List<string> searchText = string.IsNullOrEmpty(searchstr) || input.searchText.ToLower().Equals(classsearchstr.ToLower()) ? new List<string>() : searchstr.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
							//var TopicsWorkSheets = new List<StructureProgramItems>();

							//var SubjectsWorkSheets = new List<StructureProgramItems>();

							//var ClassWorkSheets = new List<StructureProgramItems>();

							//var TitleDesciptionWorkSheets = new List<StructureProgramItems>();
							//if (searchText != null && !string.IsNullOrEmpty(classsearchstr))
							//{
							//	searchText.Add(classsearchstr.Trim());
							//}

							//if (searchText != null && searchText.Any())
							//{
							//	var TitleWorkSheets = new List<StructureProgramItems>();
							//	var DesciptionWorkSheets = new List<StructureProgramItems>();
							//	foreach (var search in searchText)
							//	{
							//		var titleWorkSheet = FilteredWorkSheets.Where(x => x.Title.ToLower().Contains(search.ToLower())).ToList();
							//		var desciptionWorkSheet = FilteredWorkSheets.Where(x => x.Description.ToHtmlString().ToLower().Contains(search.ToLower())).ToList();
							//		TitleWorkSheets = TitleWorkSheets.Concat(titleWorkSheet).ToList();
							//		DesciptionWorkSheets = DesciptionWorkSheets.Concat(desciptionWorkSheet).ToList();
							//	}

							//	TitleDesciptionWorkSheets = TitleWorkSheets.Union(DesciptionWorkSheets).DistinctBy(x => x.Id).ToList();
							//}

							//if (searchText != null && searchText.Any() && agemaster != null && agemaster.Any())
							//{

							//	foreach (var search in searchText)
							//	{
							//		var age = agemaster.Where(x => x.AlternateItemName.ToLower().StartsWith(search.ToLower())).ToList();
							//		seletedAgeGroups = age.Select(x => x.Name).ToList();
							//	}

							//	if (seletedAgeGroups != null && seletedAgeGroups.Any())

							//	{

							//		ClassWorkSheets = FilteredWorkSheets.Where(item => item.SelectAgeGroup.Any(x => seletedAgeGroups.Contains(x.Name))).ToList();

							//	}

							//}

							//if (searchText != null && searchText.Any() && subjectmaster != null && subjectmaster.Any())
							//{
							//	foreach (var search in searchText)
							//	{
							//		var subject = subjectmaster.Where(x => x.SubjectName.ToLower().StartsWith(search.ToLower())).ToList();
							//		seletedSubjects = subject.Select(x => x.Name).ToList();
							//	}

							//	if (seletedSubjects != null && seletedSubjects.Any())

							//	{

							//		SubjectsWorkSheets = FilteredWorkSheets.Where(item => item.SelectSubject.Any(x => seletedSubjects.Contains(x.Name))).ToList();

							//	}

							//}

							//if (searchText != null && searchText.Any() && topicmaster != null && topicmaster.Any())
							//{
							//	foreach (var search in searchText)
							//	{
							//		var topic = topicmaster.Where(x => x.TopicName.ToLower().StartsWith(search.ToLower())).ToList();
							//		seletedTopics = topic.Select(x => x.TopicName).ToList();
							//	}

							//	if (seletedTopics != null && seletedTopics.Any())

							//	{

							//		TopicsWorkSheets = FilteredWorkSheets.Where(item => item.SelectTopic.Any(x => seletedTopics.Contains(x.Name))).ToList();

							//	}

							//}

							//if (!string.IsNullOrEmpty(input.searchText) || (TopicsWorkSheets != null && TopicsWorkSheets.Any()) || (SubjectsWorkSheets != null && SubjectsWorkSheets.Any()) || (TitleDesciptionWorkSheets != null && TitleDesciptionWorkSheets.Any()) || (ClassWorkSheets != null && ClassWorkSheets.Any()))

							//{

							//	FilteredWorkSheets = TopicsWorkSheets.Union(SubjectsWorkSheets).Union(ClassWorkSheets).Union(TitleDesciptionWorkSheets).ToList();

							//	FilteredWorkSheets = FilteredWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							

							if (input.sortBy == "1")
							{

								FilteredWorkSheets = FilteredWorkSheets.OrderByDescending(x => x.CreateDate).Take(worksheetRoot.NoOfRecentlyAddedWorksheet).ToList();
							}

							else if (input.sortBy == "2")
							{

								var MostRecommendedBonusWorkSheet = await GetMostRecommendedBonusWorkSheet();

								if (MostRecommendedBonusWorkSheet != null)

								{

									FilteredWorkSheets = (from worksheets in FilteredWorkSheets

														  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on worksheets.Id equals recommendedbonusworksheet.WorkSheetId

														  orderby recommendedbonusworksheet.DownloadCount descending

														  select worksheets).ToList();

								}

							}
							else if (input.sortBy == "3")
							{
								FilteredWorkSheets = FilteredWorkSheets.OrderBy(x => x.Title).ToList();
							}
							else if (FilteredWorkSheets.Count != 0 && string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && UserLoggedInOrNot == false)
							{
								FilteredWorkSheets = FilteredWorkSheets.OrderBy(x => (x.RankingIndex == 0 ? WorkSheets.Count : x.RankingIndex)).ToList();
							}
							else
							{
								if (UserLoggedInOrNot == true)
								{
									dbAccessClass db = new dbAccessClass();
									List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
									myagegroup = db.GetUserSelectedUserGroup();

									FilteredWorkSheets = FilteredWorkSheets?.OrderByDescending(x => x?.SelectAgeGroup.Any(y => myagegroup.Any(c => c?.AgeGroup != null && y?.Name.ToString() == c?.AgeGroup))).ToList(); //FilteredWorkSheets.OrderByDescending(lst => myagegroup.Any(y => y.AgeGroup == lst.SelectAgeGroup.Where(c => c.Name == y.AgeGroup))).ToList();
								}
							}
							//else
							//if (FilteredWorkSheets.Count == 0 && !string.IsNullOrEmpty(input.searchText))
							//{
							//    //FilteredWorkSheets = WorkSheets.OrderBy(x => (x.RankingIndex == 0 ? WorkSheets.Count : x.RankingIndex)).ToList();
							//    FilteredWorkSheets = WorkSheets.OrderBy(x => x.Title.ToLower().StartsWith(input.searchText.ToLower().Split(' ')?.FirstOrDefault()) || x.Description.ToHtmlString().ToLower().StartsWith(input.searchText.ToLower().Split(' ')?.FirstOrDefault())).ToList();

							//}

							//If record found then 
							if (input.NoRecordFound)
							{
								if (UserLoggedInOrNot)
								{
									HPPlc.Models.dbAccessClass db = new HPPlc.Models.dbAccessClass();
									List<HPPlc.Models.SelectedAgeGroup> myagegroup = new List<HPPlc.Models.SelectedAgeGroup>();
									myagegroup = db.GetUserSelectedUserGroup();
									var agegroup = string.Join(",", myagegroup.Select(x => x.AgeGroup));
									List<DownloadBonusWorkSheet> MostRecommendedBonusWorkSheet = GetAllMostRecommendedBonusWorkSheet(agegroup);
									if (MostRecommendedBonusWorkSheet != null)
									{
										FilteredWorkSheets = (from worksheets in FilteredWorkSheets
															  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on worksheets.Id equals recommendedbonusworksheet.WorkSheetId
															  orderby recommendedbonusworksheet.DownloadCount descending
															  select worksheets).ToList();
									}
									//HPPlc.Models.dbAccessClass db = new HPPlc.Models.dbAccessClass();

									//List<HPPlc.Models.SelectedAgeGroup> myagegroup = new List<HPPlc.Models.SelectedAgeGroup>();

									//myagegroup = db.GetUserSelectedUserGroup();

									//List<DownloadBonusWorkSheet> MostRecommendedBonusWorkSheet = GetAllMostRecommendedBonusWorkSheet(input);

									//if (MostRecommendedBonusWorkSheet != null)

									//{

									//	FilteredWorkSheets = (from worksheets in FilteredWorkSheets

									//						  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on worksheets.Id equals recommendedbonusworksheet.WorkSheetId

									//						  orderby recommendedbonusworksheet.DownloadCount descending

									//						  select worksheets).ToList();

									//}

								}

								else

								{

									List<DownloadBonusWorkSheet> MostRecommendedBonusWorkSheet = GetAllMostRecommendedBonusWorkSheet();

									if (MostRecommendedBonusWorkSheet != null)

									{

										FilteredWorkSheets = (from worksheets in FilteredWorkSheets

															  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on worksheets.Id equals recommendedbonusworksheet.WorkSheetId

															  orderby recommendedbonusworksheet.DownloadCount descending

															  select worksheets).ToList();

									}

								}

							}

							//End if record not found

							var RecentlyDownloadedBonusWorkSheet = await GetRecentlyDownloadedBonusWorkSheet(RefUserId, "BonusWorkSheet");

							items.NestedItems = new List<dynamic>();

							items.totalItems = FilteredWorkSheets.Count;

							var WorkSheetsPaged = FilteredWorkSheets.Skip((input.currentPage - 1) * (worksheetRoot.NoOfDisplayWorksheet)).Take(worksheetRoot.NoOfDisplayWorksheet).ToList();

							//if (!string.IsNullOrEmpty(input.selectedAgeGroup) || !string.IsNullOrEmpty(input.selectedSubject))
							//{
							//    WorkSheetsPaged = FilteredWorkSheets;
							//    //var worksheetchild = worksheetRoot?.DescendantsOrSelf()?
							//    //    .Where(x => x.ContentType.Alias == "structureProgramItems").ToList();
							//    //var videochild = VideoRoot?.DescendantsOrSelf()?.Where(x => x.Parent?.ContentType.Alias == "videoListingAgeWise")?.OfType<Video>().Where(x => x.IsActive); ;
							//    //var output = worksheetchild.Union(videochild);
							//}
							//else 
							//if ((!string.IsNullOrEmpty(input.selectedAgeGroup) || !string.IsNullOrEmpty(input.selectedSubject) || !string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup) || !string.IsNullOrEmpty(input.VideosInput?.selectedCategory)) || input.selectedPaid == "1" || input.selectedPaid == "2" || !string.IsNullOrEmpty(input.searchText))
							//{
							//    WorkSheetsPaged = FilteredWorkSheets.Skip((input.currentPage) * (worksheetRoot.NoOfDisplayWorksheet + 1)).Take(worksheetRoot.NoOfDisplayWorksheet + 1).ToList();
							//}
							//Parallel.ForEach(WorkSheetsPaged, async WorkSheet =>

							items.TopicsUrl = FilteredWorkSheets?.FirstOrDefault()?.Url();
							items.SubjectUrl = FilteredWorkSheets?.FirstOrDefault()?.Parent?.Url();
							items.ClassUrl = FilteredWorkSheets?.FirstOrDefault()?.Parent?.Parent?.Url();

							foreach (var WorkSheet in WorkSheetsPaged)
							{
								try
								{
									var SelectedAgeValues = WorkSheet?.SelectAgeGroup?.Select(x => x.Udi);

									var SelectedSubject = WorkSheet?.SelectSubject?.Select(x => x.Udi);

									var SelectedTopic = WorkSheet?.SelectTopic?.Select(x => x.Udi);

									NestedItems nested = new NestedItems();

									var Image = WorkSheet?.DesktopImage;

									string altText = Image?.Value<string>("altText");

									var NextGenImage = WorkSheet?.DesktopNextGenImage;

									nested.Category = string.Join(",", WorkSheet?.Name);

									nested.Title = WorkSheet?.Title;

									nested.SubTitle = WorkSheet?.SubTitle;

									var selectedAgeContent = Umbraco.Content(SelectedAgeValues);

									var SelectedClasssValues = selectedAgeContent?.Select(x => x.Value<string>("alternateClassName")).ToList();

									nested.SelectedClasses = SelectedClasssValues;

									var selectedSubjectContent = Umbraco.Content(SelectedSubject);



									//var SelectedSubjectValues = selectedSubjectContent.Select(x => x.Value<string>("itemName")).ToList();

									//nested.SelectedSubjects = SelectedSubjectValues;

									//var selectedTopicContent = Umbraco.Content(SelectedTopic);

									//var SelectedTopicValues = selectedTopicContent.Select(x => x.Value<string>("topicName")).ToList();

									var SelectedSubjectValues = selectedSubjectContent?.Select(x => x.Value<string>("subjectName")).ToList();

									nested.SelectedSubjects = SelectedSubjectValues;

									var selectedTopicContent = Umbraco.Content(SelectedTopic);

									var SelectedTopicValues = selectedTopicContent?.Select(x => x.Value<string>("topicName")).ToList();

									nested.SelectedTopics = SelectedTopicValues;

									nested.IsPaid = WorkSheet?.IsPaid ?? false;
									//nested.IsPaid = true;


									nested.PreviewPdf = WorkSheet?.UploadPreviewPdf;

									nested.IsEnabledForDetailsPage = WorkSheet?.IsEnableForDetailsPage ?? false;

									nested.WorksheetId = WorkSheet?.Id ?? 0;

									//nested.WorksheetDetailsUrl = WorkSheet?.Url();

									if (!String.IsNullOrEmpty(WorkSheet.Value<string>("umbracoUrlAlias")))

										nested.WorksheetDetailsUrl = WorkSheet.Value<string>("umbracoUrlAlias");

									else

										nested.WorksheetDetailsUrl = WorkSheet?.Url();

									string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet?.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");
									//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + "Paid";

									nested.subscriptionStatus = new SubscriptionStatus();

									string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");
									//string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + "Paid";

									nested.subscriptionStatus.DownloadString = DownloadString;

									nested.subscriptionStatus.DownloadUrl = downloadUrl;

									if (Image != null)

									{

										if (NextGenImage != null)
										{

											nested.NextGenImage = NextGenImage.Url();

										}

										nested.AltText = altText;

										nested.ImagesSrc = Image.Url();

									}

									nested.Description = WorkSheet?.Description;

									if (RecentlyDownloadedBonusWorkSheet != null && RecentlyDownloadedBonusWorkSheet.Count != 0)

									{

										nested.RecentlyDownloaded = RecentlyDownloadedBonusWorkSheet.Contains(WorkSheet.Id);

									}

									var commonContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
												  .Where(x => x?.ContentType.Alias == "structureProgramRoot").OfType<StructureProgramRoot>().FirstOrDefault();

									if (commonContent != null)
									{
										string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
										string referralText = string.Empty;
										string wkstPlanUrl = string.Empty;

										if (!String.IsNullOrEmpty(WorkSheet.Value<string>("umbracoUrlAlias")))
											wkstPlanUrl = domain + WorkSheet.Value<string>("umbracoUrlAlias");
										else
											wkstPlanUrl = domain + WorkSheet?.Url();

										

										if (loggedin != null)
										{
											if (!String.IsNullOrWhiteSpace(commonContent.ReferralContent.ToString()) && !String.IsNullOrWhiteSpace(loggedin.ReferralCode))
											{
												referralText = commonContent.ReferralContent.ToString().ToString().Replace("<p>", "").Replace("</p>", "");

												referralText = referralText.Replace("{referal}", loggedin.ReferralCode);

												referralText = referralText.Replace("{loginurl}", domain + "my-account/login?referralcode=" + loggedin.ReferralCode);
											}
										}

										if (!String.IsNullOrEmpty(commonContent.FacebookContent))
										{
											string FacebookContent = commonContent.FacebookContent.Replace("{worksheeturl}", wkstPlanUrl);

											if (!String.IsNullOrEmpty(FacebookContent))
												social.FBShare = FacebookContent.Replace("{referal}", referralText);
											else
											{
												string facebookContent = FacebookContent.Replace("{referal}", "");
												social.FBShare = facebookContent;
											}
										}

										if (!String.IsNullOrEmpty(commonContent.WhatsAppContent))
										{
											string WhatsAppContent = commonContent.WhatsAppContent.Replace("{worksheeturl}", wkstPlanUrl);

											if (!String.IsNullOrEmpty(referralText))
												social.WhatAppShare = WhatsAppContent.Replace("{referal}", referralText);
											else
											{
												string whatsappContent = WhatsAppContent.Replace("{referal}", "");
												social.WhatAppShare = whatsappContent;
											}
										}

										if (!String.IsNullOrEmpty(commonContent.MailContent))
										{
											string MailContent = commonContent.MailContent.Replace("{worksheeturl}", wkstPlanUrl);

											if (!String.IsNullOrEmpty(referralText))
												social.EmailShare = MailContent.Replace("{referal}", referralText);
											else
											{
												string mailContent = MailContent.Replace("{referal}", "");
												social.EmailShare = mailContent + "`" + nested?.Title;
											}
										}
									}

									nested.socialItems = social;
									nested.IsWorkSheet = true;

									items.NestedItems.Add(nested);
								}
								catch{ }
							}
							//);
							//items.NestedItems = NestedItems;

							if (VideoRoot != null && (!string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup) || !string.IsNullOrEmpty(input.VideosInput?.selectedCategory) || !string.IsNullOrEmpty(input.searchText) || !string.IsNullOrEmpty(input.selectedPaid)) && input.selectedPaid == "2" && string.IsNullOrEmpty(input.selectedTopics))
							{
								var mediaUrl = VideoRoot?.Value<IPublishedContent>("seeMoreMedia");

								var nextGenVideoMediaUrl = VideoRoot?.SeeMoreNextGen;

								int firstTimeDisplayVideos = VideoRoot.FirstTimeDisplayVideos;

								var RecentlyDownloadedVideoBonusWorkSheet = await GetRecentlyDownloadedBonusWorkSheet(RefUserId, "BonusVideoWorkSheet");
								try
								{

									VideosItems videosItems = new VideosItems();

									var Videos = VideoRoot?.DescendantsOrSelf()?.Where(x => x.Parent?.ContentType.Alias == "videoListingAgeWise"
									)?.OfType<Video>().Where(x => x.IsActive);

									if (!string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup))
									{
										seletedAgeGroups = agemaster.Where(x => input.VideosInput.selectedAgeGroup.Split(',').Contains(x.ItemValue)).Select(x => x.Name).ToList();

										if (seletedAgeGroups != null && seletedAgeGroups.Any())
										{

											Videos = Videos.Where(x => seletedAgeGroups.Contains(x.AgeTitle?.Name)).ToList();
										}

									}

									if (!string.IsNullOrEmpty(input.VideosInput?.selectedCategory))

									{

										seletedSubjects = subjectmaster.Where(x => input.VideosInput.selectedCategory.Split(',').Contains(x.SubjectValue.ToString())).Select(x => x.Name).ToList();

										if (seletedSubjects != null && seletedSubjects.Any())

										{

											Videos = Videos.Where(item => item.Category.Any(x => seletedSubjects.Contains(x.Name))).ToList();

										}

									}

									if (!string.IsNullOrEmpty(input.searchText) && agemaster != null && agemaster.Any())

									{

										seletedAgeGroups = agemaster.Where(x => x.AlternateItemName.ToLower().ContainsAny(input.searchText.ToLower().Split(' '))).Select(x => x.Name).ToList();

										if (seletedAgeGroups != null && seletedAgeGroups.Any())

										{

											Videos = Videos.Where(x => seletedAgeGroups.Contains(x.AgeTitle?.Name)).ToList();

										}

									}

									if (!string.IsNullOrEmpty(input.searchText) && subjectmaster != null && subjectmaster.Any())

									{

										seletedSubjects = subjectmaster.Where(x => x.SubjectName.ToLower().ContainsAny(input.searchText.ToLower().Split(' '))).Select(x => x.Name).ToList();

										if (seletedSubjects != null && seletedSubjects.Any())

										{

											Videos = Videos.Where(item => item.Category.Any(x => seletedSubjects.Contains(x.Name))).ToList();

										}

									}

									if (!string.IsNullOrEmpty(input.searchText))

									{

										Videos = Videos.Where(item => item.Title.ToLower().ContainsAny(input.searchText.ToLower().Split(' ')) || item.Description.ToHtmlString().ToLower().ContainsAny(input.searchText.ToLower().Split(' '))).ToList();

									}
									if (input.sortBy == "1")
									{

										Videos = Videos.OrderByDescending(x => x.CreateDate).Take(worksheetRoot.NoOfRecentlyAddedWorksheet).ToList();
									}

									else if (input.sortBy == "2")
									{
										var MostRecommendedBonusWorkSheet = await GetMostRecommendedBonusVideoWorkSheet();
										//var MostRecommendedBonusWorkSheet = await GetMostRecommendedBonusWorkSheet();

										if (MostRecommendedBonusWorkSheet != null)

										{

											Videos = (from videos in Videos

													  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on videos.Id equals recommendedbonusworksheet.WorkSheetId

													  orderby recommendedbonusworksheet.DownloadCount descending

													  select videos).ToList();

										}

									}
									else if (input.sortBy == "3")
									{
										Videos = Videos.OrderBy(x => x.Title).ToList();
									}
									if (Videos != null && Videos.Any())

									{

										List<VideosNestedItems> nesteds = new List<VideosNestedItems>();

										string videoPageURL = string.Empty;

										VideosNestedItems nested = new VideosNestedItems();

										items.totalItems = Videos.Count();
										Videos = Videos.Skip((input.currentPage - 1) * (worksheetRoot.NoOfDisplayWorksheet)).Take(worksheetRoot.NoOfDisplayWorksheet).ToList();

										//Parallel.ForEach(Videos, async videoItems =>
										foreach (var videoItems in Videos)
										{

											nested = await GetVideos(videoItems, input.VideosInput);

											videoPageURL = videoItems.Parent.Url().ToString();

											nested.IsWorkSheet = false;

											if (RecentlyDownloadedVideoBonusWorkSheet != null && RecentlyDownloadedVideoBonusWorkSheet.Count != 0)
											{
												nested.RecentlyDownloaded = RecentlyDownloadedVideoBonusWorkSheet.Contains(videoItems.Id);
											}


											items.NestedItems.Add(nested);

										}
										//);

										//videosItems.NestedItems = nesteds;

										//See More

										VideosSeeMore seeMore = new VideosSeeMore();

										seeMore.VideoDetailsUrl = videoPageURL;

										if (mediaUrl != null)

										{

											if (nextGenMediaUrl != null)

												seeMore.NextGenMediaUrl = nextGenVideoMediaUrl.Url();

											seeMore.MediaUrl = mediaUrl.Url();

										}

										videosItems.SeeMore = seeMore;

									}

								}

								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetVideosListData");
								}

							}

							//else if (VideoRoot != null && (string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup) || string.IsNullOrEmpty(input.VideosInput?.selectedCategory))

							//    && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText) && string.IsNullOrEmpty(input.selectedTopics))

							//{

							//    var mediaUrl = VideoRoot?.Value<IPublishedContent>("seeMoreMedia");

							//    var nextGenVideoMediaUrl = VideoRoot?.SeeMoreNextGen;

							//    int firstTimeDisplayVideos = VideoRoot.FirstTimeDisplayVideos;

							//    var RecentlyDownloadedVideoBonusWorkSheet = await GetRecentlyDownloadedBonusWorkSheet(RefUserId, "BonusVideoWorkSheet");

							//    try
							//    {

							//        VideosItems videosItems = new VideosItems();

							//        var Videos = VideoRoot?.DescendantsOrSelf()?.Where(x => x.Parent?.ContentType.Alias == "videoListingAgeWise"

							//        )?.OfType<Video>().Where(x => x.IsActive);

							//        if (Videos != null && Videos.Any())

							//        {

							//            List<VideosNestedItems> nesteds = new List<VideosNestedItems>();

							//            string videoPageURL = string.Empty;

							//            VideosNestedItems nested = new VideosNestedItems();

							//            var random = new Random();

							//            int index = random.Next(Videos.Count());

							//            Video videoItems = Videos.ElementAt(index);

							//            nested = await GetVideos(videoItems, input.VideosInput);

							//            videoPageURL = videoItems.Parent.Url().ToString();

							//            nested.IsWorkSheet = false;

							//            if (RecentlyDownloadedVideoBonusWorkSheet != null && RecentlyDownloadedVideoBonusWorkSheet.Count != 0)
							//            {
							//                nested.RecentlyDownloaded = RecentlyDownloadedVideoBonusWorkSheet.Contains(videoItems.Id);
							//            }


							//            if (items.NestedItems.Count > (worksheetRoot.DefautPositionOfVideoInPage - 1))

							//            {

							//                items.NestedItems.Insert(worksheetRoot.DefautPositionOfVideoInPage, nested);

							//            }

							//            else

							//            {

							//                items.NestedItems.Add(nested);

							//            }

							//            //videosItems.NestedItems = nesteds;

							//            //See More

							//            VideosSeeMore seeMore = new VideosSeeMore();

							//            seeMore.VideoDetailsUrl = videoPageURL;

							//            if (mediaUrl != null)

							//            {

							//                if (nextGenMediaUrl != null)

							//                    seeMore.NextGenMediaUrl = nextGenVideoMediaUrl.Url();

							//                seeMore.MediaUrl = mediaUrl.Url();

							//            }

							//            videosItems.SeeMore = seeMore;

							//        }

							//    }

							//    catch (Exception ex)

							//    {

							//        Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosListData");

							//    }

							//}

							//if ((!string.IsNullOrEmpty(input.selectedAgeGroup) || !string.IsNullOrEmpty(input.selectedSubject) || !string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup) || !string.IsNullOrEmpty(input.VideosInput?.selectedCategory)) && string.IsNullOrEmpty(input.searchText))

							//{

							//    items.totalItems = items.NestedItems.Count;

							//    items.NestedItems = items.NestedItems.Skip((input.currentPage - 1) * (worksheetRoot.NoOfDisplayWorksheet + 1)).Take(worksheetRoot.NoOfDisplayWorksheet + 1).ToList();

							//}

						}

						catch { }

						ReturnList.Add(items);

					}

				}

			}

			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");
			}

			return ReturnList;

		}



		//public async Task<List<WorkSheetVideoItems>> GetFilterData(StructureProgramRoot worksheetRoot, Videos VideoRoot, WorksheetInput input)
		//{
		//	List<WorkSheetVideoItems> ReturnList = new List<WorkSheetVideoItems>();

		//	SocialItems social = new SocialItems();

		//	LoggedIn loggedin = new LoggedIn();

		//	loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

		//	int RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

		//	SessionManagement.StoreInSession(SessionType.BonusWorkSheetCurrentPage, input.currentPage);

		//	try
		//	{

		//		string bitlyLink = String.Empty;

		//		string culture = CultureName.GetCultureName().Replace("/", "");

		//		if (String.IsNullOrEmpty(culture))

		//			culture = "en-US";

		//		_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//		var nextGenMediaUrl = worksheetRoot?.Value<IPublishedContent>("seeMoreNextGen");

		//		if (worksheetRoot != null)

		//		{

		//			_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//			IEnumerable<StructureProgramItems> WorksheetItem = new List<StructureProgramItems>();

		//			List<NameListItem> agemaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
		//											   .Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?.Children?
		//											   .Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?
		//											   .OfType<NameListItem>().Where(x => x.IsActice && x.DisplayInBonusWorksheet).ToList();

		//			List<Subjects> subjectmaster = worksheetRoot?.Children?
		//											   .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?
		//											   .OfType<Subjects>().ToList();

		//			List<Topics> topicmaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>()?.Where(x => x.IsActive == true)?.OrderBy(x => x.TopicName).ToList();

		//			var WorkSheets = worksheetRoot?.DescendantsOrSelf()?
		//							.Where(x => x.ContentType.Alias == "structureProgramItems")
		//							.OfType<StructureProgramItems>().ToList();

		//			var FilteredWorkSheets = WorkSheets;

		//			if (WorkSheets != null)

		//			{

		//				List<string> subjectsfilterAllId = null;

		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//				WorkSheetVideoItems items = new WorkSheetVideoItems();

		//				try

		//				{

		//					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//					List<WorksheetCategory> subjects = new List<WorksheetCategory>();

		//					List<TopicsName> topics = new List<TopicsName>();

		//					if (input.Mode != null && (input.Mode == "subject" || input.Mode == "related" || input.Mode == "single"))

		//					{

		//						FilteredWorkSheets = GetStructuredProgramSingle(input);

		//					}

		//					List<string> seletedAgeGroups = new List<string>();

		//					List<string> seletedSubjects = new List<string>();

		//					List<string> seletedTopics = new List<string>();

		//					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && agemaster != null && agemaster.Any())

		//					{

		//						seletedAgeGroups = agemaster.Where(x => input.selectedAgeGroup.Split(',').Contains(x.ItemValue)).Select(x => x.Name).ToList();

		//						if (seletedAgeGroups != null && seletedAgeGroups.Any())

		//						{

		//							FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectAgeGroup.Any(x => seletedAgeGroups.Contains(x.Name))).ToList();

		//						}

		//					}

		//					if (!string.IsNullOrEmpty(input.selectedSubject) && subjectmaster != null && subjectmaster.Any())

		//					{

		//						//seletedSubjects = subjectmaster.Where(x => input.selectedSubject.Split(',').Contains(x.SubjectName)).Select(x => x.Name).ToList();

		//						seletedSubjects = subjectmaster.Where(x => input.selectedSubject.Split(',').Contains(x.SubjectValue.ToString())).Select(x => x.Name).ToList();

		//						if (seletedSubjects != null && seletedSubjects.Any())

		//						{

		//							FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectSubject.Any(x => seletedSubjects.Contains(x.Name))).ToList();

		//						}

		//					}

		//					if (!string.IsNullOrEmpty(input.selectedTopics) && topicmaster != null && topicmaster.Any())

		//					{

		//						seletedTopics = topicmaster.Where(x => input.selectedTopics.Split(',').Contains(x.TopicValue.ToString())).Select(x => x.Name).ToList();

		//						if (seletedTopics != null && seletedTopics.Any())

		//						{

		//							FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectTopic.Any(x => seletedTopics.Contains(x.Name))).ToList();

		//						}

		//					}

		//					if (!string.IsNullOrEmpty(input.selectedPaid))

		//					{

		//						FilteredWorkSheets = FilteredWorkSheets.Where(x => input.selectedPaid.Split(',').Contains(Convert.ToString(Convert.ToInt16(x.IsPaid)))).ToList();

		//					}
		//					var selectedclass = string.IsNullOrEmpty(input.searchText) ? null : agemaster?.Where(x => input.searchText.ToLower().Contains(x.AlternateClassName.ToLower()))?.FirstOrDefault();
		//					var classsearchstr = selectedclass == null ? "" : selectedclass?.AlternateClassName;
		//					int placement = string.IsNullOrEmpty(input.searchText) ? -1 : input.searchText.IndexOf(classsearchstr, StringComparison.OrdinalIgnoreCase);

		//					//var classsearchstr = placement > -1 ? (placement == 0 ? input.searchText.Substring(placement, 8) : input.searchText.Substring(placement - 1, 8)) : "";
		//					var searchstr = !string.IsNullOrEmpty(input.searchText) ? (input.searchText.Contains(classsearchstr.ToLower()) && !string.IsNullOrEmpty(classsearchstr) ? input.searchText.ToLower().Replace(classsearchstr.ToLower(), string.Empty) : input.searchText) : "";
		//					List<string> searchText = string.IsNullOrEmpty(searchstr) || input.searchText.ToLower().Equals(classsearchstr.ToLower()) ? new List<string>() : searchstr.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
		//					var TopicsWorkSheets = new List<StructureProgramItems>();

		//					var SubjectsWorkSheets = new List<StructureProgramItems>();

		//					var ClassWorkSheets = new List<StructureProgramItems>();

		//					var TitleDesciptionWorkSheets = new List<StructureProgramItems>();
		//					if (searchText != null && !string.IsNullOrEmpty(classsearchstr))
		//					{
		//						searchText.Add(classsearchstr.Trim());
		//					}

		//					if (searchText != null && searchText.Any())
		//					{
		//						var TitleWorkSheets = new List<StructureProgramItems>();
		//						var DesciptionWorkSheets = new List<StructureProgramItems>();
		//						foreach (var search in searchText)
		//						{
		//							var titleWorkSheet = FilteredWorkSheets.Where(x => x.Title.ToLower().Contains(search.ToLower())).ToList();
		//							var desciptionWorkSheet = FilteredWorkSheets.Where(x => x.Description.ToHtmlString().ToLower().Contains(search.ToLower())).ToList();
		//							TitleWorkSheets = TitleWorkSheets.Concat(titleWorkSheet).ToList();
		//							DesciptionWorkSheets = DesciptionWorkSheets.Concat(desciptionWorkSheet).ToList();
		//						}

		//						TitleDesciptionWorkSheets = TitleWorkSheets.Union(DesciptionWorkSheets).DistinctBy(x => x.Id).ToList();
		//					}

		//					if (searchText != null && searchText.Any() && agemaster != null && agemaster.Any())
		//					{

		//						foreach (var search in searchText)
		//						{
		//							var age = agemaster.Where(x => x.AlternateItemName.ToLower().StartsWith(search.ToLower())).ToList();
		//							seletedAgeGroups = age.Select(x => x.Name).ToList();
		//						}

		//						if (seletedAgeGroups != null && seletedAgeGroups.Any())

		//						{

		//							ClassWorkSheets = FilteredWorkSheets.Where(item => item.SelectAgeGroup.Any(x => seletedAgeGroups.Contains(x.Name))).ToList();

		//						}

		//					}

		//					if (searchText != null && searchText.Any() && subjectmaster != null && subjectmaster.Any())
		//					{
		//						foreach (var search in searchText)
		//						{
		//							var subject = subjectmaster.Where(x => x.SubjectName.ToLower().StartsWith(search.ToLower())).ToList();
		//							seletedSubjects = subject.Select(x => x.Name).ToList();
		//						}

		//						if (seletedSubjects != null && seletedSubjects.Any())

		//						{

		//							SubjectsWorkSheets = FilteredWorkSheets.Where(item => item.SelectSubject.Any(x => seletedSubjects.Contains(x.Name))).ToList();

		//						}

		//					}

		//					if (searchText != null && searchText.Any() && topicmaster != null && topicmaster.Any())
		//					{
		//						foreach (var search in searchText)
		//						{
		//							var topic = topicmaster.Where(x => x.TopicName.ToLower().StartsWith(search.ToLower())).ToList();
		//							seletedTopics = topic.Select(x => x.TopicName).ToList();
		//						}

		//						if (seletedTopics != null && seletedTopics.Any())

		//						{

		//							TopicsWorkSheets = FilteredWorkSheets.Where(item => item.SelectTopic.Any(x => seletedTopics.Contains(x.Name))).ToList();

		//						}

		//					}

		//					if (!string.IsNullOrEmpty(input.searchText) || (TopicsWorkSheets != null && TopicsWorkSheets.Any()) || (SubjectsWorkSheets != null && SubjectsWorkSheets.Any()) || (TitleDesciptionWorkSheets != null && TitleDesciptionWorkSheets.Any()) || (ClassWorkSheets != null && ClassWorkSheets.Any()))

		//					{

		//						FilteredWorkSheets = TopicsWorkSheets.Union(SubjectsWorkSheets).Union(ClassWorkSheets).Union(TitleDesciptionWorkSheets).ToList();

		//						FilteredWorkSheets = FilteredWorkSheets.DistinctBy(x => x.Id).ToList();

		//					}

		//					if (!string.IsNullOrEmpty(input.searchText) && input.searchText.All(char.IsDigit))

		//					{

		//						FilteredWorkSheets = WorkSheets.Where(x => x.Id.ToString().ToLower() == input.searchText.ToLower()).ToList();

		//					}

		//					if (input.sortBy == "1")
		//					{

		//						FilteredWorkSheets = FilteredWorkSheets.OrderByDescending(x => x.CreateDate).Take(worksheetRoot.NoOfRecentlyAddedWorksheet).ToList();
		//					}

		//					else if (input.sortBy == "2")
		//					{

		//						var MostRecommendedBonusWorkSheet = await GetMostRecommendedBonusWorkSheet();

		//						if (MostRecommendedBonusWorkSheet != null)

		//						{

		//							FilteredWorkSheets = (from worksheets in FilteredWorkSheets

		//												  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on worksheets.Id equals recommendedbonusworksheet.WorkSheetId

		//												  orderby recommendedbonusworksheet.DownloadCount

		//												  select worksheets).ToList();

		//						}

		//					}
		//					else if (input.sortBy == "3")
		//					{
		//						FilteredWorkSheets = FilteredWorkSheets.OrderBy(x => x.Title).ToList();
		//					}
		//					else if (FilteredWorkSheets.Count != 0 && string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid))
		//					{
		//						FilteredWorkSheets = FilteredWorkSheets.OrderBy(x => (x.RankingIndex == 0 ? WorkSheets.Count : x.RankingIndex)).ToList();
		//					}
		//					//else
		//					//if (FilteredWorkSheets.Count == 0 && !string.IsNullOrEmpty(input.searchText))
		//					//{
		//					//    //FilteredWorkSheets = WorkSheets.OrderBy(x => (x.RankingIndex == 0 ? WorkSheets.Count : x.RankingIndex)).ToList();
		//					//    FilteredWorkSheets = WorkSheets.OrderBy(x => x.Title.ToLower().StartsWith(input.searchText.ToLower().Split(' ')?.FirstOrDefault()) || x.Description.ToHtmlString().ToLower().StartsWith(input.searchText.ToLower().Split(' ')?.FirstOrDefault())).ToList();

		//					//}
		//					var RecentlyDownloadedBonusWorkSheet = await GetRecentlyDownloadedBonusWorkSheet(RefUserId, "BonusWorkSheet");

		//					items.NestedItems = new List<dynamic>();

		//					items.totalItems = FilteredWorkSheets.Count;

		//					var WorkSheetsPaged = FilteredWorkSheets.Skip((input.currentPage - 1) * (worksheetRoot.NoOfDisplayWorksheet)).Take(worksheetRoot.NoOfDisplayWorksheet).ToList();

		//					if (!string.IsNullOrEmpty(input.selectedAgeGroup) || !string.IsNullOrEmpty(input.selectedSubject))
		//					{
		//						WorkSheetsPaged = FilteredWorkSheets;
		//						//var worksheetchild = worksheetRoot?.DescendantsOrSelf()?
		//						//    .Where(x => x.ContentType.Alias == "structureProgramItems").ToList();
		//						//var videochild = VideoRoot?.DescendantsOrSelf()?.Where(x => x.Parent?.ContentType.Alias == "videoListingAgeWise")?.OfType<Video>().Where(x => x.IsActive); ;
		//						//var output = worksheetchild.Union(videochild);
		//					}
		//					else if ((!string.IsNullOrEmpty(input.selectedAgeGroup) || !string.IsNullOrEmpty(input.selectedSubject) || !string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup) || !string.IsNullOrEmpty(input.VideosInput?.selectedCategory)) || input.selectedPaid == "1" || input.selectedPaid == "2" || !string.IsNullOrEmpty(input.searchText))
		//					{
		//						WorkSheetsPaged = FilteredWorkSheets.Skip((input.currentPage) * (worksheetRoot.NoOfDisplayWorksheet + 1)).Take(worksheetRoot.NoOfDisplayWorksheet + 1).ToList();
		//					}

		//					//Parallel.ForEach(WorkSheetsPaged, async WorkSheet =>
		//					foreach (var WorkSheet in WorkSheetsPaged)
		//					{
		//						var SelectedAgeValues = WorkSheet?.SelectAgeGroup?.Select(x => x.Udi);

		//						var SelectedSubject = WorkSheet?.SelectSubject?.Select(x => x.Udi);

		//						var SelectedTopic = WorkSheet?.SelectTopic?.Select(x => x.Udi);

		//						NestedItems nested = new NestedItems();

		//						var Image = WorkSheet?.DesktopImage;

		//						string altText = Image?.Value<string>("altText");

		//						var NextGenImage = WorkSheet?.DesktopNextGenImage;

		//						nested.Category = string.Join(",", WorkSheet?.Name);

		//						nested.Title = WorkSheet?.Title;

		//						nested.SubTitle = WorkSheet?.SubTitle;

		//						var selectedAgeContent = Umbraco.Content(SelectedAgeValues);

		//						var SelectedClasssValues = selectedAgeContent?.Select(x => x.Value<string>("alternateClassName")).ToList();

		//						nested.SelectedClasses = SelectedClasssValues;

		//						var selectedSubjectContent = Umbraco.Content(SelectedSubject);

		//						//var SelectedSubjectValues = selectedSubjectContent.Select(x => x.Value<string>("itemName")).ToList();

		//						//nested.SelectedSubjects = SelectedSubjectValues;

		//						//var selectedTopicContent = Umbraco.Content(SelectedTopic);

		//						//var SelectedTopicValues = selectedTopicContent.Select(x => x.Value<string>("topicName")).ToList();

		//						var SelectedSubjectValues = selectedSubjectContent?.Select(x => x.Value<string>("subjectName")).ToList();

		//						nested.SelectedSubjects = SelectedSubjectValues;

		//						var selectedTopicContent = Umbraco.Content(SelectedTopic);

		//						var SelectedTopicValues = selectedTopicContent?.Select(x => x.Value<string>("topicName")).ToList();

		//						nested.SelectedTopics = SelectedTopicValues;

		//						nested.IsPaid = WorkSheet?.IsPaid ?? false;
		//						//nested.IsPaid = true;


		//						nested.PreviewPdf = WorkSheet?.UploadPreviewPdf;

		//						nested.IsEnabledForDetailsPage = WorkSheet?.IsEnableForDetailsPage ?? false;

		//						nested.WorksheetId = WorkSheet?.Id ?? 0;

		//						//nested.WorksheetDetailsUrl = WorkSheet?.Url();

		//						if (!String.IsNullOrEmpty(WorkSheet.Value<string>("umbracoUrlAlias")))

		//							nested.WorksheetDetailsUrl = WorkSheet.Value<string>("umbracoUrlAlias");

		//						else

		//							nested.WorksheetDetailsUrl = WorkSheet?.Url();

		//						string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet?.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");
		//						//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + "Paid";

		//						nested.subscriptionStatus = new SubscriptionStatus();

		//						string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");
		//						//string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + "Paid";

		//						nested.subscriptionStatus.DownloadString = DownloadString;

		//						nested.subscriptionStatus.DownloadUrl = downloadUrl;

		//						if (Image != null)

		//						{

		//							if (NextGenImage != null)
		//							{

		//								nested.NextGenImage = NextGenImage.Url();

		//							}

		//							nested.AltText = altText;

		//							nested.ImagesSrc = Image.Url();

		//						}

		//						nested.Description = WorkSheet?.Description;

		//						if (RecentlyDownloadedBonusWorkSheet != null && RecentlyDownloadedBonusWorkSheet.Count != 0)

		//						{

		//							nested.RecentlyDownloaded = RecentlyDownloadedBonusWorkSheet.Contains(WorkSheet.Id);

		//						}

		//						if (!String.IsNullOrEmpty(WorkSheet.FacebookContent) || !String.IsNullOrEmpty(WorkSheet.WhatsAppContent) || !String.IsNullOrEmpty(WorkSheet.MailContent))

		//						{

		//							var commonContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?

		//											  .Where(x => x.ContentType.Alias == "commonContent").OfType<CommonContent>().FirstOrDefault();

		//							string referralText = string.Empty;

		//							if (loggedin != null)

		//							{

		//								if (!String.IsNullOrWhiteSpace(commonContent.ShareReferralText.ToString()) && !String.IsNullOrWhiteSpace(loggedin.ReferralCode))

		//								{

		//									referralText = commonContent.ShareReferralText.ToString().ToString().Replace("<p>", "").Replace("</p>", "");

		//									referralText = referralText.Replace("{referralcode}", loggedin.ReferralCode);

		//								}

		//							}

		//							if (!String.IsNullOrEmpty(WorkSheet.FacebookContent))

		//							{

		//								if (!String.IsNullOrEmpty(referralText))

		//									social.FBShare = WorkSheet.FacebookContent + "\n" + referralText;

		//								else

		//									social.FBShare = WorkSheet.FacebookContent;

		//							}

		//							if (!String.IsNullOrEmpty(WorkSheet.WhatsAppContent))

		//							{

		//								if (!String.IsNullOrEmpty(referralText))

		//									social.WhatAppShare = WorkSheet.WhatsAppContent + "\n" + referralText;

		//								else

		//									social.WhatAppShare = WorkSheet.WhatsAppContent;

		//							}

		//							if (!String.IsNullOrEmpty(WorkSheet.MailContent))

		//							{

		//								social.EmailShare = WorkSheet.MailContent + "`" + WorkSheet?.Title;

		//							}

		//						}

		//						nested.socialItems = social;

		//						nested.IsWorkSheet = true;

		//						items.NestedItems.Add(nested);

		//					}
		//					//);
		//					//items.NestedItems = NestedItems;

		//					if (VideoRoot != null && (!string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup) || !string.IsNullOrEmpty(input.VideosInput?.selectedCategory) || !string.IsNullOrEmpty(input.searchText) || !string.IsNullOrEmpty(input.selectedPaid)) && input.selectedPaid == "2" && string.IsNullOrEmpty(input.selectedTopics))

		//					{

		//						var mediaUrl = VideoRoot?.Value<IPublishedContent>("seeMoreMedia");

		//						var nextGenVideoMediaUrl = VideoRoot?.SeeMoreNextGen;

		//						int firstTimeDisplayVideos = VideoRoot.FirstTimeDisplayVideos;

		//						var RecentlyDownloadedVideoBonusWorkSheet = await GetRecentlyDownloadedBonusWorkSheet(RefUserId, "BonusVideoWorkSheet");
		//						try

		//						{

		//							VideosItems videosItems = new VideosItems();

		//							var Videos = VideoRoot?.DescendantsOrSelf()?.Where(x => x.Parent?.ContentType.Alias == "videoListingAgeWise"

		//							)?.OfType<Video>().Where(x => x.IsActive);

		//							if (!string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup))

		//							{

		//								seletedAgeGroups = agemaster.Where(x => input.VideosInput.selectedAgeGroup.Split(',').Contains(x.ItemValue)).Select(x => x.Name).ToList();

		//								if (seletedAgeGroups != null && seletedAgeGroups.Any())

		//								{

		//									Videos = Videos.Where(x => seletedAgeGroups.Contains(x.AgeTitle?.Name)).ToList();

		//								}

		//							}

		//							if (!string.IsNullOrEmpty(input.VideosInput?.selectedCategory))

		//							{

		//								seletedSubjects = subjectmaster.Where(x => input.VideosInput.selectedCategory.Split(',').Contains(x.SubjectName)).Select(x => x.Name).ToList();

		//								if (seletedSubjects != null && seletedSubjects.Any())

		//								{

		//									Videos = Videos.Where(item => item.Category.Any(x => seletedSubjects.Contains(x.Name))).ToList();

		//								}

		//							}

		//							if (!string.IsNullOrEmpty(input.searchText) && agemaster != null && agemaster.Any())

		//							{

		//								seletedAgeGroups = agemaster.Where(x => x.AlternateItemName.ToLower().ContainsAny(input.searchText.ToLower().Split(' '))).Select(x => x.Name).ToList();

		//								if (seletedAgeGroups != null && seletedAgeGroups.Any())

		//								{

		//									Videos = Videos.Where(x => seletedAgeGroups.Contains(x.AgeTitle?.Name)).ToList();

		//								}

		//							}

		//							if (!string.IsNullOrEmpty(input.searchText) && subjectmaster != null && subjectmaster.Any())

		//							{

		//								seletedSubjects = subjectmaster.Where(x => x.SubjectName.ToLower().ContainsAny(input.searchText.ToLower().Split(' '))).Select(x => x.Name).ToList();

		//								if (seletedSubjects != null && seletedSubjects.Any())

		//								{

		//									Videos = Videos.Where(item => item.Category.Any(x => seletedSubjects.Contains(x.Name))).ToList();

		//								}

		//							}

		//							if (!string.IsNullOrEmpty(input.searchText))

		//							{

		//								Videos = Videos.Where(item => item.Title.ToLower().ContainsAny(input.searchText.Split(' ')) || item.Description.ToHtmlString().ToLower().ContainsAny(input.searchText.Split(' '))).ToList();

		//							}

		//							if (Videos != null && Videos.Any())

		//							{

		//								List<VideosNestedItems> nesteds = new List<VideosNestedItems>();

		//								string videoPageURL = string.Empty;

		//								VideosNestedItems nested = new VideosNestedItems();

		//								//Parallel.ForEach(Videos, async videoItems =>
		//								foreach (var videoItems in Videos)
		//								{

		//									nested = await GetVideos(videoItems, input.VideosInput);

		//									videoPageURL = videoItems.Parent.Url().ToString();

		//									nested.IsWorkSheet = false;

		//									if (RecentlyDownloadedVideoBonusWorkSheet != null && RecentlyDownloadedVideoBonusWorkSheet.Count != 0)
		//									{
		//										nested.RecentlyDownloaded = RecentlyDownloadedVideoBonusWorkSheet.Contains(videoItems.Id);
		//									}


		//									items.NestedItems.Add(nested);

		//								}
		//								//);

		//								//videosItems.NestedItems = nesteds;

		//								//See More

		//								VideosSeeMore seeMore = new VideosSeeMore();

		//								seeMore.VideoDetailsUrl = videoPageURL;

		//								if (mediaUrl != null)

		//								{

		//									if (nextGenMediaUrl != null)

		//										seeMore.NextGenMediaUrl = nextGenVideoMediaUrl.Url();

		//									seeMore.MediaUrl = mediaUrl.Url();

		//								}

		//								videosItems.SeeMore = seeMore;

		//							}

		//						}

		//						catch (Exception ex)
		//						{
		//							Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetVideosListData");
		//						}

		//					}

		//					else if (VideoRoot != null && (string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup) || string.IsNullOrEmpty(input.VideosInput?.selectedCategory))

		//						&& string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText) && string.IsNullOrEmpty(input.selectedTopics))

		//					{

		//						var mediaUrl = VideoRoot?.Value<IPublishedContent>("seeMoreMedia");

		//						var nextGenVideoMediaUrl = VideoRoot?.SeeMoreNextGen;

		//						int firstTimeDisplayVideos = VideoRoot.FirstTimeDisplayVideos;

		//						var RecentlyDownloadedVideoBonusWorkSheet = await GetRecentlyDownloadedBonusWorkSheet(RefUserId, "BonusVideoWorkSheet");

		//						try
		//						{

		//							VideosItems videosItems = new VideosItems();

		//							var Videos = VideoRoot?.DescendantsOrSelf()?.Where(x => x.Parent?.ContentType.Alias == "videoListingAgeWise"

		//							)?.OfType<Video>().Where(x => x.IsActive);

		//							if (Videos != null && Videos.Any())

		//							{

		//								List<VideosNestedItems> nesteds = new List<VideosNestedItems>();

		//								string videoPageURL = string.Empty;

		//								VideosNestedItems nested = new VideosNestedItems();

		//								var random = new Random();

		//								int index = random.Next(Videos.Count());

		//								Video videoItems = Videos.ElementAt(index);

		//								nested = await GetVideos(videoItems, input.VideosInput);

		//								videoPageURL = videoItems.Parent.Url().ToString();

		//								nested.IsWorkSheet = false;

		//								if (RecentlyDownloadedVideoBonusWorkSheet != null && RecentlyDownloadedVideoBonusWorkSheet.Count != 0)
		//								{
		//									nested.RecentlyDownloaded = RecentlyDownloadedVideoBonusWorkSheet.Contains(videoItems.Id);
		//								}


		//								if (items.NestedItems.Count > (worksheetRoot.DefautPositionOfVideoInPage - 1))

		//								{

		//									items.NestedItems.Insert(worksheetRoot.DefautPositionOfVideoInPage, nested);

		//								}

		//								else

		//								{

		//									items.NestedItems.Add(nested);

		//								}

		//								//videosItems.NestedItems = nesteds;

		//								//See More

		//								VideosSeeMore seeMore = new VideosSeeMore();

		//								seeMore.VideoDetailsUrl = videoPageURL;

		//								if (mediaUrl != null)

		//								{

		//									if (nextGenMediaUrl != null)

		//										seeMore.NextGenMediaUrl = nextGenVideoMediaUrl.Url();

		//									seeMore.MediaUrl = mediaUrl.Url();

		//								}

		//								videosItems.SeeMore = seeMore;

		//							}

		//						}

		//						catch (Exception ex)

		//						{

		//							Logger.Error(reporting: typeof(VideoController), ex, message: "GetVideosListData");

		//						}

		//					}

		//					if ((!string.IsNullOrEmpty(input.selectedAgeGroup) || !string.IsNullOrEmpty(input.selectedSubject) || !string.IsNullOrEmpty(input.VideosInput?.selectedAgeGroup) || !string.IsNullOrEmpty(input.VideosInput?.selectedCategory)) && string.IsNullOrEmpty(input.searchText))

		//					{

		//						items.totalItems = items.NestedItems.Count;

		//						items.NestedItems = items.NestedItems.Skip((input.currentPage - 1) * (worksheetRoot.NoOfDisplayWorksheet + 1)).Take(worksheetRoot.NoOfDisplayWorksheet + 1).ToList();

		//					}

		//				}

		//				catch { }

		//				ReturnList.Add(items);

		//			}

		//		}

		//	}

		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");
		//	}

		//	return ReturnList;

		//}



		private List<AutoCompleteList> GetStructuredProgramAutoCompleteListData(WorksheetInput input)
		{

			List<AutoCompleteList> model = new List<AutoCompleteList>();

			try

			{

				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

				var worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?

									.Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();

				if (worksheetRoot != null)

				{

					try

					{

						//var searcresult = querySearchIndex(input);

						//if (searcresult.Any())

						//{

						//    foreach (var result in searcresult)

						//    {

						//        if (result.Id != null)

						//        {

						//            var node = Umbraco.Content(result.Id);

						//        }

						//    }

						//}

						var WorkSheets = worksheetRoot?.DescendantsOrSelf()?

									.Where(x => x.ContentType.Alias == "structureProgramItems")

									.OfType<StructureProgramItems>().ToList();



						var FilteredWorkSheets = new List<StructureProgramItems>();

						var TopicsWorkSheets = new List<StructureProgramItems>();

						var SubjectsWorkSheets = new List<StructureProgramItems>();

						var ClassWorkSheets = new List<StructureProgramItems>();

						var TitleDesciptionWorkSheets = new List<StructureProgramItems>();

						if (WorkSheets != null && WorkSheets.Any())
						{
							List<string> subjectsfilterAllId = null;

							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

							WorkSheetVideoItems items = new WorkSheetVideoItems();

							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

							List<WorksheetCategory> subjects = new List<WorksheetCategory>();

							List<TopicsName> topics = new List<TopicsName>();


							List<string> seletedAgeGroups = new List<string>();

							List<string> seletedSubjects = new List<string>();

							List<string> seletedTopics = new List<string>();
							List<NameListItem> agemaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
											.Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?.Children?
											.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?
											.OfType<NameListItem>().Where(x => x.IsActice).ToList();

							List<Subjects> subjectmaster = worksheetRoot?.Children?
														 .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?
														 .OfType<Subjects>().ToList();

							List<Topics> topicmaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>()?.Where(x => x.IsActive == true)?.OrderBy(x => x.TopicName).ToList();
							FilteredWorkSheets = WorkSheets;

							if (!string.IsNullOrEmpty(input.selectedAgeGroup) && agemaster != null && agemaster.Any())
							{
								seletedAgeGroups = agemaster.Where(x => input.selectedAgeGroup.Split(',').Contains(x.ItemValue)).Select(x => x.Name).ToList();

								if (seletedAgeGroups != null && seletedAgeGroups.Any())
								{
									FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectAgeGroup.Any(x => seletedAgeGroups.Contains(x.Name))).ToList();
								}

							}

							if (!string.IsNullOrEmpty(input.selectedSubject) && subjectmaster != null && subjectmaster.Any())
							{

								//seletedSubjects = subjectmaster.Where(x => input.selectedSubject.Split(',').Contains(x.SubjectName)).Select(x => x.Name).ToList();

								seletedSubjects = subjectmaster.Where(x => input.selectedSubject.Split(',').Contains(x.SubjectValue.ToString())).Select(x => x.Name).ToList();

								if (seletedSubjects != null && seletedSubjects.Any())

								{

									FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectSubject.Any(x => seletedSubjects.Contains(x.Name))).ToList();

								}

							}

							if (!string.IsNullOrEmpty(input.selectedTopics) && topicmaster != null && topicmaster.Any())

							{

								seletedTopics = topicmaster.Where(x => input.selectedTopics.Split(',').Contains(x.TopicValue.ToString())).Select(x => x.Name).ToList();

								if (seletedTopics != null && seletedTopics.Any())

								{

									FilteredWorkSheets = FilteredWorkSheets.Where(item => item.SelectTopic.Any(x => seletedTopics.Contains(x.Name))).ToList();

								}

							}

							if (!string.IsNullOrEmpty(input.selectedPaid))

							{

								FilteredWorkSheets = FilteredWorkSheets.Where(x => input.selectedPaid.Split(',').Contains(Convert.ToString(Convert.ToInt16(x.IsPaid)))).ToList();

							}
							//if (!string.IsNullOrEmpty(input.searchText))
							//{
							//    var TitleWorkSheets = WorkSheets.AsQueryable().WhereLike(x => x.Title.ToLower(), input.searchText.ToLower().Replace(" ", "%") + "%", '%').ToList();
							//    var DesciptionWorkSheets = WorkSheets.AsQueryable().WhereLike(x => x.Description.ToHtmlString().ToLower().Replace(" ", "%"), input.searchText.ToLower() + "%", '%').ToList();
							//    if (TitleWorkSheets != null && TitleWorkSheets.Any() && DesciptionWorkSheets != null && DesciptionWorkSheets.Any())
							//    {
							//        TitleDesciptionWorkSheets = TitleWorkSheets.Union(DesciptionWorkSheets).ToList();
							//    }
							//    else
							//    if (TitleWorkSheets != null && TitleWorkSheets.Any())
							//    {
							//        TitleDesciptionWorkSheets = TitleWorkSheets;
							//    }
							//    else
							//    if (DesciptionWorkSheets != null && DesciptionWorkSheets.Any())
							//    {
							//        TitleDesciptionWorkSheets = DesciptionWorkSheets;
							//    }
							//}



							var selectedclass = string.IsNullOrEmpty(input.searchText) ? null : agemaster?.Where(x => input.searchText.ToLower().Contains(x.AlternateClassName.ToLower()))?.FirstOrDefault();
							var classsearchstr = selectedclass == null ? "" : selectedclass?.AlternateClassName;
							int placement = string.IsNullOrEmpty(input.searchText) ? -1 : input.searchText.IndexOf(classsearchstr, StringComparison.OrdinalIgnoreCase);

							//var classsearchstr = placement > -1 ? (placement == 0 ? input.searchText.Substring(placement, 8) : input.searchText.Substring(placement - 1, 8)) : "";
							var searchstr = !string.IsNullOrEmpty(input.searchText) ? (input.searchText.Contains(classsearchstr.ToLower()) && !string.IsNullOrEmpty(classsearchstr) ? input.searchText.ToLower().Replace(classsearchstr.ToLower(), string.Empty) : input.searchText) : "";
							List<string> searchText = string.IsNullOrEmpty(searchstr) || input.searchText.ToLower().Equals(classsearchstr.ToLower()) ? new List<string>() : searchstr.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

							if (searchText != null && !string.IsNullOrEmpty(classsearchstr))
							{
								searchText.Add(classsearchstr.Trim());
							}

							if (searchText != null && searchText.Any())
							{
								var TitleWorkSheets = new List<StructureProgramItems>();
								var DesciptionWorkSheets = new List<StructureProgramItems>();
								foreach (var search in searchText)
								{
									var titleWorkSheet = FilteredWorkSheets.Where(x => x.Title.ToLower().StartsWith(search.ToLower())).ToList();
									var desciptionWorkSheet = FilteredWorkSheets.Where(x => x.Description.ToHtmlString().ToLower().StartsWith(search.ToLower())).ToList();
									TitleWorkSheets = TitleWorkSheets.Concat(titleWorkSheet).ToList();
									DesciptionWorkSheets = DesciptionWorkSheets.Concat(desciptionWorkSheet).ToList();
								}

								TitleDesciptionWorkSheets = TitleWorkSheets.Union(DesciptionWorkSheets).DistinctBy(x => x.Id).ToList();
							}

							if (searchText != null && searchText.Any() && agemaster != null && agemaster.Any())
							{

								foreach (var search in searchText)
								{
									var age = agemaster.Where(x => x.AlternateItemName.ToLower().StartsWith(search.ToLower())).ToList();
									seletedAgeGroups = age.Select(x => x.Name).ToList();
								}

								if (seletedAgeGroups != null && seletedAgeGroups.Any())

								{

									ClassWorkSheets = FilteredWorkSheets.Where(item => item.SelectAgeGroup.Any(x => seletedAgeGroups.Contains(x.Name))).ToList();

								}

							}

							if (searchText != null && searchText.Any() && subjectmaster != null && subjectmaster.Any())
							{
								foreach (var search in searchText)
								{
									var subject = subjectmaster.Where(x => x.SubjectName.ToLower().StartsWith(search.ToLower())).ToList();
									seletedSubjects = subject.Select(x => x.Name).ToList();
								}

								if (seletedSubjects != null && seletedSubjects.Any())
								{

									SubjectsWorkSheets = FilteredWorkSheets.Where(item => item.SelectSubject.Any(x => seletedSubjects.Contains(x.Name))).ToList();

								}

							}

							if (searchText != null && searchText.Any() && topicmaster != null && topicmaster.Any())
							{
								foreach (var search in searchText)
								{
									var topic = topicmaster.Where(x => x.TopicName.ToLower().StartsWith(search.ToLower())).ToList();
									seletedTopics = topic.Select(x => x.TopicName).ToList();
								}

								if (seletedTopics != null && seletedTopics.Any())
								{

									TopicsWorkSheets = FilteredWorkSheets.Where(item => item.SelectTopic.Any(x => seletedTopics.Contains(x.Name))).ToList();

								}

							}


							//if (!string.IsNullOrEmpty(input.searchText) && agemaster != null && agemaster.Any())

							//{

							//    seletedAgeGroups = agemaster.AsQueryable().WhereLike(x => x.AlternateClassName.ToLower(), input.searchText.ToLower().Replace(" ", "%") + "%", '%').Select(x => x.Name).ToList();

							//    if (seletedAgeGroups != null && seletedAgeGroups.Any())

							//    {

							//        ClassWorkSheets = WorkSheets.Where(item => item.SelectAgeGroup.Any(x => seletedAgeGroups.Contains(x.Name))).ToList();

							//    }

							//}

							//if (!string.IsNullOrEmpty(input.searchText) && subjectmaster != null && subjectmaster.Any())

							//{

							//    seletedSubjects = subjectmaster.AsQueryable().WhereLike(x => x.SubjectName.ToLower(), input.searchText.ToLower().Replace(" ", "%") + "%", '%').Select(x => x.Name).ToList();

							//    if (seletedSubjects != null && seletedSubjects.Any())

							//    {

							//        SubjectsWorkSheets = WorkSheets.Where(item => item.SelectSubject.Any(x => seletedSubjects.Contains(x.Name))).ToList();

							//    }

							//}

							//if (!string.IsNullOrEmpty(input.searchText) && topicmaster != null && topicmaster.Any())

							//{

							//    seletedTopics = topicmaster.AsQueryable().WhereLike(x => x.TopicName.ToLower(), input.searchText.ToLower().Replace(" ", "%") + "%", '%').Select(x => x.Name).ToList();

							//    if (seletedTopics != null && seletedTopics.Any())

							//    {

							//        TopicsWorkSheets = WorkSheets.Where(item => item.SelectTopic.Any(x => seletedTopics.Contains(x.Name))).ToList();

							//    }

							//}

							//if (TopicsWorkSheets != null && TopicsWorkSheets.Any() && SubjectsWorkSheets != null && SubjectsWorkSheets.Any() && TitleDesciptionWorkSheets != null && TitleDesciptionWorkSheets.Any() && ClassWorkSheets != null && ClassWorkSheets.Any())

							//{

							FilteredWorkSheets = TopicsWorkSheets.Union(SubjectsWorkSheets).Union(ClassWorkSheets).Union(TitleDesciptionWorkSheets).ToList();

							FilteredWorkSheets = FilteredWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							//else

							//if (TopicsWorkSheets != null && TopicsWorkSheets.Any() && SubjectsWorkSheets != null && SubjectsWorkSheets.Any() && TitleDesciptionWorkSheets != null && TitleDesciptionWorkSheets.Any())

							//{

							//    FilteredWorkSheets = TopicsWorkSheets.Union(SubjectsWorkSheets).Union(TitleDesciptionWorkSheets).ToList();

							//    FilteredWorkSheets = FilteredWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							//else

							//if (TopicsWorkSheets != null && TopicsWorkSheets.Any() && SubjectsWorkSheets != null && SubjectsWorkSheets.Any() && ClassWorkSheets != null && ClassWorkSheets.Any())

							//{

							//    FilteredWorkSheets = TopicsWorkSheets.Union(SubjectsWorkSheets).Union(ClassWorkSheets).ToList();

							//    FilteredWorkSheets = FilteredWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							//else if (SubjectsWorkSheets != null && SubjectsWorkSheets.Any() && TitleDesciptionWorkSheets != null && TitleDesciptionWorkSheets.Any() && ClassWorkSheets != null && ClassWorkSheets.Any())

							//{

							//    FilteredWorkSheets = SubjectsWorkSheets.Union(ClassWorkSheets).Union(TitleDesciptionWorkSheets).ToList();

							//    FilteredWorkSheets = FilteredWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							//else if (TopicsWorkSheets != null && TopicsWorkSheets.Any() && TitleDesciptionWorkSheets != null && TitleDesciptionWorkSheets.Any() && ClassWorkSheets != null && ClassWorkSheets.Any())

							//{

							//    FilteredWorkSheets = TopicsWorkSheets.Union(ClassWorkSheets).Union(TitleDesciptionWorkSheets).ToList();

							//    FilteredWorkSheets = FilteredWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							//else if (TopicsWorkSheets != null && TopicsWorkSheets.Any())

							//{

							//    FilteredWorkSheets = TopicsWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							//else if (TitleDesciptionWorkSheets != null && TitleDesciptionWorkSheets.Any())

							//{

							//    FilteredWorkSheets = TitleDesciptionWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							//else if (SubjectsWorkSheets != null && SubjectsWorkSheets.Any())

							//{

							//    FilteredWorkSheets = SubjectsWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							//else if (ClassWorkSheets != null && ClassWorkSheets.Any())

							//{

							//    FilteredWorkSheets = ClassWorkSheets.DistinctBy(x => x.Id).ToList();

							//}

							FilteredWorkSheets = FilteredWorkSheets.Take(worksheetRoot.NoOfSearchWorksheetInList).ToList();

							foreach (var WorkSheet in FilteredWorkSheets)
							{
								var SelectedAgeUdi = WorkSheet?.SelectAgeGroup?.Select(x => x.Udi);

								var SelectedSubjectUdi = WorkSheet.SelectSubject.Select(x => x.Udi);

								var selectedSubjectContent = Umbraco.Content(SelectedSubjectUdi);

								var SelectedSubjectValues = selectedSubjectContent.Where(x => input.searchText.ToLower().StartsWith(x.Name.ToLower())).Select(x => x.Value<string>("subjectName")).ToList();

								if (SelectedSubjectValues == null || SelectedSubjectValues.Count == 0)
								{
									SelectedSubjectValues = selectedSubjectContent.Select(x => x.Value<string>("subjectName")).ToList();
								}

								//var SelectedTopic = WorkSheet.SelectTopic.Select(x => x.Udi);

								var SelectedAgeContent = Umbraco.Content(SelectedAgeUdi);

								var SelectedAgeValues = SelectedAgeContent.Where(x => input.searchText.ToLower().StartsWith(x.Name.ToLower())).Select(x => x.Value<string>("alternateClassName")).ToList();

								if (SelectedAgeValues == null || SelectedAgeValues.Count == 0)
								{
									SelectedAgeValues = SelectedAgeContent.Select(x => x.Value<string>("alternateClassName")).ToList();
								}

								//var SelectedTopicValues = selectedTopicContent.Select(x => x.Value<string>("topicName")).ToList();

								//if (SelectedSubjectValues.Contains(input.searchText) || SelectedTopicValues.Contains(input.searchText))

								//{

								AutoCompleteList autoComplete = new AutoCompleteList();

								autoComplete.WorkSheetTitle = WorkSheet.Title;

								autoComplete.SubjectTitle = string.IsNullOrEmpty(SelectedSubjectValues.FirstOrDefault()) ? string.Empty : SelectedSubjectValues.FirstOrDefault();
								autoComplete.ClassTitle = string.IsNullOrEmpty(SelectedAgeValues.FirstOrDefault()) ? string.Empty : SelectedAgeValues.FirstOrDefault();

								autoComplete.WorkSheetId = WorkSheet.Id;

								model.Add(autoComplete);

								//}

							}

							if (model != null)
							{

								model = model.DistinctBy(x => x.WorkSheetId).OrderBy(x => x.WorkSheetTitle).OrderBy(x => x.SubjectTitle).ToList();

							}

						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetStructuredProgramListData - sub block");
					}

				}

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetStructuredProgramListData");
			}

			return model;

		}

		private ISearchResults querySearchIndex(WorksheetInput input)
		{
			if (!ExamineManager.Instance.TryGetIndex(Constants.UmbracoIndexes.ExternalIndexName, out var index))
			{
				throw new InvalidOperationException($"No index found with name {Constants.UmbracoIndexes.ExternalIndexName}");
			}
			_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

			ISearcher searcher = index.GetSearcher();
			var results = searcher.CreateQuery("content").Field($"title_{input.CultureInfo.ToLower()}", input.searchText).Or().Field($"description_{input.CultureInfo.ToLower()}", input.searchText).Or().Field($"alternateItemName_{input.CultureInfo.ToLower()}", input.searchText).Execute();
			//if (results.Any())
			//{

			//    foreach (var result in results)
			//    {
			//        if (result.Id != null)
			//        {
			//            var node = Umbraco.Content(result.Id);

			//        }
			//    }
			//}
			return results;
			//IQuery query = searcher.CreateQuery("content", BooleanOperation.And);
			//string searchFields = $"title, description";
			//IBooleanOperation terms = query.GroupedOr(searchFields.Split(','), input.searchText);
			//return terms.Execute();
		}

		[HttpPost]
		public ActionResult DownloadEligibility(BonusDownloadParam Input)
		{
			BonusWorksheetDownloadEligibility bonusWorksheetDownloadEligibility = new BonusWorksheetDownloadEligibility();
			dbAccessClass dbAccessClass = new dbAccessClass();

			bonusWorksheetDownloadEligibility = dbAccessClass.DownloadEligibilityData(Input);

			return Json(bonusWorksheetDownloadEligibility, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public async Task<ActionResult> GetRelatedStructuredProgramDetails(WorksheetInput inputRelated)
		{

			Responce responce = new Responce();

			try
			{

				WorkSheetVideoModel model = new WorkSheetVideoModel();

				model = await GetRelatedStructuredProgramListData(inputRelated);

				model.Mode = inputRelated?.Mode;

				return PartialView("/Views/Partials/Worksheets/_relatedStructuredProgramList.cshtml", model);

			}
			catch (Exception ex)
			{

				responce.StatusCode = HttpStatusCode.InternalServerError;

				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetWorksheetList");

			}

			return Json(responce, JsonRequestBehavior.AllowGet);

		}

		private async Task<WorkSheetVideoModel> GetRelatedStructuredProgramListData(WorksheetInput input)

		{

			WorkSheetVideoModel model = new WorkSheetVideoModel();

			try

			{

				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

				var worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?

									.Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();

				if (worksheetRoot != null)

				{

					try

					{

						var WorkSheets = await GetRelatedFilterData(worksheetRoot, input);

						model.WorkSheets = WorkSheets;

					}

					catch (Exception ex)

					{

						Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetStructuredProgramListData - sub block");

					}

				}

			}

			catch (Exception ex)

			{

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetStructuredProgramListData");

			}

			return model;

		}

		private async Task<List<WorkSheetVideoItems>> GetRelatedFilterData(StructureProgramRoot worksheetRoot, WorksheetInput input)
		{
			List<WorkSheetVideoItems> ReturnList = new List<WorkSheetVideoItems>();

			SocialItems social = new SocialItems();

			LoggedIn loggedin = new LoggedIn();

			loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

			int RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

			try
			{

				string bitlyLink = String.Empty;

				string culture = CultureName.GetCultureName().Replace("/", "");

				if (String.IsNullOrEmpty(culture))

					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

				var nextGenMediaUrl = worksheetRoot?.Value<IPublishedContent>("seeMoreNextGen");

				if (worksheetRoot != null)

				{

					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

					var selectedWorkSheet = Umbraco.Content(input.worksheetId);

					List<StructureProgramItems> WorkSheets = new List<StructureProgramItems>();

					//if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbytopic")
					//{
					//	_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
					//	var siblingworksheets = worksheetRoot?.DescendantsOrSelf()?
					//					.Where(x => x.ContentType.Alias == "structureProgramItems")
					//					.OfType<StructureProgramItems>().ToList();
					//	var getcurrentworksheetindex = siblingworksheets?.IndexOf(selectedWorkSheet) ?? 0;
					//	var nextworksheets = siblingworksheets?.Skip(getcurrentworksheetindex + 1).Take(worksheetRoot.NoOfRelatedContentWorksheets).OfType<StructureProgramItems>().ToList();
					//	WorkSheets = nextworksheets;
					//}
					//if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbysubject")
					//{
					//	_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
					//	var SelectedSubject = selectedWorkSheet.Parent;
					//	var siblingWorksheetsCategory = worksheetRoot?.DescendantsOrSelf()?
					//					.Where(x => x.ContentType.Alias == "worksheetCategory")
					//					.OfType<WorksheetCategory>().ToList();
					//	//var siblingsubject = SelectedSubject?.Siblings();
					//	var getcurrentsubjectindex = siblingWorksheetsCategory?.IndexOf(SelectedSubject) ?? 0;
					//	var nextworksheetsubjects = siblingWorksheetsCategory?.Where(z => z.FirstChild<StructureProgramItems>() != null)?.Select(x => x.FirstChild<StructureProgramItems>())?.Skip(getcurrentsubjectindex).Take(worksheetRoot.NoOfRelatedContentWorksheets).OfType<StructureProgramItems>().ToList();
					//	WorkSheets = nextworksheetsubjects;
					//	//.Select(x => x.FirstChild<StructureProgramItems>()).ToList();
					//}
					//IEnumerable<StructureProgramItems> WorksheetItem = new List<StructureProgramItems>();
					//if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbytopic" && WorkSheets.Count < worksheetRoot.NoOfRelatedContentWorksheets)
					//{
					//	_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
					//	var siblingworksheets = worksheetRoot?.DescendantsOrSelf()?
					//					.Where(x => x.ContentType.Alias == "structureProgramItems")
					//					.OfType<StructureProgramItems>().ToList();
					//	var nextleftworksheet = worksheetRoot.NoOfRelatedContentWorksheets - WorkSheets.Count;
					//	var nextworksheets = siblingworksheets?.Take(nextleftworksheet).OfType<StructureProgramItems>().ToList();
					//	WorkSheets = WorkSheets.Union(nextworksheets).ToList();
					//}
					//if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbysubject" && WorkSheets.Count < worksheetRoot.NoOfRelatedContentWorksheets)
					//{
					//	_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
					//	var SelectedSubject = selectedWorkSheet.Parent;
					//	var siblingWorksheetsCategory = worksheetRoot?.DescendantsOrSelf()?
					//					.Where(x => x.ContentType.Alias == "worksheetCategory")
					//					.OfType<WorksheetCategory>().ToList();
					//	//var siblingsubject = SelectedSubject?.Siblings();
					//	var nextleftworksheet = worksheetRoot.NoOfRelatedContentWorksheets - WorkSheets.Count;
					//	var nextworksheetsubjects = siblingWorksheetsCategory?.Where(z => z.FirstChild<StructureProgramItems>() != null)?.Select(x => x.FirstChild<StructureProgramItems>())?.Take(nextleftworksheet).OfType<StructureProgramItems>().ToList();
					//	WorkSheets = WorkSheets.Union(nextworksheetsubjects).ToList();
					//}

					if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbytopic")
					{
						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
						var siblingworksheets = worksheetRoot?.DescendantsOrSelf()?
										.Where(x => x.ContentType.Alias == "structureProgramItems" && x.Value<bool>("isActive") == true)
										.OfType<StructureProgramItems>().ToList();
						var getcurrentworksheetindex = siblingworksheets?.IndexOf(selectedWorkSheet) ?? 0;
						var nextworksheets = siblingworksheets?.Skip(getcurrentworksheetindex + 1).Take(worksheetRoot.NoOfRelatedContentWorksheets).OfType<StructureProgramItems>().ToList();
						WorkSheets = nextworksheets;
					}
					if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbysubject")
					{
						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
						var SelectedSubject = selectedWorkSheet.Parent;
						var siblingWorksheetsCategory = worksheetRoot?.DescendantsOrSelf()?
										.Where(x => x.ContentType.Alias == "worksheetCategory" && x.Value<bool>("isActive") == true && x.Children.Any())
										.OfType<WorksheetCategory>().ToList();
						//var siblingsubject = SelectedSubject?.Siblings();
						var getcurrentsubjectindex = siblingWorksheetsCategory?.IndexOf(SelectedSubject) ?? 0;
						var nextworksheetsubjects = siblingWorksheetsCategory?.Where(z => z.FirstChild<StructureProgramItems>() != null)?.Select(x => x.FirstChild<StructureProgramItems>())?.Skip(getcurrentsubjectindex + 1).Take(worksheetRoot.NoOfRelatedContentWorksheets).OfType<StructureProgramItems>().ToList();
						//var checkcurrentsubject = nextworksheetsubjects.FindIndex
						WorkSheets = nextworksheetsubjects;
						//.Select(x => x.FirstChild<StructureProgramItems>()).ToList();
					}
					//  IEnumerable<StructureProgramItems> WorksheetItem = new List<StructureProgramItems>();
					if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbytopic" && WorkSheets.Count < worksheetRoot.NoOfRelatedContentWorksheets)
					{
						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
						var siblingworksheets = worksheetRoot?.DescendantsOrSelf()?
										.Where(x => x.ContentType.Alias == "structureProgramItems" && x.Value<bool>("isActive") == true)
										.OfType<StructureProgramItems>().ToList();
						var nextleftworksheet = worksheetRoot.NoOfRelatedContentWorksheets - WorkSheets.Count;
						var nextworksheets = siblingworksheets?.Take(nextleftworksheet).OfType<StructureProgramItems>().ToList();
						WorkSheets = WorkSheets.Union(nextworksheets).ToList();
					}
					if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbysubject" && WorkSheets.Count < worksheetRoot.NoOfRelatedContentWorksheets)
					{
						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
						var SelectedSubject = selectedWorkSheet.Parent;
						var siblingWorksheetsCategory = worksheetRoot?.DescendantsOrSelf()?
										.Where(x => x.ContentType.Alias == "worksheetCategory" && x.Value<bool>("isActive") == true && x.Children.Any())
										.OfType<WorksheetCategory>().ToList();
						//var siblingsubject = SelectedSubject?.Siblings();
						var nextleftworksheet = worksheetRoot.NoOfRelatedContentWorksheets - WorkSheets.Count;
						var nextworksheetsubjects = siblingWorksheetsCategory?.Where(z => z.FirstChild<StructureProgramItems>() != null)?.Select(x => x.FirstChild<StructureProgramItems>())?.Take(nextleftworksheet).OfType<StructureProgramItems>().ToList();
						WorkSheets = WorkSheets.Union(nextworksheetsubjects).ToList();
					}
					//var WorkSheets = worksheetRoot?.DescendantsOrSelf()?
					//                .Where(x => x.ContentType.Alias == "structureProgramItems")
					//                .OfType<StructureProgramItems>().ToList();

					//var FilteredWorkSheets = WorkSheets;

					if (WorkSheets != null)
					{

						WorkSheetVideoItems items = new WorkSheetVideoItems();

						try

						{

							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

							List<WorksheetCategory> subjects = new List<WorksheetCategory>();

							List<TopicsName> topics = new List<TopicsName>();

							items.NestedItems = new List<dynamic>();
							var RecentlyDownloadedBonusWorkSheet = await GetRecentlyDownloadedBonusWorkSheet(RefUserId, "BonusWorkSheet");
							//var WorkSheetsPaged = WorkSheets;

							//Parallel.ForEach(WorkSheetsPaged, async WorkSheet =>
							foreach (var WorkSheet in WorkSheets)
							{
								if (WorkSheet != null)
								{
									var SelectedAgeValues = WorkSheet?.SelectAgeGroup?.Select(x => x.Udi);

									var SelectedSubject = WorkSheet?.SelectSubject?.Select(x => x.Udi);

									var SelectedTopic = WorkSheet?.SelectTopic?.Select(x => x.Udi);

									NestedItems nested = new NestedItems();

									var Image = WorkSheet?.DesktopImage;

									string altText = Image?.Value<string>("altText");

									var NextGenImage = WorkSheet?.DesktopNextGenImage;

									nested.Category = string.Join(",", WorkSheet?.Name);

									nested.Title = WorkSheet?.Title;

									nested.SubTitle = WorkSheet?.SubTitle;

									var selectedAgeContent = Umbraco.Content(SelectedAgeValues);

									var SelectedClasssValues = selectedAgeContent?.Select(x => x.Value<string>("alternateClassName")).ToList();

									nested.SelectedClasses = SelectedClasssValues;

									var selectedSubjectContent = Umbraco.Content(SelectedSubject);

									//var SelectedSubjectValues = selectedSubjectContent.Select(x => x.Value<string>("itemName")).ToList();

									//nested.SelectedSubjects = SelectedSubjectValues;

									//var selectedTopicContent = Umbraco.Content(SelectedTopic);

									//var SelectedTopicValues = selectedTopicContent.Select(x => x.Value<string>("topicName")).ToList();

									var SelectedSubjectValues = selectedSubjectContent?.Select(x => x.Value<string>("subjectName")).ToList();

									nested.SelectedSubjects = SelectedSubjectValues;

									var selectedTopicContent = Umbraco.Content(SelectedTopic);

									var SelectedTopicValues = selectedTopicContent?.Select(x => x.Value<string>("topicName")).ToList();

									nested.SelectedTopics = SelectedTopicValues;

									nested.IsPaid = WorkSheet?.IsPaid ?? false;
									//nested.IsPaid = true;


									nested.PreviewPdf = WorkSheet?.UploadPreviewPdf;

									nested.IsEnabledForDetailsPage = WorkSheet?.IsEnableForDetailsPage ?? false;

									nested.WorksheetId = WorkSheet?.Id ?? 0;

									//nested.WorksheetDetailsUrl = WorkSheet?.Url();

									if (!String.IsNullOrEmpty(WorkSheet.Value<string>("umbracoUrlAlias")))

										nested.WorksheetDetailsUrl = WorkSheet.Value<string>("umbracoUrlAlias");

									else

										nested.WorksheetDetailsUrl = WorkSheet?.Url();

									string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet?.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");
									//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + "Paid";

									nested.subscriptionStatus = new SubscriptionStatus();

									string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");
									//string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + "Paid";

									nested.subscriptionStatus.DownloadString = DownloadString;

									nested.subscriptionStatus.DownloadUrl = downloadUrl;

									if (Image != null)
									{
										if (NextGenImage != null)
										{
											nested.NextGenImage = NextGenImage.Url();
										}

										nested.AltText = altText;

										nested.ImagesSrc = Image.Url();

									}

									nested.Description = WorkSheet?.Description;

									if (RecentlyDownloadedBonusWorkSheet != null && RecentlyDownloadedBonusWorkSheet.Count != 0)

									{

										nested.RecentlyDownloaded = RecentlyDownloadedBonusWorkSheet.Contains(WorkSheet.Id);

									}

									//if (!String.IsNullOrEmpty(WorkSheet.FacebookContent) || !String.IsNullOrEmpty(WorkSheet.WhatsAppContent) || !String.IsNullOrEmpty(WorkSheet.MailContent))

									//{

									//	var commonContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?

									//					  .Where(x => x.ContentType.Alias == "commonContent").OfType<CommonContent>().FirstOrDefault();

									//	string referralText = string.Empty;

									//	if (loggedin != null)

									//	{

									//		if (!String.IsNullOrWhiteSpace(commonContent.ShareReferralText.ToString()) && !String.IsNullOrWhiteSpace(loggedin.ReferralCode))

									//		{

									//			referralText = commonContent.ShareReferralText.ToString().ToString().Replace("<p>", "").Replace("</p>", "");

									//			referralText = referralText.Replace("{referralcode}", loggedin.ReferralCode);

									//		}

									//	}

									//	if (!String.IsNullOrEmpty(WorkSheet.FacebookContent))

									//	{

									//		if (!String.IsNullOrEmpty(referralText))

									//			social.FBShare = WorkSheet.FacebookContent + "\n" + referralText;

									//		else

									//			social.FBShare = WorkSheet.FacebookContent;

									//	}

									//	if (!String.IsNullOrEmpty(WorkSheet.WhatsAppContent))

									//	{

									//		if (!String.IsNullOrEmpty(referralText))

									//			social.WhatAppShare = WorkSheet.WhatsAppContent + "\n" + referralText;

									//		else

									//			social.WhatAppShare = WorkSheet.WhatsAppContent;

									//	}

									//	if (!String.IsNullOrEmpty(WorkSheet.MailContent))

									//	{

									//		social.EmailShare = WorkSheet.MailContent + "`" + WorkSheet?.Title;

									//	}

									//}

									var commonContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
												  .Where(x => x?.ContentType.Alias == "structureProgramRoot").OfType<StructureProgramRoot>().FirstOrDefault();

									if (commonContent != null)
									{
										string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
										string referralText = string.Empty;
										string wkstPlanUrl = string.Empty;

										if (!String.IsNullOrEmpty(WorkSheet.Value<string>("umbracoUrlAlias")))
											wkstPlanUrl = domain + WorkSheet.Value<string>("umbracoUrlAlias");
										else
											wkstPlanUrl = domain + WorkSheet?.Url();

										

										if (loggedin != null)
										{
											if (!String.IsNullOrWhiteSpace(commonContent.ReferralContent.ToString()) && !String.IsNullOrWhiteSpace(loggedin.ReferralCode))
											{
												referralText = commonContent.ReferralContent.ToString().ToString().Replace("<p>", "").Replace("</p>", "");

												referralText = referralText.Replace("{referal}", loggedin.ReferralCode);

												referralText = referralText.Replace("{loginurl}", domain + "my-account/login?referralcode=" + loggedin.ReferralCode);
											}
										}

										if (!String.IsNullOrEmpty(commonContent.FacebookContent))
										{
											string FacebookContent = commonContent.FacebookContent.Replace("{worksheeturl}", wkstPlanUrl);

											if (!String.IsNullOrEmpty(FacebookContent))
												social.FBShare = FacebookContent.Replace("{referal}", referralText);
											else
											{
												string facebookContent = FacebookContent.Replace("{referal}", "");
												social.FBShare = facebookContent;
											}
										}

										if (!String.IsNullOrEmpty(commonContent.WhatsAppContent))
										{
											string WhatsAppContent = commonContent.WhatsAppContent.Replace("{worksheeturl}", wkstPlanUrl);

											if (!String.IsNullOrEmpty(referralText))
												social.WhatAppShare = WhatsAppContent.Replace("{referal}", referralText);
											else
											{
												string whatsappContent = WhatsAppContent.Replace("{referal}", "");
												social.WhatAppShare = whatsappContent;
											}
										}

										if (!String.IsNullOrEmpty(commonContent.MailContent))
										{
											string MailContent = commonContent.MailContent.Replace("{worksheeturl}", wkstPlanUrl);

											if (!String.IsNullOrEmpty(referralText))
												social.EmailShare = MailContent.Replace("{referal}", referralText);
											else
											{
												string mailContent = MailContent.Replace("{referal}", "");
												social.EmailShare = mailContent + "`" + nested?.Title;
											}
										}
									}

									nested.socialItems = social;
									nested.IsWorkSheet = true;

									items.NestedItems.Add(nested);
								}
							}
							//);
							//items.NestedItems = NestedItems;
						}

						catch { }

						ReturnList.Add(items);

					}

				}

			}

			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");
			}

			return ReturnList;

		}

		private async Task<List<DownloadBonusWorkSheet>> GetMostRecommendedBonusVideoWorkSheet()
		{
			dbProxy _db = new dbProxy();
			List<DownloadBonusWorkSheet> downloadBonusWorkSheet = new List<DownloadBonusWorkSheet>();

			string culture = CultureName.GetCultureName();
			if (String.IsNullOrEmpty(culture))
				culture = "en-US";
			downloadBonusWorkSheet = _db.GetDataMultiple("GetMostRecommendedBonusVideoWorkSheet", downloadBonusWorkSheet, null);

			return downloadBonusWorkSheet;
		}

		//private async Task<List<WorkSheetVideoItems>> GetRelatedFilterData(StructureProgramRoot worksheetRoot, WorksheetInput input)
		//{

		//	List<WorkSheetVideoItems> ReturnList = new List<WorkSheetVideoItems>();

		//	SocialItems social = new SocialItems();

		//	LoggedIn loggedin = new LoggedIn();

		//	loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

		//	int RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

		//	try

		//	{

		//		string bitlyLink = String.Empty;

		//		string culture = CultureName.GetCultureName().Replace("/", "");

		//		if (String.IsNullOrEmpty(culture))

		//			culture = "en-US";

		//		_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//		var nextGenMediaUrl = worksheetRoot?.Value<IPublishedContent>("seeMoreNextGen");

		//		if (worksheetRoot != null)

		//		{

		//			_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//			var selectedWorkSheet = Umbraco.Content(input.worksheetId);

		//			List<StructureProgramItems> WorkSheets = new List<StructureProgramItems>();

		//			if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbytopic")

		//			{

		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//				var siblingworksheets = worksheetRoot?.DescendantsOrSelf()?

		//								.Where(x => x.ContentType.Alias == "structureProgramItems")

		//								.OfType<StructureProgramItems>().ToList();

		//				var getcurrentworksheetindex = siblingworksheets?.IndexOf(selectedWorkSheet) ?? 0;

		//				var nextworksheets = siblingworksheets?.Skip(getcurrentworksheetindex + 1).Take(12).OfType<StructureProgramItems>().ToList();

		//				WorkSheets = nextworksheets;

		//			}

		//			if (!string.IsNullOrEmpty(input.Mode) && input.Mode == "relatedbysubject")

		//			{

		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//				var SelectedSubject = selectedWorkSheet.Parent;

		//				var siblingWorksheetsCategory = worksheetRoot?.DescendantsOrSelf()?

		//								.Where(x => x.ContentType.Alias == "worksheetCategory")

		//								.OfType<WorksheetCategory>().ToList();

		//				//var siblingsubject = SelectedSubject?.Siblings();

		//				var getcurrentsubjectindex = siblingWorksheetsCategory?.IndexOf(SelectedSubject) ?? 0;

		//				var nextworksheetsubjects = siblingWorksheetsCategory?.Where(z => z.FirstChild<StructureProgramItems>() != null)?.Select(x => x.FirstChild<StructureProgramItems>())?.Skip(getcurrentsubjectindex + 1).Take(12).OfType<StructureProgramItems>().ToList();

		//				WorkSheets = nextworksheetsubjects;

		//				//.Select(x => x.FirstChild<StructureProgramItems>()).ToList();

		//			}

		//			IEnumerable<StructureProgramItems> WorksheetItem = new List<StructureProgramItems>();

		//			//var WorkSheets = worksheetRoot?.DescendantsOrSelf()?

		//			//                .Where(x => x.ContentType.Alias == "structureProgramItems")

		//			//                .OfType<StructureProgramItems>().ToList();

		//			//var FilteredWorkSheets = WorkSheets;

		//			if (WorkSheets != null)

		//			{

		//				WorkSheetVideoItems items = new WorkSheetVideoItems();

		//				try

		//				{

		//					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//					List<WorksheetCategory> subjects = new List<WorksheetCategory>();

		//					List<TopicsName> topics = new List<TopicsName>();

		//					items.NestedItems = new List<dynamic>();

		//					var RecentlyDownloadedBonusWorkSheet = await GetRecentlyDownloadedBonusWorkSheet(RefUserId, "BonusWorkSheet");

		//					//var WorkSheetsPaged = WorkSheets;

		//					//Parallel.ForEach(WorkSheetsPaged, async WorkSheet =>

		//					foreach (var WorkSheet in WorkSheets)

		//					{

		//						if (WorkSheet != null)

		//						{

		//							var SelectedAgeValues = WorkSheet?.SelectAgeGroup?.Select(x => x.Udi);

		//							var SelectedSubject = WorkSheet?.SelectSubject?.Select(x => x.Udi);

		//							var SelectedTopic = WorkSheet?.SelectTopic?.Select(x => x.Udi);

		//							NestedItems nested = new NestedItems();

		//							var Image = WorkSheet?.DesktopImage;

		//							string altText = Image?.Value<string>("altText");

		//							var NextGenImage = WorkSheet?.DesktopNextGenImage;

		//							nested.Category = string.Join(",", WorkSheet?.Name);

		//							nested.Title = WorkSheet?.Title;

		//							nested.SubTitle = WorkSheet?.SubTitle;

		//							var selectedAgeContent = Umbraco.Content(SelectedAgeValues);

		//							var SelectedClasssValues = selectedAgeContent?.Select(x => x.Value<string>("alternateClassName")).ToList();

		//							nested.SelectedClasses = SelectedClasssValues;

		//							var selectedSubjectContent = Umbraco.Content(SelectedSubject);

		//							//var SelectedSubjectValues = selectedSubjectContent.Select(x => x.Value<string>("itemName")).ToList();

		//							//nested.SelectedSubjects = SelectedSubjectValues;

		//							//var selectedTopicContent = Umbraco.Content(SelectedTopic);

		//							//var SelectedTopicValues = selectedTopicContent.Select(x => x.Value<string>("topicName")).ToList();

		//							var SelectedSubjectValues = selectedSubjectContent?.Select(x => x.Value<string>("subjectName")).ToList();

		//							nested.SelectedSubjects = SelectedSubjectValues;

		//							var selectedTopicContent = Umbraco.Content(SelectedTopic);

		//							var SelectedTopicValues = selectedTopicContent?.Select(x => x.Value<string>("topicName")).ToList();

		//							nested.SelectedTopics = SelectedTopicValues;

		//							nested.IsPaid = WorkSheet?.IsPaid ?? false;

		//							//nested.IsPaid = true;


		//							nested.PreviewPdf = WorkSheet?.UploadPreviewPdf;

		//							nested.IsEnabledForDetailsPage = WorkSheet?.IsEnableForDetailsPage ?? false;

		//							nested.WorksheetId = WorkSheet?.Id ?? 0;

		//							//nested.WorksheetDetailsUrl = WorkSheet?.Url();

		//							if (!String.IsNullOrEmpty(WorkSheet.Value<string>("umbracoUrlAlias")))

		//								nested.WorksheetDetailsUrl = WorkSheet.Value<string>("umbracoUrlAlias");

		//							else

		//								nested.WorksheetDetailsUrl = WorkSheet?.Url();

		//							string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet?.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");

		//							//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + "Paid";

		//							nested.subscriptionStatus = new SubscriptionStatus();

		//							string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");

		//							//string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + "Paid";

		//							nested.subscriptionStatus.DownloadString = DownloadString;

		//							nested.subscriptionStatus.DownloadUrl = downloadUrl;

		//							if (Image != null)

		//							{

		//								if (NextGenImage != null)

		//								{

		//									nested.NextGenImage = NextGenImage.Url();

		//								}

		//								nested.AltText = altText;

		//								nested.ImagesSrc = Image.Url();

		//							}

		//							nested.Description = WorkSheet?.Description;

		//							if (RecentlyDownloadedBonusWorkSheet != null && RecentlyDownloadedBonusWorkSheet.Count != 0)

		//							{

		//								nested.RecentlyDownloaded = RecentlyDownloadedBonusWorkSheet.Contains(WorkSheet.Id);

		//							}

		//							if (!String.IsNullOrEmpty(WorkSheet.FacebookContent) || !String.IsNullOrEmpty(WorkSheet.WhatsAppContent) || !String.IsNullOrEmpty(WorkSheet.MailContent))

		//							{

		//								var commonContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?

		//												  .Where(x => x.ContentType.Alias == "commonContent").OfType<CommonContent>().FirstOrDefault();

		//								string referralText = string.Empty;

		//								if (loggedin != null)

		//								{

		//									if (!String.IsNullOrWhiteSpace(commonContent.ShareReferralText.ToString()) && !String.IsNullOrWhiteSpace(loggedin.ReferralCode))

		//									{

		//										referralText = commonContent.ShareReferralText.ToString().ToString().Replace("<p>", "").Replace("</p>", "");

		//										referralText = referralText.Replace("{referralcode}", loggedin.ReferralCode);

		//									}

		//								}

		//								if (!String.IsNullOrEmpty(WorkSheet.FacebookContent))

		//								{

		//									if (!String.IsNullOrEmpty(referralText))

		//										social.FBShare = WorkSheet.FacebookContent + "\n" + referralText;

		//									else

		//										social.FBShare = WorkSheet.FacebookContent;

		//								}

		//								if (!String.IsNullOrEmpty(WorkSheet.WhatsAppContent))

		//								{

		//									if (!String.IsNullOrEmpty(referralText))

		//										social.WhatAppShare = WorkSheet.WhatsAppContent + "\n" + referralText;

		//									else

		//										social.WhatAppShare = WorkSheet.WhatsAppContent;

		//								}

		//								if (!String.IsNullOrEmpty(WorkSheet.MailContent))

		//								{

		//									social.EmailShare = WorkSheet.MailContent + "`" + WorkSheet?.Title;

		//								}

		//							}

		//							nested.socialItems = social;

		//							nested.IsWorkSheet = true;

		//							items.NestedItems.Add(nested);

		//						}

		//					}

		//					//);

		//					//items.NestedItems = NestedItems;

		//				}

		//				catch { }

		//				ReturnList.Add(items);

		//			}

		//		}

		//	}

		//	catch (Exception ex)

		//	{

		//		Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");

		//	}

		//	return ReturnList;

		//}

		[HttpPost]
		public ActionResult GetFilteredSubjects(WorksheetInput input)
		{

			Responce responce = new Responce();
			try
			{
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				StructureProgramRoot worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
									  .Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();
				if (!string.IsNullOrEmpty(input.selectedAgeGroup))
				{
					List<WorksheetCategory> filteredsubjects = worksheetRoot?.DescendantsOrSelf()?
										   .Where(x => x.ContentType.Alias == "worksheetCategory" && x.Value<bool>("isActive") == true && x.Children.Any()
											&& input.selectedAgeGroup.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Value<Link>("ageGroup").Udi)?.Value<string>("alternateClassName").ToLower()))
										   .OfType<WorksheetCategory>().ToList();

					List<Subjects> subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>
			   ()?.Where(x => x.IsActive == true && filteredsubjects.Any(y => Umbraco?.Content(y?.CategoryName?.Udi)?.Value<int>("SubjectValue") == x?.SubjectValue))?.OrderBy(x => x?.SubjectName).ToList();

					return PartialView("/Views/Partials/_FilterSturcuredProgramSubject.cshtml", subjects);
				}
				else
				{
					List<Subjects> subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>
			  ()?.Where(x => x.IsActive == true)?.OrderBy(x => x.SubjectName).ToList();

					return PartialView("/Views/Partials/_FilterSturcuredProgramSubject.cshtml", subjects);
				}
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetFilteredSubjects");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		//[HttpPost]
		//public ActionResult GetFilteredSubjects(WorksheetInput input)
		//{
		//	_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//	StructureProgramRoot worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
		//						  .Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();
		//	if (!string.IsNullOrEmpty(input.selectedAgeGroup))
		//	{
		//		List<WorksheetCategory> filteredsubjects = worksheetRoot?.DescendantsOrSelf()?
		//							   .Where(x => x.ContentType.Alias == "worksheetCategory" && x.Value<bool>("isActive") == true && x.Children.Any()
		//							   && input.selectedAgeGroup.ToLower().Split(',').Contains(x.Parent.Name.ToLower()))
		//							   .OfType<WorksheetCategory>().ToList();

		//		List<Subjects> subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>
		//   ()?.Where(x => x.IsActive == true && filteredsubjects.Any(y => Umbraco.Content(y.CategoryName.Udi)?.Value<int>("SubjectValue") == x.SubjectValue))?.OrderBy(x => x.SubjectName).ToList();

		//		return PartialView("/Views/Partials/_FilterSturcuredProgramSubject.cshtml", subjects);
		//	}
		//	else
		//	{
		//		List<Subjects> subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>
		//  ()?.Where(x => x.IsActive == true)?.OrderBy(x => x.SubjectName).ToList();

		//		return PartialView("/Views/Partials/_FilterSturcuredProgramSubject.cshtml", subjects);
		//	}
		//}

		//		[HttpPost]
		//		public ActionResult GetFilteredTopics(WorksheetInput input)
		//		{
		//			_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//			StructureProgramRoot worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
		//								  .Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();


		//			if (!string.IsNullOrEmpty(input.selectedSubject))
		//			{
		//				List<TopicsName> filteredtopics = worksheetRoot?.DescendantsOrSelf()?
		//									   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
		//&& input.selectedSubject.ToLower().Split(',').Contains(x.Parent.Name.ToLower()))
		//									   .OfType<TopicsName>().ToList();

		//				List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
		//.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
		//.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
		//		   ()?.Where(x => x.IsActive == true && filteredtopics.Any(y => Umbraco.Content(y.TopicMapping.Udi)?.Value<int>("TopicValue") == x.TopicValue))?.OrderBy(x => x.TopicName).ToList();

		//				return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
		//			}
		//			else if (!string.IsNullOrEmpty(input.selectedAgeGroup))
		//			{
		//				List<WorksheetCategory> filteredsubjects = worksheetRoot?.DescendantsOrSelf()?
		//									   .Where(x => x.ContentType.Alias == "worksheetCategory" && x.Value<bool>("isActive") == true && x.Children.Any()
		//&& input.selectedAgeGroup.ToLower().Split(',').Contains(x.Parent.Name.ToLower()))
		//									   .OfType<WorksheetCategory>().ToList();

		//				List<Subjects> subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>
		//		  ()?.Where(x => x.IsActive == true && filteredsubjects.Any(y => Umbraco.Content(y.CategoryName.Udi)?.Value<int>("SubjectValue") == x.SubjectValue))?.OrderBy(x => x.SubjectName).ToList();

		//				List<TopicsName> filteredtopics = worksheetRoot?.DescendantsOrSelf()?
		//									   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
		//&& subjects.Any(y => y.Name.ToLower() == x.Parent.Name.ToLower()) && input.selectedAgeGroup.ToLower().Split(',').Contains(x.Parent.Parent.Name.ToLower()))
		//									   .OfType<TopicsName>().ToList();

		//				List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
		//.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
		//.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
		//		   ()?.Where(x => x.IsActive == true && filteredtopics.Any(y => Umbraco.Content(y.TopicMapping?.Udi)?.Value<int>("TopicValue") == x.TopicValue))?.OrderBy(x => x.TopicName).ToList();

		//				return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
		//			}
		//			else
		//			{
		//				List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
		//.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
		//.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
		//()?.Where(x => x.IsActive == true)?.OrderBy(x => x.TopicName).ToList();

		//				return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
		//			}
		//		}

		[HttpPost]
		public ActionResult GetFilteredTopics(WorksheetInput input)
		{

			Responce responce = new Responce();
			try
			{
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				StructureProgramRoot worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
									  .Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();


				if (!string.IsNullOrEmpty(input.selectedSubject))
				{
					string[] strnglnth = input.selectedSubject.Split(',');
					List<TopicsName> filteredtopics;
					if (strnglnth.Length == 1)
					{
						bool IsNumericValue = IsNumeric(strnglnth[0]);
						if (IsNumericValue == true)
						{
							filteredtopics = worksheetRoot?.DescendantsOrSelf()?
												   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
													&& input.selectedSubject.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Value<Link>("categoryName").Udi)?.Value<string>("subjectValue").ToLower()))
												   .OfType<TopicsName>().ToList();
						}
						else
						{
							filteredtopics = worksheetRoot?.DescendantsOrSelf()?
												   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
													&& input.selectedSubject.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Value<Link>("categoryName").Udi)?.Value<string>("subjectName").ToLower()))
												   .OfType<TopicsName>().ToList();
						}
					}
					else
					{
						filteredtopics = worksheetRoot?.DescendantsOrSelf()?
										   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
											&& input.selectedSubject.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Value<Link>("categoryName").Udi)?.Value<string>("subjectName").ToLower()))
										   .OfType<TopicsName>().ToList();
					}

					if (!string.IsNullOrEmpty(input.selectedAgeGroup))
					{
						//filteredtopics = worksheetRoot?.DescendantsOrSelf()?
						//				   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
						//					&& input.selectedSubject.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Value<Link>("categoryName").Udi)?.Value<string>("subjectValue").ToLower())
						//					&& input.selectedAgeGroup.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Parent?.Value<Link>("ageGroup").Udi)?.Value<string>("alternateClassName").ToLower()))
						//				   .OfType<TopicsName>().ToList();

						filteredtopics = worksheetRoot?.DescendantsOrSelf()?
										   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
											&& input.selectedSubject.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Value<Link>("categoryName").Udi)?.Value<string>("subjectName").ToLower())
											&& input.selectedAgeGroup.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Parent?.Value<Link>("ageGroup").Udi)?.Value<string>("alternateClassName").ToLower()))
										   .OfType<TopicsName>().ToList();
					}


					List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
						.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
						.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
							()?.Where(x => x.IsActive == true && filteredtopics.Any(y => Umbraco.Content(y.TopicMapping.Udi)?.Value<int>("topicValue") == x.TopicValue))?.OrderBy(x => x.TopicName).ToList();

					return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
				}
				else if (!string.IsNullOrEmpty(input.selectedAgeGroup))
				{
					List<WorksheetCategory> filteredsubjects = worksheetRoot?.DescendantsOrSelf()?
										   .Where(x => x.ContentType.Alias == "worksheetCategory" && x.Value<bool>("isActive") == true && x.Children.Any()
											&& input.selectedAgeGroup.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Value<Link>("ageGroup").Udi)?.Value<string>("alternateClassName").ToLower()))
										   .OfType<WorksheetCategory>().ToList();

					List<Subjects> subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>
											()?.Where(x => x.IsActive == true && filteredsubjects.Any(y => Umbraco?.Content(y?.CategoryName?.Udi)?.Value<int>("subjectValue") == x?.SubjectValue))?.OrderBy(x => x.SubjectName).ToList();

					List<TopicsName> filteredtopics = worksheetRoot?.DescendantsOrSelf()?
										   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
											&& subjects.Any(y => y.Name.ToLower() == x.Parent.Name.ToLower())
											&& input.selectedAgeGroup.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Parent?.Value<Link>("ageGroup").Udi)?.Value<string>("alternateClassName").ToLower()))
										   .OfType<TopicsName>().ToList();

					if (!string.IsNullOrEmpty(input.selectedSubject))
					{
						filteredtopics = worksheetRoot?.DescendantsOrSelf()?
										   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
											&& subjects.Any(y => y.Name.ToLower() == x.Parent.Name.ToLower())
											&& input.selectedSubject.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Value<Link>("subjectValue").Udi)?.Value<string>("subjectName").ToLower())
											&& input.selectedAgeGroup.ToLower().Split(',').Contains(Umbraco.Content(x.Parent?.Parent?.Value<Link>("ageGroup").Udi)?.Value<string>("alternateClassName").ToLower()))
										   .OfType<TopicsName>().ToList();
					}

					List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
											.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
											.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
											()?.Where(x => x.IsActive == true && filteredtopics.Any(y => Umbraco.Content(y.TopicMapping?.Udi)?.Value<int>("topicValue") == x.TopicValue))?.OrderBy(x => x.TopicName).ToList();

					return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
				}
				else
				{
					List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
							.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
							.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
							()?.Where(x => x.IsActive == true)?.OrderBy(x => x.TopicName).ToList();

					return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
				}
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetFilteredTopics");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GetRedirectUrl(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				StructureProgramRoot worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
									  .Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();
				string redirecturl = worksheetRoot.Url();
				if (worksheetRoot != null)
				{
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						var agegroups = input.selectedAgeGroup.ToLower().Split(',');
						if (agegroups.Any() && agegroups.Count() == 1)
						{
							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
							redirecturl = worksheetRoot?.DescendantsOrSelf()?
											.Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && x.Name.ToLower().Equals(input.selectedAgeGroup.ToLower())).FirstOrDefault().Url();
						}
						else
						{
							string url = "classes=" + input.selectedAgeGroup;
							redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
						}
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						var subjects = input.selectedSubject.ToLower().Split(',');
						if (subjects.Any() && subjects.Count() == 1)
						{
							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
							redirecturl = worksheetRoot?.DescendantsOrSelf()?
											.Where(x => x.ContentType.Alias == "subjects" && x.Name.ToLower().Equals(input.selectedSubject.ToLower()))?.FirstOrDefault()?.Url();
						}
						else
						{
							string url = "subjects=" + input.selectedSubject;
							redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
						}
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						var topics = input.selectedTopics.ToLower().Split(',');
						if (topics.Any() && topics.Count() == 1)
						{
							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
							redirecturl = worksheetRoot?.DescendantsOrSelf()?
											.Where(x => x.ContentType.Alias == "topics" && x.Name.ToLower().Equals(input.selectedTopics.ToLower())).FirstOrDefault().Url();
						}
						else
						{
							string url = "topics=" + input.selectedTopics;
							redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
						}
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&topics=" + input.selectedTopics;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&paymenttypes=" + input.selectedPaid;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
					{
						string url = "paymenttypes=" + input.selectedPaid;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}

					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "subjects=" + input.selectedSubject + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
					if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
					{
						string url = "paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
						redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
					}
				}
				return Json(redirecturl, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetRedirectUrl");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		//[HttpPost]
		//public ActionResult GetRedirectUrl(WorksheetInput input)
		//{
		//	_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//	StructureProgramRoot worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
		//						  .Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();
		//	string redirecturl = worksheetRoot.Url();
		//	if (worksheetRoot != null)
		//	{
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			var agegroups = input.selectedAgeGroup.ToLower().Split(',');
		//			if (agegroups.Any() && agegroups.Count() == 1)
		//			{
		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//				redirecturl = worksheetRoot?.DescendantsOrSelf()?
		//								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && x.Name.ToLower().Equals(input.selectedAgeGroup.ToLower())).FirstOrDefault().Url();
		//			}
		//			else
		//			{
		//				string url = "classes=" + input.selectedAgeGroup;
		//				redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//			}
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			var subjects = input.selectedSubject.ToLower().Split(',');
		//			if (subjects.Any() && subjects.Count() == 1)
		//			{
		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//				redirecturl = worksheetRoot?.DescendantsOrSelf()?
		//								.Where(x => x.ContentType.Alias == "subjects" && x.Name.ToLower().Equals(input.selectedSubject.ToLower())).FirstOrDefault().Url();
		//			}
		//			else
		//			{
		//				string url = "subjects=" + input.selectedSubject;
		//				redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//			}
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			var topics = input.selectedTopics.ToLower().Split(',');
		//			if (topics.Any() && topics.Count() == 1)
		//			{
		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//				redirecturl = worksheetRoot?.DescendantsOrSelf()?
		//								.Where(x => x.ContentType.Alias == "topics" && x.Name.ToLower().Equals(input.selectedTopics.ToLower())).FirstOrDefault().Url();
		//			}
		//			else
		//			{
		//				string url = "topics=" + input.selectedTopics;
		//				redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//			}
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&topics=" + input.selectedTopics;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}

		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//	}
		//	return Json(redirecturl, JsonRequestBehavior.AllowGet);
		//}

		//		[HttpPost]
		//		public ActionResult GetFilteredTopics(WorksheetInput input)
		//		{
		//			_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//			StructureProgramRoot worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
		//								  .Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();


		//			if (!string.IsNullOrEmpty(input.selectedSubject))
		//			{
		//				List<TopicsName> filteredtopics = worksheetRoot?.DescendantsOrSelf()?
		//									   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
		//									   && input.selectedSubject.ToLower().Split(',').Contains(x.Parent.Name.ToLower()))
		//									   .OfType<TopicsName>().ToList();

		//				List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
		//.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
		//.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
		//		   ()?.Where(x => x.IsActive == true && filteredtopics.Any(y => Umbraco.Content(y.TopicMapping.Udi)?.Value<int>("TopicValue") == x.TopicValue))?.OrderBy(x => x.TopicName).ToList();

		//				return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
		//			}
		//			else if (!string.IsNullOrEmpty(input.selectedAgeGroup))
		//			{
		//				List<WorksheetCategory> filteredsubjects = worksheetRoot?.DescendantsOrSelf()?
		//									   .Where(x => x.ContentType.Alias == "worksheetCategory" && x.Value<bool>("isActive") == true && x.Children.Any()
		//									   && input.selectedAgeGroup.ToLower().Split(',').Contains(x.Parent.Name.ToLower()))
		//									   .OfType<WorksheetCategory>().ToList();

		//				List<Subjects> subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>
		//		  ()?.Where(x => x.IsActive == true && filteredsubjects.Any(y => Umbraco.Content(y.CategoryName.Udi)?.Value<int>("SubjectValue") == x.SubjectValue))?.OrderBy(x => x.SubjectName).ToList();

		//				List<TopicsName> filteredtopics = worksheetRoot?.DescendantsOrSelf()?
		//									   .Where(x => x.ContentType.Alias == "topicsName" && x.Value<bool>("isActive") == true
		//									   && subjects.Any(y => y.Name.ToLower() == x.Parent.Name.ToLower()))
		//									   .OfType<TopicsName>().ToList();

		//				List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
		//.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
		//.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
		//		   ()?.Where(x => x.IsActive == true && filteredtopics.Any(y => Umbraco.Content(y.TopicMapping?.Udi)?.Value<int>("TopicValue") == x.TopicValue))?.OrderBy(x => x.TopicName).ToList();

		//				return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
		//			}
		//			else
		//			{
		//				List<Topics> topics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
		//.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?
		//.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>
		//()?.Where(x => x.IsActive == true)?.OrderBy(x => x.TopicName).ToList();

		//				return PartialView("/Views/Partials/_FilterSturcuredProgramTopic.cshtml", topics);
		//			}
		//		}


		//[HttpPost]
		//public ActionResult GetRedirectUrl(WorksheetInput input)
		//{
		//	_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//	StructureProgramRoot worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
		//						  .Where(x => x.ContentType.Alias == "structureProgramRoot")?.OfType<StructureProgramRoot>().FirstOrDefault();
		//	string redirecturl = worksheetRoot.Url();
		//	if (worksheetRoot != null)
		//	{
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			var agegroups = input.selectedAgeGroup.ToLower().Split(',');
		//			if (agegroups.Any() && agegroups.Count() == 1)
		//			{
		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//				redirecturl = worksheetRoot?.DescendantsOrSelf()?
		//								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && x.Name.ToLower().Equals(input.selectedAgeGroup.ToLower())).FirstOrDefault().Url();
		//			}
		//			else
		//			{
		//				string url = "classes=" + input.selectedAgeGroup;
		//				redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//			}
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			var subjects = input.selectedSubject.ToLower().Split(',');
		//			if (subjects.Any() && subjects.Count() == 1)
		//			{
		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//				redirecturl = worksheetRoot?.DescendantsOrSelf()?
		//								.Where(x => x.ContentType.Alias == "subjects" && x.Name.ToLower().Equals(input.selectedSubject.ToLower())).FirstOrDefault().Url();
		//			}
		//			else
		//			{
		//				string url = "subjects=" + input.selectedSubject;
		//				redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//			}
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			var topics = input.selectedTopics.ToLower().Split(',');
		//			if (topics.Any() && topics.Count() == 1)
		//			{
		//				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//				redirecturl = worksheetRoot?.DescendantsOrSelf()?
		//								.Where(x => x.ContentType.Alias == "topics" && x.Name.ToLower().Equals(input.selectedTopics.ToLower())).FirstOrDefault().Url();
		//			}
		//			else
		//			{
		//				string url = "topics=" + input.selectedTopics;
		//				redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//			}
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&topics=" + input.selectedTopics;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "paymenttypes=" + input.selectedPaid;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}

		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (!string.IsNullOrEmpty(input.selectedAgeGroup) && !string.IsNullOrEmpty(input.selectedSubject) && !string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "classes=" + input.selectedAgeGroup + "&subjects=" + input.selectedSubject + "&topics=" + input.selectedTopics + "&paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//		if (string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && !string.IsNullOrEmpty(input.selectedPaid) && !string.IsNullOrEmpty(input.searchText))
		//		{
		//			string url = "paymenttypes=" + input.selectedPaid + "&searchtext=" + input.searchText;
		//			redirecturl = worksheetRoot.Url() + "?q=" + clsCommon.Encryptwithbase64Code(url);
		//		}
		//	}
		//	return Json(redirecturl, JsonRequestBehavior.AllowGet);
		//}

		//[HttpPost]
		//public ActionResult DecryptQueryString(string queryString)
		//{
		//	var returnstring = clsCommon.DecryptWithBase64Code(queryString);
		//	return Json(returnstring, JsonRequestBehavior.AllowGet);
		//}

		[HttpPost]
		public ActionResult DecryptQueryString(string queryString)
		{
			Responce responce = new Responce();
			try
			{
				var returnstring = clsCommon.DecryptWithBase64Code(queryString);
				return Json(returnstring, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "DecryptQueryString");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult CaptureUserSubmitData(NoRecordFoundUserInputModel userInput)
		{
			Responce responce = new Responce();
			try
			{
				dbProxy _db = new dbProxy();
				HPPlc.Models.dbAccessClass db = new HPPlc.Models.dbAccessClass();
				List<HPPlc.Models.SelectedAgeGroup> myagegroup = new List<HPPlc.Models.SelectedAgeGroup>();
				myagegroup = db.GetUserSelectedUserGroup();
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

				List<SetParameters> sp = new List<SetParameters>() {
					new SetParameters { ParameterName = "@message", Value = userInput.message == null ? "":userInput.message},
					new SetParameters { ParameterName = "@searchText", Value = userInput.searchText == null ? "":userInput.searchText },
					new SetParameters { ParameterName = "@userId", Value = UserId.ToString() },
				};

				var status = _db.StoreData("Insert_NoRecordFoundUserData", sp);
				return Json(status, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "DecryptQueryString");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		//[HttpPost]
		//public ActionResult CaptureUserSubmitData(NoRecordFoundUserInputModel userInput)
		//{
		//	Responce responce = new Responce();

		//	try
		//	{

		//		dbProxy _db = new dbProxy();

		//		List<SetParameters> sp = new List<SetParameters>() {

		//			new SetParameters { ParameterName = "@message", Value = userInput.message == null ? "" : userInput.message },

		//			new SetParameters { ParameterName = "@searchText", Value = userInput.searchText == null ? "" : userInput.searchText},

		//		};

		//		var status = _db.StoreData("Insert_NoRecordFoundUserData", sp);

		//		return Json(status, JsonRequestBehavior.AllowGet);

		//	}

		//	catch (Exception ex)

		//	{

		//		responce.StatusCode = HttpStatusCode.InternalServerError;

		//		responce.Message = ex.ToString();

		//		Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "DecryptQueryString");

		//	}

		//	return Json(responce, JsonRequestBehavior.AllowGet);

		//}

		//public List<DownloadBonusWorkSheet> GetAllMostRecommendedBonusWorkSheet(WorksheetInput input)
		//{

		//	dbProxy _db = new dbProxy();

		//	List<DownloadBonusWorkSheet> downloadBonusWorkSheet = new List<DownloadBonusWorkSheet>();

		//	string culture = CultureName.GetCultureName();

		//	List<SetParameters> sp = new List<SetParameters>();

		//	downloadBonusWorkSheet = _db.GetDataMultiple("GetAllMostRecommendedBonusWorkSheet", downloadBonusWorkSheet, sp);

		//	return downloadBonusWorkSheet;

		//}

		public List<DownloadBonusWorkSheet> GetAllMostRecommendedBonusWorkSheet(string agegroup = "")
		{
			dbProxy _db = new dbProxy();
			List<DownloadBonusWorkSheet> downloadBonusWorkSheet = new List<DownloadBonusWorkSheet>();

			string culture = CultureName.GetCultureName();
			List<SetParameters> sp = new List<SetParameters>() {
					new SetParameters { ParameterName = "@AgeGroup", Value = agegroup}
				};

			downloadBonusWorkSheet = _db.GetDataMultiple("GetAllMostRecommendedBonusWorkSheet", downloadBonusWorkSheet, sp);
			return downloadBonusWorkSheet;
		}

		public async Task<SimilarResponse> GetRecommendedWord(string searchText = "")
		{
			searchText = "couning";
			SimilarResponse response = new SimilarResponse();
			JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

			string jSonVal = "{\r\n    \"word\":\""+ searchText + "\"\r\n}";
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, "https://djangostg.managesrvr.com/getwordsynonyms/");
			var content = new StringContent(jSonVal, null, "application/json");
			request.Content = content;
			var _response = await client.SendAsync(request);
			_response.EnsureSuccessStatusCode();
			var SimilarWords = await _response.Content.ReadAsStringAsync();

			response = JsonConvert.DeserializeObject<SimilarResponse>(SimilarWords);
			//response = JsonConvert.DeserializeObject<SimilarResponse>(SimilarWords);
			//response = (Response)jsonSerializer.DeserializeObject(SimilarWords);
			//var SimilarWords = await _response.Content.ReadAsStringAsync();
			//response = JsonConvert.DeserializeObject<ResponseSimilarWord>(SimilarWords);


			return response;
		}

		public bool IsNumeric(string s)
		{
			foreach (char c in s)
			{
				if (!char.IsDigit(c) && c != '.')
				{
					return false;
				}
			}

			return true;
		}
	}
}