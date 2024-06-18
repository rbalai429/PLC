using HPPlc.Models;
using HPPlc.Models.Bot;
using HPPlc.Models.HtmlRenderHelper;
using HPPlc.Models.Mailer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web.Http;
//using Microsoft.AspNetCore.Mvc;
using Umbraco.Core.Composing;
using Umbraco.Web;
//using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace HPPlc.Controllers
{
	//[Route("botapi/[action]")]
	[RoutePrefix("api/bot")]
	public class BotAPIController : UmbracoApiController
	{
		HttpResponseMessage response;
		string siteURL = ConfigurationManager.AppSettings["SiteUrl"].ToString();

		
		[Route("enroll")]
		[HttpPost]
		public HttpResponseMessage Enroll(BotUserRegistration registration)
		{
			string ResponseUrl = String.Empty;

			string encEmail = String.Empty;
			string encMobile = String.Empty;
			Result ObjResult = new Result();
			ObjResult.Status = 1;

			try
			{
				string[] agegroup = new string[1];

				if (registration == null)
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Incomplete Details!!";
					ObjResult.Navigation = "";
					response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
				}
				else if (!String.IsNullOrWhiteSpace(registration.name) && IsAlphabetAndSpace(registration.name) == false)
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Name to be in alphabet with space only!!";
					ObjResult.Navigation = "";
					response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
				}
				else if (String.IsNullOrWhiteSpace(registration.email))
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Email Id can not be blank!!";
					ObjResult.Navigation = "";
					response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
				}
				else if (!String.IsNullOrWhiteSpace(registration.email) && ValidateEmail(registration.email) == false)
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Email Id not in correct format!!";
					ObjResult.Navigation = "";
					response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
				}
				else if (!String.IsNullOrWhiteSpace(registration.mobile) && IsMobileNumberValid(registration.mobile) == false)
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Mobile no. not in correct format!!";
					ObjResult.Navigation = "";
					response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
				}
				else if (String.IsNullOrWhiteSpace(registration.PlanSelection.ToString())) // || registration.PlanSelection <= 1
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Valid plan not selected!!";
					ObjResult.Navigation = "";
					response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
				}
				else if (String.IsNullOrWhiteSpace(registration.age.ToString()))
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Age can not be blank!!";
					ObjResult.Navigation = "";
					response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
				}
				else if (!String.IsNullOrWhiteSpace(registration.age.ToString()) && IsChar(registration.age.ToString()) == false)
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Age not in correct format!!";
					ObjResult.Navigation = "";
					response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
				}
				else if (!String.IsNullOrWhiteSpace(registration.age.ToString()))// Age group validation
				{
					int currentAge = int.Parse(registration.age);
					int plusoneAge = currentAge + 1;
					string actualAgeGroup = currentAge.ToString() + "-" + plusoneAge.ToString();

					//Umbraco.Web.PublishedModels.NameListItem ageGroups;
					//ageGroups = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
					//					.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?
					//					.OfType<Umbraco.Web.PublishedModels.NameListItem>().Where(x => x.ItemValue == actualAgeGroup && x.IsActice).FirstOrDefault();
					bool IsageGroups = AgeGroupExistOrNot(actualAgeGroup);
					if (IsageGroups == false)
					{
						ObjResult.Status = 0;
						ObjResult.Message = "Age group not valid.";
						ObjResult.Navigation = "";
						response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
					}
					else
					{ agegroup[0] = actualAgeGroup; }

					HomeController home = new HomeController();
					var subscription = home.GetSubscriptionDetailsWithRanking(registration.PlanSelection.ToString());

					if (subscription == null)
					{
						ObjResult.Status = 0;
						ObjResult.Message = "Valid plan not selected!!";
						ObjResult.Navigation = "";
						response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
					}
				}


				if (String.IsNullOrWhiteSpace(ObjResult.Message) && ObjResult.Status.Equals(1))
				{
					dbProxy _db;
					GetStatus botvalidate;

					if (!String.IsNullOrWhiteSpace(registration.email))
						encEmail = clsCommon.Encrypt(registration.email);

					if (!String.IsNullOrWhiteSpace(registration.mobile))
						encMobile = clsCommon.Encrypt(registration.mobile);

					List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters{ ParameterName = "@mobile", Value = encMobile },
						new SetParameters{ ParameterName = "@email", Value = encEmail }
					};

					//Check User already registered or not
					_db = new dbProxy();
					botvalidate = new GetStatus();
					botvalidate = _db.GetData<GetStatus>("proc_BotRegistrationValidation", botvalidate, sp);
					if (botvalidate != null)
					{
						if (botvalidate.returnValue == 1)//New User
						{
							//Registration
							_db = new dbProxy();
							botvalidate = new GetStatus();
							HomeController home = new HomeController();
							Registration botRegistration = new Registration();

							botRegistration.name = registration.name;
							botRegistration.email = registration.email;
							botRegistration.whatsupprefix = "+91";
							botRegistration.whatsupnumber = registration.mobile;
							botRegistration.ageGroup = agegroup;
							botRegistration.supportOnEmailFromHP = "No";
							botRegistration.supportOnWhatsupFromHP = "No";
							botRegistration.supportOnPhoneFromHP = "No";
							botRegistration.termsChecked = "No";
							botRegistration.referralCode = "";
							botRegistration.BotPlanSelection = registration.PlanSelection;

							botvalidate = home.Register(botRegistration, "Bot");
							if (botvalidate != null && botvalidate.returnStatus == "Success")
							{
								string setPasswordUrl = string.Empty;
								string encryptUserId = string.Empty;

								if (!string.IsNullOrWhiteSpace(botvalidate.returnValue.ToString()))
								{
									ResponseUrl = siteURL + "my-account/registration?u=" + clsCommon.Encrypt(registration.email);

									ObjResult.Status = 1;
									ObjResult.Message = "User registered successfully.";
									ObjResult.Navigation = ResponseUrl;
									response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
								}
							}
							else //not registered
							{
								ObjResult.Status = 0;
								ObjResult.Message = "Registration failed!";
								ObjResult.Navigation = "";
								response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
							}
						}
						else //Email Or mobile already registered
						{
							ObjResult.Status = botvalidate.returnValue;
							ObjResult.Message = botvalidate.returnMessage;
							ObjResult.Navigation = "";
							response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
						}
					}
				}
			}
			catch (Exception ex)
			{
				ObjResult.Status = 0;
				ObjResult.Message = "Data is not in correct format";
				ObjResult.Navigation = "";
				response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);

				Logger.Error(reporting: typeof(BotAPIController), ex, message: "bot registration - Main Block");
			}

			return response;
		}

		[Route("subscriptions")]
		[HttpPost]
		public HttpResponseMessage GetDetailOfSubscriptions(string email)
		{
			string emailEnc = String.Empty;
			BotSubscriptions ObjResult = new BotSubscriptions();
			ObjResult.Status = 1;
			List<BotSubscriptionDetails> Subscriptions = new List<BotSubscriptionDetails>();

			if (!String.IsNullOrWhiteSpace(email))
				emailEnc = clsCommon.Encrypt(email);

			try
			{
				if (String.IsNullOrWhiteSpace(email))
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Email Id can not be blank!!";
					response = Request.CreateResponse<BotSubscriptions>(HttpStatusCode.OK, ObjResult);
				}
				else if (!String.IsNullOrWhiteSpace(email) && ValidateEmail(email) == false)
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Email id not in correct format!!";
					response = Request.CreateResponse<BotSubscriptions>(HttpStatusCode.OK, ObjResult);
				}
				else if (!String.IsNullOrWhiteSpace(email) && ValidateEmail(email) == true)
				{
					//check emailid registered or not
					bool IsMailRegistered = EmailRegisteredOrNot(emailEnc);
					if (IsMailRegistered == false)
					{
						ObjResult.Status = 0;
						ObjResult.Message = "Email id is not registered!!";
						response = Request.CreateResponse<BotSubscriptions>(HttpStatusCode.OK, ObjResult);
					}
				}

				if (ObjResult.Status == 1)
				{
					HomeController home = new HomeController();
					var maxSubscription = home.GetMaxSubscriptionRanking();

					string emailEncrypt = clsCommon.Encrypt(email);
					List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters{ ParameterName = "@QType", Value = "1" },
						new SetParameters{ ParameterName = "@email", Value = emailEncrypt },
						new SetParameters{ ParameterName = "@Culture", Value = "" },
						new SetParameters{ ParameterName = "@maxRanking", Value = maxSubscription }
					};

					//Check User already registered or not
					dbProxy _db = new dbProxy();
					Subscriptions = _db.GetDataMultiple("usp_getdata_bot", Subscriptions, sp);
					if (Subscriptions != null && Subscriptions.Any())
					{

						HomeController allage = new HomeController();
						var ageGroups = allage.GetAgeGroup();
						if (ageGroups != null && ageGroups.Any())
						{
							foreach (var age in ageGroups)
							{
								if (!Subscriptions.Where(x => x.AgeGroup == age.ItemValue).Any())
								//if (!age.ItemValue.Equals(Subscriptions.Where(x => x.AgeGroup == age.ItemValue)))
								{
									Subscriptions.Add(new BotSubscriptionDetails()
									{
										AgeGroup = age.ItemValue,
										//Ranking = null,
										SubscriptionName = "",
										SubscriptionPrice = null,
										PlanValidity = "",
										Is_PLC1_Customer = Subscriptions?.FirstOrDefault()?.Is_PLC1_Customer,
										//SubscriptionStartDate = null,
										//SubscriptionEndDate = null,
										//DaysRemaining = 0,
										Type = "New"
									});
								}

							}

							ObjResult.Status = 1;
							ObjResult.Message = "Subscription Details";
							ObjResult.Subscriptions = Subscriptions;
							response = Request.CreateResponse<BotSubscriptions>(HttpStatusCode.OK, ObjResult);

						}

					}
					else
					{
						ObjResult.Status = 0;
						ObjResult.Message = "Data not found";
						response = Request.CreateResponse<BotSubscriptions>(HttpStatusCode.OK, ObjResult);
					}
				}
			}
			catch (Exception ex)
			{
				ObjResult.Status = 0;
				ObjResult.Message = "Data is not in correct format";
				response = Request.CreateResponse<BotSubscriptions>(HttpStatusCode.OK, ObjResult);

				Logger.Error(reporting: typeof(BotAPIController), ex, message: "bot subscription - Main Block");
			}
			return response;
		}

		[Route("SubscriptionPaymentUrl")]
		[HttpPost]
		public HttpResponseMessage SetPaymentUrl(BotSubscriptionPaymentUrl PaymentUrlRequest)
		{
			string emailEnc = String.Empty;
			PaymentLinkResponse ObjResult = new PaymentLinkResponse();
			if (PaymentUrlRequest != null)
			{
				ObjResult.Status = 1;

				if (!String.IsNullOrWhiteSpace(PaymentUrlRequest.Email))
					emailEnc = clsCommon.Encrypt(PaymentUrlRequest.Email);

				try
				{
					if (String.IsNullOrWhiteSpace(PaymentUrlRequest.Email))
					{
						ObjResult.Status = 0;
						ObjResult.Message = "Email Id can not be blank!!";
						ObjResult.PaymentLink = null;
						response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
					}
					else if (!String.IsNullOrWhiteSpace(PaymentUrlRequest.Email) && ValidateEmail(PaymentUrlRequest.Email) == false)
					{
						ObjResult.Status = 0;
						ObjResult.Message = "Email id not in correct format!!";
						ObjResult.PaymentLink = null;
						response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
					}
					else if (EmailRegisteredOrNot(emailEnc) == false)
					{
						//check emailid registered or not
						ObjResult.Status = 0;
						ObjResult.Message = "Email id is not registered!!";
						ObjResult.PaymentLink = null;
						response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
					}
					else if (PaymentUrlRequest.botSubscriptionSetPaymentUrl == null || PaymentUrlRequest.botSubscriptionSetPaymentUrl.Count == 0)
					{
						ObjResult.Status = 0;
						ObjResult.Message = "Data not in correct format!!";
						ObjResult.PaymentLink = null;
						response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
					}
					else if (PaymentUrlRequest.botSubscriptionSetPaymentUrl != null || PaymentUrlRequest.botSubscriptionSetPaymentUrl.Count > 0)
					{
						foreach (var payrequest in PaymentUrlRequest.botSubscriptionSetPaymentUrl)
						{
							if (String.IsNullOrWhiteSpace(payrequest.Type))
							{
								ObjResult.Status = 0;
								ObjResult.Message = "Data type not in correct format!!";
								ObjResult.PaymentLink = null;
								response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);

								return response;
							}

							if (String.IsNullOrWhiteSpace(payrequest.Rank.ToString()))
							{
								ObjResult.Status = 0;
								ObjResult.Message = "Rank not in correct format!!";
								ObjResult.PaymentLink = null;
								response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
							}

							if (!String.IsNullOrWhiteSpace(payrequest.Rank.ToString()))
							{
								HomeController home = new HomeController();
								var subscription = home.GetSubscriptionDetailsWithRanking(payrequest.Rank.ToString());

								if (subscription == null)
								{
									ObjResult.Status = 0;
									ObjResult.Message = "Rank is not valid!!";
									ObjResult.PaymentLink = "";
									response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
								}
							}

							if (String.IsNullOrWhiteSpace(payrequest.Agegroup))
							{
								ObjResult.Status = 0;
								ObjResult.Message = "Data agegroup not in correct format!!";
								ObjResult.PaymentLink = null;
								response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);

								return response;
							}
							else
							{
								bool IsageGroups = AgeGroupExistOrNot(payrequest.Agegroup);
								if (IsageGroups == false)
								{
									ObjResult.Status = 0;
									ObjResult.Message = "Data agegroup not found!!";
									ObjResult.PaymentLink = null;
									response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);

									return response;
								}
							}
						}
					}

					if (String.IsNullOrWhiteSpace(ObjResult.Message) && ObjResult.Status.Equals(1))
					{
						int ageWiseDuplicacy = PaymentUrlRequest.botSubscriptionSetPaymentUrl.GroupBy(x => x.Agegroup).Count();
						if (PaymentUrlRequest.botSubscriptionSetPaymentUrl.Count == ageWiseDuplicacy)
						{
							string ageAndType = string.Empty;
							string PaymntUrl = siteURL + "bot-payment?u=" + emailEnc + "&pay=";
							foreach (var payrequest in PaymentUrlRequest.botSubscriptionSetPaymentUrl)
							{
								if (String.IsNullOrWhiteSpace(ageAndType))
									ageAndType = payrequest.Rank + "," + payrequest.Agegroup + "," + payrequest.Type;
								else
									ageAndType += "&" + payrequest.Rank + "," + payrequest.Agegroup + "," + payrequest.Type;
							}

							ageAndType = clsCommon.Encrypt(ageAndType);
							PaymntUrl = PaymntUrl + ageAndType;

							ObjResult.Status = 1;
							ObjResult.Message = "Payment Url";
							ObjResult.PaymentLink = PaymntUrl;
							response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
						}
						else
						{
							ObjResult.Status = 0;
							ObjResult.Message = "A age group can not repeat.";
							ObjResult.PaymentLink = "";
							response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
						}
					}
				}
				catch (Exception ex)
				{
					ObjResult.Status = 0;
					ObjResult.Message = "Data is not in correct format";
					ObjResult.PaymentLink = null;
					response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);

					Logger.Error(reporting: typeof(BotAPIController), ex, message: "bot subscription - Main Block");
				}
			}
			else
			{
				ObjResult.Status = 0;
				ObjResult.Message = "Data is not in correct format!!";
				ObjResult.PaymentLink = null;
				response = Request.CreateResponse<PaymentLinkResponse>(HttpStatusCode.OK, ObjResult);
			}
			return response;
		}

		[Route("botcheck")]
		[HttpGet]
		public HttpResponseMessage BotCheck()
		{
			Result ObjResult = new Result();
			ObjResult.Status = 0;
			ObjResult.Message = "Email Id can not be blank!!";
			response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);

			return response;
		}


		//[Route("registration")]
		//[HttpGet]
		//public HttpResponseMessage Registration(BotUserRegistration registration)
		//{

		//	Result ObjResult = new Result();
		//	ObjResult.Status = 0;
		//	ObjResult.Message = "Email Id can not be blank!!";
		//	response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);

		//	//Result ObjResult = new Result();
		//	//dbProxy _db = new dbProxy();

		//	//if (registration == null)
		//	//{
		//	//	ObjResult.Status = 0;
		//	//	ObjResult.Message = "Registration details not found!!";
		//	//	response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
		//	//}
		//	//else if (String.IsNullOrWhiteSpace(registration.email))
		//	//{
		//	//	ObjResult.Status = 0;
		//	//	ObjResult.Message = "Email Id can not be blank!!";
		//	//	response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
		//	//}
		//	//try
		//	//{
		//	//	result = cricketerBL.AddUpdateCricketerInfo(Cricketer);
		//	//	if (result > 0)
		//	//	{
		//	//		if (result == 1)
		//	//		{
		//	//			ObjResult = newResult();
		//	//			ObjResult.Status = result;
		//	//			ObjResult.Message = "Record Inserted Successfully!!";
		//	//			response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
		//	//		}
		//	//		else if(result == 2) {
		//	//			ObjResult = newResult();
		//	//			ObjResult.Status = result;
		//	//			ObjResult.Message = "Record Already Exists!!";
		//	//			response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
		//	//		}

		//	//else
		//	//		{
		//	//			ObjResult = newResult();
		//	//			ObjResult.Status = result;
		//	//			ObjResult.Message = "Record Not Added!!";
		//	//			response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
		//	//		}
		//	//	}
		//	//}
		//	//catch (Exception ex)
		//	//{
		//	//	ObjResult = newResult();
		//	//	ObjResult.Status = 0;
		//	//	ObjResult.Message = ex.Message;
		//	//	response = Request.CreateResponse(HttpStatusCode.InternalServerError, ObjResult);
		//	//}
		//	return response;
		//}

		private bool ValidateEmail(string email)
		{
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			Match match = regex.Match(email);
			if (match.Success)
				return true;
			else
				return false;
		}

		public bool IsMobileNumberValid(string mobile)
		{
			bool isValid = false;
			if (!string.IsNullOrWhiteSpace(mobile))
			{
				isValid = Regex.IsMatch(mobile, @"\+?[0-9]{10}",
					 RegexOptions.IgnoreCase);
			}

			return isValid;
		}

		public bool IsChar(string s)
		{
			bool isValid = false;
			if (!string.IsNullOrWhiteSpace(s))
			{
				isValid = s.All(char.IsDigit);
			}
			return isValid;
		}

		public bool IsAlphabetAndSpace(string name)
		{
			bool isValid = false;
			if (!string.IsNullOrWhiteSpace(name))
			{
				isValid = Regex.IsMatch(name, @"^[a-zA-Z ]+$",
					 RegexOptions.IgnoreCase);
			}

			return isValid;
		}

		public bool AgeGroupExistOrNot(string agegroup)
		{
			HomeController home = new HomeController();
			List<Umbraco.Web.PublishedModels.NameListItem> nameListItem;
			nameListItem = home.GetAgeGroup();
			var checkagegroup = nameListItem.Where(x => x.ItemValue == agegroup && x.IsActice).FirstOrDefault();
			if (String.IsNullOrWhiteSpace(checkagegroup.ToString()))
				return false;
			else
				return true;
		}

		public bool EmailRegisteredOrNot(string Email)
		{
			bool isMailRegistered = true;
			string isMailExist = String.Empty;
			dbProxy _db = new dbProxy();
			List<SetParameters> sp = new List<SetParameters>()
									{
										new SetParameters{ ParameterName = "@QType", Value = "3" },
										new SetParameters{ ParameterName = "@email", Value = Email }
									};
			isMailExist = _db.GetData<string>("usp_getdata_bot", isMailExist, sp);
			if (!String.IsNullOrWhiteSpace(isMailExist) && isMailExist.Equals("no"))
				isMailRegistered = false;

			return isMailRegistered;
		}
	}
}
