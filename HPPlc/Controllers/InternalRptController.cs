using ClosedXML.Excel;
using HPPlc.Models;
using HPPlc.Models.Bot;
using HPPlc.Models.HtmlRenderHelper;
using HPPlc.Models.Mailer;
using HPPlc.Models.WhatsApp;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
//using Microsoft.AspNetCore.Mvc;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;
//using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace HPPlc.Controllers
{
	public class InternalRptController : SurfaceController
	{
		string domainURL = ConfigurationManager.AppSettings["SiteUrl"].ToString();
		public HttpResponseMessage Lesson()
		{
			HttpResponseMessage result = null;

			try
			{
				List<WorksheetFormat> worksheetFormat = new List<WorksheetFormat>();

				string languagekey = "";
				string languageName = "";
				ILocalizationService ls = Services.LocalizationService;
				IEnumerable<ILanguage> languages = ls.GetAllLanguages();

				try
				{
					if (languages != null)
					{
						foreach (ILanguage language in languages)
						{
							CultureInfo cultureInfo = language.CultureInfo;
							languagekey = cultureInfo.Name.ToString();
							languageName = cultureInfo.DisplayName.ToString();

							var rootOfWorksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey))?.FirstOrDefault()?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault()?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?.ToList();


							if (rootOfWorksheet != null)
							{
								foreach (var classes in rootOfWorksheet)
								{
									var rootOfSubject = classes?.Children.Where(x => x.ContentType.Alias == "worksheetCategory" && x.Cultures.ContainsKey(languagekey))?
										.OfType<WorksheetCategory>()?.ToList();

									if (rootOfSubject != null)
									{
										foreach (var subj in rootOfSubject)
										{
											var worksheets = subj?.Children?.Where(x => x.ContentType.Alias == "worksheetRoot" && x.Cultures.ContainsKey(languagekey))?
																.OfType<WorksheetRoot>()?.ToList();

											if (worksheets != null)
											{
												foreach (var workst in worksheets)
												{
													int numberOfPages = 0;
													try
													{
														string pathOfPdf = workst?.UploadPdf;

														if (!String.IsNullOrEmpty(pathOfPdf))
														{
															if (!pathOfPdf.Contains("https://"))
																pathOfPdf = domainURL + pathOfPdf;

															PdfReader pdfReaderb = new PdfReader(pathOfPdf);
															numberOfPages = pdfReaderb.NumberOfPages;
														}
													}
													catch (Exception ex)
													{
														Logger.Error(reporting: typeof(InternalRptController), ex, message: "Lesson Data -pdf");
													}

													worksheetFormat.Add(new WorksheetFormat
													{
														WorksheetId = workst.Id,
														Languagename = languageName,
														ClassName = Umbraco.Content(workst?.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.AlternateClassName,
														SubjectName = Umbraco.Content(workst?.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectName,
														WeekName = Umbraco.Content(workst?.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemName,
														TopicName = Umbraco.Content(workst?.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicName,
														Title = workst.Title,
														Description = workst.Description,
														WorksheetLink = workst.Url(),
														CntOfPdfFilePage = numberOfPages.ToString(),
														DateOfCreation = workst.CreateDate.ToString("dddd, dd MMMM yyyy")
													});

													
												}
											}
										}
									}
								}
							}


						}



						try
						{
							dbNotificationAccess dbClass = new dbNotificationAccess();
							GetStatus status = Task.Run(() => dbClass.WorksheetPageCnt(worksheetFormat, "lesson")).Result;
						}
						catch (Exception ex)
						{
							Logger.Error(reporting: typeof(InternalRptController), ex, message: "Lesson Data - Save");
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(InternalRptController), ex, message: "Lesson Data");
				}

				try
				{
					//Export to excel
					if (worksheetFormat != null)
					{

						DataTable dataTable = new DataTable();
						dataTable = ToDataTable(worksheetFormat);

						ExportToExcel exportToExcel = new ExportToExcel();
						exportToExcel.DownloadExcelClosedXML("LessonPlanWorksheets", dataTable);
						//var workbook = new XLWorkbook();     //creates the workbook
						//var wsDetailedData = workbook.AddWorksheet("data"); //creates the worksheet with sheetname 'data'
						//wsDetailedData.Cell(1, 1).InsertTable(worksheetFormat); //inserts the data to cell A1 including default column name
						//workbook.SaveAs(@"D:\Lesson_Plan_Worksheets.xlsx");

						//List<clsTeacherPlan> List = new List<clsTeacherPlan>();
						//clsExpertHelper clsExpertHelper = new clsExpertHelper();
						//            //List = responce.Result as List<clsTeacherPlan>;
						//            byte[] bytes = clsExpertHelper.ListToExcel(worksheetFormat, "Lesson Plan Worksheets");
						//            result = Request Request.CreateResponse();
						//            result.Content = new ByteArrayContent(bytes);
						//            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
						//            result.Content.Headers.ContentDisposition.FileName = "TeacherData-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
						//            return result;
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(InternalRptController), ex, message: "Lesson Export");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(InternalRptController), ex, message: "Lesson");
			}

			return result;
		}

		public HttpResponseMessage Bonus()
		{
			HttpResponseMessage result = null;
			try
			{
				List<WorksheetFormat> worksheetFormat = new List<WorksheetFormat>();

				try
				{
					var rootOfWorksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?.ToList();


					if (rootOfWorksheet != null)
					{
						foreach (var classes in rootOfWorksheet)
						{
							var rootOfSubject = classes?.Children.Where(x => x.ContentType.Alias == "worksheetCategory")?
								.OfType<WorksheetCategory>()?.ToList();

							if (rootOfSubject != null)
							{
								foreach (var subj in rootOfSubject)
								{
									var worksheets = subj?.Children?.Where(x => x.ContentType.Alias == "structureProgramItems")?
														.OfType<StructureProgramItems>()?.ToList();

									if (worksheets != null)
									{
										foreach (var workst in worksheets)
										{
											int numberOfPages = 0;

											try
											{
												string pathOfPdf = workst?.UploadPdf;

												if (!String.IsNullOrEmpty(pathOfPdf))
												{
													if (!pathOfPdf.Contains("https://"))
														pathOfPdf = domainURL + pathOfPdf;

													PdfReader pdfReaderb = new PdfReader(pathOfPdf);
													numberOfPages = pdfReaderb.NumberOfPages;
												}
											}
											catch (Exception ex)
											{
												Logger.Error(reporting: typeof(InternalRptController), ex, message: "Bonus Data -pdf");
											}

											string WorksheetLinkUrl = String.Empty;
											if (!String.IsNullOrEmpty(workst.Value<string>("umbracoUrlAlias")))
												WorksheetLinkUrl = workst.Value<string>("umbracoUrlAlias");
											else
												WorksheetLinkUrl = workst?.Url();

											worksheetFormat.Add(new WorksheetFormat
											{
												WorksheetId = workst.Id,
												ClassName = Umbraco.Content(workst?.SelectAgeGroup?.FirstOrDefault()?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.AlternateClassName,
												SubjectName = Umbraco.Content(workst?.SelectSubject?.FirstOrDefault()?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectName,
												TopicName = Umbraco.Content(workst?.SelectTopic?.FirstOrDefault()?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicName,
												Title = workst.Title,
												Description = workst.Description,
												WorksheetLink = WorksheetLinkUrl,
												WorkshetIsPaid = workst?.IsPaid ?? false,
												CntOfPdfFilePage = numberOfPages.ToString(),
												DateOfCreation = workst.CreateDate.ToString("dddd, dd MMMM yyyy")
											});

											
										}
									}
								}
							}
						}


						try
						{
							dbNotificationAccess dbClass = new dbNotificationAccess();
							GetStatus status = Task.Run(() => dbClass.WorksheetPageCnt(worksheetFormat, "bonus")).Result;
						}
						catch (Exception ex)
						{
							Logger.Error(reporting: typeof(InternalRptController), ex, message: "Bonus Data - Save");
						}
					}

				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(InternalRptController), ex, message: "Bonus Data");
				}


				try
				{
					//Export to excel
					if (worksheetFormat != null)
					{
						DataTable dataTable = new DataTable();
						dataTable = ToDataTable(worksheetFormat);

						ExportToExcel exportToExcel = new ExportToExcel();
						exportToExcel.DownloadExcelClosedXML("BonusPlanWorksheets", dataTable);

					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(InternalRptController), ex, message: "Bonus Export");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(InternalRptController), ex, message: "Bonus");
			}
			return result;
		}

		public HttpResponseMessage Teachers()
		{
			HttpResponseMessage result = null;
			try
			{
				List<WorksheetFormat> worksheetFormat = new List<WorksheetFormat>();

				try
				{
					var rootOfWorksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "teacherRoot")?.FirstOrDefault()?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?.ToList();


					if (rootOfWorksheet != null)
					{
						foreach (var classes in rootOfWorksheet)
						{

							var worksheets = classes?.Children?.Where(x => x.ContentType.Alias == "teacherProgramItems")?
												.OfType<TeacherProgramItems>()?.ToList();

							if (worksheets != null)
							{
								foreach (var workst in worksheets)
								{
									int numberOfPages = 0;
									try
									{
										string pathOfPdf = workst?.UploadPdf;

										if (!String.IsNullOrEmpty(pathOfPdf))
										{
											if (!pathOfPdf.Contains("https://"))
												pathOfPdf = domainURL + pathOfPdf;

											PdfReader pdfReader = new PdfReader(pathOfPdf);
											numberOfPages = pdfReader.NumberOfPages;
										}
									}
									catch (Exception ex)
									{
										Logger.Error(reporting: typeof(InternalRptController), ex, message: "Teachers Data -pdf");
									}

									worksheetFormat.Add(new WorksheetFormat
									{
										WorksheetId = workst.Id,
										ClassName = Umbraco.Content(workst?.Parent?.DescendantsOrSelf()?.OfType<WorksheetListingAgeWise>()?.FirstOrDefault()?.AgeGroup?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().AlternateClassName,
										DaysName = Umbraco.Content(workst?.NoOfDays?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemName,
										Title = workst.Title,
										Description = workst.Description,
										WorksheetLink = workst.Url(),
										CntOfPdfFilePage = numberOfPages.ToString(),
										DateOfCreation = workst.CreateDate.ToString("dddd, dd MMMM yyyy")
									});

									
								}
							}
						}


						try
						{
							dbNotificationAccess dbClass = new dbNotificationAccess();
							GetStatus status = Task.Run(() => dbClass.WorksheetPageCnt(worksheetFormat, "teacher")).Result;
						}
						catch (Exception ex)
						{
							Logger.Error(reporting: typeof(InternalRptController), ex, message: "Teachers Data - Save");
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(InternalRptController), ex, message: "Teachers Data");
				}

				//Export to excel
				try
				{
					if (worksheetFormat != null)
					{

						DataTable dataTable = new DataTable();
						dataTable = ToDataTable(worksheetFormat);

						ExportToExcel exportToExcel = new ExportToExcel();
						exportToExcel.DownloadExcelClosedXML("TeachersPlanWorksheets", dataTable);

					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(InternalRptController), ex, message: "Teachers Export");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(InternalRptController), ex, message: "Teachers Root");
			}
			return result;
		}

		
		public DataTable ToDataTable<T>(List<T> items)
		{
			DataTable dataTable = new DataTable(typeof(T).Name);
			//Get all the properties
			PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo prop in Props)
			{
				//Setting column names as Property names
				dataTable.Columns.Add(prop.Name);
			}
			foreach (T item in items)
			{
				var values = new object[Props.Length];
				for (int i = 0; i < Props.Length; i++)
				{
					//inserting property values to datatable rows
					values[i] = Props[i].GetValue(item, null);
				}
				dataTable.Rows.Add(values);
			}
			//put a breakpoint here and check datatable
			return dataTable;
		}
	}
}



public class WorksheetFormat
{
	public int WorksheetId { get; set; }
	public string Languagename { get; set; }
	public string ClassName { get; set; }
	public string SubjectName { get; set; }
	public string TopicName { get; set; }
	public string WeekName { get; set; }
	public string DaysName { get; set; }

	public string Title { get; set; }
	public IHtmlString Description { get; set; }
	public string WorksheetLink { get; set; }
	public bool WorkshetIsPaid { get; set; }
	public string DateOfCreation { get; set; }
	public string CntOfPdfFilePage { get; set; }
}