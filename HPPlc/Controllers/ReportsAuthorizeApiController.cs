using DocumentFormat.OpenXml.Spreadsheet;
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
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using ClosedXML.Excel;
using System.Xml.Linq;
using HPPlc.Models.BulkUpload;
using System.Globalization;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedModels;
using Umbraco.Core.Services;
using HP_PLC_Doc.Controllers;
using System.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using NPoco.Expressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Text;

namespace HPPlc.Controllers
{
    public class ReportsAuthorizeApiController : UmbracoAuthorizedApiController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;
        public ReportsHelper reports;
        public string FolderName = "/ExcelFile/CouponCodeExcelFile/";
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
            //return reports.GetAllRegistrationList(filter);
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetAllRegistrationList(filter);
            }
            else if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.StartDate = null;
                filter.EndDate = null;
                return reports.GetAllRegistrationList(filter);
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage RegistrationExportToExcel(string Search, string StartDate, string EndDate)
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
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
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
            //return reports.GetAllSubscriptionList(filter);
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetAllSubscriptionList(filter);
            }
            else if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.StartDate = null;
                filter.EndDate = null;
                return reports.GetAllSubscriptionList(filter);
            }
            else
            {
                return null;
            }
        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage SubscriptionExportToExcel(string Search, string StartDate, string EndDate)
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
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
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
            if (responce != null && responce.StatusCode == HttpStatusCode.OK && input.CouponCodeId == 0)
            {
                reports.SendEmailToAdminCouponCodeGenerated(ImportExcelFilesHelper.GetMailContentFromCMS(ConstantUserType.CouponCode), input.NoOfCouponCode, input.TransactionId);
            }

            return responce;
        }

        [System.Web.Http.HttpPost]
        public Responce CreateEditCouponCodeExcelCouponFileSave()
        {
            //HttpFileCollection files = HttpContext.Current.Request.Files;
            Responce responce = new Responce();
            var file = HttpContext.Current.Request.Files.Count > 0 ?
                       HttpContext.Current.Request.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                // Delete all files in a directory
                DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(FolderName));
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("/ExcelFile")))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("ExcelFile"));
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("/ExcelFile/CouponCodeExcelFile")))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/ExcelFile/") + "CouponCodeExcelFile");

                if (dir.GetFiles("*").Any())
                {
                    foreach (FileInfo existingfile in dir.GetFiles())
                    {
                        existingfile.Delete();
                    }
                }
                var fileName = DateTime.Now.ToString("dd-MM-yyyy") + Path.GetFileName(file.FileName);

                var path = Path.Combine(
                    HttpContext.Current.Server.MapPath(FolderName),
                    fileName
                );

                file.SaveAs(path);
                responce.Message = "Success";
                responce.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                responce.Message = "Please Select Excel File";
                responce.StatusCode = HttpStatusCode.NotFound;
            }

            return responce;
        }

        [System.Web.Http.HttpPost]
        public Responce CreateEditCouponCodeExcelCoupon(CreateCouponCodeModel input)
        {
            Responce responce = new Responce();

            if (Directory.Exists(HttpContext.Current.Server.MapPath(FolderName)) && Directory.GetFiles(HttpContext.Current.Server.MapPath(FolderName), "*", SearchOption.AllDirectories).Length == 1)
            {
                var files = Directory.EnumerateFiles(HttpContext.Current.Server.MapPath(FolderName), "*.*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".csv") || s.EndsWith(".xlsx") || s.EndsWith(".xls"));

                if (!String.IsNullOrWhiteSpace(files.ToString()))
                {
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();

                    dt = ImportExcelFilesHelper.ConvertCSVtoDataTable(files.FirstOrDefault());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        input.filetype = "coupon";
                        ds.Tables.Add(dt);
                        string XmlParameter = ds.GetXml();
                        input.fileupload = XmlParameter;
                        input.NoOfCouponCode = dt.Rows.Count;

                        if (input.TransactionId == null || string.IsNullOrWhiteSpace(input.TransactionId))
                            input.TransactionId = clsCommon.GenerateTransactionId();

                        responce = reports.InsertCouponCodeFromExcel(input);
                        if (responce != null && responce.StatusCode == HttpStatusCode.OK && input.CouponCodeId == 0)
                        {
                            try
                            {
                                DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(FolderName));
                                if (dir.GetFiles("*").Any())
                                {
                                    foreach (FileInfo existingfile in dir.GetFiles())
                                    {
                                        existingfile.Delete();
                                    }
                                }
                            }
                            catch { }

                            reports.SendEmailToAdminCouponCodeGenerated(ImportExcelFilesHelper.GetMailContentFromCMS(ConstantUserType.CouponCode), input.NoOfCouponCode, input.TransactionId);
                        }
                    }
                }
            }
            else
            {
                responce.Message = "Please Select Excel File";
                responce.StatusCode = HttpStatusCode.NotFound;
            }

            return responce;
        }

        [System.Web.Http.HttpPost]
        public Responce CreateEditCouponCodeExcelSerial(CreateCouponCodeModel input)
        {
            Responce responce = new Responce();

            if (Directory.Exists(HttpContext.Current.Server.MapPath(FolderName)) && Directory.GetFiles(HttpContext.Current.Server.MapPath(FolderName), "*", SearchOption.AllDirectories).Length == 1)
            {
                var files = Directory.EnumerateFiles(HttpContext.Current.Server.MapPath(FolderName), "*.*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".csv") || s.EndsWith(".xlsx") || s.EndsWith(".xls"));

                if (!String.IsNullOrWhiteSpace(files.ToString()))
                {
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();

                    dt = ImportExcelFilesHelper.ConvertCSVtoDataTable(files.FirstOrDefault());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        input.filetype = "serial";
                        ds.Tables.Add(dt);
                        string XmlParameter = ds.GetXml();
                        input.fileupload = XmlParameter;
                        input.NoOfCouponCode = dt.Rows.Count;

                        if (input.TransactionId == null || string.IsNullOrWhiteSpace(input.TransactionId))
                            input.TransactionId = clsCommon.GenerateTransactionId();

                        responce = reports.InsertCouponCodeFromExcel(input);
                        if (responce != null && responce.StatusCode == HttpStatusCode.OK && input.CouponCodeId == 0)
                        {
                            try
                            {
                                DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(FolderName));
                                if (dir.GetFiles("*").Any())
                                {
                                    foreach (FileInfo existingfile in dir.GetFiles())
                                    {
                                        existingfile.Delete();
                                    }
                                }
                            }
                            catch { }

                            reports.SendEmailToAdminCouponCodeGenerated(ImportExcelFilesHelper.GetMailContentFromCMS(ConstantUserType.CouponCode), input.NoOfCouponCode, input.TransactionId);
                        }
                    }
                }
            }
            else
            {
                responce.Message = "Please Select Excel File";
                responce.StatusCode = HttpStatusCode.NotFound;
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
        #region Notificatoin Reports
        [System.Web.Http.HttpPost]
        public Responce GetAllNotificatoinList(FilterInput filter)
        {
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetAllNotificationList(filter);
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage NotificatoinExportToExcel(string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "3",
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetAllNotificationList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsNotificationReport> List = new List<clsNotificationReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsNotificationReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "NotificationReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "NotificationReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
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

        #region OTP Reports
        [System.Web.Http.HttpPost]
        public Responce GetAllOTPList(FilterInput filter)
        {
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetAllOTPList(filter);
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage OTPExportToExcel(string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "4",
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetAllOTPList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsOTPReport> List = new List<clsOTPReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsOTPReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "OTPReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "OTPReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
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
        #region Download Reports
        [System.Web.Http.HttpPost]
        public Responce GetUserReportDownloadDataList(FilterInput filter)
        {
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetUserReportDownloadDataList(filter);
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage DownloadDataExportToExcel(string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "0",
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetUserReportDownloadDataList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsWorksheetDownloadDataByUser> List = new List<clsWorksheetDownloadDataByUser>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsWorksheetDownloadDataByUser>;
                    //foreach (var item in List)
                    //{
                    //    item.UserName = item.UserName; // Decrypt The value 
                    //}
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "UserDownload");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "UserDownload-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
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
        public virtual HttpResponseMessage DownloadDataExportToExcelAll(string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "0",
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetDownloadReportDataList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsWorksheetDownloadDataByUser> List = new List<clsWorksheetDownloadDataByUser>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsWorksheetDownloadDataByUser>;
                    //foreach (var item in List)
                    //{
                    //	item.UserName = item.UserName; // Decrypt The value 
                    //}
                    if (List != null && List.Count > 0)
                    {
                        byte[] bytes = clsExpertHelper.ListToExcel(List, "UserDownloadByUser");
                        result = Request.CreateResponse();
                        result.Content = new ByteArrayContent(bytes);
                        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        result.Content.Headers.ContentDisposition.FileName = "UserDownloadByUser-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    }
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
        public virtual HttpResponseMessage DownloadDataExportToExcelByUser(string userId, string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = userId,
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetDownloadReportDataList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsUserDownloadDataByUser> List = new List<clsUserDownloadDataByUser>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsUserDownloadDataByUser>;
                    //foreach (var item in List)
                    //{
                    //	item.UserName = item.UserName; // Decrypt The value 
                    //}
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "UserDownloadByUser");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "UserDownloadByUser-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
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

        #region Referral Reports
        [System.Web.Http.HttpPost]
        public Responce GetAllReferralList(FilterInput filter)
        {
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetAllReferralList(filter);
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage ReferralExportToExcel(string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "2",
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetAllReferralList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsReferralReport> List = new List<clsReferralReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsReferralReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "ReferralReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "ReferralReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
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

        #region URL Manipulation 
        [System.Web.Http.HttpPost]
        public string InsertURLManipulationList(clsURLManipulationRequest objclsURLManipulationRequest)
        {
            string RowId = string.Empty;
            Responce responce = new Responce();
            if (objclsURLManipulationRequest != null)
            {
                responce = reports.InsertURLManipulationList(objclsURLManipulationRequest.OldUrl, objclsURLManipulationRequest.NewUrl);
                List<clsURLManipulation> List = new List<clsURLManipulation>();
                List = responce.Result as List<clsURLManipulation>;
                if (List.Count == 1)
                    RowId = List[0].RowId;
            }
            return RowId;
        }
        #endregion

        #region Worksheet 
        [System.Web.Http.HttpPost]
        public string GetAgeGoupe_Language_Master()
        {
            string strCollection = string.Empty;
            strCollection = "";
            Responce responce = new Responce();

            responce = reports.Get_AgeGroupe_Language_Master("1");
            List<clsAgeGroupeMaster> AgeGroupeList = new List<clsAgeGroupeMaster>();
            AgeGroupeList = responce.Result as List<clsAgeGroupeMaster>;
            strCollection = strCollection + "<option value='0'>Select Age Group</option>";
            foreach (var item in AgeGroupeList)
            {
                strCollection = strCollection + "<option value='" + item.id + "'>" + item.AgeGroup + "</option>";
            }
            strCollection = strCollection + "|<option value='0'>Select Language</option>";
            responce = null;

            responce = reports.Get_AgeGroupe_Language_Master("2");
            List<clsLanguageMaster> LanguageList = new List<clsLanguageMaster>();
            LanguageList = responce.Result as List<clsLanguageMaster>;
            foreach (var item in LanguageList)
            {
                strCollection = strCollection + "<option value='" + item.id + "'>" + item.LanguageDetails + "</option>";
            }
            strCollection = strCollection + "|<option value='0'>Select ProgramType</option>";
            responce = null;
            responce = reports.Get_AgeGroupe_Language_Master("3");
            List<clsProgramTypeMaster> ProgramTypeList = new List<clsProgramTypeMaster>();
            ProgramTypeList = responce.Result as List<clsProgramTypeMaster>;
            foreach (var item in ProgramTypeList)
            {
                strCollection = strCollection + "<option value='" + item.id + "'>" + item.ProgramType + "</option>";
            }


            return strCollection;
        }

        [System.Web.Http.HttpPost]
        public Responce upload_excel()
        {
            Responce responce = new Responce();
            try
            {
                var file = System.Web.HttpContext.Current.Request.Files.Count > 0 ? System.Web.HttpContext.Current.Request.Files[0] : null;
                string ProgramType = System.Web.HttpContext.Current.Request["ProgramType"].ToString();
                if (file != null && file.ContentLength > 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("ExcelFile/BulkUploadExcel"));
                    var fileName = DateTime.Now.ToString("yyyyMMdd") + Path.GetFileName(file.FileName);
                    var path = Path.Combine(
                        System.Web.HttpContext.Current.Server.MapPath("ExcelFile/BulkUploadExcel"),
                        fileName
                    );
                    path = path.ToLower().Replace(@"\umbraco\backoffice\api\reportsauthorizeapi", "");
                    file.SaveAs(path);
                    DataTable DT = new DataTable();
                    DT = GetReferUploadedExcelDataTable(path);

                    if (ProgramType == "1") //Lession
                    {
                        List<clsWorksheetLession> objclsWorksheetLession = new List<clsWorksheetLession>();
                        objclsWorksheetLession = (from DataRow dr in DT.Rows
                                                  select new clsWorksheetLession()
                                                  {
                                                      SubjectID = dr["SubjectID"].ToString(),
                                                      WorksheetTitle = dr["WorksheetTitle"].ToString(),
                                                      Title = dr["Title"].ToString(),
                                                      SubTitle = dr["SubTitle"].ToString(),
                                                      ShareContent = dr["ShareContent"].ToString(),
                                                      SharingText = dr["SharingText"].ToString(),
                                                      WeekID = dr["WeekID"].ToString(),
                                                      TopicID = dr["TopicID"].ToString(),
                                                      Description = dr["Description"].ToString(),
                                                      IsGuestUserSheet = dr["IsGuestUserSheet"].ToString(),
                                                      CBSEContentIncluded = dr["CBSEContentIncluded"].ToString(),
                                                      IsQuizWorksheet = dr["IsQuizWorksheet"].ToString(),
                                                      IsEnableforDetailsPage = dr["IsEnableforDetailsPage"].ToString(),
                                                      DescriptionPageContent = dr["DescriptionPageContent"].ToString(),
                                                      DesktopImageID = dr["DesktopImageID"].ToString(),
                                                      DesktopImageWebpID = dr["DesktopImageWebpID"].ToString(),
                                                      MobileImageID = dr["MobileImageID"].ToString(),
                                                      MobileIamgeWebpID = dr["MobileIamgeWebpID"].ToString(),
                                                      WhatsAppBannerID = dr["WhatsAppBannerID"].ToString(),
                                                      Subscription = dr["Subscription"].ToString(),
                                                      UploadPDF = dr["UploadPDF"].ToString(),
                                                      UploadPreviewPDF = dr["UploadPreviewPDF"].ToString(),
                                                  }).ToList();
                        responce.Result = objclsWorksheetLession;
                        return responce;
                    }
                    else if (ProgramType == "2") //Structure
                    {
                        List<clsWorksheetStructure> objclsWorksheetStructure = new List<clsWorksheetStructure>();
                        objclsWorksheetStructure = (from DataRow dr in DT.Rows
                                                    select new clsWorksheetStructure()
                                                    {
                                                        SubjectID = dr["SubjectID"].ToString(),
                                                        TopicID = dr["TopicID"].ToString(),
                                                        WorksheetTitle = dr["WorksheetTitle"].ToString(),
                                                        umbracoUrlAlias = dr["umbracoUrlAlias"].ToString(),
                                                        Title = dr["Title"].ToString(),
                                                        SubTitle = dr["SubTitle"].ToString(),
                                                        Description = dr["Description"].ToString(),
                                                        IsGuestUserSheet = dr["IsGuestUserSheet"].ToString(),
                                                        IsEnableForDetailsPage = dr["IsEnableForDetailsPage"].ToString(),
                                                        Paid = dr["Paid"].ToString(),
                                                        RankingIndex = dr["RankingIndex"].ToString(),
                                                        DesktopImageID = dr["DesktopImageID"].ToString(),
                                                        DesktopImageWebpID = dr["DesktopImageWebpID"].ToString(),
                                                        MobileImageID = dr["MobileImageID"].ToString(),
                                                        MobileIamgeWebpID = dr["MobileIamgeWebpID"].ToString(),
                                                        UploadPDF = dr["UploadPDF"].ToString(),
                                                        UploadPreviewPDF = dr["UploadPreviewPDF"].ToString()
                                                    }).ToList();
                        responce.Result = objclsWorksheetStructure;
                        return responce;
                    }
                    else if (ProgramType == "3") //Teacher
                    {
                        List<clsWorksheetTeacher> objclsWorksheetTeacher = new List<clsWorksheetTeacher>();
                        objclsWorksheetTeacher = (from DataRow dr in DT.Rows
                                                  select new clsWorksheetTeacher()
                                                  {
                                                      WorksheetTitle = dr["WorksheetTitle"].ToString(),
                                                      umbracoUrlAlias = dr["umbracoUrlAlias"].ToString(),
                                                      Title = dr["Title"].ToString(),
                                                      SubTitle = dr["SubTitle"].ToString(),
                                                      Description = dr["Description"].ToString(),
                                                      NoofDays = dr["NoofDays"].ToString(),
                                                      IsGuestUserSheet = dr["IsGuestUserSheet"].ToString(),
                                                      IsEnableForDetailsPage = dr["IsEnableForDetailsPage"].ToString(),
                                                      Paid = dr["Paid"].ToString(),
                                                      RankingIndex = dr["RankingIndex"].ToString(),
                                                      DesktopImageID = dr["DesktopImageID"].ToString(),
                                                      DesktopImageWebpID = dr["DesktopImageWebpID"].ToString(),
                                                      MobileImageID = dr["MobileImageID"].ToString(),
                                                      MobileIamgeWebpID = dr["MobileIamgeWebpID"].ToString(),
                                                      UploadPDF = dr["UploadPDF"].ToString(),
                                                      UploadPreviewPDF = dr["UploadPreviewPDF"].ToString()
                                                  }).ToList();
                        responce.Result = objclsWorksheetTeacher;
                        return responce;
                    }
                    else //if (ProgramType.ToLower().Contains("offer")) //Teacher
                    {
                        List<clsSpecialOffer> objclsSpecialOffer = new List<clsSpecialOffer>();
                        objclsSpecialOffer = (from DataRow dr in DT.Rows
                                              select new clsSpecialOffer()
                                              {
                                                  WorksheetTitle = dr["WorksheetTitle"].ToString(),
                                                  umbracoUrlAlias = dr["umbracoUrlAlias"].ToString(),
                                                  Title = dr["Title"].ToString(),
                                                  SubTitle = dr["SubTitle"].ToString(),
                                                  Description = dr["Description"].ToString(),
                                                  IsEnableForDetailsPage = dr["IsEnableForDetailsPage"].ToString(),
                                                  Paid = dr["Paid"].ToString(),
                                                  DesktopImageID = dr["DesktopImageID"].ToString(),
                                                  DesktopImageWebpID = dr["DesktopImageWebpID"].ToString(),
                                                  MobileImageID = dr["MobileImageID"].ToString(),
                                                  MobileIamgeWebpID = dr["MobileIamgeWebpID"].ToString(),
                                                  UploadPDF = dr["UploadPDF"].ToString(),
                                                  UploadPreviewPDF = dr["UploadPreviewPDF"].ToString()
                                              }).ToList();
                        responce.Result = objclsSpecialOffer;
                        return responce;
                    }
                }
                else
                {
                    responce.Message = "Please Select Excel File";
                    responce.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "upload_excel");
            }

            return responce;
        }
        public DataTable GetReferUploadedExcelDataTable(string flName)
        {
            //string filePath = "BulkUploadExcel/" + flName;
            string filePath = flName;
            DataTable dt = new DataTable();
            DataTable copydt = new DataTable();
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                var workSheet = workBook.Worksheet(1).RangeUsed().RowsUsed().Skip(0);
                bool firstRow = true;
                foreach (var row in workSheet)
                {
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells(false))
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //bool r = row.Cell(0).IsEmpty();
                        //Add rows to DataTable.
                        //if (!String.IsNullOrEmpty(row.Cell(0).GetString()))
                        //{
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells(false))
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                            //}
                        }
                    }
                }
            }
            copydt = dt;
            copydt.AcceptChanges();
            return copydt;
        }

        [System.Web.Http.HttpPost]
        public string upload_Worksheet(clsUploadWorksheetExcel objclsUploadWorksheetExcel)
        {
            CMSBulkUploadController cMSBulkUploadController = new CMSBulkUploadController();
            string RowId = string.Empty;
            DataTable dt = new DataTable();
            Responce responce = new Responce();
            if (objclsUploadWorksheetExcel != null)
            {
                string jsonData = objclsUploadWorksheetExcel.JsonData;
                dt = (DataTable)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData, (typeof(DataTable)));
            }

            //var save = cMSBulkUploadController.Worksheetplansave("en-US", "3-4", dt);
            //var save = cMSBulkUploadController.Worksheetplansave(objclsUploadWorksheetExcel.Language, objclsUploadWorksheetExcel.AgeGroupe, dt);
            if (objclsUploadWorksheetExcel.ProgramType == "1" || objclsUploadWorksheetExcel.ProgramType == "2" || objclsUploadWorksheetExcel.ProgramType == "3")
            {
                var save = cMSBulkUploadController.BulkUpload_WS(objclsUploadWorksheetExcel.Language, objclsUploadWorksheetExcel.ProgramType, objclsUploadWorksheetExcel.AgeGroupe, dt);
                return save;

            }
            else
            {
                var save = cMSBulkUploadController.Specialofferplansave(objclsUploadWorksheetExcel.Language, objclsUploadWorksheetExcel.ProgramType, objclsUploadWorksheetExcel.AgeGroupe, dt);
                return save;

            }



        }

        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage WorkSheetExportToExcel(string ProgramType, string AgeGroup)
        {
            HttpResponseMessage result = null;
            //ProgramType = "teachers";
            string languagekey = "";
            string languageName = "";
            ILocalizationService ls = Services.LocalizationService;
            IEnumerable<ILanguage> languages = ls.GetAllLanguages();
            List<clsLessionPlan> objclsLessionPlan = new List<clsLessionPlan>();
            List<clsworksheetPlan> objclsworksheetPlan = new List<clsworksheetPlan>();
            List<clsTeacherPlan> objclsTeacherPlan = new List<clsTeacherPlan>();

            Responce responce = new Responce();
            // Iterate over the collection
            string node_calss;
            string node_subject;
            string node_ws;

            try
            {
                foreach (ILanguage language in languages)
                {
                    // Get the .NET culture info
                    CultureInfo cultureInfo = language.CultureInfo;
                    //languagekey = language.IsoCode.ToString();
                    languagekey = cultureInfo.Name.ToString();
                    languageName = cultureInfo.DisplayName.ToString();
                    //Structure
                    if (ProgramType.ToLower() == "2")
                    {
                        _variationContextAccessor.VariationContext = new VariationContext(languagekey.ToLower());

                        var rootOfClass = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey))?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && (x.IsPublished() == true || x.IsPublished() == false))?.OfType<WorksheetListingAgeWise>()?
                                            .Where(c => c.AgeGroup.Name == AgeGroup)
                                            .FirstOrDefault()?.Children;

                        if (rootOfClass != null)
                        {
                            node_calss = AgeGroup;
                            foreach (var subjects in rootOfClass)
                            {
                                node_subject = subjects?.Name;
                                var worksheetNode = subjects?.Children.Where(x => x.ContentType.Alias == "structureProgramItems" && x.Cultures.ContainsKey(languagekey))?.OfType<StructureProgramItems>();
                                foreach (var item in worksheetNode)
                                {
                                    node_calss = AgeGroup;
                                    //node_subject = subjects.Name;

                                    clsworksheetPlan objlist = new clsworksheetPlan();
                                    node_ws = item.Name;
                                    var pub = item.IsPublished();
                                    var classId = Umbraco.Content(item.SelectAgeGroup.FirstOrDefault()?.Udi)?.DescendantsOrSelf().OfType<NameListItem>().FirstOrDefault()?.ItemValue.ToString();
                                    var SubjectValue = Umbraco.Content(item.SelectSubject.FirstOrDefault()?.Udi)?.DescendantsOrSelf().OfType<Subjects>().FirstOrDefault()?.SubjectValue;
                                    var topicValue = Umbraco.Content(item.SelectTopic.FirstOrDefault()?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue;
                                    var isGuestUserSheet = item.IsGuestUserSheet.ToString();
                                    var IsEnableForDetailsPage = item.IsEnableForDetailsPage.ToString();
                                    var IsPaid = item.IsPaid.ToString();
                                    var RankingIndex = item.RankingIndex.ToString();

                                    objlist.NodeID = item.Id.ToString();
                                    objlist.WorksheetTitle = item.Name.ToString();
                                    objlist.umbracoUrlAlias = item.Value("umbracoUrlAlias").ToString();
                                    objlist.Title = item.Value("title").ToString();
                                    objlist.SubTitle = item.Value("subtitle").ToString();
                                    objlist.SubjectID = SubjectValue.ToString();
                                    objlist.SubjectName = node_subject;
                                    //objlist.Description = item3.Value("Description").ToString();
                                    objlist.TopicID = topicValue.ToString();
                                    objlist.AgeGroup = node_calss + "(" + classId + ")";
                                    objlist.IsGuestUserSheet = isGuestUserSheet.ToString().ToLower() == "true" ? "Y" : "N";
                                    objlist.IsEnableForDetailsPage = IsEnableForDetailsPage.ToString().ToLower() == "true" ? "Y" : "N";
                                    objlist.Paid = IsPaid.ToString().ToLower() == "true" ? "Y" : "N";
                                    objlist.languageKey = languagekey;
                                    objlist.languageName = languageName;
                                    objlist.IsPublished = pub == true ? "Y" : "N";
                                    objclsworksheetPlan.Add(objlist);
                                }
                            }
                        }


                    }
                    //Lession
                    if (ProgramType.ToLower() == "1")
                    {
                        _variationContextAccessor.VariationContext = new VariationContext(languagekey.ToLower());

                        var SubjectsForTheClass = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey))?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && (x.IsPublished() == true || x.IsPublished() == false))?.OfType<WorksheetListingAgeWise>()?
                                            .Where(c => c?.AgeGroup?.Name == AgeGroup)
                                            .FirstOrDefault()?.Children;

                        if (SubjectsForTheClass != null)
                        {
                            node_calss = AgeGroup;
                            foreach (var subjects in SubjectsForTheClass)
                            {
                                var worksheetNode = subjects?.Children.Where(x => x.ContentType.Alias == "worksheetRoot" && x.Cultures.ContainsKey(languagekey))?.OfType<WorksheetRoot>();

                                foreach (var item3 in worksheetNode)
                                {
                                    clsLessionPlan objlist = new clsLessionPlan();
                                    node_subject = subjects?.Name;
                                    node_ws = item3.Name;
                                    var pub = item3.IsPublished();
                                    var classId = Umbraco.Content(item3.AgeTitle?.Udi)?.DescendantsOrSelf().OfType<NameListItem>().FirstOrDefault().ItemValue;
                                    var SubjectValue = Umbraco.Content(item3.SelectSubject?.Udi)?.DescendantsOrSelf().OfType<Subjects>().FirstOrDefault().SubjectValue;
                                    var weekValue = Umbraco.Content(item3.SelectWeek?.Udi)?.DescendantsOrSelf().OfType<NameListItem>().FirstOrDefault().ItemValue;
                                    var topicValue = Umbraco.Content(item3.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue;

                                    var isGuestUserSheet = item3.IsGuestUserSheet.ToString();
                                    var cBSEContentIncluded = item3.CBsecontentIncluded.ToString();
                                    var isQuizWorksheet = item3.IsQuizWorksheet.ToString();
                                    var isEnableforDetailsPage = item3.IsEnableForDetailsPage.ToString();


                                    objlist.NodeID = item3.Id.ToString();
                                    objlist.WorksheetTitle = item3.Name.ToString();
                                    objlist.Title = item3.Value("title").ToString();
                                    objlist.SubTitle = item3.Value("subtitle").ToString();
                                    objlist.SubjectName = node_subject;
                                    //objlist.AgeGroup = node_calss + "(" + classId + ")";
                                    objlist.AgeGroup = node_calss;
                                    //objlist.DesktopImageID = uploadThumbnail.ToString();
                                    //objlist.DesktopImageWebpID = nextGenImage.ToString();
                                    //objlist.MobileImageID = uploadMobileThumbnail.ToString();
                                    //objlist.MobileIamgeWebpID = mobileNextGenImage.ToString();
                                    objlist.languageKey = languagekey;
                                    objlist.languageName = languageName;
                                    objlist.IsPublished = pub == true ? "Y" : "N";
                                    objclsLessionPlan.Add(objlist);
                                }
                            }
                        }
                    }
                    //Teacher
                    if (ProgramType.ToLower() == "3")
                    {
                        var classRoot_teachers = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                               .Where(x => x.ContentType.Alias == "teacherRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                               .Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && (x.IsPublished() == true || x.IsPublished() == false))?
                                               .OfType<WorksheetListingAgeWise>()?.Where(c => c.AgeGroup.Name == AgeGroup);
                        var classRoot = classRoot_teachers.Where(x => x.ContentType.Alias == "teacherProgramItems")?.OfType<TeacherProgramItems>();
                        _variationContextAccessor.VariationContext = new VariationContext(languagekey.ToLower());
                        try
                        {
                            if (classRoot_teachers.Any())
                            {
                                _variationContextAccessor.VariationContext = new VariationContext(languagekey.ToLower());

                                foreach (var item in classRoot_teachers)
                                {
                                    node_calss = item.Name;
                                    foreach (var item2 in item.Children.Where(x => x.ContentType.Alias == "teacherProgramItems").OfType<TeacherProgramItems>())
                                    {
                                        node_subject = item2.Name;
                                        clsTeacherPlan objlist = new clsTeacherPlan();
                                        node_ws = item2.Name;
                                        var pub = item2.IsPublished();

                                        var Noofday = Umbraco.Content(item2.NoOfDays?.Udi)?.DescendantsOrSelf().OfType<NameListItem>().FirstOrDefault().ItemValue;
                                        var isGuestUserSheet = item2.IsGuestUserSheet.ToString();
                                        var IsEnableForDetailsPage = item2.IsEnableForDetailsPage.ToString();
                                        var IsPaid = item2.IsPaid.ToString();
                                        var RankingIndex = item2.RankingIndex.ToString();


                                        objlist.NodeID = item2.Id.ToString();
                                        objlist.WorksheetTitle = item2.Name.ToString();
                                        objlist.umbracoUrlAlias = item2.Value("umbracoUrlAlias").ToString();
                                        objlist.Title = item2.Value("title").ToString();
                                        objlist.SubTitle = item2.Value("subtitle").ToString();
                                        objlist.RankingIndex = item2.Value("rankingIndex").ToString();

                                        objlist.AgeGroup = node_calss;

                                        objlist.NoofDays = Noofday.ToString();
                                        objlist.IsGuestUserSheet = isGuestUserSheet.ToString().ToLower() == "true" ? "Y" : "N";
                                        objlist.IsEnableForDetailsPage = IsEnableForDetailsPage.ToString().ToLower() == "true" ? "Y" : "N";
                                        objlist.Paid = IsPaid.ToString().ToLower() == "true" ? "Y" : "N";
                                        objlist.languageKey = languagekey;
                                        objlist.languageName = languageName;
                                        objlist.IsPublished = pub == true ? "Y" : "N";
                                        objclsTeacherPlan.Add(objlist);
                                        //}
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                responce.Result = objclsworksheetPlan;
                if (objclsworksheetPlan != null && objclsworksheetPlan.Count > 0)
                {
                    List<clsworksheetPlan> List = new List<clsworksheetPlan>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsworksheetPlan>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "StructureData");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "StructureData-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }

                responce.Result = objclsLessionPlan;
                if (objclsLessionPlan != null && objclsLessionPlan.Count > 0)
                {
                    List<clsLessionPlan> List = new List<clsLessionPlan>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsLessionPlan>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "LessionData");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "LessionData-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }

                responce.Result = objclsTeacherPlan;
                if (objclsTeacherPlan != null && objclsTeacherPlan.Count > 0)
                {
                    List<clsTeacherPlan> List = new List<clsTeacherPlan>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsTeacherPlan>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "TeacherData");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "TeacherData-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "BulkUpload_WS");
            }
            //var json = JsonConvert.SerializeObject(responce.Result);
            return result;
            //return json;

        }

        [System.Web.Http.HttpPost]
        public Responce upload_excel_bulk_update()
        {
            Responce responce = new Responce();
            try
            {
                var file = System.Web.HttpContext.Current.Request.Files.Count > 0 ? System.Web.HttpContext.Current.Request.Files[0] : null;
                string ProgramType = System.Web.HttpContext.Current.Request["ProgramType"].ToString();
                if (file != null && file.ContentLength > 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("ExcelFile/BulkUploadExcel"));
                    var fileName = DateTime.Now.ToString("yyyyMMdd") + "_U_" + Path.GetFileName(file.FileName);
                    var path = Path.Combine(
                        System.Web.HttpContext.Current.Server.MapPath("ExcelFile/BulkUploadExcel"),
                        fileName
                    );
                    path = path.ToLower().Replace(@"\umbraco\backoffice\api\reportsauthorizeapi", "");
                    file.SaveAs(path);
                    DataTable DT = new DataTable();
                    DT = GetReferUploadedExcelDataTable(path);

                    if (ProgramType == "1") //Lession
                    {
                        List<clsLessionPlan> objclsLessionPlan = new List<clsLessionPlan>();
                        objclsLessionPlan = (from DataRow dr in DT.Rows
                                             select new clsLessionPlan()
                                             {
                                                 NodeID = dr["NodeID"].ToString(),
                                                 IsPublished = dr["IsPublished"].ToString(),
                                                 languageKey = dr["languageKey"].ToString(),
                                                 languageName = dr["languageName"].ToString(),
                                                 AgeGroup = dr["AgeGroup"].ToString(),
                                                 SubjectID = dr["SubjectID"].ToString(),
                                                 WeekID = dr["WeekID"].ToString(),
                                                 TopicID = dr["TopicID"].ToString(),
                                                 SubjectName = dr["SubjectName"].ToString(),
                                                 WorksheetTitle = dr["WorksheetTitle"].ToString(),
                                                 Title = dr["Title"].ToString(),
                                                 SubTitle = dr["SubTitle"].ToString(),
                                                 ShareContent = dr["ShareContent"].ToString(),
                                                 SharingText = dr["SharingText"].ToString(),
                                                 Description = dr["Description"].ToString(),
                                                 IsGuestUserSheet = dr["IsGuestUserSheet"].ToString(),
                                                 CBSEContentIncluded = dr["CBSEContentIncluded"].ToString(),
                                                 IsQuizWorksheet = dr["IsQuizWorksheet"].ToString(),
                                                 IsEnableforDetailsPage = dr["IsEnableforDetailsPage"].ToString(),
                                                 DescriptionPageContent = dr["DescriptionPageContent"].ToString(),
                                                 DesktopImageID = dr["DesktopImageID"].ToString(),
                                                 DesktopImageWebpID = dr["DesktopImageWebpID"].ToString(),
                                                 MobileImageID = dr["MobileImageID"].ToString(),
                                                 MobileIamgeWebpID = dr["MobileIamgeWebpID"].ToString(),
                                                 WhatsAppBannerID = dr["WhatsAppBannerID"].ToString(),
                                                 Subscription = dr["Subscription"].ToString(),
                                                 UploadPDF = dr["UploadPDF"].ToString(),
                                                 UploadPreviewPDF = dr["UploadPreviewPDF"].ToString(),
                                             }).ToList();
                        responce.Result = objclsLessionPlan;
                        return responce;
                    }
                    else if (ProgramType == "2") //Structure
                    {
                        List<clsworksheetPlan> objclsworksheetPlan = new List<clsworksheetPlan>();
                        objclsworksheetPlan = (from DataRow dr in DT.Rows
                                               select new clsworksheetPlan()
                                               {
                                                   NodeID = dr["NodeID"].ToString(),
                                                   IsPublished = dr["IsPublished"].ToString(),
                                                   languageKey = dr["languageKey"].ToString(),
                                                   languageName = dr["languageName"].ToString(),
                                                   AgeGroup = dr["AgeGroup"].ToString(),
                                                   SubjectID = dr["SubjectID"].ToString(),
                                                   SubjectName = dr["SubjectName"].ToString(),
                                                   TopicID = dr["TopicID"].ToString(),
                                                   WorksheetTitle = dr["WorksheetTitle"].ToString(),
                                                   umbracoUrlAlias = dr["umbracoUrlAlias"].ToString(),
                                                   Title = dr["Title"].ToString(),
                                                   SubTitle = dr["SubTitle"].ToString(),
                                                   Description = dr["Description"].ToString(),
                                                   IsGuestUserSheet = dr["IsGuestUserSheet"].ToString(),
                                                   IsEnableForDetailsPage = dr["IsEnableForDetailsPage"].ToString(),
                                                   Paid = dr["Paid"].ToString(),
                                                   RankingIndex = dr["RankingIndex"].ToString(),
                                                   DesktopImageID = dr["DesktopImageID"].ToString(),
                                                   DesktopImageWebpID = dr["DesktopImageWebpID"].ToString(),
                                                   MobileImageID = dr["MobileImageID"].ToString(),
                                                   MobileIamgeWebpID = dr["MobileIamgeWebpID"].ToString(),
                                                   UploadPDF = dr["UploadPDF"].ToString(),
                                                   UploadPreviewPDF = dr["UploadPreviewPDF"].ToString()
                                               }).ToList();
                        responce.Result = objclsworksheetPlan;
                        return responce;
                    }
                    else if (ProgramType == "3") //Teacher
                    {
                        List<clsTeacherPlan> objclsTeacherPlan = new List<clsTeacherPlan>();
                        objclsTeacherPlan = (from DataRow dr in DT.Rows
                                             select new clsTeacherPlan()
                                             {
                                                 NodeID = dr["NodeID"].ToString(),
                                                 IsPublished = dr["IsPublished"].ToString(),
                                                 languageKey = dr["languageKey"].ToString(),
                                                 languageName = dr["languageName"].ToString(),
                                                 AgeGroup = dr["AgeGroup"].ToString(),
                                                 WorksheetTitle = dr["WorksheetTitle"].ToString(),
                                                 umbracoUrlAlias = dr["umbracoUrlAlias"].ToString(),
                                                 Title = dr["Title"].ToString(),
                                                 SubTitle = dr["SubTitle"].ToString(),
                                                 Description = dr["Description"].ToString(),
                                                 NoofDays = dr["NoofDays"].ToString(),
                                                 IsGuestUserSheet = dr["IsGuestUserSheet"].ToString(),
                                                 IsEnableForDetailsPage = dr["IsEnableForDetailsPage"].ToString(),
                                                 Paid = dr["Paid"].ToString(),
                                                 RankingIndex = dr["RankingIndex"].ToString(),
                                                 DesktopImageID = dr["DesktopImageID"].ToString(),
                                                 DesktopImageWebpID = dr["DesktopImageWebpID"].ToString(),
                                                 MobileImageID = dr["MobileImageID"].ToString(),
                                                 MobileIamgeWebpID = dr["MobileIamgeWebpID"].ToString(),
                                                 UploadPDF = dr["UploadPDF"].ToString(),
                                                 UploadPreviewPDF = dr["UploadPreviewPDF"].ToString()
                                             }).ToList();
                        responce.Result = objclsTeacherPlan;
                        return responce;
                    }
                }
                else
                {
                    responce.Message = "Please Select Excel File";
                    responce.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(ReportsAuthorizeApiController), ex, message: "upload_excel_bulk_update");
            }

            return responce;
        }


        [System.Web.Http.HttpPost]
        public string upload_Worksheet_update(clsUploadWorksheetExcelUpdate objclsUploadWorksheetExcelUpdate)
        {
            CMSBulkUploadController cMSBulkUploadController = new CMSBulkUploadController();
            DataTable dt = new DataTable();
            Responce responce = new Responce();
            if (objclsUploadWorksheetExcelUpdate != null)
            {
                string jsonData = objclsUploadWorksheetExcelUpdate.JsonData;
                dt = (DataTable)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData, (typeof(DataTable)));
            }
            var save = cMSBulkUploadController.BulkUpDate_WS(objclsUploadWorksheetExcelUpdate.Language, objclsUploadWorksheetExcelUpdate.ProgramType, objclsUploadWorksheetExcelUpdate.AgeGroupe, dt);

            return save;
        }

        #endregion
        #region Node By Image 
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage ExportNodeByImage(string Node)
        {
            HttpResponseMessage result = null;

            //11114
            var mediaData = Umbraco.Media(Convert.ToInt32(Node))?.DescendantsOrSelf();
            List<MediaReference> mediaReference = new List<MediaReference>();
            Responce responce = new Responce();
            clsExpertHelper clsExpertHelper = new clsExpertHelper();
            if (mediaData != null)
            {
                foreach (var media in mediaData)
                {
                    mediaReference.Add(new MediaReference { Id = media.Id, Name = string.IsNullOrEmpty(media?.Url()) ? "" : media?.Url().Split('/')[media.Url().Split('/').Length - 1].ToString(), Extention = media.ContentType.Alias });
                }
            }
            if (mediaReference != null)
            {
                //mediaReference = responce.Result as List<MediaReference>;
                byte[] bytes = clsExpertHelper.ListToExcel(mediaReference, "ImageList");
                result = Request.CreateResponse();
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = "ImageList-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                return result;
            }
            return result;
        }


        #endregion


        #region Send WhatsApp Notificatoin Reports
        [System.Web.Http.HttpPost]
        public Responce GetAllSendWhatsAppNotificatoinList(FilterInput filter)
        {
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetAllSendWhatsAppNotificationList(filter);
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage SendWhatsAppNotificatoinExportToExcel(string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "5",
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetAllSendWhatsAppNotificationList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsSendWhatsAppNotification> List = new List<clsSendWhatsAppNotification>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsSendWhatsAppNotification>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "SendWhatsAppNotificationReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "SendWhatsAppNotificationReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }
        [System.Web.Http.HttpPost]
        public Responce upload_excel_SendWhatsAppNotificatoin()
        {
            Responce responce = new Responce();
            var file = System.Web.HttpContext.Current.Request.Files.Count > 0 ? System.Web.HttpContext.Current.Request.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                DirectoryInfo dir = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("ExcelFile/SendWhatsAppNotification"));
                var fileName = DateTime.Now.ToString("yyyyMMdd") + Path.GetFileName(file.FileName);
                var path = Path.Combine(
                    System.Web.HttpContext.Current.Server.MapPath("ExcelFile/SendWhatsAppNotification"),
                    fileName
                );
                path = path.Replace((@"\umbraco\backoffice\api\reportsauthorizeapi"), "");
                file.SaveAs(path);
                DataTable DT = new DataTable();
                DT = GetReferUploadedExcelDataTable(path);
                if (DT != null && DT.Rows.Count > 0)
                {
                    List<clsSendWhatsAppNotification> objList = new List<clsSendWhatsAppNotification>();
                    for (int j = 0; j < DT.Rows.Count; j++)
                    {
                        clsSendWhatsAppNotification item = new clsSendWhatsAppNotification();
                        item.UserUniqueId = DT.Rows[j]["UserUniqueId"].ToString();
                        item.Mode = DT.Rows[j]["Mode"].ToString();
                        item.iStatus = DT.Rows[j]["iStatus"].ToString();
                        item.IsTestId = DT.Rows[j]["IsTestId"].ToString();
                        objList.Add(item);
                    }
                    string isValid = UPDATE_EXCEL_DATA(objList);
                    responce.Message = isValid;
                    responce.StatusCode = HttpStatusCode.OK;
                }

            }
            else
            {
                responce.Message = "Please Select Excel File";
                responce.StatusCode = HttpStatusCode.NotFound;
            }
            return responce;
        }
        public string UPDATE_EXCEL_DATA(List<clsSendWhatsAppNotification> i)
        {
            XDocument XmlDoc = new XDocument(new XDeclaration("1.0", "UTF - 8", "yes"),
                new XElement("NewDataSet", from DataList in i
                                           select new XElement("SendWhatsAppNotification",
                                           new XElement("UserUniqueId", DataList.UserUniqueId),
                                           new XElement("Mode", DataList.Mode),
                                           new XElement("iStatus", DataList.iStatus),
                                           new XElement("IsTestId", DataList.IsTestId)
                                           )));
            string response = reports.InsertSendWhatsAppNotificationList(XmlDoc);
            return response;
        }
        #endregion
        #region Age Groupe Advanced Search
        [System.Web.Http.HttpPost]
        public string InsertAgeGroupeAdvancedSearch(clsAgeGroupeAdvancedSearch objclsAgeGroupeAdvancedSearch)
        {
            string RowId = string.Empty;
            Responce responce = new Responce();
            if (objclsAgeGroupeAdvancedSearch != null)
            {
                responce = reports.InsertAgeGroupeAdvancedSearch(objclsAgeGroupeAdvancedSearch.ClassName, objclsAgeGroupeAdvancedSearch.SynonymsName);
                List<clsAgeGroupeAdvancedSearchResponse> List = new List<clsAgeGroupeAdvancedSearchResponse>();
                List = responce.Result as List<clsAgeGroupeAdvancedSearchResponse>;
                if (List.Count == 1)
                    RowId = List[0].RowId;
            }
            return RowId;
        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage AgeGroupeAdvancedExportToExcel()
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "6",
                    StartDate = Convert.ToDateTime(System.DateTime.Now),
                    EndDate = Convert.ToDateTime(System.DateTime.Now),
                };
                responce = reports.GetAllAgeGroupeAdvancedList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsAgeGroupeAdvancedSearchList> List = new List<clsAgeGroupeAdvancedSearchList>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsAgeGroupeAdvancedSearchList>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "AgeGroupeAdvancedReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "AgeGroupeAdvancedReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(ReportsAuthorizeApiController), ex, message: "AgeGroupeAdvancedExportToExcel");
            }
            return result;

        }
        #endregion


        #region NoRecordFound Reports
        [System.Web.Http.HttpPost]
        public Responce GetAllNoRecordFoundList(FilterInput filter)
        {
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetAllNoRecordFoundList(filter);
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage NoRecordFoundExportToExcel(string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "7",
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetAllNoRecordFoundList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsNoRecordFoundReport> List = new List<clsNoRecordFoundReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsNoRecordFoundReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "NoRecordFoundReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "NoRecordFoundReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
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


        #region User Transaction Reports
        [System.Web.Http.HttpPost]
        public Responce GetAllUserTransactionList(FilterInput filter)
        {
            if (filter.EndDate != null && filter.StartDate != null)
            {
                return reports.GetAllUserTransactionList(filter);
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage UserTransactionExportToExcel(string StartDate, string EndDate)
        {
            HttpResponseMessage result = null;
            try
            {
                Responce responce = new Responce();
                FilterInput input = new FilterInput()
                {
                    itemsPerPage = 0,
                    QType = "8",
                    StartDate = Convert.ToDateTime(StartDate),
                    EndDate = Convert.ToDateTime(EndDate),
                };
                responce = reports.GetAllUserTransactionList(input);
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    List<clsUserTransaction> List = new List<clsUserTransaction>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsUserTransaction>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "UserTransactionReport");
                    result = Request.CreateResponse();
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "UserTransactionReport-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
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

        #region bulk media upload

        [System.Web.Http.HttpPost]
        public Responce BulkMediaUpload()
        {
            Responce responce = new Responce();
            try
            {
                var files = HttpContext.Current.Request.Files;

                if (files.Count <= 0)
                {
                    responce.Message = "Please Select Media File";
                    responce.StatusCode = HttpStatusCode.NotAcceptable;

                    return responce;
                }

                List<string> responseMediaPaths = new List<string>();

                for (var i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var fileName = NormalizeWhiteSpace(file.FileName).Replace(" ", "-");

                    PdfGeneratorController contpdf = new PdfGeneratorController();
                    byte[] bytes = ConvertStreamToByteArray(file.InputStream);

                    if (bytes != null && bytes.Length > 0)
                    {
                        System.IO.File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("/media/") + fileName, bytes);
                        string BucketHostname = ConfigurationManager.AppSettings["SiteUrl"];

                        S3BucketHelper s3BucketHelper = new S3BucketHelper();
                        var subFolderName = RandomString().ToLower();
                        var uploadResponse = s3BucketHelper.sendMyFileToS3Async(bytes, fileName, "media", subFolderName);

                        if (uploadResponse.StatusCode == HttpStatusCode.OK)
                            responseMediaPaths.Add(uploadResponse.Result.ToString());
                    }
                }

                responce.Result = responseMediaPaths;
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(ReportsAuthorizeApiController), ex, message: "BulkMediaUpload");
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.ExpectationFailed;

                return responce;
            }

            responce.StatusCode = HttpStatusCode.OK;
            return responce;
        }

        private static string NormalizeWhiteSpace(string input)
        {
            // ========= stringbuilder =========
            StringBuilder tmpbuilder = new StringBuilder(input.Length);
            bool inspaces = false;
            string scopy = input;
            tmpbuilder.Length = 0;

            for (int k = 0; k < input.Length; ++k)
            {
                char c = scopy[k];

                if (inspaces)
                {
                    if (c != ' ')
                    {
                        inspaces = false;
                        tmpbuilder.Append(c);
                    }
                }
                else if (c == ' ')
                {
                    inspaces = true;
                    tmpbuilder.Append(' ');
                }
                else
                    tmpbuilder.Append(c);
            }

            return tmpbuilder.ToString();
        }

        public static string RandomString(int length = 6)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static byte[] ConvertStreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage ExportBulkMediaSheet(string filesPath)
        {
            HttpResponseMessage result = null;

            if (string.IsNullOrWhiteSpace(filesPath))
                return result;

            var clsExpertHelper = new clsExpertHelper();

            var files = filesPath.Split(',').ToList();

            byte[] bytes = clsExpertHelper.GenerateBuldMediaExcel(files, "Bulk Media Upload List");
            result = Request.CreateResponse();
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "BulkMediaUploadList-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
            return result;
        }

        #endregion
    }
}