using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HPPlc.Models.SMS
{
    public static class SmsSendAPI
    {
		//public static async Task<string> SendMessage(string mobile_no, string msg_temlate)
		//{
		//	string ms = string.Empty;
		//	string senderId = "HPIPLC";
		//	string dlt_peid = "1201159145101866123";
		//	string dlt_tmid = "1105161839567860000";
		//	string dlt_templateid = "1107161900265555121";
		//	string URL = $"http://whitelist.smsapi.org/SendSMS.aspx?UserName=Redcel&password=695252&MobileNo={mobile_no}&SenderID={senderId}&CDMAHeader=HPIPLC&Message={msg_temlate}&dlt_peid={dlt_peid}&dlt_tmid={dlt_tmid}&dlt_templateid={dlt_templateid}";
		//	//string URL = "http://hapi.smsapi.org/SendSMS.aspx?UserName=sms_tlg&password=772019&MobileNo=" + mobile_no + "&SenderID=Nissan&CDMAHeader=header&Message=" + msg_temlate;
		//	try
		//	{
		//		WebRequest req = WebRequest.Create(URL);
		//		WebResponse result = await req.GetResponseAsync();
		//		Stream recieveStream = result.GetResponseStream();
		//		System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
		//		StreamReader sr = new StreamReader(recieveStream, enc);
		//		ms = sr.ReadToEnd();

		//	}
		//	catch (Exception)
		//	{

		//	}
		//	return ms;
		//}


		public static async Task<string> SendMessage(string mobile_no, string msg_temlate)
		{
			string SMSAPIURL = ConfigurationManager.AppSettings["SMSAPIURL"].ToString();
			string SMSAPIKey = ConfigurationManager.AppSettings["SMSAPIKey"].ToString();
			string dlt_templateid = ConfigurationManager.AppSettings["dlt_templateid"].ToString();
			string dlt_peid = ConfigurationManager.AppSettings["dlt_peid"].ToString();

			string ms = string.Empty;
			SMSRoot root = new SMSRoot();
			try
			{
				//var options = new RestClientOptions();
				//options.Timeout = -1;
				var client = new RestClient();
				client.Timeout = -1;
				var request = new RestRequest(SMSAPIURL, Method.POST);
				request.AddHeader("apikey", SMSAPIKey);
				request.AddHeader("Content-Type", "application/json");
				root.msg_type = 2;
				root.unicode = false;
				SMSParameterList sMSParameterList = new SMSParameterList();
				sMSParameterList.d = mobile_no;
				sMSParameterList.s = "HPIPLC";//Change key
				sMSParameterList.m = msg_temlate;
				sMSParameterList.dlt_templateid = dlt_templateid;
				sMSParameterList.dlt_peid = dlt_peid;
				List<SMSParameterList> splist = new List<SMSParameterList>();
				splist.Add(sMSParameterList);
				root.list = splist;
				var body = JsonConvert.SerializeObject(root);
				request.AddJsonBody(body, "application/json");
				IRestResponse response = await client.ExecuteAsync(request);
				ms = response.Content;



				//var client = new HttpClient();
				//var request = new HttpRequestMessage(HttpMethod.Post, "https://app.helo.ai/vivasmpp/sms/submit");
				//var content = new StringContent("{\"unicode\":false,\"msg_type\":2,\"list\":[{\"d\":\"9899609319\",\"s\":\"HPCFIN\",\"m\":\"Your one time password is 874719.Your OTP will expire in next 15 Minutes. Team HP Consumer Finance.\",\"dlt_templateid\":\"1107168387843225828\",\"dlt_peid\":\"1201159145101866123\"}]}", null, "application/json");
				//request.Content = content;
				//var response = await client.SendAsync(request);
				//response.EnsureSuccessStatusCode();
				//Console.WriteLine(await response.Content.ReadAsStringAsync());
			}
			catch (Exception ex)
			{
				ApplicationError error = new ApplicationError();
				error.PageName = "SmsSendAPI";
				error.MethodName = "SendMessage";
				error.ErrorMessage = ex.Message;

				dbAccessClass.PostApplicationError(error);
			}
			return ms;
		}

	}
}

public class SMSParameterList
{
    public string d { get; set; }
    public string s { get; set; }
    public string m { get; set; }
    public string dlt_templateid { get; set; }
    public string dlt_peid { get; set; }
}

public class SMSRoot
{
    public bool unicode { get; set; }
    public int msg_type { get; set; }
    public List<SMSParameterList> list { get; set; }
}
