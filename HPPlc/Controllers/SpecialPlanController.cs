using HPPlc.Models;
using HPPlc.Models.WorkSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Controllers
{
	public class SpecialPlanController : SurfaceController
	{
		private readonly IVariationContextAccessor _variationContextAccessor;
		public SpecialPlanController(IVariationContextAccessor variationContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
		}

		[HttpPost]
		public ActionResult GetWorkSheetOfSpecialPlans(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetModel model = new WorkSheetModel();
				model = GetSpecial365WorkSheetListData(input);

				return PartialView("/Views/Partials/Worksheets/_SpecialPlan.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(FreeDownloadsController), ex, message: "GetWorkSheetOfSpecialPlans");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GetExploreWorkSheetOfSpecialPlans(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetModel model = new WorkSheetModel();
				model = GetSpecial365WorkSheetListData(input);

				return PartialView("/Views/Partials/Worksheets/_SpecialPlanExplore.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetWorkSheetOfSpecialPlans");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}
		public WorkSheetModel GetSpecial365WorkSheetListData(WorksheetInput input)
		{
			WorkSheetModel model = new WorkSheetModel();
			try
			{
				var worksheetRoot = (List<PlanAgeGroup>)null;
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

				worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
							.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "specialPlanRoot")?.OfType<SpecialPlanRoot>()?
							.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "planAgeGroup")?
							.OfType<PlanAgeGroup>()?.Where(c => c.IsActive == true).ToList();

				if (worksheetRoot != null && worksheetRoot.Any())
				{
					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

					int loadCount = 0;
					if (input.DisplayCount == 0)
					{
						loadCount = worksheetRoot.FirstOrDefault().Parent.Value<int>("firstTimeDisplay");
						loadCount = loadCount == 0 ? 12 : loadCount;

						if (input.DisplayCount == 0)
							input.DisplayCount = loadCount;
						else
							input.DisplayCount += loadCount;
					}
					else
						input.DisplayCount = 0;


					try
					{
						model.WorkSheets = GetFilterSpecialData(worksheetRoot, null, input);
						//model.TrackTitle = trackTitle;

						try
						{
							if (model != null && model.WorkSheets != null && model.WorkSheets.Count > 0 && input.DisplayCount > 0)
							{
								int totalWorksheets = model.WorkSheets.FirstOrDefault().LoadMore;
								int bindWorksheet = model.WorkSheets.FirstOrDefault().NestedItems.Count;

								if (totalWorksheets > bindWorksheet && bindWorksheet >= loadCount)
									model.LoadMore = (totalWorksheets - bindWorksheet);
								else
									model.LoadMore = 0;
							}
						}
						catch { }

					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetSpecial365WorkSheetListData - sub block");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetSpecial365WorkSheetListData");
			}

			return model;
		}

		public List<WorkSheetItems> GetFilterSpecialData(List<PlanAgeGroup> worksheets, IEnumerable<NameListItem> ageMaster, WorksheetInput input)
		{
			List<WorkSheetItems> ReturnList = new List<WorkSheetItems>();
			try
			{
				int countOfTotalWorksheets = 0;
				//string bitlyLink = String.Empty;
				string culture = CultureName.GetCultureName().Replace("/", "");
				List<SpecialDaysItems> specialDayaitems = new List<SpecialDaysItems>();
				SpecialDaysItems specialDayaitemsdownloads;

				if (worksheets != null && worksheets.Any())
				{
					foreach (var PlanAgeGroup in worksheets)
					{
						if (!String.IsNullOrWhiteSpace(input.Mode) && input.Mode == "mydownloads")
						{
							List<MyDownloadsData> mydownloads = new List<MyDownloadsData>();
							dbAccessClass dbAccessClass = new dbAccessClass();
							mydownloads = dbAccessClass.GetDownloads();

							if (mydownloads != null && mydownloads.Count > 0)
							{
								foreach (var worksheet in mydownloads)
								{
									specialDayaitemsdownloads = PlanAgeGroup?.Children?.Where(x => x.ContentType.Alias == "specialDaysItems")?
										.OfType<SpecialDaysItems>()?.Where(c => c.IsActive == true && c?.AgeGroup?.Name == PlanAgeGroup?.AgeGroup?.Name && worksheet.WorkSheetId.ToString() == c.Id.ToString()).FirstOrDefault();

									specialDayaitems.Add(specialDayaitemsdownloads);
								}
							}
						}
						else
						{
							specialDayaitems = PlanAgeGroup?.Children?.Where(x => x.ContentType.Alias == "specialDaysItems")?
									.OfType<SpecialDaysItems>()?.Where(c => c.IsActive == true && c?.AgeGroup?.Name == PlanAgeGroup?.AgeGroup?.Name).ToList();
						}

						if (specialDayaitems != null)
						{
							WorkSheetItems worksheetItems = new WorkSheetItems();
							worksheetItems.Title = worksheetItems.Title;
							worksheetItems.Description = worksheetItems.Description;
							worksheetItems.LoadMore = countOfTotalWorksheets;

							List<NestedItems> NestedItems = new List<NestedItems>();

							foreach (var items in specialDayaitems)
							{
								NestedItems nested = new NestedItems();
								var Image = items?.DesktopImage;
								string altText = Image?.Value<string>("altText");
								nested.NextGenImage = items?.DesktopNextGenImage?.Url();
								nested.PreviewPdf = items?.PreviewDocument;

								if (Image != null)
								{
									nested.AltText = altText;
									nested.ImagesSrc = Image.Url();
								}

								var mobImage = items?.MobileImage;
								string mobAltText = mobImage?.Value<string>("altText");
								nested.MobileNextGenImage = items?.MobileNextGenImage?.Url();

								if (mobImage != null)
								{
									nested.AltText = mobAltText;
									nested.MobileImagesSrc = mobImage.Url();
								}

								nested.Title = items?.Title;
								nested.Description = items?.Description;
								nested.WorksheetId = items.Id;
								nested.Topic = items.NoOfDays.Name.ToString();
								nested.NoOfDays = int.Parse(Umbraco?.Content(items?.NoOfDays?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue);
								nested.WorksheetDetailsUrl = clsCommon.encrypto(items.Id.ToString()) + "$" + "sp365d";

								//LockedImage
								nested.IsWorksheetLocked = items.IsWorksheetLocked;
								if (items.LockedDesktopImage != null)
								{
									nested.LockedDesktopImage = items.LockedDesktopImage.Url();
								}
								if (items.LockedDesktopNextGenImage != null)
								{
									nested.LockedDesktopNextGenImage = items.LockedDesktopNextGenImage.Url();
								}
								if (items.LockedMobileImage != null)
								{
									nested.LockedMobileImage = items.LockedMobileImage.Url();
								}
								if (items.LockedMobileNextGenImage != null)
								{
									nested.LockedNextGenMobileImage = items.LockedMobileNextGenImage.Url();
								}

								NestedItems.Add(nested);
							}

							worksheetItems.NestedItems = NestedItems;
							if (worksheetItems.NestedItems.Count() > 0)
								ReturnList.Add(worksheetItems);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(FreeDownloadsController), ex, message: "GetFilterSpecialData - Bind Age wise Data");
			}

			return ReturnList;
		}


		[HttpPost]
		public ActionResult GetMyDownloads(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetModel model = new WorkSheetModel();
				model = GetSpecial365WorkSheetListData(input);

				return PartialView("/Views/Partials/Worksheets/_MyDownloads.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(SpecialPlanController), ex, message: "GetMyDownloads");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}


	}
}