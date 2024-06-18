using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace HPPlc.Models.HPUId
{
	public class HPIDAPIEnvironment
	{
		string redirectURL = String.Empty;
		string HPIDAPIClientID = ConfigurationManager.AppSettings["HPIDAPIClientID"];
		string HPIDAPISecret = ConfigurationManager.AppSettings["HPIDAPISecret"];
		static string HPIDUrl = ConfigurationManager.AppSettings["HPIDUrl"];
		string ApiRedirectURL = ConfigurationManager.AppSettings["ApiRedirectURL"];
		string ApiLogoutRedirectURL = ConfigurationManager.AppSettings["ApiLogoutRedirectURL"];

		string UrlForAccessToken = HPIDUrl + "/directory/v1/oauth/token";
		string URLLoginUserApi = HPIDUrl + "/directory/v1/scim/v2/Me";

		public string GetLoginRedirectURL(string email)
		{
			//for user login -- &login_hint=hitesh.kumar@digitas.com&target=password&allow_return=true (as per user redirect login or Signup)
			//for user Forget password -- &login_hint=hitesh.kumar@digitas.com&target=forgot-password&allow_return=true
			//for user signup -- &login_hint=hitesh.kumar@digitas.com&target=create&allow_return=true (not recommended by HP always go to singup page)
			// for change password -- https://account.stg.cd.id.hp.com/change-password
			// for My account change -- https://account.stg.cd.id.hp.com

			redirectURL = HPIDUrl + "/directory/v1/oauth/authorize?response_type=code&client_id=" + HPIDAPIClientID + "&redirect_uri=" + ApiRedirectURL + "&scope=user.profile.read&login_hint=" + email + "&target=password&allow_return=true";
			return redirectURL;
		}

		public string GetLogOutRedirectURL()
		{
			redirectURL = HPIDUrl + "/directory/v1/oauth/logout?post_logout_redirect_uri=" + ApiLogoutRedirectURL;
			return redirectURL;
		}
		public string GetAccess_Token(string URLCode)
		{
			try
			{
				string HPIDAuthorizationKey = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(HPIDAPIClientID + ":" + HPIDAPISecret));

				var myUri = new Uri(UrlForAccessToken);
				var myWebRequest = WebRequest.Create(myUri);
				var myHttpWebRequest = (HttpWebRequest)myWebRequest;
				myHttpWebRequest.Method = "POST";
				myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
				myHttpWebRequest.PreAuthenticate = true;
				myHttpWebRequest.Headers.Add("Authorization", "Basic " + HPIDAuthorizationKey);
				myHttpWebRequest.Accept = "application/json";

				Stream reqStream = myWebRequest.GetRequestStream();
				string postData = "grant_type=authorization_code&redirect_uri=" + ApiRedirectURL + "&code=" + URLCode;
				byte[] postArray = Encoding.ASCII.GetBytes(postData);
				reqStream.Write(postArray, 0, postArray.Length);
				reqStream.Close();

				var myWebResponse = myWebRequest.GetResponse();
				var responseStream = myWebResponse.GetResponseStream();
				if (responseStream == null) return null;

				var myStreamReader = new StreamReader(responseStream, Encoding.Default);
				var jsonresponse = myStreamReader.ReadToEnd();

				AccessToken accessToken = new AccessToken();
				accessToken = JsonConvert.DeserializeObject<AccessToken>(jsonresponse);

				responseStream.Close();
				myWebResponse.Close();

				return accessToken.token_type.ToString() + " " + accessToken.access_token.ToString();
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "HPIDAPIEnvironment";
				error.MethodName = "GetAccess_Token";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);

				return "";
			}
		}

		public IRestResponse GetAccess_LoginUserInfo(string URLCode)
		{
			try
			{
				var client = new RestClient(URLLoginUserApi);
				client.Timeout = -1;
				var request = new RestRequest(Method.GET);
				request.AddHeader("Content-Type", "application/json");
				string token = GetAccess_Token(URLCode);
				string Authorization = token;
				request.AddHeader("Authorization", Authorization);

				string jsonString = "";
				request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
				IRestResponse response = client.Execute(request);

				return response;
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "HPIDAPIEnvironment";
				error.MethodName = "GetAccess_LoginUserInfo";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
				IRestResponse response = null;
				return response;
			}
		}
	}

	public class HPApiErrorResponse
	{
		public string[] schemas { get; set; }
		public string status { get; set; }
		public string detail { get; set; }
	}

	public class AccessToken
	{
		public string scope { get; set; }
		public string access_token { get; set; }
		public string token_type { get; set; }
		public string expires_in { get; set; }
	}

	public class LoginUserInfoResponce
	{
		public string id { get; set; }
		public Meta meta { get; set; }
		public string[] schemas { get; set; }
		public string countryResidence { get; set; }
		public List<Emails> emails { get; set; }
		public List<PhoneNumber> phoneNumbers { get; set; }
		public string enabled { get; set; }
		public ExtendedMeta extendedMeta { get; set; }
		public string legalZone { get; set; }
		public string locale { get; set; }
		public Name name { get; set; }
		public string type { get; set; }
		public string userName { get; set; }

	}
	public class UserHpIdDetails
	{
		public string HPiD { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Mobile { get; set; }
	}
	public class Meta
	{
		public string resourceType { get; set; }
		public string created { get; set; }
		public string lastModified { get; set; }
		public string version { get; set; }
		public string location { get; set; }
	}

	public class Emails
	{
		public string accountRecovery { get; set; }
		public string primary { get; set; }
		public string type { get; set; }
		public string value { get; set; }
		public string verified { get; set; }
	}

	public class PhoneNumber
	{
		public string number { get; set; }
		public string areaCode { get; set; }
		public string countryCode { get; set; }
		public string type { get; set; }
		public bool primary { get; set; }
		public bool verified { get; set; }
		public string id { get; set; }
		public string accountRecovery { get; set; }
	}
	public class ExtendedMeta
	{
		public string createdByClient { get; set; }
		public string lastModifiedByClient { get; set; }
		public string lastModifiedByUser { get; set; }
	}

	public class Name
	{
		public string familyName { get; set; }
		public string givenName { get; set; }
		public string honorificSuffix { get; set; }
		public string middleName { get; set; }
	}
}