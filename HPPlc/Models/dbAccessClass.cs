using HPPlc.Controllers;
using HPPlc.Models.Blog;
using HPPlc.Models.HPUId;
using HPPlc.Models.Mailer;
using HPPlc.Models.PDFGenerator;
using HPPlc.Models.Videos;
using HPPlc.Models.WorkSheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Models
{
	public class dbAccessClass
	{
		dbProxy _db = new dbProxy();
		public MyProfile GetProfile()
		{
			MyProfile myprofile = new MyProfile();
			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			if (UserId > 0)
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "2" },
				new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() }
			};
				myprofile = _db.GetData("usp_getdata", myprofile, sp);
			}

			return myprofile;
		}
		public MyProfile GetProfileById(string UserId)
		{
			MyProfile myprofile = new MyProfile();
			if (!String.IsNullOrWhiteSpace(UserId))
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "2" },
				new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() }
			};
				myprofile = _db.GetData("usp_getdata", myprofile, sp);
			}

			return myprofile;
		}

		public MyProfileWithSubscription GetProfileWithPaymentId(string PaymentId)
		{
			MyProfileWithSubscription myprofile = new MyProfileWithSubscription();
			if (!String.IsNullOrEmpty(PaymentId))
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "6" },
				new SetParameters { ParameterName = "@UserId", Value = "0" },
				new SetParameters { ParameterName = "@mobile", Value = "" },
				new SetParameters { ParameterName = "@email", Value = "" },
				new SetParameters { ParameterName = "@PaymentId", Value = PaymentId }
			};
				myprofile = _db.GetData("usp_getdata", myprofile, sp);
			}

			return myprofile;
		}
		//public SubscriptionSuccessParam SetResponseFromPGPay(string responseStatus, string[] strSplitDecryptedResponse)
		//{
		//	SubscriptionController Cntl = new SubscriptionController();
		//	SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
		//	returnParam = Cntl.SetResponseFromPG(responseStatus, strSplitDecryptedResponse);

		//	return returnParam;
		//}

		public MyProfile GetProfileByEmail(string email)
		{
			MyProfile myprofile = new MyProfile();
			if (!String.IsNullOrWhiteSpace(email))
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "12" },
				new SetParameters{ ParameterName = "@UserId", Value = "0" },
				new SetParameters{ ParameterName = "@mobile", Value = "" },
				new SetParameters{ ParameterName = "@email", Value = email }
			};
				myprofile = _db.GetData("usp_getdata", myprofile, sp);
			}

			return myprofile;
		}

		public MyProfile GetMyProfileForSpecialPlan()
		{
			MyProfile myprofile = new MyProfile();
			string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
			if (!String.IsNullOrWhiteSpace(UserUniqueId))
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "6" },
				new SetParameters { ParameterName = "@UserUniqueCode", Value = UserUniqueId }
			};
				myprofile = _db.GetData("USP_GetSpecialPlan", myprofile, sp);
			}

			return myprofile;
		}
		public SubscriptionSuccessParam SetResponseFromPG(string responseStatus, string[] strSplitDecryptedResponse)
		{
			SubscriptionController subsCont = new SubscriptionController();
			SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
			PaymentStatus paymentStatus = new PaymentStatus();
			string loinparamer = String.Empty;
			string PaymentId = String.Empty;

			try
			{
				if (strSplitDecryptedResponse != null && strSplitDecryptedResponse.Length > 0)
				{
					string[] strGetMerchantParamForCompare;
					for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
					{
						strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
						if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
						{
							string[] existsInformation = strGetMerchantParamForCompare[1].Split('}');
							if (existsInformation.Length >= 2)
							{
								for (int sub = 0; sub < existsInformation.Length; sub++)
								{
									string[] splitValue = existsInformation[sub].Split(':');
									if (splitValue[0].ToString().ToLower().Replace("{", "").Replace("}", "") == "custid")
									{
										paymentStatus.PaymentId = splitValue[1];

										break;
										//string[] subscribeId = existsInformation[2].Split(':');
										//if (subscribeId[0].Replace("{", "").Replace("}", "") == "custid")
										//{
										//	paymentStatus.SubscriptionId = subscribeId[1];
										//}
									}
								}
							}
						}
						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
							paymentStatus.txn_status = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_MSG")
							paymentStatus.txn_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_ERR_MSG")
							paymentStatus.txn_err_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
							paymentStatus.clnt_txn_ref = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
							paymentStatus.tpsl_bank_cd = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
							paymentStatus.tpsl_txn_id = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
							paymentStatus.txn_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
							paymentStatus.tpsl_txn_time = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_RFND_ID")
							paymentStatus.tpsl_rfnd_id = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "BAL_AMT")
							paymentStatus.bal_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "RQST_TOKEN")
							paymentStatus.rqst_token = Convert.ToString(strGetMerchantParamForCompare[1]);
					}

					PostPayUser postpay = new PostPayUser();

					try
					{
						postpay = PostPayUserDtls(paymentStatus.PaymentId);
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "PostPayUserDtls";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}

					try
					{
						paymentStatus.PaymentMode = "";
						returnParam = subsCont.PaymentAndSubscriptionInput(paymentStatus, postpay);
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "PaymentAndSubscriptionInput";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}

					try
					{
						//Exired Bundling Cookie here
						HttpContext.Current.Response.Cookies["IsBundleUser"].Expires = DateTime.Now.AddMinutes(-1);
						HttpContext.Current.Response.Cookies["IsOfferUser"].Expires = DateTime.Now.AddMinutes(-1);

						loinparamer = PostPayLoggedIn(paymentStatus.PaymentId, "");
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "PostPayLoggedIn";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}
				}
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "Root SetResponseFromPG";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return returnParam;
		}

		public SubscriptionSuccessParam SetResponseFromPG_BonusWorksheet(string responseStatus, string[] strSplitDecryptedResponse)
		{
			SubscriptionController subsCont = new SubscriptionController();
			SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
			PaymentStatus paymentStatus = new PaymentStatus();
			string loinparamer = String.Empty;
			string PaymentId = String.Empty;

			try
			{
				if (strSplitDecryptedResponse != null && strSplitDecryptedResponse.Length > 0)
				{
					string[] strGetMerchantParamForCompare;
					for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
					{
						strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
						if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
						{
							string[] existsInformation = strGetMerchantParamForCompare[1].Split('}');
							if (existsInformation.Length >= 2)
							{
								for (int sub = 0; sub < existsInformation.Length; sub++)
								{
									string[] splitValue = existsInformation[sub].Split(':');
									if (splitValue[0].ToString().ToLower().Replace("{", "").Replace("}", "") == "custid")
									{
										paymentStatus.PaymentId = splitValue[1];

										break;
										//string[] subscribeId = existsInformation[2].Split(':');
										//if (subscribeId[0].Replace("{", "").Replace("}", "") == "custid")
										//{
										//	paymentStatus.SubscriptionId = subscribeId[1];
										//}
									}
								}
							}
						}
						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
							paymentStatus.txn_status = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_MSG")
							paymentStatus.txn_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_ERR_MSG")
							paymentStatus.txn_err_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
							paymentStatus.clnt_txn_ref = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
							paymentStatus.tpsl_bank_cd = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
							paymentStatus.tpsl_txn_id = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
							paymentStatus.txn_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
							paymentStatus.tpsl_txn_time = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_RFND_ID")
							paymentStatus.tpsl_rfnd_id = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "BAL_AMT")
							paymentStatus.bal_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "RQST_TOKEN")
							paymentStatus.rqst_token = Convert.ToString(strGetMerchantParamForCompare[1]);
					}

					PostPayUser postpay = new PostPayUser();

					try
					{
						postpay = PostPayUserDtls_Bonus(paymentStatus.PaymentId);
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass Bonus";
						error.MethodName = "PostPayUserDtls";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}

					try
					{
						paymentStatus.PaymentMode = "";
						returnParam = subsCont.PaymentAndSubscriptionInputBonus(paymentStatus, postpay, "");
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "PaymentAndSubscriptionInput";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}

					try
					{
						//Exired Bundling Cookie here
						HttpContext.Current.Response.Cookies["IsBundleUser"].Expires = DateTime.Now.AddMinutes(-1);
						HttpContext.Current.Response.Cookies["IsOfferUser"].Expires = DateTime.Now.AddMinutes(-1);

						loinparamer = PostPayLoggedIn(paymentStatus.PaymentId, "BONUS");
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "PostPayLoggedIn";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}
				}
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "Root SetResponseFromPG";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return returnParam;
		}


		public SubscriptionSuccessParam SetResponseFromPG_SpecialPlan(string responseStatus, string[] strSplitDecryptedResponse)
		{
			SubscriptionController subsCont = new SubscriptionController();
			SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
			PaymentStatus paymentStatus = new PaymentStatus();
			string loinparamer = String.Empty;
			string PaymentId = String.Empty;

			try
			{
				if (strSplitDecryptedResponse != null && strSplitDecryptedResponse.Length > 0)
				{
					string[] strGetMerchantParamForCompare;
					for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
					{
						strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
						if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
						{
							string[] existsInformation = strGetMerchantParamForCompare[1].Split('}');
							if (existsInformation.Length >= 2)
							{
								for (int sub = 0; sub < existsInformation.Length; sub++)
								{
									string[] splitValue = existsInformation[sub].Split(':');
									if (splitValue[0].ToString().ToLower().Replace("{", "").Replace("}", "") == "custid")
									{
										paymentStatus.PaymentId = splitValue[1];

										break;
									}
								}
							}
						}
						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
							paymentStatus.txn_status = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_MSG")
							paymentStatus.txn_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_ERR_MSG")
							paymentStatus.txn_err_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
							paymentStatus.clnt_txn_ref = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
							paymentStatus.tpsl_bank_cd = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
							paymentStatus.tpsl_txn_id = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
							paymentStatus.txn_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
							paymentStatus.tpsl_txn_time = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_RFND_ID")
							paymentStatus.tpsl_rfnd_id = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "BAL_AMT")
							paymentStatus.bal_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "RQST_TOKEN")
							paymentStatus.rqst_token = Convert.ToString(strGetMerchantParamForCompare[1]);
					}

					PostPayUser postpay = new PostPayUser();

					try
					{
						postpay = PostPayUserDtls_SpecialPlan(paymentStatus.PaymentId);
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "SetResponseFromPG_SpecialPlan";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}

					try
					{
						paymentStatus.PaymentMode = "";
						returnParam = subsCont.PaymentAndSubscriptionInput_SpecialPlan(paymentStatus, postpay);
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "SetResponseFromPG_SpecialPlan";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}

					try
					{
						//Exired Bundling Cookie here
						HttpContext.Current.Response.Cookies["IsBundleUser"].Expires = DateTime.Now.AddMinutes(-1);
						HttpContext.Current.Response.Cookies["IsOfferUser"].Expires = DateTime.Now.AddMinutes(-1);

						loinparamer = PostPayLoggedIn(paymentStatus.PaymentId, "SP");
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "PostPayLoggedIn";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}
				}
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "Root SetResponseFromPG_SpecialPlan";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return returnParam;
		}

		public string PostPayLoggedIn(string PaymentId, string mode)
		{
			string isloggedIn = String.Empty;
			isloggedIn = "N";
			dbProxy _db = new dbProxy();
			ReturnMessage returnMessage = new ReturnMessage();
			PostPaymentLogin postPaymentLogin = new PostPaymentLogin();
			UserHpIdDetails userHpIdDetails = new UserHpIdDetails();
			TempUserJourneyDtls tempUserJourneyDtls = new TempUserJourneyDtls();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@PaymentId", Value = PaymentId },
				new SetParameters{ ParameterName = "@mode", Value = mode }
			};

			//postPaymentLogin = _db.GetData<PostPaymentLogin>("usp_Login_PostPayment", postPaymentLogin, sp);
			//if (postPaymentLogin != null)
			//{
			//	HomeController login = new HomeController();
			//	returnMessage = login.LoggedInData(postPaymentLogin.UserName, postPaymentLogin.Password, null, null, "paylogin");
			//}

			userHpIdDetails = _db.GetData<UserHpIdDetails>("usp_Login_PostPayment", userHpIdDetails, sp);
			if (userHpIdDetails != null)
			{
				if (!String.IsNullOrWhiteSpace(userHpIdDetails.Email))
					tempUserJourneyDtls.UserEmail = userHpIdDetails.Email;
				if (!String.IsNullOrWhiteSpace(userHpIdDetails.Mobile))
					tempUserJourneyDtls.UserMobile = userHpIdDetails.Mobile;
				else
					tempUserJourneyDtls.UserMobile = "";

				tempUserJourneyDtls.UserName = "";
				tempUserJourneyDtls.UserPassword = "";
				tempUserJourneyDtls.Source = "paylogin";

				HomeController login = new HomeController();
				returnMessage = login.LoggedInData(tempUserJourneyDtls, null, "paylogin", "");
			}

			return isloggedIn = returnMessage.status;
		}

		public string BrowserLoginLoggedIn()
		{
			ReturnMessage returnMessage = new ReturnMessage();
			string isloggedIn = String.Empty;
			isloggedIn = "N";

			if (HttpContext.Current.Request.Cookies.AllKeys.Contains("userInfo") && !String.IsNullOrEmpty(HttpContext.Current.Request.Cookies["userInfo"].Value))
			{
				HttpCookie plc_usr = HttpContext.Current.Request.Cookies["userInfo"];

				if (plc_usr != null && !String.IsNullOrEmpty(plc_usr.Value))
				{
					dbProxy _db = new dbProxy();
					UserHpIdDetails userHpIdDetails = new UserHpIdDetails();
					TempUserJourneyDtls tempUserJourneyDtls = new TempUserJourneyDtls();
					string UserId = plc_usr.Values["_plc_usr"].ToString();

					UserId = clsCommon.DecryptWithBase64Code(UserId);
					if (!String.IsNullOrEmpty(UserId))
					{
						List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@UserId", Value = UserId }
			};

						userHpIdDetails = _db.GetData<UserHpIdDetails>("usp_Login_BrowserClose", userHpIdDetails, sp);
						if (userHpIdDetails != null)
						{
							if (!String.IsNullOrWhiteSpace(userHpIdDetails.Email))
								tempUserJourneyDtls.UserEmail = userHpIdDetails.Email;
							if (!String.IsNullOrWhiteSpace(userHpIdDetails.Mobile))
								tempUserJourneyDtls.UserMobile = userHpIdDetails.Mobile;
							else
								tempUserJourneyDtls.UserMobile = "";

							tempUserJourneyDtls.UserName = "";
							tempUserJourneyDtls.UserPassword = "";
							tempUserJourneyDtls.Source = "browsercloselogin";
							tempUserJourneyDtls.Rememberme = "Yes";

							CookieExpire();
							HomeController login = new HomeController();
							returnMessage = login.LoggedInData(tempUserJourneyDtls, null, "browsercloselogin", "");


						}
					}
					
				}
			}

			return isloggedIn = returnMessage.status;
		}

		public string CookieStore(string UserId)
		{
			try
			{
				System.Web.HttpCookie userInfo = new System.Web.HttpCookie("userInfo");
				userInfo.Values.Add("_plc_usr", clsCommon.Encryptwithbase64Code(UserId.ToString()));

				//userInfo.Expires = DateTime.Now.AddHours(8);
				userInfo.Expires = DateTime.MaxValue;
				HttpContext.Current.Response.Cookies.Add(userInfo);
			}
			catch { }

			return "done";
		}
		public string CookieExpire()
		{
			try
			{
				if (HttpContext.Current.Request.Cookies.AllKeys.Contains("userInfo") && !String.IsNullOrEmpty(HttpContext.Current.Request.Cookies["userInfo"].Value))
				{
					HttpCookie plc_usr = HttpContext.Current.Request.Cookies["userInfo"];
					plc_usr.Expires = DateTime.Now.AddDays(-1);
					HttpContext.Current.Response.Cookies.Add(plc_usr);
				}
			}
			catch { }

			return "done";
		}
		public PostPayUser PostPayUserDtls(string PaymentId)
		{
			PostPayUser postpay = new PostPayUser();
			string isloggedIn = String.Empty;
			isloggedIn = "N";
			dbProxy _db = new dbProxy();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@PaymentId", Value = PaymentId }
			};

			postpay = _db.GetData<PostPayUser>("usp_Data_PostPayment", postpay, sp);

			return postpay;
		}

		public PostPayUser PostPayUserDtls_Bonus(string PaymentId)
		{
			PostPayUser postpay = new PostPayUser();
			string isloggedIn = String.Empty;
			isloggedIn = "N";
			dbProxy _db = new dbProxy();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@PaymentId", Value = PaymentId }
			};

			postpay = _db.GetData<PostPayUser>("usp_Data_PostPayment_Bonus", postpay, sp);

			return postpay;
		}
		public PostPayUser PostPayUserDtls_SpecialPlan(string PaymentId)
		{
			PostPayUser postpay = new PostPayUser();
			string isloggedIn = String.Empty;
			isloggedIn = "N";
			dbProxy _db = new dbProxy();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@PaymentId", Value = PaymentId }
			};

			postpay = _db.GetData<PostPayUser>("usp_Data_PostPayment_SpecialPlan", postpay, sp);

			return postpay;
		}
		//public bool SubscriptionMailer(List<InvoiceData> InvoiceList, string InvoiceBitlyUrl, string TransactionId)
		//{
		//	List<InvoiceData> mailContent = new List<InvoiceData>();
		//	string mailStatus = String.Empty;
		//	string subscriptiondetails = String.Empty;
		//	string name = String.Empty;
		//	string email = String.Empty;
		//	bool vResponse = false;

		//	try
		//	{
		//		// send mail
		//		if (mailContent != null && !String.IsNullOrEmpty(mailContent.FirstOrDefault().Email))
		//		{
		//			if (!String.IsNullOrEmpty(mailContent.FirstOrDefault().Email))
		//				email = clsCommon.Decrypt(mailContent.FirstOrDefault().Email);
		//			if (!String.IsNullOrEmpty(mailContent.FirstOrDefault().Name))
		//				name = clsCommon.Decrypt(mailContent.FirstOrDefault().Name);

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
		//				using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/Views/mailer/subscription.html")))
		//				{
		//					body = reader.ReadToEnd();

		//					body = body.Replace("{name}", name);
		//					body = body.Replace("{subscriptionplan}", mailContent.FirstOrDefault().SubscriptionName);
		//					body = body.Replace("{SubscriptionDetails}", subscriptiondetails);
		//					body = body.Replace("{invoiceno}", InvoiceBitlyUrl);
		//					body = body.Replace("{transactionid}", TransactionId);
		//				}
		//				//mailStatus = body;

		//				vResponse = mailsend.SendPaymentEmailerContent("Welcome to HP Print Learn Center", email, "", "", body);

		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		//mailStatus = ex.Message;
		//	}

		//	return vResponse;
		//}


		public List<SelectedAgeGroup> GetUserSelectedUserGroup()
		{
			dbProxy _db = new dbProxy();
			List<SelectedAgeGroup> myagegroup = new List<SelectedAgeGroup>();
			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			if (UserId > 0)
			{
				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters{ ParameterName = "@QType", Value = "4" },
					new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() }
				};

				myagegroup = _db.GetDataMultiple<SelectedAgeGroup>("usp_getdata", myagegroup, sp);
			}

			return myagegroup;
		}

		public List<GetUserCurrentSubscription> GetUserSubscriptions()
		{
			dbProxy _db = new dbProxy();
			List<GetUserCurrentSubscription> mySubscription = new List<GetUserCurrentSubscription>();
			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			if (UserId > 0)
			{
				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters{ ParameterName = "@QType", Value = "5" },
					new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() }
				};

				mySubscription = _db.GetDataMultiple<GetUserCurrentSubscription>("usp_getdata", mySubscription, sp);
			}

			return mySubscription;
		}


		public Responce CheckExistingUserApplicableForPointsOrNot()
		{
			Responce response = new Responce();
			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

			if (UserId > 0)
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "8" },
				new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() }
			};
				response = _db.GetData("usp_getdata", response, sp);
			}

			return response;
		}

		public static GetStatus PostApplicationError(ApplicationError error)
		{
			GetStatus response = new GetStatus();
			dbProxy _db = new dbProxy();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@PageName", Value = error.PageName },
				new SetParameters{ ParameterName = "@MethodName", Value = error.MethodName },
				new SetParameters{ ParameterName = "@ErrorMessage", Value = error.ErrorMessage }
			};

			response = _db.StoreData("USP_ApplicationError", sp);

			return response;
		}

		public static string SFMCResponse(string TransactionCode, string UserId, Responce res)
		{
			dbProxy _db = new dbProxy();
			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@TransactionCode", Value = TransactionCode == null ? "" : TransactionCode},
				new SetParameters{ ParameterName = "@UserId", Value = UserId},
				new SetParameters{ ParameterName = "@Message", Value = res.Message == null ? "" : res.Message },
				new SetParameters{ ParameterName = "@Result", Value = res.Result == null ? "" : res.Result.ToString()},
				new SetParameters{ ParameterName = "@StatusCode", Value = res.StatusCode.ToString() == null ? "" : res.StatusCode.ToString()}
			};

			_db.StoreData("USP_SFMCResponse", sp);

			return "OK";
		}

		public List<EncryptionDecryption> decryptandencrypt()
		{
			List<EncryptionDecryption> myprofile = new List<EncryptionDecryption>();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "13" },
				new SetParameters{ ParameterName = "@UserId", Value = "0" }
			};
			myprofile = _db.GetDataMultiple<EncryptionDecryption>("usp_getdata", myprofile, sp);


			return myprofile;
		}
		public List<EncryptionDecryption> encDecGetData()
		{
			List<EncryptionDecryption> enc = new List<EncryptionDecryption>();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "1" }
			};
			enc = _db.GetDataMultiple<EncryptionDecryption>("USP_DataConversion", enc, sp);


			return enc;
		}

		public List<pdfdownloaddata> pdfdwnload()
		{
			List<pdfdownloaddata> enc = new List<pdfdownloaddata>();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "1" }
			};
			enc = _db.GetDataMultiple<pdfdownloaddata>("usp_downloadpdf", enc, sp);

			return enc;
		}
		public string encDecData(int UserId, string UserId_Enc,string Subscriber_Key,string decEmail,string decMobile,string decName)
		{
			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString()},
				new SetParameters{ ParameterName = "@UserId_Enc", Value = UserId_Enc},
				new SetParameters{ ParameterName = "@Subscriber_Key", Value = Subscriber_Key},
				new SetParameters{ ParameterName = "@DecEmail", Value = decEmail},
				new SetParameters{ ParameterName = "@DecName", Value = decName},
				new SetParameters{ ParameterName = "@DecMobile", Value = decMobile}
			};
			_db.StoreData("usp_EncDec", sp);


			return "Ok";
		}
		public string HPCouponBundleOffer()
		{
			GetStatus status = new GetStatus();
			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
				new SetParameters { ParameterName = "@Type", Value = "Coupon Bundling" }
			};
			_db.StoreData("usp_HPCouponBundleOffer", sp);

			return "ok";
		}

		public string HPBlogSaveViewerLog(string BlogId)
		{
			GetStatus status = new GetStatus();
			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@BlogItemId", Value = BlogId },
				new SetParameters { ParameterName = "@UserId", Value = UserId.ToString() }
			};
			_db.StoreData("[dbo].[USP_BlogViewedByUser]", sp);

			return "ok";
		}

		public List<PopularBlog> GetPopularBlog(int NoOfPopularBlogDisplayOnBlogListingPage)
		{
			List<PopularBlog> popularBlog = new List<PopularBlog>();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "1" },
				new SetParameters{ ParameterName = "@NoOfDisplay", Value = NoOfPopularBlogDisplayOnBlogListingPage.ToString() }
			};
			popularBlog = _db.GetDataMultiple<PopularBlog>("USP_GetDataBlog", popularBlog, sp);

			return popularBlog;
		}

		public ValidateUser ValidateUser(string email, string mobile)
		{
			ValidateUser validateMe = new ValidateUser();
			if (!String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(mobile))
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@email", Value = email },
				new SetParameters{ ParameterName = "@mobile", Value = mobile }
			};
				validateMe = _db.GetData("usp_ValidateUser", validateMe, sp);
			}

			return validateMe;
		}

		public MyProfile_Temp GetProfileByEmailMobile(string UserName)
		{
			MyProfile_Temp myprofile = new MyProfile_Temp();
			if (!String.IsNullOrWhiteSpace(UserName))
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@UserName", Value = UserName }
			};
				myprofile = _db.GetData("usp_getdata_With_EmailMobile", myprofile, sp);
			}

			return myprofile;
		}

		//Coupon code offer window
		public CouponCodeofferWindow CouponCodeWindowAuth(string CouponCode)
		{
			dbProxy _db = new dbProxy();
			CouponCodeofferWindow couponCodeofferWindow = new CouponCodeofferWindow();

			int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			int UserType = SessionManagement.GetCurrentSession<int>(SessionType.UserType);

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString()},
				new SetParameters{ ParameterName = "@UserType", Value = UserType.ToString()},
				new SetParameters{ ParameterName = "@CouponCode", Value = CouponCode == null ? "" : CouponCode }
			};

			couponCodeofferWindow = _db.GetData("USP_CouponCode_Discount_Appering_OrNot", couponCodeofferWindow, sp);

			return couponCodeofferWindow;
		}

		public SpecialRedirectionCheck CheckUserSpecialPlanRedeemed()
		{
			dbProxy _db = new dbProxy();
			SpecialRedirectionCheck specialRedirectionCheck = new SpecialRedirectionCheck();

			//int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
			if (!String.IsNullOrWhiteSpace(UserUniqueId))
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "1"},
				//new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString()}
				new SetParameters{ ParameterName = "@UserUniqueCode", Value = UserUniqueId}
			};

				specialRedirectionCheck = _db.GetData("USP_GetSpecialPlan", specialRedirectionCheck, sp);
			}

			return specialRedirectionCheck;
		}

		public List<MyDownloadsData> GetDownloads()
		{
			dbProxy _db = new dbProxy();
			List<MyDownloadsData> mydownloads = new List<MyDownloadsData>();

			string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);
			if (!String.IsNullOrWhiteSpace(UserUniqueId))
			{
				List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@QType", Value = "4"},
				new SetParameters{ ParameterName = "@UserUniqueCode", Value = UserUniqueId}
			};

				mydownloads = _db.GetDataMultiple<MyDownloadsData>("USP_GetSpecialPlan", mydownloads, sp);
			}

			return mydownloads;
		}
		public List<GetSpecialPlanData> SpecialPlanNotification()
		{
			List<GetSpecialPlanData> getSpecialPlanData = new List<GetSpecialPlanData>();

			try
			{
				dbProxy _db = new dbProxy();
				string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

				List<SetParameters> spwh = new List<SetParameters>()
									{
										new SetParameters { ParameterName = "@QType", Value = "3" },
										new SetParameters { ParameterName = "@UserUniqueCode", Value = UserUniqueId },
										new SetParameters { ParameterName = "@paymentId", Value = "" }
									};
				getSpecialPlanData = _db.GetDataMultiple<GetSpecialPlanData>("USP_GetSpecialPlan", getSpecialPlanData, spwh);
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "SpecialPlanNotification";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return getSpecialPlanData;
		}
		public GetSpecialPlanSubscriptionRportData GetSubscriptionDataSpecialPlan()
		{
			GetSpecialPlanSubscriptionRportData getSpecialPlanData = new GetSpecialPlanSubscriptionRportData();

			try
			{
				dbProxy _db = new dbProxy();
				string UserUniqueId = SessionManagement.GetCurrentSession<string>(SessionType.UserUniqueId);

				List<SetParameters> spwh = new List<SetParameters>()
									{
										new SetParameters { ParameterName = "@QType", Value = "5" },
										new SetParameters { ParameterName = "@UserUniqueCode", Value = UserUniqueId },
										new SetParameters { ParameterName = "@paymentId", Value = "" }
									};
				getSpecialPlanData = _db.GetData("USP_GetSpecialPlan", getSpecialPlanData, spwh);
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "GetSubscriptionDataSpecialPlan";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return getSpecialPlanData;
		}

		public BonusWorksheetDownloadEligibility DownloadEligibilityData(BonusDownloadParam Input)
		{
			BonusWorksheetDownloadEligibility bonusWorksheetDownloadEligibility = new BonusWorksheetDownloadEligibility();

			try
			{
				dbProxy _db = new dbProxy();
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

				if (UserId > 0)
				{
					List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters{ ParameterName = "@QType", Value = "1" },
							new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
							new SetParameters{ ParameterName = "@Source", Value = Input.Source == null ? "" : Input.Source },
							new SetParameters{ ParameterName = "@NodeId", Value = Input.NodeId == null ? "" : Input.NodeId }
						};

					bonusWorksheetDownloadEligibility = _db.GetData("USP_CalculationForWorksheetBonus", bonusWorksheetDownloadEligibility, sp);
				}
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "DownloadEligibilityData";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return bonusWorksheetDownloadEligibility;
		}

		public VideosBonus BonusVideosViewEligibility()
		{
			VideosBonus videosBonus = new VideosBonus();

			try
			{
				dbProxy _db = new dbProxy();
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

				if (UserId > 0)
				{
					List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters{ ParameterName = "@QType", Value = "2" },
							new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() }
						};

					videosBonus = _db.GetData("USP_CalculationForWorksheetBonus", videosBonus, sp);
				}
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "BonusVideosViewEligibility";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return videosBonus;
		}

		public string CheckGetStaredClicked(int refUserId)
		{
			dbProxy _db = new dbProxy();
			string status = string.Empty;
			try
			{
				string culture = CultureName.GetCultureName();
				if (String.IsNullOrEmpty(culture))
					culture = "en-US";

				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@UserId", Value = refUserId.ToString()}
					};
				status = _db.GetData("usp_lessonplancheckgetstarted", status, sp);
				return status;
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "CheckGetStaredClicked";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}
			return status;
		}

		public List<string> AllowSubscriptionsAgeGroupTeachers()
		{
			dbProxy _db = new dbProxy();
			List<string> allowAgegroup = new List<string>();

			List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters{ ParameterName = "@QType", Value = "1" }
				};

			allowAgegroup = _db.GetDataMultiple<string>("USP_AllowSubscriptionAgeGroupForTeachers", allowAgegroup, sp);


			return allowAgegroup;
		}

		public TeachersWorksheetDownloadEligibility DownloadEligibilityDataTeachers(TeachersDownloadParam Input)
		{
			TeachersWorksheetDownloadEligibility teachersWorksheetDownloadEligibility = new TeachersWorksheetDownloadEligibility();

			try
			{
				dbProxy _db = new dbProxy();
				int UserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);

				if (UserId > 0)
				{
					List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters{ ParameterName = "@QType", Value = "1" },
							new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
							new SetParameters{ ParameterName = "@agegroup", Value = Input.AgeGroup == null ? "" : Input.AgeGroup },
							new SetParameters{ ParameterName = "@Source", Value = Input.Source == null ? "" : Input.Source },
							new SetParameters{ ParameterName = "@NodeId", Value = Input.NodeId == null ? "" : Input.NodeId }
						};

					teachersWorksheetDownloadEligibility = _db.GetData("USP_CalculationForWorksheetTeachers", teachersWorksheetDownloadEligibility, sp);
				}
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "DownloadEligibilityData";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return teachersWorksheetDownloadEligibility;
		}

		public PostPayUser PostPayUserDtls_Teachers(string PaymentId)
		{
			PostPayUser postpay = new PostPayUser();
			string isloggedIn = String.Empty;
			isloggedIn = "N";
			dbProxy _db = new dbProxy();

			List<SetParameters> sp = new List<SetParameters>()
			{
				new SetParameters{ ParameterName = "@PaymentId", Value = PaymentId }
			};

			postpay = _db.GetData<PostPayUser>("usp_Data_PostPayment_Teachers", postpay, sp);

			return postpay;
		}

		public SubscriptionSuccessParam SetResponseFromPG_TeachersWorksheet(string responseStatus, string[] strSplitDecryptedResponse)
		{
			SubscriptionController subsCont = new SubscriptionController();
			SubscriptionSuccessParam returnParam = new SubscriptionSuccessParam();
			PaymentStatus paymentStatus = new PaymentStatus();
			string loinparamer = String.Empty;
			string PaymentId = String.Empty;

			try
			{
				if (strSplitDecryptedResponse != null && strSplitDecryptedResponse.Length > 0)
				{
					string[] strGetMerchantParamForCompare;
					for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
					{
						strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
						if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
						{
							string[] existsInformation = strGetMerchantParamForCompare[1].Split('}');
							if (existsInformation.Length >= 2)
							{
								for (int sub = 0; sub < existsInformation.Length; sub++)
								{
									string[] splitValue = existsInformation[sub].Split(':');
									if (splitValue[0].ToString().ToLower().Replace("{", "").Replace("}", "") == "custid")
									{
										paymentStatus.PaymentId = splitValue[1];

										break;
										//string[] subscribeId = existsInformation[2].Split(':');
										//if (subscribeId[0].Replace("{", "").Replace("}", "") == "custid")
										//{
										//	paymentStatus.SubscriptionId = subscribeId[1];
										//}
									}
								}
							}
						}
						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
							paymentStatus.txn_status = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_MSG")
							paymentStatus.txn_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_ERR_MSG")
							paymentStatus.txn_err_msg = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
							paymentStatus.clnt_txn_ref = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
							paymentStatus.tpsl_bank_cd = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
							paymentStatus.tpsl_txn_id = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
							paymentStatus.txn_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
							paymentStatus.tpsl_txn_time = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_RFND_ID")
							paymentStatus.tpsl_rfnd_id = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "BAL_AMT")
							paymentStatus.bal_amt = Convert.ToString(strGetMerchantParamForCompare[1]);

						else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "RQST_TOKEN")
							paymentStatus.rqst_token = Convert.ToString(strGetMerchantParamForCompare[1]);
					}

					PostPayUser postpay = new PostPayUser();

					try
					{
						postpay = PostPayUserDtls_Teachers(paymentStatus.PaymentId);
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass Bonus";
						error.MethodName = "PostPayUserDtls";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}

					try
					{
						paymentStatus.PaymentMode = "";
						returnParam = subsCont.PaymentAndSubscriptionInputTeachers(paymentStatus, postpay, "");
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "PaymentAndSubscriptionInput";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}

					try
					{
						//Exired Bundling Cookie here
						HttpContext.Current.Response.Cookies["IsBundleUser"].Expires = DateTime.Now.AddMinutes(-1);
						HttpContext.Current.Response.Cookies["IsOfferUser"].Expires = DateTime.Now.AddMinutes(-1);

						loinparamer = PostPayLoggedIn(paymentStatus.PaymentId, "TEACHERS");
					}
					catch (Exception ex)
					{
						ApplicationError error = new ApplicationError();
						error.PageName = "dbAccessClass";
						error.MethodName = "PostPayLoggedIn";
						error.ErrorMessage = ex.Message;

						dbAccessClass.PostApplicationError(error);
					}
				}
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "Root SetResponseFromPG Teachers";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}

			return returnParam;
		}

		public GetStatus GetStarted()
		{
			int RefUserId = SessionManagement.GetCurrentSession<int>(SessionType.UserId);
			dbProxy _db = new dbProxy();
			GetStatus status = new GetStatus();

			try
			{
				string culture = CultureName.GetCultureName();
				if (String.IsNullOrEmpty(culture))
					culture = "en-US";

				List<SetParameters> sp = new List<SetParameters>()
					{
						new SetParameters { ParameterName = "@UserId", Value = RefUserId.ToString()}
					};
				status = _db.GetData("usp_lessonplangetstartedLog", status, sp);
				return status;
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "dbAccessClass";
				error.MethodName = "GetStarted";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}
			return status;
		}
	}
}