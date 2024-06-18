using RestSharp;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Microsoft.IdentityModel.Tokens;
using HPPlc.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Umbraco.Web.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
using Umbraco.Core.Models.PublishedContent;
using System.Net;
using HPPlc.Models.Mailer;
using HPPlc.Model;
using HPPlc.Models.HtmlRenderHelper;
using HPPlc.Models.HttpClientServices;
using Microsoft.Security.Application;
using HPPlc.Models.Masking;
using HPPlc.Models.HPUId;
using System.Web.Script.Serialization;
using System.Globalization;
using HPPlc.Models.Enums;
using System.Threading.Tasks;
using HPPlc.Models.WhatsApp;
using static HPPlc.Models.WhatsApp.NotificationData;
using System.IO;

namespace HPPlc.Controllers
{
	public class HomeController : SurfaceController
	{
		private readonly IVariationContextAccessor _variationContextAccessor;
		string IsEnableTrackerCode = ConfigurationManager.AppSettings["IsEnableTrackerCode"].ToString();
		string IsEnableSFMCCode = ConfigurationManager.AppSettings["IsEnableSFMCCode"].ToString();
		string IsEnableEmail = ConfigurationManager.AppSettings["IsEnableEmail"].ToString();
		string IsEnableWhatsApp = ConfigurationManager.AppSettings["IsEnableWhatsApp"].ToString();
		public HomeController(IVariationContextAccessor variationContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
		}

		public HomeController()
		{
		}

		public ActionResult LanguageStoreInSession(string langName, string cultureName)
		{
			SessionManagement.StoreInSession(SessionType.SelectedLanguage, langName);
			SessionManagement.StoreInSession(SessionType.SelectedLanguageCulture, cultureName);

			return Json("ok", JsonRequestBehavior.AllowGet);
		}

		public ActionResult AgeGroupSelection(string ageGroup)
		{
			SessionManagement.StoreInSession(SessionType.UserAgeGroupSelected, ageGroup);
			return Json("ok", JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> PreLoginCheck(string UserName, string Source = "")
		{
			try
			{
				string hostEnv = ConfigurationManager.AppSettings["WebHostEnvironment"].ToString();
				string culture = String.Empty;
				culture = CultureName.GetCultureName();

				Validate validate = new Validate();
				string tobeNavigate = String.Empty;
				string myusername = String.Empty;
				string otpVerifiedMode = String.Empty;

				dbProxy _db = new dbProxy();
				LoggedIn loggedIn = new LoggedIn();

				string mobMasking = string.Empty;
				string mailMasking = string.Empty;

				if (!String.IsNullOrWhiteSpace(UserName) && validate.ValidateMobile(UserName) == true)
					mobMasking = MobMailMasking.hidePhoneNum(UserName);
				if (!String.IsNullOrWhiteSpace(UserName) && validate.ValidateEmail(UserName) == true)
					mailMasking = MobMailMasking.hideEmailId(UserName.ToLower());

				if (!String.IsNullOrWhiteSpace(UserName))
				{
					if (validate.ValidateEmail(UserName) == true)
						UserName = UserName.ToLower().Trim();

					myusername = clsCommon.Encrypt(UserName);
				}

				UserName = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(UserName, true);

				if (!String.IsNullOrWhiteSpace(UserName) && (validate.ValidateEmail(UserName) == true || validate.ValidateMobile(UserName) == true))
				{
					List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters{ ParameterName = "@username", Value = myusername },
				};

					loggedIn = _db.GetData<LoggedIn>("usp_AttemptLogin", loggedIn, sp);

					if (loggedIn != null)
					{
						SessionManagement.StoreInSession(SessionType.HpIdSession, UserName.ToLower());

						string email = String.Empty;
						string mobile = String.Empty;

						if (validate.ValidateEmail(UserName) == true)
						{
							email = UserName;
							otpVerifiedMode = "email";
						}

						if (validate.ValidateMobile(UserName) == true)
						{
							mobile = UserName;
							otpVerifiedMode = "mobile";
						}

						JourneyOfAccount journeyOfAccount = new JourneyOfAccount();

						//User not registered - go to registration page
						int Step = loggedIn.StepsCompletted;
						journeyOfAccount.Status = "Success";
						journeyOfAccount.ProfileStatus = loggedIn.ProfileStatus;
						journeyOfAccount.RegSource = loggedIn.RegSource;
						journeyOfAccount.LoginType = loggedIn.LoginType;
						journeyOfAccount.UserName = "";
						journeyOfAccount.UserEmail = email;
						journeyOfAccount.UserMobile = mobile;
						journeyOfAccount.UserId = 0;
						journeyOfAccount.mobmasking = mobMasking;
						journeyOfAccount.mailmasking = mailMasking;

						//Special Plan Login
						if (!String.IsNullOrWhiteSpace(Source) && Source == "Plan365l")
						{
							//verified mode of otp verification
							journeyOfAccount.UserName = otpVerifiedMode;

							journeyOfAccount.mobmasking = mobMasking;
							journeyOfAccount.mailmasking = mailMasking;

							journeyOfAccount.Navigation = "";
							journeyOfAccount.UserRegisteredMode = "otp";
							journeyOfAccount.Page = "Plan365Login";

							if (journeyOfAccount.LoginType != "new")
								journeyOfAccount.LoginType = "registered";

							SendOTPMessage sendOTPMessage = new SendOTPMessage();
							//Send otp
							if (!String.IsNullOrWhiteSpace(hostEnv) && hostEnv.ToLower() == "staging")
							{
								sendOTPMessage.ValidateCode = "Success";
								sendOTPMessage.ValidateMessage = "";
							}
							else
							{
								sendOTPMessage = await SendOtp_LoginRegistration(UserName, journeyOfAccount.Page, "send");
							}
							//string response = await OtpManagementData(UserName, "login", "send");
							if (sendOTPMessage != null && sendOTPMessage.ValidateCode == "Success")
							{
								journeyOfAccount.mobmasking = mobMasking;
								journeyOfAccount.mailmasking = mailMasking;
							}
							else
							{
								journeyOfAccount.Status = sendOTPMessage.ValidateCode;
							}
						}
						else if (!String.IsNullOrWhiteSpace(Source) && Source == "Bonus")
						{
							//verified mode of otp verification
							journeyOfAccount.UserName = otpVerifiedMode;

							journeyOfAccount.mobmasking = mobMasking;
							journeyOfAccount.mailmasking = mailMasking;

							journeyOfAccount.Navigation = "";
							journeyOfAccount.UserRegisteredMode = "otp";
							journeyOfAccount.Page = Source;

							if (journeyOfAccount.LoginType != "new")
								journeyOfAccount.LoginType = "registered";

							//Send otp
							SendOTPMessage sendOTPMessage = new SendOTPMessage();
							//sendOTPMessage = await SendOtp_LoginRegistration(UserName, journeyOfAccount.Page, "send");

							if (!String.IsNullOrWhiteSpace(hostEnv) && hostEnv.ToLower() == "staging")
							{
								sendOTPMessage.ValidateCode = "Success";
								sendOTPMessage.ValidateMessage = "";
							}
							else
							{
								sendOTPMessage = await SendOtp_LoginRegistration(UserName, journeyOfAccount.Page, "send");
							}


							if (sendOTPMessage != null && sendOTPMessage.ValidateCode == "Success")
							{
								journeyOfAccount.mobmasking = mobMasking;
								journeyOfAccount.mailmasking = mailMasking;
							}
							else
							{
								journeyOfAccount.Status = sendOTPMessage.ValidateCode;
							}
						}
						else // Main login
						{
							//New User
							if (!String.IsNullOrWhiteSpace(loggedIn.LoginType) && loggedIn.LoginType == "new")
							{
								journeyOfAccount.Navigation = "";
								journeyOfAccount.UserRegisteredMode = "otp";
								journeyOfAccount.Page = "registration";
								journeyOfAccount.UserName = otpVerifiedMode;

								//Send otp
								SendOTPMessage sendOTPMessage = new SendOTPMessage();
								//sendOTPMessage = await SendOtp_LoginRegistration(UserName, "registration", "send");

								if (!String.IsNullOrWhiteSpace(hostEnv) && hostEnv.ToLower() == "staging")
								{
									sendOTPMessage.ValidateCode = "Success";
									sendOTPMessage.ValidateMessage = "";
								}
								else
								{
									sendOTPMessage = await SendOtp_LoginRegistration(UserName, "registration", "send");
								}

								if (sendOTPMessage != null && sendOTPMessage.ValidateCode == "Success")
								{
									journeyOfAccount.mobmasking = mobMasking;
									journeyOfAccount.mailmasking = mailMasking;
								}
								else
								{
									journeyOfAccount.Status = sendOTPMessage.ValidateCode;
									journeyOfAccount.ValidateMessage = sendOTPMessage.ValidateMessage;
								}
							}
							//User Registered but password not set yet
							//else if (!String.IsNullOrWhiteSpace(loggedIn.LoginType) && (loggedIn.LoginType == "hpidntpwd" || loggedIn.LoginType == "pwdntset"))
							//{
							//	journeyOfAccount.Navigation = "";
							//	journeyOfAccount.UserRegisteredMode = "auth";
							//	journeyOfAccount.Page = "auth";
							//}
							//User Registered and login with otp
							else if (!String.IsNullOrWhiteSpace(loggedIn.LoginType) && loggedIn.LoginType == "otp")
							{
								journeyOfAccount.Navigation = "";
								journeyOfAccount.UserRegisteredMode = "otp";
								journeyOfAccount.Page = "login";
								journeyOfAccount.UserName = otpVerifiedMode;

								//Send otp
								SendOTPMessage sendOTPMessage = new SendOTPMessage();
								//sendOTPMessage = await SendOtp_LoginRegistration(UserName, "login", "send");
								//string response = await OtpManagementData(UserName, "login", "send");

								if (!String.IsNullOrWhiteSpace(hostEnv) && hostEnv.ToLower() == "staging")
								{
									sendOTPMessage.ValidateCode = "Success";
									sendOTPMessage.ValidateMessage = "";
								}
								else
								{
									sendOTPMessage = await SendOtp_LoginRegistration(UserName, "login", "send");
								}

								if (sendOTPMessage != null && sendOTPMessage.ValidateCode == "Success")
								{
									journeyOfAccount.mobmasking = mobMasking;
									journeyOfAccount.mailmasking = mailMasking;
								}
								else
								{
									journeyOfAccount.Status = sendOTPMessage.ValidateCode;
								}
							}
							//User Registered and password already set
							//else if (!String.IsNullOrWhiteSpace(loggedIn.LoginType) && (loggedIn.LoginType == "hpidpwd" || loggedIn.LoginType == "pwdset"))
							//{
							//	journeyOfAccount.Navigation = "";
							//	journeyOfAccount.UserRegisteredMode = "pwd";
							//}
						}

						SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

						return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);

						//if (!String.IsNullOrWhiteSpace(loggedIn.IsUserRegistered.ToString()) && loggedIn.IsUserRegistered == "N")
						//{
						//	//User not registered - go to registration page
						//	int Step = loggedIn.StepsCompletted;
						//	journeyOfAccount.Status = "Success";
						//	journeyOfAccount.Navigation = culture + "/my-account/registration";
						//	//if (Step == 2)//Next level is Otp
						//	//{
						//	//	//string response = OtpManagementData(UserName, "registration", "send");
						//	//	journeyOfAccount.Navigation = culture + "/my-account/user-authentication/";
						//	//}
						//	//else
						//	//	journeyOfAccount.Navigation = culture + "/my-account/registration";

						//	journeyOfAccount.UserRegistered = ((int)IsUserRegistered.No).ToString();
						//	journeyOfAccount.StatusOfJrny = ((int)IsPasswordSet.PasswordNotAssigned).ToString();
						//	journeyOfAccount.StepsCompletted = loggedIn.StepsCompletted;
						//	journeyOfAccount.ProfileStatus = loggedIn.ProfileStatus;

						//	return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						//}
						//else if (!String.IsNullOrWhiteSpace(loggedIn.IsUserRegistered.ToString()) && loggedIn.IsUserRegistered == "Y")
						//{
						//	if (!String.IsNullOrWhiteSpace(loggedIn.IsPasswordSetPostHPIdRemoval.ToString()))
						//	{
						//		if (loggedIn.IsPasswordSetPostHPIdRemoval == "Y")
						//		{
						//			//User password already set
						//			journeyOfAccount.Status = "Success";
						//			journeyOfAccount.Navigation = "";
						//			journeyOfAccount.UserRegistered = ((int)IsUserRegistered.Yes).ToString();
						//			journeyOfAccount.StatusOfJrny = ((int)IsPasswordSet.PasswordAssigned).ToString();//User Password 
						//			journeyOfAccount.StepsCompletted = loggedIn.StepsCompletted;
						//			journeyOfAccount.ProfileStatus = loggedIn.ProfileStatus;

						//			return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						//		}

						//		else if (loggedIn.IsPasswordSetPostHPIdRemoval == "N")
						//		{
						//			//User password not set
						//			int Step = loggedIn.StepsCompletted;
						//			journeyOfAccount.Status = "Success";

						//			//string response = OtpManagementData(UserName, "login", "send");
						//			journeyOfAccount.Navigation = culture + "/my-account/user-authentication/";

						//			//if (Step == 1 || Step == 2)//Next level is Otp
						//			//{
						//			//	string response = OtpManagementData(UserName, "registration", "send");
						//			//	journeyOfAccount.Navigation = culture + "/my-account/user-authentication/";
						//			//}
						//			//else
						//			//	journeyOfAccount.Navigation = "";

						//			journeyOfAccount.UserRegistered = ((int)IsUserRegistered.Yes).ToString();
						//			journeyOfAccount.StatusOfJrny = ((int)IsPasswordSet.PasswordNotAssigned).ToString();//User Password 
						//			journeyOfAccount.StepsCompletted = loggedIn.StepsCompletted;
						//			journeyOfAccount.ProfileStatus = loggedIn.ProfileStatus;

						//			if (!String.IsNullOrWhiteSpace(UserName))
						//			{
						//				string email = String.Empty;
						//				string mobile = String.Empty;

						//				if (validate.ValidateEmail(UserName) == true)
						//				{
						//					email = UserName;
						//				}
						//				if (validate.ValidateMobile(UserName) == true)
						//				{
						//					mobile = UserName;
						//				}

						//				TempUserJourneyDtls UserHpIdDetails = new TempUserJourneyDtls();
						//				UserHpIdDetails.UserEmail = email;
						//				UserHpIdDetails.UserName = "";
						//				UserHpIdDetails.UserId = 0;
						//				UserHpIdDetails.UserMobile = mobile;
						//				UserHpIdDetails.Source = "login";

						//				SessionManagement.StoreInSession(SessionType.TempUserDetails, UserHpIdDetails);
						//			}

						//			return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						//		}
						//	}
						//}

						//Culture Cookie
						//CultureManagePostHPID.SetCultureCookies();

						//HPIDAPIEnvironment objhpid = new HPIDAPIEnvironment();
						//tobeNavigate = objhpid.GetLoginRedirectURL(EmailId);
						//HttpContext.Response.Redirect(tobeNavigate,true);
						//if (loggedIn.ResponseText == "existing")
						//{
						//	if (!String.IsNullOrWhiteSpace(tobeNavigate))
						//		return Json(new { status = "Existing", navigation = tobeNavigate }, JsonRequestBehavior.AllowGet);
						//}
						//else if (loggedIn.ResponseText == "new")
						//{
						//	if (!String.IsNullOrWhiteSpace(tobeNavigate))
						//		return Json(new { status = "Success", navigation = tobeNavigate }, JsonRequestBehavior.AllowGet);
						//}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "Pre Login - Main Block");
			}

			return Json(new { status = "Fail", navigation = "" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public async Task<ActionResult> AddAlternateNumberSpcialPlan(string alternatemobileno, string Source = "")
		{
			try
			{
				string culture = String.Empty;
				culture = CultureName.GetCultureName();

				Validate validate = new Validate();
				string tobeNavigate = String.Empty;
				string encalternatemobileno = String.Empty;
				string otpVerifiedMode = String.Empty;

				dbProxy _db = new dbProxy();
				string mobMasking = string.Empty;

				if (!String.IsNullOrWhiteSpace(alternatemobileno) && validate.ValidateMobile(alternatemobileno) == true)
					mobMasking = MobMailMasking.hidePhoneNum(alternatemobileno);

				JourneyOfAccount journeyOfAccount = new JourneyOfAccount();
				journeyOfAccount = SessionManagement.GetCurrentSession<JourneyOfAccount>(SessionType.JrnyUserDetails);

				journeyOfAccount.Page = "Plan365AddAltntMob";

				//Send otp
				SendOTPMessage sendOTPMessage = new SendOTPMessage();
				sendOTPMessage = await SendOtp_LoginRegistration(alternatemobileno, journeyOfAccount.Page, "send");

				if (sendOTPMessage != null && sendOTPMessage.ValidateCode == "Success")
				{
					journeyOfAccount.mobmasking = mobMasking;
					journeyOfAccount.Status = sendOTPMessage.ValidateCode;
					journeyOfAccount.AlternateUserMobile = alternatemobileno;
				}
				else
				{
					journeyOfAccount.Status = sendOTPMessage.ValidateCode;
				}

				SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

				return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "Pre Login - Main Block");
			}

			return Json(new { Status = "Fail", navigation = "" }, JsonRequestBehavior.AllowGet);
		}

		public async Task<SendOTPMessage> SendOtp_LoginRegistration(string UserName, string eventType, string sendType)
		{
			string OtpStatus = String.Empty;
			List<string> CC = new List<string>();
			List<string> BCC = new List<string>();
			string email = String.Empty;
			string name = String.Empty;
			string mobile = string.Empty;

			dbAccessClass dbAccessClass = new dbAccessClass();
			ValidateUser validateMe = new ValidateUser();
			Validate validate = new Validate();
			SendOTPMessage sendOTPMessage = new SendOTPMessage();

			if (!String.IsNullOrWhiteSpace(UserName))
			{
				try
				{
					if (!String.IsNullOrWhiteSpace(UserName) && validate.ValidateEmail(UserName) == true)
						email = UserName;
					if (!String.IsNullOrWhiteSpace(UserName) && validate.ValidateMobile(UserName) == true)
						mobile = UserName;

					validateMe = dbAccessClass.ValidateUser(email, mobile);

					if (validateMe != null)
					{
						if (validateMe.emailExists > 0)
						{
							sendOTPMessage.ValidateCode = "EmailExt";
							sendOTPMessage.ValidateMessage = "";
						}
						else if (validateMe.mobilenoExists > 0)
						{
							sendOTPMessage.ValidateCode = "MobileExt";
							sendOTPMessage.ValidateMessage = "";
						}
						else if (validateMe.emailExists == 0 && validateMe.mobilenoExists == 0)
						{
							string mailContentFrom = eventType;
							if (String.IsNullOrWhiteSpace(mailContentFrom))
								mailContentFrom = "registration";

							var mailContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
									.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "sMSOTPContentRoot")?.FirstOrDefault()?
									.Children?.OfType<MailContent>()?.Where(c => c.Type == mailContentFrom)?.FirstOrDefault();

							if (mailContent != null)
							{
								if (mailContent.EmailCC != null && mailContent.EmailCC.Count() > 0)
								{
									foreach (var item in mailContent.EmailCC)
									{
										CC.Add(item);
									}
								}

								if (mailContent.EmailBcc != null && mailContent.EmailBcc.Count() > 0)
								{
									foreach (var item in mailContent.EmailBcc)
									{
										BCC.Add(item);
									}
								}
							}
							else
							{
								Logger.Info(reporting: typeof(HomeContainer), "mail Content not found");
							}

							OtpStatus = await OtpManagement(mobile, email, name, null, "", "", "", eventType, sendType, mailContent?.Subject, CC, BCC);
							if (!String.IsNullOrWhiteSpace(OtpStatus) && OtpStatus == "Done")
							{
								sendOTPMessage.ValidateCode = "Success";
								sendOTPMessage.ValidateMessage = "";
							}
							else if (!String.IsNullOrWhiteSpace(OtpStatus) && OtpStatus == "Exceed")
							{
								sendOTPMessage.ValidateCode = "Exceed";
								sendOTPMessage.ValidateMessage = "";
							}
							else if (!String.IsNullOrWhiteSpace(OtpStatus) && (OtpStatus == "MAXFATT" || OtpStatus == "MAXRATT"))
							{
								string strTimeLeft = String.Empty;
								string remainingTime = SessionManagement.GetCurrentSession<string>(SessionType.InvalidOtpFailedTime);
								if (!String.IsNullOrWhiteSpace(remainingTime) && int.Parse(remainingTime) > 0)
								{
									TimeSpan rmtime = TimeSpan.FromSeconds(Convert.ToInt32(remainingTime));
									strTimeLeft = rmtime.ToString(@"mm\:ss");
								}

								sendOTPMessage.ValidateCode = OtpStatus;
								sendOTPMessage.ValidateMessage = strTimeLeft;
							}
							else
							{
								sendOTPMessage.ValidateCode = OtpStatus;
								sendOTPMessage.ValidateMessage = "";
							}
						}
					}
					else
					{
						sendOTPMessage.ValidateCode = "Fail";
						sendOTPMessage.ValidateMessage = "";
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(HomeContainer), ex, message: "SendOtp_LoginRegistration");
				}
			}

			return sendOTPMessage;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> LoginOtpSend(OtpResendParam otpResendParam)
		{
			try
			{

				JourneyOfAccount journeyOfAccount = new JourneyOfAccount();
				journeyOfAccount = SessionManagement.GetCurrentSession<JourneyOfAccount>(SessionType.JrnyUserDetails);

				string mode = journeyOfAccount?.Page == "" ? "auth" : journeyOfAccount?.Page;
				string response = await OtpManagementData(otpResendParam.UserName, mode, "send");

				if (!String.IsNullOrWhiteSpace(response) && (response == "ok" || response == "Done"))
				{
					string culture = String.Empty;
					culture = CultureName.GetCultureName();
					string navigaton = culture + "/my-account/user-authentication/";

					return Json(new { status = "Success", message = "", navigation = navigaton }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = response, message = "", navigation = "" }, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "LoginOtpSend");
			}

			return Json(new { status = "Fail", message = "", navigation = "" }, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginParam Input)
		{
			try
			{
				if (!String.IsNullOrEmpty(Input.UserName) && !String.IsNullOrEmpty(Input.PwdText))
				{
					ReturnMessage returnMessage = new ReturnMessage();
					Validate validate = new Validate();
					string email = String.Empty;
					string mobile = String.Empty;

					if (validate.ValidateEmail(Input.UserName) == true)
						email = Input.UserName.ToLower();
					else if (validate.ValidateMobile(Input.UserName) == true)
						mobile = Input.UserName;

					TempUserJourneyDtls UserHpIdDetails = new TempUserJourneyDtls();
					UserHpIdDetails.UserEmail = email;
					UserHpIdDetails.UserName = "";
					UserHpIdDetails.UserPassword = MD5HashPassword.GetMD5Hash(Input.PwdText);
					UserHpIdDetails.UserMobile = mobile;
					UserHpIdDetails.Source = "login";

					SessionManagement.StoreInSession(SessionType.TempUserDetails, UserHpIdDetails);

					returnMessage = LoggedInData(UserHpIdDetails, "", UserHpIdDetails.Source, "", Input.PageId);

					if (returnMessage.status == "Success")
					{
						SessionManagement.DeleteFromSession(SessionType.SpecialRedirection);

						string RegisteredUserId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
						return Json(new { status = returnMessage.status, navigation = returnMessage.navigation, message = returnMessage.message, UniqueUserId = RegisteredUserId, EnableTrackerCode = IsEnableTrackerCode }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "Login - Main Block");
			}

			return Json(new { status = "Error", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public ReturnMessage LoggedInData(TempUserJourneyDtls UserHpIdDetails, string redirectTo = "", string source = "", string loginuserInfoResponce = "", string PageId = "")
		{
			dbProxy _db = new dbProxy();
			LoggedIn loggedIn = new LoggedIn();
			ReturnMessage returnMessage = new ReturnMessage();

			//Store Culture Name
			//CultureManagePostHPID cultureManagePostHPID = new CultureManagePostHPID();
			//cultureManagePostHPID.CultureStorePostHpId();

			string culture = CultureName.GetCultureName();
			string myusername = String.Empty;
			string mobile = String.Empty;
			string loginsource = String.Empty;
			string LoginUserName = String.Empty;
			string UserTypeParam = String.Empty;
			string navigationUrl = String.Empty;

			if (source == "paylogin")
			{
				loginsource = "pay";
				myusername = UserHpIdDetails.UserEmail;
				mobile = UserHpIdDetails.UserMobile == null ? "" : UserHpIdDetails.UserMobile;
				//name = UserHpIdDetails.Name;
				//loginuserInfoResponce = "";
				if (!String.IsNullOrWhiteSpace(mobile))
					UserHpIdDetails.UserMobile = clsCommon.Encrypt(UserHpIdDetails.UserMobile.ToLower().Trim());
				if (!String.IsNullOrWhiteSpace(myusername))
					UserHpIdDetails.UserEmail = clsCommon.Encrypt(UserHpIdDetails.UserEmail.ToLower().Trim());
				//UserHpIdDetails.HPiD = UserHpIdDetails.HPiD;
			}
			else if (source == "browsercloselogin")
			{
				if (!String.IsNullOrWhiteSpace(UserHpIdDetails.UserEmail))
					myusername = UserHpIdDetails.UserEmail;
				if (!String.IsNullOrWhiteSpace(UserHpIdDetails.UserMobile))
					mobile = UserHpIdDetails.UserMobile;
			}
			else
			{
				if (!String.IsNullOrWhiteSpace(UserHpIdDetails.UserEmail))
					myusername = clsCommon.Encrypt(UserHpIdDetails.UserEmail.ToLower().Trim());
				if (!String.IsNullOrWhiteSpace(UserHpIdDetails.UserMobile))
					mobile = clsCommon.Encrypt(UserHpIdDetails.UserMobile);
			}

			//Sent parameter for hp.com users
			try
			{
				if (UserHpIdDetails.UserEmail.Contains("@"))
					UserTypeParam = UserHpIdDetails.UserEmail.ToString().Split('@')[1];
			}
			catch { }

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@email", Value = myusername },
				new SetParameters{ ParameterName = "@mobile", Value = mobile },
				new SetParameters{ ParameterName = "@UserTypeCheckParam", Value = UserTypeParam },
				new SetParameters{ ParameterName = "@Source", Value = source },
				new SetParameters{ ParameterName = "@Password", Value = UserHpIdDetails.UserPassword  == null ? "" :UserHpIdDetails.UserPassword}
			};

			loggedIn = _db.GetData<LoggedIn>("usp_Login", loggedIn, sp);
			if (loggedIn != null && loggedIn.ResponseText == "Success" && loggedIn.UserId > 0)
			{
				SessionManagement.DeleteFromSession(SessionType.TempUserDetails);

				SessionManagement.StoreInSession(SessionType.UserId, loggedIn.UserId);
				SessionManagement.StoreInSession(SessionType.UserUniqueId, loggedIn.UserUniqueId);
				SessionManagement.StoreInSession(SessionType.UserType, loggedIn.UserType);
				SessionManagement.StoreInSession(SessionType.IsLoggedIn, "Y");
				SessionManagement.StoreInSession(SessionType.LoggedInDtls, loggedIn);
				SessionManagement.StoreInSession(SessionType.whatsAppNo, loggedIn.u_whatsappno);
				SessionManagement.StoreInSession(SessionType.emailid, loggedIn.u_email);
				SessionManagement.StoreInSession(SessionType.UserReferralCode, loggedIn.ReferralCode);
				SessionManagement.StoreInSession(SessionType.DataSource, loggedIn.RegSource);
				SessionManagement.StoreInSession(SessionType.RegistrationMode, loggedIn.RegistrationMode);
				SessionManagement.StoreInSession(SessionType.SubscribedOrNot, loggedIn.SubscribedOrNot);
				SessionManagement.StoreInSession(SessionType.SubscribedOrNotBonus, loggedIn.SubscribedOrNotBonus);
				SessionManagement.StoreInSession(SessionType.AgeGroupExistsOrNot, loggedIn.AgeGroupExistsOrNot);
				SessionManagement.StoreInSession(SessionType.UserRegistrationMode, loggedIn.UserRegistrationMode);


				//Remember me
				dbAccessClass dbAccessClass;
				if (!String.IsNullOrEmpty(UserHpIdDetails.Rememberme) && UserHpIdDetails.Rememberme.ToLower().Equals("yes"))
				{
					dbAccessClass = new dbAccessClass();
					dbAccessClass.CookieStore(loggedIn.UserId.ToString());
				}
				else
				{
					dbAccessClass = new dbAccessClass();
					dbAccessClass.CookieExpire();
				}

				if (!String.IsNullOrWhiteSpace(source) && source != "notCommunicate")
				{
					//SessionManagement.StoreInSession(SessionType.NoOfSubscribed, loggedIn.NoOfSubscribed);
					//SessionManagement.StoreInSession(SessionType.SubscriptionValidationText, loggedIn.SubscriptionValidationText);
					//SessionManagement.StoreInSession(SessionType.UserAgeGroup, loggedIn.AgeGroup);

					//Get all subscription
					List<GetUserCurrentSubscription> mySubscription = new List<GetUserCurrentSubscription>();
					dbAccessClass db = new dbAccessClass();
					mySubscription = db.GetUserSubscriptions();

					SessionManagement.StoreInSession(SessionType.SubscriptionInDtls, mySubscription);


					try
					{
						//Check Age Group Exists
						if (loggedIn != null && loggedIn.UserId > 0 && !string.IsNullOrEmpty(loggedIn.AgeGroupExistsOrNot))
						{
							//download pdf file
							string dwnldPdf = HPPlc.Models.SessionManagement.GetCurrentSession<string>(HPPlc.Models.SessionType.WorksheetDownloadUrl);
							if (!String.IsNullOrWhiteSpace(dwnldPdf))
							{
								returnMessage = DownloadPDF(dwnldPdf);
							}
							else
							{

								string IsBotRequest = String.Empty;
								//string IsBotRequest = SessionManagement.GetCurrentSession<string>(SessionType.IsBotRequest);
								if (!String.IsNullOrWhiteSpace(IsBotRequest) && IsBotRequest == "Yes")
								{
									navigationUrl = culture + "/subscription/buy-now";

									returnMessage.status = "Success";
									returnMessage.navigation = navigationUrl;
									returnMessage.message = "";
								}
								else if (loggedIn.AgeGroupExistsOrNot.ToLower() == "no")
								{
									navigationUrl = culture + "/subscription/add-age-group";

									returnMessage.status = "Success";
									returnMessage.navigation = navigationUrl;
									returnMessage.message = "";
								}
								else if (!String.IsNullOrWhiteSpace(PageId) && PageId == "offer")
								{
									//User from offer page
									string offerSubscriptionUrl = ConfigurationManager.AppSettings["OfferSubscriptionUrl"].ToString();
									navigationUrl = culture + offerSubscriptionUrl;

									//SessionManagement.DeleteFromSession(SessionType.HpOfferRedirection);

									returnMessage.status = "Success";
									returnMessage.navigation = navigationUrl;
									returnMessage.message = "";
								}
								else if (!String.IsNullOrWhiteSpace(loggedIn.ReferralBenefitPlan) && (loggedIn.ReferralBenefitPlan.Equals("B") || loggedIn.ReferralBenefitPlan.Equals("L")))// Referral Joining Plan
								{
									string redirectUrl = String.Empty;
									string couponcode = String.Empty;
									if (loggedIn.ReferralBenefitPlan.Equals("B"))//Bonus
									{
										redirectUrl = ConfigurationManager.AppSettings["bonus199PlanUrl"].ToString();
										couponcode = ConfigurationManager.AppSettings["RefBonusCoupon"].ToString();

										SessionManagement.StoreInSession(SessionType.SplRedirection, redirectUrl);
										HPPlc.Models.SessionManagement.StoreInSession(HPPlc.Models.SessionType.CouponCode, couponcode);
									}
									else if (loggedIn.ReferralBenefitPlan.Equals("L"))//Lesson
									{
										redirectUrl = ConfigurationManager.AppSettings["lesson399PlanUrl"].ToString();
										couponcode = ConfigurationManager.AppSettings["RefLessonCoupon"].ToString();

										SessionManagement.StoreInSession(SessionType.SplRedirection, redirectUrl);
										HPPlc.Models.SessionManagement.StoreInSession(HPPlc.Models.SessionType.CouponCode, couponcode);
									}
								}
								else
								{
									SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
									subscriptionDetails = SessionManagement.GetCurrentSession<SubscriptionDetails>(SessionType.SubscriptionDtls);

									if (subscriptionDetails != null)
									{
										//string navigationUrl = String.Empty;
										var subscriptionRank = Umbraco?.Content(subscriptionDetails.subscriptionId).DescendantsOrSelf().OfType<Subscriptions>().FirstOrDefault();

										if (subscriptionRank != null)
										{
											if (subscriptionRank.Ranking == "1" || !String.IsNullOrEmpty(subscriptionDetails.WorksheetId))
											{
												returnMessage.status = "Success";
												returnMessage.navigation = culture + "/";
												returnMessage.message = "";
											}
											else
											{
												navigationUrl = subscriptionDetails.targetUrl + "?subscriptionid=" + clsCommon.Encrypt(subscriptionDetails.subscriptionId);

												if (!String.IsNullOrEmpty(subscriptionDetails.ageGroup))
													navigationUrl = navigationUrl + "&age=" + subscriptionDetails.ageGroup;

												returnMessage.status = "Success";
												returnMessage.navigation = navigationUrl;
												returnMessage.message = "";
											}
										}

										//return Json(new { status = "Success", navigation = culture + subscriptionDetails.targetUrl, message = "" }, JsonRequestBehavior.AllowGet);
									}
									else
									{
										string subsStatus = SessionManagement.GetCurrentSession<string>(SessionType.subscriptionPopup);
										if (subsStatus == "open" || redirectTo == "redirectonsubscription")
										{
											returnMessage.status = "Success";
											returnMessage.navigation = culture + "/subscription";
											returnMessage.message = "";
											//return Json(new { status = "Success", navigation = culture + "/subscription", message = "" }, JsonRequestBehavior.AllowGet);
										}
										else
										{
											if (SessionManagement.GetCurrentSession<string>(SessionType.ExpertTalkUrl) != null)
											{
												clsExpertHelper clsExpertHelper = new clsExpertHelper();
												GetStatus insertStatus = clsExpertHelper.insertExpertData();
												returnMessage.status = "Success";
												returnMessage.navigation = SessionManagement.GetCurrentSession<string>(SessionType.MettingName);
												returnMessage.message = "";
											}
											if (SessionManagement.GetCurrentSession<string>(SessionType.JoinNowUrl) != null)
											{
												returnMessage.status = "Success";
												returnMessage.navigation = SessionManagement.GetCurrentSession<string>(SessionType.JoinNowUrl);
												returnMessage.message = "";
											}
											else if (SessionManagement.GetCurrentSession<string>(SessionType.SpecialRedirection) != null)
											{
												//CheckUserSpecialPlanRedeemed
												db = new dbAccessClass();
												SpecialRedirectionCheck specialRedirectionCheck = new SpecialRedirectionCheck();
												specialRedirectionCheck = db.CheckUserSpecialPlanRedeemed();

												if (specialRedirectionCheck.UserId == 0)
												{
													string specialRedirection = SessionManagement.GetCurrentSession<string>(SessionType.SpecialRedirection);
													if (!String.IsNullOrWhiteSpace(specialRedirection) && specialRedirection == "Y")
													{
														returnMessage.status = "Success";
														returnMessage.navigation = "";
														returnMessage.message = "SPR_365Plan";
													}
												}
												else
												{
													returnMessage.status = "Success";
													returnMessage.navigation = "/learn-365";
													returnMessage.message = "";
												}
												//SessionManagement.DeleteFromSession(SessionType.SpecialRedirection);
											}
											else if (SessionManagement.GetCurrentSession<string>(SessionType.SplRedirection) != null)
											{
												returnMessage.status = "Success";
												returnMessage.navigation = SessionManagement.GetCurrentSession<string>(SessionType.SplRedirection);
												returnMessage.message = "";
											}
											else if (!String.IsNullOrWhiteSpace(source) && source.ToLower() == "teacher")
											{
												returnMessage.status = "Success";
												returnMessage.navigation = ConfigurationManager.AppSettings["teracherplanredirection"].ToString();
												returnMessage.message = "";
											}
											//else
											//{
											//	string redirectUrl = "/";
											//	//System.Web.HttpCookie PreUrlRedirections = Request?.Cookies["PreUrlRedirection"];

											//	//if (PreUrlRedirections != null && PreUrlRedirections.Value != null)
											//	//	redirectUrl = PreUrlRedirections.Value;

											//	returnMessage.status = "Success";
											//	returnMessage.navigation = redirectUrl;
											//	returnMessage.message = "";

											//}
										}
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(HomeController), ex, message: "Login - Blank age group block");
					}
				}
			}
			//else if (loggedIn != null && loggedIn.ResponseText == "register")
			//{
			//	returnMessage.status = loggedIn.ResponseText;

			//	if (!String.IsNullOrEmpty(culture))
			//		culture = "/" + culture;

			//	returnMessage.navigation = culture + "/my-account/registration";
			//	returnMessage.message = "";
			//}
			//User registered but password not set
			//else if (loggedIn != null && loggedIn.ResponseText == "Set" && loggedIn.UserId > 0)
			//{
			//	string siteURL = ConfigurationManager.AppSettings["SiteUrl"].ToString();

			//	string OneTimeSetPasswordLink = siteURL + "my-account/set-password?id=" + clsCommon.encrypto(loggedIn.UserId.ToString());

			//	returnMessage.status = "Set";
			//	returnMessage.navigation = OneTimeSetPasswordLink;
			//	returnMessage.message = loggedIn.ReferralCode;
			//}
			else
			{
				returnMessage.status = "Fail";
				returnMessage.navigation = "";
				returnMessage.message = "";

				//return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
			}

			return returnMessage;
			//return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public ReturnMessage LoggedInDataSpecialPlan(TempUserJourneyDtls UserHpIdDetails, string redirectTo = "", string source = "", string loginuserInfoResponce = "", string PageId = "")
		{
			dbProxy _db = new dbProxy();
			LoggedIn loggedIn = new LoggedIn();
			ReturnMessage returnMessage = new ReturnMessage();

			string culture = CultureName.GetCultureName();
			string myusername = String.Empty;
			string mobile = String.Empty;
			string loginsource = String.Empty;
			string LoginUserName = String.Empty;
			string UserTypeParam = String.Empty;
			string navigationUrl = String.Empty;


			if (!String.IsNullOrWhiteSpace(UserHpIdDetails.UserEmail))
				myusername = clsCommon.Encrypt(UserHpIdDetails.UserEmail.ToLower().Trim());
			if (!String.IsNullOrWhiteSpace(UserHpIdDetails.UserMobile))
				mobile = clsCommon.Encrypt(UserHpIdDetails.UserMobile);

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@email", Value = myusername },
				new SetParameters{ ParameterName = "@mobile", Value = mobile },
				new SetParameters{ ParameterName = "@UserTypeCheckParam", Value = UserTypeParam== null ? "" : UserTypeParam },
				new SetParameters{ ParameterName = "@Source", Value = UserHpIdDetails.Source == null ? "" : UserHpIdDetails.Source },
				new SetParameters{ ParameterName = "@Password", Value = UserHpIdDetails.UserPassword == null ? "" : UserHpIdDetails.UserPassword }
			};

			if (UserHpIdDetails.AlreadyRegCommMode > 0)
				loggedIn = _db.GetData<LoggedIn>("usp_Login_SpecialPlan", loggedIn, sp);
			if (UserHpIdDetails.AlreadyRegCommMode == -1)
				loggedIn = _db.GetData<LoggedIn>("usp_Login_SecondForm", loggedIn, sp);
			else
				loggedIn = _db.GetData<LoggedIn>("usp_Login", loggedIn, sp);

			if (loggedIn != null && loggedIn.ResponseText == "Success" && loggedIn.UserId > 0)
			{
				SessionManagement.DeleteFromSession(SessionType.TempUserDetails);

				SessionManagement.StoreInSession(SessionType.UserId, loggedIn.UserId);
				SessionManagement.StoreInSession(SessionType.UserUniqueId, loggedIn.UserUniqueId);
				SessionManagement.StoreInSession(SessionType.UserType, loggedIn.UserType);
				SessionManagement.StoreInSession(SessionType.IsLoggedIn, "Y");
				SessionManagement.StoreInSession(SessionType.LoggedInDtls, loggedIn);
				SessionManagement.StoreInSession(SessionType.whatsAppNo, loggedIn.u_whatsappno);
				SessionManagement.StoreInSession(SessionType.emailid, loggedIn.u_email);
				SessionManagement.StoreInSession(SessionType.UserReferralCode, loggedIn.ReferralCode);
				SessionManagement.StoreInSession(SessionType.RegistrationMode, loggedIn.RegistrationMode);
				SessionManagement.StoreInSession(SessionType.SubscribedOrNot, loggedIn.SubscribedOrNot);
				SessionManagement.StoreInSession(SessionType.AgeGroupExistsOrNot, loggedIn.AgeGroupExistsOrNot);

				returnMessage.status = "Success";
				returnMessage.navigation = "";
				returnMessage.message = "";

			}
			if (loggedIn != null && loggedIn.ResponseText == "Success" && loggedIn.UserId == 0)
			{
				returnMessage.status = "new";
				returnMessage.navigation = "";
				returnMessage.message = "";
			}
			else
			{
				returnMessage.status = "Fail";
				returnMessage.navigation = "";
				returnMessage.message = "";

				//return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
			}

			return returnMessage;
			//return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public ReturnMessage DownloadPDF(string dwnldPdf)
		{
			ReturnMessage returnMessage = new ReturnMessage();

			try
			{
				Uri myUri = new Uri(dwnldPdf);
				string param = HttpUtility.ParseQueryString(myUri.Query).Get("d");

				string dec = clsCommon.DecryptWithBase64Code(param);
				if (!String.IsNullOrWhiteSpace(dec))
				{
					string[] queryvar = dec.Split('&');
					if (queryvar.Length >= 2)
					{
						int loggedInUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
						int UserId = int.Parse(queryvar[1].Replace("UID=", ""));

						if (loggedInUserId == UserId)
						{
							//string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
							//string downloadUrl = "/umbraco/Surface/WorkSheet/DownloadData?WID=" + clsCommon.encrypto(queryvar[0].ToString().Replace("WID=", "")) + "&source=WhatsApp";
							string downloadUrl = "/worksheet-download?d=" + clsCommon.Encryptwithbase64Code("WID=" + queryvar[0].ToString().Replace("WID=", "") + "&UID=" + UserId.ToString());
							//SessionManagement.StoreInSession(SessionType.WorksheetDownloadUrl, downloadUrl);

							SessionManagement.DeleteFromSession(SessionType.WorksheetDownloadUrl);

							returnMessage.status = "Success";
							returnMessage.navigation = downloadUrl;
							returnMessage.message = "";
						}
						else
						{
							Logger.Info(reporting: typeof(HomeController), "Not downloaded because userid not matched");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "Login - download pdf file");
			}

			return returnMessage;
		}


		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Registration(Models.Registration registration)
		{
			try
			{
				JourneyOfAccount journeyOfAccount = new JourneyOfAccount();
				journeyOfAccount = SessionManagement.GetCurrentSession<JourneyOfAccount>(SessionType.JrnyUserDetails);
				if (journeyOfAccount != null && journeyOfAccount.IsotpVerified == 1)
				{
					Validate validate = new Validate();
					if (String.IsNullOrWhiteSpace(registration.email) || validate.ValidateEmail(registration.email) == false)
					{
						return Json(new { status = "vldtnmail" }, JsonRequestBehavior.AllowGet);
					}
					else if (registration.ageGroup == null || registration.ageGroup.Length == 0)
					{
						return Json(new { status = "vldtnage" }, JsonRequestBehavior.AllowGet);
					}
					else if (!String.IsNullOrWhiteSpace(registration.mobileno) && validate.ValidateMobile(registration.mobileno.ToString()) == false)
					{
						return Json(new { status = "vldtnmob" }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						//Referral Code
						if (!string.IsNullOrWhiteSpace(registration.referralCode))
						{
							Responce refRresponce = new Responce();
							refRresponce = CheckReferralCodeIsValid(registration.referralCode);
							if (refRresponce != null && refRresponce.StatusCode != 0 && refRresponce.StatusCode != HttpStatusCode.OK)
							{
								return Json(new
								{
									status = "Fail",
									navigation = "referral",
									message = refRresponce.Message
								}, JsonRequestBehavior.AllowGet);
							}
						}

						string culture = CultureName.GetCultureName();
						GetStatus insertStatus = new GetStatus();
						//Register here

						//Otp Verification
						GetStatus response = new GetStatus();
						if (!String.IsNullOrWhiteSpace(registration.email))
						{
							string UserName = clsCommon.Encrypt(registration.email.ToLower());

							//Registration
							insertStatus = Register(registration, "Form");
							if (insertStatus != null && insertStatus.returnStatus == "Success" && insertStatus.returnValue > 0)
							{

								try
								{
									//SFMC Data Entry
									if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
									{
										SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
										sendDataToSFMC.PostDataSFMC(insertStatus.returnValue, "", "registration");
									}
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(HomeController), ex, message: "registration - SFMC Issue");
								}

								try
								{
									//Send Data to SFMC Bonus plan
									if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
									{
										SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
										sendDataToSFMC.PostDataSFMCBonus(insertStatus.returnValue, "", "subscriptionbonus");
									}
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(SubscriptionController), ex, message: "registration bonus - Send Data to SFMC");
								}

								try
								{
									//SFMC Data Entry
									if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
									{
										SendDataToSFMC sendDataToSFMCt = new SendDataToSFMC();
										sendDataToSFMCt.PostDataSFMC(insertStatus.returnValue, "", "registrationInviteUser");
									}
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(HomeController), ex, message: "registration - SFMC Issue");
								}

								//After login
								string loginStatus = await PostRegistrationActivity();

								//Return URL for OTp window
								string navUrl = culture + "/subscription";
								string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
								string navigationUrl = String.Empty;

								SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
								subscriptionDetails = SessionManagement.GetCurrentSession<SubscriptionDetails>(SessionType.SubscriptionDtls);
								string IsOfferActive = SessionManagement.GetCurrentSession<string>(SessionType.HpOfferRedirection);
								if (subscriptionDetails != null)
								{
									//string navigationUrl = String.Empty;
									var subscriptionRank = Umbraco?.Content(subscriptionDetails.subscriptionId).DescendantsOrSelf().OfType<Subscriptions>().FirstOrDefault();

									if (subscriptionRank != null)
									{
										if (subscriptionRank.Ranking == "1" || !String.IsNullOrEmpty(subscriptionDetails.WorksheetId))
										{
											return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = navUrl, message = "" }, JsonRequestBehavior.AllowGet);
										}
										else
										{
											navigationUrl = subscriptionDetails.targetUrl + "?subscriptionid=" + clsCommon.Encrypt(subscriptionDetails.subscriptionId);

											if (!String.IsNullOrEmpty(subscriptionDetails.ageGroup))
												navigationUrl = navigationUrl + "&age=" + subscriptionDetails.ageGroup;

											return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = navigationUrl, message = "" }, JsonRequestBehavior.AllowGet);
										}
									}
								}
								else if (!String.IsNullOrWhiteSpace(IsOfferActive) && IsOfferActive.ToLower() == "yes")
								{
									//User Clicked on Offer
									string offerSubscriptionUrl = ConfigurationManager.AppSettings["OfferSubscriptionUrl"].ToString();
									navigationUrl = culture + offerSubscriptionUrl;

									//SessionManagement.DeleteFromSession(SessionType.HpOfferRedirection);

									return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = navigationUrl, message = "" }, JsonRequestBehavior.AllowGet);
								}
								else if (SessionManagement.GetCurrentSession<string>(SessionType.JoinNowUrl) != null)
								{
									return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = SessionManagement.GetCurrentSession<string>(SessionType.JoinNowUrl), message = "" }, JsonRequestBehavior.AllowGet);
								}

								return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = navUrl, message = "" }, JsonRequestBehavior.AllowGet);
							}
							else
							{

								return Json(new { status = insertStatus.returnStatus, message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
							}

							//}
							//else
							//{
							//	return Json(new { status = response.returnStatus, navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
							//}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "Registration - Main Block" + ex.Message.ToString());
			}

			return Json(new { status = "Fail", message = "" }, JsonRequestBehavior.AllowGet);
		}

		//[HttpPost]
		//[ValidateInput(false)]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> Registration(Models.Registration registration)
		//{
		//	try
		//	{
		//		Validate validate = new Validate();
		//		if (String.IsNullOrWhiteSpace(registration.email) || validate.ValidateEmail(registration.email) == false)
		//		{
		//			return Json(new { status = "vldtnmail" }, JsonRequestBehavior.AllowGet);
		//		}
		//		else if (registration.ageGroup == null || registration.ageGroup.Length == 0)
		//		{
		//			return Json(new { status = "vldtnage" }, JsonRequestBehavior.AllowGet);
		//		}
		//		else if (!String.IsNullOrWhiteSpace(registration.mobileno) && validate.ValidateMobile(registration.mobileno.ToString()) == false)
		//		{
		//			return Json(new { status = "vldtnmob" }, JsonRequestBehavior.AllowGet);
		//		}
		//		else if (String.IsNullOrWhiteSpace(registration.regpassword) || validate.ValidatePassword(registration.regpassword) == false)
		//		{
		//			return Json(new { status = "vldtnpasswd" }, JsonRequestBehavior.AllowGet);
		//		}
		//		else
		//		{
		//			string culture = CultureName.GetCultureName();
		//			GetStatus insertStatus = new GetStatus();
		//			//Register here

		//			//Otp Verification
		//			GetStatus response = new GetStatus();
		//			if (!String.IsNullOrWhiteSpace(registration.email))
		//			{
		//				string UserName = clsCommon.Encrypt(registration.email.ToLower());
		//				response = OtpVerify("registration", UserName, registration.Otp);

		//				if (response != null && response?.returnStatus == "OTP_NM")
		//				{
		//					return Json(new { status = "OTP_NM", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		//				}
		//				if (response != null && response?.returnStatus == "SUCCESS")
		//				{
		//					//Registration
		//					insertStatus = Register(registration, "Form");
		//					if (insertStatus != null && insertStatus.returnStatus == "Success" && insertStatus.returnValue > 0)
		//					{
		//						//save data into sycn with solution start
		//						//try
		//						//{
		//						//	Responce post = new Responce();
		//						//	ApiCallServices apiCall = new ApiCallServices();
		//						//	RegistrationPostModel postModel = new RegistrationPostModel();
		//						//	List<Item> items = new List<Item>();
		//						//	items.Add(new Item()
		//						//	{
		//						//		UserId = insertStatus.returnValue,
		//						//		u_name = registration.name,
		//						//		EmailAddress = registration.email,
		//						//		u_whatsappno_prefix = registration.whatsupprefix,
		//						//		u_whatsappno = registration.whatsupnumber,
		//						//		age_group = string.Join(",", registration.ageGroup),
		//						//		WhatsAppConsent = registration.supportOnWhatsupFromHP,
		//						//		EmailConsent = registration.supportOnEmailFromHP,
		//						//		PhoneConsent = registration.supportOnPhoneFromHP,
		//						//		CheckedTnC = registration.termsChecked,
		//						//		EncPassword = "",//null
		//						//		register_date = DateTime.Now.ToString("yyyy-MM-dd"),
		//						//		subscription_lvl = "",//null
		//						//		TransationId = ""
		//						//	});
		//						//	;
		//						//	postModel.Items = items;
		//						//	post = apiCall.PostRegistartionData(postModel);
		//						//}
		//						//catch (Exception ex)
		//						//{
		//						//	Logger.Error(reporting: typeof(HomeController), ex, message: "registration - Save RquestId in Sync Solution");
		//						//}

		//						try
		//						{
		//							//SFMC Data Entry
		//							if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
		//							{
		//								SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
		//								sendDataToSFMC.PostDataSFMC(insertStatus.returnValue, "", "registration");
		//							}
		//						}
		//						catch (Exception ex)
		//						{
		//							Logger.Error(reporting: typeof(HomeController), ex, message: "registration - SFMC Issue");
		//						}

		//						//After login
		//						string loginStatus = await PostRegistrationActivity();

		//						//Return URL for OTp window
		//						string navUrl = culture + "/subscription";
		//						string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
		//						string navigationUrl = String.Empty;

		//						SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
		//						subscriptionDetails = SessionManagement.GetCurrentSession<SubscriptionDetails>(SessionType.SubscriptionDtls);
		//						string IsOfferActive = SessionManagement.GetCurrentSession<string>(SessionType.HpOfferRedirection);
		//						if (subscriptionDetails != null)
		//						{
		//							//string navigationUrl = String.Empty;
		//							var subscriptionRank = Umbraco?.Content(subscriptionDetails.subscriptionId).DescendantsOrSelf().OfType<Subscriptions>().FirstOrDefault();

		//							if (subscriptionRank != null)
		//							{
		//								if (subscriptionRank.Ranking == "1" || !String.IsNullOrEmpty(subscriptionDetails.WorksheetId))
		//								{
		//									return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = navUrl, message = "" }, JsonRequestBehavior.AllowGet);
		//								}
		//								else
		//								{
		//									navigationUrl = subscriptionDetails.targetUrl + "?subscriptionid=" + clsCommon.Encrypt(subscriptionDetails.subscriptionId);

		//									if (!String.IsNullOrEmpty(subscriptionDetails.ageGroup))
		//										navigationUrl = navigationUrl + "&age=" + subscriptionDetails.ageGroup;

		//									return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = navigationUrl, message = "" }, JsonRequestBehavior.AllowGet);
		//								}
		//							}
		//						}
		//						else if (SessionManagement.GetCurrentSession<string>(SessionType.SpecialRedirection) != null)
		//						{
		//							dbAccessClass db = new dbAccessClass();
		//							SpecialRedirectionCheck specialRedirectionCheck = new SpecialRedirectionCheck();
		//							specialRedirectionCheck = db.CheckUserSpecialPlanRedeemed();

		//							if (specialRedirectionCheck.UserId == 0)
		//							{
		//								string specialRedirection = SessionManagement.GetCurrentSession<string>(SessionType.SpecialRedirection);
		//								if (!String.IsNullOrWhiteSpace(specialRedirection) && specialRedirection == "Y")
		//								{
		//									return Json(new { status = "Success", navigation = "", message = "SPR_365Plan" }, JsonRequestBehavior.AllowGet);
		//								}
		//							}
		//							else
		//							{
		//								return Json(new { status = "Success", navigation = "/special-plan", message = "" }, JsonRequestBehavior.AllowGet);
		//							}

		//							SessionManagement.DeleteFromSession(SessionType.SpecialRedirection);
		//						}
		//						else if (!String.IsNullOrWhiteSpace(IsOfferActive) && IsOfferActive.ToLower() == "yes")
		//						{
		//							//User Clicked on Offer
		//							string offerSubscriptionUrl = ConfigurationManager.AppSettings["OfferSubscriptionUrl"].ToString();
		//							navigationUrl = culture + offerSubscriptionUrl;

		//							//SessionManagement.DeleteFromSession(SessionType.HpOfferRedirection);

		//							return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = navigationUrl, message = "" }, JsonRequestBehavior.AllowGet);
		//						}


		//						return Json(new { status = "Success", UniqueUserId = UserUniqueId, navigation = navUrl, message = "" }, JsonRequestBehavior.AllowGet);
		//					}
		//					else
		//					{
		//						if (insertStatus != null && insertStatus.returnStatus == "Fail" && insertStatus.returnValue > 0)
		//						{
		//							return Json(new { status = "Regist", message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
		//						}

		//						return Json(new { status = "Fail", message = "" }, JsonRequestBehavior.AllowGet);
		//					}

		//				}
		//				else
		//				{
		//					return Json(new { status = response.returnStatus, navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		//				}
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(HomeController), ex, message: "Registration - Main Block" + ex.Message.ToString());
		//	}

		//	return Json(new { status = "Fail", message = "" }, JsonRequestBehavior.AllowGet);
		//}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RegistrationOtp(RegistrationOtp Input)
		{
			string OtpStatus = String.Empty;
			List<string> CC = new List<string>();
			List<string> BCC = new List<string>();
			string email = String.Empty;
			string name = String.Empty;
			string page = String.Empty;
			string mobile = string.Empty;
			//string mobMasking = string.Empty;
			//string mailMasking = string.Empty;

			if (Input != null && (!String.IsNullOrWhiteSpace(Input.UserName)))
			{
				//if (!String.IsNullOrWhiteSpace(Input.mobileno))
				//	mobMasking = MobMailMasking.hidePhoneNum(Input.mobileno);
				//if (!String.IsNullOrWhiteSpace(Input.email))
				//	mailMasking = MobMailMasking.hideEmailId(Input.email.ToLower());

				Validate validate = new Validate();
				ValidateUser validateMe = new ValidateUser();

				if (validate.ValidateEmail(Input.UserName) == true)
					email = Input.UserName.ToLower();
				else if (validate.ValidateMobile(Input.UserName) == true)
					mobile = Input.UserName.ToLower();

				dbAccessClass dbAccessClass = new dbAccessClass();

				try
				{
					//TempUserJourneyDtls tempUserJourneyDtls = new TempUserJourneyDtls();
					//tempUserJourneyDtls.UserMobile = mobile;
					//tempUserJourneyDtls.UserEmail = email.ToLower();
					//tempUserJourneyDtls.Source = "registration";
					//tempUserJourneyDtls.StepsCompletted = 1;
					//tempUserJourneyDtls.UserId = 0;
					//tempUserJourneyDtls.UserPassword = "";

					JourneyOfAccount journeyOfAccount = new JourneyOfAccount();

					journeyOfAccount = SessionManagement.GetCurrentSession<JourneyOfAccount>(SessionType.JrnyUserDetails);
					if (journeyOfAccount != null)
						page = journeyOfAccount.Page;
					else
						page = "Login";

					//User not registered - go to registration page
					journeyOfAccount.Status = "Success";
					journeyOfAccount.UserName = "";
					journeyOfAccount.UserEmail = email;
					journeyOfAccount.UserMobile = mobile;
					journeyOfAccount.UserId = 0;

					//SessionManagement.StoreInSession(SessionType.TempUserDetails, tempUserJourneyDtls);
					SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

					//if (!String.IsNullOrWhiteSpace(Input.email))
					//	email = clsCommon.Encrypt(Input.email.ToLower().Trim());
					//if (!String.IsNullOrWhiteSpace(Input.mobileno))
					//	mobile = clsCommon.Encrypt(Input.mobileno);
					//if (!String.IsNullOrWhiteSpace(Input.name))
					//	name = clsCommon.Encrypt(Input.name);

					validateMe = dbAccessClass.ValidateUser(email, mobile);

					if (validateMe != null)
					{
						//Check referral 
						//Responce refRresponce = new Responce();
						//if (!string.IsNullOrWhiteSpace(Input.referralCode))
						//	refRresponce = CheckReferralCodeIsValid(Input.referralCode);
						//else
						//	Input.referralCode = "";

						if (validateMe.emailExists > 0)
						{
							return Json(new { status = "EmailExt", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
						}
						else if (validateMe.mobilenoExists > 0)
						{
							return Json(new { status = "MobileExt", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
						}
						//else if (refRresponce != null && refRresponce.StatusCode != 0 && refRresponce.StatusCode != HttpStatusCode.OK)
						//{
						//	return Json(new
						//	{
						//		status = "Fail",
						//		navigation = "referral",
						//		message = refRresponce.Message
						//	}, JsonRequestBehavior.AllowGet);
						//}
						else if (validateMe.emailExists == 0 && validateMe.mobilenoExists == 0)
						{
							var mailContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
									.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "sMSOTPContentRoot")?.FirstOrDefault()?
									.Children?.OfType<MailContent>()?.Where(c => c.Type == "registration")?.FirstOrDefault();

							if (mailContent != null)
							{
								if (mailContent.EmailCC != null && mailContent.EmailCC.Count() > 0)
								{
									foreach (var item in mailContent.EmailCC)
									{
										CC.Add(item);
									}
								}

								if (mailContent.EmailBcc != null && mailContent.EmailBcc.Count() > 0)
								{
									foreach (var item in mailContent.EmailBcc)
									{
										BCC.Add(item);
									}
								}
							}

							OtpStatus = await OtpManagement(mobile, email, Input.name, Input.ageGroup, Input.supportOnEmailFromHP, Input.supportOnWhatsupFromHP, Input.referralCode, page, Input.type == "" ? "send" : Input.type, mailContent.Subject, CC, BCC);

							if (!String.IsNullOrWhiteSpace(OtpStatus) && OtpStatus == "Done")
							{
								return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
							}
							else if (!String.IsNullOrWhiteSpace(OtpStatus) && OtpStatus == "Exceed")
							{
								return Json(new { status = "Exceed", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
							}
							else if (!String.IsNullOrWhiteSpace(OtpStatus) && (OtpStatus == "MAXFATT" || OtpStatus == "MAXRATT"))
							{
								string strTimeLeft = String.Empty;
								string remainingTime = SessionManagement.GetCurrentSession<string>(SessionType.InvalidOtpFailedTime);
								if (!String.IsNullOrWhiteSpace(remainingTime) && int.Parse(remainingTime) > 0)
								{
									TimeSpan rmtime = TimeSpan.FromSeconds(Convert.ToInt32(remainingTime));
									strTimeLeft = rmtime.ToString(@"mm\:ss");
								}
								return Json(new { status = OtpStatus, navigation = "", message = strTimeLeft }, JsonRequestBehavior.AllowGet);
							}
							else
							{
								return Json(new { status = OtpStatus, navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
							}
						}
					}
					else
					{
						OtpStatus = "Fail";
					}

					string response = await OtpManagementData(Input.email.ToLower(), "registration", "send");

					if (!String.IsNullOrWhiteSpace(response) && response.ToLower() == "done" || response.ToLower() == "ok")
					{
						return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(HomeContainer), ex, message: "RegistrationOtp");
				}
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SpecialPlanRegistration(Models.Registration Input)
		{
			string email = String.Empty;
			string name = String.Empty;
			string mobile = string.Empty;

			if (Input != null && (!String.IsNullOrWhiteSpace(Input.email)) && (!String.IsNullOrWhiteSpace(Input.mobileno)))
			{
				Validate validate = new Validate();
				ValidateUser validateMe = new ValidateUser();


				if (!String.IsNullOrWhiteSpace(Input.name))
					name = clsCommon.Encrypt(Input.name);

				//Confirm EmailId field
				if (validate.ValidateEmail(Input.email) == true)
					email = Input.email.ToLower();
				if (validate.ValidateEmail(Input.mobileno) == true)
					email = Input.mobileno.ToLower();

				//Confirm mobile field
				if (validate.ValidateMobile(Input.email) == true)
					mobile = Input.email.ToLower();
				if (validate.ValidateMobile(Input.mobileno) == true)
					mobile = Input.mobileno.ToLower();

				dbAccessClass dbAccessClass = new dbAccessClass();

				try
				{
					JourneyOfAccount journeyOfAccount = new JourneyOfAccount();
					GetStatus insertStatus = new GetStatus();

					//User not registered - go to registration page
					journeyOfAccount.Status = "Success";
					journeyOfAccount.UserName = "";
					journeyOfAccount.UserEmail = email;
					journeyOfAccount.UserMobile = mobile;
					journeyOfAccount.UserId = 0;

					//SessionManagement.StoreInSession(SessionType.TempUserDetails, tempUserJourneyDtls);
					SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

					if (!String.IsNullOrWhiteSpace(email))
						email = clsCommon.Encrypt(email);
					if (!String.IsNullOrWhiteSpace(mobile))
						mobile = clsCommon.Encrypt(mobile);

					//validateMe = dbAccessClass.ValidateUser(email, mobile);

					//if (validateMe != null)
					//{
					//	if (validateMe.emailExists > 0)
					//	{
					//		return Json(new { status = "EmailExt", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
					//	}
					//	else if (validateMe.mobilenoExists > 0)
					//	{
					//		return Json(new { status = "MobileExt", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
					//	}
					//	else if (validateMe.emailExists == 0 && validateMe.mobilenoExists == 0)
					//	{
					//Registration
					insertStatus = Register(Input, "Special365Plan");
					if (insertStatus != null && insertStatus.returnStatus == "Success")
					{
						journeyOfAccount.AlreadyRegCommMode = insertStatus.returnValue;
						//try
						//{
						//	//SFMC Data Entry
						//	if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y" && insertStatus.returnValue == 0)
						//	{
						//		SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
						//		sendDataToSFMC.PostDataSFMC(insertStatus.returnValue, "", "registration");
						//	}
						//}
						//catch (Exception ex)
						//{
						//	Logger.Error(reporting: typeof(HomeController), ex, message: "Special Plan registration - SFMC Issue");
						//}

						//After login
						string loginStatus = await PostRegistrationActivity("Plan365Special");
						string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

						return Json(new { status = "Success", message = "", UserUniqueCode = UserUniqueId }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { status = insertStatus.returnStatus, message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
					}
					//	}
					//}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(HomeContainer), ex, message: "SpecialPlanRegistration");
				}
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public async Task<ActionResult> SecondFormRegistration(Models.Registration Input)
		{
			string email = String.Empty;
			string name = String.Empty;
			string mobile = string.Empty;

			if (Input != null && (!String.IsNullOrWhiteSpace(Input.email)) && (!String.IsNullOrWhiteSpace(Input.mobileno)))
			{
				Validate validate = new Validate();
				ValidateUser validateMe = new ValidateUser();


				if (!String.IsNullOrWhiteSpace(Input.name))
					name = clsCommon.Encrypt(Input.name);

				//Confirm EmailId field
				if (validate.ValidateEmail(Input.email) == true)
					email = Input.email.ToLower();
				if (validate.ValidateEmail(Input.mobileno) == true)
					email = Input.mobileno.ToLower();

				//Confirm mobile field
				if (validate.ValidateMobile(Input.email) == true)
					mobile = Input.email.ToLower();
				if (validate.ValidateMobile(Input.mobileno) == true)
					mobile = Input.mobileno.ToLower();

				dbAccessClass dbAccessClass = new dbAccessClass();

				try
				{
					JourneyOfAccount journeyOfAccount = new JourneyOfAccount();
					GetStatus insertStatus = new GetStatus();

					//User not registered - go to registration page
					journeyOfAccount.Status = "Success";
					journeyOfAccount.UserName = "";
					journeyOfAccount.UserEmail = email;
					journeyOfAccount.UserMobile = mobile;
					journeyOfAccount.UserId = 0;

					//SessionManagement.StoreInSession(SessionType.TempUserDetails, tempUserJourneyDtls);
					SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

					if (!String.IsNullOrWhiteSpace(email))
						email = clsCommon.Encrypt(email);
					if (!String.IsNullOrWhiteSpace(mobile))
						mobile = clsCommon.Encrypt(mobile);

					validateMe = dbAccessClass.ValidateUser(email, mobile);

					if (validateMe != null)
					{
						if (validateMe.emailExists > 0)
						{
							return Json(new { status = "EmailExt", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
						}
						else if (validateMe.mobilenoExists > 0)
						{
							return Json(new { status = "MobileExt", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
						}
						else if (validateMe.emailExists == 0 && validateMe.mobilenoExists == 0)
						{
							//Referral Code
							if (!string.IsNullOrWhiteSpace(Input.ReferedBy))
							{
								Responce refRresponce = new Responce();
								refRresponce = CheckReferralCodeIsValid(Input.ReferedBy);
								if (refRresponce != null && refRresponce.StatusCode != 0 && refRresponce.StatusCode != HttpStatusCode.OK)
								{
									return Json(new
									{
										status = "referral",
										navigation = "referral",
										message = refRresponce.Message
									}, JsonRequestBehavior.AllowGet);
								}
							}

							//Registration
							insertStatus = Register(Input, Input.page);
							if (insertStatus != null && insertStatus.returnStatus == "Success")
							{
								journeyOfAccount.AlreadyRegCommMode = insertStatus.returnValue;

								try
								{
									//SFMC Data Entry
									if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
									{
										SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
										sendDataToSFMC.PostDataSFMC(insertStatus.returnValue, "", "registration");
									}
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(HomeController), ex, message: "registration - SFMC Issue");
								}

								try
								{
									//SFMC Data Entry
									if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
									{
										//Invite User
										SendDataToSFMC sendDataToSFMCt = new SendDataToSFMC();
										sendDataToSFMCt.PostDataSFMC(insertStatus.returnValue, "", "registrationInviteUser");
									}
								}
								catch (Exception ex)
								{
									Logger.Error(reporting: typeof(HomeController), ex, message: "registration - SFMC Issue");
								}

								//After login
								string loginStatus = await PostRegistrationActivity(Input.page);
								string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
								string redirection = SessionManagement.GetCurrentSession<string>(SessionType.SplRedirection);

								return Json(new { status = "Success", message = "", UserUniqueCode = UserUniqueId, Navigation = redirection }, JsonRequestBehavior.AllowGet);
							}
							else
							{
								return Json(new { status = insertStatus.returnStatus, message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(reporting: typeof(HomeContainer), ex, message: "SpecialPlanRegistration");
				}
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public bool Welcomemail(int UserId, string _email)
		{
			bool sendmailstatus = false;
			try
			{
				//	//free subscription mail
				dbProxy _db = new dbProxy();
				SubscriptionController subMailer = new SubscriptionController();
				if (!String.IsNullOrEmpty(_email))
				{
					SelectedAgeGroup maxagegroup = new SelectedAgeGroup();
					List<SetParameters> spp = new List<SetParameters>()
												{
													new SetParameters{ ParameterName = "@QType", Value = "9" },
													new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
												};

					maxagegroup = _db.GetData<SelectedAgeGroup>("usp_getdata", maxagegroup, spp);
					if (!String.IsNullOrEmpty(_email))
					{
						//bool sendmailstatus = subMailer.SubscriptionEmailerBonus(1, "", "", _email, "");
						sendmailstatus = subMailer.SubscriptionEmailer(1, "", "", _email, maxagegroup.AgeGroup);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeContainer), ex, message: "On Registration - SubscriptionMailer");
			}

			return sendmailstatus;
		}

		public GetStatus Register(Models.Registration registration, string datasource)
		{
			string agegroup = String.Empty;
			string RegisteredUserId = String.Empty;

			string name = String.Empty;
			string mobile = String.Empty;
			string email = String.Empty;
			string InviteUrlCode = String.Empty;
			string ReferralCode = String.Empty;

			if (!String.IsNullOrEmpty(datasource) && datasource == "webhook")
			{
				name = registration.name;
				email = registration.email;
				mobile = registration.mobileno;
			}
			else
			{
				if (!String.IsNullOrWhiteSpace(registration.name))
					name = clsCommon.Encrypt(registration.name);

				Validate validate = new Validate();
				if (validate.ValidateEmail(registration.email) == true)
					email = clsCommon.Encrypt(registration.email.ToLower());
				if (validate.ValidateEmail(registration.mobileno) == true)
					email = clsCommon.Encrypt(registration.mobileno.ToLower());

				//Confirm mobile field
				if (validate.ValidateMobile(registration.email) == true)
					mobile = clsCommon.Encrypt(registration.email.ToLower());
				if (validate.ValidateMobile(registration.mobileno) == true)
					mobile = clsCommon.Encrypt(registration.mobileno.ToLower());

				InviteUrlCode = SessionManagement.GetCurrentSession<string>(SessionType.InviteUrlCode);
			}

			GetStatus insertStatus = new GetStatus();
			dbProxy _db = new dbProxy();

			if (!String.IsNullOrWhiteSpace(registration.email))
			{
				if (registration.ageGroup != null)
				{
					for (int i = 0; i < registration.ageGroup.Length; i++)
					{
						agegroup += registration.ageGroup[i].ToString() + ",";
					}

					agegroup = agegroup.TrimEnd(',');
				}

				string UserTypeParam = String.Empty;
				//Sent parameter for hp.com users
				try
				{
					if (registration.email.Contains("@"))
						UserTypeParam = registration.email.ToString().Split('@')[1];
				}
				catch { }

				ReferralCode = clsCommon.GenerateReferralCode();

				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters { ParameterName = "@name", Value = name },
					new SetParameters { ParameterName = "@email", Value = email },
					new SetParameters { ParameterName = "@mobileno", Value = mobile },
					new SetParameters { ParameterName = "@age_group", Value = agegroup },
					//new SetParameters { ParameterName = "@password", Value = MD5HashPassword.GetMD5Hash(registration.regpassword) },
					new SetParameters { ParameterName = "@ComWithWhatsApp", Value = registration.supportOnWhatsupFromHP == null ? "" : registration.supportOnWhatsupFromHP },
					new SetParameters { ParameterName = "@ComWithPhone", Value = registration.supportOnPhoneFromHP == null ? "" : registration.supportOnPhoneFromHP },
					new SetParameters { ParameterName = "@ComWithEmail", Value = registration.supportOnEmailFromHP == null ? "" : registration.supportOnEmailFromHP },
					//new SetParameters { ParameterName = "@RerenceReferralCode", Value = Sanitizer.GetSafeHtmlFragment(registration.referralCode)},
					new SetParameters { ParameterName = "@termsChecked", Value = Sanitizer.GetSafeHtmlFragment(registration.termsChecked)},
					new SetParameters { ParameterName = "@UserTypeCheckParam", Value = UserTypeParam },
					new SetParameters { ParameterName = "@DataSource", Value = datasource },
					new SetParameters { ParameterName = "@UserRegisteredMode", Value = "otp" },
					new SetParameters { ParameterName = "@RuParentOrStudent", Value = registration.RuParentOrStudent == null ? "" : registration.RuParentOrStudent  },
					new SetParameters { ParameterName = "@InviteUrlCode", Value = InviteUrlCode == null ? "" : InviteUrlCode},
					new SetParameters { ParameterName = "@RerenceReferralCode", Value = Sanitizer.GetSafeHtmlFragment(ReferralCode)},
					new SetParameters { ParameterName = "@ReferredByCode", Value = Sanitizer.GetSafeHtmlFragment(registration.ReferedBy == null ? "" : registration.ReferedBy)}
					//new SetParameters { ParameterName = "@PlanSelection", Value = registration.BotPlanSelection.ToString() }
			};

				if (datasource == "Special365Plan")
					insertStatus = _db.StoreData("Insert_Registration_SpecialPlan", sp);
				else
					insertStatus = _db.StoreData("Insert_Registration", sp);


			}

			return insertStatus;
		}

		public async Task<string> PostRegistrationActivity(string source = "")
		{
			string PostLoginStatus = String.Empty;
			ReturnMessage returnMessage = new ReturnMessage();
			//returnMessage = TobeLogginCode("");

			JourneyOfAccount journeyOfAccount = new JourneyOfAccount();
			journeyOfAccount = SessionManagement.GetCurrentSession<JourneyOfAccount>(SessionType.JrnyUserDetails);

			TempUserJourneyDtls UserHpIdDetails = new TempUserJourneyDtls();
			UserHpIdDetails.UserEmail = journeyOfAccount.UserEmail;
			UserHpIdDetails.UserName = "";
			UserHpIdDetails.UserPassword = "";
			UserHpIdDetails.UserMobile = journeyOfAccount.UserMobile;
			UserHpIdDetails.Source = journeyOfAccount.UserRegisteredMode;
			UserHpIdDetails.AlreadyRegCommMode = journeyOfAccount.AlreadyRegCommMode;

			if (UserHpIdDetails != null)
			{
				if (!String.IsNullOrWhiteSpace(source) && source == "Plan365Special")
				{
					returnMessage = LoggedInDataSpecialPlan(UserHpIdDetails, "", "", "");
				}
				else
				{
					returnMessage = LoggedInData(UserHpIdDetails, "", source, "");
				}

				if (!String.IsNullOrWhiteSpace(IsEnableEmail) && IsEnableEmail == "Y" && (!String.IsNullOrWhiteSpace(source) && source != "Plan365Special"))
				{
					//free subscription mail
					//try
					//{
					//	int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
					//	//	//free subscription mail
					//	dbProxy _db = new dbProxy();
					//	string _email = UserHpIdDetails.UserEmail;
					//	SubscriptionController subMailer = new SubscriptionController();
					//	if (!String.IsNullOrEmpty(_email))
					//	{
					//		//SelectedAgeGroup maxagegroup = new SelectedAgeGroup();
					//		//List<SetParameters> spp = new List<SetParameters>()
					//		//					{
					//		//						new SetParameters{ ParameterName = "@QType", Value = "9" },
					//		//						new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
					//		//					};

					//		//maxagegroup = _db.GetData<SelectedAgeGroup>("usp_getdata", maxagegroup, spp);

					//		//string _emailId = clsCommon.Decrypt(_email);
					//		bool sendmailstatus = subMailer.SubscriptionEmailerBonus(1, "", "", _email, "");
					//		//	vResponse = storesData.SubscriptionMailer(myprofile, subscriptionData, GetUserAllSubscription.First().SubscriptionName);
					//	}
					//}
					//catch (Exception ex)
					//{
					//	Logger.Error(reporting: typeof(HomeContainer), ex, message: "AddPassword - SubscriptionMailer");
					//}

					try
					{
						int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
						string _email = UserHpIdDetails.UserEmail;
						Welcomemail(UserId, _email);

						//	//free subscription mail
						//	dbProxy _db = new dbProxy();

						//	string _email = UserHpIdDetails.UserEmail;
						//	SubscriptionController subMailer = new SubscriptionController();
						//	if (!String.IsNullOrEmpty(_email))
						//	{
						//		SelectedAgeGroup maxagegroup = new SelectedAgeGroup();
						//		List<SetParameters> spp = new List<SetParameters>()
						//							{
						//								new SetParameters{ ParameterName = "@QType", Value = "9" },
						//								new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
						//							};

						//		maxagegroup = _db.GetData<SelectedAgeGroup>("usp_getdata", maxagegroup, spp);
						//		if (!String.IsNullOrEmpty(_email))
						//		{
						//			//bool sendmailstatus = subMailer.SubscriptionEmailerBonus(1, "", "", _email, "");
						//			bool sendmailstatus = subMailer.SubscriptionEmailer(1, "", "", _email, maxagegroup.AgeGroup);
						//		}
						//	}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(HomeContainer), ex, message: "On Registration - SubscriptionMailer");
					}
				}

				//WhatsApp Welcome Message
				if (!String.IsNullOrWhiteSpace(IsEnableWhatsApp) && IsEnableWhatsApp == "Y" && (!String.IsNullOrWhiteSpace(source) && source != "Plan365Special"))
				{
					try
					{
						LoggedIn loggedIn = new LoggedIn();
						loggedIn = SessionManagement.GetCurrentSession<LoggedIn>(SessionType.LoggedInDtls);

						string mobile = UserHpIdDetails.UserMobile;
						if (!String.IsNullOrWhiteSpace(mobile) && loggedIn != null && loggedIn.ComWithWhatsApp.ToLower().Equals("yes"))
						{
							//mobile = "91" + mobile;
							if (!String.IsNullOrWhiteSpace(mobile) && (mobile.Substring(0, 3) == "+91" || mobile.Length == 10))
								mobile = "91" + mobile;

							string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
							WhatsAppDynamicValue lst = new WhatsAppDynamicValue();
							WhatsAppHelper whatsAppHelper = new WhatsAppHelper();

							//if (!String.IsNullOrWhiteSpace(domain))
							//	lst.Domain = domain;

							dbNotificationAccess dbClass = new dbNotificationAccess();
							WhatsAppWelcomeTimeZone timeZone = dbClass.GetNotificationWelcomeTimeZone();

							if (timeZone != null && !String.IsNullOrWhiteSpace(timeZone.TimeFormat))
							{
								var notificationItem = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
									.Where(x => x.ContentType.Alias == "notificationTemplate2")?
										.OfType<NotificationTemplate2>()?.Where(x => x.IsActive == true && x?.TypesOfNotification?.ToLower() == "welcome")?.FirstOrDefault();

								if (notificationItem != null)
								{
									string notificationCode = notificationItem?.NotificationMapping?.Where(c => c?.NotificationPeriod == timeZone.TimeFormat)?.FirstOrDefault()?.NotificationCode;

									if (!String.IsNullOrWhiteSpace(notificationCode))
									{
										var worksheet = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
										.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
										.Where(c => Umbraco?.Content(c?.AgeGroup?.Udi).DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == timeZone.MinAgeGroup)?.FirstOrDefault()?
										.DescendantsOrSelf()?.Where(b => b.ContentType.Alias == "worksheetRoot")?.OfType<WorksheetRoot>()?
										.Where(v => Umbraco?.Content(v?.SelectWeek?.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == timeZone.WeekId.ToString())?
										.FirstOrDefault();

										if (worksheet != null)
										{
											string whatsappBanner = String.Empty;
											if (String.IsNullOrWhiteSpace(worksheet?.WhatsAppBanner?.Url()))
											{
												whatsappBanner = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
																.Where(x => x.ContentType.Alias == "worksheetNode")?.OfType<WorksheetNode>()?.FirstOrDefault()?.WhatsAppBannerDefault?.Url();
											}
											else
											{
												whatsappBanner = worksheet?.WhatsAppBanner?.Url();
											}

											if (loggedIn != null)
											{
												if (!String.IsNullOrWhiteSpace(loggedIn.u_name))
													lst.Name = clsCommon.Decrypt(loggedIn.u_name);
											}

											if (!String.IsNullOrWhiteSpace(worksheet?.SelectSubject?.Name))
												lst.Subjects = worksheet?.SelectSubject?.Name;

											string downloadUrl = " " + domain + "worksheet-download?d=" + clsCommon.Encryptwithbase64Code("WID=" + worksheet?.Id.ToString() + "&UID=" + loggedIn.UserId.ToString()) + "&jumpid=mb_in_oc_mk_ot_cm017589_aw_cr&utm_source=mobile_apps";
											if (!String.IsNullOrWhiteSpace(whatsappBanner) && whatsappBanner.Contains("https://"))
												lst.BannerUrl = whatsappBanner;
											else
												lst.BannerUrl = domain + whatsappBanner;

											lst.PdfUrl = downloadUrl;

											if (!String.IsNullOrWhiteSpace(lst.PdfUrl) && !String.IsNullOrWhiteSpace(lst.Subjects) && !String.IsNullOrWhiteSpace(lst.BannerUrl) && !String.IsNullOrWhiteSpace(mobile) && !String.IsNullOrWhiteSpace(notificationCode))
											{
												IRestResponse response = whatsAppHelper.CreateMessage(mobile, notificationCode, lst);

												if (response != null)
												{
													dbClass = new dbNotificationAccess();
													List<NotificationLog> notificationLog = new List<NotificationLog>();

													notificationLog.Add(new NotificationLog { UserId = loggedIn.UserId, WeekId = timeZone.WeekId, NotificationName = notificationCode, NotificationStatus = 1, NotificationJson = response.Content, SendStatus = response.StatusCode.ToString(), MobileNo = mobile, AgeGroup = worksheet?.AgeTitle?.Name, Subject = worksheet?.SelectSubject?.Name, NotificationType = "", BannerUrl = "", PdfUrl = downloadUrl, TypeOfNotif = "", UserUniqueId = loggedIn.UserUniqueId, WorksheetId = worksheet.Id, TopicName = worksheet?.Topic?.Name });
													await dbClass.NotificationLog(notificationLog);
												}
											}
											else
											{
												Logger.Info(reporting: typeof(HomeContainer), "WhatsApp Registration Message - parameter missing");
											}
										}
										else
										{
											Logger.Info(reporting: typeof(HomeContainer), "WhatsApp Registration Message - worksheet missing");
										}
									}
									else
									{
										Logger.Info(reporting: typeof(HomeContainer), "WhatsApp Registration Message - notificationcode missing");
									}
								}
								else
								{
									Logger.Info(reporting: typeof(HomeContainer), "WhatsApp Registration Message - notification item");
								}
							}
							else
							{
								Logger.Info(reporting: typeof(HomeContainer), "WhatsApp Registration Message - timezone issue");
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(HomeContainer), ex, message: "WhatsApp Registration Message");
					}
				}
			}

			//Subscription redirection
			//SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
			//subscriptionDetails = SessionManagement.GetCurrentSession<SubscriptionDetails>(SessionType.SubscriptionDtls);
			//if (subscriptionDetails != null)
			//{
			//	//string navigationUrl = String.Empty;
			//	var subscriptionRank = Umbraco?.Content(subscriptionDetails.subscriptionId).DescendantsOrSelf().OfType<Subscriptions>().FirstOrDefault();
			//	if (subscriptionRank != null)
			//	{
			//		if (subscriptionRank.Ranking == "1" || !String.IsNullOrEmpty(subscriptionDetails.WorksheetId))
			//		{
			//			return Json(new { status = "Success", navigation = "/", EnableTrackerCode = IsEnableTrackerCode, message = "" }, JsonRequestBehavior.AllowGet);
			//		}
			//		else
			//		{
			//			string navigationUrl = subscriptionDetails.targetUrl + "?subscriptionid=" + clsCommon.Encrypt(subscriptionDetails.subscriptionId);

			//			if (!String.IsNullOrEmpty(subscriptionDetails.ageGroup))
			//				navigationUrl = navigationUrl + "&age=" + subscriptionDetails.ageGroup;

			//			return Json(new { status = "Success", navigation = navigationUrl, EnableTrackerCode = IsEnableTrackerCode, message = "" }, JsonRequestBehavior.AllowGet);
			//		}
			//	}
			//}
			//else
			//{
			//	string RegisteredUserId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
			//	return Json(new { status = insertStatus.returnStatus, UniqueUserId = RegisteredUserId, EnableTrackerCode = IsEnableTrackerCode, message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
			//}

			return PostLoginStatus = returnMessage.status;
		}


		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> ForgotPasswordSendOtp(ForgotPasswordParam Input)
		//{
		//	try
		//	{
		//		if (!String.IsNullOrEmpty(Input.UserName))
		//		{
		//			string culture = String.Empty;
		//			string email = String.Empty;
		//			string mobile = String.Empty;
		//			culture = CultureName.GetCultureName();

		//			Validate validate = new Validate();
		//			if (validate.ValidateEmail(Input.UserName) == true)
		//				email = Input.UserName.ToLower();
		//			else if (validate.ValidateMobile(Input.UserName) == true)
		//				mobile = Input.UserName.ToLower();

		//			JourneyOfAccount journeyOfAccount = new JourneyOfAccount();
		//			journeyOfAccount.Page = "forgot";
		//			journeyOfAccount.UserEmail = email;
		//			journeyOfAccount.UserMobile = mobile;
		//			SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

		//			//dbProxy _db = new dbProxy();
		//			List<string> CC = new List<string>();
		//			List<string> BCC = new List<string>();

		//			string OtpStatus = await OtpManagementData(Input.UserName, "forgot", "send");

		//			if (!String.IsNullOrWhiteSpace(OtpStatus) && (OtpStatus.ToLower() == "ok" || OtpStatus.ToLower() == "done"))
		//			{
		//				return Json(new { status = "Success", navigation = culture + "/my-account/user-authentication", message = "" }, JsonRequestBehavior.AllowGet);
		//			}
		//			else if (!String.IsNullOrWhiteSpace(OtpStatus) && OtpStatus == "Exceed")
		//			{
		//				return Json(new { status = "Exceed", navigation = "", message = "", mobmasking = "", mailmasking = "" }, JsonRequestBehavior.AllowGet);
		//			}
		//			//if (response.ResponseText == "Set" && response.UserId > 0)
		//			//{
		//			//	string siteURL = ConfigurationManager.AppSettings["SiteUrl"].ToString();

		//			//	string OneTimeSetPasswordLink = siteURL + "/my-account/set-password?id=" + clsCommon.encrypto(response.UserId.ToString());

		//			//	return Json(new { status = response.ResponseText, navigation = OneTimeSetPasswordLink, message = response.ResponseMessage }, JsonRequestBehavior.AllowGet);
		//			//}
		//			else { return Json(new { status = "Fail", message = "" }, JsonRequestBehavior.AllowGet); }
		//		}
		//		else
		//		{ return Json(new { status = "Fail", message = "" }, JsonRequestBehavior.AllowGet); }
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(HomeController), ex, message: "SendOtp - Main Block");
		//	}

		//	return Json(new { status = "Fail", message = "" }, JsonRequestBehavior.AllowGet);
		//}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResendOtp()
		{
			try
			{
				string UserName = String.Empty;
				string page = String.Empty;
				string otptype = String.Empty;

				//TempUserJourneyDtls tempUserJourneyDtls = new TempUserJourneyDtls();
				//tempUserJourneyDtls = SessionManagement.GetCurrentSession<TempUserJourneyDtls>(SessionType.TempUserDetails);

				JourneyOfAccount journeyOfAccount = new JourneyOfAccount();
				journeyOfAccount = SessionManagement.GetCurrentSession<JourneyOfAccount>(SessionType.JrnyUserDetails);

				if (journeyOfAccount != null)
				{
					if (!String.IsNullOrWhiteSpace(journeyOfAccount.UserEmail))
						UserName = journeyOfAccount.UserEmail;
					else if (!String.IsNullOrWhiteSpace(journeyOfAccount.UserMobile))
						UserName = journeyOfAccount.UserMobile;

					page = journeyOfAccount.Page == null ? "forgot" : journeyOfAccount.Page;
					otptype = "resend";

					string OtpStatus = await OtpManagementData(UserName, page, otptype);

					if (!String.IsNullOrWhiteSpace(OtpStatus) && (OtpStatus == "OK" || OtpStatus == "Done"))
						return Json(new { status = "Success", page = page, message = "Otp has been sent." }, JsonRequestBehavior.AllowGet);

					if (!String.IsNullOrWhiteSpace(OtpStatus) && OtpStatus == "Exceed")
						return Json(new { status = "Exceed", page = page, message = "Otp not sent." }, JsonRequestBehavior.AllowGet);
					else
					{
						return Json(new { status = OtpStatus, page = page, message = "" }, JsonRequestBehavior.AllowGet);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "ResendOtp - Main Block");
			}

			return Json(new { status = "Fail", message = "" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult OtpVerification(OtpVerification Input)
		{
			string UserName = String.Empty;
			GetStatus response = new GetStatus();
			string culture = CultureName.GetCultureName();
			//TempUserJourneyDtls tempUserJourneyDtls = new TempUserJourneyDtls();
			JourneyOfAccount journeyOfAccount = new JourneyOfAccount();

			try
			{
				//tempUserJourneyDtls = SessionManagement.GetCurrentSession<TempUserJourneyDtls>(SessionType.TempUserDetails);
				journeyOfAccount = SessionManagement.GetCurrentSession<JourneyOfAccount>(SessionType.JrnyUserDetails);
				if (!String.IsNullOrWhiteSpace(Input.OneTimePwd) && journeyOfAccount != null)
				{
					if (journeyOfAccount.Page == "Plan365AddAltntMob")
					{
						UserName = clsCommon.Encrypt(journeyOfAccount.AlternateUserMobile);
					}
					else
					{
						if (!String.IsNullOrWhiteSpace(journeyOfAccount.UserEmail))
							UserName = clsCommon.Encrypt(journeyOfAccount.UserEmail);
						else if (!String.IsNullOrWhiteSpace(journeyOfAccount.UserMobile))
							UserName = clsCommon.Encrypt(journeyOfAccount.UserMobile);
					}

					string mode = journeyOfAccount.Page;

					response = OtpVerify(mode, UserName, Input.OneTimePwd);

					if (response != null)
					{
						if (response != null && response?.returnStatus == "SUCCESS" && mode == "login")// && response.returnValue == 2
						{
							journeyOfAccount.IsotpVerified = 1;
							journeyOfAccount.Status = response?.returnStatus;

							//login here
							TempUserJourneyDtls UserHpIdDetails = new TempUserJourneyDtls();
							UserHpIdDetails.UserEmail = journeyOfAccount.UserEmail;
							UserHpIdDetails.UserName = "";
							UserHpIdDetails.UserPassword = "";
							UserHpIdDetails.UserMobile = journeyOfAccount.UserMobile;
							UserHpIdDetails.Source = "otp";
							UserHpIdDetails.Rememberme = Input.RememberMe;

							ReturnMessage returnMessage = new ReturnMessage();
							if (!String.IsNullOrWhiteSpace(mode) && mode == "Plan365Login")
								returnMessage = LoggedInDataSpecialPlan(UserHpIdDetails, "", "", "", "login");
							else
								returnMessage = LoggedInData(UserHpIdDetails, "", UserHpIdDetails.Source, "", "login");

							if (returnMessage != null && !String.IsNullOrWhiteSpace(returnMessage.navigation))
							{
								journeyOfAccount.Navigation = returnMessage.navigation;
							}
							else
							{
								journeyOfAccount.Navigation = "/";
							}
							//return Json(new { status = "Success", navigation = culture + journeyOfAccount.Navigation, message = "" }, JsonRequestBehavior.AllowGet);
							//return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);

							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
						else if (response != null && response?.returnStatus == "SUCCESS" && (mode == "Plan365Login" || mode == "Bonus"))//  && (!String.IsNullOrWhiteSpace(journeyOfAccount.LoginType)) && journeyOfAccount.LoginType == "registered")
						{
							journeyOfAccount.IsotpVerified = 1;
							journeyOfAccount.Status = response?.returnStatus;

							//login here
							TempUserJourneyDtls UserHpIdDetails = new TempUserJourneyDtls();
							UserHpIdDetails.UserEmail = journeyOfAccount.UserEmail;
							UserHpIdDetails.UserName = "";
							UserHpIdDetails.UserPassword = "";
							UserHpIdDetails.UserMobile = journeyOfAccount.UserMobile;
							UserHpIdDetails.Source = "otp";
							UserHpIdDetails.Rememberme = Input.RememberMe;

							ReturnMessage returnMessage = new ReturnMessage();
							if (!String.IsNullOrWhiteSpace(mode) && mode == "Bonus")
							{
								UserHpIdDetails.AlreadyRegCommMode = -1;
								returnMessage = LoggedInData(UserHpIdDetails, "", UserHpIdDetails.Source, "", "login");

								if (returnMessage != null && returnMessage.status != null && returnMessage.status == "Success")
								{
									journeyOfAccount.Navigation = returnMessage.navigation;
								}
							}
							else
							{
								returnMessage = LoggedInDataSpecialPlan(UserHpIdDetails, "", "", "", "login");
							}

							if (returnMessage != null && returnMessage.status != "Fail" && mode != "Bonus")
							{
								journeyOfAccount.Navigation = returnMessage.navigation;

								//CheckUserSpecialPlanRedeemed
								dbAccessClass db = new dbAccessClass();
								SpecialRedirectionCheck specialRedirectionCheck = new SpecialRedirectionCheck();
								specialRedirectionCheck = db.CheckUserSpecialPlanRedeemed();

								journeyOfAccount.UserId = specialRedirectionCheck.UserId;
							}
							//return Json(new { status = "Success", navigation = culture + journeyOfAccount.Navigation, message = "" }, JsonRequestBehavior.AllowGet);
							//return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);

							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
						else if (response != null && response?.returnStatus == "SUCCESS" && mode == "registration")// && response.returnValue == 2
						{
							journeyOfAccount.IsotpVerified = 1;
							journeyOfAccount.Status = response?.returnStatus;
							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
						else if (response != null && response?.returnStatus == "SUCCESS" && mode == "forgot")// && response.returnValue == 2
						{
							journeyOfAccount.IsotpVerified = 1;
							journeyOfAccount.Status = response?.returnStatus;
							journeyOfAccount.Navigation = "/my-account/set-password";

							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
						else if (response != null && response?.returnStatus == "SUCCESS" && mode == "auth")// && response.returnValue == 2
						{
							journeyOfAccount.IsotpVerified = 1;
							journeyOfAccount.Status = response?.returnStatus;
							journeyOfAccount.Navigation = "/my-account/set-password";

							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
						else if (response != null && response?.returnStatus == "OTP_NM")
						{
							journeyOfAccount.Status = response?.returnStatus;
							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							//return Json(new { status = "OTP_NM", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
						else if (response != null && !String.IsNullOrWhiteSpace(response.returnStatus) && (response.returnStatus == "MAXFATT" || response.returnStatus == "MAXRATT"))
						{
							journeyOfAccount.Status = response?.returnStatus;
							string strTimeLeft = String.Empty;
							string remainingTime = SessionManagement.GetCurrentSession<string>(SessionType.InvalidOtpFailedTime);
							if (!String.IsNullOrWhiteSpace(remainingTime) && int.Parse(remainingTime) > 0)
							{
								TimeSpan rmtime = TimeSpan.FromSeconds(Convert.ToInt32(remainingTime));
								strTimeLeft = rmtime.ToString(@"mm\:ss");
							}

							journeyOfAccount.ValidateMessage = strTimeLeft;
							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							//return Json(new { status = response.returnStatus, navigation = "", message = strTimeLeft, mobmasking = "", mailmasking = "" }, JsonRequestBehavior.AllowGet);
							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
						else if (response != null && response?.returnStatus == "SUCCESS" && mode == "Plan365AddAltntMob")
						{
							journeyOfAccount.Status = response?.returnStatus;
							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							//return Json(new { status = response.returnStatus, navigation = "", message = "", mobmasking = "", mailmasking = "" }, JsonRequestBehavior.AllowGet);
							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
						else
						{
							journeyOfAccount.Status = response?.returnStatus;
							journeyOfAccount.UserUniqueCode = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

							SessionManagement.StoreInSession(SessionType.JrnyUserDetails, journeyOfAccount);

							//return Json(new { status = response.returnStatus, navigation = "", message = "", mobmasking = "", mailmasking = "" }, JsonRequestBehavior.AllowGet);
							return Json(journeyOfAccount, JsonRequestBehavior.AllowGet);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "OtpVerification - Main block");
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		public GetStatus OtpVerify(string mode, string UserName, string Otp)
		{
			GetStatus response = new GetStatus();
			string hostEnv = ConfigurationManager.AppSettings["WebHostEnvironment"].ToString();
			if (!String.IsNullOrWhiteSpace(UserName) && !String.IsNullOrWhiteSpace(Otp))
			{
				int otpId = SessionManagement.GetCurrentSession<int>(SessionType.OtpId);
				dbProxy _db = new dbProxy();

				Validate validate = new Validate();
				string otpSource = String.Empty;
				string _username = String.Empty;

				if (!String.IsNullOrWhiteSpace(UserName))
					_username = clsCommon.Decrypt(UserName);

				if (!String.IsNullOrWhiteSpace(_username) && validate.ValidateEmail(_username) == true)
					otpSource = "email";
				else if (!String.IsNullOrWhiteSpace(_username) && validate.ValidateMobile(_username) == true)
					otpSource = "mobile";

				//for staging and local
				if (!String.IsNullOrWhiteSpace(hostEnv) && hostEnv.ToLower() == "staging" && Otp == "1111")
				{
					response.returnStatus = "SUCCESS";
					response.returnValue = 2;
					response.returnMessage = "";
				}
				else if (!String.IsNullOrWhiteSpace(hostEnv) && hostEnv.ToLower() == "staging" && Otp != "1111")
				{
					response.returnStatus = "OTP_NM";
					response.returnValue = 1;
					response.returnMessage = "";
				}
				else
				{
					if (otpId > 0)
					{
						List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters{ ParameterName = "@page", Value = mode  == null ? "" : mode},
							new SetParameters{ ParameterName = "@UserName", Value = UserName },
							new SetParameters{ ParameterName = "@otp", Value = Otp },
							new SetParameters{ ParameterName = "@otpSource", Value = otpSource == null ? "" : otpSource},
							new SetParameters{ ParameterName = "@RawId", Value = otpId.ToString() }
						};

						response = _db.GetData<GetStatus>("USP_OtpVerification", response, sp);
					}
				}
				//if (response != null && response.returnStatus != null && response.returnStatus == "SUCCESS")
				//	SessionManagement.DeleteFromSession(SessionType.OtpId);
			}

			return response;
		}

		public async Task<string> OtpManagementData(string UserName, string page, string otptype)
		{
			string OtpStatus = "OK";
			//Send Otp on Mobile and Email and redirect on otp page
			MyProfile_Temp myprofile = new MyProfile_Temp();
			List<string> CC = new List<string>();
			List<string> BCC = new List<string>();

			var mailContent = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?
				.DescendantsOrSelf()?.Where(x => x.ContentType.Alias == "sMSOTPContentRoot")?.FirstOrDefault()?
				.Children?.OfType<MailContent>()?.Where(c => c.Type == page)?.FirstOrDefault();

			if (mailContent != null)
			{
				if (mailContent.EmailCC != null && mailContent.EmailCC.Count() > 0)
				{
					foreach (var item in mailContent.EmailCC)
					{
						CC.Add(item);
					}
				}

				if (mailContent.EmailBcc != null && mailContent.EmailBcc.Count() > 0)
				{
					foreach (var item in mailContent.EmailBcc)
					{
						BCC.Add(item);
					}
				}
			}

			if (!String.IsNullOrEmpty(UserName))
			{
				dbAccessClass dbAccessClass = new dbAccessClass();
				TempUserJourneyDtls tempUserJourneyDtls = new TempUserJourneyDtls();

				Validate validate = new Validate();
				if (validate.ValidateEmail(UserName) == true)
					UserName = UserName.ToLower();
				if (validate.ValidateMobile(UserName) == true)
					UserName = UserName.ToLower();

				string _UserName = clsCommon.Encrypt(UserName);
				myprofile = dbAccessClass.GetProfileByEmailMobile(_UserName);
				string email = String.Empty;
				string mobile = String.Empty;

				if (myprofile != null)
				{
					tempUserJourneyDtls.UserId = myprofile.UserId;
					if (!String.IsNullOrWhiteSpace(myprofile.Name))
						tempUserJourneyDtls.UserName = clsCommon.Decrypt(myprofile.Name);
					else
						tempUserJourneyDtls.UserName = "";
					if (!String.IsNullOrWhiteSpace(myprofile.Mobileno))
						tempUserJourneyDtls.UserMobile = clsCommon.Decrypt(myprofile.Mobileno);
					else
						tempUserJourneyDtls.UserMobile = "";
					if (!String.IsNullOrWhiteSpace(myprofile.Email))
						tempUserJourneyDtls.UserEmail = clsCommon.Decrypt(myprofile.Email);
					else
						tempUserJourneyDtls.UserEmail = "";

					tempUserJourneyDtls.UserPassword = "";
					tempUserJourneyDtls.StepsCompletted = myprofile.StepsCompletted;
					tempUserJourneyDtls.Source = page;

					SessionManagement.StoreInSession(SessionType.TempUserDetails, tempUserJourneyDtls);

					if (!String.IsNullOrWhiteSpace(myprofile.Email))
						email = clsCommon.Decrypt(myprofile.Email);
					if (!String.IsNullOrWhiteSpace(myprofile.Mobileno))
						mobile = clsCommon.Decrypt(myprofile.Mobileno);

					OtpStatus = await OtpManagement(mobile, email, "", null, "", "", "", page, otptype, mailContent?.Subject == null ? "HP Print Learn Center" : mailContent?.Subject, CC, BCC);
				}
				else
				{
					OtpStatus = "Fail";
				}
			}

			return OtpStatus;
		}
		public async Task<string> OtpManagement(string mobileno, string regemail, string name, string[] ageGroup, string supportOnEmailFromHP, string supportOnWhatsupFromHP, string referralCode, string page, string otptype, string subject, IEnumerable<string> CC, IEnumerable<string> BCC)
		{
			dbProxy _db = new dbProxy();
			GetStatus response = new GetStatus();
			string mobile = String.Empty;
			string email = String.Empty;
			string uname = String.Empty;
			string otpSource = String.Empty;

			if (!String.IsNullOrWhiteSpace(mobileno))
			{ mobile = clsCommon.Encrypt(mobileno); otpSource = "mobile"; }
			if (!String.IsNullOrWhiteSpace(regemail))
			{ email = clsCommon.Encrypt(regemail.ToLower()); otpSource = "email"; }
			if (!String.IsNullOrWhiteSpace(name))
				uname = clsCommon.Encrypt(name);

			SMS_Management otpg = new SMS_Management();
			string otp = otpg.Generate4Otp();

			if (!String.IsNullOrWhiteSpace(otp))
			{
				string agegroup = String.Empty;
				if (ageGroup != null)
				{
					for (int i = 0; i < ageGroup.Length; i++)
					{
						agegroup += ageGroup[i].ToString() + ",";
					}

					agegroup = agegroup.TrimEnd(',');
				}

				List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters{ ParameterName = "@page", Value = page },
							new SetParameters{ ParameterName = "@type", Value = otptype },
							new SetParameters{ ParameterName = "@mobile", Value = mobile },
							new SetParameters{ ParameterName = "@email", Value = email },
							new SetParameters{ ParameterName = "@name", Value = uname },
							new SetParameters{ ParameterName = "@otp", Value = otp },
							new SetParameters{ ParameterName = "@otpSource", Value = otpSource },
							new SetParameters{ ParameterName = "@ageGroup", Value = agegroup },
							new SetParameters{ ParameterName = "@supportOnEmailFromHP", Value = supportOnEmailFromHP == null ? "" : supportOnEmailFromHP},
							new SetParameters{ ParameterName = "@supportOnWhatsupFromHP", Value = supportOnWhatsupFromHP == null ? "" : supportOnWhatsupFromHP},
							new SetParameters{ ParameterName = "@referralCode", Value = referralCode == null ? "" : referralCode}
						};

				response = _db.GetData<GetStatus>("USP_OtpEntry", response, sp);
				if (response != null && response?.returnStatus == "Ok" && !String.IsNullOrWhiteSpace(response.returnMessage) && !String.IsNullOrWhiteSpace(response.returnValue.ToString()))
				{
					SessionManagement.StoreInSession(SessionType.OtpId, response.returnValue);
					Validate validate = new Validate();
					try
					{
						if (!String.IsNullOrWhiteSpace(regemail) && validate.ValidateEmail(regemail) == true)
						{
							//email send
							SenderMailer mailsend = new SenderMailer();
							await mailsend.SendOTPEmailerContent(page, subject, regemail, CC, BCC, "", response.returnMessage);
						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(HomeController), ex, message: "SendOtp - Email block");
					}

					try
					{
						//mobile send
						if (!String.IsNullOrWhiteSpace(mobileno) && validate.ValidateMobile(mobileno) == true)
						{
							if (!String.IsNullOrWhiteSpace(mobileno) && mobileno.Substring(0, 3) == "+91")
								mobileno = mobileno.Replace("+91", "");

							string ms = string.Empty;
							SMS_Management smsManagement = new SMS_Management();
							ms = await smsManagement.SendSMSForVerification(mobileno, response.returnMessage);

							if (String.IsNullOrWhiteSpace(ms))
								ms = "SMS not working";

							Logger.Info(reporting: typeof(HomeController), ms);
						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(HomeController), ex, message: "SendOtp - Mobile block");
					}
				}
				else if (response != null && response?.returnStatus == "NOTOk")
				{
					return "Exceed";
				}
				else if (response != null && (response?.returnStatus == "MAXFATT" || response?.returnStatus == "MAXRATT"))
				{
					SessionManagement.StoreInSession(SessionType.InvalidOtpFailedTime, response.returnValue.ToString());
					return response.returnStatus;
				}
				else
				{
					return response.returnStatus;
				}
			}

			return "Done";
		}

		[HttpPost]
		public ActionResult LoginRedirection()
		{
			string siteUrl = ConfigurationManager.AppSettings["SiteUrl"].ToString();
			try
			{
				//Store Culture Name
				string culture = String.Empty;
				culture = CultureName.GetCultureName();
				if (String.IsNullOrWhiteSpace(culture))
				{
					CultureManagePostHPID cultureManagePostHPID = new CultureManagePostHPID();
					string LogoutCulture = cultureManagePostHPID.CultureStorePostHpId();
					//System.Web.HttpCookie LogoutCulture = Request?.Cookies["PreUrlRedirectionCulture"];
					if (!String.IsNullOrWhiteSpace(LogoutCulture))
					{
						siteUrl = siteUrl + LogoutCulture + "/my-account/login";

						return Json(new { status = "Success", navigation = siteUrl }, JsonRequestBehavior.AllowGet);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "Login - HPId Url Manage Block");
			}

			return Json(new { status = "Fail", navigation = siteUrl, message = "" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddPassword(UserSetPassword Input)
		{
			try
			{
				TempUserJourneyDtls tempUserJourneyDtls = new TempUserJourneyDtls();
				tempUserJourneyDtls = SessionManagement.GetCurrentSession<TempUserJourneyDtls>(SessionType.TempUserDetails);

				if (tempUserJourneyDtls != null)
				{
					if (!String.IsNullOrEmpty(Input.regpassword) && !String.IsNullOrEmpty(Input.regpasswordconfirm))
					{
						if (Input.regpassword == Input.regpasswordconfirm)
						{
							string username = String.Empty;
							dbProxy _db = new dbProxy();
							GetStatus insertStatus = new GetStatus();

							if (!String.IsNullOrWhiteSpace(tempUserJourneyDtls.UserEmail))
								username = clsCommon.Encrypt(tempUserJourneyDtls.UserEmail);
							else if (!String.IsNullOrWhiteSpace(tempUserJourneyDtls.UserMobile))
								username = clsCommon.Encrypt(tempUserJourneyDtls.UserMobile);


							if (!String.IsNullOrWhiteSpace(username))
							{
								List<SetParameters> sp = new List<SetParameters>()
								{
									new SetParameters{ ParameterName = "@UserName", Value =username },
									new SetParameters{ ParameterName = "@Password", Value = MD5HashPassword.GetMD5Hash(Input.regpassword) },
								};

								insertStatus = _db.StoreData("Update_Registration_Password", sp);
								if (insertStatus.returnStatus == "Success")
								{

									TempUserJourneyDtls UserHpIdDetails = new TempUserJourneyDtls();
									UserHpIdDetails = SessionManagement.GetCurrentSession<TempUserJourneyDtls>(SessionType.TempUserDetails);

									if (UserHpIdDetails != null)
									{
										UserHpIdDetails.UserPassword = MD5HashPassword.GetMD5Hash(Input.regpassword);
									}
									ReturnMessage returnMessage = new ReturnMessage();
									returnMessage = LoggedInData(tempUserJourneyDtls, "redirectonsubscription");

									string UniqueUserId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

									return Json(new { status = returnMessage.status, UniqueUserId = UniqueUserId, navigation = returnMessage.navigation, message = returnMessage.message }, JsonRequestBehavior.AllowGet);
								}
							}
						}
					}
					else
					{
						return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "Add Password - Main Block");
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ChangePassword(ChangePasswordParam Input)
		{
			try
			{
				if (!String.IsNullOrEmpty(Input.CurrentPassword) && !String.IsNullOrEmpty(Input.NewPassword) && !String.IsNullOrEmpty(Input.ConfirmNewPassword))
				{
					if (Input.NewPassword == Input.ConfirmNewPassword)
					{
						dbProxy _db = new dbProxy();
						GetStatus insertStatus = new GetStatus();

						int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
						if (UserId > 0)
						{
							List<SetParameters> sp = new List<SetParameters>()
								{
									new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
									new SetParameters{ ParameterName = "@CurrentPassword", Value = MD5HashPassword.GetMD5Hash(Input.CurrentPassword) },
									new SetParameters{ ParameterName = "@Password", Value = MD5HashPassword.GetMD5Hash(Input.NewPassword) },
								};

							insertStatus = _db.StoreData("ChangePassword", sp);
							if (insertStatus.returnStatus == "Success")
								return Json(new { status = insertStatus.returnStatus, message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
							else
								return Json(new { status = "Fail", message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
						}
					}
				}
				else
				{
					return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "Change Password - Main Block");
			}

			return Json(new { status = "Error", message = "" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateProfile(Models.Registration registration)
		{
			try
			{
				Validate validate = new Validate();
				//if (registration.ageGroup == null || registration.ageGroup.Length == 0)
				//{
				//	return Json(new { status = "vldtnage" }, JsonRequestBehavior.AllowGet);
				//}
				//else if (!String.IsNullOrWhiteSpace(registration.mobileno) || validate.ValidateMobile(registration.mobileno) == false)
				//{
				//	return Json(new { status = "vldtnmob" }, JsonRequestBehavior.AllowGet);
				//}
				//else
				//{
				GetStatus insertStatus = new GetStatus();
				insertStatus = UpdateYourProfile(registration);
				if (insertStatus != null && insertStatus.returnStatus == "Success")
				{
					try
					{
						//SFMC Data Entry
						if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage))
						{
							int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
							try
							{
								if (UserId > 0 && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
								{
									SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
									sendDataToSFMC.PostDataSFMC(UserId, "", "updateprofile");
								}
								else
								{
									Logger.Info(reporting: typeof(HomeController), message: "SFMC Issue - UserId is null");
								}
							}
							catch (Exception ex)
							{
								Logger.Error(reporting: typeof(HomeContainer), ex, message: "WorksheetPlan - Send Data to SFMC Lesson");
							}

							//try
							//{

							//	if (!String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
							//	{
							//		SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
							//		sendDataToSFMC.PostDataSFMCBonus(UserId, "", "subscriptionbonus");
							//	}
							//}
							//catch (Exception ex)
							//{
							//	Logger.Error(reporting: typeof(HomeContainer), ex, message: "WorksheetPlan - Send Data to SFMC Bonus");
							//}
						}
					}
					catch (Exception ex)
					{
						Logger.Error(reporting: typeof(HomeController), ex, message: "Update profile - SFMC Issue");
					}

					return Json(new { status = "Success", navigation = "", message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = insertStatus.returnStatus, message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
				}
				//}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "UpdateProfile - Main Block");
			}

			return Json(new { status = "Error", navigation = "", message = "Profile has been not updated." }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateProfile_SpecialPlan(Models.Registration registration)
		{
			try
			{
				Validate validate = new Validate();
				if (!String.IsNullOrWhiteSpace(registration.alternatemobileno) && validate.ValidateMobile(registration.alternatemobileno) == false)
				{
					return Json(new { status = "altmexts" }, JsonRequestBehavior.AllowGet);
				}

				GetStatus insertStatus = new GetStatus();
				insertStatus = UpdateYourProfile_SpecialPlan(registration);
				if (insertStatus != null && insertStatus.returnStatus == "Success")
				{
					return Json(new { status = "Success", navigation = "", message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = insertStatus.returnStatus, message = insertStatus.returnMessage }, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "UpdateProfile - Main Block");
			}

			return Json(new { status = "Error", navigation = "", message = "Profile has been not updated." }, JsonRequestBehavior.AllowGet);
		}
		public GetStatus UpdateYourProfile(Models.Registration registration)
		{
			dbProxy _db = new dbProxy();
			GetStatus insertStatus = new GetStatus();

			try
			{
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
				if (!String.IsNullOrEmpty(UserId.ToString()))
				{
					string name = String.Empty;
					string mobile = String.Empty;
					string agegroup = String.Empty;

					if (!String.IsNullOrEmpty(registration.name))
						name = clsCommon.Encrypt(registration.name);
					if (!String.IsNullOrEmpty(registration.mobileno))
						mobile = clsCommon.Encrypt(registration.mobileno);

					if (registration.ageGroup != null)
					{
						for (int i = 0; i < registration.ageGroup.Length; i++)
						{
							agegroup += registration.ageGroup[i].ToString() + ",";
						}

						agegroup = agegroup.TrimEnd(',');
					}

					List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() },
							new SetParameters { ParameterName = "@Name", Value = name },
							new SetParameters { ParameterName = "@age_group", Value = agegroup },
							new SetParameters { ParameterName = "@Mobile", Value = mobile },
							new SetParameters { ParameterName = "@ComWithWhatsApp", Value = registration.supportOnWhatsupFromHP == null ? "" : registration.supportOnWhatsupFromHP.Trim() },
							new SetParameters { ParameterName = "@ComWithPhone", Value = registration.supportOnPhoneFromHP == null ? "" : registration.supportOnPhoneFromHP.Replace("\"","") },
							new SetParameters { ParameterName = "@ComWithEmail", Value = registration.supportOnEmailFromHP == null ? "" : registration.supportOnEmailFromHP.Replace("\"","") },
							new SetParameters { ParameterName = "@RuParentOrStudent", Value = registration.RuParentOrStudent == null ? "" : registration.RuParentOrStudent  }
						};

					insertStatus = _db.StoreData("UpdateProfile", sp);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "UpdateProfile - Main Block");
			}

			return insertStatus;
		}

		public GetStatus UpdateYourProfile_SpecialPlan(Models.Registration registration)
		{
			dbProxy _db = new dbProxy();
			GetStatus insertStatus = new GetStatus();

			try
			{
				string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
				if (!String.IsNullOrEmpty(UserUniqueId))
				{
					string name = String.Empty;
					string alternatemobile = String.Empty;

					if (!String.IsNullOrEmpty(registration.name))
						name = clsCommon.Encrypt(registration.name);
					if (!String.IsNullOrEmpty(registration.alternatemobileno))
						alternatemobile = clsCommon.Encrypt(registration.alternatemobileno);

					List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters { ParameterName = "@UserUniqueId", Value = UserUniqueId },
							new SetParameters { ParameterName = "@Name", Value = name },
							new SetParameters { ParameterName = "@AlternateMobileNo", Value = alternatemobile }
						};

					insertStatus = _db.StoreData("USP_SpecialPlan_UpdateProfile", sp);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "UpdateProfile - Main Block");
			}

			return insertStatus;
		}

		public ActionResult Logout(string logOutSource)
		{
			//Set Cookie
			CultureManagePostHPID.SetCultureCookies();

			//HPIDAPIEnvironment hPIDAPIEnvironment = new HPIDAPIEnvironment();
			//string redirectUrl = hPIDAPIEnvironment.GetLogOutRedirectURL();

			Session.Abandon();
			Session.Clear();

			//Store in cookie
			dbAccessClass dbAccessClass = new dbAccessClass();
			dbAccessClass.CookieExpire();

			return Json(new { status = "Success", navigation = "/", message = "" }, JsonRequestBehavior.AllowGet);

			//if (logOutSource == "buynow")
			//	return Json(new { status = "Success", navigation = cultureUrl + "my-account/login", message = "" }, JsonRequestBehavior.AllowGet);
			//else
			//	return Json(new { status = "Success", navigation = "/" + cultureUrl, message = "" }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetMySubscription(string currentCultureName)
		{
			try
			{
				SubscriptionModel subscriptionModel = new SubscriptionModel();
				List<GetYourSubscriptionDetails> subscriptionDetails = new List<GetYourSubscriptionDetails>();
				dbProxy _db = new dbProxy();

				string culture = CultureName.GetCultureName();
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

				if (UserId > 0)
				{
					SessionManagement.DeleteFromSession(SessionType.subscriptionPopup);
					List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters{ ParameterName = "@QType", Value = "1" },
							new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() }
						};

					_variationContextAccessor.VariationContext = new VariationContext(currentCultureName);
					subscriptionDetails = _db.GetDataMultiple<GetYourSubscriptionDetails>("usp_getdata", subscriptionDetails, sp);

					if (subscriptionDetails == null)
					{
						return Json(new { status = "Success", navigation = culture + "/subscription/buy-now/", message = "" }, JsonRequestBehavior.AllowGet);
					}
					else if (subscriptionDetails != null && subscriptionDetails.Any())
					{
						SubscriptionDisplayContent subscriptionDisplayContent = new SubscriptionDisplayContent();
						//var title = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
						//						.Where(x => x.ContentType.Alias == "subscriptionTypeRoot")?.FirstOrDefault()?.Children?
						//						.Where(x => x.ContentType.Alias == "subscriptionList")?.OfType<SubscriptionList>().FirstOrDefault();

						subscriptionDisplayContent.Culture = culture;
						//subscriptionDisplayContent.activatedOnTitle = title.ActivatedOnTitle;
						//subscriptionDisplayContent.daysRemainingTitle = title.DaysRemainingTitle;
						//subscriptionDisplayContent.durationTitle = title.DurationTitle;
						//subscriptionDisplayContent.endsOnTitle = title.EndsOnTitle;
						//subscriptionDisplayContent.planPunchLine = title.PlanPunchLine;
						//subscriptionDisplayContent.planTitle = title.PlanTitle;
						//subscriptionDisplayContent.renewNowButtonTitle = title.RenewNowButtonTitle;
						//subscriptionDisplayContent.subscriptionPlanName = title.SubscriptionPlanName;
						//subscriptionDisplayContent.upgradeNowButtonTitle = title.UpgradeNowButtonTitle;
						//subscriptionDisplayContent.YearsTitle = title.YearsTitle;
						//subscriptionDisplayContent.DaysTitle = title.DaysTitle;
						//subscriptionDisplayContent.NoLimitTitle = title.NoLimitTitle;

						subscriptionModel.getYourSubscriptionDetails = subscriptionDetails;
						subscriptionModel.subscriptionDisplayContent = subscriptionDisplayContent;

						return PartialView("/Views/Partials/_subscribedWindow.cshtml", subscriptionModel);
					}
				}
				else
				{
					return Json(new { status = "Success", navigation = culture + "/my-account/login/", message = "" }, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "GetMySubscription - Main Block");
			}

			return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}


		public Responce CheckReferralCodeIsValid(string ReferralCode)
		{
			Responce responce = new Responce();
			try
			{
				if (!string.IsNullOrWhiteSpace(ReferralCode))
				{
					MyProfile myprofile = new MyProfile();
					dbProxy _db = new dbProxy();

					List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "1" },
				new SetParameters { ParameterName = "@ReferralCode", Value = ReferralCode }
			};
					myprofile = _db.GetData("CheckReferralCodeIsValid", myprofile, sp);
					if (myprofile != null && myprofile.Email != null && myprofile.ReferralCode != null)
					{
						responce.Result = myprofile;
						responce.StatusCode = HttpStatusCode.OK;
					}
					else
					{
						responce.Result = myprofile;
						responce.StatusCode = HttpStatusCode.NotFound;
						responce.Message = "Please enter the valid referral code";
					}
				}
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.Message;

				Logger.Error(reporting: typeof(HomeController), ex, message: "CheckReferralCodeIsValid - Main Block");
			}

			return responce;

		}

		[HttpGet]
		public ActionResult GetShareText(string ReferralCode)
		{
			Responce responce = new Responce();
			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("<div class='share-icon'>");
				sb.Append("<div class='share-box'>");
				sb.Append("<ul>");
				string Url = ConfigurationManager.AppSettings["SiteUrl"].ToString() + "my-account/registration?referralcode=" + ReferralCode;

				sb.Append(" <li><a href='javascript:void(0)' class='fb-icon aFBShare'><span style='display:none'>" + Url + "</span></a></li>");

				sb.Append(" <li><a href='javascript:void(0)' class='whatsapp-icon aWHTAppSH'><span style='display:none'>" + Url + "</span></a></li>");

				sb.Append(" <li><a href='javascript:void(0)' class='mail-icon aMailSh'><span style='display:none'>" + Url + "</span></a></li>");

				sb.Append("</ul>");
				sb.Append("</div>");
				sb.Append("</div>");
				responce.StatusCode = HttpStatusCode.OK;
				responce.Result = sb.ToString();
				responce.Message = Url;
			}
			catch (Exception ex)
			{
				responce.StatusCode = HttpStatusCode.InternalServerError;
				responce.Message = ex.Message;

				Logger.Error(reporting: typeof(HomeController), ex, message: "GetShareText - Main Block");
			}

			return Json(new
			{
				responce
			}, JsonRequestBehavior.AllowGet);
		}


		public string GetMaxSubscriptionRanking()
		{
			string maxRanking = String.Empty;
			maxRanking = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "subscriptionTypeRoot")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "subscriptionList")?.FirstOrDefault().Children?.OfType<Subscriptions>().ToList().Where(c => c.IsActive).Max(m => m.Ranking);

			return maxRanking;
		}
		public Subscriptions GetSubscriptionDetailsWithRanking(string ranking)
		{
			Subscriptions subscriptionDetails;
			subscriptionDetails = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "subscriptionTypeRoot")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "subscriptionList")?.FirstOrDefault().Children?.OfType<Subscriptions>()?
																  .Where(x => x.Ranking == ranking)?.FirstOrDefault();

			return subscriptionDetails;
		}

		public BonusAddSubscriptions GetSubscriptionDetailsWithRanking_BonusWorksheet(string ranking)
		{
			BonusAddSubscriptions subscriptionDetails;
			subscriptionDetails = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "bonusSubscriptionRoot")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "bonusSubscriptionList")?.FirstOrDefault().Children?.OfType<BonusAddSubscriptions>()?
																  .Where(x => x.Ranking == ranking)?.FirstOrDefault();

			return subscriptionDetails;
		}

		public TeachersAddSubscriptions GetSubscriptionDetailsWithRanking_TeachersWorksheet(string ranking)
		{
			TeachersAddSubscriptions subscriptionDetails;
			subscriptionDetails = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "teachersSubscriptionRoot")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "teachersSubscriptionList")?.FirstOrDefault().Children?.OfType<TeachersAddSubscriptions>()?
																  .Where(x => x.Ranking == ranking)?.FirstOrDefault();

			return subscriptionDetails;
		}
		public List<Subscriptions> GetSubscriptionDetails()
		{
			List<Subscriptions> subscriptionDetails;
			subscriptionDetails = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "subscriptionTypeRoot")?.FirstOrDefault()?.Children?
																  .Where(x => x.ContentType.Alias == "subscriptionList")?.FirstOrDefault().Children?.OfType<Subscriptions>().ToList();

			return subscriptionDetails;
		}

		public List<NameListItem> GetAgeGroup()
		{
			List<NameListItem> nameListItems = new List<NameListItem>();
			nameListItems = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()
							?.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>()?.ToList();

			return nameListItems;
		}

		public List<NameListItem> GetAgeGroup_lang(string language)
		{
			List<NameListItem> nameListItems = new List<NameListItem>();

			nameListItems = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()
							?.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>()?.ToList();

			return nameListItems;
		}

		public ActionResult GenerateReferralCode()
		{
			try
			{
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
				string Code = SessionManagement.GetCurrentSession<string>(SessionType.UserReferralCode);
				if (string.IsNullOrWhiteSpace(Code))
				{

					dbProxy _db = new dbProxy();
					List<SetParameters> spaddreferralcode = new List<SetParameters>()
			{

				new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() },
				new SetParameters{ ParameterName = "@ReferralCode",Value=clsCommon.GenerateReferralCode()},
			};
					GetStatus insertStatus = new GetStatus();
					insertStatus = _db.StoreData("InsertReferralCode", spaddreferralcode);
					if (insertStatus.returnStatus == "Success")
					{
						SessionManagement.StoreInSession(SessionType.UserReferralCode, spaddreferralcode[1].Value);
					}
				}
				TempData["IsGeneratedReferralCode"] = true;
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "GenerateReferralCode - Main Block");
			}

			return CurrentUmbracoPage();
		}

		public ActionResult EprintMailSend(string file, string printemail, string fromemail, string worksheetid, string itemname)
		{
			try
			{
				if (!String.IsNullOrEmpty(file))
				{
					SenderMailer mailsend = new SenderMailer();
					bool status = mailsend.SendEPrintEmailContent("HP Print Learn Center EPrint Mail", printemail, "", "", file);
					if (status)
					{
						string rowId = "0";
						string CultureInfo = CultureName.GetCultureName();
						rowId = InsertDownloadPrint(CultureInfo, printemail, itemname, worksheetid, file, "eprintdownload");
						return Json(new { status = "Success", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
					}
					else
						return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = "Fail", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "EprintMailSend - Main Block");
			}

			return Json(new { status = "Error", navigation = "", message = "" }, JsonRequestBehavior.AllowGet);
		}
		public string InsertDownloadPrint(string CultureInfo, string RefUserId, string Age, string WorkSheetId, string WorkshhetPDFUrl, string vFrom)
		{
			string RowId = "0";
			clsWorksheet _objclsWorksheet = new clsWorksheet();
			RowId = _objclsWorksheet.Insert_WorkSheet_Download_Print(CultureInfo, RefUserId, Age, WorkSheetId, WorkshhetPDFUrl, vFrom);
			return RowId;
		}

		[HttpPost]
		public ActionResult ToBeDecrypt(string converttext, int convertiontyp)
		{
			string decryptText = String.Empty;
			if (!String.IsNullOrWhiteSpace(converttext))
			{
				if (convertiontyp > 0 && convertiontyp == 1)
					decryptText = clsCommon.Decrypt(converttext);
				else if (convertiontyp > 0 && convertiontyp == 2)
					decryptText = clsCommon.encrypto(converttext);
			}

			return Json(new { status = "Success", message = decryptText }, JsonRequestBehavior.AllowGet);
		}


		public ActionResult GetBundlingPopup()
		{
			int HpBundlePopupOpenCount = SessionManagement.GetCurrentSession<int>(SessionType.HpBundlePopupOpenCount);
			if (HpBundlePopupOpenCount == 0)
				SessionManagement.StoreInSession(SessionType.HpBundlePopupOpenCount, 1);
			else
				SessionManagement.StoreInSession(SessionType.HpBundlePopupOpenCount, HpBundlePopupOpenCount + 1);

			return PartialView("/Views/Partials/Offer/_BundlingOffer.cshtml");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DontShowAgain(string CouponCode)
		{
			try
			{
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
				if (UserId > 0 && !String.IsNullOrWhiteSpace(CouponCode))
				{
					dbProxy _db = new dbProxy();
					GetStatus insertStatus = new GetStatus();

					List<SetParameters> spaddreferralcode = new List<SetParameters>()
						{
							new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() },
							new SetParameters{ ParameterName = "@CouponCode",Value=CouponCode},
						};

					insertStatus = _db.StoreData("USP_CouponOffer_DontShowAgain", spaddreferralcode);

					return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					Logger.Info(reporting: typeof(HomeController), message: "DontShowAgain - UserId or Couponcode is null");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(HomeController), ex, message: "DontShowAgain - DontShowAgain Issue");
			}

			return Json(new { status = "Fail" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public PartialViewResult LoginMessageChange(string messageId)
		{
			SessionManagement.StoreInSession(SessionType.LoginMessageDynamic, messageId);

			//return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);

			return PartialView("/Views/Partials/Login/_loginSnd.cshtml");
		}

		[HttpPost]
		public ActionResult LoginMessageChangeAjax(string messageId)
		{
			SessionManagement.StoreInSession(SessionType.LoginMessageDynamic, messageId);

			return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);

		}
	}
}