using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;
using Umbraco.Web.PublishedModels;
using HPPlc.Controllers.APIs;
using Umbraco.Web;
using System.Configuration;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using HPPlc.Models.WhatsApp;

namespace HPPlc.Controllers
{
    [RoutePrefix("api/plcnotification")]
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class NotificationAPIController : UmbracoApiController
    {
		[Route("notificationcrmdata")]
		[HttpPost]
		public HttpResponseMessage NotificationCrmData()
		{
			WhatsAppApiResponse response = new WhatsAppApiResponse();
			HttpResponseMessage notificationData = new HttpResponseMessage();

			NotificationItem notificationItem = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
						.Where(x => x.ContentType.Alias == "notificationItem")?
						.OfType<NotificationItem>()?.Where(x => x.IsActive == true)?.FirstOrDefault();
			

			if (notificationItem != null)
			{
				response.StatusCode = 1;
				response.StatusMessage = "Done";
				response.Result = notificationItem;
				notificationData = Request.CreateResponse<WhatsAppApiResponse>(HttpStatusCode.OK, response);
			}

			return notificationData;
		}


		[Route("notificationcrmdataformaxlimit")]
		[HttpPost]
		public HttpResponseMessage NotificationCrmDataForMaxLimit(NotificationItem notificationItem,int NoOfWeek)
		{
			WhatsAppApiResponse response = new WhatsAppApiResponse();
			HttpResponseMessage notificationDataForMaxLimit = new HttpResponseMessage();

			NotificationItems notifItem = notificationItem?.NotificationMapping?
										.Where(x => Umbraco?.Content(x.SelectWeek.Udi)?.DescendantsOrSelf()?
										.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == NoOfWeek.ToString())?.OfType<NotificationItems>()?.FirstOrDefault();


			if (notifItem != null)
			{
				response.StatusCode = 1;
				response.StatusMessage = "Done";
				response.Result = notifItem;
				notificationDataForMaxLimit = Request.CreateResponse<WhatsAppApiResponse>(HttpStatusCode.OK, response);
			}

			return notificationDataForMaxLimit;
		}

		[Route("notificationworksheet")]
		[HttpPost]
		public HttpResponseMessage NotificationWorksheet(string MinAgegroup, int NoOfWeek)
		{
			WhatsAppApiResponse response = new WhatsAppApiResponse();
			HttpResponseMessage notificationWorksheet = new HttpResponseMessage();

			WorksheetRoot worksheet = Umbraco.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
								.Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?.OfType<WorksheetListingAgeWise>()?
								.Where(c => Umbraco?.Content(c?.AgeGroup?.Udi).DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == MinAgegroup)?.FirstOrDefault()?
								.DescendantsOrSelf()?.Where(b => b.ContentType.Alias == "worksheetRoot")?.OfType<WorksheetRoot>()?
								.Where(v => Umbraco?.Content(v.SelectWeek.Udi).DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == NoOfWeek.ToString())?
								.FirstOrDefault();

			if (worksheet != null)
			{
				response.StatusCode = 1;
				response.StatusMessage = "Done";
				response.Result = worksheet;
				notificationWorksheet = Request.CreateResponse<WhatsAppApiResponse>(HttpStatusCode.OK, response);
			}

			return notificationWorksheet;
		}

		[Route("notificationcrmdatawithsubject")]
		[HttpPost]
		public HttpResponseMessage NotificationCrmDataWithSubject(NotificationItem notificationItem, WorksheetRoot worksheet, int NoOfWeek)
		{
			WhatsAppApiResponse response = new WhatsAppApiResponse();
			HttpResponseMessage notificationDataWithSubject = new HttpResponseMessage();

			NotificationItems notifItem = notificationItem?.NotificationMapping?
										.Where(x => Umbraco?.Content(x.SelectWeek.Udi)?.DescendantsOrSelf()?.OfType<NameListItem>()?.FirstOrDefault()?.ItemValue == NoOfWeek.ToString() &&
										Umbraco?.Content(x?.NotificationSubjects?.Udi).DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue == Umbraco?.Content(worksheet.SelectSubject.Udi).DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault()?.SubjectValue)
										?.OfType<NotificationItems>()?.FirstOrDefault();

			if (notifItem != null)
			{
				response.StatusCode = 1;
				response.StatusMessage = "Done";
				response.Result = notifItem;
				notificationDataWithSubject = Request.CreateResponse<WhatsAppApiResponse>(HttpStatusCode.OK, response);
			}

			return notificationDataWithSubject;
		}
	}
}