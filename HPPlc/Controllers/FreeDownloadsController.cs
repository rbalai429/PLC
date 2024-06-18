using HPPlc.Models;
using HPPlc.Models.FestivalOffers;
using HPPlc.Models.WorkSheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Controllers
{
	public class FreeDownloadsController : SurfaceController
	{
		private readonly IVariationContextAccessor _variationContextAccessor;
		public FreeDownloadsController(IVariationContextAccessor variationContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
		}

		[HttpPost]
		public ActionResult GetFestivalWorksheetList(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetModel model = new WorkSheetModel();
				model = GetFestivalWorkSheetListData(input);

				string partialPath = String.Empty;
				if (!String.IsNullOrWhiteSpace(input.Mode) && input.Mode == "festival")
					partialPath = "/Views/Partials/FreeDownloads/_festivalOffersDetails.cshtml"; 
				else
					partialPath = "/Views/Partials/FreeDownloads/_freeDownloads.cshtml";

				return PartialView(partialPath, model);

			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(FreeDownloadsController), ex, message: "GetFestivalWorksheetList");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}
		public WorkSheetModel GetFestivalWorkSheetListData(WorksheetInput input)
		{
			WorkSheetModel model = new WorkSheetModel();
			try
			{
				var worksheetRoot = (List<FreeDownloadsTitle>)null;
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

				string trackTitle = String.Empty;
				if (!String.IsNullOrWhiteSpace(input.Mode) && input.Mode == "festival")
				{
					worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
							.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "festivalOfferRoot")?
							.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "freeDownloadsTitle")?
							.OfType<FreeDownloadsTitle>()?.Where(x => x.Id == int.Parse(input.FilterId)).ToList();

					trackTitle = "Festival Offer";
				}
				else
				{
					if (String.IsNullOrWhiteSpace(input.FilterType))
					{
						worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
							.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "freeDownloadsTitle")?
							.OfType<FreeDownloadsTitle>().Where(x => x.IsActive == true)?.ToList();
					}
					else
					{
						worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
														  .DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "freeDownloadsTitle")?
														  .OfType<FreeDownloadsTitle>().Where(x => x.Id == int.Parse(input.FilterId)).ToList();
					}

					trackTitle = "Free Downloads";
				}

				if (worksheetRoot != null && worksheetRoot.Count > 0)
				{
					_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

					int loadCount = 0;
					if (!String.IsNullOrWhiteSpace(input.FilterType))
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
						model.WorkSheets = GetFilterAgeWiseData(worksheetRoot, null, input);
						model.TrackTitle = trackTitle;

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
						Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetWorkSheetListData - sub block");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(FreeDownloadsController), ex, message: "GetWorkSheetListData");
			}

			return model;
		}

		
		public List<WorkSheetItems> GetFilterAgeWiseData(List<FreeDownloadsTitle> worksheetRoot, IEnumerable<NameListItem> ageMaster, WorksheetInput input)
		{
			List<WorkSheetItems> ReturnList = new List<WorkSheetItems>();
			try
			{
				int countOfTotalWorksheets = 0;
				//string bitlyLink = String.Empty;
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (worksheetRoot != null && worksheetRoot.Any())
				{
					foreach (var items in worksheetRoot)
					{
						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

						if (items != null)
						{
							string IsUserLoggedIn = SessionManagement.GetCurrentSession<string>(SessionType.IsLoggedIn);
							var WorkSheets = (List<FreeDownloadsContent>)null;
							if (String.IsNullOrWhiteSpace(IsUserLoggedIn) && String.IsNullOrWhiteSpace(input.FilterType) && input.FilterType == "no")
							{
								WorkSheets = items?.Children?.Where(x => x.ContentType.Alias == "freeDownloadsContent")?
									.OfType<FreeDownloadsContent>()?.Where(x => x.IsActive == true && x.IsAppliedForLoggedInUserOnly == false).ToList();
							}
							else if(!String.IsNullOrWhiteSpace(input.FilterType) && input.FilterType == "no")
							{
								WorkSheets = items?.Children?.Where(x => x.ContentType.Alias == "freeDownloadsContent")?
									.OfType<FreeDownloadsContent>()?.Where(x => x.IsActive == true).ToList();
							}
							else if (!String.IsNullOrWhiteSpace(input.FilterType) && input.FilterType == "yes")
							{
								WorkSheets = items?.Children?.Where(x => x.ContentType.Alias == "freeDownloadsContent")?
									.OfType<FreeDownloadsContent>()?.Where(x => x.IsActive == true && x.Id == input.worksheetId)?.ToList();
							}

							if (WorkSheets != null)
							{
								countOfTotalWorksheets = WorkSheets.Count();

								if (input.DisplayCount.Value > 0)
									WorkSheets = WorkSheets.Take(input.DisplayCount.Value).ToList();

								WorkSheetItems worksheetItems = new WorkSheetItems();
								worksheetItems.Title = items.Title;
								worksheetItems.Description = items.Description;
								worksheetItems.LoadMore = countOfTotalWorksheets;
								
								List<NestedItems> NestedItems = new List<NestedItems>();
								
								foreach (var WorkSheet in WorkSheets)
								{
									NestedItems nested = new NestedItems();
									var Image = WorkSheet?.ThumbnailMedia;
									string altText = Image?.Value<string>("altText");

									if (Image != null)
									{
										nested.AltText = altText;
										nested.ImagesSrc = Image.Url();
									}

									nested.Title = WorkSheet?.Title;
									nested.NextGenImage = WorkSheet?.ThumbnailNextGenMedia?.Url();
									nested.Description = WorkSheet?.Description;
									nested.IsAppliedforLoggedInUserOnly = WorkSheet.IsAppliedForLoggedInUserOnly;
									
									FestivalOffersHepler worksheet = new FestivalOffersHepler();
									nested = worksheet.GetSocialItemsAndSubscriptionDetailsForFetstivalWorkSheet(WorkSheet, input, nested);

									NestedItems.Add(nested);
								}

								worksheetItems.NestedItems = NestedItems;
								if (worksheetItems.NestedItems.Count() > 0)
									ReturnList.Add(worksheetItems);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(FreeDownloadsController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");
			}

			return ReturnList;
		}
		public bool CheckCategory(IEnumerable<Link> Cate, string CateName)
		{
			bool Result = false;
			if (CateName != null && !string.IsNullOrWhiteSpace(CateName))
			{
				string[] arry = CateName.Split(',');
				foreach (var item in Cate)
				{
					if (arry.Where(x => x.Contains(item.Name))?.Count() > 0)
						Result = true;
					else
						Result = !Result ? false : true;

				}
			}
			return Result;
		}

		public bool ContainsInt(string str, int value)
		{
			bool Result = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => int.Parse(x.Trim()))
				.Contains(value);
			return Result;
		}
	}
}