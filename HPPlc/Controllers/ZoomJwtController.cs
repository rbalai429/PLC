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
using Newtonsoft.Json;

namespace HPPlc.Controllers
{
    public class ZoomJwtController : SurfaceController
    {
        // GET: ZoomJwt
		[HttpGet]
		[Route("GetZoomAPI")]
        public ActionResult GetZoomAPI(string vMeetingTitle, string vMeetingDateTime, string vMeetingAgenda)
        {
            string[] vTemp = vMeetingDateTime.Split('T');
            string vFinalDate = vTemp[0] + " 00:00:00T" + vTemp[1];
            string tokenString = GetToken();
			//var client = new RestClient("https://api.zoom.us/v2/users?status=active&page_size=30&page_number=1");
			var client = new RestClient("https://api.zoom.us/v2/users/kumarashish.mca@gmail.com/meetings");
			var request = new RestRequest(Method.POST);
			request.AddHeader("content-type", "application/json");
			request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));
            //request.AddParameter("application/json", "{\"topic\" : \"TEST\",\"type\" : \"1\",\"start_time\" : \"2021-04-07 00:00:00T18:00:00Z\",\"timezone\":\"GMT\",\"agenda\":\"test\",\"settings\":{\"host_video\":\"true\",\"mute_upon_entry\":\"true\",\"approval_type\":\"1\",\"audio\":\"both\",\"auto_recording\":\"none\"} }", ParameterType.RequestBody);
            request.AddParameter("application/json", "{\"topic\" : \"'" + vMeetingTitle + "'\",\"type\" : \"1\",\"start_time\" : \"'" + vFinalDate + "'\",\"timezone\":\"GMT\",\"agenda\":\"'" + vMeetingAgenda + "'\",\"settings\":{\"host_video\":\"true\",\"mute_upon_entry\":\"true\",\"approval_type\":\"1\",\"audio\":\"both\",\"auto_recording\":\"none\"} }", ParameterType.RequestBody);
            //request.AddParameter("application/json", "{\"topic\" : '"+ vMeetingTitle + "',\"type\" : \"1\",\"start_time\" : '"+ vFinalDate + "',\"timezone\":\"GMT\",\"agenda\":'" + vMeetingTitle + "',\"settings\":{\"host_video\":\"true\",\"mute_upon_entry\":\"true\",\"approval_type\":\"1\",\"audio\":\"both\",\"auto_recording\":\"none\"} }", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
			var json = JsonConvert.DeserializeObject<ZoomApiForMeeting>(response.Content);

			return Json(new { status = json .join_url}, JsonRequestBehavior.AllowGet);
		}

		public string GetToken()
		{
			var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
			var now = DateTime.UtcNow;
			var apiSecret = "cJd8qlg62P6FqAayiCwh95JqJdWkk75HcP5Z";
			byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = "omEGwWJUSA6oXgikOd_A6A",
				SigningCredentials = new SigningCredentials(
									 new SymmetricSecurityKey(symmetricKey), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			var tokenString = tokenHandler.WriteToken(token);

			return tokenString.ToString();
		}

	}
}

public class ZoomApiForMeeting
{
	public string join_url { get; set; }
}