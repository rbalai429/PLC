using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HPPlc.Model;
using HPPlc.Models;
using HPPlc.Models.WhatsApp;
using HPPlc.Models.WorkSheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;
using static HPPlc.FilterConfig;

namespace HPPlc.Controllers
{
	//[OutputCache(CacheProfile = "ClientResourceCache")]
	public class WorkSheetController : SurfaceController
	{
		//public IContentService _contentService { get; set; }
		private readonly IVariationContextAccessor _variationContextAccessor;

		//public WorkSheetController(IContentService contentService)
		//{
		//	_contentService = contentService;
		//}
		public WorkSheetController(IVariationContextAccessor variationContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
		}

		public ActionResult Index()
		{
			//List<string> lst = new List<string>();
			//lst.Add("test");
			//lst.Add("test1");
			//lst.Add("test2");
			//WhatsAppHelper.CreateMessage("919999973729", TemplateTypeEnum.plc_free_week_1, lst);

			return View();
		}

		public async Task<ActionResult> DownloadData()
		{
			string redirectUrl = "";
			try
			{
				string documentUrl = "";
				string Age = "";
				string WorkSheetId = "";
				string source = "";
				bool? IsPaid = null;

				if (Request.QueryString["WID"] != null && Request.QueryString["source"] != null)
				{
					var downloadUrl = (dynamic)null;
					WorkSheetId = Request.QueryString["WID"].ToString();
					source = Request.QueryString["source"].ToString();

					if (Request.QueryString["paid"] != null && !string.IsNullOrEmpty(Request.QueryString["paid"].ToString()) && Request.QueryString["paid"].ToString() == "Paid")
					{
						IsPaid = true;
					}
					else if (Request.QueryString["paid"] != null && !string.IsNullOrEmpty(Request.QueryString["paid"].ToString()) && Request.QueryString["paid"].ToString() == "Free")
					{
						IsPaid = false;
					}

					WorkSheetId = clsCommon.Decrypto(WorkSheetId);
					//WorkSheetId = "25119";

					if (!String.IsNullOrWhiteSpace(WorkSheetId))
					{
						string culture = CultureName.GetCultureName().Replace("/", "");
						if (String.IsNullOrWhiteSpace(culture))
							culture = "en-US";

						_variationContextAccessor.VariationContext = new VariationContext(culture);
						if (!String.IsNullOrEmpty(source) && source.ToLower() == "video")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<Video>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "bonusvideoworksheet")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<Video>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "worksheet")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<WorksheetRoot>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "sfmc")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<WorksheetRoot>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "sfmcfreecontent")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf()?.OfType<FreeDownloadsContent>()?.Where(x => x.Id.ToString() == WorkSheetId)?.FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "sfmcpaid")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf()?.Where(x => x?.ContentType?.Alias == "worksheetRoot")?.OfType<WorksheetRoot>()?.FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "whatsapp")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<WorksheetRoot>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "sp365d")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<SpecialDaysItems>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "worksheetfestival")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<FreeDownloadsContent>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "bonusworksheet")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<StructureProgramItems>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "teachersworksheet")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<TeacherProgramItems>().FirstOrDefault();
						else if (!String.IsNullOrEmpty(source) && source.ToLower() == "specialoffer")
							downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<SpecialOfferItems>().FirstOrDefault();

						if (downloadUrl != null)
						{
							if (!String.IsNullOrEmpty(source) && source.ToLower() == "sp365d")
								documentUrl = downloadUrl?.UploadDocument;
							else
								documentUrl = downloadUrl?.UploadPdf;

							if (!String.IsNullOrEmpty(source) && source.ToLower() != "worksheetfestival" && source.ToLower() != "whatsapp" && source.ToLower() != "sfmcfreecontent" && source.ToLower() != "sp365d" && source.ToLower() != "bonusworksheet" && source.ToLower() != "teachersworksheet" && source.ToLower() != "specialoffer")
								Age = downloadUrl?.AgeTitle.Name;
							else if (!String.IsNullOrEmpty(source) && source.ToLower() == "specialoffer")
								Age = downloadUrl?.Parent?.ClassName?.Name;
							else if (!String.IsNullOrEmpty(source) && source.ToLower() == "teachersworksheet")
								Age = downloadUrl?.Parent?.AgeGroup?.Name;
							else if (source.ToLower() == "bonusworksheet")
							{
								var SelectedAgeValues = (IEnumerable<Link>)downloadUrl.SelectAgeGroup;
								var SelectedAgeValuesUdi = SelectedAgeValues.Select(x => x.Udi);
								var selectedAgeContent = Umbraco.Content(SelectedAgeValuesUdi);
								Age = string.Join(",", selectedAgeContent?.Select(x => x.Value<string>("itemValue")).ToList());
							}
							//var AgeTitleDesc = Umbraco?.Content(downloadUrl?.AgeTitle?.Udi);
							//var AgeValue = AgeTitleDesc?.Value("itemValue");

							try
							{
								if (!String.IsNullOrWhiteSpace(source) && source.ToLower() == "worksheet")
								{
									if (downloadUrl?.IsSubscriptionWiseDocument)
									{
										LoggedIn loggedin = new LoggedIn();
										loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

										//Get All subscription detais for user
										GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
										List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
										getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

										if (getUserCurrentSubscription != null)
										{
											if (getUserCurrentSubscription.Count > 0 && getUserCurrentSubscription.Where(x => x.AgeGroup == Age).Any())
												UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.AgeGroup == Age && x.IsActive == 1)?.SingleOrDefault();
											else
												UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.Ranking == "1" && x.IsActive == 1)?.SingleOrDefault();

											if (UserCurrentSubscription != null)
											{
												//var data = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<WorksheetRoot>().FirstOrDefault();
												//var datas = data.DocumentAndSubscriptions;
												var SubscriptionsWiseDocument = downloadUrl?.DocumentAndSubscriptions;
												bool subscriptionWiseDoc = downloadUrl?.IsSubscriptionWiseDocument;

												if (SubscriptionsWiseDocument != null && subscriptionWiseDoc == true)
												{
													foreach (var documentNode in SubscriptionsWiseDocument)
													{
														if (documentNode.SelectSubscriptions != null)
														{
															string subscriptionNode = documentNode.SelectSubscriptions.Udi.ToString();
															if (!String.IsNullOrWhiteSpace(subscriptionNode))
															{
																bool SubscriptionDataExits = Umbraco?.Content(subscriptionNode)?.DescendantsOrSelf().OfType<Subscriptions>().FirstOrDefault().Ranking == UserCurrentSubscription.Ranking;
																if (SubscriptionDataExits)
																	documentUrl = documentNode.BrowseDocument.Url;
															}
														}
													}
												}
											}
										}
									}
								}
							}
							catch (Exception ex){
								Logger.Error(reporting: typeof(WorkSheetController), ex, message: "get download pdf");
							}

							//Download data
							if (downloadUrl != null)
							{
								if (!String.IsNullOrEmpty(source) && source.ToLower() != "worksheetfestival" && source.ToLower() != "whatsapp" && source.ToLower() != "sfmcfreecontent" && source.ToLower() != "sp365d" && source.ToLower() != "bonusworksheet" && source.ToLower() != "teachersworksheet" && source.ToLower() != "specialoffer")
									Age = downloadUrl?.AgeTitle.Name;
								else if (!String.IsNullOrEmpty(source) && source.ToLower() == "teachersworksheet")
									Age = downloadUrl?.Parent?.AgeGroup?.Name;
								else if (!String.IsNullOrEmpty(source) && source.ToLower() == "specialoffer")
									Age = downloadUrl?.Parent?.ClassName?.Name;
								else if (source.ToLower() == "bonusworksheet")
								{
									var SelectedAgeValues = (IEnumerable<Link>)downloadUrl.SelectAgeGroup;
									var SelectedAgeValuesUdi = SelectedAgeValues.Select(x => x.Udi);
									var selectedAgeContent = Umbraco.Content(SelectedAgeValuesUdi);
									Age = string.Join(",", selectedAgeContent?.Select(x => x.Value<string>("itemValue")).ToList());
								}

								string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
								string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
								string BucketName = ConfigurationManager.AppSettings["BucketFileSystem:BucketName"].ToString();

								int RefUserId = 0;
								string UserUniqueId = String.Empty;
								string CultureInfo = CultureName.GetCultureName();

								if (!String.IsNullOrWhiteSpace(source) && source == "sp365d")
								{
									UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
								}
								else {
									RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
								}

								GetStatus entryStatus = new GetStatus();
								entryStatus = InsertDownloadPrint(CultureInfo, RefUserId, Age, WorkSheetId, documentUrl, source, UserUniqueId, IsPaid);
								//entryStatus = InsertDownloadPrint(CultureInfo, RefUserId, Age, WorkSheetId, documentUrl, source, UserUniqueId);
								if (entryStatus.returnStatus == "Success")
								{
									string fullFileName = String.Empty;
									string[] temp = documentUrl.Split('/');
									string fileName = temp[temp.Length - 1];
									fileName = AntiXssEncoder.HtmlEncode(fileName, true);
									if (temp.Length >= 5)
										fullFileName = temp[3] + "/" + temp[4] + "/" + fileName;
									else if (temp.Length >= 3)
										fullFileName = temp[1] + "/" + temp[2] + "/" + fileName;
									else
										fullFileName = fileName;

									try
									{
										var credentials = new BasicAWSCredentials(AWSAccessKey, AWSSecretKey);
										var config = new AmazonS3Config
										{
											RegionEndpoint = Amazon.RegionEndpoint.APSouth1
										};

										var client = new AmazonS3Client(credentials, config);
										var fileTransferUtility = new TransferUtility(client);

										var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
										{
											BucketName = BucketName,
											Key = fullFileName
										});

										if (objectResponse.ResponseStream == null)
										{
											//return NotFound();
										}

										//download session remove
										SessionManagement.DeleteFromSession(SessionType.WorksheetDownloadUrl);

										//string rdrctVal = SessionManagement.GetCurrentSession<string>(SessionType.WorksheetDownloadUrl);
										//if (!String.IsNullOrWhiteSpace(rdrctVal))
										//{
										//	SessionManagement.DeleteFromSession(SessionType.WorksheetDownloadUrl);
										//	redirectUrl = "/worksheet-download/";
										//}
										//else
										//{
										//	redirectUrl = "/";
										//}


										return File(objectResponse.ResponseStream, objectResponse.Headers.ContentType, fileName);
									}
									catch (AmazonS3Exception amazonS3Exception)
									{
										if (amazonS3Exception.ErrorCode != null
											&& (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity") || amazonS3Exception.ErrorCode.Equals("NoSuchKey")))
										{
											Logger.Error(reporting: typeof(WorkSheetController), null, message: "Check the provided AWS Credentials.");
											//throw new Exception("Check the provided AWS Credentials.");
										}
										else
										{
											Logger.Error(reporting: typeof(WorkSheetController), null, message: amazonS3Exception.Message);
											//throw new Exception("Error occurred: " + amazonS3Exception.Message);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Download Data");
			}

			return Redirect("/");
		}



		//public async Task<ActionResult> DownloadData()
		//{
		//	try
		//	{
		//		string url = "";
		//		string Age = "";
		//		string WorkSheetId = "";
		//		string source = "";

		//		if (Request.QueryString["WID"] != null && Request.QueryString["source"] != null)
		//		{
		//			var downloadUrl = (dynamic)null;
		//			WorkSheetId = Request.QueryString["WID"].ToString();
		//			source = Request.QueryString["source"].ToString();

		//			WorkSheetId = clsCommon.Decrypto(WorkSheetId);

		//			if (!String.IsNullOrEmpty(source) && source.ToLower() == "video")
		//				downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<Video>().FirstOrDefault();
		//			else
		//				downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<WorksheetRoot>().FirstOrDefault();

		//			if (!String.IsNullOrEmpty(source) && source.ToLower() == "worksheetfestival")
		//				downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<FestivalOffersItems>().FirstOrDefault();

		//			if (downloadUrl != null)
		//			{
		//				url = downloadUrl?.UploadPdf;
		//				Age = downloadUrl?.AgeTitle.Name;

		//				if (!String.IsNullOrEmpty(url))
		//				{
		//					string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
		//					string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
		//					string BucketName = ConfigurationManager.AppSettings["BucketFileSystem:BucketName"].ToString();

		//					int RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
		//					string CultureInfo = CultureName.GetCultureName();

		//					GetStatus entryStatus = new GetStatus();
		//					entryStatus = InsertDownloadPrint(CultureInfo, RefUserId, Age, WorkSheetId, url, source);
		//					if (entryStatus.returnStatus == "Success")
		//					{
		//						string fullFileName = String.Empty;
		//						string[] temp = url.Split('/');
		//						string fileName = temp[temp.Length - 1];
		//						fileName = AntiXssEncoder.HtmlEncode(fileName, true);
		//						if (temp.Length >= 5)
		//							fullFileName = temp[3] + "/" + temp[4] + "/" + fileName;
		//						else
		//							fullFileName = fileName;

		//						try
		//						{
		//							var credentials = new BasicAWSCredentials(AWSAccessKey, AWSSecretKey);
		//							var config = new AmazonS3Config
		//							{
		//								RegionEndpoint = Amazon.RegionEndpoint.APSouth1
		//							};

		//							var client = new AmazonS3Client(credentials, config);
		//							var fileTransferUtility = new TransferUtility(client);

		//							var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
		//							{
		//								BucketName = BucketName,
		//								Key = fullFileName
		//							});

		//							if (objectResponse.ResponseStream == null)
		//							{
		//								//return NotFound();
		//							}

		//							return File(objectResponse.ResponseStream, objectResponse.Headers.ContentType, fileName);
		//						}
		//						catch (AmazonS3Exception amazonS3Exception)
		//						{
		//							if (amazonS3Exception.ErrorCode != null
		//								&& (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
		//							{
		//								Logger.Error(reporting: typeof(WorkSheetController), null, message: "Check the provided AWS Credentials.");
		//								//throw new Exception("Check the provided AWS Credentials.");
		//							}
		//							else
		//							{
		//								Logger.Error(reporting: typeof(WorkSheetController), null, message: amazonS3Exception.Message);
		//								//throw new Exception("Error occurred: " + amazonS3Exception.Message);
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Download Data");
		//	}

		//	return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
		//}


		public async Task<string> WorksheetPrint(string WorkSheetId, string source, string Type, bool? IsPaid)
		{
			var downloadUrl = (dynamic)null;
			string docBase64 = String.Empty;

			//WorkSheetId = Request.QueryString["WID"].ToString();
			//source = Request.QueryString["source"].ToString();

			string culture = CultureName.GetCultureName().Replace("/", "");
			if (String.IsNullOrWhiteSpace(culture))
				culture = "en-US";

			_variationContextAccessor.VariationContext = new VariationContext(culture);

			if (!String.IsNullOrWhiteSpace(WorkSheetId))
				WorkSheetId = clsCommon.Decrypto(WorkSheetId);

			if (!String.IsNullOrWhiteSpace(WorkSheetId))
			{
				if (!String.IsNullOrEmpty(source) && source.ToLower() == "video")
					downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<Video>().FirstOrDefault();
				else if (!String.IsNullOrEmpty(source) && source.ToLower() == "bonusvideoworksheetprint")
					downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<Video>().FirstOrDefault();
				else if (!String.IsNullOrEmpty(source) && source.ToLower() == "print")//free downloads
					downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<FreeDownloadsContent>().FirstOrDefault();
				else if (!String.IsNullOrEmpty(source) && source.ToLower() == "sp365d")//plan 365 downloads
					downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<SpecialDaysItems>().FirstOrDefault();
				else if (!String.IsNullOrEmpty(source) && source.ToLower() == "bonusworksheetprint")//bonus worksheet
					downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<StructureProgramItems>().FirstOrDefault();
				else if (!String.IsNullOrEmpty(source) && source.ToLower() == "teachesworksheetprint")//bonus worksheet
					downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<TeacherProgramItems>().FirstOrDefault();
				else if (!String.IsNullOrEmpty(source) && source.ToLower() == "specialofferprint")
					downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<SpecialOfferItems>().FirstOrDefault();

				else
					downloadUrl = Umbraco?.Content(WorkSheetId)?.DescendantsOrSelf().OfType<WorksheetRoot>().FirstOrDefault();


				if (downloadUrl != null)
				{
					string documentUrl = String.Empty;
					if (!String.IsNullOrEmpty(source) && source.ToLower() == "sp365d")
						documentUrl = downloadUrl?.UploadDocument;
					else
						documentUrl = downloadUrl?.UploadPdf;

					string Age = String.Empty;
					if (!String.IsNullOrEmpty(source) && (source.ToLower() != "print" && source.ToLower() != "sp365d" && source.ToLower() != "bonusworksheetprint" && source.ToLower() != "teachesworksheetprint" && source.ToLower() != "specialofferprint"))
						Age = downloadUrl?.AgeTitle.Name;
					else if (!String.IsNullOrEmpty(source) && source.ToLower() == "teachesworksheetprint")
						Age = downloadUrl?.Parent?.AgeGroup?.Name;
					else if (!String.IsNullOrEmpty(source) && source.ToLower() == "specialofferprint")
						Age = downloadUrl?.Parent?.ClassName?.Name;
					else if (!String.IsNullOrEmpty(source) && source.ToLower() == "bonusworksheetprint")
					{
						var SelectedAgeValues = (IEnumerable<Link>)downloadUrl.SelectAgeGroup;
						var SelectedAgeValuesUdi = SelectedAgeValues.Select(x => x.Udi);
						var selectedAgeContent = Umbraco.Content(SelectedAgeValuesUdi);
						Age = string.Join(",", selectedAgeContent?.Select(x => x.Value<string>("itemValue")).ToList());
					}

					int RefUserId = 0;
					string UserUniqueId = String.Empty;
					string CultureInfo = CultureName.GetCultureName();

					if (!String.IsNullOrWhiteSpace(source) && source == "sp365d")
						UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
					else
						RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

					GetStatus entryStatus = new GetStatus();
					entryStatus = InsertDownloadPrint(CultureInfo, RefUserId, Age, WorkSheetId, documentUrl, source + "print", UserUniqueId,IsPaid);
					if (entryStatus.returnStatus == "Success")
					{
						string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
						string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
						string BucketName = ConfigurationManager.AppSettings["BucketFileSystem:BucketName"].ToString();

						string fullFileName = String.Empty;
						string[] temp = documentUrl.Split('/');
						string fileName = temp[temp.Length - 1];
						fileName = AntiXssEncoder.HtmlEncode(fileName, true);
						if (temp.Length >= 5)
							fullFileName = temp[3] + "/" + temp[4] + "/" + fileName;
						else if (temp.Length >= 4)
							fullFileName = temp[1] + "/" + temp[2] + "/" + fileName;
						else
							fullFileName = fileName;

						try
						{
							var credentials = new BasicAWSCredentials(AWSAccessKey, AWSSecretKey);
							var config = new AmazonS3Config
							{
								RegionEndpoint = Amazon.RegionEndpoint.APSouth1
							};

							var client = new AmazonS3Client(credentials, config);
							var fileTransferUtility = new TransferUtility(client);

							var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
							{
								BucketName = BucketName,
								Key = fullFileName
							});

							if (objectResponse.HttpStatusCode == HttpStatusCode.OK)
							{
								byte[] bytes;
								using (var memoryStream = new MemoryStream())
								{
									objectResponse.ResponseStream.CopyTo(memoryStream);
									bytes = memoryStream.ToArray();
								}

								docBase64 = Convert.ToBase64String(bytes);
								//docBase64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
							}
						}
						catch (AmazonS3Exception amazonS3Exception)
						{
							if (amazonS3Exception.ErrorCode != null
								&& (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
							{
								Logger.Error(reporting: typeof(WorkSheetController), null, message: "Check the provided AWS Credentials.");
								//throw new Exception("Check the provided AWS Credentials.");
							}
							else
							{
								Logger.Error(reporting: typeof(WorkSheetController), null, message: amazonS3Exception.Message);
								//throw new Exception("Error occurred: " + amazonS3Exception.Message);
							}
						}
						//byte[] bytes;
						//GetObjectResponse objectResponse = new GetObjectResponse();
						//objectResponse = await GetS3BucketFile(documentUrl);

						//if (objectResponse.ResponseStream.Length > 0)
						//{
						//    using (var memoryStream = new MemoryStream())
						//    {
						//        objectResponse.ResponseStream.CopyTo(memoryStream);
						//        bytes = memoryStream.ToArray();
						//    }

						//    docBase64 = Convert.ToBase64String(bytes);
						//    //docBase64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
						//}
					}
				}
			}

			return docBase64;
		}

		public GetStatus InsertDownloadPrint(string CultureInfo, int RefUserId, string Age, string WorkSheetId, string WorkshhetPDFUrl, string vFrom,string UserUniqueId = "",bool? IsPaid = null)
		{
			dbProxy _db = new dbProxy();
			GetStatus entryStatus = new GetStatus();

			string culture = CultureName.GetCultureName();
			if (String.IsNullOrEmpty(culture))
				culture = "en-US";

			List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@CultureInfo", Value = culture },
						new SetParameters { ParameterName = "@RefUserId", Value = RefUserId.ToString()},
						new SetParameters { ParameterName = "@Age", Value = Age },
						new SetParameters { ParameterName = "@WorkSheetId", Value = WorkSheetId },
						new SetParameters { ParameterName = "@WorkshhetPDFUrl", Value = WorkshhetPDFUrl },
						new SetParameters { ParameterName = "@FromDestination", Value = vFrom },
						new SetParameters { ParameterName = "@UserUniqueId", Value = UserUniqueId },
						new SetParameters { ParameterName = "@StructuredProgramIsPaid", Value = IsPaid.ToString()}
					};

			entryStatus = _db.StoreData("INSERT_DOWNLOAD_PRINT_USERDATA", sp);

			return entryStatus;
		}

		public string AgeDetails(string CultureInfo)
		{

			StringBuilder sb = new StringBuilder();
			sb.Clear();
			_variationContextAccessor.VariationContext = new VariationContext(CultureInfo);
			var AgeNode = Umbraco.Content(1197);
			if (AgeNode != null)
			{
				foreach (var Item in AgeNode.Children)
				{
					sb.Append("<option value=" + Item.Name + " selected>" + Item.Value("itemName") + "</option>");
				}
			}
			return sb.ToString();

		}

		public string generateBitlyLink(string PDF_File, string bitlyLink, int worksheetid, string CultureInfo)
		{
			string newbitlylink = string.Empty;
			try
			{
				if (String.IsNullOrEmpty(bitlyLink.ToString()) && !(String.IsNullOrEmpty(PDF_File)))
				{
					GenerateBitlyLink bitLink = new GenerateBitlyLink();
					newbitlylink = bitLink.Shorten(PDF_File);

					//Update bitly link in CRM
					var content = Services.ContentService.GetById(worksheetid);
					content.SetValue("bitlyLink", newbitlylink, CultureInfo);
					Services.ContentService.SaveAndPublish(content);
				}
				else
				{
					newbitlylink = bitlyLink;
				}
			}
			catch { }

			return newbitlylink;
		}

		[HttpPost]
		public ActionResult GetWorksheetList(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetModel model = new WorkSheetModel();
				model = GetWorkSheetListData(input);

				if (input != null && !String.IsNullOrWhiteSpace(input.Mode) && input.Mode.ToLower() == "quiz")
				{
					model.Mode = input.Mode;
				}

				return PartialView("/Views/Partials/Worksheets/_workSheetsList.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetWorksheetList");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GetWorkSheetBySubject(WorksheetInput input)
		{
			Responce responce = new Responce();
			WorkSheetItems items = new WorkSheetItems();
			List<WorkSheetItems> subjectItems = new List<WorkSheetItems>();
			List<WorksheetClass> classes = null;
			WorkSheetModel model = new WorkSheetModel();

			try
			{
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(culture);
				if (input.Mode != null && input.Mode == "agewisesubject" && !String.IsNullOrWhiteSpace(input.selectedAgeGroup) && !String.IsNullOrWhiteSpace(input.selectedSubject))
				{
					var ageGroup = Umbraco.Content(input.selectedAgeGroup)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue;
					var subjectId = Umbraco.Content(input.selectedSubject)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue;

					if (!String.IsNullOrWhiteSpace(ageGroup) && !String.IsNullOrWhiteSpace(subjectId.ToString()))
					{
						classes = new List<WorksheetClass>();
						classes.Add(new WorksheetClass { ClassId = ageGroup });
						items = GetWorkSheetDetailsBySubject(input, classes, subjectId.ToString());
					}

					return PartialView("/Views/Partials/Worksheets/_workSheetsDetails.cshtml", items);
				}
				else if (input.Mode != null && input.Mode == "agewisesubjecttopic" && !String.IsNullOrWhiteSpace(input.selectedAgeGroup) && !String.IsNullOrWhiteSpace(input.selectedSubject) && !String.IsNullOrWhiteSpace(input.selectedTopics))
				{
					var ageGroup = input.selectedAgeGroup;
					var subjectId = input.selectedSubject;
					var topicId = input.selectedTopics;

					if (!String.IsNullOrWhiteSpace(ageGroup) && !String.IsNullOrWhiteSpace(subjectId.ToString()) && !String.IsNullOrWhiteSpace(topicId.ToString()))
					{
						classes = new List<WorksheetClass>();
						classes.Add(new WorksheetClass { ClassId = ageGroup });
						items = GetWorkSheetDetailsByTopic(input, classes);
					}

					return PartialView("/Views/Partials/Worksheets/_workSheetsDetails.cshtml", items);
				}
				else if (input.Mode != null && input.Mode == "subject" && !String.IsNullOrWhiteSpace(input.selectedSubject))
				{
					model = GetWorkSheetListData(input);

					return PartialView("/Views/Partials/Worksheets/_workSheetsList.cshtml", model);
				}
				else if (input.Mode != null && input.Mode == "topics" && !String.IsNullOrWhiteSpace(input.selectedTopics))
				{
					model = GetWorkSheetListData(input);

					return PartialView("/Views/Partials/Worksheets/_workSheetsList.cshtml", model);
				}
				else if (input.Mode != null && input.Mode == "bysubjects")
				{
					try
					{
						var root = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?
								   .FirstOrDefault();

						input.DisplayCount = root?.Children?.Where(x => x.ContentType.Alias == "worksheetNode")?
								   .FirstOrDefault()?.Value<int>("firstTimeDisplayWorksheet");

						var AllClasses = root?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>();

						classes = new List<WorksheetClass>();
						if (AllClasses != null && AllClasses.Any())
						{
							foreach (var cls in AllClasses)
							{
								classes.Add(new WorksheetClass { ClassId = cls.AgeGroup.Name });
							}
						}

						List<WorksheetSubjects> subject = new List<WorksheetSubjects>();
						if (String.IsNullOrWhiteSpace(input.selectedSubject))
						{
							var allsubject = root?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "subjectsRoot")?
									.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjects").OfType<Subjects>().ToList();

							if (allsubject != null)
							{
								foreach (var sub in allsubject)
								{
									subject.Add(new WorksheetSubjects { SubjectId = sub.SubjectValue.ToString() });
								}
							}
						}
						else
						{
							string[] subjectArray = input.selectedSubject.Split(',');
							if (subjectArray != null)
							{
								for (int i = 0; i < subjectArray.Length; i++)
								{
									subject.Add(new WorksheetSubjects { SubjectId = subjectArray[i].ToString().Trim() });
								}
							}
						}

						if (subject != null && subject.Any())
						{
							foreach (var sbjt in subject)
							{
								try
								{
									items = GetWorkSheetDetailsBySubject(input, classes, sbjt.SubjectId.ToString());

									#region See More

									var mediaContent = root?.Children?.Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()?.FirstOrDefault();

									SeeMore seeMore = new SeeMore();

									string subjectUrl = root?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "subjectsRoot")?
									.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subjects")?
									.OfType<Subjects>()?.Where(c => c.SubjectValue.ToString() == sbjt.SubjectId)?.FirstOrDefault()?.Url();

									seeMore.VideoDetailsUrl = subjectUrl;

									var mediaUrl = mediaContent?.SeeMoreMedia;
									var nextGenMediaUrl = mediaContent.SeeMoreNextGen;

									if (mediaUrl != null)
									{
										if (nextGenMediaUrl != null)
											seeMore.NextGenMediaUrl = nextGenMediaUrl.Url();

										seeMore.MediaAltText = mediaUrl.Value("altText").ToString();
										seeMore.MediaUrl = mediaUrl.Url().ToString();
									}

									items.SeeMore = seeMore;
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData -See More- Bind Age wise Data");
								}
								#endregion


								subjectItems.Add(items);
							}
						}

						model.WorkSheets = subjectItems;
					}
					catch { }

					return PartialView("/Views/Partials/Worksheets/_workSheetsList.cshtml", model);
				}
				else if (input.Mode != null && input.Mode == "bytopics")
				{
					try
					{
						var root = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?
								   .FirstOrDefault();

						input.DisplayCount = root?.Children?.Where(x => x.ContentType.Alias == "worksheetNode")?
								   .FirstOrDefault()?.Value<int>("firstTimeDisplayWorksheet");

						var AllClasses = root?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>();

						classes = new List<WorksheetClass>();
						if (AllClasses != null && AllClasses.Any())
						{
							foreach (var cls in AllClasses)
							{
								classes.Add(new WorksheetClass { ClassId = cls.AgeGroup.Name });
							}
						}

						List<WorksheetTopics> topic = new List<WorksheetTopics>();
						if (String.IsNullOrWhiteSpace(input.selectedTopics))
						{
							var allTopics = root?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "topicsRoot")?
									.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics").OfType<Topics>().Take(input.DisplayCount.Value).ToList();

							if (allTopics != null)
							{
								foreach (var tpics in allTopics)
								{
									topic.Add(new WorksheetTopics { TopicsId = tpics.TopicValue.ToString() });
								}
							}
						}
						else
						{
							string[] subjectTopics = input.selectedTopics.Split(',');
							if (subjectTopics != null)
							{
								for (int i = 0; i < subjectTopics.Length; i++)
								{
									topic.Add(new WorksheetTopics { TopicsId = subjectTopics[i].ToString().Trim() });
								}
							}
						}

						if (topic != null && topic.Any())
						{
							foreach (var sbjt in topic)
							{
								try
								{
									input.selectedTopics = sbjt.TopicsId;
									items = GetWorkSheetDetailsByTopic(input, classes);

									#region See More

									var mediaContent = root?.Children?.Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()?.FirstOrDefault();

									SeeMore seeMore = new SeeMore();

									string topicUrl = root?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "topicsRoot")?
									.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "topics")?
									.OfType<Topics>()?.Where(c => c.TopicValue.ToString() == sbjt.TopicsId)?.FirstOrDefault()?.Url();

									seeMore.VideoDetailsUrl = topicUrl;

									var mediaUrl = mediaContent?.SeeMoreMedia;
									var nextGenMediaUrl = mediaContent.SeeMoreNextGen;

									if (mediaUrl != null)
									{
										if (nextGenMediaUrl != null)
											seeMore.NextGenMediaUrl = nextGenMediaUrl.Url();

										seeMore.MediaAltText = mediaUrl.Value("altText").ToString();
										seeMore.MediaUrl = mediaUrl.Url().ToString();
									}

									items.SeeMore = seeMore;
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData -See More- Bind Age wise Data");
								}
								#endregion


								subjectItems.Add(items);
							}
						}

						model.WorkSheets = subjectItems;
					}
					catch { }

					return PartialView("/Views/Partials/Worksheets/_workSheetsList.cshtml", model);
				}

			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetWorksheetList");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GetSingleWorkSheet(WorksheetInput InputWorksheet)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetModel model = new WorkSheetModel();
				model = GetWorkSheetListData(InputWorksheet);

				return PartialView("/Views/Partials/Worksheets/_workSheetSingle.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetSingleWorkSheet");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public ActionResult GetRelatedWorkSheetDetails(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetModel model = new WorkSheetModel();
				model = GetWorkSheetListData(input);

				model.Mode = input?.Mode;

				return PartialView("/Views/Partials/Worksheets/_workSheetsList.cshtml", model);
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetWorksheetList");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}


		public WorkSheetModel GetWorkSheetListData(WorksheetInput input)
		{
			WorkSheetModel model = new WorkSheetModel();
			try
			{
				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
				var worksheetRoot = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
									.Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>().FirstOrDefault();

				if (worksheetRoot != null)
				{
					bool filterSelected = false;
					List<string> list = new List<string> { input.selectedAgeGroup, input.selectedSubject, input.IsCbseContent };
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
						int toBeDisplayWorksheet = worksheetRoot.NoOfDisplayAgeGroupeWorksheet;
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
						model.WorkSheets = GetFilterAgeWiseData(worksheetRoot, agemaster, input, myagegroup);
						//model.LoadMore = ;
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetWorkSheetListData - sub block");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetWorkSheetListData");
			}

			return model;
		}

		public List<WorkSheetItems> GetFilterAgeWiseData(WorksheetNode worksheetRoot, IEnumerable<NameListItem> ageMaster, WorksheetInput input, List<SelectedAgeGroup> myagegroup)
		{
			List<WorkSheetItems> ReturnList = new List<WorkSheetItems>();
			try
			{
				string bitlyLink = String.Empty;
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrEmpty(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

				int firstTimeDisplayWorksheet = worksheetRoot.FirstTimeDisplayWorksheet;
				int startVolumeForReferral = worksheetRoot.StartVolumeForReferral;
				var mediaUrl = worksheetRoot?.SeeMoreMedia;
				var nextGenMediaUrl = worksheetRoot?.Value<IPublishedContent>("seeMoreNextGen");

				if (worksheetRoot != null)
				{
					if (ageMaster != null && ageMaster.Any())
					{
						_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
						//var WorkSheets = worksheetRoot?.Children?.Where(x => x.ContentType.Alias == "worksheet")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "worksheetRoot").OfType<WorksheetRoot>();
						//var WorkSheets = worksheetRoot?.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>();
						IEnumerable<WorksheetRoot> WorksheetItem = new List<WorksheetRoot>();

						var WorksheetAgeGroup = worksheetRoot?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")
									.OfType<WorksheetListingAgeWise>().ToList();

						if (WorksheetAgeGroup != null)
						{
							List<string> subjectsfilterAllId = null;
							foreach (var age in ageMaster)
							{
								_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
								WorkSheetItems items = new WorkSheetItems();

								try
								{
									if (!String.IsNullOrWhiteSpace(age.ItemValue))
									{
										_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
										List<WorksheetCategory> subjects = new List<WorksheetCategory>();
										List<TopicsName> topics = new List<TopicsName>();

										if (input.Mode != null && (input.Mode == "subject" || input.Mode == "related" || input.Mode == "single"))
										{
											subjects = WorksheetAgeGroup?.Where(x => Umbraco.Content(x.AgeGroup.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == age.ItemValue)?
												 .FirstOrDefault()?.Children.Where(k => k.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
												 .Where(x => x.IsActive == true && Umbraco?.Content(x.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue.ToString() == input.selectedSubject).ToList();
										}
										else if (input.Mode != null && input.Mode == "topics")
										{
											var subjectsForCurrentClass = WorksheetAgeGroup?.Where(x => Umbraco.Content(x.AgeGroup.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == age.ItemValue)?
												.FirstOrDefault()?.Children.Where(k => k.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?.Where(x => x.IsActive == true);
											if (subjectsForCurrentClass != null && subjectsForCurrentClass.Any())
											{
												List<TopicsName> currentTopics = new List<TopicsName>();
												foreach (var sub in subjectsForCurrentClass)
												{
													currentTopics = sub?.Children.Where(k => k.ContentType.Alias == "topicsName").OfType<TopicsName>()?
															.Where(x => x.IsActive == true && Umbraco?.Content(x?.TopicMapping?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault().TopicValue.ToString() == input.selectedTopics).ToList();

													topics.AddRange(currentTopics);
												}
											}
										}
										else if (!String.IsNullOrWhiteSpace(input.selectedSubject))
										{
											subjectsfilterAllId = new List<string>();
											if (!String.IsNullOrWhiteSpace(input.selectedSubject))
											{
												string[] subjectArray = input.selectedSubject.Split(',');
												if (subjectArray != null)
												{
													for (int i = 0; i < subjectArray.Length; i++)
													{
														subjectsfilterAllId.Add(subjectArray[i].ToString().Trim());
													}
												}
												//for (int i = 0; i < input.selectedSubject.Length; i++)
												//{
												//	subjectsfilterAllId.Add(input.selectedSubject[i].ToString());
												//}
											}

											if (subjectsfilterAllId != null && subjectsfilterAllId.Count > 0)
											{
												subjects = WorksheetAgeGroup?.Where(x => Umbraco.Content(x.AgeGroup.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == age.ItemValue)?
													 .FirstOrDefault()?.Children.Where(k => k.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
													 .Where(x => x.IsActive == true && subjectsfilterAllId.Contains(Umbraco?.Content(x?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue.ToString())).ToList();
											}
										}
										else
										{
											subjects = WorksheetAgeGroup?.Where(x => Umbraco.Content(x.AgeGroup.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == age.ItemValue)?
													 .FirstOrDefault()?.Children.Where(k => k.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
													 .Where(x => x.IsActive == true).ToList();
										}

										if ((subjects != null && subjects.Any()) || (topics != null && topics.Any()))
										{
											List<WorksheetRoot> worksheetAllSubjectWise = new List<WorksheetRoot>();
											if (input.Mode == "topics")
												worksheetAllSubjectWise = GetWorksheetsByTopic(topics, firstTimeDisplayWorksheet, input);
											else if (input.Mode == "single")
												worksheetAllSubjectWise = GetWorksheetSingle(input);
											else
												worksheetAllSubjectWise = GetWorksheetsBySubject(subjects, firstTimeDisplayWorksheet, input);

											if (worksheetAllSubjectWise != null && worksheetAllSubjectWise.Any() && worksheetAllSubjectWise.Count > 0)
											{
												//sorting
												try
												{
													if (worksheetAllSubjectWise != null && worksheetAllSubjectWise.Any() && worksheetAllSubjectWise.Count > 0)
													{
														WorksheetItem = worksheetAllSubjectWise?.
															OrderBy(x => Convert.ToInt32(Umbraco?.Content(x.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue)).Take(firstTimeDisplayWorksheet).ToList();
													}
												}
												catch { }

												if (WorksheetItem != null && WorksheetItem.Any())
												{
													if (WorksheetItem != null && WorksheetItem.Count() > 0 && WorksheetItem.Any())
													{
														items.WorksheetTitle = WorksheetAgeGroup?.Where(x => x.AgeGroup.Name == age.ItemValue)?.FirstOrDefault()?.Title;
														items.Title = age.ItemName;
														items.ItemId = age.Id;
														items.AgeValue = age.ItemValue;
														items.ViewAll = worksheetRoot.ViewAllTitle;

														List<NestedItems> NestedItems = new List<NestedItems>();
														foreach (var WorkSheet in WorksheetItem)
														{
															NestedItems nested = new NestedItems();
															var Image = WorkSheet?.UploadThumbnail;
															string altText = Image?.Value<string>("altText");
															var NextGenImage = WorkSheet?.NextGenImage;
															nested.Volume = WorkSheet?.SelectWeek?.Name.ToString();
															nested.Category = string.Join(",", WorkSheet?.Name);

															nested.Subject = WorkSheet?.SelectSubject?.Name.ToString();
															nested.Topic = WorkSheet?.Topic?.Name.ToString();

															//nested.Category = string.Join(",", WorkSheet.Category?.Select(x => x.Name).ToList());
															nested.IsQuizWorksheet = WorkSheet.IsQuizWorksheet;
															nested.CBSEContentIncluded = WorkSheet.CBsecontentIncluded;
															nested.PreviewPdf = WorkSheet?.UploadPreviewPdf;
															nested.IsEnabledForDetailsPage = WorkSheet.IsEnableForDetailsPage;
															nested.WorksheetDetailsUrl = WorkSheet?.Url();

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
															nested.WorksheetDetailsDescription = WorkSheet?.DescriptionPageContent;

															WorksheetVideosHelper worksheetVideos = new WorksheetVideosHelper();
															nested = worksheetVideos.GetSocialItemsAndSubscriptionDetailsForWorkSheet(WorkSheet, input, nested, myagegroup);
															#region See More
															try
															{
																SeeMore seeMore = new SeeMore();
																if (input.Mode != null && input.Mode == "subject")
																{
																	seeMore.VideoDetailsUrl = WorksheetAgeGroup?.Where(x => x?.AgeGroup?.Name == age?.ItemValue)?
																		.FirstOrDefault()?.Children?.OfType<WorksheetCategory>()?.Where(x => x?.CategoryName?.Name == WorkSheet?.SelectSubject?.Name.ToString())?.FirstOrDefault()?.Url();
																}
																else
																{
																	seeMore.VideoDetailsUrl = WorksheetAgeGroup?.Where(x => x.AgeGroup.Name == age.ItemValue)?.FirstOrDefault()?.Url();//WorksheetItem?.FirstOrDefault()?.Parent?.Url();
																}
																if (mediaUrl != null)
																{
																	if (nextGenMediaUrl != null)
																		seeMore.NextGenMediaUrl = nextGenMediaUrl.Url();
																	seeMore.MediaAltText = mediaUrl.Value("altText").ToString();
																	seeMore.MediaUrl = mediaUrl.Url().ToString();
																}

																NestedItems.Add(nested);
																items.SeeMore = seeMore;
															}
															catch (Exception ex)
															{
																Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData -See More- Bind Age wise Data");
															}
															#endregion
														}
														items.NestedItems = NestedItems;
													}

													if (items.ItemId > 0)
													{
														ReturnList.Add(items);
													}
												}
											}
										}
									}
								}
								catch { }
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");
			}

			return ReturnList;
		}

		public bool CheckCategory(Link Cate, string CateName)
		{
			bool Result = false;
			if (CateName != null && !string.IsNullOrWhiteSpace(CateName))
			{
				string[] arry = CateName.Split(',');

				if (arry.Where(x => x.Contains(Cate.Name))?.Count() > 0)
					Result = true;
				else
					Result = !Result ? false : true;
				//foreach (var item in Cate)
				//{
				//	if (arry.Where(x => x.Contains(item.Name))?.Count() > 0)
				//		Result = true;
				//	else
				//		Result = !Result ? false : true;

				//}
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

		[HttpPost]
		public ActionResult GetWorkSheetById(WorksheetInput input)
		{
			Responce responce = new Responce();
			try
			{
				WorkSheetItems model = new WorkSheetItems();
				model = GetWorkSheetDetailsById(input);

				return PartialView("/Views/Partials/Worksheets/_workSheetsDetails.cshtml", model);
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
									Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
				}
				if (countOfTotalWorksheets > input.DisplayCount)
					model.LoadMore = input.DisplayCount.Value;

				model.PageTitle = input?.FilterType;

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
			}

			return model;
		}



		//public List<WorkSheetItems> GetFilterAgeWiseDataByWorkSheetId(WorksheetNode worksheetRoot, IEnumerable<NameListItem> ageMaster, WorksheetInput input)
		//{
		//	List<WorkSheetItems> ReturnList = new List<WorkSheetItems>();
		//	try
		//	{
		//		string bitlyLink = String.Empty;
		//		string culture = CultureName.GetCultureName();
		//		_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);

		//		int firstTimeDisplayWorksheet = worksheetRoot.FirstTimeDisplayWorksheet;
		//		int startVolumeForReferral = worksheetRoot.StartVolumeForReferral;
		//		var mediaUrl = worksheetRoot?.SeeMoreMedia;
		//		var nextGenMediaUrl = worksheetRoot?.Value<IPublishedContent>("seeMoreNextGen");

		//		if (worksheetRoot != null)
		//		{
		//			if (ageMaster != null && ageMaster.Any())
		//			{
		//				var WorkSheets = worksheetRoot?.Children?.Where(x => x.ContentType.Alias == "worksheet")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "worksheetRoot").OfType<WorksheetRoot>();
		//				IEnumerable<WorksheetRoot> WorksheetRoot = new List<WorksheetRoot>();
		//				foreach (var age in ageMaster)
		//				{
		//					WorkSheetItems items = new WorkSheetItems();
		//					WorksheetRoot = WorkSheets.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue).Take(firstTimeDisplayWorksheet);
		//					//if (input.selectedVolume != null && !string.IsNullOrWhiteSpace(input.selectedVolume) && WorksheetRoot != null && WorksheetRoot.Any())
		//					//{
		//					//	WorksheetRoot = WorksheetRoot.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue && x.SelectWeek != null && x.SelectWeek.Udi != null && ContainsInt(input.selectedVolume, Convert.ToInt32(Umbraco?.Content(x.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue))).Take(firstTimeDisplayWorksheet);
		//					//}

		//					if (input.selectedSubject != null && !string.IsNullOrWhiteSpace(input.selectedSubject) && WorksheetRoot != null && WorksheetRoot.Any())
		//					{
		//						WorksheetRoot = WorksheetRoot.Where(x => x.IsActive && Umbraco?.Content(x.AgeTitle?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue == age.ItemValue && x.SelectSubject != null && x.SelectSubject?.Udi != null && input.selectedSubject.Contains(Umbraco?.Content(x.SelectSubject?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault().ItemValue)).Take(firstTimeDisplayWorksheet);
		//					}
		//					if (WorksheetRoot != null && WorksheetRoot.Count() > 0 && WorksheetRoot.Any())
		//					{
		//						items.Title = age.ItemName;
		//						items.ItemId = age.Id;
		//						items.Description = WorkSheets?.FirstOrDefault()?.Description;


		//						List<NestedItems> NestedItems = new List<NestedItems>();
		//						foreach (var WorkSheet in WorksheetRoot)
		//						{
		//							NestedItems nested = new NestedItems();
		//							nested.Volume = WorkSheet?.SelectWeek?.Name.ToString();
		//							nested.Category = WorkSheet?.SelectSubject?.Name.ToString();
		//							//nested.Category = string.Join(",", WorkSheet.Category?.Select(x => x.Name).ToList());

		//							//if (WorkSheet.Category != null && WorkSheet.Category.Count() > 0)
		//							//{
		//							//	foreach (var item in WorkSheet.Category)
		//							//	{
		//							//		nested.Category += item.Name + ",";
		//							//	}
		//							//	nested.Category.TrimEnd(',');
		//							//}
		//							nested.Age = WorkSheet?.AgeTitle?.Name.ToString();
		//							//var Image = WorkSheet?.UploadThumbnail;
		//							//string altText = Image?.Value<string>("altText");
		//							//var NextGenImage = WorkSheet?.NextGenImage;

		//							var desktopmedia = WorkSheet?.UploadThumbnail;
		//							var desktopmediaNextgen = WorkSheet?.NextGenImage;
		//							var mobilemedia = WorkSheet?.UploadMobileThumbnail;
		//							var mobilemediaNextgen = WorkSheet?.MobileNextGenImage;

		//							var AgeTitle = WorkSheet?.AgeTitle;
		//							var AgeTitleDesc = Umbraco?.Content(AgeTitle?.Udi);
		//							var ItemName = AgeTitleDesc?.Value("itemName");

		//							var weekTextRef = WorkSheet?.SelectWeek;
		//							int? volumeItemValue = Umbraco?.Content(weekTextRef?.Udi)?.Value<int>("itemValue");
		//							var volumeCSS = WorkSheet?.VolumeBackgroungCss;
		//							bool IsGuestUserSheet = WorkSheet.IsGuestUserSheet;

		//							var PDF_File = WorkSheet?.UploadPdf;
		//							bitlyLink = WorkSheet?.BitlyLink;
		//							string textFacebook = WorkSheet?.FacebookContent;
		//							string textWhatsApp = WorkSheet?.WhatsAppContent;
		//							string textMail = WorkSheet?.Value<string>("mailContent");
		//							//var textMailSubject = WorkSheet?.SharingText;
		//							var UpgradeButton = WorkSheet?.UpgradeButton;
		//							var Subscriptions = WorkSheet?.Subscription;
		//							//if (Image != null)
		//							//{
		//							//	if (NextGenImage != null)
		//							//	{
		//							//		nested.NextGenImage = NextGenImage.Url();
		//							//	}
		//							//	nested.AltText = altText;
		//							//	nested.ImagesSrc = Image.Url();
		//							//}

		//							if (desktopmediaNextgen != null)
		//								nested.NextGenImage = desktopmediaNextgen.Url();
		//							if (desktopmedia != null)
		//							{
		//								nested.AltText = desktopmedia.Value("altText").ToString();
		//								nested.ImagesSrc = desktopmedia.Url().ToString();
		//							}

		//							if (mobilemediaNextgen != null)
		//								nested.MobileNextGenImage = mobilemediaNextgen.Url();
		//							if (mobilemedia != null)
		//							{
		//								nested.MobileAltText = mobilemedia.Value("altText").ToString();
		//								nested.MobileImagesSrc = mobilemedia.Url().ToString();
		//							}
		//							nested.Description = WorkSheet?.Description;

		//							#region Social Share
		//							SocialItems socialItems = new SocialItems();
		//							LoggedIn loggedin = new LoggedIn();
		//							loggedin = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

		//							//Get All subscription detais for user
		//							GetUserCurrentSubscription UserCurrentSubscription = new GetUserCurrentSubscription();
		//							List<GetUserCurrentSubscription> getUserCurrentSubscription = new List<GetUserCurrentSubscription>();
		//							getUserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

		//							//find registered age group
		//							dbAccessClass db = new dbAccessClass();
		//							List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
		//							myagegroup = db.GetUserSelectedUserGroup();

		//							if (loggedin != null)
		//							{
		//								//Get User subscription based on age group
		//								if (loggedin?.UserTransactionType == "free")
		//									UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.Ranking == "1" && x.IsActive == 1)?.SingleOrDefault();
		//								else
		//									UserCurrentSubscription = getUserCurrentSubscription?.Where(x => x.AgeGroup == AgeTitle?.Name?.ToString() && x.IsActive == 1)?.SingleOrDefault();
		//							}
		//							string DownloadString = String.Empty;
		//							if (loggedin == null && IsGuestUserSheet == false)
		//								DownloadString = input.CultureInfo + "$" + ItemName + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";
		//							else if (loggedin == null && IsGuestUserSheet == true)
		//								DownloadString = input.CultureInfo + "$" + "0" + "$" + ItemName + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";
		//							else
		//								DownloadString = input.CultureInfo + "$" + loggedin.UserId.ToString() + "$" + ItemName + "$" + WorkSheet.Id + "$" + PDF_File + "$" + "Print";

		//							string loginUrl = culture + "/my-account/login?ref=u";

		//							//Check if user have only free subscription and have referral
		//							int tobeDisplayNoOfVolumes = 0;
		//							int tobeStartVolumeDisplay = 0;
		//							int? currentVolume = 0;
		//							if (loggedin != null && loggedin.UserTransactionType == "free" && UserCurrentSubscription.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0)
		//							{
		//								tobeDisplayNoOfVolumes = UserCurrentSubscription.ReferralRewardMonth;
		//								tobeStartVolumeDisplay = startVolumeForReferral;
		//								currentVolume = volumeItemValue;
		//							}

		//							try
		//							{
		//								//Share Icons activation
		//								if (Subscriptions != null && Subscriptions.Any())
		//								{
		//									if ((loggedin == null && IsGuestUserSheet == true) ||
		//										((loggedin != null && !String.IsNullOrEmpty(AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == AgeTitle?.Name?.ToString()))) && Umbraco.Content(Subscriptions?.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")) ||
		//										((loggedin != null && loggedin?.UserTransactionType == "paid" && UserCurrentSubscription != null && UserCurrentSubscription.DaysRemaining > 0 && Umbraco.Content(Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == UserCurrentSubscription?.Ranking))) ||
		//										(loggedin != null && loggedin?.UserTransactionType == "free" && UserCurrentSubscription?.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
		//									{
		//										// || (loggedin != null && getUserCurrentSubscription != null)
		//										if (!String.IsNullOrEmpty(textFacebook) || !String.IsNullOrEmpty(textWhatsApp) || !String.IsNullOrEmpty(textMail))
		//										{
		//											if (!String.IsNullOrEmpty(textFacebook))
		//												socialItems.FBShare = textFacebook;

		//											if (!String.IsNullOrEmpty(textWhatsApp))
		//												socialItems.WhatAppShare = textWhatsApp;

		//											if (!String.IsNullOrEmpty(textMail))
		//												socialItems.EmailShare = textMail;
		//										}
		//										nested.socialItems = socialItems;
		//									}
		//								}
		//							}
		//							catch (Exception ex)
		//							{
		//								Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData -Social share- Bind Age wise Data");
		//							}
		//							#endregion Social share

		//							#region Subscriptions Button Test
		//							SubscriptionStatus status = new SubscriptionStatus();
		//							try
		//							{
		//								if (Subscriptions != null && Subscriptions.Any())
		//								{
		//									if (culture == "/")
		//										culture = String.Empty;
		//									string subscriptionRanking = Umbraco.Content(Subscriptions.Select(x => x.Udi))?.ToList().OfType<Subscriptions>().First()?.Ranking.ToString().Trim();
		//									string subscriptionUrl = culture + "/subscription?subscptn=" + clsCommon.Encrypt(subscriptionRanking) + "&WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&age=" + clsCommon.Encrypt(AgeTitle?.Name.ToString());
		//									string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(WorkSheet.Id.ToString()) + "&source=Worksheet";
		//									status.DownloadUrl = downloadUrl;
		//									status.DownloadString = DownloadString;
		//									status.SubscriptionUrl = subscriptionUrl;

		//									if (loggedin == null && IsGuestUserSheet == true)
		//										status.Condition1 = true;
		//									//2. if user not loggedin and worksheet not mapped with isguestuser true
		//									else if (loggedin == null && IsGuestUserSheet == false)
		//										status.Condition2 = true;
		//									// 3.1 if User have free subscription and have referral
		//									else if (loggedin != null && loggedin?.UserTransactionType == "free" && !String.IsNullOrEmpty(AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == AgeTitle?.Name?.ToString()) && UserCurrentSubscription.ReferralRewardMonth > 0 && UserCurrentSubscription.DaysRemaining > 0 && (currentVolume <= ((tobeDisplayNoOfVolumes + tobeStartVolumeDisplay) - 1))))
		//										status.Condition3 = true;
		//									// 3.2 if User have loggedin and age group is same with worksheet and worksheet is free
		//									else if (loggedin != null && !String.IsNullOrEmpty(AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == AgeTitle?.Name?.ToString()) && Umbraco.Content(Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => o.Ranking == "1")))
		//										status.Condition4 = true;
		//									else if (loggedin != null && !String.IsNullOrEmpty(AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == AgeTitle?.Name?.ToString()) && Umbraco.Content(Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) == Convert.ToInt32(o.Ranking))))
		//										status.Condition5 = true;
		//									else if (loggedin != null && !String.IsNullOrEmpty(AgeTitle?.Name?.ToString()) && (myagegroup != null && myagegroup.Any(x => x.AgeGroup == AgeTitle?.Name?.ToString()) && Umbraco.Content(Subscriptions.Select(x => x.Udi)).ToList().OfType<Subscriptions>().Any(o => Convert.ToInt32(UserCurrentSubscription?.Ranking) < Convert.ToInt32(o.Ranking))))
		//										status.Condition6 = true;
		//									// 4.1 if User have loggedin and age group is not same with worksheet and user is free
		//									else if (loggedin != null && loggedin?.UserTransactionType == "free" && !String.IsNullOrEmpty(AgeTitle?.Name?.ToString()) && !(myagegroup != null && myagegroup.Any(x => x.AgeGroup == AgeTitle?.Name?.ToString())))
		//										status.Condition7 = true;
		//									else if (loggedin != null && loggedin?.UserTransactionType == "paid" && (UserCurrentSubscription != null && UserCurrentSubscription?.DaysRemaining == 0) && !String.IsNullOrEmpty(AgeTitle?.Name?.ToString()))
		//										status.Condition8 = true;
		//									//6. if User have loggedin and age group is not same with worksheet and worksheet is paid
		//									else if (loggedin != null && loggedin?.UserTransactionType == "paid" && !String.IsNullOrEmpty(AgeTitle?.Name?.ToString()) && (myagegroup != null && !myagegroup.Any(x => x.AgeGroup == AgeTitle?.Name?.ToString())))
		//										status.Condition9 = true;
		//									else
		//									{
		//									}
		//								}
		//								nested.subscriptionStatus = status;

		//							}
		//							catch (Exception ex)
		//							{
		//								Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData - Subscriptions Section");
		//							}
		//							#endregion Subscriptions Button Text End

		//							#region See More
		//							try
		//							{
		//								SeeMore seeMore = new SeeMore();
		//								seeMore.VideoDetailsUrl = "worksheets?type=age&typeid=" + clsCommon.encrypto(age.ItemValue);

		//								if (mediaUrl != null)
		//								{
		//									if (nextGenMediaUrl != null)
		//										seeMore.NextGenMediaUrl = nextGenMediaUrl.Url();
		//									seeMore.MediaAltText = mediaUrl.Value("altText").ToString();
		//									seeMore.MediaUrl = mediaUrl.Url().ToString();
		//								}

		//								NestedItems.Add(nested);
		//								items.SeeMore = seeMore;
		//							}
		//							catch (Exception ex)
		//							{
		//								Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData -See More- Bind Age wise Data");
		//							}
		//							#endregion
		//						}
		//						items.NestedItems = NestedItems;
		//					}

		//					ReturnList.Add(items);
		//				}

		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetFilterAgeWiseData - Bind Age wise Data");
		//	}
		//	return ReturnList;
		//}

		public WorkSheetItems GetWorkSheetDetailsBySubject(WorksheetInput input, List<WorksheetClass> ageGroup, string SubjectId)
		{
			WorkSheetItems model = new WorkSheetItems();
			try
			{
				//if (String.IsNullOrWhiteSpace(input.selectedSubject))
				//	input.selectedSubject = SessionManagement.GetCurrentSession<string>(SessionType.UserSelectCategoryOnWorksSheet);
				//else
				//	SessionManagement.StoreInSession(SessionType.UserSelectCategoryOnWorksSheet, SubjectId);

				//if (String.IsNullOrWhiteSpace(input.IsCbseContent))
				//	input.IsCbseContent = SessionManagement.GetCurrentSession<string>(SessionType.CbseContentChecked);
				//else
				//	SessionManagement.StoreInSession(SessionType.CbseContentChecked, input.IsCbseContent);

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
					if (String.IsNullOrWhiteSpace(culture))
						culture = "en-US";

					_variationContextAccessor.VariationContext = new VariationContext(culture);

					if (!String.IsNullOrEmpty(SubjectId))
					{
						//_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
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
						WorksheetCategory subject;

						foreach (var classname in ageGroup)
						{
							subject = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?
								.FirstOrDefault()?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
								.Where(x => x.AgeGroup.Name == classname.ClassId)?.FirstOrDefault()?
								//.Where(x => ageGroup.Any(c => c.ClassId == x.AgeGroup.Name))?.FirstOrDefault()?
								//.Where(x => Umbraco?.Content(x.AgeGroup.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == ageGroup)?.FirstOrDefault()?
								.Children.OfType<WorksheetCategory>()?.Where(c => Umbraco.Content(c?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == Int32.Parse(SubjectId) && c.IsActive == true).FirstOrDefault();

							if (subject != null)
								subjects.Add(subject);
						}

						if (subjects != null && subjects.Any())
						{
							model.WorksheetTitle = subjects?.FirstOrDefault()?.CategoryName?.Name;
							List<WorksheetRoot> worksheetAllSubjectWise = new List<WorksheetRoot>();
							worksheetAllSubjectWise = GetWorksheetsBySubject(subjects, 0, input);

							if (worksheetAllSubjectWise != null && worksheetAllSubjectWise.Count > 0)
							{
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

								countOfTotalWorksheets = worksheetAllSubjectWise.Count();
								model.Title = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<string>("title");
								model.Description = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<IHtmlString>("description");

								//model.Title = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<string>("title");
								//model.Description = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<IHtmlString>("description");

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
									Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
				}
				if (countOfTotalWorksheets > input.DisplayCount)
					model.LoadMore = input.DisplayCount.Value;

				model.PageTitle = input?.FilterType;

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
			}

			return model;
		}

		public WorkSheetItems GetWorkSheetDetailsByTopic(WorksheetInput input, List<WorksheetClass> ageGroup)
		{
			WorkSheetItems model = new WorkSheetItems();
			try
			{
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
					if (String.IsNullOrWhiteSpace(culture))
						culture = "en-US";

					_variationContextAccessor.VariationContext = new VariationContext(culture);


					//load more 
					if (input.DisplayCount == 0)
					{
						int? DisplayCount = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?
						   .FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "worksheetNode")?
						   .FirstOrDefault()?.Value<int>("firstTimeDisplayWorksheetOnDetails");

						input.DisplayCount = DisplayCount;
					}
					List<TopicsName> topics = new List<TopicsName>();
					TopicsName topic;

					foreach (var classname in ageGroup)
					{
						if (input.Mode == "bytopics")
						{
							var subjects = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
									.Where(c => c.AgeGroup.Name == classname.ClassId)?.FirstOrDefault()?.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "worksheetCategory")?
									.OfType<WorksheetCategory>().ToList();

							if (subjects != null && subjects.Any())
							{
								foreach (var sub in subjects)
								{
									var allTopics = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
											.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
											.Where(c => c.AgeGroup.Name == classname.ClassId)?.FirstOrDefault()?
											.Children.OfType<WorksheetCategory>()?.Where(c => Umbraco.Content(c?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?
											.FirstOrDefault()?.SubjectValue == Umbraco.Content(sub?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue && c.IsActive == true)?.FirstOrDefault()?
											.Children.OfType<TopicsName>()?.Where(b => b.IsActive == true && Umbraco.Content(b?.TopicMapping?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue.ToString() == input.selectedTopics).ToList();

									if (allTopics != null && allTopics.Count > 0)
									{
										topics.AddRange(allTopics);
									}
								}
							}
						}
						else
						{
							topic = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?
								.FirstOrDefault()?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
								.Where(x => x.AgeGroup.Name == classname.ClassId)?.FirstOrDefault()?
								.Children.OfType<WorksheetCategory>()?.Where(c => Umbraco.Content(c?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == Int32.Parse(input.selectedSubject) && c.IsActive == true).FirstOrDefault()?
								.Children.OfType<TopicsName>()?.Where(c => Umbraco.Content(c?.TopicMapping?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue == Int32.Parse(input.selectedTopics) && c.IsActive == true).FirstOrDefault();

							if (topic != null)
								topics.Add(topic);
						}
					}

					if (topics != null && topics.Any())
					{
						topics = topics.GroupBy(i => i.Name).Select(y => y.FirstOrDefault()).ToList();

						model.WorksheetTitle = topics?.FirstOrDefault()?.TopicMapping?.Name;
						List<WorksheetRoot> worksheetAllSubjectWise = new List<WorksheetRoot>();
						worksheetAllSubjectWise = GetWorksheetsByTopic(topics, 0, input);

						if (worksheetAllSubjectWise != null && worksheetAllSubjectWise.Count > 0)
						{
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

							model.ReadMore = topics?.FirstOrDefault()?.Parent?.Parent?.Parent?.Value<string>("readMore");
							int startVolumeForReferral = topics.FirstOrDefault().Parent.Parent.Parent.Value<int>("startVolumeForReferral");

							countOfTotalWorksheets = worksheetAllSubjectWise.Count();
							model.Title = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<string>("title");
							model.Description = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<IHtmlString>("description");

							//model.Title = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<string>("title");
							//model.Description = worksheetAllSubjectWise?.FirstOrDefault()?.Parent?.Value<IHtmlString>("description");

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
								Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
							}
						}
					}

				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
				}
				if (countOfTotalWorksheets > input.DisplayCount)
					model.LoadMore = input.DisplayCount.Value;

				model.PageTitle = input?.FilterType;

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "Worksheet - WorksheetDetailsData");
			}

			return model;
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

		public List<WorksheetRoot> GetWorksheetSingle(WorksheetInput input)
		{
			//int _firstTimeDisplayWorksheetRemaining = firstTimeDisplayWorksheet;
			List<WorksheetRoot> worksheetSingle = new List<WorksheetRoot>();
			//_variationContextAccessor.VariationContext = new VariationContext(input.CultureInfo);
			if (input.worksheetId > 0)
			{
				worksheetSingle = Umbraco?.Content(input.worksheetId)?.DescendantsOrSelf()?.OfType<WorksheetRoot>()?
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
		[ValidateAntiForgeryToken]
		public ActionResult GetStarted()
		{
			Responce responce = new Responce();
			try
			{
				dbAccessClass dbAccessClass = new dbAccessClass();
				GetStatus status = new GetStatus();
				status = dbAccessClass.GetStarted();
				
				return Json(status);
				
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.ToString();

				Logger.Error(reporting: typeof(WorkSheetController), ex, message: "GetStarted");
			}

			return Json(responce, JsonRequestBehavior.AllowGet);
		}

	}
}
