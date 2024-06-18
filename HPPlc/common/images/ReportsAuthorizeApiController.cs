using HPPlc.Models;
using HPPlc.Models.Constant;
using HPPlc.Models.Coupon;
using HPPlc.Models.FAQ;
using HPPlc.Models.ImportExcelFiles;
using HPPlc.Models.Reports;
using HPPlc.Models.ReportsHelper;
using HPPlc.Models.S3Buckets;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace HPPlc.Controllers
{
    public class ReportsAuthorizeApiController : UmbracoAuthorizedApiController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;
        public ReportsHelper reports;
        public string FolderName = "/CouponCodeExcelFile/";
        private static ImportExcelFilesHelper ImportExcelFilesHelper;
        private IUmbracoContextFactory _context;
        public ReportsAuthorizeApiController(IVariationContextAccessor variationContextAccessor, IUmbracoContextFactory context)
        {
            _context = context;
            ImportExcelFilesHelper = new ImportExcelFilesHelper(_context);
            reports = new ReportsHelper();
            _variationContextAccessor = variationContextAccessor;

        }
        #region Webinar Reports

        #endregion

        #region Registration Reports
        [System.Web.Http.HttpPost]
        public Responce GetAllRegistrationList(FilterInput filter)
        {
            return reports.GetAllRegistrationList(filter);
        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage RegistrationExportToExcel(string Search)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "1",
                    Search = Search == null ? "" : Search,
                };
                responce = reports.GetAllRegistrationList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsRegistrationReport> List = new List<clsRegistrationReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsRegistrationReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "RegistrationReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "RegistrationReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }
        #endregion

        #region Subscription Reports
        [System.Web.Http.HttpPost]
        public Responce GetAllSubscriptionList(FilterInput filter)
        {
            return reports.GetAllSubscriptionList(filter);
        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage SubscriptionExportToExcel(string Search)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "1",
                    Search = Search == null ? "" : Search,
                };
                responce = reports.GetAllSubscriptionList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsSubscriptionReport> List = new List<clsSubscriptionReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsSubscriptionReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "SubscriptionReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "SubscriptionReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }
        #endregion

        #region FAQ Reports
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage FAQRequestListExportToExcel(string Search, DateTime? StartDate, DateTime? EndDate, string Status)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                responce = reports.RequestList(Search, StartDate, EndDate, Status);
                if (responce != null && responce.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<FAQRequestModel> List = new List<FAQRequestModel>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<FAQRequestModel>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "FAQRequest");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "RequestList-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }
        [System.Web.Http.HttpGet]
        public Responce GetFAQRequestList(string Search, DateTime? StartDate, DateTime? EndDate, string Status, int itemsPerPage = 0, int Page = 0)
        {
            Responce responce = new Responce();
            try
            {
                responce = reports.RequestList(Search, StartDate, EndDate, Status, itemsPerPage, Page);
            }
            catch (Exception ex)
            {
                responce.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            return responce;
        }
        [System.Web.Http.HttpGet]
        public Responce GetFAQRequestList(int RequestId)
        {
            Responce responce = new Responce();
            try
            {
                responce = reports.GetRequestDetails(RequestId);
            }
            catch (Exception ex)
            {
                responce.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            return responce;
        }
        [System.Web.Http.HttpPost]
        public Responce PostFAQRequestSaveFile()
        {
            Responce responce = new Responce();
            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0 ?
       HttpContext.Current.Request.Files[0] : null;

                if (file != null && file.ContentLength > 0)
                {

                    responce = reports.SaveFAQRequestFeebBack(file.InputStream);

                }
                else
                {
                    responce.Message = "Please Select Excel File";
                    responce.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }

        #endregion FAQ

        #region User Login
        [System.Web.Http.HttpPost]
        public Responce UserLoginList(FilterInput input)
        {
            return reports.GetUserLoginList(input);
        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage DownloadUserLoginListExportToExcel(string Search, DateTime? StartDate, DateTime? EndDate, string Status)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "1",
                    Search = Search == null ? "" : Search,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Status = Status
                };
                responce = reports.GetUserLoginList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<UserLoginReport> List = new List<UserLoginReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<UserLoginReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "UserLoginDownload");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "UserLoginDownload-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }
        #endregion

        #region WorkSheet Download Reports 
        [System.Web.Http.HttpPost]

        public Responce WorksSheetDownloadList(FilterInput input)
        {
            return reports.WorksSheetDownloadList(input);
        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage DownloadWorksSheetListExportToExcel(string Search, DateTime? StartDate, DateTime? EndDate, string Status)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "1",
                    Search = Search == null ? "" : Search,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Status = Status
                };
                responce = reports.WorksSheetDownloadList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<WorkSheetDownloadReport> List = new List<WorkSheetDownloadReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<WorkSheetDownloadReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "WorksSheetDownload");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "WorksSheetDownload-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }
        #endregion

        #region Coupon Code 
        [System.Web.Http.HttpPost]
        public Responce CouponCodeList(FilterInput input)
        {
            return reports.CouponCodeList(input);
        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage CouponCodeListExportToExcel(string Search, DateTime? StartDate, DateTime? EndDate, string Status)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "1",
                    Search = Search == null ? "" : Search,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Status = Status
                };
                responce = reports.CouponCodeList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<CouponCode> List = new List<CouponCode>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<CouponCode>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "CouponCode");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "CouponCode-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }

        [System.Web.Http.HttpGet]
        public Responce GetCreateCouponCodeDetails()
        {
            return reports.GetCreateCouponCodeDetails();
        }
        [System.Web.Http.HttpPost]
        public Responce CreateEditCouponCode(CreateCouponCodeModel input)
        {
            if (input.TransactionId == null || string.IsNullOrWhiteSpace(input.TransactionId))
                input.TransactionId = clsCommon.GenerateTransactionId();
            Responce responce = new Responce();

            responce = reports.CreateEditCouponCode(input);
            if (responce != null && responce.StatusCode == HttpStatusCode.OK && input.CouponCodeId==0)
            {
                reports.SendEmailToAdminCouponCodeGenerated(ImportExcelFilesHelper.GetMailContentFromCMS(ConstantUserType.CouponCode), input.NoOfCouponCode, input.TransactionId);
            }
            
            return responce;
        }
        [System.Web.Http.HttpPost]
        public Responce PostCouponCodeSaveFile(int UserId)
        {
            Responce responce = new Responce();
            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0 ?
       HttpContext.Current.Request.Files[0] : null;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(FolderName)))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(FolderName));
                    var path = Path.Combine(
                        HttpContext.Current.Server.MapPath(FolderName),
                        fileName
                    );

                    file.SaveAs(path);
                    responce = reports.ImportCouponCodeFromExcelToTable(HttpContext.Current.Server.MapPath(FolderName), UserId);
                }
                else
                {
                    responce.Message = "Please Select Excel File";
                    responce.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        [System.Web.Http.HttpGet]
        public Responce GetCouponCodeEditDetails(int CouponCodeId)
        {
            return reports.GetCouponCodeEditDetails(CouponCodeId);
        }
        [System.Web.Http.HttpGet]
        public Responce CouponCodeStatusChange(int CouponCodeId, bool Status, int UserId)
        {
            return reports.CouponCodeStatusChange(CouponCodeId, Status, UserId);
        }
        [System.Web.Http.HttpDelete]
        public Responce CouponCodeDelete(string CouponCodeId, int UserId)
        {
            CouponCodeId = CouponCodeId.TrimEnd(',');
            return reports.CouponCodeDelete(CouponCodeId, UserId);
        }
        #endregion

        #region Coupon code Log
        [System.Web.Http.HttpPost]
        public Responce CouponCodeLogList(FilterInput input)
        {
            return reports.CouponCodeLogList(input);
        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage CouponCodeLogListExportToExcel(string Search, DateTime? StartDate, DateTime? EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "1",
                    Search = Search == null ? "" : Search,
                    StartDate = StartDate,
                    EndDate = EndDate,
                };
                responce = reports.CouponCodeLogList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<CouponCodeLog> List = new List<CouponCodeLog>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<CouponCodeLog>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "CouponCodeLog");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "CouponCodeLog-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }
        #endregion

    }
}