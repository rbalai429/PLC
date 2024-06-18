
using HPPlc.Controllers.APIs;
using HPPlc.Models;
using HPPlc.Models.HttpClientServices;
using HPPlc.Models.WebHook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace HPPlc.Controllers
{
	[RoutePrefix("api/webhooks")]
	public class WebHooksController : UmbracoApiController
	{
		string IsEnableSFMCCode = ConfigurationManager.AppSettings["IsEnableSFMCCode"].ToString();

		[HttpGet]
		[Route("checkAPI")]
		public async Task<string> checkAPI()
		{
			string capilaryTechLeadDataSent = "N";
			capilaryTechLeadData capilaryTechLeadData = new capilaryTechLeadData();
			capilaryTechResponse capilaryTechResponse = new capilaryTechResponse();

			string capilaryTechAPI = ConfigurationManager.AppSettings["capilaryTechAPI"].ToString();
			string capilaryTechSecret = ConfigurationManager.AppSettings["capilaryTechSecret"].ToString();

			var client_capilarytech = new HttpClient();
			var request_capilarytech = new HttpRequestMessage(HttpMethod.Post, capilaryTechAPI);
			request_capilarytech.Headers.Add("ClientSecret", capilaryTechSecret);

			capilaryTechLeadData.customerFullName = "Ashish test";
			capilaryTechLeadData.customerPhoneNumber = "918768768734";
			capilaryTechLeadData.emailId = "test@gmal.com";
			capilaryTechLeadData.pinCode = "273402";
			capilaryTechLeadData.comment = "Please follow-up with the lead";
			capilaryTechLeadData.sku = "facebook01";
			capilaryTechLeadData.campaign = "test";
			capilaryTechLeadData.leadSource = "87";

			string creditApplicationJson = JsonConvert.SerializeObject(capilaryTechLeadData);

			var content = new StringContent(creditApplicationJson, null, "application/json");
			request_capilarytech.Content = content;
			var response_capilarytech = await client_capilarytech.SendAsync(request_capilarytech);
			response_capilarytech.EnsureSuccessStatusCode();
			var capilaryTechResponseString = await response_capilarytech.Content.ReadAsStringAsync();

			capilaryTechResponse = JsonConvert.DeserializeObject<capilaryTechResponse>(capilaryTechResponseString);

			if (capilaryTechResponse != null && capilaryTechResponse.code == 1)
				capilaryTechLeadDataSent = "Y";
			else
				capilaryTechLeadDataSent = "N";

			return "done";
		}

		[HttpPost]
		[Route("getwebhookdata")]
		public async Task<HttpResponseMessage> getwebhookdata([FromBody] JObject entry)
		{
			string WebHookAccessToken = ConfigurationManager.AppSettings["WebHookAccessToken"].ToString();
			string IsEnableWebHookCode = ConfigurationManager.AppSettings["IsEnableWebHookCode"].ToString();
			string capilaryTechLeadDataSent = "N";
			dbProxy _db;

			try
			{
				if (!String.IsNullOrEmpty(IsEnableWebHookCode) && IsEnableWebHookCode == "Y" && entry != null)
				{
					//Json Backup
					_db = new dbProxy();
					List<SetParameters> spbkp = new List<SetParameters>()
							{
							new SetParameters{ ParameterName = "@data", Value = entry.ToString() },
							};

					_db.StoreData("usp_WebHookData_JsonBackup", spbkp);

					string leadId = String.Empty;
					string formId = String.Empty;
					//string Json = entry.ToString().Replace("{{", "{").Replace("}}", "}");
					//var leadGenId = JsonConvert.DeserializeObject<Entry>(Json);

					var leadnode = entry.SelectToken("$....leadgen_id");
					var formnode = entry.SelectToken("$....form_id");

					if (leadnode != null)
						leadId = leadnode.ToString().Replace("{", "").Replace("}", "");

					if (formnode != null)
						formId = formnode.ToString().Replace("{", "").Replace("}", "");

					if (!String.IsNullOrEmpty(leadId))
					{
						//var leadGenId = JsonConvert.DeserializeObject<Entry>(entry.ToString());
						//if (leadGenId != null)
						//{
						//	string leadId = leadGenId.changes.FirstOrDefault().Value.LeadGenId;

						//	if (!String.IsNullOrEmpty(leadId))
						//	{

						string facebookCallbackUrl = "https://graph.facebook.com/v16.0/" + leadId + "?access_token=" + WebHookAccessToken;

						var client = new HttpClient();
						var request = new HttpRequestMessage(HttpMethod.Get, facebookCallbackUrl);
						var response = await client.SendAsync(request);
						response.EnsureSuccessStatusCode();
						var responseData = await response.Content.ReadAsStringAsync();

						string facebookCallbackUrl_consent = "https://graph.facebook.com/v16.0/" + leadId + "?fields=custom_disclaimer_responses&access_token=" + WebHookAccessToken;

						var client_consent = new HttpClient();
						var request_consent = new HttpRequestMessage(HttpMethod.Get, facebookCallbackUrl_consent);
						var response_consent = await client_consent.SendAsync(request_consent);
						response_consent.EnsureSuccessStatusCode();
						var responseData_consent = await response_consent.Content.ReadAsStringAsync();

						if (responseData != null && responseData_consent != null)
						{
							var jsonObjLead = JsonConvert.DeserializeObject<LeadData>(responseData);
							var jsonObjLead_consent = JsonConvert.DeserializeObject<LeadData_Consent>(responseData_consent);

							if (jsonObjLead != null && jsonObjLead_consent != null)
							{
								HomeController homeController = new HomeController();
								Validate validate = new Validate();
								ValidateUser validateMe = new ValidateUser();
								dbAccessClass dbAccessClass = new dbAccessClass();
								Response callbackResponse = new Response();

								GetStatus insertStatus = new GetStatus();
								Registration Input = new Registration();
								_db = new dbProxy();

								string[] selectedAgeGroup = new string[1];
								string selectedPhoneNo = String.Empty;
								string emailformail = String.Empty;

								string ageGroup = jsonObjLead?.FieldData?.Where(x => x?.Name == "select_age_of_your_kid.")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();
								string email = jsonObjLead?.FieldData?.Where(x => x?.Name == "email")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();
								string you_are_a = jsonObjLead?.FieldData?.Where(x => x?.Name == "you_are_a?")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();
								string phone_number = jsonObjLead?.FieldData?.Where(x => x?.Name == "phone_number")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();
								string full_name = jsonObjLead?.FieldData?.Where(x => x?.Name == "full_name")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();

								string whatsappConsent = jsonObjLead_consent?.custom_disclaimer_responses?.Where(x => x?.checkbox_key == "whatsapp")?.FirstOrDefault().is_checked;
								string phoneConsent = jsonObjLead_consent?.custom_disclaimer_responses?.Where(x => x?.checkbox_key == "phone")?.FirstOrDefault().is_checked;
								string email_idConsent = jsonObjLead_consent?.custom_disclaimer_responses?.Where(x => x?.checkbox_key == "email_id")?.FirstOrDefault().is_checked;
								string hp_may_contact_me_with_personalized_offers = jsonObjLead_consent?.custom_disclaimer_responses?.Where(x => x?.checkbox_key == "hp_may_contact_me_with_personalized_offers,_support_updates_and_events_beyond_hp_print_learn_center")?.FirstOrDefault().is_checked;

								if (!String.IsNullOrEmpty(whatsappConsent) && whatsappConsent.ToLower() == "true")
									whatsappConsent = "Yes";
								else
									whatsappConsent = "No";

								if (!String.IsNullOrEmpty(phoneConsent) && phoneConsent.ToLower() == "true")
									phoneConsent = "Yes";
								else
									phoneConsent = "No";

								if (!String.IsNullOrEmpty(email_idConsent) && email_idConsent.ToLower() == "true")
									email_idConsent = "Yes";
								else
									email_idConsent = "No";

								if (!String.IsNullOrEmpty(hp_may_contact_me_with_personalized_offers) && hp_may_contact_me_with_personalized_offers.ToLower() == "true")
									hp_may_contact_me_with_personalized_offers = "1";
								else
									hp_may_contact_me_with_personalized_offers = "0";

								//phone_number = "9999973777";
								//email = "tehiv71142@fkcod.com";

								if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(phone_number))
								{
									if (!String.IsNullOrEmpty(ageGroup))
									{
										int start = ageGroup.IndexOf("(") + 1;
										int end = ageGroup.IndexOf(")", start);
										string result = ageGroup.Substring(start, end - start);

										if (!String.IsNullOrEmpty(result))
											ageGroup = result.Replace("_yrs", "");

										string[] ages = { "3-4", "4-5", "5-6", "6-7", "7-8", "8-9", "9-10", "10-11", "11-12", "12-13", "13-14" };
										bool ageExists = ages.Contains(ageGroup);

										if (ageExists == false)
											selectedAgeGroup = null;
										else
											selectedAgeGroup[0] = ageGroup;
									}

									if (!String.IsNullOrEmpty(phone_number))
									{
										phone_number = phone_number.Replace("+91", "");
										bool phoneIsCorrectFormat = validate.ValidateMobile(phone_number);

										if (phoneIsCorrectFormat == true)
										{
											selectedPhoneNo = clsCommon.Encrypt(phone_number);
										}
									}

									if (!String.IsNullOrWhiteSpace(full_name))
										full_name = clsCommon.Encrypt(full_name);

									if (validate.ValidateEmail(email) == true)
									{
										email = email.ToLower();
										emailformail = email;
										email = clsCommon.Encrypt(email);
									}

									validateMe = dbAccessClass.ValidateUser(email, phone_number);

									if (validateMe != null && validateMe.emailExists == 0 && validateMe.mobilenoExists == 0)
									{
										//Registration
										Input.name = full_name;
										Input.email = email;
										Input.mobileno = selectedPhoneNo;
										Input.ageGroup = selectedAgeGroup;
										Input.supportOnWhatsupFromHP = whatsappConsent;
										Input.supportOnPhoneFromHP = phoneConsent;
										Input.supportOnEmailFromHP = email_idConsent;
										Input.termsChecked = hp_may_contact_me_with_personalized_offers;
										Input.RuParentOrStudent = you_are_a;

										insertStatus = homeController.Register(Input, "webhook");
										if (insertStatus != null && insertStatus.returnStatus == "Success")
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
												Logger.Error(reporting: typeof(HomeController), ex, message: "registration webhook - SFMC Issue");
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
												Logger.Error(reporting: typeof(WebHooksController), ex, message: "Webhook - SFMC Issue");
											}

											//After login
											//string loginStatus = await homeController.PostRegistrationActivity("Bonus");

											//free subscription mail
											try
											{
												string isEnableEmail = ConfigurationManager.AppSettings["IsEnableEmail"].ToString();
												if (!String.IsNullOrWhiteSpace(isEnableEmail) && isEnableEmail == "Y")
												{
													homeController.Welcomemail(insertStatus.returnValue, emailformail);
												}
											}
											catch (Exception ex)
											{
												Logger.Error(reporting: typeof(WebHooksController), ex, message: "Webhook - mail");
											}

											//Third party send data
											if (!String.IsNullOrWhiteSpace(formId))
											{
												string allowedFormId = ConfigurationManager.AppSettings["webhookformid"].ToString();
												if (!String.IsNullOrWhiteSpace(allowedFormId))
												{
													List<String> formsId = new List<String>();
													string[] strFormIds = allowedFormId.Split(',');
													foreach (var forms in strFormIds)
													{
														formsId.Add(forms);
													}
													if (formsId != null && formsId.Count > 0)
													{

														if (formsId.Any(x => x.Equals(formId)))
														{
															try
															{
																capilaryTechLeadData capilaryTechLeadData = new capilaryTechLeadData();
																capilaryTechResponse capilaryTechResponse = new capilaryTechResponse();

																string capilaryTechAPI = ConfigurationManager.AppSettings["capilaryTechAPI"].ToString();
																string capilaryTechSecret = ConfigurationManager.AppSettings["capilaryTechSecret"].ToString();

																var client_capilarytech = new HttpClient();
																var request_capilarytech = new HttpRequestMessage(HttpMethod.Post, capilaryTechAPI);
																request_capilarytech.Headers.Add("ClientSecret", capilaryTechSecret);

																string uName = String.Empty;
																string uEmail = String.Empty;
																string uMobile = String.Empty;

																if (!String.IsNullOrWhiteSpace(full_name))
																	uName = clsCommon.Decrypt(full_name);

																if (!String.IsNullOrWhiteSpace(selectedPhoneNo))
																{
																	uMobile = clsCommon.Decrypt(selectedPhoneNo);
																	uMobile = "91" + uMobile;
																}

																if (!String.IsNullOrWhiteSpace(email))
																	uEmail = clsCommon.Decrypt(email);

																capilaryTechLeadData.customerFullName = uName;
																capilaryTechLeadData.customerPhoneNumber = uMobile;
																capilaryTechLeadData.emailId = uEmail;
																capilaryTechLeadData.pinCode = "273402";
																capilaryTechLeadData.comment = "Please follow-up with the lead";
																capilaryTechLeadData.sku = "facebook01";
																capilaryTechLeadData.campaign = "test";
																capilaryTechLeadData.leadSource = "87";

																string creditApplicationJson = JsonConvert.SerializeObject(capilaryTechLeadData);

																var content = new StringContent(creditApplicationJson, null, "application/json");
																request_capilarytech.Content = content;
																var response_capilarytech = await client_capilarytech.SendAsync(request_capilarytech);
																response_capilarytech.EnsureSuccessStatusCode();
																var capilaryTechResponseString = await response_capilarytech.Content.ReadAsStringAsync();

																capilaryTechResponse = JsonConvert.DeserializeObject<capilaryTechResponse>(capilaryTechResponseString);

																if (capilaryTechResponse != null && capilaryTechResponse.code == 1)
																	capilaryTechLeadDataSent = "Y";
																else
																	capilaryTechLeadDataSent = "N";
															}
															catch (Exception ex)
															{
																capilaryTechLeadDataSent = "N";
																Logger.Error(reporting: typeof(WebHooksController), ex, message: ex.StackTrace);
															}
														}
													}
												}
											}

											//Save in Db
											List<SetParameters> sp = new List<SetParameters>()
														{
															new SetParameters{ ParameterName = "@data", Value = entry.ToString() },
															new SetParameters{ ParameterName = "@EntryStatus", Value = "Data validated" },
															new SetParameters{ ParameterName = "@leadId", Value = leadId },
															new SetParameters{ ParameterName = "@FormId", Value = formId },
															new SetParameters{ ParameterName = "@capilaryTechLeadDataSent", Value = capilaryTechLeadDataSent == null ? "" : capilaryTechLeadDataSent }
														};

											_db.StoreData("usp_WebHookData", sp);

											callbackResponse.StatusCode = 1;
											callbackResponse.StatusMessage = "Done";
											callbackResponse.Result = "";

											return Request.CreateResponse<Response>(HttpStatusCode.OK, callbackResponse);
										}
										else
										{
											Logger.Info(reporting: typeof(WebHooksController), "Data not validated - Registration not success");

											List<SetParameters> sp = new List<SetParameters>()
											{
												new SetParameters{ ParameterName = "@data", Value = entry.ToString() },
												new SetParameters{ ParameterName = "@EntryStatus", Value = "Data not validated" },
												new SetParameters{ ParameterName = "@leadId", Value = leadId },
												new SetParameters{ ParameterName = "@FormId", Value = formId },
												new SetParameters{ ParameterName = "@capilaryTechLeadDataSent", Value = capilaryTechLeadDataSent == null ? "" : capilaryTechLeadDataSent }
											};

											_db.StoreData("usp_WebHookData", sp);


											return Request.CreateResponse<Response>(HttpStatusCode.BadRequest, null);
										}
									}
									else
									{
										Logger.Info(reporting: typeof(WebHooksController), "Data not validated - User email or mobile no already recorded");

										List<SetParameters> sp = new List<SetParameters>()
											{
												new SetParameters{ ParameterName = "@data", Value = entry.ToString() },
												new SetParameters{ ParameterName = "@EntryStatus", Value = "Data not validated" },
												new SetParameters{ ParameterName = "@leadId", Value = leadId },
												new SetParameters{ ParameterName = "@FormId", Value = formId },
												new SetParameters{ ParameterName = "@capilaryTechLeadDataSent", Value = capilaryTechLeadDataSent == null ? "" : capilaryTechLeadDataSent }
											};

										_db.StoreData("usp_WebHookData", sp);


										return Request.CreateResponse<Response>(HttpStatusCode.BadRequest, null);
									}
								}
							}
						}
						//	}
						//}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(reporting: typeof(WebHooksController), ex, message: ex.StackTrace);
			}

			return Request.CreateResponse<Response>(HttpStatusCode.BadRequest, null);


			//leadgen_id

			//try
			//{
			//	var entry = data.Entry.FirstOrDefault();
			//	var change = entry?.Changes.FirstOrDefault();

			//	if (change == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);

			//	const string token = "EAAG6f03ck9QBO9Tw1gAGRHYtMZBZAl9MfEQJQ4lZBRzZCEqZCZAP6a2EZAZBoj1ZANdcdhqMoYPgj2bx6GemBJHXKIoilzuzB4PnT5b0p0TsjmE3LnCWauYHusU3B0NbDfUJt8JjZAOZA5iAWZAEdUee7xFJ5NvCm3yHLjKVGV4RntziQ6rCKskZCn7syxwmghT3ZCT1akI5JVWsVVxUTwyHmSBKlckX34My23";

			//	var leadUrl = $"https://graph.facebook.com/v2.10/{change.Value.LeadGenId}?access_token=={token}";
			//	var formUrl = $"https://graph.facebook.com/v2.10/{change.Value.FormId}?access_token=={token}";

			//	using (var httpClientLead = new HttpClient())
			//	{
			//		var responses = await httpClientLead.GetStringAsync(formUrl);
			//		if (!String.IsNullOrEmpty(responses))
			//		{
			//			var jsonObjLead = JsonConvert.DeserializeObject<LeadFormData>(responses);

			//			using (var httpClientFields = new HttpClient())
			//			{
			//				var responseFields = await httpClientFields.GetStringAsync(leadUrl);
			//				if (!String.IsNullOrEmpty(responseFields))
			//				{
			//					var jsonObjFields = JsonConvert.DeserializeObject<LeadData>(responseFields);

			//					List<SetParameters> sp = new List<SetParameters>()
			//		{
			//			new SetParameters{ ParameterName = "@data", Value = jsonObjFields.ToString() }
			//		};

			//					_db.StoreData("usp_WebHookData", sp);
			//				}
			//			}
			//		}
			//	}
			//}
			//catch (Exception ex)
			//{
			//	//Logger.Error(reporting: typeof(WebHookController), ex, message: "getwebhookdata");
			//}

			//return new HttpResponseMessage(HttpStatusCode.OK);



			//var client = new HttpClient();
			//var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.facebook.com/122129071454060974/subscribed_apps?access_token=EAAG6f03ck9QBO6fvKw1tRbiZCCXQHOug7rsZBy2kAuQJOKAVMQ93SeBSW9drXc17ES2ZCkVSdHP9tTeGLZBPDrgtpqD6ZCsSZAH1EiZBQbR8Wf9YUljduLkOflQZADwSMOCZAdZBRbH6ZAXj8sou28NgIDWsCCPDfQH4orahSZC8vt4A9FADHL9hAEWzfk5PIJyr6XPpusiTY9kcVYjBxXKsSw803NdjtrAZD");
			//var response = await client.SendAsync(request);
			//response.EnsureSuccessStatusCode();
			//var responseData = await response.Content.ReadAsStringAsync();

			//List<SetParameters> sp = new List<SetParameters>()
			//	{
			//		new SetParameters{ ParameterName = "@data", Value = responseData }
			//	};

			//_db.StoreData("usp_WebHookData", sp);


			//string challenge = System.Web.HttpContext.Current.Request.QueryString["hub.challenge"].ToString();
			//Console.WriteLine(challenge);

			//Response callbackResponse = new Response();
			//callbackResponse.StatusCode = 1;
			//callbackResponse.StatusMessage = "Done";
			//callbackResponse.Result = "";

			//return Request.CreateResponse<Response>(HttpStatusCode.OK, callbackResponse);
		}
		//[HttpPost]
		//[Route("getwebhookdata")]
		//public async Task<HttpResponseMessage> Getwebhookdata([FromBody] JObject entry)
		//{
		//	string WebHookAccessToken = ConfigurationManager.AppSettings["WebHookAccessToken"].ToString();
		//	string IsEnableWebHookCode = ConfigurationManager.AppSettings["IsEnableWebHookCode"].ToString();

		//	try
		//	{
		//		if (!String.IsNullOrEmpty(IsEnableWebHookCode) && IsEnableWebHookCode == "Y" && entry != null)
		//		{
		//			var leadGenId = JsonConvert.DeserializeObject<Entry>(entry.ToString());
		//			if (leadGenId != null)
		//			{
		//				string leadId = leadGenId.changes.FirstOrDefault().Value.LeadGenId;

		//				if (!String.IsNullOrEmpty(leadId))
		//				{

		//					string facebookCallbackUrl = "https://graph.facebook.com/v16.0/" + leadId + "?access_token=" + WebHookAccessToken;

		//					var client = new HttpClient();
		//					var request = new HttpRequestMessage(HttpMethod.Get, facebookCallbackUrl);
		//					var response = await client.SendAsync(request);
		//					response.EnsureSuccessStatusCode();
		//					var responseData = await response.Content.ReadAsStringAsync();

		//					string facebookCallbackUrl_consent = "https://graph.facebook.com/v16.0/" + leadId + "?fields=custom_disclaimer_responses&access_token=" + WebHookAccessToken;

		//					var client_consent = new HttpClient();
		//					var request_consent = new HttpRequestMessage(HttpMethod.Get, facebookCallbackUrl_consent);
		//					var response_consent = await client_consent.SendAsync(request_consent);
		//					response_consent.EnsureSuccessStatusCode();
		//					var responseData_consent = await response_consent.Content.ReadAsStringAsync();

		//					if (responseData != null && responseData_consent != null)
		//					{
		//						var jsonObjLead = JsonConvert.DeserializeObject<LeadData>(responseData);
		//						var jsonObjLead_consent = JsonConvert.DeserializeObject<LeadData_Consent>(responseData_consent);

		//						if (jsonObjLead != null && jsonObjLead_consent != null)
		//						{
		//							HomeController homeController = new HomeController();
		//							Validate validate = new Validate();
		//							ValidateUser validateMe = new ValidateUser();
		//							dbAccessClass dbAccessClass = new dbAccessClass();
		//							Response callbackResponse = new Response();

		//							GetStatus insertStatus = new GetStatus();
		//							Registration Input = new Registration();
		//							dbProxy _db = new dbProxy();

		//							string[] selectedAgeGroup = new string[1];
		//							string selectedPhoneNo = String.Empty;
		//							string emailformail = String.Empty;

		//							string ageGroup = jsonObjLead?.FieldData?.Where(x => x?.Name == "select_age_of_your_kid.")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();
		//							string email = jsonObjLead?.FieldData?.Where(x => x?.Name == "email")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();
		//							string you_are_a = jsonObjLead?.FieldData?.Where(x => x?.Name == "you_are_a?")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();
		//							string phone_number = jsonObjLead?.FieldData?.Where(x => x?.Name == "phone_number")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();
		//							string full_name = jsonObjLead?.FieldData?.Where(x => x?.Name == "full_name")?.FirstOrDefault()?.Values?.FirstOrDefault().ToString();

		//							string whatsappConsent = jsonObjLead_consent?.custom_disclaimer_responses?.Where(x => x?.checkbox_key == "whatsapp")?.FirstOrDefault().is_checked;
		//							string phoneConsent = jsonObjLead_consent?.custom_disclaimer_responses?.Where(x => x?.checkbox_key == "phone")?.FirstOrDefault().is_checked;
		//							string email_idConsent = jsonObjLead_consent?.custom_disclaimer_responses?.Where(x => x?.checkbox_key == "email_id")?.FirstOrDefault().is_checked;
		//							string hp_may_contact_me_with_personalized_offers = jsonObjLead_consent?.custom_disclaimer_responses?.Where(x => x?.checkbox_key == "hp_may_contact_me_with_personalized_offers,_support_updates_and_events_beyond_hp_print_learn_center")?.FirstOrDefault().is_checked;

		//							if (!String.IsNullOrEmpty(whatsappConsent) && whatsappConsent.ToLower() == "true")
		//								whatsappConsent = "Yes";
		//							else
		//								whatsappConsent = "No";

		//							if (!String.IsNullOrEmpty(phoneConsent) && phoneConsent.ToLower() == "true")
		//								phoneConsent = "Yes";
		//							else
		//								phoneConsent = "No";

		//							if (!String.IsNullOrEmpty(email_idConsent) && email_idConsent.ToLower() == "true")
		//								email_idConsent = "Yes";
		//							else
		//								email_idConsent = "No";

		//							if (!String.IsNullOrEmpty(hp_may_contact_me_with_personalized_offers) && hp_may_contact_me_with_personalized_offers.ToLower() == "true")
		//								hp_may_contact_me_with_personalized_offers = "1";
		//							else
		//								hp_may_contact_me_with_personalized_offers = "0";

		//							//phone_number = "3690073623";
		//							//email = "binalag501@fahih.com";

		//							if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(phone_number))
		//							{
		//								if (!String.IsNullOrEmpty(ageGroup))
		//								{
		//									int start = ageGroup.IndexOf("(") + 1;
		//									int end = ageGroup.IndexOf(")", start);
		//									string result = ageGroup.Substring(start, end - start);

		//									if (!String.IsNullOrEmpty(result))
		//										ageGroup = result.Replace("_yrs", "");

		//									string[] ages = { "3-4", "4-5", "5-6", "6-7", "7-8", "8-9", "9-10", "10-11", "11-12", "12-13", "13-14" };
		//									bool ageExists = ages.Contains(ageGroup);

		//									if (ageExists == false)
		//										selectedAgeGroup = null;
		//									else
		//										selectedAgeGroup[0] = ageGroup;
		//								}

		//								if (!String.IsNullOrEmpty(phone_number))
		//								{
		//									phone_number = phone_number.Replace("+91", "");
		//									bool phoneIsCorrectFormat = validate.ValidateMobile(phone_number);

		//									if (phoneIsCorrectFormat == true)
		//									{
		//										selectedPhoneNo = clsCommon.Encrypt(phone_number);
		//									}
		//								}

		//								if (!String.IsNullOrWhiteSpace(full_name))
		//									full_name = clsCommon.Encrypt(full_name);

		//								if (validate.ValidateEmail(email) == true)
		//								{
		//									email = email.ToLower();
		//									emailformail = email;
		//									email = clsCommon.Encrypt(email);
		//								}

		//								validateMe = dbAccessClass.ValidateUser(email, phone_number);

		//								if (validateMe != null && validateMe.emailExists == 0 && validateMe.mobilenoExists == 0)
		//								{
		//									//Registration
		//									Input.name = full_name;
		//									Input.email = email;
		//									Input.mobileno = selectedPhoneNo;
		//									Input.ageGroup = selectedAgeGroup;
		//									Input.supportOnWhatsupFromHP = whatsappConsent;
		//									Input.supportOnPhoneFromHP = phoneConsent;
		//									Input.supportOnEmailFromHP = email_idConsent;
		//									Input.termsChecked = hp_may_contact_me_with_personalized_offers;
		//									Input.RuParentOrStudent = you_are_a;

		//									insertStatus = homeController.Register(Input, "webhook");
		//									if (insertStatus != null && insertStatus.returnStatus == "Success")
		//									{
		//										try
		//										{
		//											//SFMC Data Entry
		//											if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
		//											{
		//												SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
		//												sendDataToSFMC.PostDataSFMC(insertStatus.returnValue, "", "registration");
		//											}
		//										}
		//										catch (Exception ex)
		//										{
		//											Logger.Error(reporting: typeof(HomeController), ex, message: "registration webhook - SFMC Issue");
		//										}

		//										try
		//										{
		//											//SFMC Data Entry
		//											if (!String.IsNullOrWhiteSpace(insertStatus.returnMessage) && !String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
		//											{
		//												//Invite User
		//												SendDataToSFMC sendDataToSFMCt = new SendDataToSFMC();
		//												sendDataToSFMCt.PostDataSFMC(insertStatus.returnValue, "", "registrationInviteUser");
		//											}
		//										}
		//										catch (Exception ex)
		//										{
		//											Logger.Error(reporting: typeof(HomeController), ex, message: "registration - SFMC Issue");
		//										}

		//										//After login
		//										//string loginStatus = await homeController.PostRegistrationActivity("Bonus");

		//										try
		//										{
		//											//free subscription mail
		//											homeController.Welcomemail(insertStatus.returnValue, emailformail);
		//										}
		//										catch (Exception ex)
		//										{
		//											Logger.Error(reporting: typeof(HomeController), ex, message: "registration mailer - Webhook");
		//										}

		//										//Save in Db
		//										List<SetParameters> sp = new List<SetParameters>()
		//											{
		//												new SetParameters{ ParameterName = "@data", Value = entry.ToString() },
		//												new SetParameters{ ParameterName = "@EntryStatus", Value = "Data validated" }
		//											};

		//										_db.StoreData("usp_WebHookData", sp);

		//										callbackResponse.StatusCode = 1;
		//										callbackResponse.StatusMessage = "Done";
		//										callbackResponse.Result = "";

		//										return Request.CreateResponse<Response>(HttpStatusCode.OK, callbackResponse);
		//									}
		//									else
		//									{

		//										List<SetParameters> sp = new List<SetParameters>()
		//								{
		//									new SetParameters{ ParameterName = "@data", Value = entry.ToString() },
		//									new SetParameters{ ParameterName = "@EntryStatus", Value = "Data not validated" }
		//								};

		//										_db.StoreData("usp_WebHookData", sp);


		//										return Request.CreateResponse<Response>(HttpStatusCode.BadRequest, null);
		//									}
		//								}
		//								else
		//								{
		//									List<SetParameters> sp = new List<SetParameters>()
		//								{
		//									new SetParameters{ ParameterName = "@data", Value = entry.ToString() },
		//									new SetParameters{ ParameterName = "@EntryStatus", Value = "Data not validated" }
		//								};

		//									_db.StoreData("usp_WebHookData", sp);


		//									return Request.CreateResponse<Response>(HttpStatusCode.BadRequest, null);
		//								}
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error(reporting: typeof(WebHooksController), ex, message: "webhook - Getwebhookdata");
		//	}

		//	return Request.CreateResponse<Response>(HttpStatusCode.BadRequest, null);

		//}
		//	[HttpPost]
		//	[Route("getwebhookdata")]
		//	public HttpResponseMessage getwebhookdata([FromBody] JObject entry)
		//	{
		//		//JObject obj = JObject.Parse(entry);
		//		dbProxy _db = new dbProxy();

		//		List<SetParameters> sp = new List<SetParameters>()
		//			{
		//				new SetParameters{ ParameterName = "@data", Value = entry.ToString() }
		//			};

		//		_db.StoreData("usp_WebHookData", sp);

		//		Response callbackResponse = new Response();
		//		callbackResponse.StatusCode = 1;
		//		callbackResponse.StatusMessage = "Done";
		//		callbackResponse.Result = "";

		//		return Request.CreateResponse<Response>(HttpStatusCode.OK, callbackResponse);
		//	}
		//}

		//[Route("GetFbwebhook")]
		//[HttpPost]
		//public HttpResponseMessage GetFbwebhook([FromBody] JsonData data)
		//{
		//	var entry = data.Entry.FirstOrDefault();
		//	var change = entry?.Changes.FirstOrDefault();

		//	if (change == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);

		//	Console.Write(data);

		//	return new HttpResponseMessage(HttpStatusCode.OK);
		//}

		//[HttpPost]
		//[Route("webhook")]
		//public HttpResponseMessage webhook()
		//{
		//	// Process the incoming event payload
		//	// Save the event to the database
		//	string challenge = System.Web.HttpContext.Current.Request.QueryString["hub.challenge"].ToString();
		//	Console.Write(challenge);

		//	return new HttpResponseMessage(HttpStatusCode.OK);
		//}

		//public HttpResponseMessage Webhook([Microsoft.AspNetCore.Mvc.FromQuery] string hub_challenge)
		//{
		//	//var response = new HttpResponseMessage(HttpStatusCode.OK)
		//	//{
		//	//	Content = new StringContent(System.Web.HttpContext.Current.Request.QueryString["hub_challenge"])
		//	//};
		//	//response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
		//	Response.Write(hub_challenge);

		//	return new HttpResponseMessage(HttpStatusCode.OK);
		//}
		//[HttpPost]
		//public ActionResult getwebhookdata()
		//{
		//	string challenge = System.Web.HttpContext.Current.Request.QueryString["hub.challenge"].ToString();
		//	System.Web.HttpContext.Current.Response.Write(challenge);

		//	return View();
		//	//Response callbackResponse = new Response();
		//	//callbackResponse.StatusCode = 1;
		//	//callbackResponse.StatusMessage = "Done";
		//	//callbackResponse.Result = "";

		//	//return Request.CreateResponse<Response>(HttpStatusCode.OK, callbackResponse);
		//}

		//#region Get Request
		//[Route("getwebhookdata")]
		//[HttpPost]
		//public HttpResponseMessage getwebhookdata()
		//{
		//	var response = new HttpResponseMessage(HttpStatusCode.OK)
		//	{
		//		Content = new StringContent(System.Web.HttpContext.Current.Request.QueryString["hub.challenge"])
		//	};
		//	response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
		//	return response;
		//}
		//#endregion Get Request

		//#region Post Request

		//[HttpPost]
		//public HttpResponseMessage Post(Feed data)
		//{
		//	try
		//	{
		//		//You got the data do whatever you want here!!!Happy programming!!
		//		return new HttpResponseMessage(HttpStatusCode.OK);
		//	}
		//	catch (Exception ex)
		//	{
		//		return new HttpResponseMessage(HttpStatusCode.BadGateway);
		//	}
		//}

		//#endregion

		//[HttpPost]
		//[Route("getwebhookdata")]
		//public HttpResponseMessage getwebhookdata()
		//{
		//	//dbProxy _db = new dbProxy();

		//	//try
		//	//{
		//	//	var entry = data.Entry.FirstOrDefault();
		//	//	var change = entry?.Changes.FirstOrDefault();

		//	//	if (change == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);

		//	//	const string token = "EAAG6f03ck9QBO9Tw1gAGRHYtMZBZAl9MfEQJQ4lZBRzZCEqZCZAP6a2EZAZBoj1ZANdcdhqMoYPgj2bx6GemBJHXKIoilzuzB4PnT5b0p0TsjmE3LnCWauYHusU3B0NbDfUJt8JjZAOZA5iAWZAEdUee7xFJ5NvCm3yHLjKVGV4RntziQ6rCKskZCn7syxwmghT3ZCT1akI5JVWsVVxUTwyHmSBKlckX34My23";

		//	//	var leadUrl = $"https://graph.facebook.com/v2.10/{change.Value.LeadGenId}?access_token=={token}";
		//	//	var formUrl = $"https://graph.facebook.com/v2.10/{change.Value.FormId}?access_token=={token}";

		//	//	using (var httpClientLead = new HttpClient())
		//	//	{
		//	//		var responses = await httpClientLead.GetStringAsync(formUrl);
		//	//		if (!String.IsNullOrEmpty(responses))
		//	//		{
		//	//			var jsonObjLead = JsonConvert.DeserializeObject<LeadFormData>(responses);

		//	//			using (var httpClientFields = new HttpClient())
		//	//			{
		//	//				var responseFields = await httpClientFields.GetStringAsync(leadUrl);
		//	//				if (!String.IsNullOrEmpty(responseFields))
		//	//				{
		//	//					var jsonObjFields = JsonConvert.DeserializeObject<LeadData>(responseFields);

		//	//					List<SetParameters> sp = new List<SetParameters>()
		//	//		{
		//	//			new SetParameters{ ParameterName = "@data", Value = jsonObjFields.ToString() }
		//	//		};

		//	//					_db.StoreData("usp_WebHookData", sp);
		//	//				}
		//	//			}
		//	//		}
		//	//	}
		//	//}
		//	//catch (Exception ex)
		//	//{
		//	//	//Logger.Error(reporting: typeof(WebHookController), ex, message: "getwebhookdata");
		//	//}

		//	//return new HttpResponseMessage(HttpStatusCode.OK);



		//	//var client = new HttpClient();
		//	//var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.facebook.com/122129071454060974/subscribed_apps?access_token=EAAG6f03ck9QBO6fvKw1tRbiZCCXQHOug7rsZBy2kAuQJOKAVMQ93SeBSW9drXc17ES2ZCkVSdHP9tTeGLZBPDrgtpqD6ZCsSZAH1EiZBQbR8Wf9YUljduLkOflQZADwSMOCZAdZBRbH6ZAXj8sou28NgIDWsCCPDfQH4orahSZC8vt4A9FADHL9hAEWzfk5PIJyr6XPpusiTY9kcVYjBxXKsSw803NdjtrAZD");
		//	//var response = await client.SendAsync(request);
		//	//response.EnsureSuccessStatusCode();
		//	//var responseData = await response.Content.ReadAsStringAsync();

		//	//List<SetParameters> sp = new List<SetParameters>()
		//	//	{
		//	//		new SetParameters{ ParameterName = "@data", Value = responseData }
		//	//	};

		//	//_db.StoreData("usp_WebHookData", sp);


		//	string challenge = System.Web.HttpContext.Current.Request.QueryString["hub.challenge"].ToString();
		//	Console.WriteLine(challenge);

		//	Response callbackResponse = new Response();
		//	callbackResponse.StatusCode = 1;
		//	callbackResponse.StatusMessage = "Done";
		//	callbackResponse.Result = "";

		//	return Request.CreateResponse<Response>(HttpStatusCode.OK, callbackResponse);
		//}
	}
}


public class capilaryTechLeadData
{
	public string customerFullName { get; set; }
	public string customerPhoneNumber { get; set; }
	public string emailId { get; set; }
	public string pinCode { get; set; }
	public string comment { get; set; }
	public string sku { get; set; }
	public string campaign { get; set; }
	public string leadSource { get; set; }
}

public class capilaryTechResponse
{
	public int code { get; set; }
	public string message { get; set; }
}