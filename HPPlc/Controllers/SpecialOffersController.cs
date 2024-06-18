﻿using HPPlc.Models;
using HPPlc.Models.FestivalOffers;
using HPPlc.Models.WorkSheet;
using System;
using System.Collections.Generic;
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
	public class SpecialOffersController : SurfaceController
	{
		private readonly IVariationContextAccessor _variationContextAccessor;
		public SpecialOffersController(IVariationContextAccessor variationContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
		}

		//[HttpPost]
		//public async Task<ActionResult> GetTeachersList(WorksheetInput input)
		//{
		//	Responce responce = new Responce();
		//	try
		//	{
		//		WorkSheetModel model = new WorkSheetModel();
		//		model = await GetWorkSheetListData(input);

		//		if (model != null)
		//		{
		//			_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//			var classRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
		//								.Where(x => x.ContentType.Alias == "teacherRoot")?.FirstOrDefault()?.Children?
		//								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
		//								.Where(x => x.AgeGroup.Name == input.selectedAgeGroup)?.FirstOrDefault();


		//			if (classRoot != null)
		//			{
		//				model.Title = classRoot.Title;
		//				model.SubTitle = classRoot.SubTitle;
		//			}
		//			//List<GetUserCurrentSubscription> UserCurrentSubscription = new List<GetUserCurrentSubscription>();
		//			//UserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtlsTeachers);

		//			//if (UserCurrentSubscription != null && UserCurrentSubscription.Count > 0)
		//			//{
		//			//	model.WorkSheets.OrderByDescending(o => UserCurrentSubscription.Any(c => c.AgeGroup == o.AgeValue));
		//			//}
		//		}

		//		return PartialView("/Views/Partials/Worksheets/_workSheetsListTeachers.cshtml", model);
		//	}
		//	catch (Exception ex)
		//	{
		//		responce.StatusCode = HttpStatusCode.InternalServerError;
		//		responce.Message = ex.ToString();

		//		Logger.Error(reporting: typeof(TeachersController), ex, message: "GetWorksheetList");
		//	}

		//	return Json(responce, JsonRequestBehavior.AllowGet);
		//}


		[HttpPost]
		public async Task<ActionResult> GetSpecialoffersListDetails(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetModel model = new WorkSheetModel();
				model = await GetWorkSheetListData(input);

				if (model != null)
				{
					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
					var classRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
										.Where(x => x.ContentType.Alias == "offerRoot")?.FirstOrDefault()?.Children?
										.Where(x => x.ContentType.Alias == "specialOffersRoot")?.OfType<SpecialOffersRoot>()?
										.Where(x => x.OfferName == input.Mode)?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "specialOfferAge")?
										.OfType<SpecialOfferAge>()?.Where(x => Umbraco.Content(x.ClassName.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == input.selectedAgeGroup)?.FirstOrDefault();


					if (classRoot != null)
					{
						model.Title = classRoot.Title;
						model.SubTitle = classRoot.SubTitle;
					}
				}

				return PartialView("/Views/Partials/Worksheets/_workSheetsDetailSpecialOffer.cshtml", model);

			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(TeachersController), ex, message: "GetWorksheetList");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		//[HttpPost]
		//public ActionResult GetWorkSheetById(WorksheetInput input)
		//{
		//	Responce responce = new Responce();
		//	try
		//	{
		//		WorkSheetItems model = new WorkSheetItems();
		//		model = GetWorkSheetDetailsById(input);



		//		return PartialView("/Views/Partials/Worksheets/_teachersProgramSingle.cshtml", model);
		//	}
		//	catch (Exception ex)
		//	{
		//		responce.StatusCode = HttpStatusCode.InternalServerError;
		//		responce.Message = ex.ToString();
		//	}
		//	return Json(responce, JsonRequestBehavior.AllowGet);
		//}


		//public WorkSheetItems GetWorkSheetDetailsById(WorksheetInput input)
		//{
		//	WorkSheetItems model = new WorkSheetItems();
		//	try
		//	{

		//		//if (!String.IsNullOrWhiteSpace(input.selectedSubject))
		//		//	model.selectedSubjectsForSearch = string.Join(",", input.selectedSubject);
		//		//if (!String.IsNullOrWhiteSpace(input.IsCbseContent) && !input.IsCbseContent.Equals("0"))
		//		//	model.CbseContentCheckedForSearch = input.IsCbseContent;

		//		string vUserSubscription = String.Empty;
		//		string recommendedTitle = String.Empty;

		//		try
		//		{
		//			//Check here user subscription
		//			//LoggedIn loggedin = new LoggedIn();
		//			//loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);
		//			//string culture = CultureName.GetCultureName().Replace("/", "");
		//			//string subscribeUrl = culture + "subscription/";


		//			_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);


		//			TeacherProgramItems worksheet;
		//			worksheet = Umbraco?.Content(input.FilterId).DescendantsOrSelf().OfType<TeacherProgramItems>().FirstOrDefault();
		//			if (worksheet != null)
		//			{
		//				try
		//				{
		//					List<NestedItems> List = new List<NestedItems>();
		//					NestedItems nested = new NestedItems();
		//					string worksheetType = String.Empty;
		//					var PDF_File = worksheet?.UploadPdf;

		//					if (String.IsNullOrEmpty(worksheet?.UploadPreviewPdf))
		//						nested.PreviewPdf = worksheet?.UploadPdf;
		//					else
		//						nested.PreviewPdf = worksheet?.UploadPreviewPdf;

		//					//bitlyLink = worksheetItems?.BitlyLink;

		//					var desktopmedia = worksheet?.DesktopImage;
		//					var desktopmediaNextgen = worksheet?.DesktopNextGenImage;
		//					var mobilemedia = worksheet?.MobileImage;
		//					var mobilemediaNextgen = worksheet?.MobileNextGenImage;

		//					var agegroup = Umbraco.Content(worksheet?.Parent?.Parent?.DescendantsOrSelf()?
		//							.OfType<WorksheetListingAgeWise>()?.FirstOrDefault()?.AgeGroup?.Udi)?
		//							.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault();

		//					//var subject = Umbraco.Content(worksheet?.Parent?.Parent?.DescendantsOrSelf()?
		//					//				.OfType<WorksheetCategory>()?.FirstOrDefault()?.CategoryName?.Udi)?
		//					//				.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault();

		//					//var agegroup = worksheet?.Parent?.Parent?.Value<NameListItem>("ageGroup");
		//					//var subject = worksheet?.Parent?.Value<NameListItem>("categoryName");

		//					//var AgeTitle = worksheet?.SelectAgeGroup;
		//					//var AgeTitleDesc = Umbraco?.Content(AgeTitle?.Udi);
		//					var ItemName = agegroup.ItemName;
		//					if (!String.IsNullOrEmpty(ItemName))
		//						worksheetType = "age";

		//					if (desktopmediaNextgen != null)
		//						nested.NextGenImage = desktopmediaNextgen.Url();
		//					if (desktopmedia != null)
		//					{
		//						nested.AltText = desktopmedia.Value("altText").ToString();
		//						nested.ImagesSrc = desktopmedia.Url().ToString();
		//					}

		//					if (mobilemediaNextgen != null)
		//						nested.MobileNextGenImage = mobilemediaNextgen.Url();
		//					if (mobilemedia != null)
		//					{
		//						nested.MobileAltText = mobilemedia.Value("altText").ToString();
		//						nested.MobileImagesSrc = mobilemedia.Url().ToString();
		//					}

		//					//if (!String.IsNullOrEmpty(ItemName.ToString()))
		//					//	nested.Title = ItemName.ToString();

		//					nested.NoOfDays = int.Parse(Umbraco?.Content(worksheet?.NoOfDays?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue);
		//					nested.NoOfDaysName = worksheet?.NoOfDays?.Name;
		//					nested.Age = agegroup.ItemValue;

		//					nested.Title = worksheet.Title;
		//					nested.SubTitle = worksheet.SubTitle;
		//					nested.Description = worksheet?.Description;
		//					nested.WorksheetId = worksheet.Id;
		//					//nested.WorksheetDetailsDescription = worksheetItems?.DescriptionPageContent;
		//					nested.WorksheetDetailsDescription = null;
		//					//nested.Volume = worksheet.SelectWeek?.Name.ToString();
		//					//nested.Category = string.Join(",", worksheetItems.Category?.Select(x => x.Name).ToList());
		//					//nested.Category = string.Join(",", worksheetItems?.Name);

		//					//nested.IsQuizWorksheet = worksheetItems.IsQuizWorksheet;
		//					//nested.CBSEContentIncluded = worksheetItems.CBsecontentIncluded;

		//					nested.IsEnabledForDetailsPage = worksheet.IsEnableForDetailsPage;
		//					nested.WorksheetDetailsUrl = worksheet?.Url();

		//					string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(worksheet?.Id.ToString()) + "&source=TeachersWorkSheet";
		//					//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + "Paid";

		//					nested.subscriptionStatus = new SubscriptionStatus();
		//					string DownloadString = clsCommon.encrypto(worksheet.Id.ToString()) + "$" + "teachesworksheetPrint";
		//					//string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + "Paid";

		//					nested.subscriptionStatus.DownloadString = DownloadString;
		//					nested.subscriptionStatus.DownloadUrl = downloadUrl;

		//					//if (worksheet.SelectWeek != null)
		//					//	nested.Category = worksheet?.Name;

		//					//nested.Subject = subject.SubjectName;
		//					//nested.Topic = worksheet?.SelectTopic?.Name.ToString();


		//					WorksheetVideosHelper worksheetVideos = new WorksheetVideosHelper();
		//					nested = worksheetVideos.GetSocialItemsAndSubscriptionDetailsForTeachersWorkSheet(worksheet, input, nested, null);

		//					List.Add(nested);

		//					//load more display till totalcount more than displayed on page
		//					model.NestedItems = List;

		//				}
		//				catch (Exception ex)
		//				{
		//					Logger.Error(reporting: typeof(TeachersController), ex, message: "Worksheet - WorksheetDetailsData");
		//				}
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			Logger.Error(reporting: typeof(TeachersController), ex, message: "Worksheet - WorksheetDetailsData");
		//		}

		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(TeachersController), ex, message: "Worksheet - WorksheetDetailsData");
		//	}

		//	return model;
		//}

		public async Task<WorkSheetModel> GetWorkSheetListData(WorksheetInput input)
		{
			WorkSheetModel model = new WorkSheetModel();
			try
			{
				if (!String.IsNullOrEmpty(input.Mode))
				{
					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
					var worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
										.Where(x => x.ContentType.Alias == "offerRoot")?.FirstOrDefault()?.Children?
										.Where(x => x.ContentType.Alias == "specialOffersRoot")?.OfType<SpecialOffersRoot>()?
										.Where(x => x.OfferName == input.Mode)?.FirstOrDefault();


					if (worksheetRoot != null)
					{
						bool filterSelected = false;
						List<string> list = new List<string> { input.selectedAgeGroup };
						foreach (var item in list)
						{
							if (!String.IsNullOrWhiteSpace(item))
								filterSelected = true;
						}

						dbAccessClass db = new dbAccessClass();
						List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
						myagegroup = db.GetUserSelectedUserGroup();

						List<NameListItem> agemaster = new List<NameListItem>();

						if (myagegroup == null || myagegroup.Count == 0)
						{
							agemaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
															.Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?.Children?
															.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?
															.OfType<NameListItem>().Where(x => x.IsActice).ToList();

						}
						else
						{
							agemaster = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
															.Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?.Children?
															.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?
															.OfType<NameListItem>().Where(x => x.IsActice).OrderByDescending(lst => myagegroup.Any(y => y.AgeGroup == lst.ItemValue)).ToList();

						}

						if (input.DisplayAgeGroup > 0 && filterSelected == false)
						{
							int toBeDeduct = 0;
							int toBeDisplayWorksheet = worksheetRoot.NoOfDisplayWorksheet;
							//int toBeDisplayWorksheet = 10;
							if (toBeDisplayWorksheet > 0 && input.DisplayAgeGroup > 0)
								toBeDeduct = input.DisplayAgeGroup - toBeDisplayWorksheet;

							agemaster = (from agem in agemaster
										 select agem).Skip(toBeDeduct).Take(toBeDisplayWorksheet).ToList();
						}


						if (input.selectedAgeGroup != null && !string.IsNullOrWhiteSpace(input.selectedAgeGroup) && agemaster != null)
						{
							agemaster = agemaster.Where(x => input.selectedAgeGroup.Contains(x.Name)).ToList();
						}

						try
						{
							model.WorkSheets = await GetFilterAgeWiseData(worksheetRoot, agemaster, input, myagegroup);
							//model.LoadMore = ;
						}
						catch (Exception ex)
						{
							Logger.Error(reporting: typeof(TeachersController), ex, message: "GetWorkSheetListData - sub block");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(TeachersController), ex, message: "GetWorkSheetListData");
			}

			return model;
		}

		public async Task<List<WorkSheetItems>> GetFilterAgeWiseData(SpecialOffersRoot worksheetRoot, IEnumerable<NameListItem> ageMaster, WorksheetInput input, List<SelectedAgeGroup> myagegroup)
		{
			List<WorkSheetItems> ReturnList = new List<WorkSheetItems>();
			try
			{
				string bitlyLink = String.Empty;
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrEmpty(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

				//int firstTimeDisplayWorksheet = worksheetRoot;
				int firstTimeDisplayWorksheet = worksheetRoot.NoOfDisplayWorksheet;
				//var mediaUrl = worksheetRoot?.SeeMoreMedia;
				//var nextGenMediaUrl = worksheetRoot?.Value<IPublishedContent>("seeMoreNextGen");

				if (worksheetRoot != null)
				{
					if (ageMaster != null && ageMaster.Any())
					{
						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

						IEnumerable<SpecialOfferItems> WorksheetItem = new List<SpecialOfferItems>();

						var WorksheetAgeGroup = worksheetRoot?.Children?.Where(x => x.ContentType.Alias == "specialOfferAge")?
									.OfType<SpecialOfferAge>().ToList();

						if (WorksheetAgeGroup != null)
						{
							//List<string> subjectsfilterAllId = null;
							foreach (var age in ageMaster)
							{
								_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
								WorkSheetItems items = new WorkSheetItems();

								try
								{
									if (!String.IsNullOrWhiteSpace(age.ItemValue))
									{
										_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

										List<SpecialOfferItems> worksheetAllDaysWise = new List<SpecialOfferItems>();

										if (input.Mode == "single")
											worksheetAllDaysWise = GetWorksheetSingle(input);
										else
											worksheetAllDaysWise = GetWorksheetsByDays(age, worksheetRoot, firstTimeDisplayWorksheet, input);

										if (worksheetAllDaysWise != null && worksheetAllDaysWise.Any() && worksheetAllDaysWise.Count > 0)
										{
											WorksheetItem = worksheetAllDaysWise;
											//sorting
											//try
											//{
											//	if (worksheetAllDaysWise != null && worksheetAllDaysWise.Any() && worksheetAllDaysWise.Count > 0)
											//	{
											//		WorksheetItem = worksheetAllDaysWise?.
											//			OrderBy(x => Convert.ToInt32(Umbraco?.Content(x.NoOfDays?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue)).Take(firstTimeDisplayWorksheet).ToList();
											//	}
											//}
											//catch { }

											if (WorksheetItem != null && WorksheetItem.Any())
											{
												if (WorksheetItem != null && WorksheetItem.Count() > 0 && WorksheetItem.Any())
												{
													items.WorksheetTitle = WorksheetAgeGroup?.Where(x => x.ClassName.Name == age.ItemValue)?.FirstOrDefault()?.Title;
													items.Title = age.ItemName;
													items.ItemId = age.Id;
													items.AgeValue = age.ItemValue;

													//items.ViewAll = worksheetRoot.ViewAllTitle;

													List<NestedItems> NestedItems = new List<NestedItems>();
													foreach (var WorkSheet in WorksheetItem)
													{
														NestedItems nested = new NestedItems();

														//var subject = WorkSheet?.Parent?.Value<Subjects>("categoryName");

														//var subject = Umbraco.Content(WorkSheet?.Parent?.DescendantsOrSelf()?
														//				.OfType<WorksheetCategory>()?.FirstOrDefault()?.CategoryName?.Udi)?
														//				.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault();

														//int RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
														//bool UserLoggedInOrNot = HPPlc.Models.SessionExpireAttribute.UserLoggedIn();

														//if (UserLoggedInOrNot == true)
														//{
														//	var RecentlyDownloadedTeachersWorkSheet = await GetRecentlyDownloadedTeachersWorkSheet(RefUserId, "TeachersWorkSheet");

														//	if (RecentlyDownloadedTeachersWorkSheet != null && RecentlyDownloadedTeachersWorkSheet.Count != 0)
														//	{
														//		nested.RecentlyDownloaded = RecentlyDownloadedTeachersWorkSheet.Contains(WorkSheet.Id);
														//	}
														//}

														var Image = WorkSheet?.DesktopImage;
														string altText = Image?.Value<string>("altText");
														var NextGenImage = WorkSheet?.DesktopNextGenImage;
														//nested.NoOfDays = int.Parse(Umbraco?.Content(WorkSheet?.NoOfDays?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue);
														//nested.NoOfDaysName = WorkSheet?.NoOfDays?.Name;
														nested.Age = items.AgeValue;

														//nested.Category = string.Join(",", WorkSheet?.Name);
														//nested.Subject = subject.SubjectName.ToString();
														//nested.Topic = WorkSheet?.SelectTopic?.Name.ToString();

														//nested.Category = string.Join(",", WorkSheet.Category?.Select(x => x.Name).ToList());
														//nested.IsQuizWorksheet = null;
														//nested.CBSEContentIncluded = WorkSheet.CBsecontentIncluded;

														nested.PreviewPdf = WorkSheet?.UploadPreviewPdf;
														nested.IsEnabledForDetailsPage = WorkSheet.IsEnableForDetailsPage;
														//nested.WorksheetDetailsUrl = WorkSheet?.Url();

														if (!String.IsNullOrEmpty(WorkSheet.Value<string>("umbracoUrlAlias")))
															nested.WorksheetDetailsUrl = WorkSheet.Value<string>("umbracoUrlAlias");
														else
															nested.WorksheetDetailsUrl = WorkSheet?.Url();


														string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet?.Id.ToString()) + "&source=specialoffer";
														//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + "Paid";

														nested.subscriptionStatus = new SubscriptionStatus();
														string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "specialofferPrint";

														nested.subscriptionStatus.DownloadString = DownloadString;

														nested.subscriptionStatus.DownloadUrl = downloadUrl;
														//string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + "Paid";



														if (Image != null)
														{
															if (NextGenImage != null)
															{
																nested.NextGenImage = NextGenImage.Url();
															}
															nested.AltText = altText;
															nested.ImagesSrc = Image.Url();
														}

														nested.Title = WorkSheet?.Title;
														nested.SubTitle = WorkSheet?.SubTitle;
														nested.Description = WorkSheet?.Description;
														nested.WorksheetId = WorkSheet.Id;

														FestivalOffersHepler worksheet = new FestivalOffersHepler();
														nested = worksheet.GetSocialItemsAndSubscriptionDetailsForSpecialOfferWorkSheet(WorkSheet, input, nested);

														
														//nested.WorksheetDetailsDescription = WorkSheet?.DescriptionPageContent;

														//WorksheetVideosHelper worksheetVideos = new WorksheetVideosHelper();
														//nested = worksheetVideos.GetSocialItemsAndSubscriptionDetailsForTeachersWorkSheet(WorkSheet, input, nested, myagegroup);
														#region See More
														//try
														//{
														//	SeeMore seeMore = new SeeMore();
														//	seeMore.VideoDetailsUrl = WorksheetAgeGroup?.Where(x => x.ClassName.Name == age.ItemValue)?.FirstOrDefault()?.Url();
														//	if (mediaUrl != null)
														//	{
														//		if (nextGenMediaUrl != null)
														//			seeMore.NextGenMediaUrl = nextGenMediaUrl.Url();
														//		seeMore.MediaAltText = mediaUrl.Value("altText").ToString();
														//		seeMore.MediaUrl = mediaUrl.Url().ToString();
														//	}

														//	NestedItems.Add(nested);
														//	items.SeeMore = seeMore;
														//}
														//catch (Exception ex)
														//{
														//	Logger.Error(reporting: typeof(SpecialOffersController), ex, message: "GetFilterAgeWiseData -See More- Bind Age wise Data");
														//}
														#endregion

														NestedItems.Add(nested);
													}

													items.NestedItems = NestedItems;

												}

												if (items.ItemId > 0)
												{
													ReturnList.Add(items);
												}
											}
										}
										//}
									}
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(SpecialOffersController), ex, message: "GetFilterAgeWiseData - Bind teacher wise Data");
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SpecialOffersController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");
			}

			return ReturnList;
		}

		public List<SpecialOfferItems> GetWorksheetSingle(WorksheetInput input)
		{
			//int _firstTimeDisplayWorksheetRemaining = firstTimeDisplayWorksheet;
			List<SpecialOfferItems> worksheetSingle = new List<SpecialOfferItems>();
			//_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
			if (input.worksheetId > 0)
			{
				worksheetSingle = Umbraco?.Content(input.worksheetId)?.DescendantsOrSelf()?.OfType<SpecialOfferItems>()?
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

		public List<SpecialOfferItems> GetWorksheetsByDays(NameListItem age, SpecialOffersRoot worksheetRoot, int firstTimeDisplayWorksheet, WorksheetInput input)
		{
			//int _firstTimeDisplayWorksheetRemaining = firstTimeDisplayWorksheet;
			List<SpecialOfferItems> worksheetGetDaysWise = new List<SpecialOfferItems>();

			worksheetGetDaysWise = worksheetRoot?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "specialOfferAge")?.OfType<SpecialOfferAge>()?
									.Where(w => Umbraco?.Content(w.ClassName.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == age.ItemValue)?
									.FirstOrDefault()?.Children?.OfType<SpecialOfferItems>()?.ToList();

			//items.totalItems = worksheetGetDaysWise.Count;

			worksheetGetDaysWise = worksheetGetDaysWise.Skip((input.currentPage - 1) * (worksheetRoot.NoOfDisplayWorksheet)).Take(worksheetRoot.NoOfDisplayWorksheet).ToList();

			return worksheetGetDaysWise;
		}

		[HttpPost]
		public ActionResult GetWorkSheetById(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetItems model = new WorkSheetItems();
				model = GetWorkSheetDetailsById(input);

				return PartialView("/Views/Partials/Worksheets/_specialofferProgramSingle.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();
			}
			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		public WorkSheetItems GetWorkSheetDetailsById(WorksheetInput input)
		{
			WorkSheetItems model = new WorkSheetItems();
			try
			{

				string vUserSubscription = String.Empty;
				string recommendedTitle = String.Empty;

				try
				{
					
					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);


					SpecialOfferItems worksheet;
					worksheet = Umbraco?.Content(input.FilterId).DescendantsOrSelf().OfType<SpecialOfferItems>().FirstOrDefault();
					if (worksheet != null)
					{
						try
						{
							List<NestedItems> List = new List<NestedItems>();
							NestedItems nested = new NestedItems();
							string worksheetType = String.Empty;
							var PDF_File = worksheet?.UploadPdf;

							if (String.IsNullOrEmpty(worksheet?.UploadPreviewPdf))
								nested.PreviewPdf = worksheet?.UploadPdf;
							else
								nested.PreviewPdf = worksheet?.UploadPreviewPdf;

							//bitlyLink = worksheetItems?.BitlyLink;

							var desktopmedia = worksheet?.DesktopImage;
							var desktopmediaNextgen = worksheet?.DesktopNextGenImage;
							var mobilemedia = worksheet?.MobileImage;
							var mobilemediaNextgen = worksheet?.MobileNextGenImage;

							var agegroup = Umbraco.Content(worksheet?.Parent?.Parent?.DescendantsOrSelf()?
									.OfType<SpecialOfferAge>()?.FirstOrDefault()?.ClassName?.Udi)?
									.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault();

							//var subject = Umbraco.Content(worksheet?.Parent?.Parent?.DescendantsOrSelf()?
							//				.OfType<WorksheetCategory>()?.FirstOrDefault()?.CategoryName?.Udi)?
							//				.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault();

							//var agegroup = worksheet?.Parent?.Parent?.Value<NameListItem>("ageGroup");
							//var subject = worksheet?.Parent?.Value<NameListItem>("categoryName");

							//var AgeTitle = worksheet?.SelectAgeGroup;
							//var AgeTitleDesc = Umbraco?.Content(AgeTitle?.Udi);
							var ItemName = agegroup?.ItemName;
							if (!String.IsNullOrEmpty(ItemName))
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

							//if (!String.IsNullOrEmpty(ItemName.ToString()))
							//	nested.Title = ItemName.ToString();

							//nested.NoOfDays = int.Parse(Umbraco?.Content(worksheet?.NoOfDays?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue);
							//nested.NoOfDaysName = worksheet?.NoOfDays?.Name;
							nested.Age = agegroup?.ItemValue;

							nested.Title = worksheet.Title;
							nested.SubTitle = worksheet.SubTitle;
							nested.Description = worksheet?.Description;
							nested.WorksheetId = worksheet.Id;
							//nested.WorksheetDetailsDescription = worksheetItems?.DescriptionPageContent;
							nested.WorksheetDetailsDescription = null;
							//nested.Volume = worksheet.SelectWeek?.Name.ToString();
							//nested.Category = string.Join(",", worksheetItems.Category?.Select(x => x.Name).ToList());
							//nested.Category = string.Join(",", worksheetItems?.Name);

							//nested.IsQuizWorksheet = worksheetItems.IsQuizWorksheet;
							//nested.CBSEContentIncluded = worksheetItems.CBsecontentIncluded;

							nested.IsEnabledForDetailsPage = worksheet.IsEnableForDetailsPage;
							//nested.WorksheetDetailsUrl = worksheet?.Url();

							if (!String.IsNullOrEmpty(worksheet.Value<string>("umbracoUrlAlias")))
								nested.WorksheetDetailsUrl = worksheet.Value<string>("umbracoUrlAlias");
							else
								nested.WorksheetDetailsUrl = worksheet?.Url();

							string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(worksheet?.Id.ToString()) + "&source=specialoffer";
							//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=BonusWorkSheet" + "&paid=" + "Paid";

							nested.subscriptionStatus = new SubscriptionStatus();
							string DownloadString = clsCommon.encrypto(worksheet.Id.ToString()) + "$" + "specialofferPrint";
							//string DownloadString = clsCommon.encrypto(WorkSheet.Id.ToString()) + "$" + "bonusworksheetPrint" + "$" + "Paid";

							nested.subscriptionStatus.DownloadString = DownloadString;
							nested.subscriptionStatus.DownloadUrl = downloadUrl;

							//if (worksheet.SelectWeek != null)
							//	nested.Category = worksheet?.Name;

							//nested.Subject = subject.SubjectName;
							//nested.Topic = worksheet?.SelectTopic?.Name.ToString();

							FestivalOffersHepler worksheets = new FestivalOffersHepler();
							nested = worksheets.GetSocialItemsAndSubscriptionDetailsForSpecialOfferWorkSheet(worksheet, input, nested);

							//WorksheetVideosHelper worksheetVideos = new WorksheetVideosHelper();
							//nested = worksheetVideos.GetSocialItemsAndSubscriptionDetailsForTeachersWorkSheet(worksheet, input, nested, null);

							List.Add(nested);

							//load more display till totalcount more than displayed on page
							model.NestedItems = List;

						}
						catch (Exception ex)
						{
							Logger.Error(reporting: typeof(SpecialOffersController), ex, message: "Worksheet - WorksheetDetailsData");
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(SpecialOffersController), ex, message: "Worksheet - WorksheetDetailsData");
				}

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SpecialOffersController), ex, message: "Worksheet - WorksheetDetailsData");
			}

			return model;
		}

		//public async Task<List<int>> GetRecentlyDownloadedTeachersWorkSheet(int userId, string Doctype)
		//{
		//	dbProxy _db = new dbProxy();
		//	List<int> recentlyDownloadTeachersWorkSheet = new List<int>();

		//	try
		//	{
		//		string culture = CultureName.GetCultureName();
		//		if (String.IsNullOrEmpty(culture))
		//			culture = "en-US";

		//		List<SetParameters> sp = new List<SetParameters>()
		//			{
		//				new SetParameters { ParameterName = "@UserId", Value = userId.ToString()},
		//				new SetParameters{ParameterName="@DocType",Value=Doctype}
		//			};
		//		recentlyDownloadTeachersWorkSheet = _db.GetDataMultiple("GetRecentlyDownloadedTeachersWorkSheet", recentlyDownloadTeachersWorkSheet, sp);
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(StructuredProgramController), ex, message: "GetRecentlyDownloadedTeachersWorkSheet");
		//	}

		//	return recentlyDownloadTeachersWorkSheet;
		//}


		//public List<TeacherProgramItems> GetWorksheetSingle(WorksheetInput input)
		//{
		//	//int _firstTimeDisplayWorksheetRemaining = firstTimeDisplayWorksheet;
		//	List<TeacherProgramItems> worksheetSingle = new List<TeacherProgramItems>();
		//	//_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
		//	if (input.worksheetId > 0)
		//	{
		//		worksheetSingle = Umbraco?.Content(input.worksheetId)?.DescendantsOrSelf()?.OfType<TeacherProgramItems>()?
		//			.Where(x => x.IsActive == true).ToList();

		//		//if (worksheetSingle != null)
		//		//{
		//		//	//SessionManagement.StoreInSession(SessionType.UserSelectCategoryOnWorksSheet, input.selectedSubject);

		//		//	if (input.IsCbseContent == "1")
		//		//		worksheetAllTopicWise.AddRange(worksheetGetTopicWise.Where(x => x.CBsecontentIncluded == true));
		//		//	else
		//		//		worksheetAllTopicWise.AddRange(worksheetGetTopicWise);
		//		//}

		//	}

		//	return worksheetSingle;
		//}

		//public List<TeacherProgramItems> GetWorksheetsByDays(NameListItem age, TeacherRoot worksheetRoot, int firstTimeDisplayWorksheet, WorksheetInput input)
		//{
		//	//int _firstTimeDisplayWorksheetRemaining = firstTimeDisplayWorksheet;
		//	List<TeacherProgramItems> worksheetGetDaysWise = new List<TeacherProgramItems>();

		//	worksheetGetDaysWise = worksheetRoot?.DescendantsOrSelf()?
		//							.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
		//							.Where(w => Umbraco?.Content(w.AgeGroup.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == age.ItemValue)?
		//							.FirstOrDefault()?.Children?.OfType<TeacherProgramItems>()?.ToList();

		//	//items.totalItems = worksheetGetDaysWise.Count;

		//	worksheetGetDaysWise = worksheetGetDaysWise.Skip((input.currentPage - 1) * (worksheetRoot.NoOfDisplayWorksheet)).Take(worksheetRoot.NoOfDisplayWorksheet).ToList();

		//	return worksheetGetDaysWise;
		//}

	}
}