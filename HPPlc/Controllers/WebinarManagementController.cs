using HPPlc.Model;
using HPPlc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;

namespace HPPlc.Controllers
{
    public class WebinarManagementController : SurfaceController
    {
        // GET: WebinarManagement
        public ActionResult Index()
        {
            return View();
        }
        private readonly IVariationContextAccessor _variationContextAccessor;

        public WebinarManagementController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }

        public string GetCategory(string CultureInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            _variationContextAccessor.VariationContext = new VariationContext(CultureInfo);
            var vrCategoryNode = Umbraco.Content(1055);

            if (vrCategoryNode != null)
            {
                foreach (var item in vrCategoryNode.Children)
                {
                    if ((bool)item.Value("isActice") == true)
                    {
                        sb.Append("<option>" + item.Value("itemName") + "</option>");
                    }
                }
            }
            return sb.ToString();
        }
        public string GetAgeGroupe(string CultureInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            _variationContextAccessor.VariationContext = new VariationContext(CultureInfo);
            var vrAgeNode = Umbraco.Content(1197);

            if (vrAgeNode != null)
            {
                foreach (var item in vrAgeNode.Children)
                {
                    if ((bool)item.Value("isActice") == true)
                    {
                        sb.Append("<option>" + item.Value("itemName") + "</option>");
                    }
                }
            }
            return sb.ToString();
        }
        public string GetSubscriptions(string CultureInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            _variationContextAccessor.VariationContext = new VariationContext(CultureInfo);
            var vrSubscription = Umbraco.Content(1220);

            if (vrSubscription != null)
            {
                foreach (var item in vrSubscription.Children)
                {
                    if ((bool)item.Value("isActive") == true)
                    {
                        sb.Append("<option>" + item.Value("subscriptionName") + "</option>");
                    }
                }
            }
            return sb.ToString();
        }

		//public string GenerateZoomMeeting(string vLanguage, string vCategory, string vSubCategory
		//    , string vAgeGroupe, string vSubscriptionType, string vMeetingDate, string vMeetingTitle, string vMeetingUrl, string vMeetingAgenda, string vMeetingDuration, string vThumnailImage)
		//{
		//    string vRowId = "0";
		//    clsWebinarManagement _objclsWebinarManagement = new clsWebinarManagement();
		//    vRowId = _objclsWebinarManagement.Insert_WebinarDetails(vLanguage,vCategory,vSubCategory,vAgeGroupe,vSubscriptionType,vMeetingDate,vMeetingTitle,vMeetingUrl, vMeetingAgenda, vMeetingDuration, vThumnailImage);
		//    return vRowId;
		//}

		public string GenerateZoomMeeting(string vLanguage, string vCategory, string vSubCategory
	   , string vAgeGroupe, string vSubscriptionType, string vMeetingDate, string vMeetingTitle, string vMeetingUrl, string vMeetingAgenda, string vMeetingDuration, string vThumnailImage, string vAuthorName, string vWebinarId)
		{
			string vRowId = "0";
			clsWebinarManagement _objclsWebinarManagement = new clsWebinarManagement();
			IUser currentUser = null;
			var userTicket = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current).GetUmbracoAuthTicket();
			if (userTicket != null)
			{
				currentUser = Current.Services.UserService.GetByUsername(userTicket.Identity.Name);
			}
			if (Convert.ToInt32(vWebinarId) == 0)
				vRowId = _objclsWebinarManagement.Insert_WebinarDetails("1", vLanguage, vCategory, vSubCategory, vAgeGroupe, vSubscriptionType, vMeetingDate, vMeetingTitle, vMeetingUrl, vMeetingAgenda, vMeetingDuration, vThumnailImage, vAuthorName, vWebinarId);
			else
			{
				vRowId = _objclsWebinarManagement.Insert_WebinarDetails("2", vLanguage, vCategory, vSubCategory, vAgeGroupe, vSubscriptionType, vMeetingDate, vMeetingTitle, vMeetingUrl, vMeetingAgenda, vMeetingDuration, vThumnailImage, vAuthorName, vWebinarId);
				if (Convert.ToInt32(vRowId) > 0)
					_objclsWebinarManagement.Insert_WebinarLogDetails(vWebinarId, vLanguage, vCategory, vSubCategory, vAgeGroupe, vSubscriptionType, vMeetingDate, vMeetingTitle, vMeetingUrl, vMeetingAgenda, vMeetingDuration, vThumnailImage, vAuthorName, Convert.ToString(currentUser.Id));
			}
			return vRowId;
		}

		[HttpGet]
		public ActionResult GetWebinarsList(string filter)
		{
			Responce responce = new Responce();
			try
			{

				List<clsWebinarManagement> List = new List<clsWebinarManagement>();
				dbProxy _db = new dbProxy();

				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters{ ParameterName = "@QType", Value = "1" },
					new SetParameters{ ParameterName = "@WebinarId", Value = "0" },
					new SetParameters{ ParameterName = "@UserName", Value = "" },
					new SetParameters{ ParameterName = "@Query", Value = filter },
				};

				List = _db.GetDataMultiple("GetWebinarLists", List, sp);
				if (List != null && List.Count > 0)
				{
					foreach (var item in List)
					{
						item.id = item.id;
						item.Language = item.Language;
						item.Category = item.Category;
						item.MeetingDate = item.MeetingDate;
						item.MeetingTitle = item.MeetingTitle;
						item.MeetingUrl = item.MeetingUrl;
						item.MeetingDuration = item.MeetingDuration;
						item.MeetingAgenda = item.MeetingAgenda;
						item.AuthorName = item.AuthorName;
						item.ImageFile = item.ImageFile;
					}
				}
				responce.Result = List;
				responce.StatusCode = HttpStatusCode.OK;

			}
			catch (Exception ex)
			{
				responce.Result = null;
				responce.StatusCode = HttpStatusCode.InternalServerError;
			}
			//return returnMessage;
			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult DeleteWebinarByID(int id)
		{
			Responce responce = new Responce();
			IUser currentUser = null;

			var userTicket = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current).GetUmbracoAuthTicket();

			if (userTicket != null)
			{
				currentUser = Current.Services.UserService.GetByUsername(userTicket.Identity.Name);
			}

			try
			{

				GetStatus insertStatus = new GetStatus();
				dbProxy _db = new dbProxy();
				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters{ ParameterName = "@QType", Value = "2" },
					new SetParameters{ ParameterName = "@WebinarId", Value = id.ToString() },
					new SetParameters{ ParameterName = "@UserName", Value = currentUser.Id.ToString() },
				};

				insertStatus = _db.GetData<GetStatus>("GetWebinarLists", insertStatus, sp);

				responce.Result = "Success";
				responce.StatusCode = HttpStatusCode.OK;

			}
			catch (Exception ex)
			{
				responce.Result = null;
				responce.StatusCode = HttpStatusCode.InternalServerError;
			}
			//return returnMessage;
			return Json(responce, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetWebinarDetail(int webinarId)
		{
			Responce responce = new Responce();
			try
			{

				List<clsWebinarManagement> List = new List<clsWebinarManagement>();
				dbProxy _db = new dbProxy();
				List<SetParameters> sp = new List<SetParameters>()
				{
					new SetParameters{ ParameterName = "@QType", Value = "3" },
					new SetParameters{ ParameterName = "@WebinarId", Value = webinarId.ToString() },
					new SetParameters{ ParameterName = "@UserName", Value = "" },
				};

				List = _db.GetDataMultiple("GetWebinarLists", List, sp);
				if (List != null && List.Count > 0)
				{
					foreach (var item in List)
					{
						item.id = item.id;
						item.Language = item.Language;
						item.Category = item.Category;
						item.AgeGroupe = item.AgeGroupe;
						item.SubscriptionType = item.SubscriptionType;
						item.MeetingDate = item.MeetingDate;
						item.MeetingTitle = item.MeetingTitle;
						item.MeetingUrl = item.MeetingUrl;
						item.MeetingDuration = item.MeetingDuration;
						item.MeetingAgenda = item.MeetingAgenda;
						item.AuthorName = item.AuthorName;
						item.ImageFile = item.ImageFile;
					}
				}
				responce.Result = List;

				//responce.Result = "Success";
				responce.StatusCode = HttpStatusCode.OK;

			}
			catch (Exception ex)
			{
				responce.Result = null;
				responce.StatusCode = HttpStatusCode.InternalServerError;
			}
			//return returnMessage;
			return Json(responce, JsonRequestBehavior.AllowGet);
		}
	}
}