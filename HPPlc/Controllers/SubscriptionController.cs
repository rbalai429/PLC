using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using HPPlc.Models;
using System.Configuration;
using System.Data;
using Umbraco.Web.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
using Umbraco.Core.Models.PublishedContent;
using DotNetIntegrationKit;
using System.Net;
using HPPlc.Models.Mailer;
using HPPlc.Model;
using HPPlc.Models.Constant;
using HPPlc.Models.PDFGenerator;
using System.IO;
using Umbraco.Web.Composing;
using HP_PLC_Doc;
using HP_PLC_Doc.Controllers;
using HPPlc.Models.S3Buckets;
using HPPlc.Models.HtmlRenderHelper;
using HPPlc.Models.HttpClientServices;
using HPPlc.Models.Coupon;
using Renci.SshNet;
using System.Web.Hosting;
using FluentFTP;
using HPPlc.Models.WhatsApp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Amazon;

namespace HPPlc.Controllers
{
	public class SubscriptionController : SurfaceController
	{
		string _userName, _userEmail, _userMobile = String.Empty;
		dbProxy _db = new dbProxy();
		string IsEnableSFMCCode = ConfigurationManager.AppSettings["IsEnableSFMCCode"].ToString();
		private readonly IVariationContextAccessor _variationContextAccessor;
		public SubscriptionController(IVariationContextAccessor variationContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
		}

		public SubscriptionController()
		{
		}

		public string pdfcheck()
		{

			string sdffddff = clsCommon.Encrypt("8882038811");

			//SendDataToSFMC sendDataToSFMCt = new SendDataToSFMC();
			//sendDataToSFMCt.PostDataSFMC(487105, "", "registrationInviteUser");

			//Email Encryption in SFMC format
			List<EncryptionDecryption> enc = new List<EncryptionDecryption>();
			dbAccessClass db = new dbAccessClass();
			enc = db.encDecGetData();
			if (enc != null && enc.Count > 0)
			{
				foreach (var item in enc)
				{
					string UserId_Enc = clsCommon.Encryptwithbase64Code(item.UserId.ToString());

					string decEmail = clsCommon.Decrypt(item.EncEmail);
					string decMobile = clsCommon.Decrypt(item.EncMobile);
					string decName = clsCommon.Decrypt(item.EncName);

					string Subscriber_Key = MD5HashPassword.CreateMD5Hash(decEmail.ToLower());

					db = new dbAccessClass();
					db.encDecData(item.UserId, UserId_Enc, Subscriber_Key, decEmail, decMobile, decName);
				}
			}

			string plana = clsCommon.Encryptwithbase64Code("plan=buy199");
			string planb = clsCommon.Encryptwithbase64Code("plan=buy899");
			string dff = clsCommon.Decrypto("6nVd72AZHRSbBTzv/p4T7Q==");
			string mob = clsCommon.Encrypt("9999271717");

			string mobddff = clsCommon.Decrypt("NwNV3y5NDl9e75zPFpE1EbQCdsuo3+3/RQTod7UAqDI=");

			string downloadUrl = "worksheet-download?d=" + clsCommon.Encryptwithbase64Code("WID=13825&UID=49") + "&jumpid=mb_in_oc_mk_ot_cm017595_aw_ot&utm_source=mobile_apps&utm_medium=other_social&utm_campaign=sep_0709&type=sp365d";
			string sdf = clsCommon.Encrypt("animesh.shah1998@gmail.com");
			string sdfhj = clsCommon.Encrypt("25795");

			string sdf_bd = clsCommon.Encrypt("25794");
			string sdfhj_bd = clsCommon.Encrypt("25795");

			string sdf_pr = clsCommon.Encrypt("25794");
			string sdfhj_pr = clsCommon.Encrypt("25795");

			string sdff = clsCommon.Decrypt("rWKDZlUnml5Olk1hjS7UUQ==");
			int day = 1;

			//LoggedIn loggedIn = new LoggedIn();
			//loggedIn = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

			//var notificationItem = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
			//			.Where(x => x.ContentType.Alias == "specialPlanNotification")?
			//				.OfType<SpecialPlanNotification>()?.Where(x => x.IsActive == true)?.FirstOrDefault();

			//string notificationCode = notificationItem?.NotificationMapping?.Where(c => Umbraco.Content(c?.SelectDay?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == day.ToString())?.FirstOrDefault()?.NotificationCode;
			//SpecialDaysItems worksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
			//											.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "specialPlanRoot")?.OfType<SpecialPlanRoot>()?
			//											.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "planAgeGroup")?
			//											.OfType<PlanAgeGroup>()?.Where(c => c.IsActive == true)?.FirstOrDefault()?
			//											.Children?.Where(c => c.ContentType.Alias == "specialDaysItems")?.OfType<SpecialDaysItems>()?
			//											.Where(v => v.IsActive == true && Umbraco.Content(v?.NoOfDays?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == day.ToString())?.FirstOrDefault();


			//string mobile = "9999973729";
			//if (!String.IsNullOrWhiteSpace(mobile) && loggedIn != null && loggedIn.ComWithWhatsApp.ToLower().Equals("y"))
			//{
			//	//mobile = "91" + mobile;
			//	if (!String.IsNullOrWhiteSpace(mobile) && (mobile.Substring(0, 3) == "+91" || mobile.Length == 10))
			//		mobile = "91" + mobile;

			//	string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
			//	WhatsAppDynamicValue lst = new WhatsAppDynamicValue();
			//	WhatsAppHelper whatsAppHelper = new WhatsAppHelper();

			//	if (notificationItem != null)
			//	{
			//		if (!String.IsNullOrWhiteSpace(notificationCode))
			//		{
			//			if (worksheet != null)
			//			{
			//				if (loggedIn != null)
			//				{
			//					if (!String.IsNullOrWhiteSpace(loggedIn.u_name))
			//						lst.Name = clsCommon.Decrypt(loggedIn.u_name);
			//				}

			//				if (worksheet != null)
			//				{
			//					if (worksheet?.WhatsAppShareBanner != null)
			//						lst.BannerUrl = worksheet.WhatsAppShareBanner.Url();
			//				}

			//				string downloadUrl = " " + domain + "worksheet-download?d=" + clsCommon.Encryptwithbase64Code("WID=" + worksheet?.Id.ToString() + "&UID=" + loggedIn.UserId.ToString()) + "&jumpid=mb_in_oc_mk_ot_cm017595_aw_ot&utm_source=mobile_apps&utm_medium=other_social&utm_campaign=sep_0709&type=sp365d";
			//				lst.PdfUrl = downloadUrl;

			//				if (!String.IsNullOrWhiteSpace(lst.PdfUrl) && !String.IsNullOrWhiteSpace(mobile) && !String.IsNullOrWhiteSpace(notificationCode))
			//				{
			//					IRestResponse response = whatsAppHelper.CreateMessageForSpecialPlan(mobile, notificationCode, lst);

			//					if (response != null)
			//					{
			//						dbNotificationAccess dbClass = new dbNotificationAccess();
			//						List<NotificationLog> notificationLog = new List<NotificationLog>();

			//						notificationLog.Add(new NotificationLog { UserId = loggedIn.UserId, WeekId = day, NotificationName = notificationCode, NotificationStatus = 1, NotificationJson = response.Content, SendStatus = response.StatusCode.ToString(), TypeOfNotif = "S" });
			//						GetStatus status = Task.Run(() => dbClass.NotificationLog(notificationLog)).Result;
			//					}
			//				}
			//				else
			//				{
			//					Logger.Info(reporting: typeof(HomeContainer), "WhatsApp SpecialPlan Message - parameter missing");
			//				}
			//			}
			//			else
			//			{
			//				Logger.Info(reporting: typeof(HomeContainer), "WhatsApp SpecialPlan Message - worksheet missing");
			//			}
			//		}
			//		else
			//		{
			//			Logger.Info(reporting: typeof(HomeContainer), "WhatsApp SpecialPlan Message - notificationcode missing");
			//		}
			//	}
			//	else
			//	{
			//		Logger.Info(reporting: typeof(HomeContainer), "WhatsApp SpecialPlan Message - notification item");
			//	}
			//}

			//NotificationController notificationController = new NotificationController();
			//string status = Task.Run(() => notificationController.SpecialPlanNotification(1, loggedIn, notificationItem, notificationCode, worksheet)).Result;

			//byte[] utf16Data = Encoding.UTF32.GetBytes("waghravi32rw@gmail.com");
			//string dec = clsCommon.DecryptWithBase64Code("cTF5Z0RyVlFSWUdWVXJMMGZHTGlHQT09");
			//string dfff = MD5HashPassword.CreateMD5Hash("waghravi32rw@gmail.com");
			//string f = "";
			//SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
			//string str = sendDataToSFMC.PostDataSFMC("310921CD44054B5D", "");


			//List<EncryptionDecryption> enc = new List<EncryptionDecryption>();
			//dbAccessClass db = new dbAccessClass();
			//enc = db.encDecGetData();
			//if (enc != null && enc.Count > 0)
			//{
			//	foreach (var item in enc)
			//	{
			//		db = new dbAccessClass();
			//		db.encDecData(item.U_Email, clsCommon.Encryptwithbase64Code(item.U_Name));
			//	}
			//}

			//try
			//{
			//	Responce post = new Responce();
			//	ApiCallServices apiCall = new ApiCallServices();
			//	RegistrationPostModel postModel = new RegistrationPostModel();
			//	List<Item> items = new List<Item>();
			//	items.Add(new Item()
			//	{
			//		UserId = 1,
			//		userUniqueId = "Test123",
			//		DataSource = "New",
			//		u_name = "Test",
			//		u_email = "test@gmail.com",
			//		u_whatsappno = "9999999999",
			//		age_group = "3-4",
			//		WhatsAppConsent = "Yes",
			//		EmailConsent = "No",
			//		Subscription_Plan_Opted = "Free",
			//		Plan_Amount = "0",
			//		Discount = "0",
			//		Coupon_Redeemed = "0",
			//		Coupon_Source = "0",
			//		Amount_Received = "0",
			//		referralCode = "dfdfdfd",
			//		register_date = DateTime.Now.ToString("yyyy-MM-dd"),
			//		Date_of_Subscriber = DateTime.Now.ToString("yyyy-MM-dd"),
			//		Subscriber_Key = "dfdfd4555fd",
			//	});
			//	;
			//	postModel.Data = items;
			//	post = apiCall.PostRegistartionData(postModel);
			//}
			//catch (Exception ex)
			//{
			//	Logger.Error(reporting: typeof(HomeController), ex, message: "registration - Save RquestId in Sync Solution");
			//}
			//string sdf = clsCommon.Decrypt("jUd1mY3f5e2ChAG4p1KWT0b470IZ6d0oP3W3ve6IENM=");
			//string ff = "";

			//TimeSpan rmtime = TimeSpan.FromSeconds(Convert.ToInt32(10));
			//string strTimeLeft = rmtime.ToString(@"mm\:ss");

			//var myJsonData = "[\r\n  {\r\n    \"a\": \"b\",\r\n    \"c\": \"d\",\r\n    \"e\": \"f\"\r\n  }\r\n]";
			//string cleanJson = Regex.Unescape(myJsonData);
			//string data = Regex.Replace(myJsonData, @"\r", "");
			//List<string> lst = new List<string>();
			//lst.Add("test");
			//lst.Add("test1");
			//lst.Add("test2");
			//WhatsAppHelper.CreateMessage("9999973729", TemplateTypeEnum.plc_free_week_1, lst);

			//string culture = CultureName.GetCultureName().Replace("/", "");
			//if (String.IsNullOrWhiteSpace(culture))
			//	culture = "en-US";

			//_variationContextAccessor.VariationContext = new VariationContext(culture);
			//var home = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault();
			//var worksheetRootUrl = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.Any(cl => cl.Value.Culture == "hi"))
			//		?.FirstOrDefault()?.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()
			//		?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()
			//		?.Where(x => x.AgeGroup.Name == "4-5" && x.IsPublished())
			//		?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "worksheetRoot")?.OfType<WorksheetRoot>()
			//		?.Where(y => y.SelectVolume.Name == "Week 1" && y.IsActive == true).Where(c => c.Cultures.Any(cl => cl.Key == "hi")).FirstOrDefault();


			//List<string> list = new List<string> { "", "", "Download" };
			//if (list.Any(n => n != ""))
			//{
			//	string test = list.Where(n => n != "").FirstOrDefault();
			//}
			//string fileName = "21092212134536_A.pdf";
			//string[] servers = System.IO.File.ReadAllLines(HostingEnvironment.MapPath("/Content/Servers.txt"));

			//foreach (string server in servers)
			//{
			//	SavePDFFile(fileName, server);
			//}
			//
			//SavePDFFile(fileName, "local", "10.10.20.170" + "/invoice/" + fileName, "10.10.20.170");

			//SavePDFFile(fileName, "local", "10.10.20.48" + "/invoice/" + fileName, "10.10.20.48");

			//SavePDFFile(fileName, "local", "10.10.10.12" + "/invoice/" + fileName, "10.10.10.12");
			//var fileName = DateTime.Now.Ticks.ToString() + ".pdf";
			//PdfGeneratorController cont = new PdfGeneratorController();
			//byte[] bytes = cont.PdfGenerateAndSave();
			//S3BucketHelper s3BucketHelper = new S3BucketHelper();
			//s3BucketHelper.sendMyFileToS3Async(bytes, fileName);
			//int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			//HP_PLC_Doc.Models.InvoiceDetails invoiceDetails = new HP_PLC_Doc.Models.InvoiceDetails();
			//List<HP_PLC_Doc.Models.InvoiceData> InvoiceList = new List<HP_PLC_Doc.Models.InvoiceData>();
			//MyProfileWithSubscription myprofile = new MyProfileWithSubscription();
			//myprofile = GetProfileWithPaymentId(paymentStatus.PaymentId);
			//List<SetParameters> invoice = new List<SetParameters>()
			//		{
			//			new SetParameters { ParameterName = "@QType", Value = "1" },
			//			new SetParameters { ParameterName = "@UserId", Value = "1081" },
			//			new SetParameters { ParameterName = "@TransactionId", Value = "21090318303998a0fa2" }
			//		};
			//InvoiceList = _db.GetDataMultiple("GetInvoiceList", InvoiceList, invoice);


			//HP_PLC_Doc.Models.InvoiceModel invoiceModel = new HP_PLC_Doc.Models.InvoiceModel();
			//invoiceModel = GetInvoiceDetailsFromCMS();
			//invoiceModel.InvoiceList = InvoiceList;
			//invoiceModel.UserEmailId = clsCommon.Decrypt(SessionManagement.GetCurrentSession<string>(SessionType.emailid));

			//Responce responce = new Responce();
			//var fileName = DateTime.Now.Ticks.ToString() + ".pdf";
			//PdfGeneratorController contpdf = new PdfGeneratorController();
			//byte[] bytes = contpdf.PdfGenerateAndSave(invoiceModel, "21082914241989ffa2b");
			//S3BucketHelper s3BucketHelper = new S3BucketHelper();
			//responce = s3BucketHelper.sendMyFileToS3Async(bytes, fileName);

			//System.IO.File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("/Invoice/") + fileName, bytes);

			return "";
		}

		public async Task<string> pdfdwnld()
		{
			string downloadUrl = String.Empty;
			List<pdfdownloaddata> enc = new List<pdfdownloaddata>();
			dbAccessClass db = new dbAccessClass();
			enc = db.pdfdwnload();
			if (enc != null && enc.Count > 0)
			{
				foreach (var item in enc)
				{
					try
					{
						string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
						string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
						string BucketName = ConfigurationManager.AppSettings["BucketFileSystem:BucketName"].ToString();

						string filePath = Server.MapPath("/pdffile");
						//downloadUrl = "media/racl3nfn/brain-games-4.pdf";
						downloadUrl = item.pdffilename;

						string[] temp = downloadUrl.Split('/');
						string fileName = temp[temp.Length - 1];

						try
						{
							var client = new AmazonS3Client(AWSAccessKey, AWSSecretKey);

							try
							{
								using (var obj = client.GetObject(BucketName, downloadUrl))
								{
									try
									{
										obj.WriteResponseStreamToFile(filePath + "\\" + fileName);
									}
									catch { }
								}
							}
							catch { }
						}
						catch { }
					}
					catch { }
					//string filePath = Server.MapPath("/pdffile");
					//downloadUrl = "ukg-quick-subtraction2023-11-21-10-11-1.pdf";//item.pdffilename;

					//TransferUtility fileTransferUtility = new TransferUtility(new AmazonS3Client(AWSAccessKey, AWSSecretKey, Amazon.RegionEndpoint.APSouth1));

					//// Note the 'fileName' is the 'key' of the object in S3 (which is usually just the file name)
					//fileTransferUtility.Download(filePath, BucketName, downloadUrl);

					//try
					//{
					//	var credentials = new BasicAWSCredentials(AWSAccessKey, AWSSecretKey);
					//	var config = new AmazonS3Config
					//	{
					//		RegionEndpoint = Amazon.RegionEndpoint.APSouth1
					//	};

					//	var client = new AmazonS3Client(credentials, config);
					//	var fileTransferUtility = new TransferUtility(client);

					//	var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
					//	{
					//		BucketName = BucketName,
					//		Key = downloadUrl
					//	});

					//	if (objectResponse.ResponseStream == null)
					//	{
					//		//return NotFound();
					//	}


					//	return File(objectResponse.ResponseStream, objectResponse.Headers.ContentType, fileName);
					//}
					//catch (AmazonS3Exception amazonS3Exception)
					//{
					//	if (amazonS3Exception.ErrorCode != null
					//		&& (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity") || amazonS3Exception.ErrorCode.Equals("NoSuchKey")))
					//	{
					//		Logger.Error(reporting: typeof(WorkSheetController), null, message: "Check the provided AWS Credentials.");
					//		//throw new Exception("Check the provided AWS Credentials.");
					//	}
					//	else
					//	{
					//		Logger.Error(reporting: typeof(WorkSheetController), null, message: amazonS3Exception.Message);
					//		//throw new Exception("Error occurred: " + amazonS3Exception.Message);
					//	}
					//}
				}
			}

			return "";
		}
		//Firsttime add actual age group to temp
		public string SetAgeGroupForAddSubscription(string agegroup = "")
		{
			try
			{
				//string culture = CultureName.GetCultureName().Replace("/", "");
				//if (String.IsNullOrWhiteSpace(culture))
				//	culture = "en-US";

				//_variationContextAccessor.VariationContext = new VariationContext(culture);

				List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
				UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);
				if (UsertempSubscription == null || UsertempSubscription.Count == 0)
				{
					TempSubscriptionCreatedByUser UserSubscription = new TempSubscriptionCreatedByUser();
					dbAccessClass _db = new dbAccessClass();
					List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
					myagegroup = _db.GetUserSelectedUserGroup();
					UsertempSubscription = new List<TempSubscriptionCreatedByUser>();

					if (myagegroup != null && myagegroup.Any())
					{
						int row = 1;
						if (!String.IsNullOrEmpty(agegroup) && agegroup.Any(char.IsLetter) == false)
						{
							UserSubscription = new TempSubscriptionCreatedByUser();
							UserSubscription.SrNo = row;
							UserSubscription.AgeGroup = agegroup;
							UserSubscription.Ranking = "";
							UserSubscription.SubscriptionName = "";
							UserSubscription.SubscriptionPrice = "";

							UsertempSubscription.Add(UserSubscription);
							row++;
						}
						else
						{
							foreach (var age in myagegroup)
							{
								if (age.AgeGroup.Any(char.IsLetter) == false)
								{
									UserSubscription = new TempSubscriptionCreatedByUser();
									UserSubscription.SrNo = row;
									UserSubscription.AgeGroup = age.AgeGroup;
									UserSubscription.Ranking = "";
									UserSubscription.SubscriptionName = "";
									UserSubscription.SubscriptionPrice = "";

									UsertempSubscription.Add(UserSubscription);
									row++;
								}
							}
						}

						SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UsertempSubscription);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SetAgeGroupForAddSubscription - Add First Time Age Group For Subscription");
			}

			return "ok";
		}

		public string SetAgeGroupForAddSubscriptionTeacher(string agegroup = "")
		{
			try
			{
				//Age group allow for Subscription
				dbAccessClass _db = new dbAccessClass();
				_db = new dbAccessClass();
				List<string> allowSubscriptionAgeGroup = new List<string>();
				allowSubscriptionAgeGroup = _db.AllowSubscriptionsAgeGroupTeachers();
				SessionManagement.StoreInSession(SessionType.AllowuserGroup, allowSubscriptionAgeGroup);

				List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
				UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);
				if (UsertempSubscription == null || UsertempSubscription.Count == 0)
				{
					TempSubscriptionCreatedByUser UserSubscription = new TempSubscriptionCreatedByUser();
					List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
					myagegroup = _db.GetUserSelectedUserGroup();

					UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					List<GetUserCurrentSubscription> UserCurrentSubscription = new List<GetUserCurrentSubscription>();
					UserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtlsTeachers);

					if (myagegroup != null && myagegroup.Any())
					{
						int row = 1;
						if (!String.IsNullOrEmpty(agegroup) && agegroup.Any(char.IsLetter) == false)
						{
							UserSubscription = new TempSubscriptionCreatedByUser();
							UserSubscription.SrNo = row;
							UserSubscription.AgeGroup = agegroup;
							UserSubscription.Ranking = "";
							UserSubscription.SubscriptionName = "";
							UserSubscription.SubscriptionPrice = "";

							UsertempSubscription.Add(UserSubscription);
							row++;
						}
						else
						{
							if (UserCurrentSubscription != null)
							{
								var selectedAge = myagegroup?.Where(x => UserCurrentSubscription.Any(c => c.AgeGroup.ToString() != x.AgeGroup.ToString()));
								selectedAge = selectedAge.Where(x => allowSubscriptionAgeGroup.Any(c => c == x.AgeGroup.ToString()));

								if (selectedAge != null && selectedAge.Count() > 0)
								{
									foreach (var age in selectedAge)
									{
										if (age.AgeGroup.Any(char.IsLetter) == false)
										{
											UserSubscription = new TempSubscriptionCreatedByUser();
											UserSubscription.SrNo = row;
											UserSubscription.AgeGroup = age.AgeGroup;
											UserSubscription.Ranking = "";
											UserSubscription.SubscriptionName = "";
											UserSubscription.SubscriptionPrice = "";

											UsertempSubscription.Add(UserSubscription);
											row++;
										}
									}
								}
							}
							else
							{
								UserSubscription = new TempSubscriptionCreatedByUser();
								UserSubscription.SrNo = row;
								UserSubscription.AgeGroup = "3-4";
								UserSubscription.Ranking = "";
								UserSubscription.SubscriptionName = "";
								UserSubscription.SubscriptionPrice = "";

								UsertempSubscription.Add(UserSubscription);
							}
						}

						SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UsertempSubscription);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SetAgeGroupForAddSubscription - Add First Time Age Group For Subscription");
			}

			return "ok";
		}

		public ActionResult AddAgeGroup(string ageGroup)
		{
			try
			{
				if (!String.IsNullOrEmpty(ageGroup))
				{
					//int? maxAgeGroupSelection = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
					//		.Where(x => x.ContentType.Alias == "subscriptionList")?
					//		.OfType<SubscriptionList>()?.FirstOrDefault().MaxAgeGroup;

					string culture = CultureName.GetCultureName().Replace("/", "");
					if (String.IsNullOrWhiteSpace(culture))
						culture = "en-US";

					_variationContextAccessor.VariationContext = new VariationContext(culture);

					//Get All subscription detais for user
					List<GetUserCurrentSubscription> UserCurrentSubscription = new List<GetUserCurrentSubscription>();
					UserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

					List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

					TempSubscriptionCreatedByUser UserSubscription = new TempSubscriptionCreatedByUser();
					//UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					string[] ages = ageGroup.Split(',');

					//check age group selection - age cap defines in cms - if 0 then no limit
					int CartAgeGroup = UsertempSubscription.GroupBy(x => x.AgeGroup).Count();
					var newAges = new List<string>();
					if (ages.Length > 0)
					{
						foreach (var age in ages)
						{
							if (!UsertempSubscription.Any(x => x.AgeGroup == age))
							{
								newAges.Add(age);
							}
						}
					}

					//if (maxAgeGroupSelection > 0 && (CartAgeGroup + newAges.Count) > maxAgeGroupSelection)// + UserCurrentSubscription.GroupBy(x => x.AgeGroup).Count()
					//{
					//	ViewData["Message"] = "agevalidation";
					//	ViewData["MessageText"] = maxAgeGroupSelection;

					//	return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
					//}
					//else
					//{
					//int row = (UsertempSubscription.Count + 1);
					int row = 1;
					if (UsertempSubscription.Count > 0)
						row = (UsertempSubscription.Max(x => x.SrNo) + 1);

					for (int i = 0; i < ages.Length; i++)
					{
						bool agealreadyadded = UsertempSubscription.Where(x => x.AgeGroup == ages[i]).Any();
						if (agealreadyadded == false)
						{
							UserSubscription = new TempSubscriptionCreatedByUser();
							UserSubscription.SrNo = row;
							UserSubscription.AgeGroup = ages[i];
							UserSubscription.Ranking = "";
							UserSubscription.SubscriptionName = "";
							UserSubscription.SubscriptionPrice = "";

							UsertempSubscription.Add(UserSubscription);
							row++;
						}
					}

					ViewData["Message"] = "Success";
					return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
					//}
				}
				else
				{
					ViewData["Message"] = "Fail";
					return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "AddAgeGroup");
			}

			return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
		}

		public ActionResult ChangeAgeGroupTeachers(string ageGroup,int ranking)
		{
			try
			{
				if (!String.IsNullOrEmpty(ageGroup))
				{
					List<string> allowSubscriptionAgeGroup = new List<string>();
					allowSubscriptionAgeGroup = SessionManagement.GetCurrentSession<List<string>>(SessionType.AllowuserGroup);

					string[] ageGroupSelected = ageGroup.Split(',');

					if (allowSubscriptionAgeGroup != null)
					{
						if (ageGroupSelected != null)
						{
							foreach (var ageItem in ageGroupSelected)
							{
								bool ageExists = allowSubscriptionAgeGroup.Where(x => x == ageItem.ToString()).Any();

								if (ageExists == false)
								{
									ViewData["Message"] = "notallowed";
									return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
								}
							}
						}
					}

					string culture = CultureName.GetCultureName().Replace("/", "");
					if (String.IsNullOrWhiteSpace(culture))
						culture = "en-US";

					_variationContextAccessor.VariationContext = new VariationContext(culture);

					//Get All subscription detais for user
					List<GetUserCurrentSubscription> UserCurrentSubscription = new List<GetUserCurrentSubscription>();
					UserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtlsTeachers);

					List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

					TempSubscriptionCreatedByUser UserSubscription = new TempSubscriptionCreatedByUser();
					//UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					//string newage = ageGroup.Split(',');

					//check age group selection - age cap defines in cms - if 0 then no limit

					if (ageGroupSelected != null)
					{
						SessionManagement.DeleteFromSession(SessionType.SubscriptionTempDtls);
						UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
						int row = 1;

						foreach (var ageItems in ageGroupSelected)
						{
							UsertempSubscription.Add(new TempSubscriptionCreatedByUser { SrNo = row, AgeGroup = ageItems });

							row++;
							//if (!UsertempSubscription.Where(x => x.AgeGroup == ageItems).Any())
							//{
							//	UsertempSubscription.Add(new TempSubscriptionCreatedByUser { SrNo = row, AgeGroup = ageItems });

							//	row++;
							//}
						}

						
						SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UsertempSubscription);
						SubscriptionMappingTeachersWorksheet(ranking,0, "onload");
					}

					ViewData["Message"] = "Success";
					return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");

				}
				else
				{
					ViewData["Message"] = "Fail";
					return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "AddAgeGroup");
			}

			return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
		}

		public ActionResult AddAgeGroupExistingGroup(string ageGroup)
		{
			GetStatus responce = new GetStatus();
			try
			{
				if (!String.IsNullOrEmpty(ageGroup))
				{
					//int? maxAgeGroupSelection = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
					//		.Where(x => x.ContentType.Alias == "subscriptionList")?
					//		.OfType<SubscriptionList>()?.FirstOrDefault().MaxAgeGroup;
					string culture = CultureName.GetCultureName().Replace("/", "");
					if (String.IsNullOrWhiteSpace(culture))
						culture = "en-US";

					_variationContextAccessor.VariationContext = new VariationContext(culture);

					string[] ages = ageGroup.Split(',');
					//if (ages.Length > maxAgeGroupSelection)
					//{
					//	responce.returnStatus = "Fail";
					//	responce.returnMessage = "You can not add more than " + maxAgeGroupSelection + " age groups.";
					//}
					//else
					//{
					int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
					if (UserId > 0)
					{
						dbProxy _db = new dbProxy();
						List<SetParameters> sp = new List<SetParameters>()
							{
								new SetParameters{ ParameterName = "@userid", Value = UserId.ToString() },
								new SetParameters{ ParameterName = "@agegroup", Value = ageGroup }
							};

						responce = _db.GetData<GetStatus>("usp_AddAgeGroup", responce, sp);
						if (responce.returnStatus == "OK")
						{
							SessionManagement.StoreInSession(SessionType.AgeGroupExistsOrNot, "Yes");

							return Json(new { status = responce.returnStatus, message = responce.returnMessage }, JsonRequestBehavior.AllowGet);
						}
					}
					//}
				}

				return Json(new { status = responce.returnStatus, message = responce.returnMessage }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "AddAgeGroup");
			}

			return Json(new { status = responce.returnStatus, message = responce.returnMessage }, JsonRequestBehavior.AllowGet);
		}
		public ActionResult DeleteSelectedItem(int srno)
		{
			try
			{
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(culture);

				if (!String.IsNullOrEmpty(srno.ToString()))
				{
					List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);
					if (UsertempSubscription != null && UsertempSubscription.Any())
					{
						UsertempSubscription = UsertempSubscription.Where(x => x.SrNo != srno).ToList();

						SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UsertempSubscription);
						ViewData["Message"] = "Success";
						return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
					}
				}
				else
				{
					ViewData["Message"] = "Fail";
					return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "DeleteSelectedItem");
			}

			return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
		}

		public ActionResult DeleteSelectedItemTeachers(int srno)
		{
			try
			{
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(culture);

				if (!String.IsNullOrEmpty(srno.ToString()))
				{
					List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);
					if (UsertempSubscription != null && UsertempSubscription.Any())
					{
						UsertempSubscription = UsertempSubscription.Where(x => x.SrNo != srno).ToList();

						SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UsertempSubscription);
						ViewData["Message"] = "Success";
						return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
					}
				}
				else
				{
					ViewData["Message"] = "Fail";
					return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "DeleteSelectedItem");
			}

			return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
		}
		public ActionResult SubscriptionMapping(int Ranking = 0, int SrNo = 0, string eventtype = "", string agegroup = "", string IsBotRequest = "")
		{
			try
			{
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(culture);
				//List<TempSubscriptionBind> tempSubscriptionBinds = new List<TempSubscriptionBind>();
				List<TempSubscriptionCreatedByUser> UserSubscription = new List<TempSubscriptionCreatedByUser>();
				UserSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);
				List<TempSubscriptionCreatedByUser> tempUserSubscriptionBinds = new List<TempSubscriptionCreatedByUser>();
				if (UserSubscription == null)
				{
					TempSubscriptionCreatedByUser userSubscriptionSelected = new TempSubscriptionCreatedByUser();
					if (!String.IsNullOrEmpty(IsBotRequest) && IsBotRequest.ToLower().Equals("yes"))
					{
						List<BotPaySubscriptionDetails> botSubscriptionDetails = new List<BotPaySubscriptionDetails>();
						botSubscriptionDetails = SessionManagement.GetCurrentSession<List<BotPaySubscriptionDetails>>(SessionType.SubscriptionBotDtls);
						if (botSubscriptionDetails != null && botSubscriptionDetails.Any())
						{
							int row = 1;
							foreach (var bot in botSubscriptionDetails)
							{
								//tempUserSubscriptionBinds.Add(new TempSubscriptionCreatedByUser()
								//{
								//	Ranking = bot.Ranking.ToString()
								//});
								if (!String.IsNullOrEmpty(bot.Ranking) && int.Parse(bot.Ranking) > 0)
								{
									HomeController home = new HomeController();
									var subscription = home.GetSubscriptionDetailsWithRanking(bot.Ranking);

									SubscriptionItem item = (SubscriptionItem)Umbraco?.Content(subscription.SubscriptionName.Udi);
									NameListItem PartCode = (NameListItem)Umbraco?.Content(subscription.StandardSelection.Udi);

									int DiscountAmt = 0;
									string RegistrationMode = SessionManagement.GetCurrentSession<string>(SessionType.RegistrationMode);
									string SubscriptionAvailedOrNor = SessionManagement.GetCurrentSession<string>(SessionType.SubscribedOrNot);

									//Assign Discount Amount
									if (!String.IsNullOrWhiteSpace(RegistrationMode) && RegistrationMode == "Existing" && !String.IsNullOrWhiteSpace(SubscriptionAvailedOrNor) && SubscriptionAvailedOrNor.ToLower() == "no")
									{
										if (subscription.IsAppliedForExistingUser)
											DiscountAmt = subscription.DiscountAmount;
									}
									//Assign Discount Amount End

									if (!String.IsNullOrEmpty(bot.ageGroup) && bot.ageGroup.Any(char.IsLetter) == false)
									{
										userSubscriptionSelected = new TempSubscriptionCreatedByUser();
										userSubscriptionSelected.SrNo = row;
										userSubscriptionSelected.SubscriptionId = subscription.Id;
										userSubscriptionSelected.PartCode = PartCode?.ItemValue;
										userSubscriptionSelected.ValidMonthsText = subscription?.ValidationText;
										userSubscriptionSelected.DiscountAmount = DiscountAmt;
										userSubscriptionSelected.ValidMonths = item.ValidToVariable;
										userSubscriptionSelected.AgeGroup = bot.ageGroup;
										userSubscriptionSelected.Ranking = bot.Ranking;
										userSubscriptionSelected.SubscriptionName = item.SubscriptionName;
										userSubscriptionSelected.SubscriptionPrice = item.Amount.ToString();

										tempUserSubscriptionBinds.Add(userSubscriptionSelected);
										row++;
									}
								}
							}
						}
					}
					//else
					//{
					//	tempUserSubscriptionBinds.Add(new TempSubscriptionCreatedByUser()
					//	{
					//		Ranking = Ranking.ToString()
					//	});
					//	HomeController home = new HomeController();
					//	var subscription = home.GetSubscriptionDetailsWithRanking(Ranking.ToString());

					//	SubscriptionItem item = (SubscriptionItem)Umbraco?.Content(subscription.SubscriptionName.Udi);

					//	userSubscriptionSelected = new TempSubscriptionCreatedByUser();
					//	userSubscriptionSelected.SrNo = 1;
					//	userSubscriptionSelected.AgeGroup = agegroup;
					//	userSubscriptionSelected.Ranking = Ranking.ToString();
					//	userSubscriptionSelected.SubscriptionName = item.SubscriptionName;
					//	userSubscriptionSelected.SubscriptionPrice = item.Amount.ToString();

					//	tempUserSubscriptionBinds.Add(userSubscriptionSelected);

					//}

					SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, tempUserSubscriptionBinds);
					//UserSubscription = tempUserSubscriptionBinds;
				}

				try
				{
					if (!String.IsNullOrEmpty(Ranking.ToString()) && !String.IsNullOrEmpty(SrNo.ToString()) && Ranking > 0)
					{
						bool validation = true;
						//User already buyed subscriptions details
						GetUserCurrentSubscription UserAlreadySubscription = new GetUserCurrentSubscription();
						List<GetUserCurrentSubscription> UserHaveSubscriptions = new List<GetUserCurrentSubscription>();
						UserHaveSubscriptions = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

						//User current subscription to buy
						List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
						UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

						if (UsertempSubscription != null && UsertempSubscription.Any())
						{
							int? tempAddedThisSubscription = UsertempSubscription?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup))?.Count();
							UserAlreadySubscription = UserHaveSubscriptions?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup) && x.IsActive == 1)?.SingleOrDefault();
							if (Ranking == 1)
							{
								validation = false;
								ViewData["Message"] = "free";
								ViewData["MessageText"] = "You are already have Free subscription.";

								return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
							}
							if (tempAddedThisSubscription >= 1)
							{
								validation = false;
								ViewData["Message"] = "morethanone";
								ViewData["MessageText"] = "Duplicate subscription with same age group can not apply.";

								return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
							}
							if (UserAlreadySubscription != null && UserAlreadySubscription?.DaysRemaining > 15)
							{
								validation = false;
								ViewData["Message"] = "already";
								ViewData["MessageText"] = "You already have this subscription for selected age group.";

								return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
							}

							if (validation)
							{
								int? maxAgeGroupSelection = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
															.Where(x => x.ContentType.Alias == "subscriptionList")?
															.OfType<SubscriptionList>()?.FirstOrDefault().MaxAgeGroup;

								string RegistrationMode = SessionManagement.GetCurrentSession<string>(SessionType.RegistrationMode);
								string SubscriptionAvailedOrNor = SessionManagement.GetCurrentSession<string>(SessionType.SubscribedOrNot);
								int DiscountAmt = 0;

								HomeController home = new HomeController();
								var subscription = home.GetSubscriptionDetailsWithRanking(Ranking.ToString());
								if (subscription != null)
								{
									SubscriptionItem item = (SubscriptionItem)Umbraco?.Content(subscription?.SubscriptionName?.Udi);
									NameListItem PartCode = (NameListItem)Umbraco?.Content(subscription?.StandardSelection?.Udi);
									if (item != null)
									{
										//Assign Discount Amount
										if (!String.IsNullOrWhiteSpace(RegistrationMode) && RegistrationMode == "Existing" && !String.IsNullOrWhiteSpace(SubscriptionAvailedOrNor) && SubscriptionAvailedOrNor.ToLower() == "no")
										{
											if (subscription.IsAppliedForExistingUser)
												DiscountAmt = subscription.DiscountAmount;
										}
										//Assign Discount Amount End

										if (!String.IsNullOrEmpty(eventtype) && eventtype == "onload")// && !String.IsNullOrEmpty(agegroup)
										{
											if (!String.IsNullOrEmpty(agegroup) && agegroup.Any(char.IsLetter) == false)
											{
												bool isAgeGroupAvailable = UsertempSubscription.Where(x => x.AgeGroup == agegroup).Any();

												if (isAgeGroupAvailable)
													UsertempSubscription.Where(x => x.AgeGroup == agegroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
												else
												{
													if (UsertempSubscription.Count < maxAgeGroupSelection)
													{
														AddAgeGroup(agegroup);
														UsertempSubscription.Where(x => x.AgeGroup == agegroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
													}
													else
													{
														validation = false;
														ViewData["Message"] = "agevalidation";
														ViewData["MessageText"] = maxAgeGroupSelection.ToString();

														return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
													}
												}
											}
											else
											{
												foreach (var age in UsertempSubscription)
												{
													if (age.AgeGroup.Any(char.IsLetter) == false)
													{
														bool isAgeGroupAvailable = UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Any();

														if (isAgeGroupAvailable)
															UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
														else
														{
															AddAgeGroup(age.AgeGroup);
															UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
														}
													}
												}
											}
										}
										else
										{
											UsertempSubscription.Where(x => x.SrNo == SrNo).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
										}
									}
								}

								SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UsertempSubscription);
								ViewData["Message"] = "Success";
								return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SubscriptionMapping");
				}

				return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
				//if (UserSubscription != null && UserSubscription.Any())
				//{
				//	bool validation = true;
				//	//User already buyed subscriptions details
				//	GetUserCurrentSubscription UserAlreadySubscription = new GetUserCurrentSubscription();
				//	List<GetUserCurrentSubscription> UserHaveSubscriptions = new List<GetUserCurrentSubscription>();
				//	UserHaveSubscriptions = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

				//	//User current subscription to buy
				//	List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
				//	UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);


				//	foreach (var bindSubscription in UserSubscription.OrderByDescending(x =>x.SrNo))
				//	{
				//		if (String.IsNullOrWhiteSpace(Ranking.ToString()) || Ranking == 0)
				//			Ranking =int.Parse(bindSubscription.Ranking);
				//		//else
				//		//	UsertempSubscription.Where(x => x.SrNo == SrNo).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();

				//		if (String.IsNullOrWhiteSpace(agegroup.ToString()))
				//			agegroup = bindSubscription.AgeGroup;

				//		if (!String.IsNullOrEmpty(Ranking.ToString()) && Ranking > 0 && !String.IsNullOrEmpty(SrNo.ToString()))
				//		{

				//			if (UsertempSubscription != null && UsertempSubscription.Any())
				//			{
				//				//int? tempAddedThisSubscription = UsertempSubscription?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup))?.Count();
				//				int? tempAddedThisSubscription = UsertempSubscription?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == agegroup)?.Count();
				//				UserAlreadySubscription = UserHaveSubscriptions?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup) && x.IsActive == 1)?.SingleOrDefault();
				//				if (Ranking == 1)
				//				{
				//					validation = false;
				//					ViewData["Message"] = "free";
				//					ViewData["MessageText"] = "You are already have Free subscription.";

				//					return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
				//				}
				//				if (tempAddedThisSubscription > 1)
				//				{
				//					validation = false;
				//					ViewData["Message"] = "morethanone";
				//					ViewData["MessageText"] = "Duplicate subscription with same age group can not apply.";

				//					return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
				//				}
				//				if (UserAlreadySubscription != null && UserAlreadySubscription?.DaysRemaining > 15)
				//				{
				//					validation = false;
				//					ViewData["Message"] = "already";
				//					ViewData["MessageText"] = "You already have this subscription for selected age group.";

				//					return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
				//				}

				//				if (validation)
				//				{
				//					int? maxAgeGroupSelection = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
				//												.Where(x => x.ContentType.Alias == "subscriptionList")?
				//												.OfType<SubscriptionList>()?.FirstOrDefault().MaxAgeGroup;

				//					string RegistrationMode = SessionManagement.GetCurrentSession<string>(SessionType.RegistrationMode);
				//					string SubscriptionAvailedOrNor = SessionManagement.GetCurrentSession<string>(SessionType.SubscribedOrNot);
				//					int DiscountAmt = 0;

				//					HomeController home = new HomeController();
				//					var subscription = home.GetSubscriptionDetailsWithRanking(Ranking.ToString());
				//					if (subscription != null)
				//					{
				//						SubscriptionItem item = (SubscriptionItem)Umbraco?.Content(subscription.SubscriptionName.Udi);
				//						NameListItem PartCode = (NameListItem)Umbraco?.Content(subscription.StandardSelection.Udi);
				//						if (item != null)
				//						{
				//							//Assign Discount Amount
				//							if (!String.IsNullOrWhiteSpace(RegistrationMode) && RegistrationMode == "Existing" && !String.IsNullOrWhiteSpace(SubscriptionAvailedOrNor) && SubscriptionAvailedOrNor.ToLower() == "no")
				//							{
				//								if (subscription.IsAppliedForExistingUser)
				//									DiscountAmt = subscription.DiscountAmount;
				//							}
				//							//Assign Discount Amount End

				//							if (!String.IsNullOrEmpty(eventtype) && eventtype == "onload")// && !String.IsNullOrEmpty(agegroup)
				//							{
				//								if (!String.IsNullOrEmpty(agegroup) && agegroup.Any(char.IsLetter) == false)
				//								{
				//									bool isAgeGroupAvailable = UsertempSubscription.Where(x => x.AgeGroup == agegroup).Any();

				//									if (isAgeGroupAvailable)
				//										UsertempSubscription.Where(x => x.AgeGroup == agegroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
				//									else
				//									{
				//										if (UsertempSubscription.Count < maxAgeGroupSelection)
				//										{
				//											AddAgeGroup(agegroup);
				//											UsertempSubscription.Where(x => x.AgeGroup == agegroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
				//										}
				//										else
				//										{
				//											validation = false;
				//											ViewData["Message"] = "agevalidation";
				//											ViewData["MessageText"] = maxAgeGroupSelection.ToString();

				//											return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
				//										}
				//									}
				//								}
				//								else
				//								{
				//									foreach (var age in UsertempSubscription)
				//									{
				//										if (age.AgeGroup.Any(char.IsLetter) == false)
				//										{
				//											bool isAgeGroupAvailable = UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Any();

				//											if (isAgeGroupAvailable)
				//												UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
				//											else
				//											{
				//												AddAgeGroup(age.AgeGroup);
				//												UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
				//											}
				//										}
				//									}
				//								}
				//							}
				//							else
				//							{
				//								UsertempSubscription.Where(x => x.SrNo == SrNo).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
				//							}
				//						}
				//					}
				//				}
				//			}
				//		}

				//		Ranking = 0;
				//		agegroup = String.Empty;
				//	}

				//	SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UsertempSubscription);
				//	ViewData["Message"] = "Success";
				//	return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
				//}

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SubscriptionMapping");
			}
			return PartialView("/Views/Partials/_SubscriptionCart.cshtml");
		}

		public ActionResult SubscriptionMappingBonusWorksheet(int Ranking = 0, int SrNo = 0, string eventtype = "")
		{
			try
			{
				SessionManagement.DeleteFromSession(SessionType.SubscriptionTempDtls);
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(culture);
				//List<TempSubscriptionBind> tempSubscriptionBinds = new List<TempSubscriptionBind>();
				List<TempSubscriptionCreatedByUser> UserSubscription = new List<TempSubscriptionCreatedByUser>();
				//UserSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);
				//List<TempSubscriptionCreatedByUser> tempUserSubscriptionBinds = new List<TempSubscriptionCreatedByUser>();

				try
				{
					if (!String.IsNullOrEmpty(Ranking.ToString()) && !String.IsNullOrEmpty(SrNo.ToString()) && Ranking > 0)
					{
						//bool validation = true;
						//User already buyed subscriptions details
						//GetUserCurrentSubscription UserAlreadySubscription = new GetUserCurrentSubscription();
						//List<GetUserCurrentSubscription> UserHaveSubscriptions = new List<GetUserCurrentSubscription>();
						//UserHaveSubscriptions = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

						//User current subscription to buy
						//TempSubscriptionCreatedByUser UsertempSubscription = new TempSubscriptionCreatedByUser();
						//UsertempSubscription = SessionManagement.GetCurrentSession<TempSubscriptionCreatedByUser>(SessionType.SubscriptionTempDtls);

						HomeController home = new HomeController();
						var subscription = home.GetSubscriptionDetailsWithRanking_BonusWorksheet(Ranking.ToString());

						if (subscription != null)
						{
							SubscriptionItemStructureProgram item = (SubscriptionItemStructureProgram)Umbraco?.Content(subscription?.SubscriptionName?.Udi);
							NameListItem PartCode = (NameListItem)Umbraco?.Content(subscription?.StandardSelection?.Udi);
							if (item != null)
							{
								//Assign Discount Amount
								//if (!String.IsNullOrWhiteSpace(RegistrationMode) && RegistrationMode == "Existing" && !String.IsNullOrWhiteSpace(SubscriptionAvailedOrNor) && SubscriptionAvailedOrNor.ToLower() == "no")
								//{
								//	if (subscription.IsAppliedForExistingUser)
								//		DiscountAmt = subscription.DiscountAmount;
								//}

								//Assign Discount Amount End
								UserSubscription.Add(new TempSubscriptionCreatedByUser { SubscriptionId = subscription.Id, PartCode = PartCode?.ItemValue, ValidMonthsText = subscription?.ValidationText, SubscriptionName = item.SubscriptionName, SubscriptionPrice = item.Amount.ToString(), DiscountAmount = 0, ValidMonths = item.ValidToVariable, Ranking = subscription.Ranking });
								//UsertempSubscription.SubscriptionId = subscription.Id;
								//UsertempSubscription.PartCode = PartCode?.ItemValue;
								//UsertempSubscription.ValidMonthsText = subscription?.ValidationText;
								//UsertempSubscription.SubscriptionName = item.SubscriptionName;
								//UsertempSubscription.SubscriptionPrice = item.Amount.ToString();
								//UsertempSubscription.DiscountAmount = 0;
								//UsertempSubscription.ValidMonths = item.ValidToVariable;
								//UsertempSubscription.Ranking = subscription.Ranking;
							}


							SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UserSubscription);

							ViewData["Message"] = "Success";
							return PartialView("/Views/Partials/_SubscriptionCartStructurePrm.cshtml");
						}



						//if (UsertempSubscription != null && UsertempSubscription.Any())
						//{
						//	int? tempAddedThisSubscription = UsertempSubscription?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup))?.Count();
						//	UserAlreadySubscription = UserHaveSubscriptions?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup) && x.IsActive == 1)?.SingleOrDefault();

						//	if (validation)
						//	{
						//		string RegistrationMode = SessionManagement.GetCurrentSession<string>(SessionType.RegistrationMode);
						//		string SubscriptionAvailedOrNor = SessionManagement.GetCurrentSession<string>(SessionType.SubscribedOrNot);
						//		int DiscountAmt = 0;

						//		HomeController home = new HomeController();
						//		var subscription = home.GetSubscriptionDetailsWithRanking_BonusWorksheet(Ranking.ToString());

						//		if (subscription != null)
						//		{
						//			SubscriptionItemStructureProgram item = (SubscriptionItemStructureProgram)Umbraco?.Content(subscription?.SubscriptionName?.Udi);
						//			NameListItem PartCode = (NameListItem)Umbraco?.Content(subscription?.StandardSelection?.Udi);
						//			if (item != null)
						//			{
						//				//Assign Discount Amount
						//				//if (!String.IsNullOrWhiteSpace(RegistrationMode) && RegistrationMode == "Existing" && !String.IsNullOrWhiteSpace(SubscriptionAvailedOrNor) && SubscriptionAvailedOrNor.ToLower() == "no")
						//				//{
						//				//	if (subscription.IsAppliedForExistingUser)
						//				//		DiscountAmt = subscription.DiscountAmount;
						//				//}

						//				//Assign Discount Amount End
						//				//UsertempSubscription.Where(x => x.Ranking == Ranking.ToString()).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
						//				UsertempSubscription.Add(new TempSubscriptionCreatedByUser { SubscriptionId = subscription.Id, PartCode = PartCode?.ItemValue, ValidMonthsText = subscription?.ValidationText, SubscriptionName = item.SubscriptionName, SubscriptionPrice = item.Amount.ToString(), DiscountAmount = DiscountAmt, ValidMonths = item.ValidToVariable, Ranking = subscription.Ranking });
						//			}
						//		}

						//		SessionManagement.StoreInSession(SessionType.SubscriptionTempDtlsBonus, UsertempSubscription);

						//		ViewData["Message"] = "Success";
						//		return PartialView("/Views/Partials/_SubscriptionCartStructurePrm.cshtml");
						//	}
						//}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SubscriptionMapping");
				}


				return PartialView("/Views/Partials/_SubscriptionCartStructurePrm.cshtml");

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SubscriptionMapping");
			}
			return PartialView("/Views/Partials/_SubscriptionCartStructurePrm.cshtml");
		}

		public ActionResult SubscriptionMappingTeachersWorksheet(int Ranking = 0, int SrNo = 0, string eventtype = "", string agegroup = "")
		{
			try
			{
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				_variationContextAccessor.VariationContext = new VariationContext(culture);
				//List<TempSubscriptionBind> tempSubscriptionBinds = new List<TempSubscriptionBind>();
				List<TempSubscriptionCreatedByUser> UserSubscription = new List<TempSubscriptionCreatedByUser>();
				UserSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);
				List<TempSubscriptionCreatedByUser> tempUserSubscriptionBinds = new List<TempSubscriptionCreatedByUser>();

				if (UserSubscription == null)
				{
					TempSubscriptionCreatedByUser userSubscriptionSelected = new TempSubscriptionCreatedByUser();

					SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, tempUserSubscriptionBinds);
				}

				try
				{
					if (!String.IsNullOrEmpty(Ranking.ToString()) && !String.IsNullOrEmpty(SrNo.ToString()) && Ranking > 0)
					{
						bool validation = true;
						//User already buyed subscriptions details
						GetUserCurrentSubscription UserAlreadySubscription = new GetUserCurrentSubscription();
						List<GetUserCurrentSubscription> UserHaveSubscriptions = new List<GetUserCurrentSubscription>();
						UserHaveSubscriptions = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtlsTeachers);

						//User current subscription to buy
						List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
						UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

						if (UsertempSubscription != null && UsertempSubscription.Any())
						{
							int? tempAddedThisSubscription = UsertempSubscription?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup))?.Count();
							UserAlreadySubscription = UserHaveSubscriptions?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup) && x.IsActive == 1)?.SingleOrDefault();

							if (tempAddedThisSubscription >= 1)
							{
								validation = false;
								ViewData["Message"] = "morethanone";
								ViewData["MessageText"] = "Duplicate subscription with same age group can not apply.";

								return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
							}
							if (UserAlreadySubscription != null && UserAlreadySubscription?.DaysRemaining > 15)
							{
								validation = false;
								ViewData["Message"] = "already";
								ViewData["MessageText"] = "You already have this subscription for selected age group.";

								return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
							}

							if (validation)
							{
								int? maxAgeGroupSelection = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
															.Where(x => x.ContentType.Alias == "teachersSubscriptionList")?
															.OfType<TeachersSubscriptionList>()?.FirstOrDefault().MaxAgeGroup;

								string RegistrationMode = SessionManagement.GetCurrentSession<string>(SessionType.RegistrationMode);
								string SubscriptionAvailedOrNor = SessionManagement.GetCurrentSession<string>(SessionType.SubscribedOrNot);
								int DiscountAmt = 0;

								HomeController home = new HomeController();
								var subscription = home.GetSubscriptionDetailsWithRanking_TeachersWorksheet(Ranking.ToString());
								if (subscription != null)
								{
									TeachersSubscriptionItem item = (TeachersSubscriptionItem)Umbraco?.Content(subscription?.SubscriptionName?.Udi);
									NameListItem PartCode = (NameListItem)Umbraco?.Content(subscription?.StandardSelection?.Udi);
									if (item != null)
									{

										if (!String.IsNullOrEmpty(eventtype) && eventtype == "onload")// && !String.IsNullOrEmpty(agegroup)
										{
											if (!String.IsNullOrEmpty(agegroup) && agegroup.Any(char.IsLetter) == false)
											{
												bool isAgeGroupAvailable = UsertempSubscription.Where(x => x.AgeGroup == agegroup).Any();

												if (isAgeGroupAvailable)
													UsertempSubscription.Where(x => x.AgeGroup == agegroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
												else
												{
													if (UsertempSubscription.Count < maxAgeGroupSelection)
													{
														AddAgeGroup(agegroup);
														UsertempSubscription.Where(x => x.AgeGroup == agegroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
													}
													else
													{
														validation = false;
														ViewData["Message"] = "agevalidation";
														ViewData["MessageText"] = maxAgeGroupSelection.ToString();

														return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
													}
												}
											}
											else
											{
												foreach (var age in UsertempSubscription)
												{
													if (age.AgeGroup.Any(char.IsLetter) == false)
													{
														bool isAgeGroupAvailable = UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Any();

														if (isAgeGroupAvailable)
															UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
														else
														{
															AddAgeGroup(age.AgeGroup);
															UsertempSubscription.Where(x => x.AgeGroup == age.AgeGroup).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
														}
													}
												}
											}
										}
										else
										{
											UsertempSubscription.Where(x => x.SrNo == SrNo).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
										}
									}
								}

								SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UsertempSubscription);
								ViewData["Message"] = "Success";

								return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SubscriptionMapping");
				}

				return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");

			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SubscriptionMapping");
			}

			return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");

			//try
			//{
			//	SessionManagement.DeleteFromSession(SessionType.SubscriptionTempDtls);
			//	string culture = CultureName.GetCultureName().Replace("/", "");
			//	if (String.IsNullOrWhiteSpace(culture))
			//		culture = "en-US";

			//	_variationContextAccessor.VariationContext = new VariationContext(culture);
			//	//List<TempSubscriptionBind> tempSubscriptionBinds = new List<TempSubscriptionBind>();
			//	List<TempSubscriptionCreatedByUser> UserSubscription = new List<TempSubscriptionCreatedByUser>();
			//	//UserSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);
			//	//List<TempSubscriptionCreatedByUser> tempUserSubscriptionBinds = new List<TempSubscriptionCreatedByUser>();

			//	try
			//	{
			//		if (!String.IsNullOrEmpty(Ranking.ToString()) && !String.IsNullOrEmpty(SrNo.ToString()) && Ranking > 0)
			//		{
			//			//bool validation = true;
			//			//User already buyed subscriptions details
			//			//GetUserCurrentSubscription UserAlreadySubscription = new GetUserCurrentSubscription();
			//			//List<GetUserCurrentSubscription> UserHaveSubscriptions = new List<GetUserCurrentSubscription>();
			//			//UserHaveSubscriptions = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtls);

			//			//User current subscription to buy
			//			//TempSubscriptionCreatedByUser UsertempSubscription = new TempSubscriptionCreatedByUser();
			//			//UsertempSubscription = SessionManagement.GetCurrentSession<TempSubscriptionCreatedByUser>(SessionType.SubscriptionTempDtls);

			//			HomeController home = new HomeController();
			//			var subscription = home.GetSubscriptionDetailsWithRanking_TeachersWorksheet(Ranking.ToString());

			//			if (subscription != null)
			//			{
			//				TeachersSubscriptionItem item = (TeachersSubscriptionItem)Umbraco?.Content(subscription?.SubscriptionName?.Udi);
			//				//SubscriptionItemStructureProgram item = (SubscriptionItemStructureProgram)Umbraco?.Content(subscription?.SubscriptionName?.Udi);
			//				NameListItem PartCode = (NameListItem)Umbraco?.Content(subscription?.StandardSelection?.Udi);
			//				if (item != null)
			//				{
			//					//Assign Discount Amount
			//					//if (!String.IsNullOrWhiteSpace(RegistrationMode) && RegistrationMode == "Existing" && !String.IsNullOrWhiteSpace(SubscriptionAvailedOrNor) && SubscriptionAvailedOrNor.ToLower() == "no")
			//					//{
			//					//	if (subscription.IsAppliedForExistingUser)
			//					//		DiscountAmt = subscription.DiscountAmount;
			//					//}

			//					//Assign Discount Amount End
			//					UserSubscription.Add(new TempSubscriptionCreatedByUser { SubscriptionId = subscription.Id, PartCode = PartCode?.ItemValue, ValidMonthsText = subscription?.ValidationText, SubscriptionName = item.SubscriptionName, SubscriptionPrice = item.Amount.ToString(), DiscountAmount = 0, ValidMonths = item.ValidToVariable, Ranking = subscription.Ranking });
			//					//UsertempSubscription.SubscriptionId = subscription.Id;
			//					//UsertempSubscription.PartCode = PartCode?.ItemValue;
			//					//UsertempSubscription.ValidMonthsText = subscription?.ValidationText;
			//					//UsertempSubscription.SubscriptionName = item.SubscriptionName;
			//					//UsertempSubscription.SubscriptionPrice = item.Amount.ToString();
			//					//UsertempSubscription.DiscountAmount = 0;
			//					//UsertempSubscription.ValidMonths = item.ValidToVariable;
			//					//UsertempSubscription.Ranking = subscription.Ranking;
			//				}


			//				SessionManagement.StoreInSession(SessionType.SubscriptionTempDtls, UserSubscription);

			//				ViewData["Message"] = "Success";
			//				return PartialView("/Views/Partials/_SubscriptionCartTeachersPrm.cshtml");
			//			}



			//			//if (UsertempSubscription != null && UsertempSubscription.Any())
			//			//{
			//			//	int? tempAddedThisSubscription = UsertempSubscription?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup))?.Count();
			//			//	UserAlreadySubscription = UserHaveSubscriptions?.Where(x => x.Ranking == Ranking.ToString() && x.AgeGroup == (UsertempSubscription?.Where(c => c.SrNo == SrNo)?.FirstOrDefault()?.AgeGroup) && x.IsActive == 1)?.SingleOrDefault();

			//			//	if (validation)
			//			//	{
			//			//		string RegistrationMode = SessionManagement.GetCurrentSession<string>(SessionType.RegistrationMode);
			//			//		string SubscriptionAvailedOrNor = SessionManagement.GetCurrentSession<string>(SessionType.SubscribedOrNot);
			//			//		int DiscountAmt = 0;

			//			//		HomeController home = new HomeController();
			//			//		var subscription = home.GetSubscriptionDetailsWithRanking_BonusWorksheet(Ranking.ToString());

			//			//		if (subscription != null)
			//			//		{
			//			//			SubscriptionItemStructureProgram item = (SubscriptionItemStructureProgram)Umbraco?.Content(subscription?.SubscriptionName?.Udi);
			//			//			NameListItem PartCode = (NameListItem)Umbraco?.Content(subscription?.StandardSelection?.Udi);
			//			//			if (item != null)
			//			//			{
			//			//				//Assign Discount Amount
			//			//				//if (!String.IsNullOrWhiteSpace(RegistrationMode) && RegistrationMode == "Existing" && !String.IsNullOrWhiteSpace(SubscriptionAvailedOrNor) && SubscriptionAvailedOrNor.ToLower() == "no")
			//			//				//{
			//			//				//	if (subscription.IsAppliedForExistingUser)
			//			//				//		DiscountAmt = subscription.DiscountAmount;
			//			//				//}

			//			//				//Assign Discount Amount End
			//			//				//UsertempSubscription.Where(x => x.Ranking == Ranking.ToString()).Select(x => { x.SubscriptionId = subscription.Id; x.PartCode = PartCode?.ItemValue; x.ValidMonthsText = subscription?.ValidationText; x.SubscriptionName = item.SubscriptionName; x.SubscriptionPrice = item.Amount.ToString(); x.DiscountAmount = DiscountAmt; x.ValidMonths = item.ValidToVariable; x.Ranking = subscription.Ranking; return x; }).ToList();
			//			//				UsertempSubscription.Add(new TempSubscriptionCreatedByUser { SubscriptionId = subscription.Id, PartCode = PartCode?.ItemValue, ValidMonthsText = subscription?.ValidationText, SubscriptionName = item.SubscriptionName, SubscriptionPrice = item.Amount.ToString(), DiscountAmount = DiscountAmt, ValidMonths = item.ValidToVariable, Ranking = subscription.Ranking });
			//			//			}
			//			//		}

			//			//		SessionManagement.StoreInSession(SessionType.SubscriptionTempDtlsBonus, UsertempSubscription);

			//			//		ViewData["Message"] = "Success";
			//			//		return PartialView("/Views/Partials/_SubscriptionCartStructurePrm.cshtml");
			//			//	}
			//			//}
			//		}
			//	}
			//	catch (Exception ex)
			//	{
			//		Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SubscriptionMapping");
			//	}


			//	return PartialView("/Views/Partials/_SubscriptionCartStructurePrm.cshtml");

			//}
			//catch (Exception ex)
			//{
			//	Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SubscriptionMapping");
			//}
			//return PartialView("/Views/Partials/_SubscriptionCartStructurePrm.cshtml");
		}

		public ActionResult SubscriptionPayLoad()
		{
			string culture = CultureName.GetCultureName().Replace("/", "");
			if (String.IsNullOrWhiteSpace(culture))
				culture = "en-US";

			_variationContextAccessor.VariationContext = new VariationContext(culture);

			return PartialView("/Views/Partials/_SubscriptionPay.cshtml");
		}

		public ActionResult SubscriptionPayLoadBonusWorksheet()
		{
			string culture = CultureName.GetCultureName().Replace("/", "");
			if (String.IsNullOrWhiteSpace(culture))
				culture = "en-US";

			_variationContextAccessor.VariationContext = new VariationContext(culture);

			return PartialView("/Views/Partials/_SubscriptionPayStructurePrm.cshtml");
		}

		public ActionResult SubscriptionCouponLoadBonusWorksheet()
		{
			string culture = CultureName.GetCultureName().Replace("/", "");
			if (String.IsNullOrWhiteSpace(culture))
				culture = "en-US";

			_variationContextAccessor.VariationContext = new VariationContext(culture);

			return PartialView("/Views/Partials/_Couponcode.cshtml");
		}

		public ActionResult SubscriptionPayLoadTeachersWorksheet()
		{
			string culture = CultureName.GetCultureName().Replace("/", "");
			if (String.IsNullOrWhiteSpace(culture))
				culture = "en-US";

			_variationContextAccessor.VariationContext = new VariationContext(culture);

			return PartialView("/Views/Partials/_SubscriptionPayTeachersPrm.cshtml");
		}
		public PartialViewResult AgeGroupLoad()
		{
			string culture = CultureName.GetCultureName().Replace("/", "");
			if (String.IsNullOrWhiteSpace(culture))
				culture = "en-US";

			_variationContextAccessor.VariationContext = new VariationContext(culture);

			return PartialView("/Views/Partials/_SubscriptionAddAgeGroup.cshtml");
		}

		public PartialViewResult AgeGroupLoadTeachers()
		{
			string culture = CultureName.GetCultureName().Replace("/", "");
			if (String.IsNullOrWhiteSpace(culture))
				culture = "en-US";

			_variationContextAccessor.VariationContext = new VariationContext(culture);

			return PartialView("/Views/Partials/_SubscriptionlistTeachersPrm.cshtml");
		}
		//Get CMS AgeGroup
		public List<NameListItem> GetSubscriptionDetails()
		{
			string culture = CultureName.GetCultureName().Replace("/", "");
			if (String.IsNullOrWhiteSpace(culture))
				culture = "en-US";

			_variationContextAccessor.VariationContext = new VariationContext(culture);

			List<NameListItem> ageGroups;
			ageGroups = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?
																  .OfType<NameListItem>().Where(x => x.IsActice).ToList();

			return ageGroups;
		}

		//Hemant Code
		//[System.Web.Http.HttpGet]
		//public ActionResult GetInvoicePDFFile()
		//{
		//	Responce responce = new Responce();
		//	try
		//	{
		//		InvoiceModel invoiceModel = new InvoiceModel();
		//		invoiceModel = GetInvoiceDetailsFromCMS();
		//		List<InvoiceData> InvoiceList = new List<InvoiceData>();
		//		List<SetParameters> invoice = new List<SetParameters>()
		//			{
		//				new SetParameters { ParameterName = "@QType", Value = "1" },
		//				new SetParameters { ParameterName = "@UserId", Value = SessionManagement.GetCurrentSession<int>(SessionType.UserId).ToString()},
		//							};
		//		dbProxy _db = new dbProxy();
		//		invoiceModel.UserEmailId = clsCommon.Decrypt(SessionManagement.GetCurrentSession<string>(SessionType.emailid));
		//		InvoiceList = _db.GetDataMultiple("GetInvoiceList", InvoiceList, invoice);
		//		invoiceModel.InvoiceList = InvoiceList;
		//		//string html = RenderRazorViewToString("/Views/Partials/PDFGeneratorPartialView/_Invoice_Partial_View.cshtml", invoiceModel,context);
		//		//responce = PDFGeneratorHelper.GetInvoicePDf(html);

		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(SubscriptionController), ex, message: "GetInvoicePDFFile");
		//		responce.StatusCode = HttpStatusCode.InternalServerError;
		//		responce.Message = ex.Message;
		//	}
		//	return Json(new
		//	{
		//		responce
		//	}, JsonRequestBehavior.AllowGet);

		//}

		[HttpPost]
		public Responce SaveInvoicePDFFile(List<HP_PLC_Doc.Models.InvoiceData> List, string TransactionId, string emailid, string specialplan)
		{
			Responce responce = new Responce();
			try
			{
				HP_PLC_Doc.Models.InvoiceModel invoiceModel = new HP_PLC_Doc.Models.InvoiceModel();
				invoiceModel = GetInvoiceDetailsFromCMS();
				invoiceModel.InvoiceList = List;
				invoiceModel.UserEmailId = emailid;

				//Generate pdf file

				//string html = RenderRazorViewToString("~/Views/PDFGeneratorPartialView/_Invoice_Partial_View.cshtml", invoiceModel);
				//responce = PDFGeneratorHelper.GetInvoicePDf(html);
				var fileName = DateTime.Now.ToString("yyMMddHHmmssff") + ".pdf";//DateTime.Now.Ticks.ToString() + ".pdf";
				PdfGeneratorController contpdf = new PdfGeneratorController();
				byte[] bytes = contpdf.PdfGenerateAndSave(invoiceModel, TransactionId, specialplan);

				if (bytes != null && bytes.Length > 0)
				{
					System.IO.File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("/Invoice/") + fileName, bytes);
					string BucketHostname = ConfigurationManager.AppSettings["SiteUrl"];

					S3BucketHelper s3BucketHelper = new S3BucketHelper();
					responce = s3BucketHelper.sendMyFileToS3Async(bytes, fileName);

					if (responce.StatusCode == System.Net.HttpStatusCode.OK)
					{
						responce.StatusCode = System.Net.HttpStatusCode.OK;
						responce.Result = responce.Result;
					}
					else
					{
						responce.StatusCode = System.Net.HttpStatusCode.OK;
						responce.Result = BucketHostname + "/invoice/" + fileName;
					}

					//try
					//{
					//	//Push file to all other code
					//	string[] servers = System.IO.File.ReadAllLines(HostingEnvironment.MapPath("/Content/Servers.txt"));

					//	foreach (string server in servers)
					//	{
					//		try
					//		{
					//			SavePDFFile(fileName, server);
					//		}
					//		catch (Exception ex)
					//		{
					//			Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Server Invoice Upload - Loop");
					//		}
					//	}
					//}
					//catch (Exception ex)
					//{
					//	Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Server Invoice Upload");
					//}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "SaveInvoicePDFFile");
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.Message;
			}
			return responce;
		}

		public Responce SavePDFFile(string FileName, string Host)
		{
			string filePath = HostingEnvironment.MapPath("/Invoice/" + FileName);

			string Username = "ftp.printlearncenter.com|userweb506";
			string Password = "at0manPeSryg";
			FtpClient client = new FtpClient(Host, 21, Username, Password);

			Responce responce = new Responce();
			try
			{
				client.ConnectTimeout = 5900000;
				client.AutoConnect();

				client.RetryAttempts = 3;
				client.UploadFile(filePath, "Html/Invoice/" + FileName,
					FtpRemoteExists.Overwrite, false, FtpVerify.Retry);

				client.Disconnect();
			}
			catch (Exception)
			{

			}
			finally
			{
				client.Disconnect();
			}
			return responce;
		}
		//public string RenderRazorViewToString(string viewName, InvoiceModel model)
		//{
		//	//var context = new ControllerContext(Request.RequestContext, new SubscriptionController());
		//	ViewData.Model = model;
		//	using (var sw = new StringWriter())
		//	{
		//		var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
		//		var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
		//		viewResult.View.Render(viewContext, sw);
		//		viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
		//		return sw.GetStringBuilder().ToString();
		//	}
		//}
		public HP_PLC_Doc.Models.InvoiceModel GetInvoiceDetailsFromCMS()
		{
			HP_PLC_Doc.Models.InvoiceModel invoice = new HP_PLC_Doc.Models.InvoiceModel();
			try
			{
				//var hjj = Umbraco.Content(4532);
				var helper = Current.UmbracoHelper;
				IPublishedContent node = helper?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault().Children?.Where(x => x.ContentType.Alias == ConstantDocType.InvoiceDocType)?.FirstOrDefault();
				//IPublishedContent node = helper.Content(Constant.InvoiceDetails);
				if (node != null)
				{
					invoice.Name = node.Name;
					if (node.HasProperty("address"))
					{
						invoice.HasAddress = true;
						invoice.Address = node.Value("address").ToString();
					}
					if (node.HasProperty("gSTNo"))
					{
						invoice.HasGSTNo = true;
						invoice.GSTNo = node.Value("gSTNo").ToString();
					}
					if (node.HasProperty("sGST"))
					{
						invoice.HasSGST = true;
						invoice.SGST = Convert.ToInt32(node.Value("sGST").ToString());
					}
					if (node.HasProperty("sGST"))
					{
						invoice.HasCGST = true;
						invoice.CGST = Convert.ToInt32(node.Value("sGST").ToString());
					}
					if (node.HasProperty("logo"))
					{
						invoice.Logo = node.Value<IPublishedContent>("logo")?.Url();
					}

					invoice.InvoiceTitle = node.Value("invoiceTitle").ToString();

					invoice.InvoiceFontFamily = node.Value("invoiceFontFamily").ToString();

					invoice.InvoiceFontSize = Convert.ToInt32(node.Value("invoiceFontSize"));

					invoice.ComputerGeneratedText = node.Value("computerGeneratedText").ToString();
					invoice.BelowAddress = node.Value("belowAddress").ToString();
					invoice.PlaceOfSupply = node.Value("placeOfSupply").ToString();
					invoice.SAC = node.Value("sAC").ToString();
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "GetInvoiceDetailsFromCMS");
			}

			return invoice;

		}
		public int GetHasAlreadyRererralExcelUser(int UserId)
		{
			int count = 0;
			try
			{
				if (UserId > 0)
				{
					List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() }
				};
					dbProxy _db = new dbProxy();
					GetStatus getStatus = new GetStatus();
					List<ReferralFromExcelUser> List = new List<ReferralFromExcelUser>();
					List = _db.GetDataMultiple("GetReferralFromExcel", List, sp);
					return List.Count();
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "GetHasAlreadyRererralExcelUser");
				return 0;
			}
			return count;
		}

		public async Task<ActionResult> PayNow(string CouponCode)
		{
			try
			{
				bool IsValid = false;
				bool validation = true;
				decimal payableAmount = 0;
				//decimal ExistingUserDiscountedAmount = 0;
				decimal CouponDiscountAmount = 0;
				//decimal SubscriptionbAmount = 0;

				dbProxy _db = new dbProxy();
				GetStatus dbresponse = new GetStatus();

				//SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
				LoggedIn loggedIn = new LoggedIn();
				string IsLoggedIn = SessionManagement.GetCurrentSession<string>(SessionType.IsLoggedIn);
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
				loggedIn = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

				CouponCodeResponse couponCodeResponse = new CouponCodeResponse();
				couponCodeResponse = SessionManagement.GetCurrentSession<CouponCodeResponse>(SessionType.CouponTempDtls);

				string Serial_UniqueId, Shoppingcartdetails = String.Empty;
				if (IsLoggedIn == "Y")
				{
					//entry of subscription
					List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

					try
					{
						//Coupon Code Session
						if (couponCodeResponse != null)
						{
							string response = AvailCouponCode(couponCodeResponse.CouponCodeName, "lesson", false);
							if (!String.IsNullOrEmpty(response))
							{
								string[] code = response.Split(',');
								if (code.Length > 0)
								{
									if (code[0] == "0")
									{
										return Json(new { status = "Fail", navigation = "", message = code[1].ToString() }, JsonRequestBehavior.AllowGet);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Pay Now - Coupon Code Logic");
					}

					//Validate all subscription has been mapped
					if (UsertempSubscription != null && UsertempSubscription.Any())
					{
						foreach (var subsvald in UsertempSubscription)
						{
							if ((String.IsNullOrEmpty(subsvald.Ranking.ToString()) || Convert.ToInt32(subsvald.Ranking) == 1) || String.IsNullOrEmpty(subsvald.AgeGroup) || String.IsNullOrEmpty(subsvald.SubscriptionId.ToString()) || String.IsNullOrEmpty(subsvald.ValidMonths.ToString()))
							{
								validation = false;
								return Json(new { status = "Fail", navigation = "", message = "Please select plan" }, JsonRequestBehavior.AllowGet);
							}
						}
						//Check Subscription Item multiple for same age group
						var CntDuplcates = UsertempSubscription?.GroupBy(x => new { x.Ranking, x.AgeGroup }).Any(g => g.Count() > 1);
						if (CntDuplcates == true)
						{
							validation = false;
							return Json(new { status = "Fail", navigation = "", message = "Duplicate subscription with same age group can not apply." }, JsonRequestBehavior.AllowGet);
						}

						//Check not be avail coupon discount amount for multiple items
						//if (!String.IsNullOrWhiteSpace(CouponCode) && couponCodeResponse.BenefitRestrict == 1)
						//{
						//	var CntCouponAmt = UsertempSubscription?.GroupBy(x => new { x.Ranking }).Any(g => g.Count() > 1);
						//	if (CntCouponAmt == true)
						//	{
						//		validation = false;
						//		return Json(new { status = "Fail", navigation = "", message = "Please select only one age group under 899 plan to avail the offer" }, JsonRequestBehavior.AllowGet);
						//	}
						//}
					}

					if (validation)
					{
						string TransactionId = DateTime.Now.ToString("yyMMddHHmmssff") + Guid.NewGuid().ToString().Substring(0, 5).Replace("-", "");
						string entrystatus = String.Empty;

						if (!String.IsNullOrEmpty(TransactionId))
						{
							if (UsertempSubscription != null && UsertempSubscription.Any())
							{
								foreach (var subs in UsertempSubscription)
								{
									if ((!String.IsNullOrEmpty(subs.Ranking.ToString()) && Convert.ToInt32(subs.Ranking) > 1) && !String.IsNullOrEmpty(subs.AgeGroup))
									{
										List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
										new SetParameters{ ParameterName = "@subscriptionId", Value = subs.SubscriptionId.ToString() },
										new SetParameters{ ParameterName = "@subscriptionName", Value = subs.SubscriptionName },
										new SetParameters{ ParameterName = "@subscriptionPrice", Value = subs.SubscriptionPrice },
										new SetParameters{ ParameterName = "@SubscriptionDuration", Value = subs.ValidMonths.ToString() },
										new SetParameters{ ParameterName = "@SubscriptionValidation", Value = subs.ValidMonthsText },
										new SetParameters{ ParameterName = "@Ranking", Value = subs.Ranking.ToString() },
										new SetParameters{ ParameterName = "@AgeGroup", Value = subs.AgeGroup },
										new SetParameters{ ParameterName = "@AutopaymentId", Value = TransactionId },
										new SetParameters{ ParameterName = "@PartCode", Value = subs.PartCode },
										new SetParameters{ ParameterName = "@DiscountAmt", Value = subs.DiscountAmount.ToString() }
									};
										dbresponse = _db.StoreData("USP_AddSubscriptions_Primary", sp);
									}
									else
									{
										entrystatus = "fail";
										return Json(new { status = "Fail", navigation = "", message = "Subscription data is not valid." }, JsonRequestBehavior.AllowGet);
									}

									if (String.IsNullOrEmpty(dbresponse.returnStatus) || dbresponse.returnStatus == "Fail")
									{
										entrystatus = "fail";
										return Json(new { status = "Fail", navigation = "", message = dbresponse.returnMessage }, JsonRequestBehavior.AllowGet);
									}
									else
									{ entrystatus = "ok"; }

									//discountedAmount += subs.DiscountAmount;
									//payableAmount += Convert.ToInt32(subs.SubscriptionPrice);
								}

								//Payable amount
								//payableAmount = payableAmount - discountedAmount;

								SubscriptionAmountCalc subscriptionAmountCalc = new SubscriptionAmountCalc();
								//SubscriptionbAmount = subscriptionAmountCalc.GetSubscriptionAmount();
								//ExistingUserDiscountedAmount = subscriptionAmountCalc.GetExistingUserDiscountAmount();
								CouponDiscountAmount = subscriptionAmountCalc.GetCouponDiscountAmount();
								payableAmount = subscriptionAmountCalc.GetPayableAmount();

								//Coupon Code update
								if (CouponDiscountAmount > 0 && couponCodeResponse != null)
								{
									GetStatus responce = new GetStatus();
									List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
										new SetParameters{ ParameterName = "@CouponCodeId", Value = couponCodeResponse.CouponCodeId.ToString() },
										new SetParameters{ ParameterName = "@CouponAvailedUniqueId", Value = couponCodeResponse.CouponAvailedUniqueId.ToString() },
										new SetParameters{ ParameterName = "@DiscountAvailed", Value = CouponDiscountAmount.ToString() },
										new SetParameters{ ParameterName = "@TransactionCode", Value = TransactionId }
									};

									responce = _db.StoreData("USP_AddCouponCodeAvailed", sp);
								}


								if (entrystatus == "ok" && payableAmount > 0)
								{
									//payment gateway
									//string payNowUrl = ConfigurationManager.AppSettings["paymentUrl"].ToString();

									String response = "";
									RequestURL objRequestURL = new RequestURL();
									//string ErrorFile = ConfigurationManager.AppSettings["ErrorFile"].ToString();
									//string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
									string TPSLService = ConfigurationManager.AppSettings["TPSLService"].ToString();
									//string IsFixedPath = ConfigurationManager.AppSettings["IsFixedPath"].ToString();
									//string IsCustomLog = ConfigurationManager.AppSettings["IsCustomLog"].ToString();
									string EncryptKey = ConfigurationManager.AppSettings["EncryptKey"].ToString();
									string EncryptIV = ConfigurationManager.AppSettings["EncryptIV"].ToString();
									string RequestType = ConfigurationManager.AppSettings["RequestType"].ToString();
									string MerchantCode = ConfigurationManager.AppSettings["MerchantCode"].ToString();

									//Embed culture name in URL
									string ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"].ToString();
									if (!String.IsNullOrWhiteSpace(ReturnUrl))
									{
										string culture = CultureName.GetCultureName().Replace("/", "");
										if (!String.IsNullOrWhiteSpace(culture))
										{
											Uri myUri = new Uri(ReturnUrl);
											string host = myUri.Authority;
											string absolutepath = myUri.AbsolutePath;

											ReturnUrl = myUri.Scheme + "://" + host + "/" + culture + absolutepath;
										}
									}

									string SchemeCode = ConfigurationManager.AppSettings["SchemeCode"].ToString();
									string PayUserId = ConfigurationManager.AppSettings["UserId"].ToString();
									string PayPassword = ConfigurationManager.AppSettings["Password"].ToString();

									SessionManagement.StoreInSession(SessionType.PaymentId, TransactionId);
									objRequestURL.strIgnoreSSL = "Y";
									//objRequestURL.LogFilePath = Server.MapPath("LogFilePath");
									objRequestURL.TPSLService = TPSLService;
									//objRequestURL.ErrorFile = "~/DotnetIntegrationKit\\ErrorMessage.property";

									if (!String.IsNullOrEmpty(loggedIn.u_name))
										_userName = clsCommon.Decrypt(loggedIn.u_name);

									if (!String.IsNullOrEmpty(loggedIn.u_email))
										_userEmail = clsCommon.Decrypt(loggedIn.u_email);

									if (!String.IsNullOrEmpty(loggedIn.u_whatsappno))
									{
										_userMobile = clsCommon.Decrypt(loggedIn.u_whatsappno);
										if (_userMobile.Contains("+"))
											_userMobile = _userMobile.Replace("+91", "");
									}

									Serial_UniqueId = TransactionId;// "19042021192316";

									string IsPaymentModeLive = ConfigurationManager.AppSettings["IsPaymentModeLive"].ToString();
									if (IsPaymentModeLive == "N")
									{
										//For testing data
										payableAmount = 1;
										Shoppingcartdetails = SchemeCode + "_" + payableAmount + "_0.0";
									}
									else if (IsPaymentModeLive == "Y")
									{
										//For live data
										Shoppingcartdetails = SchemeCode + "_" + payableAmount + "_0.0";
									}

									objRequestURL.strUserName = PayUserId;
									objRequestURL.strPassword = PayPassword;

									//objRequestURL.strUserName = Username;
									//objRequestURL.strPassword = Password;

									if (payableAmount > 0)
									{
										if (RequestType.ToUpper() == "T" && (Serial_UniqueId != null || Serial_UniqueId != "" || Serial_UniqueId != "0"))
										{
											ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
											response = objRequestURL.SendRequest
													  (
														RequestType
													  , MerchantCode
													  , Serial_UniqueId
													  , _userName == null ? _userEmail : _userName
													  , payableAmount.ToString()//"1.00"//
													  , "INR"
													  , TransactionId
													  , ReturnUrl
													  , "NA"
													  , "NA"
													  , Shoppingcartdetails
													  , DateTime.Now.ToString("dd-MM-yyyy")
													  , _userEmail
													  , _userMobile
													  , "NA"
													  , _userName == null ? _userEmail : _userName
													  , "NA"
													  , "NA"
													  , EncryptKey
													  , EncryptIV
													  );
										}

										String strResponse = response.ToUpper();

										LogWriter Log = null;
										Log = new LogWriter(strResponse.ToString(), "");

										if (strResponse.StartsWith("ERROR"))
										{
											if (strResponse == "ERROR073")
											{
												IsValid = false;
												response = objRequestURL.SendRequest
														   (
															  RequestType
															  , MerchantCode
															  , Serial_UniqueId
															  , _userName == null ? _userEmail : _userName
															  , payableAmount.ToString()//"1.00"//
															  , "INR"
															  , TransactionId
															  , ReturnUrl
															  , "NA"
															  , "NA"
															  , Shoppingcartdetails
															  , DateTime.Now.ToString("dd-MM-yyyy")
															  , _userEmail
															  , _userMobile
															  , "NA"
															  , _userName == null ? _userEmail : _userName
															  , "NA"
															  , "NA"
															  , EncryptKey
															  , EncryptIV
													);
												strResponse = response.ToUpper();
											}
											else
											{
												Log = new LogWriter(response.ToString() + " ERROR073", "");
											}
										}
										else
										{
											IsValid = true;
										}
									}
									//Update In Database
									try
									{
										List<SetParameters> spp = new List<SetParameters>()
										{
											new SetParameters{ ParameterName = "@PaymentId", Value = TransactionId },
											new SetParameters{ ParameterName = "@subscriptionStatus", Value = "Valid" },
											new SetParameters{ ParameterName = "@subscriptionUrlResponse", Value = response }
										};

										_db.StoreData("usp_UpdatePaymentLinkStatus", spp);
									}
									catch { }

									if (IsValid)
									{
										//bool IsAppliedForFree = subscptDetails.Value<bool>("isAppliedFree");
										bool IsAppliedForFree = false;
										//Check HP Employee Coupn
										if (!String.IsNullOrEmpty(CouponCode) && IsAppliedForFree == true)
										{
											try
											{
												GetStatus couponStatus = new GetStatus();
												List<SetParameters> spCpn = new List<SetParameters>()
													{
														new SetParameters{ ParameterName = "@CouponCode", Value = CouponCode },
														new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() }
													};

												couponStatus = _db.GetData("[dbo].[USP_HPEmployeesCoupon]", couponStatus, spCpn);
												if (couponStatus != null && couponStatus.returnStatus != null)
												{
													if (couponStatus.returnStatus == "Success")
													{
														SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
														dbAccessClass dbClass = new dbAccessClass();
														PaymentStatus paymentStatus = new PaymentStatus();
														paymentStatus.PaymentId = TransactionId;
														paymentStatus.txn_status = "Direct";
														paymentStatus.txn_msg = "Direct";
														paymentStatus.txn_err_msg = "Direct";
														paymentStatus.clnt_txn_ref = "Direct";
														paymentStatus.tpsl_bank_cd = "Direct";
														paymentStatus.tpsl_txn_id = "Direct";
														paymentStatus.txn_amt = "Direct";
														paymentStatus.tpsl_txn_time = "Direct";
														paymentStatus.tpsl_rfnd_id = "Direct";
														paymentStatus.bal_amt = "Direct";
														paymentStatus.rqst_token = "Direct";
														paymentStatus.PaymentMode = "Direct";

														returnParam = PaymentAndSubscriptionInput(paymentStatus, null);
														if (!String.IsNullOrEmpty(returnParam.Amount))
															return Json(new { status = "Success", navigation = "", message = "home" }, JsonRequestBehavior.AllowGet);
														else
															return Json(new { status = "Fail", navigation = "", message = "Transaction not successfull!!" }, JsonRequestBehavior.AllowGet);
													}
													else
													{
														return Json(new { status = "Fail", navigation = "", message = couponStatus.returnMessage }, JsonRequestBehavior.AllowGet);
													}
												}
											}
											catch (Exception ex)
											{
												Logger.Error(reporting: typeof(HomeController), ex, message: "Pay Now - Coupon Block");
											}
										}
										else
										{
											return Json(new { status = "Success", navigation = "", message = response }, JsonRequestBehavior.AllowGet);
										}
									}
									else
									{
										return Json(new { status = "Fail", navigation = "", message = "Payment can not be proceed!!!" }, JsonRequestBehavior.AllowGet);
									}
								}
								else if (payableAmount == 0)//when amount is 0
								{
									PostPayUser postpay = new PostPayUser();
									dbAccessClass db;

									try
									{
										db = new dbAccessClass();
										postpay = db.PostPayUserDtls(TransactionId);
									}
									catch (Exception ex)
									{
										Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Payment 0 Get Post pay data");
									}

									if (postpay != null)
									{
										try
										{
											PaymentStatus paymentStatus = new PaymentStatus();
											paymentStatus.txn_status = "Direct";
											paymentStatus.txn_msg = "Direct";
											paymentStatus.txn_err_msg = "Direct";
											paymentStatus.clnt_txn_ref = "Direct";
											paymentStatus.tpsl_bank_cd = "Direct";
											paymentStatus.tpsl_txn_id = "Direct";
											paymentStatus.txn_amt = "Direct";
											paymentStatus.tpsl_txn_time = "Direct";
											paymentStatus.tpsl_rfnd_id = "Direct";
											paymentStatus.bal_amt = "Direct";
											paymentStatus.rqst_token = "Direct";
											paymentStatus.PaymentMode = "Direct";
											paymentStatus.PaymentId = TransactionId;

											SubscriptionSuccessParam payData = new SubscriptionSuccessParam();
											payData = PaymentAndSubscriptionInput(paymentStatus, postpay);

											string IsEnableTrackerCode = ConfigurationManager.AppSettings["IsEnableTrackerCode"].ToString();
											if (payData.InvoiceData != null && IsEnableTrackerCode == "Y")
											{
												SessionManagement.StoreInSession(SessionType.PayResponseTracker, payData);
											}
										}
										catch (Exception ex)
										{
											Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Payment 0 only entry in Db");
										}

										//set user subscribed because 0 amount subscription can not login again
										SessionManagement.StoreInSession(SessionType.SubscribedOrNot, "Yes");

										//Get all subscription
										List<GetUserCurrentSubscription> mySubscription = new List<GetUserCurrentSubscription>();
										dbAccessClass _dbs = new dbAccessClass();
										mySubscription = _dbs.GetUserSubscriptions();
										SessionManagement.StoreInSession(SessionType.SubscriptionInDtls, mySubscription);

										try
										{
											LoggedIn loggedInData = new LoggedIn();
											//Get User LoggedIn Data
											List<SetParameters> sp = new List<SetParameters>()
											{
												new SetParameters{ ParameterName = "@QType", Value = "10" },
												new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
											};

											loggedInData = _db.GetData<LoggedIn>("usp_getdata", loggedInData, sp);
											if (loggedInData != null && loggedInData.ResponseText == "Success" && loggedInData.UserId > 0)
											{
												SessionManagement.StoreInSession(SessionType.LoggedInDtls, loggedInData);
											}
										}
										catch { }


										//temp session delete
										SessionManagement.DeleteFromSession(SessionType.SubscriptionTempDtls);
										SessionManagement.DeleteFromSession(SessionType.IsCouponCodeOffer);
										SessionManagement.DeleteFromSession(SessionType.CouponCode);
										SessionManagement.DeleteFromSession(SessionType.ExpertTalkId);
										//Exired Bundling Cookie here
										Response.Cookies["IsBundleUser"].Expires = DateTime.Now.AddMinutes(-1);
										Response.Cookies["IsOfferUser"].Expires = DateTime.Now.AddMinutes(-1);

										return Json(new { status = "Success", navigation = "", message = "lesson" }, JsonRequestBehavior.AllowGet);

									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Pay Now - Main Block");
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public async Task<ActionResult> PayNowstructureprogram(string CouponCode)
		{
			try
			{
				bool IsValid = false;
				bool validation = true;
				decimal payableAmount = 0;
				//decimal ExistingUserDiscountedAmount = 0;
				decimal CouponDiscountAmount = 0;
				//decimal SubscriptionbAmount = 0;

				dbProxy _db = new dbProxy();
				GetStatus dbresponse = new GetStatus();

				//SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
				LoggedIn loggedIn = new LoggedIn();
				string IsLoggedIn = SessionManagement.GetCurrentSession<string>(SessionType.IsLoggedIn);
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
				loggedIn = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

				CouponCodeResponse couponCodeResponse = new CouponCodeResponse();
				couponCodeResponse = SessionManagement.GetCurrentSession<CouponCodeResponse>(SessionType.CouponTempDtls);

				string Serial_UniqueId, Shoppingcartdetails = String.Empty;
				if (IsLoggedIn == "Y")
				{
					//entry of subscription
					List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
					UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

					try
					{
						//Coupon Code Session
						if (couponCodeResponse != null)
						{
							string response = AvailCouponCode(couponCodeResponse.CouponCodeName, "bonus", false);
							if (!String.IsNullOrEmpty(response))
							{
								string[] code = response.Split(',');
								if (code.Length > 0)
								{
									if (code[0] == "0")
									{
										return Json(new { status = "Fail", navigation = "", message = code[1].ToString() }, JsonRequestBehavior.AllowGet);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Pay Now - Coupon Code Logic");
					}

					//Validate all subscription has been mapped
					if (UsertempSubscription != null && UsertempSubscription.Any())
					{
						foreach (var subsvald in UsertempSubscription)
						{
							if ((String.IsNullOrEmpty(subsvald.Ranking.ToString()) || Convert.ToInt32(subsvald.Ranking) == 1) || String.IsNullOrEmpty(subsvald.SubscriptionId.ToString()) || String.IsNullOrEmpty(subsvald.ValidMonths.ToString()))
							{
								validation = false;
								return Json(new { status = "Fail", navigation = "", message = "Please select plan" }, JsonRequestBehavior.AllowGet);
							}
						}
						//Check Subscription Item multiple for same age group
						//var CntDuplcates = UsertempSubscription?.GroupBy(x => new { x.Ranking, x.AgeGroup }).Any(g => g.Count() > 1);
						//if (CntDuplcates == true)
						//{
						//	validation = false;
						//	return Json(new { status = "Fail", navigation = "", message = "Duplicate subscription with same age group can not apply." }, JsonRequestBehavior.AllowGet);
						//}

						//Check not be avail coupon discount amount for multiple items
						//if (!String.IsNullOrWhiteSpace(CouponCode) && couponCodeResponse.BenefitRestrict == 1)
						//{
						//	var CntCouponAmt = UsertempSubscription?.GroupBy(x => new { x.Ranking }).Any(g => g.Count() > 1);
						//	if (CntCouponAmt == true)
						//	{
						//		validation = false;
						//		return Json(new { status = "Fail", navigation = "", message = "Please select only one age group under 899 plan to avail the offer" }, JsonRequestBehavior.AllowGet);
						//	}
						//}
					}

					if (validation)
					{
						string TransactionId = DateTime.Now.ToString("yyMMddHHmmssff") + Guid.NewGuid().ToString().Substring(0, 5).Replace("-", "");
						string entrystatus = String.Empty;

						if (!String.IsNullOrEmpty(TransactionId))
						{
							if (UsertempSubscription != null && UsertempSubscription.Any())
							{
								foreach (var subs in UsertempSubscription)
								{
									if ((!String.IsNullOrEmpty(subs.Ranking.ToString()) && Convert.ToInt32(subs.Ranking) > 1))
									{
										List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
										new SetParameters{ ParameterName = "@subscriptionId", Value = subs.SubscriptionId.ToString() },
										new SetParameters{ ParameterName = "@subscriptionName", Value = subs.SubscriptionName },
										new SetParameters{ ParameterName = "@subscriptionPrice", Value = subs.SubscriptionPrice },
										new SetParameters{ ParameterName = "@SubscriptionDuration", Value = subs.ValidMonths.ToString() },
										new SetParameters{ ParameterName = "@SubscriptionValidation", Value = subs.ValidMonthsText },
										new SetParameters{ ParameterName = "@Ranking", Value = subs.Ranking.ToString() },
										new SetParameters{ ParameterName = "@AgeGroup", Value = subs.AgeGroup == null ? "" : subs.AgeGroup},
										new SetParameters{ ParameterName = "@AutopaymentId", Value = TransactionId },
										new SetParameters{ ParameterName = "@PartCode", Value = subs.PartCode == null ? "" : subs.PartCode },
										new SetParameters{ ParameterName = "@DiscountAmt", Value = subs.DiscountAmount.ToString() }
									};
										dbresponse = _db.StoreData("USP_AddSubscriptions_Primary_BonusWorksheet", sp);
									}
									else
									{
										entrystatus = "fail";
										return Json(new { status = "Fail", navigation = "", message = "Subscription data is not valid." }, JsonRequestBehavior.AllowGet);
									}

									if (String.IsNullOrEmpty(dbresponse.returnStatus) || dbresponse.returnStatus == "Fail")
									{
										entrystatus = "fail";
										return Json(new { status = "Fail", navigation = "", message = dbresponse.returnMessage }, JsonRequestBehavior.AllowGet);
									}
									else
									{ entrystatus = "ok"; }

									//discountedAmount += subs.DiscountAmount;
									//payableAmount += Convert.ToInt32(subs.SubscriptionPrice);
								}

								//Payable amount
								//payableAmount = payableAmount - discountedAmount;

								SubscriptionAmountCalc subscriptionAmountCalc = new SubscriptionAmountCalc();
								//SubscriptionbAmount = subscriptionAmountCalc.GetSubscriptionAmount();
								//ExistingUserDiscountedAmount = subscriptionAmountCalc.GetExistingUserDiscountAmount();
								CouponDiscountAmount = subscriptionAmountCalc.GetCouponDiscountAmount();
								payableAmount = subscriptionAmountCalc.GetPayableAmount();

								//Coupon Code update
								if (CouponDiscountAmount > 0 && couponCodeResponse != null)
								{
									GetStatus responce = new GetStatus();
									List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
										new SetParameters{ ParameterName = "@CouponCodeId", Value = couponCodeResponse.CouponCodeId.ToString() },
										new SetParameters{ ParameterName = "@CouponAvailedUniqueId", Value = couponCodeResponse.CouponAvailedUniqueId.ToString() },
										new SetParameters{ ParameterName = "@DiscountAvailed", Value = CouponDiscountAmount.ToString() },
										new SetParameters{ ParameterName = "@TransactionCode", Value = TransactionId }
									};

									responce = _db.StoreData("USP_AddCouponCodeAvailed", sp);
								}


								if (entrystatus == "ok" && payableAmount > 0)
								{
									//payment gateway
									//string payNowUrl = ConfigurationManager.AppSettings["paymentUrl"].ToString();

									String response = "";
									RequestURL objRequestURL = new RequestURL();
									//string ErrorFile = ConfigurationManager.AppSettings["ErrorFile"].ToString();
									//string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
									string TPSLService = ConfigurationManager.AppSettings["TPSLService"].ToString();
									//string IsFixedPath = ConfigurationManager.AppSettings["IsFixedPath"].ToString();
									//string IsCustomLog = ConfigurationManager.AppSettings["IsCustomLog"].ToString();
									string EncryptKey = ConfigurationManager.AppSettings["EncryptKey"].ToString();
									string EncryptIV = ConfigurationManager.AppSettings["EncryptIV"].ToString();
									string RequestType = ConfigurationManager.AppSettings["RequestType"].ToString();
									string MerchantCode = ConfigurationManager.AppSettings["MerchantCode"].ToString();

									//Embed culture name in URL
									string ReturnUrl = ConfigurationManager.AppSettings["StructurePrmReturnUrl"].ToString();
									if (!String.IsNullOrWhiteSpace(ReturnUrl))
									{
										string culture = CultureName.GetCultureName().Replace("/", "");
										if (!String.IsNullOrWhiteSpace(culture))
										{
											Uri myUri = new Uri(ReturnUrl);
											string host = myUri.Authority;
											string absolutepath = myUri.AbsolutePath;

											ReturnUrl = myUri.Scheme + "://" + host + "/" + culture + absolutepath;
										}
									}

									string SchemeCode = ConfigurationManager.AppSettings["SchemeCode"].ToString();
									string PayUserId = ConfigurationManager.AppSettings["UserId"].ToString();
									string PayPassword = ConfigurationManager.AppSettings["Password"].ToString();

									SessionManagement.StoreInSession(SessionType.PaymentId, TransactionId);
									objRequestURL.strIgnoreSSL = "Y";
									//objRequestURL.LogFilePath = Server.MapPath("LogFilePath");
									objRequestURL.TPSLService = TPSLService;
									//objRequestURL.ErrorFile = "~/DotnetIntegrationKit\\ErrorMessage.property";

									if (!String.IsNullOrEmpty(loggedIn.u_name))
										_userName = clsCommon.Decrypt(loggedIn.u_name);

									if (!String.IsNullOrEmpty(loggedIn.u_email))
										_userEmail = clsCommon.Decrypt(loggedIn.u_email);

									if (!String.IsNullOrEmpty(loggedIn.u_whatsappno))
									{
										_userMobile = clsCommon.Decrypt(loggedIn.u_whatsappno);
										if (_userMobile.Contains("+"))
											_userMobile = _userMobile.Replace("+91", "");
									}

									Serial_UniqueId = TransactionId;// "19042021192316";

									string IsPaymentModeLive = ConfigurationManager.AppSettings["IsPaymentModeLive"].ToString();
									if (IsPaymentModeLive == "N")
									{
										//For testing data
										payableAmount = 1;
										Shoppingcartdetails = SchemeCode + "_" + payableAmount + "_0.0";
									}
									else if (IsPaymentModeLive == "Y")
									{
										//For live data
										Shoppingcartdetails = SchemeCode + "_" + payableAmount + "_0.0";
									}

									objRequestURL.strUserName = PayUserId;
									objRequestURL.strPassword = PayPassword;

									//objRequestURL.strUserName = Username;
									//objRequestURL.strPassword = Password;

									if (payableAmount > 0)
									{
										if (RequestType.ToUpper() == "T" && (Serial_UniqueId != null || Serial_UniqueId != "" || Serial_UniqueId != "0"))
										{
											ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
											response = objRequestURL.SendRequest
													  (
														RequestType
													  , MerchantCode
													  , Serial_UniqueId
													  , _userName == null ? _userEmail : _userName
													  , payableAmount.ToString()//"1.00"//
													  , "INR"
													  , TransactionId
													  , ReturnUrl
													  , "NA"
													  , "NA"
													  , Shoppingcartdetails
													  , DateTime.Now.ToString("dd-MM-yyyy")
													  , _userEmail
													  , _userMobile
													  , "NA"
													  , _userName == null ? _userEmail : _userName
													  , "NA"
													  , "NA"
													  , EncryptKey
													  , EncryptIV
													  );
										}

										String strResponse = response.ToUpper();

										LogWriter Log = null;
										Log = new LogWriter(strResponse.ToString(), "");

										if (strResponse.StartsWith("ERROR"))
										{
											if (strResponse == "ERROR073")
											{
												IsValid = false;
												response = objRequestURL.SendRequest
														   (
															  RequestType
															  , MerchantCode
															  , Serial_UniqueId
															  , _userName == null ? _userEmail : _userName
															  , payableAmount.ToString()//"1.00"//
															  , "INR"
															  , TransactionId
															  , ReturnUrl
															  , "NA"
															  , "NA"
															  , Shoppingcartdetails
															  , DateTime.Now.ToString("dd-MM-yyyy")
															  , _userEmail
															  , _userMobile
															  , "NA"
															  , _userName == null ? _userEmail : _userName
															  , "NA"
															  , "NA"
															  , EncryptKey
															  , EncryptIV
													);
												strResponse = response.ToUpper();
											}
											else
											{
												Log = new LogWriter(response.ToString() + " ERROR073", "");
											}
										}
										else
										{
											IsValid = true;
										}
									}
									//Update In Database
									try
									{
										List<SetParameters> spp = new List<SetParameters>()
										{
											new SetParameters{ ParameterName = "@PaymentId", Value = TransactionId },
											new SetParameters{ ParameterName = "@subscriptionStatus", Value = "Valid" },
											new SetParameters{ ParameterName = "@subscriptionUrlResponse", Value = response }
										};

										_db.StoreData("usp_UpdatePaymentLinkStatus", spp);
									}
									catch { }

									if (IsValid)
									{
										//bool IsAppliedForFree = subscptDetails.Value<bool>("isAppliedFree");
										bool IsAppliedForFree = false;
										//Check HP Employee Coupn
										if (!String.IsNullOrEmpty(CouponCode) && IsAppliedForFree == true)
										{
											try
											{
												GetStatus couponStatus = new GetStatus();
												List<SetParameters> spCpn = new List<SetParameters>()
													{
														new SetParameters{ ParameterName = "@CouponCode", Value = CouponCode },
														new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() }
													};

												couponStatus = _db.GetData("[dbo].[USP_HPEmployeesCoupon]", couponStatus, spCpn);
												if (couponStatus != null && couponStatus.returnStatus != null)
												{
													if (couponStatus.returnStatus == "Success")
													{
														SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
														dbAccessClass dbClass = new dbAccessClass();
														PaymentStatus paymentStatus = new PaymentStatus();
														paymentStatus.PaymentId = TransactionId;
														paymentStatus.txn_status = "Direct";
														paymentStatus.txn_msg = "Direct";
														paymentStatus.txn_err_msg = "Direct";
														paymentStatus.clnt_txn_ref = "Direct";
														paymentStatus.tpsl_bank_cd = "Direct";
														paymentStatus.tpsl_txn_id = "Direct";
														paymentStatus.txn_amt = "Direct";
														paymentStatus.tpsl_txn_time = "Direct";
														paymentStatus.tpsl_rfnd_id = "Direct";
														paymentStatus.bal_amt = "Direct";
														paymentStatus.rqst_token = "Direct";
														paymentStatus.PaymentMode = "Direct";

														returnParam = PaymentAndSubscriptionInput(paymentStatus, null);
														if (!String.IsNullOrEmpty(returnParam.Amount))
															return Json(new { status = "Success", navigation = "", message = "home" }, JsonRequestBehavior.AllowGet);
														else
															return Json(new { status = "Fail", navigation = "", message = "Transaction not successfull!!" }, JsonRequestBehavior.AllowGet);
													}
													else
													{
														return Json(new { status = "Fail", navigation = "", message = couponStatus.returnMessage }, JsonRequestBehavior.AllowGet);
													}
												}
											}
											catch (Exception ex)
											{
												Logger.Error(reporting: typeof(HomeController), ex, message: "Pay Now - Coupon Block");
											}
										}
										else
										{
											return Json(new { status = "Success", navigation = "", message = response }, JsonRequestBehavior.AllowGet);
										}
									}
									else
									{
										return Json(new { status = "Fail", navigation = "", message = "Payment can not be proceed!!!" }, JsonRequestBehavior.AllowGet);
									}
								}
								else if (payableAmount == 0)//when amount is 0
								{
									PostPayUser postpay = new PostPayUser();
									dbAccessClass db;

									try
									{
										db = new dbAccessClass();
										postpay = db.PostPayUserDtls_Bonus(TransactionId);
									}
									catch (Exception ex)
									{
										Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Payment 0 Get Post pay data");
									}

									if (postpay != null)
									{
										try
										{
											PaymentStatus paymentStatus = new PaymentStatus();
											paymentStatus.txn_status = "Direct";
											paymentStatus.txn_msg = "Direct";
											paymentStatus.txn_err_msg = "Direct";
											paymentStatus.clnt_txn_ref = "Direct";
											paymentStatus.tpsl_bank_cd = "Direct";
											paymentStatus.tpsl_txn_id = "Direct";
											paymentStatus.txn_amt = "Direct";
											paymentStatus.tpsl_txn_time = "Direct";
											paymentStatus.tpsl_rfnd_id = "Direct";
											paymentStatus.bal_amt = "Direct";
											paymentStatus.rqst_token = "Direct";
											paymentStatus.PaymentMode = "Direct";
											paymentStatus.PaymentId = TransactionId;

											SubscriptionSuccessParam payData = new SubscriptionSuccessParam();
											payData = PaymentAndSubscriptionInputBonus(paymentStatus, postpay);

											string IsEnableTrackerCode = ConfigurationManager.AppSettings["IsEnableTrackerCode"].ToString();
											if (payData.InvoiceData != null && IsEnableTrackerCode == "Y")
											{
												SessionManagement.StoreInSession(SessionType.PayResponseTracker, payData);
											}
										}
										catch (Exception ex)
										{
											Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Payment 0 only entry in Db");
										}

										//set user subscribed because 0 amount subscription can not login again
										SessionManagement.StoreInSession(SessionType.SubscribedOrNot, "Yes");
										SessionManagement.StoreInSession(SessionType.SubscribedOrNotBonus, "1");

										//Get all subscription
										List<GetUserCurrentSubscription> mySubscription = new List<GetUserCurrentSubscription>();
										dbAccessClass _dbs = new dbAccessClass();
										mySubscription = _dbs.GetUserSubscriptions();
										SessionManagement.StoreInSession(SessionType.SubscriptionInDtls, mySubscription);

										try
										{
											LoggedIn loggedInData = new LoggedIn();
											//Get User LoggedIn Data
											List<SetParameters> sp = new List<SetParameters>()
											{
												new SetParameters{ ParameterName = "@QType", Value = "10" },
												new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
											};

											loggedInData = _db.GetData<LoggedIn>("usp_getdata", loggedInData, sp);
											if (loggedInData != null && loggedInData.ResponseText == "Success" && loggedInData.UserId > 0)
											{
												SessionManagement.StoreInSession(SessionType.LoggedInDtls, loggedInData);
											}
										}
										catch { }


										//temp session delete
										SessionManagement.DeleteFromSession(SessionType.SubscriptionTempDtls);
										SessionManagement.DeleteFromSession(SessionType.IsCouponCodeOffer);
										SessionManagement.DeleteFromSession(SessionType.CouponCode);
										SessionManagement.DeleteFromSession(SessionType.ExpertTalkId);
										//Exired Bundling Cookie here
										Response.Cookies["IsBundleUser"].Expires = DateTime.Now.AddMinutes(-1);
										Response.Cookies["IsOfferUser"].Expires = DateTime.Now.AddMinutes(-1);

										return Json(new { status = "Success", navigation = "", message = "home" }, JsonRequestBehavior.AllowGet);

									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Pay Now - Main Block");
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public async Task<ActionResult> SpecialPlanPayNow()
		{

			//if (UserId == 0)
			//{
			//	SessionManagement.StoreInSession(SessionType.SpecialRedirection, "Y");

			//	return Json(new { status = "lgn", navigation = "/my-account/login", message = "" }, JsonRequestBehavior.AllowGet);
			//	//Response.Redirect("/my-account/login");
			//}
			//else
			//{
			try
			{
				bool IsValid = false;
				bool validation = true;
				decimal payableAmount = 0;
				//decimal ExistingUserDiscountedAmount = 0;
				//decimal CouponDiscountAmount = 0;
				//decimal SubscriptionbAmount = 0;

				var buyNowContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
						.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "specialPlanRoot")?
						.FirstOrDefault()?.Children.Where(c => c.ContentType.Alias == "buyNow")?.OfType<BuyNow>()?
						.FirstOrDefault();

				if (buyNowContent != null)
				{
					dbProxy _db = new dbProxy();
					GetStatus dbresponse = new GetStatus();

					int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

					//SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
					LoggedIn loggedIn = new LoggedIn();
					string IsLoggedIn = SessionManagement.GetCurrentSession<string>(SessionType.IsLoggedIn);
					loggedIn = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);


					string Serial_UniqueId, Shoppingcartdetails = String.Empty;
					if (IsLoggedIn == "Y")
					{
						if (validation)
						{
							string TransactionId = DateTime.Now.ToString("yyMMddHHmmssff") + Guid.NewGuid().ToString().Substring(0, 5).Replace("-", "");
							string entrystatus = String.Empty;

							if (!String.IsNullOrEmpty(TransactionId))
							{
								var buyNowPlan = buyNowContent.SpecialPlanPrice;
								foreach (var subs in buyNowPlan)
								{
									if (subs.Price > 0)
									{
										payableAmount += subs.Price;
									}
									List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
										new SetParameters{ ParameterName = "@subscriptionId", Value = subs.PlanId.ToString() },
										new SetParameters{ ParameterName = "@subscriptionName", Value = subs.PlanName },
										new SetParameters{ ParameterName = "@subscriptionPrice", Value = subs.Price.ToString() },
										new SetParameters{ ParameterName = "@SubscriptionDuration", Value = subs.ValidInMonths.ToString() },
										new SetParameters{ ParameterName = "@AgeGroup", Value = subs.AgeGroup.Name },
										new SetParameters{ ParameterName = "@AutopaymentId", Value = TransactionId },
										new SetParameters{ ParameterName = "@PartCode", Value = subs.PartCode },
										new SetParameters{ ParameterName = "@MaxPrice", Value = subs.MaxPrice.ToString() },
										new SetParameters{ ParameterName = "@DiscountPrice", Value = subs.DiscountPrice.ToString() },
										new SetParameters{ ParameterName = "@UserUniqueId", Value = loggedIn.UserUniqueId == null ? "" : loggedIn.UserUniqueId}
									};

									dbresponse = _db.StoreData("USP_AddSubscriptions_SpecialPlan_Primary", sp);

									if (String.IsNullOrEmpty(dbresponse.returnStatus) || dbresponse.returnStatus == "Fail")
									{
										entrystatus = "fail";
										return Json(new { status = "Fail", navigation = "", message = dbresponse.returnMessage }, JsonRequestBehavior.AllowGet);
									}
									else
									{ entrystatus = "ok"; }
								}
							}
							else
							{
								entrystatus = "fail";
								return Json(new { status = "Fail", navigation = "", message = "Subscription data is not valid." }, JsonRequestBehavior.AllowGet);
							}

							//SubscriptionAmountCalc subscriptionAmountCalc = new SubscriptionAmountCalc();
							////SubscriptionbAmount = subscriptionAmountCalc.GetSubscriptionAmount();
							////ExistingUserDiscountedAmount = subscriptionAmountCalc.GetExistingUserDiscountAmount();
							//CouponDiscountAmount = subscriptionAmountCalc.GetCouponDiscountAmount();
							//payableAmount = subscriptionAmountCalc.GetPayableAmount();

							////Coupon Code update
							//if (CouponDiscountAmount > 0 && couponCodeResponse != null)
							//{
							//	GetStatus responce = new GetStatus();
							//	List<SetParameters> sp = new List<SetParameters>()
							//			{
							//				new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
							//				new SetParameters{ ParameterName = "@CouponCodeId", Value = couponCodeResponse.CouponCodeId.ToString() },
							//				new SetParameters{ ParameterName = "@CouponAvailedUniqueId", Value = couponCodeResponse.CouponAvailedUniqueId.ToString() },
							//				new SetParameters{ ParameterName = "@DiscountAvailed", Value = CouponDiscountAmount.ToString() },
							//				new SetParameters{ ParameterName = "@TransactionCode", Value = TransactionId }
							//			};

							//	responce = _db.StoreData("USP_AddCouponCodeAvailed", sp);
							//}


							if (entrystatus == "ok" && payableAmount > 0)
							{
								//payment gateway
								//string payNowUrl = ConfigurationManager.AppSettings["paymentUrl"].ToString();

								String response = "";
								RequestURL objRequestURL = new RequestURL();
								//string ErrorFile = ConfigurationManager.AppSettings["ErrorFile"].ToString();
								//string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
								string TPSLService = ConfigurationManager.AppSettings["TPSLService"].ToString();
								//string IsFixedPath = ConfigurationManager.AppSettings["IsFixedPath"].ToString();
								//string IsCustomLog = ConfigurationManager.AppSettings["IsCustomLog"].ToString();
								string EncryptKey = ConfigurationManager.AppSettings["EncryptKey"].ToString();
								string EncryptIV = ConfigurationManager.AppSettings["EncryptIV"].ToString();
								string RequestType = ConfigurationManager.AppSettings["RequestType"].ToString();
								string MerchantCode = ConfigurationManager.AppSettings["MerchantCode"].ToString();

								//Embed culture name in URL
								string ReturnUrl = ConfigurationManager.AppSettings["SpecialPlanReturnUrl"].ToString();
								if (!String.IsNullOrWhiteSpace(ReturnUrl))
								{
									string culture = CultureName.GetCultureName().Replace("/", "");
									if (!String.IsNullOrWhiteSpace(culture))
									{
										Uri myUri = new Uri(ReturnUrl);
										string host = myUri.Authority;
										string absolutepath = myUri.AbsolutePath;

										ReturnUrl = myUri.Scheme + "://" + host + "/" + culture + absolutepath;
									}
								}

								string SchemeCode = ConfigurationManager.AppSettings["SchemeCode"].ToString();
								string PayUserId = ConfigurationManager.AppSettings["UserId"].ToString();
								string PayPassword = ConfigurationManager.AppSettings["Password"].ToString();

								SessionManagement.StoreInSession(SessionType.PaymentId, TransactionId);
								objRequestURL.strIgnoreSSL = "Y";
								//objRequestURL.LogFilePath = Server.MapPath("LogFilePath");
								objRequestURL.TPSLService = TPSLService;
								//objRequestURL.ErrorFile = "~/DotnetIntegrationKit\\ErrorMessage.property";

								if (!String.IsNullOrEmpty(loggedIn.u_name))
									_userName = clsCommon.Decrypt(loggedIn.u_name);

								if (!String.IsNullOrEmpty(loggedIn.u_email))
									_userEmail = clsCommon.Decrypt(loggedIn.u_email);

								if (!String.IsNullOrEmpty(loggedIn.u_whatsappno))
								{
									_userMobile = clsCommon.Decrypt(loggedIn.u_whatsappno);
									if (_userMobile.Contains("+"))
										_userMobile = _userMobile.Replace("+91", "");
								}

								Serial_UniqueId = TransactionId;// "19042021192316";

								string IsPaymentModeLive = ConfigurationManager.AppSettings["IsPaymentModeLive"].ToString();
								if (IsPaymentModeLive == "N")
								{
									//For testing data
									payableAmount = 1;
									Shoppingcartdetails = SchemeCode + "_" + payableAmount + "_0.0";
								}
								else if (IsPaymentModeLive == "Y")
								{
									//For live data
									Shoppingcartdetails = SchemeCode + "_" + payableAmount + "_0.0";
								}

								objRequestURL.strUserName = PayUserId;
								objRequestURL.strPassword = PayPassword;

								//objRequestURL.strUserName = Username;
								//objRequestURL.strPassword = Password;

								if (payableAmount > 0)
								{
									if (RequestType.ToUpper() == "T" && (Serial_UniqueId != null || Serial_UniqueId != "" || Serial_UniqueId != "0"))
									{
										ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
										response = objRequestURL.SendRequest
												  (
													RequestType
												  , MerchantCode
												  , Serial_UniqueId
												  , _userName == null ? _userEmail : _userName
												  , payableAmount.ToString()//"1.00"//
												  , "INR"
												  , TransactionId
												  , ReturnUrl
												  , "NA"
												  , "NA"
												  , Shoppingcartdetails
												  , DateTime.Now.ToString("dd-MM-yyyy")
												  , _userEmail
												  , _userMobile
												  , "NA"
												  , _userName == null ? _userEmail : _userName
												  , "NA"
												  , "NA"
												  , EncryptKey
												  , EncryptIV
												  );
									}

									String strResponse = response.ToUpper();

									LogWriter Log = null;
									Log = new LogWriter(strResponse.ToString(), "");

									if (strResponse.StartsWith("ERROR"))
									{
										if (strResponse == "ERROR073")
										{
											IsValid = false;
											response = objRequestURL.SendRequest
													   (
														  RequestType
														  , MerchantCode
														  , Serial_UniqueId
														  , _userName == null ? _userEmail : _userName
														  , payableAmount.ToString()//"1.00"//
														  , "INR"
														  , TransactionId
														  , ReturnUrl
														  , "NA"
														  , "NA"
														  , Shoppingcartdetails
														  , DateTime.Now.ToString("dd-MM-yyyy")
														  , _userEmail
														  , _userMobile
														  , "NA"
														  , _userName == null ? _userEmail : _userName
														  , "NA"
														  , "NA"
														  , EncryptKey
														  , EncryptIV
												);
											strResponse = response.ToUpper();
										}
										else
										{
											Log = new LogWriter(response.ToString() + " ERROR073", "");
										}
									}
									else
									{
										IsValid = true;
									}
								}

								//Update In Database
								try
								{
									List<SetParameters> spp = new List<SetParameters>()
										{
											new SetParameters{ ParameterName = "@PaymentId", Value = TransactionId },
											new SetParameters{ ParameterName = "@subscriptionStatus", Value = "Valid" },
											new SetParameters{ ParameterName = "@subscriptionUrlResponse", Value = response }
										};

									_db.StoreData("usp_UpdatePaymentLinkStatus_SpecialPlan", spp);
								}
								catch { }

								if (IsValid)
								{
									return Json(new { status = "Success", navigation = "", message = response }, JsonRequestBehavior.AllowGet);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Pay Now - Main Block");
			}
			//}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public async Task<ActionResult> PayNowTeachersProgram(string CouponCode)
		{
			try
			{
				//validation
				List<string> allowSubscriptionAgeGroup = new List<string>();
				allowSubscriptionAgeGroup = SessionManagement.GetCurrentSession<List<string>>(SessionType.AllowuserGroup);

				List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
				UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

				//Subscription allowed or not age wise
				if (allowSubscriptionAgeGroup != null)
				{
					bool ageExists = allowSubscriptionAgeGroup.Where(x => UsertempSubscription.Any(c => c.AgeGroup == x)).Any();

					if (ageExists == false)
					{
						return Json(new { status = "Fail", navigation = "", message = "Selected class not allowed for subscription" }, JsonRequestBehavior.AllowGet);
					}
				}
				//Already redeemed or not
				List<GetUserCurrentSubscription> UserCurrentSubscription = new List<GetUserCurrentSubscription>();
				UserCurrentSubscription = SessionManagement.GetCurrentSession<List<GetUserCurrentSubscription>>(SessionType.SubscriptionInDtlsTeachers);
				if (UserCurrentSubscription != null)
				{
					bool alreadyredeemed = UserCurrentSubscription.Where(x => UserCurrentSubscription.Any(c => c.AgeGroup == x.AgeGroup)).Any();

					if (alreadyredeemed == true)
					{
						return Json(new { status = "Fail", navigation = "", message = "Selected class already redeemed by you." }, JsonRequestBehavior.AllowGet);
					}
				}

				bool IsValid = false;
				bool validation = true;
				decimal payableAmount = 0;
				//decimal ExistingUserDiscountedAmount = 0;
				decimal CouponDiscountAmount = 0;
				//decimal SubscriptionbAmount = 0;

				dbProxy _db = new dbProxy();
				GetStatus dbresponse = new GetStatus();

				//SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
				LoggedIn loggedIn = new LoggedIn();
				string IsLoggedIn = SessionManagement.GetCurrentSession<string>(SessionType.IsLoggedIn);
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
				loggedIn = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

				CouponCodeResponse couponCodeResponse = new CouponCodeResponse();
				couponCodeResponse = SessionManagement.GetCurrentSession<CouponCodeResponse>(SessionType.CouponTempDtls);

				string Serial_UniqueId, Shoppingcartdetails = String.Empty;
				if (IsLoggedIn == "Y")
				{

					try
					{
						//Coupon Code Session
						if (couponCodeResponse != null)
						{
							string response = AvailCouponCode(couponCodeResponse.CouponCodeName, "teachers", false);
							if (!String.IsNullOrEmpty(response))
							{
								string[] code = response.Split(',');
								if (code.Length > 0)
								{
									if (code[0] == "0")
									{
										return Json(new { status = "Fail", navigation = "", message = code[1].ToString() }, JsonRequestBehavior.AllowGet);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Pay Now - Coupon Code Logic");
					}

					//Validate all subscription has been mapped
					if (UsertempSubscription != null && UsertempSubscription.Any())
					{
						foreach (var subsvald in UsertempSubscription)
						{
							if ((String.IsNullOrEmpty(subsvald.Ranking.ToString()) || Convert.ToInt32(subsvald.Ranking) == 1) || String.IsNullOrEmpty(subsvald.SubscriptionId.ToString()) || String.IsNullOrEmpty(subsvald.ValidMonths.ToString()))
							{
								validation = false;
								return Json(new { status = "Fail", navigation = "", message = "Please select plan" }, JsonRequestBehavior.AllowGet);
							}
						}
						//Check Subscription Item multiple for same age group
						//var CntDuplcates = UsertempSubscription?.GroupBy(x => new { x.Ranking, x.AgeGroup }).Any(g => g.Count() > 1);
						//if (CntDuplcates == true)
						//{
						//	validation = false;
						//	return Json(new { status = "Fail", navigation = "", message = "Duplicate subscription with same age group can not apply." }, JsonRequestBehavior.AllowGet);
						//}

						//Check not be avail coupon discount amount for multiple items
						//if (!String.IsNullOrWhiteSpace(CouponCode) && couponCodeResponse.BenefitRestrict == 1)
						//{
						//	var CntCouponAmt = UsertempSubscription?.GroupBy(x => new { x.Ranking }).Any(g => g.Count() > 1);
						//	if (CntCouponAmt == true)
						//	{
						//		validation = false;
						//		return Json(new { status = "Fail", navigation = "", message = "Please select only one age group under 899 plan to avail the offer" }, JsonRequestBehavior.AllowGet);
						//	}
						//}
					}

					if (validation)
					{
						string TransactionId = DateTime.Now.ToString("yyMMddHHmmssff") + Guid.NewGuid().ToString().Substring(0, 5).Replace("-", "");
						string entrystatus = String.Empty;

						if (!String.IsNullOrEmpty(TransactionId))
						{
							if (UsertempSubscription != null && UsertempSubscription.Any())
							{
								foreach (var subs in UsertempSubscription)
								{
									if ((!String.IsNullOrEmpty(subs.Ranking.ToString()) && Convert.ToInt32(subs.Ranking) > 1))
									{
										List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
										new SetParameters{ ParameterName = "@subscriptionId", Value = subs.SubscriptionId.ToString() },
										new SetParameters{ ParameterName = "@subscriptionName", Value = subs.SubscriptionName },
										new SetParameters{ ParameterName = "@subscriptionPrice", Value = subs.SubscriptionPrice },
										new SetParameters{ ParameterName = "@SubscriptionDuration", Value = subs.ValidMonths.ToString() },
										new SetParameters{ ParameterName = "@SubscriptionValidation", Value = subs.ValidMonthsText },
										new SetParameters{ ParameterName = "@Ranking", Value = subs.Ranking.ToString() },
										new SetParameters{ ParameterName = "@AgeGroup", Value = subs.AgeGroup == null ? "" : subs.AgeGroup},
										new SetParameters{ ParameterName = "@AutopaymentId", Value = TransactionId },
										new SetParameters{ ParameterName = "@PartCode", Value = subs.PartCode == null ? "" : subs.PartCode },
										new SetParameters{ ParameterName = "@DiscountAmt", Value = subs.DiscountAmount.ToString() }
									};
										dbresponse = _db.StoreData("USP_AddSubscriptions_Primary_TeachersWorksheet", sp);
									}
									else
									{
										entrystatus = "fail";
										return Json(new { status = "Fail", navigation = "", message = "Subscription data is not valid." }, JsonRequestBehavior.AllowGet);
									}

									if (String.IsNullOrEmpty(dbresponse.returnStatus) || dbresponse.returnStatus == "Fail")
									{
										entrystatus = "fail";
										return Json(new { status = "Fail", navigation = "", message = dbresponse.returnMessage }, JsonRequestBehavior.AllowGet);
									}
									else
									{ entrystatus = "ok"; }

									//discountedAmount += subs.DiscountAmount;
									//payableAmount += Convert.ToInt32(subs.SubscriptionPrice);
								}

								//Payable amount
								//payableAmount = payableAmount - discountedAmount;

								SubscriptionAmountCalc subscriptionAmountCalc = new SubscriptionAmountCalc();
								//SubscriptionbAmount = subscriptionAmountCalc.GetSubscriptionAmount();
								//ExistingUserDiscountedAmount = subscriptionAmountCalc.GetExistingUserDiscountAmount();
								CouponDiscountAmount = subscriptionAmountCalc.GetCouponDiscountAmount();
								payableAmount = subscriptionAmountCalc.GetPayableAmount();

								//Coupon Code update
								if (CouponDiscountAmount > 0 && couponCodeResponse != null)
								{
									GetStatus responce = new GetStatus();
									List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
										new SetParameters{ ParameterName = "@CouponCodeId", Value = couponCodeResponse.CouponCodeId.ToString() },
										new SetParameters{ ParameterName = "@CouponAvailedUniqueId", Value = couponCodeResponse.CouponAvailedUniqueId.ToString() },
										new SetParameters{ ParameterName = "@DiscountAvailed", Value = CouponDiscountAmount.ToString() },
										new SetParameters{ ParameterName = "@TransactionCode", Value = TransactionId }
									};

									responce = _db.StoreData("USP_AddCouponCodeAvailed", sp);
								}


								if (entrystatus == "ok" && payableAmount > 0)
								{
									//payment gateway
									//string payNowUrl = ConfigurationManager.AppSettings["paymentUrl"].ToString();

									String response = "";
									RequestURL objRequestURL = new RequestURL();
									//string ErrorFile = ConfigurationManager.AppSettings["ErrorFile"].ToString();
									//string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
									string TPSLService = ConfigurationManager.AppSettings["TPSLService"].ToString();
									//string IsFixedPath = ConfigurationManager.AppSettings["IsFixedPath"].ToString();
									//string IsCustomLog = ConfigurationManager.AppSettings["IsCustomLog"].ToString();
									string EncryptKey = ConfigurationManager.AppSettings["EncryptKey"].ToString();
									string EncryptIV = ConfigurationManager.AppSettings["EncryptIV"].ToString();
									string RequestType = ConfigurationManager.AppSettings["RequestType"].ToString();
									string MerchantCode = ConfigurationManager.AppSettings["MerchantCode"].ToString();

									//Embed culture name in URL
									string ReturnUrl = ConfigurationManager.AppSettings["TeachersPrmReturnUrl"].ToString();
									if (!String.IsNullOrWhiteSpace(ReturnUrl))
									{
										string culture = CultureName.GetCultureName().Replace("/", "");
										if (!String.IsNullOrWhiteSpace(culture))
										{
											Uri myUri = new Uri(ReturnUrl);
											string host = myUri.Authority;
											string absolutepath = myUri.AbsolutePath;

											ReturnUrl = myUri.Scheme + "://" + host + "/" + culture + absolutepath;
										}
									}

									string SchemeCode = ConfigurationManager.AppSettings["SchemeCode"].ToString();
									string PayUserId = ConfigurationManager.AppSettings["UserId"].ToString();
									string PayPassword = ConfigurationManager.AppSettings["Password"].ToString();

									SessionManagement.StoreInSession(SessionType.PaymentId, TransactionId);
									objRequestURL.strIgnoreSSL = "Y";
									//objRequestURL.LogFilePath = Server.MapPath("LogFilePath");
									objRequestURL.TPSLService = TPSLService;
									//objRequestURL.ErrorFile = "~/DotnetIntegrationKit\\ErrorMessage.property";

									if (!String.IsNullOrEmpty(loggedIn.u_name))
										_userName = clsCommon.Decrypt(loggedIn.u_name);

									if (!String.IsNullOrEmpty(loggedIn.u_email))
										_userEmail = clsCommon.Decrypt(loggedIn.u_email);

									if (!String.IsNullOrEmpty(loggedIn.u_whatsappno))
									{
										_userMobile = clsCommon.Decrypt(loggedIn.u_whatsappno);
										if (_userMobile.Contains("+"))
											_userMobile = _userMobile.Replace("+91", "");
									}

									Serial_UniqueId = TransactionId;// "19042021192316";

									string IsPaymentModeLive = ConfigurationManager.AppSettings["IsPaymentModeLive"].ToString();
									if (IsPaymentModeLive == "N")
									{
										//For testing data
										payableAmount = 1;
										Shoppingcartdetails = SchemeCode + "_" + payableAmount + "_0.0";
									}
									else if (IsPaymentModeLive == "Y")
									{
										//For live data
										Shoppingcartdetails = SchemeCode + "_" + payableAmount + "_0.0";
									}

									objRequestURL.strUserName = PayUserId;
									objRequestURL.strPassword = PayPassword;

									//objRequestURL.strUserName = Username;
									//objRequestURL.strPassword = Password;

									if (payableAmount > 0)
									{
										if (RequestType.ToUpper() == "T" && (Serial_UniqueId != null || Serial_UniqueId != "" || Serial_UniqueId != "0"))
										{
											ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
											response = objRequestURL.SendRequest
													  (
														RequestType
													  , MerchantCode
													  , Serial_UniqueId
													  , _userName == null ? _userEmail : _userName
													  , payableAmount.ToString()//"1.00"//
													  , "INR"
													  , TransactionId
													  , ReturnUrl
													  , "NA"
													  , "NA"
													  , Shoppingcartdetails
													  , DateTime.Now.ToString("dd-MM-yyyy")
													  , _userEmail
													  , _userMobile
													  , "NA"
													  , _userName == null ? _userEmail : _userName
													  , "NA"
													  , "NA"
													  , EncryptKey
													  , EncryptIV
													  );
										}

										String strResponse = response.ToUpper();

										LogWriter Log = null;
										Log = new LogWriter(strResponse.ToString(), "");

										if (strResponse.StartsWith("ERROR"))
										{
											if (strResponse == "ERROR073")
											{
												IsValid = false;
												response = objRequestURL.SendRequest
														   (
															  RequestType
															  , MerchantCode
															  , Serial_UniqueId
															  , _userName == null ? _userEmail : _userName
															  , payableAmount.ToString()//"1.00"//
															  , "INR"
															  , TransactionId
															  , ReturnUrl
															  , "NA"
															  , "NA"
															  , Shoppingcartdetails
															  , DateTime.Now.ToString("dd-MM-yyyy")
															  , _userEmail
															  , _userMobile
															  , "NA"
															  , _userName == null ? _userEmail : _userName
															  , "NA"
															  , "NA"
															  , EncryptKey
															  , EncryptIV
													);
												strResponse = response.ToUpper();
											}
											else
											{
												Log = new LogWriter(response.ToString() + " ERROR073", "");
											}
										}
										else
										{
											IsValid = true;
										}
									}
									//Update In Database
									try
									{
										List<SetParameters> spp = new List<SetParameters>()
										{
											new SetParameters{ ParameterName = "@PaymentId", Value = TransactionId },
											new SetParameters{ ParameterName = "@subscriptionStatus", Value = "Valid" },
											new SetParameters{ ParameterName = "@subscriptionUrlResponse", Value = response }
										};

										_db.StoreData("usp_UpdatePaymentLinkStatus", spp);
									}
									catch { }

									if (IsValid)
									{
										//bool IsAppliedForFree = subscptDetails.Value<bool>("isAppliedFree");
										bool IsAppliedForFree = false;
										//Check HP Employee Coupn
										if (!String.IsNullOrEmpty(CouponCode) && IsAppliedForFree == true)
										{
											try
											{
												GetStatus couponStatus = new GetStatus();
												List<SetParameters> spCpn = new List<SetParameters>()
													{
														new SetParameters{ ParameterName = "@CouponCode", Value = CouponCode },
														new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() }
													};

												couponStatus = _db.GetData("[dbo].[USP_HPEmployeesCoupon]", couponStatus, spCpn);
												if (couponStatus != null && couponStatus.returnStatus != null)
												{
													if (couponStatus.returnStatus == "Success")
													{
														SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
														dbAccessClass dbClass = new dbAccessClass();
														PaymentStatus paymentStatus = new PaymentStatus();
														paymentStatus.PaymentId = TransactionId;
														paymentStatus.txn_status = "Direct";
														paymentStatus.txn_msg = "Direct";
														paymentStatus.txn_err_msg = "Direct";
														paymentStatus.clnt_txn_ref = "Direct";
														paymentStatus.tpsl_bank_cd = "Direct";
														paymentStatus.tpsl_txn_id = "Direct";
														paymentStatus.txn_amt = "Direct";
														paymentStatus.tpsl_txn_time = "Direct";
														paymentStatus.tpsl_rfnd_id = "Direct";
														paymentStatus.bal_amt = "Direct";
														paymentStatus.rqst_token = "Direct";
														paymentStatus.PaymentMode = "Direct";

														returnParam = PaymentAndSubscriptionInputTeachers(paymentStatus, null);
														if (!String.IsNullOrEmpty(returnParam.Amount))
															return Json(new { status = "Success", navigation = "", message = "home" }, JsonRequestBehavior.AllowGet);
														else
															return Json(new { status = "Fail", navigation = "", message = "Transaction not successfull!!" }, JsonRequestBehavior.AllowGet);
													}
													else
													{
														return Json(new { status = "Fail", navigation = "", message = couponStatus.returnMessage }, JsonRequestBehavior.AllowGet);
													}
												}
											}
											catch (Exception ex)
											{
												Logger.Error(reporting: typeof(HomeController), ex, message: "Pay Now - Coupon Block");
											}
										}
										else
										{
											return Json(new { status = "Success", navigation = "", message = response }, JsonRequestBehavior.AllowGet);
										}
									}
									else
									{
										return Json(new { status = "Fail", navigation = "", message = "Payment can not be proceed!!!" }, JsonRequestBehavior.AllowGet);
									}
								}
								else if (payableAmount == 0)//when amount is 0
								{
									PostPayUser postpay = new PostPayUser();
									dbAccessClass db;

									try
									{
										db = new dbAccessClass();
										postpay = db.PostPayUserDtls_Teachers(TransactionId);
									}
									catch (Exception ex)
									{
										Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Payment 0 Get Post pay data");
									}

									if (postpay != null)
									{
										try
										{
											PaymentStatus paymentStatus = new PaymentStatus();
											paymentStatus.txn_status = "Direct";
											paymentStatus.txn_msg = "Direct";
											paymentStatus.txn_err_msg = "Direct";
											paymentStatus.clnt_txn_ref = "Direct";
											paymentStatus.tpsl_bank_cd = "Direct";
											paymentStatus.tpsl_txn_id = "Direct";
											paymentStatus.txn_amt = "Direct";
											paymentStatus.tpsl_txn_time = "Direct";
											paymentStatus.tpsl_rfnd_id = "Direct";
											paymentStatus.bal_amt = "Direct";
											paymentStatus.rqst_token = "Direct";
											paymentStatus.PaymentMode = "Direct";
											paymentStatus.PaymentId = TransactionId;

											SubscriptionSuccessParam payData = new SubscriptionSuccessParam();
											payData = PaymentAndSubscriptionInputTeachers(paymentStatus, postpay);

											string IsEnableTrackerCode = ConfigurationManager.AppSettings["IsEnableTrackerCode"].ToString();
											if (payData.InvoiceData != null && IsEnableTrackerCode == "Y")
											{
												SessionManagement.StoreInSession(SessionType.PayResponseTracker, payData);
											}
										}
										catch (Exception ex)
										{
											Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Payment 0 only entry in Db");
										}

										//set user subscribed because 0 amount subscription can not login again
										SessionManagement.StoreInSession(SessionType.SubscribedOrNot, "Yes");
										SessionManagement.StoreInSession(SessionType.SubscribedOrNotBonus, "1");

										//Get all subscription
										List<GetUserCurrentSubscription> mySubscription = new List<GetUserCurrentSubscription>();
										dbAccessClass _dbs = new dbAccessClass();
										mySubscription = _dbs.GetUserSubscriptions();
										SessionManagement.StoreInSession(SessionType.SubscriptionInDtls, mySubscription);

										try
										{
											LoggedIn loggedInData = new LoggedIn();
											//Get User LoggedIn Data
											List<SetParameters> sp = new List<SetParameters>()
											{
												new SetParameters{ ParameterName = "@QType", Value = "10" },
												new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
											};

											loggedInData = _db.GetData<LoggedIn>("usp_getdata", loggedInData, sp);
											if (loggedInData != null && loggedInData.ResponseText == "Success" && loggedInData.UserId > 0)
											{
												SessionManagement.StoreInSession(SessionType.LoggedInDtls, loggedInData);
											}
										}
										catch { }


										//temp session delete
										SessionManagement.DeleteFromSession(SessionType.SubscriptionTempDtls);
										SessionManagement.DeleteFromSession(SessionType.IsCouponCodeOffer);
										SessionManagement.DeleteFromSession(SessionType.CouponCode);
										SessionManagement.DeleteFromSession(SessionType.ExpertTalkId);
										//Exired Bundling Cookie here
										Response.Cookies["IsBundleUser"].Expires = DateTime.Now.AddMinutes(-1);
										Response.Cookies["IsOfferUser"].Expires = DateTime.Now.AddMinutes(-1);

										return Json(new { status = "Success", navigation = "", message = "teachers" }, JsonRequestBehavior.AllowGet);

									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Pay Now - Main Block");
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public SubscriptionSuccessParam PaymentAndSubscriptionInput(PaymentStatus paymentStatus, PostPayUser postpaydata, string source = "")
		{
			SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();

			try
			{
				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@PaymentId", Value = paymentStatus.PaymentId },
						new SetParameters { ParameterName = "@txn_status", Value = paymentStatus.txn_status },
						new SetParameters { ParameterName = "@txn_msg", Value = paymentStatus.txn_msg },
						new SetParameters { ParameterName = "@txn_err_msg", Value = paymentStatus.txn_err_msg },
						new SetParameters { ParameterName = "@clnt_txn_ref", Value = paymentStatus.clnt_txn_ref },
						new SetParameters { ParameterName = "@tpsl_bank_cd", Value = paymentStatus.tpsl_bank_cd },
						new SetParameters { ParameterName = "@tpsl_txn_id", Value = paymentStatus.tpsl_txn_id },
						new SetParameters { ParameterName = "@txn_amt", Value = paymentStatus.txn_amt },
						new SetParameters { ParameterName = "@tpsl_txn_time", Value = paymentStatus.tpsl_txn_time },
						new SetParameters { ParameterName = "@tpsl_rfnd_id", Value = paymentStatus.tpsl_rfnd_id },
						new SetParameters { ParameterName = "@bal_amt", Value = paymentStatus.bal_amt },
						new SetParameters { ParameterName = "@rqst_token", Value = paymentStatus.rqst_token },
						new SetParameters { ParameterName = "@paymentMode", Value = paymentStatus.PaymentMode }
					};

				GetStatus payStatus = new GetStatus();
				payStatus = _db.StoreData("usp_InsertSubscriptionPaymentStatus", sp);

				if (payStatus?.returnStatus == "Success")
				{
					try
					{
						int UserId = postpaydata.UserId; //SessionManagement.GetCurrentSession<int>(SessionType.UserId);
						string EmailId = clsCommon.Decrypt(postpaydata.Email);
						HP_PLC_Doc.Models.InvoiceDetails invoiceDetails = new HP_PLC_Doc.Models.InvoiceDetails();
						List<HP_PLC_Doc.Models.InvoiceData> InvoiceList = new List<HP_PLC_Doc.Models.InvoiceData>();
						//MyProfileWithSubscription myprofile = new MyProfileWithSubscription();
						//myprofile = GetProfileWithPaymentId(paymentStatus.PaymentId);
						List<SetParameters> invoice = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@QType", Value = "1" },
						new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() },
						new SetParameters { ParameterName = "@TransactionId", Value = paymentStatus.PaymentId }
					};
						InvoiceList = _db.GetDataMultiple("GetInvoiceList", InvoiceList, invoice);

						if (InvoiceList != null && InvoiceList.Any())
						{
							//save data into sycn with solution start
							try
							{
								//MyProfile registration = new MyProfile();
								//dbProxy _db = new dbProxy();
								//List<SetParameters> spreg = new List<SetParameters>()
								//{
								//	new SetParameters{ ParameterName = "@QType", Value = "2" },
								//	new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
								//};

								//registration = _db.GetData("usp_getdata", registration, spreg);
								//Responce post = new Responce();
								//ApiCallServices apiCall = new ApiCallServices();
								//RegistrationPostModel postModel = new RegistrationPostModel();
								//List<Item> items = new List<Item>();
								//items.Add(new Item()
								//{
								//	UserId = UserId,
								//	u_name = registration.Name,
								//	u_email = registration.Email,
								//	u_whatsappno_prefix = registration.WhatsAppNoPrefix,
								//	u_whatsappno = registration.WhatsAppNo,
								//	age_group = registration.SelectedAgeGroup,
								//	WhatsAppConsent = registration.ComWithWhatsApp,
								//	EmailConsent = registration.ComWithEmail,
								//	PhoneConsent = registration.ComWithPhone,
								//	CheckedTnC = "Yes",
								//	EncPassword = "",//null
								//	register_date = DateTime.Now.ToString("yyyy-MM-dd"),
								//	subscription_lvl = string.Join(",", InvoiceList?.Select(x => x?.SubscriptionName).ToList()),//null
								//	TransationId = paymentStatus.PaymentId

								//});
								//;
								//postModel.Data = items;
								returnParam.InvoiceData = InvoiceList.FirstOrDefault();
								returnParam.InvoiceData.TransactionId = paymentStatus.PaymentId;

								List<Product> ProductList = new List<Product>();
								try
								{
									foreach (var item in InvoiceList)
									{
										var sub = Umbraco?.Content(item.SubscriptionId)?.DescendantsOrSelf()?.OfType<Subscriptions>()?.FirstOrDefault();
										ProductList.Add(new Product()
										{
											name = item.SubscriptionName,
											id = item.SubscriptionId,
											brand = item.AgeGroup,
											category = sub?.SubscriptionBenefits?.Where(x => x.SCoflag)?.FirstOrDefault()?.EnterTitleAny,
											coupon = item.CouponNameDiscountItemWise,
											price = item.SubscriptionPrice.ToString(),
											discountAmount = item.CouponDiscountAmtItemWise.ToString(),
											quantity = "1",
											variant = ""
										});
									}

									returnParam.ProductList = ProductList;
									returnParam.TotalAmount = InvoiceList.Sum(x => x.SubscriptionPrice);

								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Payment Tracker");
								}


								//post = apiCall.PostRegistartionData(postModel);
							}
							catch (Exception ex)
							{
								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Save RquestId in Sync Solution");

								//Logger.Error(reporting: typeof(HomeController), ex, message: "registration - Save RquestId in Sync Solution");
							}

							try
							{
								//save for invoice pdf into s3 bucket start-hemant
								if (UserId > 0)
								{
									Responce responce = new Responce();
									SubscriptionController subscrp = new SubscriptionController();
									invoiceDetails.PaymentId = paymentStatus.PaymentId;
									invoiceDetails.SubscriptionId = "";
									invoiceDetails.UserId = UserId;

									responce = SaveInvoicePDFFile(InvoiceList, paymentStatus.PaymentId, EmailId, "");
									if (responce.StatusCode == System.Net.HttpStatusCode.OK && !String.IsNullOrEmpty(responce.Result.ToString()))
									{
										string invSource = ConfigurationManager.AppSettings["InvoiceSource"].ToString();
										if (!String.IsNullOrWhiteSpace(invSource) && invSource == "bitly")
										{
											GenerateBitlyLink bitLink = new GenerateBitlyLink();
											invoiceDetails.InvoicePDFUrl = bitLink.Shorten(responce.Result.ToString());
										}
										else if (!String.IsNullOrWhiteSpace(invSource) && invSource == "cdn")
											invoiceDetails.InvoicePDFUrl = responce.Result.ToString();
										else
											invoiceDetails.InvoicePDFUrl = responce.Result.ToString();

										SaveInvoiceDetailsItoDatabase(invoiceDetails);
									}
								}

								//save for invoice pdf into s3 bucket end
							}
							catch (Exception ex)
							{
								HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								error.PageName = "Subscription Controller";
								error.MethodName = "PaymentAndSubscriptionInput - SaveInvoice to bucket and database";
								error.ErrorMessage = ex.Message;

								HPPlc.Models.dbAccessClass.PostApplicationError(error);

								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - SaveInvoice to bucket and database");
							}

							if (String.IsNullOrWhiteSpace(source))
							{
								//Send Data to SFMC
								try
								{
									if (!String.IsNullOrWhiteSpace(paymentStatus.PaymentId) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
									{
										SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
										sendDataToSFMC.PostDataSFMC(UserId, invoiceDetails.InvoicePDFUrl, "subscription");
									}
								}
								catch (Exception ex)
								{
									HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
									error.PageName = "Subscription Controller";
									error.MethodName = "PaymentAndSubscriptionInput - Send Data to SFMC";
									error.ErrorMessage = ex.Message;

									HPPlc.Models.dbAccessClass.PostApplicationError(error);

									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Send Data to SFMC");
								}


								try
								{
									//save user form excel referral with get discount amount start
									//MyProfile profile = new MyProfile();
									//profile = GetProfile();
									Subscriptions subscription;
									subscription = ExistingUserRewardData();
									if (subscription != null && subscription.IsAppliedForExistingUser)
									{
										if (InvoiceList != null && !string.IsNullOrWhiteSpace(InvoiceList.FirstOrDefault().Mode) && InvoiceList.FirstOrDefault().Equals("Excel"))
										{
											ReferralFromExcelUser user = new ReferralFromExcelUser()
											{
												UserId = UserId,
												PaymentId = paymentStatus.PaymentId,
												Mode = "Excel",
												Amount = Umbraco?.Content(subscription.SubscriptionName.Udi)?.DescendantsOrSelf()?.OfType<SubscriptionItem>()?.FirstOrDefault().Amount.ToString(),
												SubscriptionId = subscription.Id.ToString()
											};
											SaveReferralUserFromExceltoDatabase(user);
										}
									}
									//save user form excel referral with get discount amount end
								}
								catch (Exception ex)
								{
									HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
									error.PageName = "Subscription Controller";
									error.MethodName = "PaymentAndSubscriptionInput - Save Reward Data";
									error.ErrorMessage = ex.Message;

									HPPlc.Models.dbAccessClass.PostApplicationError(error);

									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Save Reward Data");
								}

								//subscriptionmailer here
								try
								{
									if (InvoiceList != null && InvoiceList.Any())
									{
										Responce response = new Responce();
										HtmlRenderHelper htmlRender = new HtmlRenderHelper();
										foreach (var ranking in InvoiceList)
										{
											bool sendmailstatus = SubscriptionEmailer(ranking.Ranking, paymentStatus.PaymentId, invoiceDetails?.InvoicePDFUrl, EmailId, ranking.AgeGroup);
										}
									}
								}
								catch (Exception ex)
								{
									HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
									error.PageName = "Subscription Controller";
									error.MethodName = "PaymentAndSubscriptionInput - Subscription Mailer";
									error.ErrorMessage = ex.Message;

									HPPlc.Models.dbAccessClass.PostApplicationError(error);

									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Subscription Mailer");
								}
								//SubscriptionMailer(InvoiceList, invoiceDetails.InvoicePDFUrl, paymentStatus.PaymentId);
							}
						}
					}
					catch (Exception ex)
					{
						HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
						error.PageName = "Subscription Controller";
						error.MethodName = "PaymentAndSubscriptionInput - GetInvoice Details";
						error.ErrorMessage = ex.Message;

						HPPlc.Models.dbAccessClass.PostApplicationError(error);

						Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - GetInvoice Details");
					}
				}
			}
			catch (Exception ex)
			{
				HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
				error.PageName = "Subscription Controller";
				error.MethodName = "PaymentAndSubscriptionInput";
				error.ErrorMessage = ex.Message;

				HPPlc.Models.dbAccessClass.PostApplicationError(error);

				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput");
			}

			return returnParam;
		}

		public SubscriptionSuccessParam PaymentAndSubscriptionInputBonus(PaymentStatus paymentStatus, PostPayUser postpaydata, string source = "")
		{
			SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();

			try
			{
				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@PaymentId", Value = paymentStatus.PaymentId },
						new SetParameters { ParameterName = "@txn_status", Value = paymentStatus.txn_status },
						new SetParameters { ParameterName = "@txn_msg", Value = paymentStatus.txn_msg },
						new SetParameters { ParameterName = "@txn_err_msg", Value = paymentStatus.txn_err_msg },
						new SetParameters { ParameterName = "@clnt_txn_ref", Value = paymentStatus.clnt_txn_ref },
						new SetParameters { ParameterName = "@tpsl_bank_cd", Value = paymentStatus.tpsl_bank_cd },
						new SetParameters { ParameterName = "@tpsl_txn_id", Value = paymentStatus.tpsl_txn_id },
						new SetParameters { ParameterName = "@txn_amt", Value = paymentStatus.txn_amt },
						new SetParameters { ParameterName = "@tpsl_txn_time", Value = paymentStatus.tpsl_txn_time },
						new SetParameters { ParameterName = "@tpsl_rfnd_id", Value = paymentStatus.tpsl_rfnd_id },
						new SetParameters { ParameterName = "@bal_amt", Value = paymentStatus.bal_amt },
						new SetParameters { ParameterName = "@rqst_token", Value = paymentStatus.rqst_token },
						new SetParameters { ParameterName = "@paymentMode", Value = paymentStatus.PaymentMode }
					};

				GetStatus payStatus = new GetStatus();
				payStatus = _db.StoreData("usp_InsertSubscriptionPaymentStatus_Bonus", sp);

				if (payStatus?.returnStatus == "Success")
				{
					try
					{
						int UserId = postpaydata.UserId; //SessionManagement.GetCurrentSession<int>(SessionType.UserId);
						string EmailId = clsCommon.Decrypt(postpaydata.Email);
						HP_PLC_Doc.Models.InvoiceDetails invoiceDetails = new HP_PLC_Doc.Models.InvoiceDetails();
						List<HP_PLC_Doc.Models.InvoiceData> InvoiceList = new List<HP_PLC_Doc.Models.InvoiceData>();
						//MyProfileWithSubscription myprofile = new MyProfileWithSubscription();
						//myprofile = GetProfileWithPaymentId(paymentStatus.PaymentId);
						List<SetParameters> invoice = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@QType", Value = "-1" },
						new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() },
						new SetParameters { ParameterName = "@TransactionId", Value = paymentStatus.PaymentId }
					};
						InvoiceList = _db.GetDataMultiple("GetInvoiceList", InvoiceList, invoice);

						if (InvoiceList != null && InvoiceList.Any())
						{
							//save data into sycn with solution start
							try
							{
								//MyProfile registration = new MyProfile();
								//dbProxy _db = new dbProxy();
								//List<SetParameters> spreg = new List<SetParameters>()
								//{
								//	new SetParameters{ ParameterName = "@QType", Value = "2" },
								//	new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
								//};

								//registration = _db.GetData("usp_getdata", registration, spreg);
								//Responce post = new Responce();
								//ApiCallServices apiCall = new ApiCallServices();
								//RegistrationPostModel postModel = new RegistrationPostModel();
								//List<Item> items = new List<Item>();
								//items.Add(new Item()
								//{
								//	UserId = UserId,
								//	u_name = registration.Name,
								//	u_email = registration.Email,
								//	u_whatsappno_prefix = registration.WhatsAppNoPrefix,
								//	u_whatsappno = registration.WhatsAppNo,
								//	age_group = registration.SelectedAgeGroup,
								//	WhatsAppConsent = registration.ComWithWhatsApp,
								//	EmailConsent = registration.ComWithEmail,
								//	PhoneConsent = registration.ComWithPhone,
								//	CheckedTnC = "Yes",
								//	EncPassword = "",//null
								//	register_date = DateTime.Now.ToString("yyyy-MM-dd"),
								//	subscription_lvl = string.Join(",", InvoiceList?.Select(x => x?.SubscriptionName).ToList()),//null
								//	TransationId = paymentStatus.PaymentId

								//});
								//;
								//postModel.Data = items;
								returnParam.InvoiceData = InvoiceList.FirstOrDefault();
								returnParam.InvoiceData.TransactionId = paymentStatus.PaymentId;

								List<Product> ProductList = new List<Product>();
								try
								{
									foreach (var item in InvoiceList)
									{
										var sub = Umbraco?.Content(item.SubscriptionId)?.DescendantsOrSelf()?.OfType<BonusAddSubscriptions>()?.FirstOrDefault();
										ProductList.Add(new Product()
										{
											name = item.SubscriptionName,
											id = item.SubscriptionId,
											brand = item.AgeGroup,
											category = sub?.SubscriptionBenefits?.Where(x => x.SCoflag)?.FirstOrDefault()?.EnterTitleAny,
											coupon = item.CouponNameDiscountItemWise,
											price = item.SubscriptionPrice.ToString(),
											discountAmount = item.CouponDiscountAmtItemWise.ToString(),
											quantity = "1",
											variant = ""
										});
									}

									returnParam.ProductList = ProductList;
									returnParam.TotalAmount = InvoiceList.Sum(x => x.SubscriptionPrice);

								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Payment Tracker Bonus");
								}


								//post = apiCall.PostRegistartionData(postModel);
							}
							catch (Exception ex)
							{
								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Save RquestId in Sync Solution Bonus");

								//Logger.Error(reporting: typeof(HomeController), ex, message: "registration - Save RquestId in Sync Solution");
							}

							try
							{
								//save for invoice pdf into s3 bucket start-hemant
								if (UserId > 0)
								{
									Responce responce = new Responce();
									SubscriptionController subscrp = new SubscriptionController();
									invoiceDetails.PaymentId = paymentStatus.PaymentId;
									invoiceDetails.SubscriptionId = "";
									invoiceDetails.UserId = UserId;

									responce = SaveInvoicePDFFile(InvoiceList, paymentStatus.PaymentId, EmailId, "");
									if (responce.StatusCode == System.Net.HttpStatusCode.OK && !String.IsNullOrEmpty(responce.Result.ToString()))
									{
										string invSource = ConfigurationManager.AppSettings["InvoiceSource"].ToString();
										if (!String.IsNullOrWhiteSpace(invSource) && invSource == "bitly")
										{
											GenerateBitlyLink bitLink = new GenerateBitlyLink();
											invoiceDetails.InvoicePDFUrl = bitLink.Shorten(responce.Result.ToString());
										}
										else if (!String.IsNullOrWhiteSpace(invSource) && invSource == "cdn")
											invoiceDetails.InvoicePDFUrl = responce.Result.ToString();
										else
											invoiceDetails.InvoicePDFUrl = responce.Result.ToString();

										SaveInvoiceDetailsItoDatabase(invoiceDetails);
									}
								}

								//save for invoice pdf into s3 bucket end
							}
							catch (Exception ex)
							{
								HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								error.PageName = "Subscription Controller";
								error.MethodName = "PaymentAndSubscriptionInputBonus - SaveInvoice to bucket and database Bonus";
								error.ErrorMessage = ex.Message;

								HPPlc.Models.dbAccessClass.PostApplicationError(error);

								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInputBonus - SaveInvoice to bucket and database Bonus");
							}

							if (String.IsNullOrWhiteSpace(source))
							{
								//Send Data to SFMC Bonus plan
								try
								{
									if (!String.IsNullOrWhiteSpace(paymentStatus.PaymentId) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
									{
										SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
										sendDataToSFMC.PostDataSFMCBonus(UserId, invoiceDetails.InvoicePDFUrl, "subscriptionbonus");
									}
								}
								catch (Exception ex)
								{
									HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
									error.PageName = "Subscription Controller";
									error.MethodName = "PaymentAndSubscriptionInputBonus - Send Data to SFMC";
									error.ErrorMessage = ex.Message;

									HPPlc.Models.dbAccessClass.PostApplicationError(error);

									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInputBonus - Send Data to SFMC");
								}


								//try
								//{
								//	//save user form excel referral with get discount amount start
								//	//MyProfile profile = new MyProfile();
								//	//profile = GetProfile();
								//	Subscriptions subscription;
								//	subscription = ExistingUserRewardData();
								//	if (subscription != null && subscription.IsAppliedForExistingUser)
								//	{
								//		if (InvoiceList != null && !string.IsNullOrWhiteSpace(InvoiceList.FirstOrDefault().Mode) && InvoiceList.FirstOrDefault().Equals("Excel"))
								//		{
								//			ReferralFromExcelUser user = new ReferralFromExcelUser()
								//			{
								//				UserId = UserId,
								//				PaymentId = paymentStatus.PaymentId,
								//				Mode = "Excel",
								//				Amount = Umbraco?.Content(subscription.SubscriptionName.Udi)?.DescendantsOrSelf()?.OfType<SubscriptionItem>()?.FirstOrDefault().Amount.ToString(),
								//				SubscriptionId = subscription.Id.ToString()
								//			};
								//			SaveReferralUserFromExceltoDatabase(user);
								//		}
								//	}
								//	//save user form excel referral with get discount amount end
								//}
								//catch (Exception ex)
								//{
								//	HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								//	error.PageName = "Subscription Controller";
								//	error.MethodName = "PaymentAndSubscriptionInput - Save Reward Data";
								//	error.ErrorMessage = ex.Message;

								//	HPPlc.Models.dbAccessClass.PostApplicationError(error);

								//	Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Save Reward Data");
								//}

								//subscriptionmailer here
								try
								{
									if (InvoiceList != null && InvoiceList.Any())
									{
										Responce response = new Responce();
										HtmlRenderHelper htmlRender = new HtmlRenderHelper();
										foreach (var ranking in InvoiceList)
										{
											bool sendmailstatus = SubscriptionEmailerBonus(ranking.Ranking, paymentStatus.PaymentId, invoiceDetails?.InvoicePDFUrl, EmailId, ranking?.AgeGroup);
										}
									}
								}
								catch (Exception ex)
								{
									HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
									error.PageName = "Subscription Controller";
									error.MethodName = "PaymentAndSubscriptionInput - Subscription Mailer Bonus";
									error.ErrorMessage = ex.Message;

									HPPlc.Models.dbAccessClass.PostApplicationError(error);

									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Subscription Mailer Bonus");
								}
								//SubscriptionMailer(InvoiceList, invoiceDetails.InvoicePDFUrl, paymentStatus.PaymentId);
							}
						}
					}
					catch (Exception ex)
					{
						HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
						error.PageName = "Subscription Controller";
						error.MethodName = "PaymentAndSubscriptionInput - GetInvoice Details Bonus";
						error.ErrorMessage = ex.Message;

						HPPlc.Models.dbAccessClass.PostApplicationError(error);

						Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - GetInvoice Details Bonus");
					}
				}
			}
			catch (Exception ex)
			{
				HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
				error.PageName = "Subscription Controller";
				error.MethodName = "PaymentAndSubscriptionInput";
				error.ErrorMessage = ex.Message;

				HPPlc.Models.dbAccessClass.PostApplicationError(error);

				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput");
			}

			return returnParam;
		}

		public SubscriptionSuccessParam PaymentAndSubscriptionInput_SpecialPlan(PaymentStatus paymentStatus, PostPayUser postpaydata)
		{
			SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();

			try
			{
				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@PaymentId", Value = paymentStatus.PaymentId },
						new SetParameters { ParameterName = "@txn_status", Value = paymentStatus.txn_status },
						new SetParameters { ParameterName = "@txn_msg", Value = paymentStatus.txn_msg },
						new SetParameters { ParameterName = "@txn_err_msg", Value = paymentStatus.txn_err_msg },
						new SetParameters { ParameterName = "@clnt_txn_ref", Value = paymentStatus.clnt_txn_ref },
						new SetParameters { ParameterName = "@tpsl_bank_cd", Value = paymentStatus.tpsl_bank_cd },
						new SetParameters { ParameterName = "@tpsl_txn_id", Value = paymentStatus.tpsl_txn_id },
						new SetParameters { ParameterName = "@txn_amt", Value = paymentStatus.txn_amt },
						new SetParameters { ParameterName = "@tpsl_txn_time", Value = paymentStatus.tpsl_txn_time },
						new SetParameters { ParameterName = "@tpsl_rfnd_id", Value = paymentStatus.tpsl_rfnd_id },
						new SetParameters { ParameterName = "@bal_amt", Value = paymentStatus.bal_amt },
						new SetParameters { ParameterName = "@rqst_token", Value = paymentStatus.rqst_token },
						new SetParameters { ParameterName = "@paymentMode", Value = paymentStatus.PaymentMode }
					};

				GetStatus payStatus = new GetStatus();
				payStatus = _db.StoreData("usp_InsertSubscriptionPaymentStatus_SpecialPlan", sp);

				if (payStatus?.returnStatus == "Success")
				{
					try
					{
						int UserId = postpaydata.UserId; //SessionManagement.GetCurrentSession<int>(SessionType.UserId);
						string EmailId = clsCommon.Decrypt(postpaydata.Email);
						HP_PLC_Doc.Models.InvoiceDetails invoiceDetails = new HP_PLC_Doc.Models.InvoiceDetails();
						List<HP_PLC_Doc.Models.InvoiceData> InvoiceList = new List<HP_PLC_Doc.Models.InvoiceData>();
						//MyProfileWithSubscription myprofile = new MyProfileWithSubscription();
						//myprofile = GetProfileWithPaymentId(paymentStatus.PaymentId);
						List<SetParameters> invoice = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@QType", Value = "0" },
						new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() },
						new SetParameters { ParameterName = "@TransactionId", Value = paymentStatus.PaymentId }
					};
						InvoiceList = _db.GetDataMultiple("GetInvoiceList", InvoiceList, invoice);

						if (InvoiceList != null && InvoiceList.Any())
						{
							//save data into sycn with solution start
							try
							{

								returnParam.InvoiceData = InvoiceList.FirstOrDefault();
								returnParam.InvoiceData.TransactionId = paymentStatus.PaymentId;

								List<Product> ProductList = new List<Product>();
								try
								{
									foreach (var item in InvoiceList)
									{
										//var sub = Umbraco?.Content(item.SubscriptionId)?.DescendantsOrSelf()?.OfType<Subscriptions>()?.FirstOrDefault();
										ProductList.Add(new Product()
										{
											name = item.SubscriptionName,
											id = item.SubscriptionId,
											brand = item.AgeGroup,
											//category = sub?.SubscriptionBenefits?.Where(x => x.SCoflag)?.FirstOrDefault()?.EnterTitleAny,
											coupon = item.CouponNameDiscountItemWise,
											price = item.SubscriptionPrice.ToString(),
											discountAmount = item.CouponDiscountAmtItemWise.ToString(),
											quantity = "1",
											variant = "",
											MaxPrice = item.MaxPrice.ToString(),
											DiscountPrice = item.DiscountPrice.ToString()
										});
									}

									returnParam.ProductList = ProductList;
									returnParam.TotalAmount = InvoiceList.Sum(x => x.SubscriptionPrice);

								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Payment Tracker");
								}


								//post = apiCall.PostRegistartionData(postModel);
							}
							catch (Exception ex)
							{
								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Save RquestId in Sync Solution");

								//Logger.Error(reporting: typeof(HomeController), ex, message: "registration - Save RquestId in Sync Solution");
							}

							try
							{
								//save for invoice pdf into s3 bucket start-hemant
								if (UserId > 0)
								{
									Responce responce = new Responce();
									SubscriptionController subscrp = new SubscriptionController();
									invoiceDetails.PaymentId = paymentStatus.PaymentId;
									invoiceDetails.SubscriptionId = "";
									invoiceDetails.UserId = UserId;

									responce = SaveInvoicePDFFile(InvoiceList, paymentStatus.PaymentId, EmailId, "sp");
									if (responce.StatusCode == System.Net.HttpStatusCode.OK && !String.IsNullOrEmpty(responce.Result.ToString()))
									{
										string invSource = ConfigurationManager.AppSettings["InvoiceSource"].ToString();
										if (!String.IsNullOrWhiteSpace(invSource) && invSource == "bitly")
										{
											GenerateBitlyLink bitLink = new GenerateBitlyLink();
											invoiceDetails.InvoicePDFUrl = bitLink.Shorten(responce.Result.ToString());
										}
										else if (!String.IsNullOrWhiteSpace(invSource) && invSource == "cdn")
											invoiceDetails.InvoicePDFUrl = responce.Result.ToString();
										else
											invoiceDetails.InvoicePDFUrl = responce.Result.ToString();

										SaveInvoiceDetailsItoDatabase(invoiceDetails);
									}
								}

								//save for invoice pdf into s3 bucket end
							}
							catch (Exception ex)
							{
								HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								error.PageName = "Subscription Controller";
								error.MethodName = "PaymentAndSubscriptionInput - SaveInvoice to bucket and database";
								error.ErrorMessage = ex.Message;

								HPPlc.Models.dbAccessClass.PostApplicationError(error);

								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - SaveInvoice to bucket and database");
							}


							//subscriptionmailer here
							try
							{
								if (InvoiceList != null && InvoiceList.Any())
								{
									Responce response = new Responce();
									HtmlRenderHelper htmlRender = new HtmlRenderHelper();

									foreach (var ranking in InvoiceList)
									{
										if (!String.IsNullOrWhiteSpace(ranking.Mode) && ranking.Mode == "SP")
										{
											bool sendmailstatus = SubscriptionEmailer_sp(ranking.Ranking, paymentStatus.PaymentId, invoiceDetails?.InvoicePDFUrl, EmailId, ranking.AgeGroup);
										}
										else
										{
											bool sendmailstatus = SubscriptionEmailer(ranking.Ranking, paymentStatus.PaymentId, invoiceDetails?.InvoicePDFUrl, EmailId, ranking.AgeGroup);
										}
									}
								}
							}
							catch (Exception ex)
							{
								HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								error.PageName = "Subscription Controller";
								error.MethodName = "PaymentAndSubscriptionInput - Subscription Mailer";
								error.ErrorMessage = ex.Message;

								HPPlc.Models.dbAccessClass.PostApplicationError(error);

								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Subscription Mailer");
							}

							//Special Plan WhatsApp here
							try
							{
								SpecialProgramGetDataForWhatsApp specialProgramGetDataForWhatsApp = new SpecialProgramGetDataForWhatsApp();
								_db = new dbProxy();
								List<SetParameters> spwh = new List<SetParameters>()
									{
										new SetParameters { ParameterName = "@QType", Value = "2" },
										new SetParameters { ParameterName = "@UserUniqueCode", Value = "" },
										new SetParameters { ParameterName = "@paymentId", Value = paymentStatus.PaymentId }
									};

								specialProgramGetDataForWhatsApp = _db.GetData<SpecialProgramGetDataForWhatsApp>("USP_GetSpecialPlan", specialProgramGetDataForWhatsApp, spwh);
								if (specialProgramGetDataForWhatsApp != null && !String.IsNullOrWhiteSpace(specialProgramGetDataForWhatsApp.u_mobileno))
								{
									NotificationController notificationController = new NotificationController();
									notificationController.SendWhatsAppForSpecialPlan(1, UserId, specialProgramGetDataForWhatsApp.UserUniqueCode, specialProgramGetDataForWhatsApp.u_mobileno);
								}
							}
							catch (Exception ex)
							{
								HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								error.PageName = "Subscription Controller";
								error.MethodName = "Special Plan - Subscription WhatsApp";
								error.ErrorMessage = ex.Message;

								HPPlc.Models.dbAccessClass.PostApplicationError(error);

								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Special Plan - Subscription WhatsApp");
							}
						}
					}
					catch (Exception ex)
					{
						HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
						error.PageName = "Subscription Controller";
						error.MethodName = "PaymentAndSubscriptionInput - GetInvoice Details";
						error.ErrorMessage = ex.Message;

						HPPlc.Models.dbAccessClass.PostApplicationError(error);

						Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - GetInvoice Details");
					}
				}
			}
			catch (Exception ex)
			{
				HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
				error.PageName = "Subscription Controller";
				error.MethodName = "PaymentAndSubscriptionInput";
				error.ErrorMessage = ex.Message;

				HPPlc.Models.dbAccessClass.PostApplicationError(error);

				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput");
			}

			return returnParam;
		}



		public SubscriptionSuccessParam PaymentAndSubscriptionInputTeachers(PaymentStatus paymentStatus, PostPayUser postpaydata, string source = "")
		{
			SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();

			try
			{
				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@PaymentId", Value = paymentStatus.PaymentId },
						new SetParameters { ParameterName = "@txn_status", Value = paymentStatus.txn_status },
						new SetParameters { ParameterName = "@txn_msg", Value = paymentStatus.txn_msg },
						new SetParameters { ParameterName = "@txn_err_msg", Value = paymentStatus.txn_err_msg },
						new SetParameters { ParameterName = "@clnt_txn_ref", Value = paymentStatus.clnt_txn_ref },
						new SetParameters { ParameterName = "@tpsl_bank_cd", Value = paymentStatus.tpsl_bank_cd },
						new SetParameters { ParameterName = "@tpsl_txn_id", Value = paymentStatus.tpsl_txn_id },
						new SetParameters { ParameterName = "@txn_amt", Value = paymentStatus.txn_amt },
						new SetParameters { ParameterName = "@tpsl_txn_time", Value = paymentStatus.tpsl_txn_time },
						new SetParameters { ParameterName = "@tpsl_rfnd_id", Value = paymentStatus.tpsl_rfnd_id },
						new SetParameters { ParameterName = "@bal_amt", Value = paymentStatus.bal_amt },
						new SetParameters { ParameterName = "@rqst_token", Value = paymentStatus.rqst_token },
						new SetParameters { ParameterName = "@paymentMode", Value = paymentStatus.PaymentMode }
					};

				GetStatus payStatus = new GetStatus();
				payStatus = _db.StoreData("usp_InsertSubscriptionPaymentStatus_Teachers", sp);

				if (payStatus?.returnStatus == "Success")
				{
					try
					{
						int UserId = postpaydata.UserId; //SessionManagement.GetCurrentSession<int>(SessionType.UserId);
						string EmailId = clsCommon.Decrypt(postpaydata.Email);
						HP_PLC_Doc.Models.InvoiceDetails invoiceDetails = new HP_PLC_Doc.Models.InvoiceDetails();
						List<HP_PLC_Doc.Models.InvoiceData> InvoiceList = new List<HP_PLC_Doc.Models.InvoiceData>();
						//MyProfileWithSubscription myprofile = new MyProfileWithSubscription();
						//myprofile = GetProfileWithPaymentId(paymentStatus.PaymentId);
						List<SetParameters> invoice = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@QType", Value = "-2" },
						new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() },
						new SetParameters { ParameterName = "@TransactionId", Value = paymentStatus.PaymentId }
					};
						InvoiceList = _db.GetDataMultiple("GetInvoiceList", InvoiceList, invoice);

						if (InvoiceList != null && InvoiceList.Any())
						{
							//save data into sycn with solution start
							try
							{
								//MyProfile registration = new MyProfile();
								//dbProxy _db = new dbProxy();
								//List<SetParameters> spreg = new List<SetParameters>()
								//{
								//	new SetParameters{ ParameterName = "@QType", Value = "2" },
								//	new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
								//};

								//registration = _db.GetData("usp_getdata", registration, spreg);
								//Responce post = new Responce();
								//ApiCallServices apiCall = new ApiCallServices();
								//RegistrationPostModel postModel = new RegistrationPostModel();
								//List<Item> items = new List<Item>();
								//items.Add(new Item()
								//{
								//	UserId = UserId,
								//	u_name = registration.Name,
								//	u_email = registration.Email,
								//	u_whatsappno_prefix = registration.WhatsAppNoPrefix,
								//	u_whatsappno = registration.WhatsAppNo,
								//	age_group = registration.SelectedAgeGroup,
								//	WhatsAppConsent = registration.ComWithWhatsApp,
								//	EmailConsent = registration.ComWithEmail,
								//	PhoneConsent = registration.ComWithPhone,
								//	CheckedTnC = "Yes",
								//	EncPassword = "",//null
								//	register_date = DateTime.Now.ToString("yyyy-MM-dd"),
								//	subscription_lvl = string.Join(",", InvoiceList?.Select(x => x?.SubscriptionName).ToList()),//null
								//	TransationId = paymentStatus.PaymentId

								//});
								//;
								//postModel.Data = items;
								returnParam.InvoiceData = InvoiceList.FirstOrDefault();
								returnParam.InvoiceData.TransactionId = paymentStatus.PaymentId;

								List<Product> ProductList = new List<Product>();
								try
								{
									foreach (var item in InvoiceList)
									{
										var sub = Umbraco?.Content(item.SubscriptionId)?.DescendantsOrSelf()?.OfType<TeachersAddSubscriptions>()?.FirstOrDefault();
										ProductList.Add(new Product()
										{
											name = item.SubscriptionName,
											id = item.SubscriptionId,
											brand = item.AgeGroup,
											category = sub?.SubscriptionBenefits?.Where(x => x.SCoflag)?.FirstOrDefault()?.EnterTitleAny,
											coupon = item.CouponNameDiscountItemWise,
											price = item.SubscriptionPrice.ToString(),
											discountAmount = item.CouponDiscountAmtItemWise.ToString(),
											quantity = "1",
											variant = ""
										});
									}

									returnParam.ProductList = ProductList;
									returnParam.TotalAmount = InvoiceList.Sum(x => x.SubscriptionPrice);

								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Payment Tracker Bonus");
								}


								//post = apiCall.PostRegistartionData(postModel);
							}
							catch (Exception ex)
							{
								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Save RquestId in Sync Solution Bonus");

								//Logger.Error(reporting: typeof(HomeController), ex, message: "registration - Save RquestId in Sync Solution");
							}

							try
							{
								//save for invoice pdf into s3 bucket start-hemant
								if (UserId > 0)
								{
									Responce responce = new Responce();
									SubscriptionController subscrp = new SubscriptionController();
									invoiceDetails.PaymentId = paymentStatus.PaymentId;
									invoiceDetails.SubscriptionId = "";
									invoiceDetails.UserId = UserId;

									responce = SaveInvoicePDFFile(InvoiceList, paymentStatus.PaymentId, EmailId, "");
									if (responce.StatusCode == System.Net.HttpStatusCode.OK && !String.IsNullOrEmpty(responce.Result.ToString()))
									{
										string invSource = ConfigurationManager.AppSettings["InvoiceSource"].ToString();
										if (!String.IsNullOrWhiteSpace(invSource) && invSource == "bitly")
										{
											GenerateBitlyLink bitLink = new GenerateBitlyLink();
											invoiceDetails.InvoicePDFUrl = bitLink.Shorten(responce.Result.ToString());
										}
										else if (!String.IsNullOrWhiteSpace(invSource) && invSource == "cdn")
											invoiceDetails.InvoicePDFUrl = responce.Result.ToString();
										else
											invoiceDetails.InvoicePDFUrl = responce.Result.ToString();

										SaveInvoiceDetailsItoDatabase(invoiceDetails);
									}
								}

								//save for invoice pdf into s3 bucket end
							}
							catch (Exception ex)
							{
								HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								error.PageName = "Subscription Controller";
								error.MethodName = "PaymentAndSubscriptionInputBonus - SaveInvoice to bucket and database Teachers";
								error.ErrorMessage = ex.Message;

								HPPlc.Models.dbAccessClass.PostApplicationError(error);

								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInputTeachers - SaveInvoice to bucket and database Teachers");
							}

							if (String.IsNullOrWhiteSpace(source))
							{
								//Send Data to SFMC Bonus plan
								//try
								//{
								//	if (!String.IsNullOrWhiteSpace(paymentStatus.PaymentId) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
								//	{
								//		SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
								//		sendDataToSFMC.PostDataSFMCBonus(UserId, invoiceDetails.InvoicePDFUrl, "subscriptionbonus");
								//	}
								//}
								//catch (Exception ex)
								//{
								//	HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								//	error.PageName = "Subscription Controller";
								//	error.MethodName = "PaymentAndSubscriptionInputBonus - Send Data to SFMC";
								//	error.ErrorMessage = ex.Message;

								//	HPPlc.Models.dbAccessClass.PostApplicationError(error);

								//	Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInputBonus - Send Data to SFMC");
								//}


								//try
								//{
								//	//save user form excel referral with get discount amount start
								//	//MyProfile profile = new MyProfile();
								//	//profile = GetProfile();
								//	Subscriptions subscription;
								//	subscription = ExistingUserRewardData();
								//	if (subscription != null && subscription.IsAppliedForExistingUser)
								//	{
								//		if (InvoiceList != null && !string.IsNullOrWhiteSpace(InvoiceList.FirstOrDefault().Mode) && InvoiceList.FirstOrDefault().Equals("Excel"))
								//		{
								//			ReferralFromExcelUser user = new ReferralFromExcelUser()
								//			{
								//				UserId = UserId,
								//				PaymentId = paymentStatus.PaymentId,
								//				Mode = "Excel",
								//				Amount = Umbraco?.Content(subscription.SubscriptionName.Udi)?.DescendantsOrSelf()?.OfType<SubscriptionItem>()?.FirstOrDefault().Amount.ToString(),
								//				SubscriptionId = subscription.Id.ToString()
								//			};
								//			SaveReferralUserFromExceltoDatabase(user);
								//		}
								//	}
								//	//save user form excel referral with get discount amount end
								//}
								//catch (Exception ex)
								//{
								//	HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
								//	error.PageName = "Subscription Controller";
								//	error.MethodName = "PaymentAndSubscriptionInput - Save Reward Data";
								//	error.ErrorMessage = ex.Message;

								//	HPPlc.Models.dbAccessClass.PostApplicationError(error);

								//	Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Save Reward Data");
								//}

								//subscriptionmailer here
								try
								{
									if (InvoiceList != null && InvoiceList.Any())
									{
										Responce response = new Responce();
										HtmlRenderHelper htmlRender = new HtmlRenderHelper();
										foreach (var ranking in InvoiceList)
										{
											bool sendmailstatus = SubscriptionEmailerTeachers(ranking.Ranking, paymentStatus.PaymentId, invoiceDetails?.InvoicePDFUrl, EmailId, ranking?.AgeGroup);
										}
									}
								}
								catch (Exception ex)
								{
									HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
									error.PageName = "Subscription Controller";
									error.MethodName = "PaymentAndSubscriptionInput - Subscription Mailer Teachers";
									error.ErrorMessage = ex.Message;

									HPPlc.Models.dbAccessClass.PostApplicationError(error);

									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - Subscription Mailer Teachers");
								}
								//SubscriptionMailer(InvoiceList, invoiceDetails.InvoicePDFUrl, paymentStatus.PaymentId);
							}
						}
					}
					catch (Exception ex)
					{
						HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
						error.PageName = "Subscription Controller";
						error.MethodName = "PaymentAndSubscriptionInput - GetInvoice Details Teachers";
						error.ErrorMessage = ex.Message;

						HPPlc.Models.dbAccessClass.PostApplicationError(error);

						Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput - GetInvoice Details Teachers");
					}
				}
			}
			catch (Exception ex)
			{
				HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
				error.PageName = "Subscription Controller";
				error.MethodName = "PaymentAndSubscriptionInput";
				error.ErrorMessage = ex.Message;

				HPPlc.Models.dbAccessClass.PostApplicationError(error);

				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "PaymentAndSubscriptionInput");
			}

			return returnParam;
		}

		//public SubscriptionSuccessParam SetResponseFromPG(string responseStatus, string[] strSplitDecryptedResponse)
		//{
		//	SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
		//	PaymentStatus paymentStatus = new PaymentStatus();
		//	string loinparamer = String.Empty;
		//	string PaymentId = String.Empty;

		//	try
		//	{
		//		if (strSplitDecryptedResponse != null && strSplitDecryptedResponse.Length > 0)
		//		{
		//			string[] strGetMerchantParamForCompare;
		//			for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
		//			{
		//				strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
		//				if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
		//				{
		//					string[] existsInformation = strGetMerchantParamForCompare[1].Split('}');
		//					if (existsInformation.Length >= 2)
		//					{
		//						for (int sub = 0; sub < existsInformation.Length; sub++)
		//						{
		//							string[] splitValue = existsInformation[sub].Split(':');
		//							if (splitValue[0].ToString().ToLower().Replace("{", "").Replace("}", "") == "custid")
		//							{
		//								paymentStatus.PaymentId = splitValue[1];

		//								break;
		//								//string[] subscribeId = existsInformation[2].Split(':');
		//								//if (subscribeId[0].Replace("{", "").Replace("}", "") == "custid")
		//								//{
		//								//	paymentStatus.SubscriptionId = subscribeId[1];
		//								//}
		//							}
		//						}
		//					}
		//				}
		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
		//					paymentStatus.txn_status = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_MSG")
		//					paymentStatus.txn_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_ERR_MSG")
		//					paymentStatus.txn_err_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
		//					paymentStatus.clnt_txn_ref = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
		//					paymentStatus.tpsl_bank_cd = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
		//					paymentStatus.tpsl_txn_id = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
		//					paymentStatus.txn_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
		//					paymentStatus.tpsl_txn_time = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_RFND_ID")
		//					paymentStatus.tpsl_rfnd_id = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "BAL_AMT")
		//					paymentStatus.bal_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

		//				else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "RQST_TOKEN")
		//					paymentStatus.rqst_token = Convert.ToString(strGetMerchantParamForCompare[1]);
		//			}

		//			try
		//			{
		//				loinparamer = PostPayLoggedIn(paymentStatus.PaymentId);
		//			}
		//			catch { }

		//			paymentStatus.PaymentMode = "";
		//			returnParam = PaymentAndSubscriptionInput(paymentStatus);
		//		}
		//	}
		//	catch { }

		//	return returnParam;
		//}

		//public string PostPayLoggedIn(string PaymentId)
		//{
		//	string isloggedIn = String.Empty;
		//	isloggedIn = "N";
		//	dbProxy _db = new dbProxy();
		//	ReturnMessage returnMessage = new ReturnMessage();
		//	PostPaymentLogin postPaymentLogin = new PostPaymentLogin();

		//	List<SetParameters> sp = new List<SetParameters>()
		//	{
		//		new SetParameters{ ParameterName = "@PaymentId", Value = PaymentId }
		//	};

		//	postPaymentLogin = _db.GetData<PostPaymentLogin>("usp_Login_PostPayment", postPaymentLogin, sp);
		//	if (postPaymentLogin != null)
		//	{
		//		HomeController login = new HomeController();
		//		returnMessage = login.LoggedInData(postPaymentLogin.UserName, postPaymentLogin.Password, null, null, "paylogin");
		//	}

		//	return isloggedIn = returnMessage.status;
		//}

		//public SubscriptionSuccessParam PaymentAndSubscriptionInput(PaymentStatus paymentStatus)
		//{
		//	SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
		//	List<SetParameters> sp = new List<SetParameters>()
		//			{
		//				new SetParameters { ParameterName = "@PaymentId", Value = paymentStatus.PaymentId },
		//				new SetParameters { ParameterName = "@txn_status", Value = paymentStatus.txn_status },
		//				new SetParameters { ParameterName = "@txn_msg", Value = paymentStatus.txn_msg },
		//				new SetParameters { ParameterName = "@txn_err_msg", Value = paymentStatus.txn_err_msg },
		//				new SetParameters { ParameterName = "@clnt_txn_ref", Value = paymentStatus.clnt_txn_ref },
		//				new SetParameters { ParameterName = "@tpsl_bank_cd", Value = paymentStatus.tpsl_bank_cd },
		//				new SetParameters { ParameterName = "@tpsl_txn_id", Value = paymentStatus.tpsl_txn_id },
		//				new SetParameters { ParameterName = "@txn_amt", Value = paymentStatus.txn_amt },
		//				new SetParameters { ParameterName = "@tpsl_txn_time", Value = paymentStatus.tpsl_txn_time },
		//				new SetParameters { ParameterName = "@tpsl_rfnd_id", Value = paymentStatus.tpsl_rfnd_id },
		//				new SetParameters { ParameterName = "@bal_amt", Value = paymentStatus.bal_amt },
		//				new SetParameters { ParameterName = "@rqst_token", Value = paymentStatus.rqst_token },
		//				new SetParameters { ParameterName = "@paymentMode", Value = paymentStatus.PaymentMode }
		//			};

		//	GetStatus payStatus = new GetStatus();
		//	payStatus = _db.StoreData("usp_InsertSubscriptionPaymentStatus", sp);

		//	if (payStatus.returnStatus == "Success")
		//	{
		//		try
		//		{
		//			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
		//			InvoiceDetails invoiceDetails = new InvoiceDetails();
		//			List<InvoiceData> InvoiceList = new List<InvoiceData>();
		//			//MyProfileWithSubscription myprofile = new MyProfileWithSubscription();
		//			//myprofile = GetProfileWithPaymentId(paymentStatus.PaymentId);
		//			List<SetParameters> invoice = new List<SetParameters>()
		//			{
		//				new SetParameters { ParameterName = "@QType", Value = "1" },
		//				new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() },
		//				new SetParameters { ParameterName = "@TransactionId", Value = paymentStatus.PaymentId }
		//			};
		//			InvoiceList = _db.GetDataMultiple("GetInvoiceList", InvoiceList, invoice);

		//			if (InvoiceList != null && InvoiceList.Any())
		//			{
		//				//GetSubscriptionDetails
		//				//HomeController home = new HomeController();
		//				//Subscriptions subscriptionDetails;
		//				//subscriptionDetails = home.GetSubscriptionDetailsWithRanking(myprofile.Ranking);

		//				//returnParam.Amount = myprofile.Amount;
		//				//returnParam.Duration = myprofile.Duration;

		//				try
		//				{
		//					//save for invoice pdf into s3 bucket start-hemant
		//					if (UserId > 0)
		//					{
		//						Responce responce = new Responce();
		//						SubscriptionController subscrp = new SubscriptionController();
		//						invoiceDetails.PaymentId = paymentStatus.PaymentId;
		//						invoiceDetails.SubscriptionId = "";
		//						invoiceDetails.UserId = UserId;

		//						responce = subscrp.SaveInvoicePDFFile(InvoiceList,this.ControllerContext);
		//						if (responce.StatusCode == System.Net.HttpStatusCode.OK)
		//						{
		//							GenerateBitlyLink bitLink = new GenerateBitlyLink();
		//							invoiceDetails.InvoicePDFUrl = bitLink.Shorten(responce.Result.ToString());
		//						}

		//						SaveInvoiceDetailsItoDatabase(invoiceDetails);
		//					}

		//					//save for invoice pdf into s3 bucket end
		//				}
		//				catch { }

		//				try
		//				{
		//					//save user form excel referral with get discount amount start
		//					//MyProfile profile = new MyProfile();
		//					//profile = GetProfile();
		//					if (InvoiceList != null && !string.IsNullOrWhiteSpace(InvoiceList.FirstOrDefault().Mode) && InvoiceList.FirstOrDefault().Equals("Excel"))
		//					{
		//						ReferralFromExcelUser user = new ReferralFromExcelUser()
		//						{
		//							UserId = UserId,
		//							PaymentId = paymentStatus.PaymentId,
		//							Mode = "Excel",
		//							Amount = Constant.ExcelUserDiscountAmount.ToString(),
		//							SubscriptionId = "",
		//						};
		//						SaveReferralUserFromExceltoDatabase(user);
		//					}
		//					//save user form excel referral with get discount amount end
		//				}
		//				catch { }

		//				SubscriptionMailer(InvoiceList, invoiceDetails.InvoicePDFUrl, paymentStatus.PaymentId);
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			//Umbraco.Core.Logging.Logger.Error(reporting: typeof(dbAccessClass), ex, message: "EprintMailSend - Main Block");
		//		}
		//	}

		//	return returnParam;
		//}
		//public string PostPayLoggedIn(string PaymentId)
		//{
		//	string isloggedIn = String.Empty;
		//	isloggedIn = "N";
		//	dbProxy _db = new dbProxy();
		//	ReturnMessage returnMessage = new ReturnMessage();
		//	PostPaymentLogin postPaymentLogin = new PostPaymentLogin();

		//	List<SetParameters> sp = new List<SetParameters>()
		//	{
		//		new SetParameters{ ParameterName = "@PaymentId", Value = PaymentId }
		//	};

		//	postPaymentLogin = _db.GetData<PostPaymentLogin>("usp_Login_PostPayment", postPaymentLogin, sp);
		//	if (postPaymentLogin != null)
		//	{
		//		HomeController login = new HomeController();
		//		returnMessage = login.LoggedInData(postPaymentLogin.UserName, postPaymentLogin.Password, null, null, "paylogin");
		//	}

		//	return isloggedIn = returnMessage.status;
		//}

		//public bool SubscriptionMailer(List<HP_PLC_Doc.Models.InvoiceData> InvoiceList, string InvoiceBitlyUrl, string TransactionId)
		//{
		//	//List<InvoiceData> mailContent = new List<InvoiceData>();
		//	string mailStatus = String.Empty;
		//	string subscriptiondetails = String.Empty;
		//	string name = String.Empty;
		//	string email = String.Empty;
		//	bool vResponse = false;

		//	try
		//	{
		//		// send mail
		//		if (InvoiceList != null && !String.IsNullOrEmpty(InvoiceList.FirstOrDefault().Email))
		//		{
		//			if (!String.IsNullOrEmpty(InvoiceList.FirstOrDefault().Email))
		//				email = clsCommon.Decrypt(InvoiceList.FirstOrDefault().Email);
		//			if (!String.IsNullOrEmpty(InvoiceList.FirstOrDefault().Name))
		//				name = clsCommon.Decrypt(InvoiceList.FirstOrDefault().Name);

		//			if (!String.IsNullOrEmpty(email))
		//			{
		//				//if (subscriptionData?.SubscriptionDetails != null)
		//				//{
		//				//	foreach (var items in subscriptionData?.SubscriptionDetails)
		//				//	{
		//				//		subscriptiondetails += "<tr><td width='10' align='left' valign='top'>&bull;</td><td align='left' valign='top'>" + items + "</td></tr>";
		//				//	}
		//				//}

		//				string body = String.Empty;
		//				SenderMailer mailsend = new SenderMailer();
		//				using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("/Views/mailer/subscription.html")))
		//				{
		//					body = reader.ReadToEnd();

		//					body = body.Replace("{name}", name);
		//					body = body.Replace("{subscriptionplan}", InvoiceList.FirstOrDefault().SubscriptionName);
		//					body = body.Replace("{SubscriptionDetails}", subscriptiondetails);
		//					body = body.Replace("{invoiceno}", InvoiceBitlyUrl);
		//					body = body.Replace("{transactionid}", TransactionId);
		//				}
		//				//mailStatus = body;

		//				vResponse = mailsend.SendPaymentEmailerContent("Welcome to HP Print Learn Center", email, body, null, null);

		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		//mailStatus = ex.Message;
		//	}

		//	return vResponse;
		//}

		public Boolean SaveInvoiceDetailsItoDatabase(HP_PLC_Doc.Models.InvoiceDetails invoice)
		{
			try
			{
				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@UserId", Value = invoice.UserId.ToString() },
						new SetParameters { ParameterName = "@InvoiceURL", Value = invoice.InvoicePDFUrl },
						new SetParameters { ParameterName = "@PaymentId", Value = invoice.PaymentId },
					};

				GetStatus Status = new GetStatus();
				Status = _db.StoreData("InsertInvoice", sp);

				if (Status != null && Status.returnStatus == "Success")
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public Boolean SaveReferralUserFromExceltoDatabase(ReferralFromExcelUser user)
		{
			try
			{
				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@UserId", Value = user.UserId.ToString() },
						new SetParameters { ParameterName = "@SubscriptionId", Value = user.SubscriptionId },
						new SetParameters { ParameterName = "@Amount", Value = user.Amount },
						new SetParameters { ParameterName = "@PaymentId", Value = user.PaymentId },
						new SetParameters { ParameterName = "@Mode", Value = user.Mode },
					};

				GetStatus Status = new GetStatus();
				Status = _db.StoreData("InsertReferralFromExcel", sp);

				if (Status != null && Status.returnStatus == "Success")
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public Subscriptions ExistingUserRewardData()
		{
			Subscriptions subscription;
			subscription = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
							.Where(x => x.ContentType.Alias == "subscriptions")?.OfType<Subscriptions>().Where(x => x.IsAppliedForExistingUser == true).FirstOrDefault();

			return subscription;
		}

		public bool SubscriptionEmailer(int ranking, string PaymentId, string invoicepdf, string email, string AgeGroup)
		{
			bool status = false;
			HtmlRenderHelper htmlRender = new HtmlRenderHelper();

			Responce responce = new Responce();
			SubscriptionMailerModel model = new SubscriptionMailerModel();
			model = GetSubscriptionWiseHtml(ranking, AgeGroup);
			model.ViewName = "/Views/mailer/subscriptionItemMailer.cshtml";
			//switch (ranking)
			//{
			//	case 1:
			//		model.ViewName = "/Views/mailer/free.cshtml";
			//		break;

			//	case 2:
			//		model.ViewName = "/Views/mailer/199.cshtml";
			//		break;

			//	case 3:
			//		model.ViewName = "/Views/mailer/599.cshtml";
			//		break;
			//	case 4:
			//		model.ViewName = "/Views/mailer/899.cshtml";
			//		break;
			//	default:
			//		break;
			//}
			model.TransactionId = PaymentId;
			model.PDFUrl = invoicepdf;

			responce = htmlRender.GetSubscriptionHtml(model);
			string body = responce.Result as string;
			SenderMailer mailsend = new SenderMailer();
			status = mailsend.SendPaymentEmailerContent(model.Subject, email, body, model.EmailCC, model.EmailBcc);

			return status;
		}

		public bool SubscriptionEmailerBonus(int ranking, string PaymentId, string invoicepdf, string email, string AgeGroup)
		{
			bool status = false;
			HtmlRenderHelper htmlRender = new HtmlRenderHelper();

			Responce responce = new Responce();
			SubscriptionMailerModel model = new SubscriptionMailerModel();
			model = GetSubscriptionWiseHtmlBonus(ranking, "");
			model.ViewName = "/Views/mailer/subscriptionItemMailer.cshtml";
			//switch (ranking)
			//{
			//	case 1:
			//		model.ViewName = "/Views/mailer/free.cshtml";
			//		break;

			//	case 2:
			//		model.ViewName = "/Views/mailer/199.cshtml";
			//		break;

			//	case 3:
			//		model.ViewName = "/Views/mailer/599.cshtml";
			//		break;
			//	case 4:
			//		model.ViewName = "/Views/mailer/899.cshtml";
			//		break;
			//	default:
			//		break;
			//}
			model.TransactionId = PaymentId;
			model.PDFUrl = invoicepdf;

			responce = htmlRender.GetSubscriptionHtml(model);
			string body = responce.Result as string;
			SenderMailer mailsend = new SenderMailer();
			status = mailsend.SendPaymentEmailerContent(model.Subject, email, body, model.EmailCC, model.EmailBcc);

			return status;
		}

		public bool SubscriptionEmailer_sp(int ranking, string PaymentId, string invoicepdf, string email, string AgeGroup)
		{
			bool status = false;
			HtmlRenderHelper htmlRender = new HtmlRenderHelper();

			Responce responce = new Responce();
			SubscriptionMailerModel model = new SubscriptionMailerModel();
			model = GetSubscriptionWiseHtml_sp(ranking, AgeGroup);
			model.ViewName = "/Views/mailer/subscriptionItemMailer.cshtml";

			model.TransactionId = PaymentId;
			model.PDFUrl = invoicepdf;

			responce = htmlRender.GetSubscriptionHtml(model);
			string body = responce.Result as string;
			SenderMailer mailsend = new SenderMailer();
			status = mailsend.SendPaymentEmailerContent(model.Subject, email, body, model.EmailCC, model.EmailBcc);

			return status;
		}

		public bool SubscriptionEmailerTeachers(int ranking, string PaymentId, string invoicepdf, string email, string AgeGroup)
		{
			bool status = false;
			HtmlRenderHelper htmlRender = new HtmlRenderHelper();

			Responce responce = new Responce();
			SubscriptionMailerModel model = new SubscriptionMailerModel();
			model = GetSubscriptionWiseHtmlTeachers(ranking, "");
			model.ViewName = "/Views/mailer/subscriptionItemMailer.cshtml";
			//switch (ranking)
			//{
			//	case 1:
			//		model.ViewName = "/Views/mailer/free.cshtml";
			//		break;

			//	case 2:
			//		model.ViewName = "/Views/mailer/199.cshtml";
			//		break;

			//	case 3:
			//		model.ViewName = "/Views/mailer/599.cshtml";
			//		break;
			//	case 4:
			//		model.ViewName = "/Views/mailer/899.cshtml";
			//		break;
			//	default:
			//		break;
			//}
			model.TransactionId = PaymentId;
			model.PDFUrl = invoicepdf;

			responce = htmlRender.GetSubscriptionHtml(model);
			string body = responce.Result as string;
			SenderMailer mailsend = new SenderMailer();
			status = mailsend.SendPaymentEmailerContent(model.Subject, email, body, model.EmailCC, model.EmailBcc);

			return status;
		}


		public SubscriptionMailerModel GetSubscriptionWiseHtml(int Ranking, string ageTitle)
		{
			SubscriptionMailerModel model = new SubscriptionMailerModel();
			try
			{
				//string culture = CultureName.GetCultureName().Replace("/", "");
				//if (String.IsNullOrWhiteSpace(culture))
				//	culture = "en-US";

				var nodes = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
					.Children?.Where(x => x.ContentType.Alias == "emailerRoot")?.FirstOrDefault()?.Children?
					.Where(x => x.ContentType.Alias == "subscriptionMailerRoot").FirstOrDefault()?
					.Children?.OfType<SubscriptionMailer>()?.Where(x => x.Ranking == Ranking.ToString()).ToList();

				if (nodes != null)
				{
					foreach (var node in nodes)
					{
						model.HeaderLogoUrl = node.HeaderLogo?.Url()?.ToString();
						model.HeaderUrl = node.HeaderLogoUrl?.ToString();
						var AgeGroup = node.AgeGroup?.ToList();
						if (AgeGroup != null && AgeGroup.Count() > 0)
						{
							if (AgeGroup.Where(x => x.Name.Equals(ageTitle)).Count() > 0)
								model.Body = node.BodyWithVideos;
							else
								model.Body = node.Body;
						}
						else
						if (model.Body == null || string.IsNullOrWhiteSpace(model.Body?.ToHtmlString()))
							model.Body = node.Body;

						model.VisitPrintCenterLogoUrl = node.VisitPrintLearnCenter?.Url()?.ToString();
						//model.VisitPrintCenterUrl = node.VisitPrintLearnCenterUrl?.ToString();
						model.VisitPrintCenterUrl = node.FreeWorksheetAttachment?.ToString();
						model.SocialShare = node.SocialShare;
						model.Footer = node.Footer;
						model.Ranking = node.Ranking;
						model.Subject = node.Subject;
						model.EmailCC = node.MailCC;
						model.EmailBcc = node.MailBcc;

						if (node.FreeWorksheetAttachment != null)
						{
							//var Week1FreeWorksheetNode = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
							//	.Where(x => x.ContentType.Alias == "worksheetRoot")?
							//	.OfType<WorksheetRoot>()?.Where(c => c.SelectVolume?.Name == node?.FreeWorksheetAttachment?.Name && c.AgeTitle.Name == ageTitle && c.IsActive).FirstOrDefault().UploadPdf;

							var worksheetRootUrl = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault();
							string documentUrl = String.Empty;
							documentUrl = GetDocumentPdfUrlForSendOnMail(ageTitle, node?.FreeWorksheetAttachment?.Name, node.Ranking);
							model.VisitPrintCenterUrl = documentUrl;
						}
						else
						{
							model.VisitPrintCenterUrl = String.Empty;
						}

					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Mailer - Main Block");
			}

			return model;

		}

		public SubscriptionMailerModel GetSubscriptionWiseHtmlBonus(int Ranking, string ageTitle)
		{
			SubscriptionMailerModel model = new SubscriptionMailerModel();
			try
			{
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				var nodes = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
					.Children?.Where(x => x.ContentType.Alias == "emailerRoot")?.FirstOrDefault()?.Children?
					.Where(x => x.ContentType.Alias == "bonusPlanMailRoot").FirstOrDefault()?
					.Children?.OfType<SubscriptionMailer>()?.Where(x => x.Ranking == Ranking.ToString()).ToList();

				if (nodes != null)
				{
					foreach (var node in nodes)
					{
						model.HeaderLogoUrl = node.HeaderLogo?.Url()?.ToString();
						model.HeaderUrl = node.HeaderLogoUrl?.ToString();

						//var AgeGroup = node.AgeGroup?.ToList();
						//if (AgeGroup != null && AgeGroup.Count() > 0)
						//{
						//	if (AgeGroup.Where(x => x.Name.Equals(ageTitle)).Count() > 0)
						//		model.Body = node.BodyWithVideos;
						//	else
						//		model.Body = node.Body;
						//}
						//else
						if (model.Body == null || string.IsNullOrWhiteSpace(model.Body?.ToHtmlString()))
							model.Body = node.Body;

						model.VisitPrintCenterLogoUrl = node.VisitPrintLearnCenter?.Url()?.ToString();
						//model.VisitPrintCenterUrl = node.VisitPrintLearnCenterUrl?.ToString();
						model.VisitPrintCenterUrl = node.FreeWorksheetAttachment?.ToString();
						model.SocialShare = node.SocialShare;
						model.Footer = node.Footer;
						model.Ranking = node.Ranking;
						model.Subject = node.Subject;
						model.EmailCC = node.MailCC;
						model.EmailBcc = node.MailBcc;

						if (node.FreeWorksheetAttachment != null)
						{
							//var Week1FreeWorksheetNode = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
							//	.Where(x => x.ContentType.Alias == "worksheetRoot")?
							//	.OfType<WorksheetRoot>()?.Where(c => c.SelectVolume?.Name == node?.FreeWorksheetAttachment?.Name && c.AgeTitle.Name == ageTitle && c.IsActive).FirstOrDefault().UploadPdf;

							var worksheetRootUrl = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault();
							string documentUrl = String.Empty;
							documentUrl = GetDocumentPdfUrlForSendOnMail(ageTitle, node?.FreeWorksheetAttachment?.Name, node.Ranking);
							model.VisitPrintCenterUrl = documentUrl;
						}
						else
						{
							model.VisitPrintCenterUrl = String.Empty;
						}

					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Mailer - Main Block");
			}

			return model;

		}
		public SubscriptionMailerModel GetSubscriptionWiseHtml_sp(int Ranking, string ageTitle)
		{
			SubscriptionMailerModel model = new SubscriptionMailerModel();
			try
			{
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				var nodes = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
					.Children?.Where(x => x.ContentType.Alias == "emailerRoot")?.FirstOrDefault()?.Children?
					.Where(x => x.ContentType.Alias == "specialPlanMailerRoot").FirstOrDefault()?
					.Children?.OfType<SpecialPlanMailer>()?.Where(x => x.Ranking == Ranking.ToString()).ToList();

				if (nodes != null)
				{
					foreach (var node in nodes)
					{
						model.HeaderLogoUrl = node.HeaderLogo?.Url()?.ToString();
						model.HeaderUrl = node.HeaderLogoUrl?.ToString();
						var AgeGroup = node.AgeGroup?.ToList();
						if (AgeGroup != null && AgeGroup.Count() > 0)
						{
							if (AgeGroup.Where(x => x.Name.Equals(ageTitle)).Count() > 0)
								model.Body = node.BodyWithVideos;
							else
								model.Body = node.Body;
						}
						else
						if (model.Body == null || string.IsNullOrWhiteSpace(model.Body?.ToHtmlString()))
							model.Body = node.Body;

						model.VisitPrintCenterLogoUrl = node.VisitPrintLearnCenter?.Url()?.ToString();
						//model.VisitPrintCenterUrl = node.VisitPrintLearnCenterUrl?.ToString();
						model.VisitPrintCenterUrl = node.FreeWorksheetAttachment?.ToString();
						model.SocialShare = node.SocialShare;
						model.Footer = node.Footer;
						model.Ranking = node.Ranking;
						model.Subject = node.Subject;
						model.EmailCC = node.MailCC;
						model.EmailBcc = node.MailBcc;

						if (node.FreeWorksheetAttachment != null)
						{
							var worksheetRootUrl = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault();
							string documentUrl = String.Empty;
							documentUrl = GetDocumentPdfUrlForSendOnMail(ageTitle, node?.FreeWorksheetAttachment?.Name, node.Ranking);
							model.VisitPrintCenterUrl = documentUrl;
						}
						else
						{
							model.VisitPrintCenterUrl = String.Empty;
						}

					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Mailer - Main Block");
			}

			return model;

		}

		public SubscriptionMailerModel GetSubscriptionWiseHtmlTeachers(int Ranking, string ageTitle)
		{
			SubscriptionMailerModel model = new SubscriptionMailerModel();
			try
			{
				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				var nodes = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
					.Children?.Where(x => x.ContentType.Alias == "emailerRoot")?.FirstOrDefault()?.Children?
					.Where(x => x.ContentType.Alias == "teachersPlanMailRoot").FirstOrDefault()?
					.Children?.OfType<SubscriptionMailer>()?.Where(x => x.Ranking == Ranking.ToString()).ToList();

				if (nodes != null)
				{
					foreach (var node in nodes)
					{
						model.HeaderLogoUrl = node.HeaderLogo?.Url()?.ToString();
						model.HeaderUrl = node.HeaderLogoUrl?.ToString();

						//var AgeGroup = node.AgeGroup?.ToList();
						//if (AgeGroup != null && AgeGroup.Count() > 0)
						//{
						//	if (AgeGroup.Where(x => x.Name.Equals(ageTitle)).Count() > 0)
						//		model.Body = node.BodyWithVideos;
						//	else
						//		model.Body = node.Body;
						//}
						//else
						if (model.Body == null || string.IsNullOrWhiteSpace(model.Body?.ToHtmlString()))
							model.Body = node.Body;

						model.VisitPrintCenterLogoUrl = node.VisitPrintLearnCenter?.Url()?.ToString();
						//model.VisitPrintCenterUrl = node.VisitPrintLearnCenterUrl?.ToString();
						model.VisitPrintCenterUrl = node.FreeWorksheetAttachment?.ToString();
						model.SocialShare = node.SocialShare;
						model.Footer = node.Footer;
						model.Ranking = node.Ranking;
						model.Subject = node.Subject;
						model.EmailCC = node.MailCC;
						model.EmailBcc = node.MailBcc;

						if (node.FreeWorksheetAttachment != null)
						{
							//var Week1FreeWorksheetNode = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
							//	.Where(x => x.ContentType.Alias == "worksheetRoot")?
							//	.OfType<WorksheetRoot>()?.Where(c => c.SelectVolume?.Name == node?.FreeWorksheetAttachment?.Name && c.AgeTitle.Name == ageTitle && c.IsActive).FirstOrDefault().UploadPdf;

							var worksheetRootUrl = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault();
							string documentUrl = String.Empty;
							documentUrl = GetDocumentPdfUrlForSendOnMail(ageTitle, node?.FreeWorksheetAttachment?.Name, node.Ranking);
							model.VisitPrintCenterUrl = documentUrl;
						}
						else
						{
							model.VisitPrintCenterUrl = String.Empty;
						}

					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Mailer - Main Block");
			}

			return model;

		}
		public string GetDocumentPdfUrlForSendOnMail(string ageTitle, string Weekname, string Ranking)
		{
			string documentUrl = String.Empty;
			string bucketUrl = ConfigurationManager.AppSettings["BucketFileSystem:BucketHostname"].ToString();

			//string culture = CultureName.GetCultureName().Replace("/", "");
			//if (String.IsNullOrWhiteSpace(culture))
			//	culture = "en-US";

			//_variationContextAccessor.VariationContext = new VariationContext("en-US");

			var worksheetRootUrl = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")
					?.FirstOrDefault()?.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()
					?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()
					?.Where(x => x.AgeGroup.Name == ageTitle && x.IsPublished())?.FirstOrDefault()
					?.Children?.Where(x => x.ContentType.Alias == "worksheetCategory")?.OfType<WorksheetCategory>()
					?.Where(c => c?.CategoryName?.Name?.ToLower() == "maths")?.FirstOrDefault()
					?.Children?.Where(x => x.ContentType.Alias == "worksheetRoot")?.OfType<WorksheetRoot>()
					?.Where(y => y.SelectWeek?.Name == Weekname && y.IsActive == true).FirstOrDefault();

			if (worksheetRootUrl != null)
			{
				documentUrl = worksheetRootUrl.UploadPdf;
				var SubscriptionsWiseDocument = worksheetRootUrl.DocumentAndSubscriptions;
				var subscriptionWiseDoc = worksheetRootUrl?.IsSubscriptionWiseDocument;

				if (SubscriptionsWiseDocument != null && subscriptionWiseDoc == true)
				{
					foreach (var documentNode in SubscriptionsWiseDocument)
					{
						if (documentNode.SelectSubscriptions != null)
						{
							string subscriptionNode = documentNode.SelectSubscriptions.Udi.ToString();
							if (!String.IsNullOrWhiteSpace(subscriptionNode))
							{
								bool SubscriptionDataExits = Umbraco?.Content(subscriptionNode)?.DescendantsOrSelf().OfType<Subscriptions>().FirstOrDefault().Ranking == Ranking;
								if (SubscriptionDataExits == true && documentNode != null && documentNode.BrowseDocument != null)
									documentUrl = documentNode.BrowseDocument.Url();
							}
						}
					}
				}
			}

			if (!String.IsNullOrWhiteSpace(documentUrl) && !documentUrl.Contains("https"))
				documentUrl = bucketUrl + documentUrl;

			return documentUrl;
		}

		//public SubscriptionMailerModel GetSubscriptionWiseHtml(int Ranking,string ageTitle)
		//{
		//	SubscriptionMailerModel model = new SubscriptionMailerModel();
		//	try
		//	{
		//		var node = Umbraco.ContentAtRoot().Where(x => x.ContentType.Alias == "home")?.FirstOrDefault().Children?.Where(x => x.ContentType.Alias == "emailerRoot")?.FirstOrDefault()?.Children?.Where(x => x.ContentType.Alias == "subscriptionMailerRoot").FirstOrDefault().Children?.OfType<SubscriptionMailer>().Where(x => x.Ranking == Ranking.ToString()).FirstOrDefault();
		//		if (node != null)
		//		{
		//			model.HeaderLogoUrl = node.HeaderLogo?.Url()?.ToString();
		//			model.HeaderUrl = node.HeaderLogoUrl?.ToString();
		//			model.Body = node.Body;
		//			model.VisitPrintCenterLogoUrl = node.VisitPrintLearnCenter?.Url()?.ToString();
		//			//model.VisitPrintCenterUrl = node.VisitPrintLearnCenterUrl?.ToString();
		//			model.VisitPrintCenterUrl = node.FreeWorksheetAttachment?.ToString();
		//			model.SocialShare = node.SocialShare;
		//			model.Footer = node.Footer;
		//			model.Ranking = node.Ranking;
		//			model.Subject = node.Subject;
		//			model.EmailCC = node.MailCC;
		//			model.EmailBcc = node.MailBcc;

		//			if (node.FreeWorksheetAttachment != null)
		//			{
		//				var Week1FreeWorksheetNode = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
		//					.Where(x => x.ContentType.Alias == "worksheetRoot")?
		//					.OfType<WorksheetRoot>()?.Where(c => c.SelectVolume?.Name == node?.FreeWorksheetAttachment?.Name && c.AgeTitle.Name == ageTitle && c.IsActive).FirstOrDefault().UploadPdf;

		//				model.VisitPrintCenterUrl = Week1FreeWorksheetNode;
		//			}
		//			else
		//			{ model.VisitPrintCenterUrl = String.Empty; }
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Mailer - Main Block");
		//	}
		//	return model;

		//}

		public string AvailCouponCode(string CouponCode, string sourceOfCoupon, bool isValid = true)
		{
			string ageGroup = String.Empty;
			string ranking = string.Empty;
			string couponOfferSource = string.Empty;
			string couponOfferSourceParam = string.Empty;

			couponOfferSource = SessionManagement.GetCurrentSession<string>(SessionType.OfferCodeCouponValidate);
			if (!String.IsNullOrEmpty(couponOfferSource) && couponOfferSource == "Yes")
				couponOfferSourceParam = "offer";

			CouponCodeResponse couponCodeResponse = new CouponCodeResponse();
			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			int UserType = SessionManagement.GetCurrentSession<int>(SessionType.UserType);

			if (!String.IsNullOrEmpty(CouponCode))
			{
				List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
				UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

				if (UsertempSubscription != null && UsertempSubscription.Any())
				{
					foreach (var items in UsertempSubscription)
					{
						ageGroup += items.AgeGroup + ",";
						ranking += items.Ranking + ",";
					}

					ageGroup = ageGroup.TrimEnd(',');
					ranking = ranking.TrimEnd(',');
				}

				try
				{
					if (!String.IsNullOrEmpty(CouponCode) && UserId > 0 && UserType > 0)
					{
						string domainname = String.Empty;
						string email = SessionManagement.GetCurrentSession<string>(SessionType.emailid);
						try
						{
							if (!String.IsNullOrEmpty(email))
							{
								if (!String.IsNullOrWhiteSpace(email))
									email = clsCommon.Decrypt(email);

								if (email.Contains("@"))
									domainname = email.ToString().Split('@')[1];
							}
						}
						catch { }

						dbProxy _db = new dbProxy();
						List<SetParameters> sp = new List<SetParameters>()
							{
								new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
								new SetParameters{ ParameterName = "@UserType", Value = UserType.ToString() },
								new SetParameters{ ParameterName = "@Ranking", Value = ranking },
								new SetParameters{ ParameterName = "@AgeGroup", Value = ageGroup },
								new SetParameters{ ParameterName = "@CouponCode", Value = CouponCode },
								new SetParameters{ ParameterName = "@DomainName", Value = domainname },
								new SetParameters{ ParameterName = "@SessionId", Value = Session.SessionID.ToString() },
								new SetParameters{ ParameterName = "@sourceOfCoupon", Value = sourceOfCoupon == null ? "" : sourceOfCoupon },
								new SetParameters{ ParameterName = "@CouponRedSource", Value = couponOfferSourceParam == null ? "" : couponOfferSourceParam }
							};

						couponCodeResponse = _db.GetData<CouponCodeResponse>("USP_CouponCodeAvailLogic", couponCodeResponse, sp);
						if (couponCodeResponse != null)
						{
							SessionManagement.StoreInSession(SessionType.CouponTempDtls, couponCodeResponse);
							if (couponCodeResponse.ResponseCode == 1)
								isValid = true;
							else
								isValid = false;

							if (isValid == false)
							{
								//return couponCodeResponse.ResponseCode.ToString() + "," + couponCodeResponse.ResponseMessage.ToString();
								return couponCodeResponse.ResponseMessage.ToString();
							}
						}
						//else if (couponCodeResponse.IsAppliedForSubscription == 1)//Applied Subscription
						//{
						//}
						//else if (couponCodeResponse.IsCouponAppliedForAgeGroup == 1)//Applied Agegroup
						//{
						//}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(SubscriptionController), ex, message: "AvailCouponCode - Main Block");
				}


			}

			string couponResponse = "valid";
			if (couponCodeResponse.ResponseCode == 0)
				couponResponse = "notvalid";
			else if (couponCodeResponse.ResponseCode == 1)
				couponResponse = "valid";

			return couponResponse;

			//return PartialView("/Views/Partials/_SubscriptionPay.cshtml");
		}


		public ActionResult SubscriptionCancelCouponPayLoad()
		{
			dbProxy _db = new dbProxy();
			GetStatus insertStatus = new GetStatus();
			CouponCodeResponse couponCodeResponse = new CouponCodeResponse();
			couponCodeResponse = SessionManagement.GetCurrentSession<CouponCodeResponse>(SessionType.CouponTempDtls);

			if (couponCodeResponse != null && couponCodeResponse.CouponCodeId > 0)
			{

				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters { ParameterName = "@CoupnId", Value = couponCodeResponse.CouponCodeId.ToString() },
					new SetParameters { ParameterName = "@CouponLockedUniqueId", Value = couponCodeResponse.CouponAvailedUniqueId.ToString() }
				};

				insertStatus = _db.StoreData("USP_CouponCodeRelease", sp);
				if (insertStatus.returnStatus == "Success")
				{
					SessionManagement.DeleteFromSession(SessionType.CouponTempDtls);
				}
			}

			return PartialView("/Views/Partials/_SubscriptionPay.cshtml");
		}


		public ActionResult BonusSubscriptionCancelCouponPayLoad()
		{
			dbProxy _db = new dbProxy();
			GetStatus insertStatus = new GetStatus();
			CouponCodeResponse couponCodeResponse = new CouponCodeResponse();
			couponCodeResponse = SessionManagement.GetCurrentSession<CouponCodeResponse>(SessionType.CouponTempDtls);

			if (couponCodeResponse != null && couponCodeResponse.CouponCodeId > 0)
			{

				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters { ParameterName = "@CoupnId", Value = couponCodeResponse.CouponCodeId.ToString() },
					new SetParameters { ParameterName = "@CouponLockedUniqueId", Value = couponCodeResponse.CouponAvailedUniqueId.ToString() }
				};

				insertStatus = _db.StoreData("USP_CouponCodeRelease", sp);
				if (insertStatus.returnStatus == "Success")
				{
					SessionManagement.DeleteFromSession(SessionType.CouponTempDtls);
				}
			}

			return PartialView("/Views/Partials/_SubscriptionPayStructurePrm.cshtml");
		}

		public ActionResult TeachersSubscriptionCancelCouponPayLoad()
		{
			dbProxy _db = new dbProxy();
			GetStatus insertStatus = new GetStatus();
			CouponCodeResponse couponCodeResponse = new CouponCodeResponse();
			couponCodeResponse = SessionManagement.GetCurrentSession<CouponCodeResponse>(SessionType.CouponTempDtls);

			if (couponCodeResponse != null && couponCodeResponse.CouponCodeId > 0)
			{

				string culture = CultureName.GetCultureName().Replace("/", "");
				if (String.IsNullOrWhiteSpace(culture))
					culture = "en-US";

				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters { ParameterName = "@CoupnId", Value = couponCodeResponse.CouponCodeId.ToString() },
					new SetParameters { ParameterName = "@CouponLockedUniqueId", Value = couponCodeResponse.CouponAvailedUniqueId.ToString() }
				};

				insertStatus = _db.StoreData("USP_CouponCodeRelease", sp);
				if (insertStatus.returnStatus == "Success")
				{
					SessionManagement.DeleteFromSession(SessionType.CouponTempDtls);
				}
			}

			return PartialView("/Views/Partials/_SubscriptionPayTeachersPrm.cshtml");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> UnSubscribe(UnSubscribe Input)
		{
			GetStatus response = new GetStatus();
			try
			{
				if (Input != null && !String.IsNullOrWhiteSpace(Input.CurrentUrl) && !String.IsNullOrWhiteSpace(Input.UnsubscribeOpt))
				{
					if (Input.UnsubscribeOpt.Contains("Others (please specify)") && String.IsNullOrWhiteSpace(Input.OtherContent))
					{
						response.returnStatus = "vald";
						response.returnValue = 0;
						response.returnMessage = "In case of Others please enter value.";

						return Json(response, JsonRequestBehavior.AllowGet);
					}

					Uri myUri = new Uri(Input.CurrentUrl);
					string param = HttpUtility.ParseQueryString(myUri.Query).Get("u");
					string UserId = clsCommon.DecryptWithBase64Code(param);

					if (!String.IsNullOrWhiteSpace(UserId))
					{
						List<SetParameters> sp = new List<SetParameters>()
							{
								new SetParameters{ ParameterName = "@UserId", Value = UserId },
								new SetParameters{ ParameterName = "@UnsubscribeOpt", Value = Input.UnsubscribeOpt },
								new SetParameters{ ParameterName = "@OtherContent", Value = Input.OtherContent == null ? "" : Input.OtherContent }
							};

						response = await _db.StoreDataAsync("usp_UnSubscription", sp);

						if (response != null && response.returnValue > 0)
							try
							{
								Responce post = new Responce();
								ApiCallServices apiCall = new ApiCallServices("unsubscribe");
								UnSubscribePostModel postModel = new UnSubscribePostModel();
								UnSubscribeItem items = new UnSubscribeItem();

								items.UserId = int.Parse(UserId);
								items.UnSubscribeOption = Input.UnsubscribeOpt;
								items.OtherOption = Input.OtherContent;
								items.IsActive = 1;
								items.DateOfUnSubscribe = DateTime.Now.AddMinutes(330).ToString("MM-dd-yyyy");
								items.TransactionType = "UnSubscribe";

								postModel.Data = items;
								post = apiCall.UnsubscribeUser(postModel);

								//DB Log
								string status = dbAccessClass.SFMCResponse("", items.UserId.ToString(), post);
							}
							catch (Exception ex)
							{
								Logger.Error(reporting: typeof(SubscriptionController), ex, message: "Unsubscribe - Save Unsubscribe data");
							}
					}
					else
					{
						response.returnStatus = "vald";
						response.returnValue = 0;
						response.returnMessage = "Input is not in correct format.";
					}
				}
				else
				{
					response.returnStatus = "vald";
					response.returnValue = 0;
					response.returnMessage = "Please select reason to unsubscription.";
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(SubscriptionController), ex, message: "UnSubscription - Main Block");
			}

			return Json(response, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public PartialViewResult SubscriptionPageBannerChange(string tabText)
		{
			ViewData["PlanType"] = tabText;
			return PartialView("/Views/Partials/_BannerSubscptn.cshtml");
		}
	}
}