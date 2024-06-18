using HPPlc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.WebApi;

namespace HPPlc.Controllers
{
    public class ReportAuthorizeApiController : UmbracoAuthorizedApiController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;
        public ReportAuthorizeApiController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }

        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage RegistrationExportToExcel()
        {
            List<clsRegistrationReport> List = new List<clsRegistrationReport>();
            List<clsRegistrationReport> FianalList = new List<clsRegistrationReport>();
            dbProxy _db = new dbProxy();

            List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value = "" },
                };
            List = _db.GetDataMultiple("GetAllRegistration", List, sp);

			if (List != null && List.Count > 0)
			{
				for (int i = 0; i < List.Count; i++)
				{
					if (!String.IsNullOrEmpty(List[i].u_name))
						List[i].u_name = clsCommon.Decrypt(List[i].u_name);
					if (!String.IsNullOrEmpty(List[i].u_email))
						List[i].u_email = clsCommon.Decrypt(List[i].u_email);
					if (!String.IsNullOrEmpty(List[i].u_whatsappno))
						List[i].u_whatsappno = clsCommon.Decrypt(List[i].u_whatsappno);
				}
				//foreach (var item in List)
				//{
				//	if (!String.IsNullOrEmpty(item.u_name))
				//		item.u_name = clsCommon.Decrypt(item.u_name);
				//	if (!String.IsNullOrEmpty(item.u_email))
				//		item.u_email = clsCommon.Decrypt(item.u_email);
				//	if (!String.IsNullOrEmpty(item.u_whatsappno))
				//		item.u_whatsappno = (item.u_whatsappno != "" && item.u_whatsappno != null) ? clsCommon.Decrypt(item.u_whatsappno) : "";
				//	item.u_whatsappno_prefix = item.u_whatsappno_prefix;
				//	item.DOC = item.DOC;
				//	item.IsActive = item.IsActive;
				//	item.userUniqueId = item.userUniqueId;
				//	FianalList.Add(item);
				//}
			}

			clsExpertHelper clsExpertHelper = new clsExpertHelper();
            byte[] bytes = clsExpertHelper.ListToExcel(List, "Reports");

            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "RegistrationReports.xlsx";
            return result;

        }

        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage SubscriptionExportToExcel()
        {
            List<clsSubscriptionReport> List = new List<clsSubscriptionReport>();
            List<clsSubscriptionReport> FinalList = new List<clsSubscriptionReport>();
            dbProxy _db = new dbProxy();

            List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value = "" },
                };
            List = _db.GetDataMultiple("GetAllSubscriptionDetails", List, sp);
            if (List != null && List.Count > 0)
            {
                foreach (var item in List)
                {
					try
					{
						for (int i = 0; i < List.Count; i++)
						{
							if (!String.IsNullOrEmpty(List[i].u_name))
								List[i].u_name = clsCommon.Decrypt(List[i].u_name);
							if (!String.IsNullOrEmpty(List[i].u_email))
								List[i].u_email = clsCommon.Decrypt(List[i].u_email);
						}
					}
					catch { }
					//if (!String.IsNullOrEmpty(item.u_name))
					//	item.u_name = clsCommon.Decrypt(item.u_name);
					//if (!String.IsNullOrEmpty(item.u_email))
					//	item.u_email = clsCommon.Decrypt(item.u_email);
					//item.Ranking = item.Ranking;
					//               item.SubscriptionName = item.SubscriptionName;
					//               item.SubscriptionPrice = item.SubscriptionPrice;
					//               item.SubscriptionDuration = item.SubscriptionDuration;
					//               item.AgeGroup = item.AgeGroup;
					//               item.SubscriptionStartDate = item.SubscriptionStartDate;
					//               item.SubscriptionEndDate = item.SubscriptionEndDate;
					//               item.DOC = item.DOC;
					//               item.IsActive = item.IsActive;
					//               item.PaymentStatus = item.PaymentStatus;
					//               item.PaymentDate = item.PaymentDate;
					//               item.PaymentId = item.PaymentId;
					//               FinalList.Add(item);
				}
            }

            clsExpertHelper clsExpertHelper = new clsExpertHelper();
            byte[] bytes = clsExpertHelper.ListToExcel(List, "Reports");

            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "SubscriptionReports.xlsx";
            return result;

        }

        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage ExportToExcelReferralDetails()
        {
            List<clsReferralDetailReport> List = new List<clsReferralDetailReport>();
            List<clsReferralDetailReport> FinalList = new List<clsReferralDetailReport>();
            dbProxy _db = new dbProxy();

            List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value = "" },
                };
            List = _db.GetDataMultiple("GetAllReferralDetails", List, sp);

            if (List != null && List.Count > 0)
            {

                foreach (var item in List)
                {
					if (!String.IsNullOrEmpty(item.u_name))
						item.u_name = clsCommon.Decrypt(item.u_name);
					if (!String.IsNullOrEmpty(item.u_email))
						item.u_email = clsCommon.Decrypt(item.u_email);
					if (!String.IsNullOrEmpty(item.u_whatsappno))
						item.u_whatsappno = clsCommon.Decrypt(item.u_whatsappno);
					//item.u_name = clsCommon.Decrypt(item.u_name);
					//item.u_email = clsCommon.Decrypt(item.u_email);
					//item.u_whatsappno = clsCommon.Decrypt(item.u_whatsappno);
					item.u_whatsappno_prefix = item.u_whatsappno_prefix;
                    item.RefereeName = clsCommon.Decrypt(item.RefereeName);
                    item.RefereeEmail = clsCommon.Decrypt(item.RefereeEmail);
                    item.RefereeWNumber = clsCommon.Decrypt(item.RefereeWNumber);
                    item.RefereeWPrefix = item.RefereeWPrefix;
                    item.DOC = item.DOC;
                    item.IsActive = item.IsActive;
                    item.ReferrerCode = item.ReferrerCode;
                    FinalList.Add(item);
                }
            }

            clsExpertHelper clsExpertHelper = new clsExpertHelper();
            byte[] bytes = clsExpertHelper.ListToExcel(List, "Reprts");
           
            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "ReferralDetailsReports.xlsx";
            return result;

        }

        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage ExportToExcelReferralTransaction()
        {
            List<clsReferralDetailReport> List = new List<clsReferralDetailReport>();
            List<clsReferralDetailReport> FinalList = new List<clsReferralDetailReport>();
            dbProxy _db = new dbProxy();

            List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value = "" },
                };
            List = _db.GetDataMultiple("GetAllReferralTransactionsDetails", List, sp);
            List = _db.GetDataMultiple("GetAllReferralTransactionsDetails", List, sp);
            if (List != null && List.Count > 0)
            {

                foreach (var item in List)
                {
					if (!String.IsNullOrEmpty(item.u_name))
						item.u_name = clsCommon.Decrypt(item.u_name);
					if (!String.IsNullOrEmpty(item.u_email))
						item.u_email = clsCommon.Decrypt(item.u_email);
					if (!String.IsNullOrEmpty(item.u_whatsappno))
						item.u_whatsappno = clsCommon.Decrypt(item.u_whatsappno);
					//item.u_name = clsCommon.Decrypt(item.u_name);
					//item.u_email = clsCommon.Decrypt(item.u_email);
					//item.u_whatsappno = clsCommon.Decrypt(item.u_whatsappno);
					item.u_whatsappno_prefix = item.u_whatsappno_prefix;
                    item.RewardReferralInMonths = item.RewardReferralInMonths;
                    item.RewardReferralInDays = item.RewardReferralInDays;
                    item.StartDate = item.StartDate;
                    item.EndDate = item.EndDate;
                    item.DOC = item.DOC;
                    item.IsActive = item.IsActive;
                    item.ReferrerCode = item.ReferrerCode;
                    FinalList.Add(item);
                }
            }

            clsExpertHelper clsExpertHelper = new clsExpertHelper();
            byte[] bytes = clsExpertHelper.ListToExcel(FinalList, "Reprts");

            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "ReferralTransactionReports.xlsx";
            return result;

        }



    }
}