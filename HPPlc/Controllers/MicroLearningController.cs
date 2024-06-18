using HPPlc.Models;
using HPPlc.Models.WorkSheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Controllers
{
    public class MicroLearningController : SurfaceController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;
        public MicroLearningController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }
		[HttpPost]
		public async Task<ActionResult> GetMicroLearningProgramList(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetVideoModel model = new WorkSheetVideoModel();
				model = await GetMicroLearningProgramListData(input);

				return PartialView("/Views/Partials/Worksheets/_structuredProgramList.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(MicroLearningController), ex, message: "GetMicroLearningProgramList");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		private async Task<WorkSheetVideoModel> GetMicroLearningProgramListData(WorksheetInput input)
		{
			WorkSheetVideoModel model = new WorkSheetVideoModel();
			try
			{
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				var worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
									.Where(x => x.ContentType.Alias == "microLearningRoot")?.OfType<MicroLearningRoot>().FirstOrDefault();

				//if (worksheetRoot != null)
				//{
				//	try
				//	{
				//		var pagingInfo = new Paging();
				//		var WorkSheets = await GetFilterData(worksheetRoot, input);
				//		int nextPage = input.currentPage + 1;
				//		pagingInfo.NextPage = nextPage;
				//		pagingInfo.DisplayItems = worksheetRoot.NoOfDisplayWorksheet;
				//		pagingInfo.TotalItems = WorkSheets?.FirstOrDefault()?.totalItems ?? 0;
				//		model.WorkSheets = WorkSheets;
				//		model.Paging = pagingInfo;
				//	}
				//	catch (Exception ex)
				//	{
				//		Logger.Error(reporting: typeof(MicroLearningController), ex, message: "GetMicroLearningProgramListData - sub block");
				//	}
				//}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(MicroLearningController), ex, message: "GetMicroLearningProgramListData");
			}

			return model;
		}

		public async Task<List<WorkSheetVideoItems>> GetFilterData(StructureProgramRoot worksheetRoot, WorksheetInput input)
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

				//var nextGenMediaUrl = worksheetRoot?.Value<IPublishedContent>("seeMoreNextGen");

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

					//List<Topics> topicmaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?.OfType<Topics>()?.Where(x => x.IsActive == true)?.OrderBy(x => x.TopicName).ToList();


					var WorkSheets = worksheetRoot?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "microLearningItems")
									.OfType<MicroLearningItems>().ToList();

					var FilteredWorkSheets = WorkSheets;

					if (WorkSheets != null)
					{

						List<string> subjectsfilterAllId = null;
						WorkSheetVideoItems items = new WorkSheetVideoItems();

						try
						{

							_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

							List<WorksheetCategory> subjects = new List<WorksheetCategory>();
							List<string> seletedAgeGroups = new List<string>();
							List<string> seletedSubjects = new List<string>();

							
							//if (!string.IsNullOrEmpty(input.selectedAgeGroup) && agemaster != null && agemaster.Any())
							//{
							//	seletedAgeGroups = agemaster?.Where(x => input.selectedAgeGroup.Split(',').Contains(x.ItemValue)).Select(x => x.Name).ToList();

							//	if (seletedAgeGroups != null && seletedAgeGroups.Any())
							//	{
							//		FilteredWorkSheets = FilteredWorkSheets.Where(item => item?.Parent?.Parent?.DescendantsOrSelf()?.OfType<WorksheetListingAgeWise>().FirstOrDefault()?.AgeGroup?.Name.Any(x => seletedAgeGroups.Contains(x.na))).ToList();
							//	}

							//}

							//if (!string.IsNullOrEmpty(input.selectedSubject) && subjectmaster != null && subjectmaster.Any())

							//{

							//	//seletedSubjects = subjectmaster.Where(x => input.selectedSubject.Split(',').Contains(x.SubjectName)).Select(x => x.Name).ToList();

							//	seletedSubjects = subjectmaster.Where(x => input.selectedSubject.Split(',').Contains(x.SubjectValue.ToString())).Select(x => x.Name).ToList();

							//	if (seletedSubjects != null && seletedSubjects.Any())

							//	{

							//		FilteredWorkSheets = FilteredWorkSheets.Where(item => item?.Parent?.SelectSubject.Any(x => seletedSubjects.Contains(x.Name))).ToList();

							//	}

							//}

							if (!string.IsNullOrEmpty(input.selectedPaid))
							{
								FilteredWorkSheets = FilteredWorkSheets.Where(x => input.selectedPaid.Split(',').Contains(Convert.ToString(Convert.ToInt16(x.IsPaid)))).ToList();
							}

							if (!string.IsNullOrEmpty(input.searchText) && input.searchText.All(char.IsDigit))
							{
								FilteredWorkSheets = WorkSheets.Where(x => x.Id.ToString().ToLower() == input.searchText.ToLower()).ToList();
							}
							//else if (!String.IsNullOrEmpty(input.searchText))
							//{
							//	//get Node Id based on search text
							//	List<AdvanceSearch> advanceSearches = new List<AdvanceSearch>();
							//	dbProxy _db = new dbProxy();
							//	List<SetParameters> sp = new List<SetParameters>()
							//	{
							//		new SetParameters { ParameterName = "@Searchtext", Value = input.searchText}
							//	};
							//	advanceSearches = _db.GetDataMultiple("USP_Proc_AdvanceSearch", advanceSearches, sp);

							//	if (advanceSearches == null || advanceSearches.Count == 0)
							//	{
							//		advanceSearches = new List<AdvanceSearch>();
							//	}

							//	FilteredWorkSheets = FilteredWorkSheets.Where(x => advanceSearches.Any(z => z.NodeId == x.Id)).ToList();
							//	//else
							//	//{
							//	//	var WorkSheetsEmpty = null;
							//	//	FilteredWorkSheets = WorkSheetsEmpty;
							//	//}
							//}


							//if (input.sortBy == "1")
							//{
							//	FilteredWorkSheets = FilteredWorkSheets.OrderByDescending(x => x.CreateDate).Take(worksheetRoot.NoOfRecentlyAddedWorksheet).ToList();
							//}

							//else if (input.sortBy == "2")
							//{
							//	var MostRecommendedBonusWorkSheet = await GetMostRecommendedBonusWorkSheet();

							//	if (MostRecommendedBonusWorkSheet != null)

							//	{

							//		FilteredWorkSheets = (from worksheets in FilteredWorkSheets

							//							  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on worksheets.Id equals recommendedbonusworksheet.WorkSheetId

							//							  orderby recommendedbonusworksheet.DownloadCount descending

							//							  select worksheets).ToList();

							//	}

							//}
							//else if (input.sortBy == "3")
							//{
							//	FilteredWorkSheets = FilteredWorkSheets.OrderBy(x => x.Title).ToList();
							//}
							//else if (FilteredWorkSheets.Count != 0 && string.IsNullOrEmpty(input.selectedAgeGroup) && string.IsNullOrEmpty(input.selectedSubject) && string.IsNullOrEmpty(input.selectedTopics) && string.IsNullOrEmpty(input.selectedPaid) && UserLoggedInOrNot == false)
							//{
							//	FilteredWorkSheets = FilteredWorkSheets.OrderBy(x => (x.RankingIndex == 0 ? WorkSheets.Count : x.RankingIndex)).ToList();
							//}
							//else
							//{
							//	if (UserLoggedInOrNot == true)
							//	{
							//		dbAccessClass db = new dbAccessClass();
							//		List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
							//		myagegroup = db.GetUserSelectedUserGroup();

							//		FilteredWorkSheets = FilteredWorkSheets?.OrderByDescending(x => x?.SelectAgeGroup.Any(y => myagegroup.Any(c => c?.AgeGroup != null && y?.Name.ToString() == c?.AgeGroup))).ToList(); //FilteredWorkSheets.OrderByDescending(lst => myagegroup.Any(y => y.AgeGroup == lst.SelectAgeGroup.Where(c => c.Name == y.AgeGroup))).ToList();
							//	}
							//}
							

							//If record found then 
							//if (input.NoRecordFound)
							//{
							//	if (UserLoggedInOrNot)
							//	{
							//		HPPlc.Models.dbAccessClass db = new HPPlc.Models.dbAccessClass();
							//		List<HPPlc.Models.SelectedAgeGroup> myagegroup = new List<HPPlc.Models.SelectedAgeGroup>();
							//		myagegroup = db.GetUserSelectedUserGroup();
							//		var agegroup = string.Join(",", myagegroup.Select(x => x.AgeGroup));
							//		List<DownloadBonusWorkSheet> MostRecommendedBonusWorkSheet = GetAllMostRecommendedBonusWorkSheet(agegroup);
							//		if (MostRecommendedBonusWorkSheet != null)
							//		{
							//			FilteredWorkSheets = (from worksheets in FilteredWorkSheets
							//								  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on worksheets.Id equals recommendedbonusworksheet.WorkSheetId
							//								  orderby recommendedbonusworksheet.DownloadCount descending
							//								  select worksheets).ToList();
							//		}
									
							//	}

							//	else
							//	{
							//		List<DownloadBonusWorkSheet> MostRecommendedBonusWorkSheet = GetAllMostRecommendedBonusWorkSheet();

							//		if (MostRecommendedBonusWorkSheet != null)
							//		{

							//			FilteredWorkSheets = (from worksheets in FilteredWorkSheets

							//								  join recommendedbonusworksheet in MostRecommendedBonusWorkSheet on worksheets.Id equals recommendedbonusworksheet.WorkSheetId

							//								  orderby recommendedbonusworksheet.DownloadCount descending

							//								  select worksheets).ToList();

							//		}

							//	}

							//}

							//End if record not found

							var RecentlyDownloadedMicroLearningWorkSheet = await GetRecentlyDownloadedMicroLearningWorkSheet(RefUserId, "microlearning");

							items.NestedItems = new List<dynamic>();

							items.totalItems = FilteredWorkSheets.Count;

							var WorkSheetsPaged = FilteredWorkSheets.Skip((input.currentPage - 1) * (worksheetRoot.NoOfDisplayWorksheet)).Take(worksheetRoot.NoOfDisplayWorksheet).ToList();

							
							items.TopicsUrl = FilteredWorkSheets?.FirstOrDefault()?.Url();
							items.SubjectUrl = FilteredWorkSheets?.FirstOrDefault()?.Parent?.Url();
							items.ClassUrl = FilteredWorkSheets?.FirstOrDefault()?.Parent?.Parent?.Url();

							foreach (var WorkSheet in WorkSheetsPaged)
							{
								try
								{
									var SelectedAgeValues = Umbraco.Content(WorkSheet?.Parent?.Parent?.DescendantsOrSelf().OfType<WorksheetListingAgeWise>()?.FirstOrDefault()?.AgeGroup?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault();

									var SelectedSubject = Umbraco.Content(WorkSheet?.Parent?.DescendantsOrSelf().OfType<WorksheetCategory>()?.FirstOrDefault()?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault();

									NestedItems nested = new NestedItems();

									var Image = WorkSheet?.DesktopImage;

									string altText = Image?.Value<string>("altText");

									var NextGenImage = WorkSheet?.DesktopNextGenImage;

									nested.Category = string.Join(",", WorkSheet?.Name);

									nested.Title = WorkSheet?.Title;

									nested.SubTitle = WorkSheet?.SubTitle;

									var SelectedClasssValues = SelectedAgeValues.AlternateClassName;

									nested.Age = SelectedClasssValues;

									nested.Subject = SelectedSubject.SubjectName;

									nested.IsPaid = WorkSheet?.IsPaid ?? false;
									
									nested.PreviewPdf = WorkSheet?.UploadPreviewPdf;

									nested.IsEnabledForDetailsPage = WorkSheet?.IsEnableForDetailsPage ?? false;

									nested.WorksheetId = WorkSheet?.Id ?? 0;

									//nested.WorksheetDetailsUrl = WorkSheet?.Url();

									if (!String.IsNullOrEmpty(WorkSheet.Value<string>("umbracoUrlAlias")))

										nested.WorksheetDetailsUrl = WorkSheet.Value<string>("umbracoUrlAlias");

									else

										nested.WorksheetDetailsUrl = WorkSheet?.Url();

									string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet?.Id.ToString()) + "&source=microlearning" + "&paid=" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");
									//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + "Paid";

									nested.subscriptionStatus = new SubscriptionStatus();

									string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "microlearningPrint" + "$" + ((bool)(WorkSheet?.IsPaid) ? "Paid" : "Free");
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

									if (RecentlyDownloadedMicroLearningWorkSheet != null && RecentlyDownloadedMicroLearningWorkSheet.Count != 0)
										nested.RecentlyDownloaded = RecentlyDownloadedMicroLearningWorkSheet.Contains(WorkSheet.Id);

									var commonContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
												  .Where(x => x?.ContentType.Alias == "microLearningRoot").OfType<MicroLearningRoot>().FirstOrDefault();

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
								catch { }
							}
							
						}

						catch { }

						ReturnList.Add(items);

					}

				}

			}

			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(MicroLearningController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");
			}

			return ReturnList;

		}


		public async Task<List<int>> GetRecentlyDownloadedMicroLearningWorkSheet(int userId, string Doctype)
		{
			dbProxy _db = new dbProxy();
			List<int> recentlyDownloadMicroLearningWorkSheet = new List<int>();

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
				recentlyDownloadMicroLearningWorkSheet = _db.GetDataMultiple("GetRecentlyDownloadedBonusWorkSheet", recentlyDownloadMicroLearningWorkSheet, sp);
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(MicroLearningController), ex, message: "GetRecentlyDownloadedMicroLearningWorkSheet");
			}

			return recentlyDownloadMicroLearningWorkSheet;
		}
	}
}