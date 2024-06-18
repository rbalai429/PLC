using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using HPPlc.Models.Coupon;
using HPPlc.Models.FAQ;
using HPPlc.Models.Mailer;
using HPPlc.Models.Reports;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Models.ReportsHelper
{
    public class ReportsHelper
    {
        #region Registration Report 
        public Responce GetAllRegistrationList(FilterInput filter)
        {
            Responce responce = new Responce();
            try
            {

                List<clsRegistrationReport> List = new List<clsRegistrationReport>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = filter.QType},
                    new SetParameters{ ParameterName = "@Query", Value =clsCommon.Encrypt(filter.Search) },
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
                }
                responce.Result = List;
                responce.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        public void SendEmailToRegistration(MailContentModel mailContent)
        {
            try
            {
                if (mailContent != null && mailContent.EmailTo != null)
                {
                    Responce responce = new Responce();
                    FilterInput filter = new FilterInput()
                    {
                        QType = "3",
                    };
                    responce = GetAllRegistrationList(filter);
                    List<clsRegistrationReport> List = new List<clsRegistrationReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsRegistrationReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "FAQRequest");
                    MemoryStream stream = new MemoryStream(bytes);
                    SenderMailer mailer = new SenderMailer();
                    string FileName = DateTime.Now.ToString("dd-MM-yyyy") + "-RegistrationReport.xlsx";
                    mailer.SendeMailWithAttchment(stream, FileName, "application/vnd.ms-excel", mailContent);
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Subscription Report 
        public Responce GetAllSubscriptionList(FilterInput filter)
        {
            Responce responce = new Responce();
            try
            {

                List<clsSubscriptionReport> List = new List<clsSubscriptionReport>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = filter.QType },
                    new SetParameters{ ParameterName = "@Query", Value =clsCommon.Encrypt(filter.Search)  },
                };
                List = _db.GetDataMultiple("GetAllSubscriptionDetails", List, sp);
                if (List != null && List.Count > 0)
                {
                    foreach (var item in List)
                    {
                        if (!String.IsNullOrEmpty(item.u_name))
                            item.u_name = clsCommon.Decrypt(item.u_name);
                        if (!String.IsNullOrEmpty(item.u_email))
                            item.u_email = clsCommon.Decrypt(item.u_email);

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
            return responce;
        }
        public Responce GetExtractReport()
        {
            Responce responce = new Responce();
            try
            {

                List<clsSubscriptionReportFoScheduler> List = new List<clsSubscriptionReportFoScheduler>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                };
                List = _db.GetDataMultiple("usp_SFMC_PLC_ExtractReport", List, sp);
               
                responce.Result = List;
                responce.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        public void SendEmailToSubscription(MailContentModel mailContent)
        {
            try
            {
                if (mailContent != null && mailContent.EmailTo != null)
                {
                    Responce responce = new Responce();
                    FilterInput filter = new FilterInput()
                    {
                        QType = "3",
                    };
                    responce = GetAllSubscriptionList(filter);
                    List<clsSubscriptionReport> List = new List<clsSubscriptionReport>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<clsSubscriptionReport>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "FAQRequest");
                    MemoryStream stream = new MemoryStream(bytes);
                    SenderMailer mailer = new SenderMailer();
                    string FileName = DateTime.Now.ToString("dd-MM-yyyy") + "-SubscriptionReport.xlsx";
                    mailer.SendeMailWithAttchment(stream, FileName, "application/vnd.ms-excel", mailContent);
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region User Login Report
        public Responce GetUserLoginList(FilterInput input)
        {
            Responce responce = new Responce();

            try
            {
                List<UserLoginReport> List = new List<UserLoginReport>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = input.QType },
                    new SetParameters{ ParameterName = "@Query", Value =clsCommon.Encrypt(input.Search)  },
                };
                List = _db.GetDataMultiple("GetRegistrationUserLoginDetails", List, sp);

                if (input.StartDate != null && input.StartDate.HasValue)
                {
                    List = List.Where(x => Convert.ToDateTime(x.LastLoginDate) >= input.StartDate.Value).ToList();
                }
                if (input.EndDate != null && input.EndDate.HasValue)
                {
                    List = List.Where(x => Convert.ToDateTime(x.LastLoginDate) <= input.EndDate.Value).ToList();
                }
                if (input.Status != null && !string.IsNullOrWhiteSpace(input.Status))
                {
                    List = List.Where(x => x.UserType.Equals(input.Status)).ToList();
                }
                responce.TotalPage = (int)Math.Ceiling((double)List.Count() / input.itemsPerPage);
                input.Page = input.Page - 1;
                if (input.itemsPerPage != 0)
                {
                    List = List.Skip((input.itemsPerPage * input.Page)).Take(input.itemsPerPage).ToList();
                }
                else
                    responce.Result = List;
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
            return responce;
        }
        #endregion  

        #region WorkSheet Download Reports 
        public Responce WorksSheetDownloadList(FilterInput input)
        {
            Responce responce = new Responce();

            try
            {
                List<WorkSheetDownloadReport> List = new List<WorkSheetDownloadReport>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = input.QType },
                    new SetParameters{ ParameterName = "@Query", Value =clsCommon.Encrypt(input.Search)  },
                };
                List = _db.GetDataMultiple("GetWorkSheetDownloadDetails", List, sp);
                if (input.StartDate != null && input.StartDate.HasValue)
                {
                    List = List.Where(x => Convert.ToDateTime(x.InsertedOn) >= input.StartDate.Value).ToList();
                }
                if (input.EndDate != null && input.EndDate.HasValue)
                {
                    List = List.Where(x => Convert.ToDateTime(x.InsertedOn) <= input.EndDate.Value).ToList();
                }
                if (input.Status != null && !string.IsNullOrWhiteSpace(input.Status))
                {
                    List = List.Where(x => x.UserType.Equals(input.Status)).ToList();
                }
                responce.TotalPage = (int)Math.Ceiling((double)List.Count() / input.itemsPerPage);
                input.Page = input.Page - 1;
                if (input.itemsPerPage != 0)
                {

                    List = List.Skip((input.itemsPerPage * input.Page)).Take(input.itemsPerPage).ToList();
                }
                else
                    responce.Result = List;
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
            return responce;
        }
        #endregion

        #region FAQ Reports 
        public Responce RequestList(string Search, DateTime? StartDate, DateTime? EndDate, string Status, int itemsPerPage = 0, int Page = 0)
        {
            Responce responce = new Responce();

            try
            {
                dbProxy _db = new dbProxy();
                List<FAQRequestModel> fAQRequestsList = new List<FAQRequestModel>();
                List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@QType", Value ="1"}
                    };
                fAQRequestsList = _db.GetDataMultiple("GetFAQRequestsList", fAQRequestsList, sp);
                if (!string.IsNullOrWhiteSpace(Search))
                {
                    fAQRequestsList = fAQRequestsList.Where(x => x.FullName.ToLower().Contains(Search.ToLower()) || x.Mobile.Contains(Search)).ToList();
                }
                if (StartDate != null && StartDate.HasValue)
                {
                    fAQRequestsList = fAQRequestsList.Where(x => Convert.ToDateTime(x.SelectDate) >= StartDate.Value).ToList();
                }
                if (EndDate != null && EndDate.HasValue)
                {
                    fAQRequestsList = fAQRequestsList.Where(x => Convert.ToDateTime(x.SelectDate) <= EndDate.Value).ToList();
                }
                if (StartDate != null && StartDate.HasValue && EndDate != null && EndDate.HasValue)
                {
                    fAQRequestsList = fAQRequestsList.Where(x => Convert.ToDateTime(x.SelectDate) <= EndDate.Value && Convert.ToDateTime(x.SelectDate) <= EndDate.Value).ToList();
                }
                if (Status != null && !string.IsNullOrWhiteSpace(Status))
                {
                    fAQRequestsList = fAQRequestsList.Where(x => x.Status != null && !string.IsNullOrWhiteSpace(x.Status) && x.Status.Contains(Status)).ToList();
                }
                if (itemsPerPage != 0)
                {

                    responce.TotalPage = (int)Math.Ceiling((double)fAQRequestsList.Count() / itemsPerPage);
                    Page = Page - 1;
                    responce.Result = fAQRequestsList.Skip((itemsPerPage * Page)).Take(itemsPerPage).ToList();
                    responce.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    responce.Result = fAQRequestsList;
                    responce.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        public Responce GetRequestDetails(int RequestId)
        {
            Responce responce = new Responce();

            try
            {
                dbProxy _db = new dbProxy();
                List<FAQRequestResultModel> fAQRequestsList = new List<FAQRequestResultModel>();
                List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@QType", Value ="1"},
                        new SetParameters { ParameterName = "@RequestId",Value= RequestId.ToString()}
                    };
                fAQRequestsList = _db.GetDataMultiple("GetFAQRequestsList", fAQRequestsList, sp);
                fAQRequestsList.ForEach(x => x.CreatedDate = x.DOC.ToString());
                responce.Result = fAQRequestsList;
                responce.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        public Responce SaveFAQRequestFeebBack(Stream MyExcelStream)
        {
            Responce responce = new Responce();
            try
            {
                DataTable dtAll = new DataTable();
                List<FAQRequestResultModel> AllList = new List<FAQRequestResultModel>();
                DataTable dt = GetDataTableFromSpreadsheet(MyExcelStream);
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            FAQRequestResultModel faq = new FAQRequestResultModel()
                            {
                                RequestId = Convert.ToInt32(dt.Rows[i]["RequestId"] != null ? dt.Rows[i]["RequestId"] : 0),
                                FollowUp = Convert.ToString(dt.Rows[i]["FollowUp"]),
                                Status = Convert.ToString(dt.Rows[i]["Status"]),
                            };
                            if (faq != null && faq.RequestId > 0 && !string.IsNullOrWhiteSpace(faq.FollowUp) && !string.IsNullOrWhiteSpace(faq.Status))
                                AllList.Add(faq);
                            else
                            {
                                dt.Rows[i].Delete();
                                i--;
                            }
                        }
                        dtAll.Merge(dt);
                        responce = InsertFAQRequestFollowUp(dtAll);
                    }
                }
            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        public DataTable GetDataTableFromSpreadsheet(Stream MyExcelStream)
        {

            DataTable dt = new DataTable();
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream, false))
            {
                WorkbookPart workbookPart = sDoc.WorkbookPart;
                IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
                DocumentFormat.OpenXml.Spreadsheet.Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    string header = GetCellValue(sDoc, cell);
                    if (header != null && !string.IsNullOrWhiteSpace(header))
                        dt.Columns.Add(header);
                    else
                        break;
                }

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        string value = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                        if (value != null && !string.IsNullOrWhiteSpace(value))
                            tempRow[i] = value;
                        else
                            break;
                    }

                    dt.Rows.Add(tempRow);
                }
            }
            dt.Rows.RemoveAt(0);//remove header row
            return dt;
        }
        public string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document?.WorkbookPart?.SharedStringTablePart;
            string value = cell?.CellValue?.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        private Responce InsertFAQRequestFollowUp(DataTable registration)
        {
            Responce responce = new Responce();
            try
            {
                dbProxy _db = new dbProxy();
                GetStatus insertStatus = new GetStatus();
                DataSet ds = new DataSet();
                ds.Tables.Add(registration);
                string XmlParameter = ds.GetXml();

                List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@XMLParameter", Value = XmlParameter }
                    };

                insertStatus = _db.StoreData("Insert_FAQRequestFollowup_WithExcel", sp);
                if (insertStatus.returnStatus == "Success")
                {
                    responce.StatusCode = HttpStatusCode.OK;
                    responce.Message = insertStatus.returnStatus;
                }
            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }

            return responce;
        }
        #endregion

        #region Coupon Code
        public Responce CouponCodeList(FilterInput input)
        {
            Responce responce = new Responce();

            try
            {

                List<CouponCode> List = new List<CouponCode>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = input.QType },
                    new SetParameters{ ParameterName = "@Query", Value =input.Search==null?"":input.Search  },
                };
                List = _db.GetDataMultiple("GetCouponCodeList", List, sp);
                if (input.StartDate != null && input.StartDate.HasValue)
                {
                    input.StartDate = new DateTime(input.StartDate.Value.Year, input.StartDate.Value.Month, input.StartDate.Value.Day + 1, 0, 0, 0);
                    List = List.Where(x => Convert.ToDateTime(x.ValidityStartDate) >= input.StartDate.Value).ToList();
                }
                if (input.EndDate != null && input.EndDate.HasValue)
                {
                    input.EndDate = new DateTime(input.EndDate.Value.Year, input.EndDate.Value.Month, input.EndDate.Value.Day + 1, 23, 59, 59);
                    List = List.Where(x => Convert.ToDateTime(x.ValidityEndDate) <= input.EndDate.Value).ToList();
                }
                responce.TotalPage = (int)Math.Ceiling((double)List.Count() / input.itemsPerPage);
                input.Page = input.Page - 1;
                if (input.itemsPerPage != 0)
                {

                    List = List.Skip((input.itemsPerPage * input.Page)).Take(input.itemsPerPage).ToList();
                    responce.Result = List;
                }
                else
                    responce.Result = List;
                
                responce.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        public Responce GetCreateCouponCodeDetails()
        {
            Responce responce = new Responce();
            CreateCouponCodeModel coupon = new CreateCouponCodeModel();
            dbProxy _db = new dbProxy();

            #region Get Coupon Type
            try
            {
                List<CouponType> CouponTypeList = new List<CouponType>();
                List<SetParameters> CouponTypesp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@GetValue", Value ="CouponType"  },
                };
                CouponTypeList = _db.GetDataMultiple("GetCreateCouponCodeDetails", CouponTypeList, CouponTypesp);
                coupon.CouponTypeList = CouponTypeList;
            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.Message = ex.Message.ToString();
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            #endregion

            #region Get Coupon Usages Type
            try
            {
                List<CouponUsagesType> CouponUsagesTypeList = new List<CouponUsagesType>();
                List<SetParameters> CouponTypesp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@GetValue", Value ="CouponUsagesType"  },
                };
                CouponUsagesTypeList = _db.GetDataMultiple("GetCreateCouponCodeDetails", CouponUsagesTypeList, CouponTypesp);
                coupon.CouponUsagesTypeList = CouponUsagesTypeList;
            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.Message = ex.Message.ToString();
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            #endregion

            #region Get Discount Type
            try
            {
                List<DiscountTypes> DiscountTypesList = new List<DiscountTypes>();
                List<SetParameters> CouponTypesp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@GetValue", Value ="DiscountType"  },
                };
                DiscountTypesList = _db.GetDataMultiple("GetCreateCouponCodeDetails", DiscountTypesList, CouponTypesp);
                coupon.DiscountTypesList = DiscountTypesList;
            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.Message = ex.Message.ToString();
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            #endregion

            #region Get User Type
            try
            {
                List<UserTypeMaster> UserTypeList = new List<UserTypeMaster>();
                List<SetParameters> CouponTypesp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@GetValue", Value ="UserTypeMaster"  },
                };
                UserTypeList = _db.GetDataMultiple("GetCreateCouponCodeDetails", UserTypeList, CouponTypesp);
                coupon.UserTypeList = UserTypeList;
            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.Message = ex.Message.ToString();
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            #endregion

            #region Get Domain Name Master
            try
            {
                List<DomainNameMaster> DomainNameMaster = new List<DomainNameMaster>();
                List<SetParameters> CouponTypesp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@GetValue", Value ="DomainNameMaster"  },
                };
                DomainNameMaster = _db.GetDataMultiple("GetCreateCouponCodeDetails", DomainNameMaster, CouponTypesp);
                coupon.DomainNameMasterList = DomainNameMaster;
            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.Message = ex.Message.ToString();
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            #endregion

            coupon.AgeGroupItemForCouponCodeList = GetAgeGroupList();
            coupon.SubscriptionsItemForCouponCodeList = GetSubscriptionList();
            responce.Result = coupon;
            responce.StatusCode = HttpStatusCode.OK;
            return responce;
        }
        public List<AgeGroupItemForCouponCode> GetAgeGroupList()
        {
            List<AgeGroupItemForCouponCode> ageGroup = new List<AgeGroupItemForCouponCode>();
            var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
            var allAges = helper?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home").FirstOrDefault()?
                           .Children?.Where(x => x.ContentType.Alias == "masterRoot")?.FirstOrDefault()?
                           .Children?.Where(x => x.ContentType.Alias == "ageMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>()?.Where(x => x.IsActice);
            if (allAges != null && allAges.Count() > 0)
            {

                foreach (var item in allAges)
                {
                    ageGroup.Add(new AgeGroupItemForCouponCode()
                    {
                        AgeGroupId = item.Id,
                        AgeGroupName = item.ItemName,
                        Selected = false,
                    });
                }

            }
            return ageGroup;
        }
        public List<SubscriptionsItemForCouponCode> GetSubscriptionList()
        {
            List<SubscriptionsItemForCouponCode> SubscriptionList = new List<SubscriptionsItemForCouponCode>();
            var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
            var Subscription = helper?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home").FirstOrDefault()?
                           .Children?.Where(x => x.ContentType.Alias == "subscriptionTypeRoot")?.FirstOrDefault()?
                           .Children?.Where(x => x.ContentType.Alias == "subscriptionList")?.FirstOrDefault()?.Children?.OfType<Subscriptions>()?.Where(x => x.IsActive);
            if (Subscription != null && Subscription.Count() > 0)
            {

                foreach (var item in Subscription)
                {
                    SubscriptionList.Add(new SubscriptionsItemForCouponCode()
                    {
                        SubscriptionId = item.Id,
                        SubscriptionName = item.Name,
                        Ranking = item.Ranking,
                        Selected = false,
                    });
                }

            }
            return SubscriptionList;
        }
        public Responce CreateEditCouponCode(CreateCouponCodeModel input)
        {
            Responce responce = new Responce();

            try
            {
                dbProxy _db = new dbProxy();
                //if (input.CouponCodeId == 0)
                // {
                bool IsCouponCodeExist = false;
                List<CouponCode> List = new List<CouponCode>();

                List<SetParameters> CouponExist = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value =input.CouponCodeName  },
                };
                List = _db.GetDataMultiple("GetCouponCodeList", List, CouponExist);
                if (List == null || List.Count() == 0)
                    IsCouponCodeExist = false;
                else
                {
                    if (List.Where(x => x.CouponCodeId == input.CouponCodeId).Count() == 0)
                    {
                        IsCouponCodeExist = true;
                        responce.Message = "Coupon Code " + input.CouponCodeName + " Already Exist.. ";
                        responce.StatusCode = HttpStatusCode.Ambiguous;
                        return responce;
                    }
                    else
                        IsCouponCodeExist = false;
                }
                if (!IsCouponCodeExist)
                {

                    if (input.ValidityStartDate.HasValue)
                        input.ValidityStartDate = new DateTime(input.ValidityStartDate.Value.Year, input.ValidityStartDate.Value.Month, input.ValidityStartDate.Value.Day + 1, 0, 0, 0);
                    if (input.ValidityEndDate.HasValue)
                        if (input.CouponCodeId == 0)
                            input.ValidityEndDate = new DateTime(input.ValidityEndDate.Value.Year, input.ValidityEndDate.Value.Month, input.ValidityEndDate.Value.Day + 1, 23, 59, 59);
                        else
                            input.ValidityEndDate = new DateTime(input.ValidityEndDate.Value.Year, input.ValidityEndDate.Value.Month, input.ValidityEndDate.Value.Day, 23, 59, 59);
                    ;

                    GetStatus insertStatus = new GetStatus();

                    List<SetParameters> sp = new List<SetParameters>()
            {
                new SetParameters{ ParameterName = "@QType", Value = "1" },
                new SetParameters{ ParameterName = "@TransactionId", Value = input.TransactionId!=null?input.TransactionId:"" },
                new SetParameters{ ParameterName = "@CouponCodeId", Value = input.CouponCodeId.ToString() },
                new SetParameters{ ParameterName = "@CouponCodeName", Value = input.CouponCodeName },
                new SetParameters { ParameterName = "@CouponType", Value = input.CouponType },
                new SetParameters { ParameterName = "@CouponUsagesType", Value = input.CouponUsagesType },
                new SetParameters { ParameterName = "@NoOfUsages", Value = input.NoOfUsages.ToString() },
                new SetParameters { ParameterName = "@ValidityStartDate", Value = input.ValidityStartDate.ToString() },
                new SetParameters { ParameterName = "@ValidityEndDate", Value = input.ValidityEndDate.ToString()},
                new SetParameters { ParameterName = "@CouponSource", Value = input.CouponSource==null?"":input.CouponSource},
                new SetParameters { ParameterName = "@DiscountType", Value = input.DiscountType},
                new SetParameters { ParameterName = "@DiscountValue", Value = input.DiscountValue.ToString() },
                new SetParameters { ParameterName = "@IsAppliedForSubscription", Value = input.IsAppliedForSubscription.HasValue && input.IsAppliedForSubscription.Value?"1":"0" },
                new SetParameters { ParameterName = "@IsCouponAppliedForAgeGroup", Value = input.IsCouponAppliedForAgeGroup.HasValue && input.IsCouponAppliedForAgeGroup.Value?"1":"0"},
                new SetParameters { ParameterName = "@IsAppliedForMultipleUserType", Value = input.IsAppliedForMultipleUserType.HasValue &&input.IsAppliedForMultipleUserType.Value?"1":"0" },
                new SetParameters { ParameterName = "@UserType", Value = input.UserType},
                new SetParameters { ParameterName = "@UserId", Value = input.UserId.ToString()},
                new SetParameters { ParameterName = "@DomainId", Value = input.DomainId!=null?input.DomainId:""}
            };

                    insertStatus = _db.StoreData("InsertCouponCode", sp);
                    if (insertStatus != null && insertStatus.returnStatus != null)
                    {
                        if (insertStatus.returnStatus == "Success")
                        {
                            responce.Result = insertStatus.returnMessage;
                            if (insertStatus.returnValue > 0)
                            {
                                if (input.IsAppliedForMultipleUserType.HasValue && input.IsAppliedForMultipleUserType.Value)
                                    SaveCouponCodeUserTypeMaster(insertStatus.returnValue, input.UserType, input.UserId);

                                if (input.IsCouponAppliedForAgeGroup.HasValue && input.IsCouponAppliedForAgeGroup.Value)
                                    foreach (var item in input.AgeGroupItemForCouponCodeList)
                                    {
                                        SaveCouponCodeAgeGroup(insertStatus.returnValue, item, input.UserId);
                                    }
                                if (input.IsAppliedForSubscription.HasValue && input.IsAppliedForSubscription.Value)
                                    foreach (var item in input.SubscriptionsItemForCouponCodeList)
                                    {
                                        SaveCouponCodeSubscription(insertStatus.returnValue, item, input.UserId);
                                    }
                            }
                            responce.StatusCode = HttpStatusCode.OK;
                            responce.Message = insertStatus.returnMessage;
                        }
                        else
                        {
                            responce.Result = insertStatus.returnMessage;
                            responce.StatusCode = HttpStatusCode.InternalServerError;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        public bool SaveCouponCodeUserTypeMaster(int CouponCodeId, string userType, int UserId)
        {
            bool Result = false;
            List<CouponType> CouponTypeList = new List<CouponType>();
            dbProxy _db = new dbProxy();
            GetStatus insertStatus = new GetStatus();
            List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
                    new SetParameters{ ParameterName = "@UserType", Value =userType  },
                    new SetParameters{ ParameterName = "@CouponCodeId", Value =CouponCodeId.ToString()  },
                };
            insertStatus = _db.StoreData("InserCouponCodeUserTypeMatser", sp);
            if (insertStatus != null && insertStatus.returnStatus != null)
            {
                if (insertStatus.returnStatus == "Success")
                    Result = true;
                else
                    Result = false;
            }
            return Result;
        }
        public bool SaveCouponCodeAgeGroup(int CouponCodeId, AgeGroupItemForCouponCode ageGroup, int UserId)
        {
            bool Result = false;
            List<CouponType> CouponTypeList = new List<CouponType>();
            dbProxy _db = new dbProxy();
            GetStatus insertStatus = new GetStatus();
            List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
                    new SetParameters{ ParameterName = "@AgeGroup", Value =ageGroup.AgeGroupId.ToString()  },
                    new SetParameters{ ParameterName = "@CouponCodeId", Value =CouponCodeId.ToString()  },
                };
            insertStatus = _db.StoreData("InserCouponCodeAgeGroup", sp);
            if (insertStatus != null && insertStatus.returnStatus != null)
            {
                if (insertStatus.returnStatus == "Success")
                    Result = true;
                else
                    Result = false;
            }
            return Result;
        }
        public bool SaveCouponCodeSubscription(int CouponCodeId, SubscriptionsItemForCouponCode subscriptionsItemForCoupon, int UserId)
        {
            bool Result = false;
            List<CouponType> CouponTypeList = new List<CouponType>();
            dbProxy _db = new dbProxy();
            GetStatus insertStatus = new GetStatus();
            List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() },
                    new SetParameters{ ParameterName = "@Ranking", Value =subscriptionsItemForCoupon.Ranking.ToString()  },
                    new SetParameters{ ParameterName = "@CouponCodeId", Value =CouponCodeId.ToString()  },
                };
            insertStatus = _db.StoreData("InserCouponCodeSubscriptionRanking", sp);
            if (insertStatus != null && insertStatus.returnStatus != null)
            {
                if (insertStatus.returnStatus == "Success")
                    Result = true;
                else
                    Result = false;
            }
            return Result;
        }
        public Responce GetCouponCodeEditDetails(int CouponCodeId)
        {
            Responce responce = new Responce();
            try
            {
                dbProxy _db = new dbProxy();
                CreateCouponCodeModel CreateCouponCodeModel = new CreateCouponCodeModel();
                responce = GetCreateCouponCodeDetails();
                if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                {
                    CreateCouponCodeModel = responce.Result as CreateCouponCodeModel;
                }
                CreateCouponCodeModel EditCode = new CreateCouponCodeModel();
                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value =""  },
                    new SetParameters{ ParameterName = "@CouponCodeId", Value =CouponCodeId.ToString()  },
                };
                EditCode = _db.GetData<CreateCouponCodeModel>("GetCouponCodeList", EditCode, sp);
                if (EditCode != null && EditCode.CouponCodeId > 0)
                {
                    CreateCouponCodeModel.CouponCodeId = EditCode.CouponCodeId;
                    CreateCouponCodeModel.CouponCodeName = EditCode.CouponCodeName;
                    CreateCouponCodeModel.CouponType = EditCode.CouponType;
                    CreateCouponCodeModel.CouponUsagesType = EditCode.CouponUsagesType;
                    CreateCouponCodeModel.DiscountType = EditCode.DiscountType;
                    CreateCouponCodeModel.IsAppliedForMultipleUserType = EditCode.IsAppliedForMultipleUserType;
                    CreateCouponCodeModel.IsCouponAppliedForAgeGroup = EditCode.IsCouponAppliedForAgeGroup;
                    CreateCouponCodeModel.IsAppliedForSubscription = EditCode.IsAppliedForSubscription;
                    CreateCouponCodeModel.NoOfUsages = EditCode.NoOfUsages;
                    CreateCouponCodeModel.CouponSource = EditCode.CouponSource;
                    CreateCouponCodeModel.ValidityStartDate = EditCode.ValidityStartDate;
                    CreateCouponCodeModel.ValidityStartDate = EditCode.ValidityStartDate.Value.AddDays(-1);
                    CreateCouponCodeModel.ValidityEndDate = EditCode.ValidityEndDate;
                    CreateCouponCodeModel.DiscountValue = EditCode.DiscountValue;
                    CreateCouponCodeModel.UserType = EditCode.UserType;
                    CreateCouponCodeModel.DomainId = EditCode.DomainId;

                    if (CreateCouponCodeModel.CouponTypeList != null && CreateCouponCodeModel.CouponTypeList.Count() > 0)
                        foreach (var item in CreateCouponCodeModel.CouponTypeList)
                        {
                            if (item.CouponTypeId > 0 && item.CouponTypeId.ToString().Equals(EditCode.CouponType))
                                item.Selected = true;
                        }

                    if (EditCode.IsAppliedForMultipleUserType.HasValue && EditCode.IsAppliedForMultipleUserType.Value && CreateCouponCodeModel.UserTypeList != null && CreateCouponCodeModel.UserTypeList.Count() > 0)
                        foreach (var item in CreateCouponCodeModel.UserTypeList)
                        {
                            if (item.UserTypeId > 0 && item.UserTypeId.ToString().Equals(EditCode.UserType))
                                item.Selected = true;
                        }

                    if (CreateCouponCodeModel.CouponUsagesTypeList != null && CreateCouponCodeModel.CouponUsagesTypeList.Count() > 0)
                        foreach (var item in CreateCouponCodeModel.CouponUsagesTypeList)
                        {
                            if (item.CouponUsagesTypeId > 0 && item.CouponUsagesTypeId.ToString().Equals(EditCode.CouponUsagesType))
                                item.Selected = true;
                        }

                    if (CreateCouponCodeModel.DiscountTypesList != null && CreateCouponCodeModel.DiscountTypesList.Count() > 0)
                        foreach (var item in CreateCouponCodeModel.DiscountTypesList)
                        {
                            if (item.DiscountType > 0 && item.DiscountType.ToString().Equals(EditCode.DiscountType))
                                item.Selected = true;
                        }
                    if (EditCode.IsCouponAppliedForAgeGroup.HasValue && EditCode.IsCouponAppliedForAgeGroup.Value && CreateCouponCodeModel.AgeGroupItemForCouponCodeList != null && CreateCouponCodeModel.AgeGroupItemForCouponCodeList.Count() > 0 && EditCode.SelectedAgeGroup != null)
                    {
                        var SelectedAgeArray = EditCode.SelectedAgeGroup.Split(',');
                        foreach (var item in CreateCouponCodeModel.AgeGroupItemForCouponCodeList)
                        {
                            if (SelectedAgeArray.Where(x => x.Equals(item.AgeGroupId.ToString())).Count() > 0)
                                item.Selected = true;
                        }
                    }

                    if (EditCode.IsAppliedForSubscription.HasValue && EditCode.IsAppliedForSubscription.Value && CreateCouponCodeModel.SubscriptionsItemForCouponCodeList != null && CreateCouponCodeModel.SubscriptionsItemForCouponCodeList.Count() > 0 && EditCode.SelectedSubscription != null)
                    {
                        var SelectedSubArray = EditCode.SelectedSubscription.Split(',');
                        foreach (var item in CreateCouponCodeModel.SubscriptionsItemForCouponCodeList)
                        {
                            if (SelectedSubArray.Where(x => x.Equals(item.Ranking.ToString())).Count() > 0)
                                item.Selected = true;
                        }
                    }
                    responce.Result = CreateCouponCodeModel;
                    responce.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.Message;
            }
            return responce;

        }
        public Responce CouponCodeStatusChange(int CouponCodeId, bool Status, int UserId)
        {
            Responce responce = new Responce();
            try
            {
                dbProxy _db = new dbProxy();
                GetStatus insertStatus = new GetStatus();
                string QType = "";
                QType = Status ? "2" : "1";
                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = QType },
                    new SetParameters{ ParameterName = "@CouponCodeId", Value =CouponCodeId.ToString()  },
                    new SetParameters{ ParameterName = "@UserId", Value =UserId.ToString()  },
                };
                insertStatus = _db.StoreData("CouponCodeChangeStatus", sp);
                if (insertStatus != null && insertStatus.returnStatus != null)
                {
                    if (insertStatus.returnStatus == "Success")
                    {
                        responce.StatusCode = HttpStatusCode.OK;
                        responce.Message = insertStatus.returnMessage;
                    }
                    else
                    {
                        responce.StatusCode = HttpStatusCode.InternalServerError;
                        responce.Message = insertStatus.returnMessage;
                    }
                }

            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.Message;
            }
            return responce;
        }

        public Responce CouponCodeDelete(string CouponCodeId, int UserId)
        {
            Responce responce = new Responce();
            try
            {
                dbProxy _db = new dbProxy();
                GetStatus insertStatus = new GetStatus();
                string QType = "";
                QType = "3";
                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = QType },
                    new SetParameters{ ParameterName = "@CouponCodeId", Value =CouponCodeId.ToString()  },
                    new SetParameters{ ParameterName = "@UserId", Value =UserId.ToString()  },
                };
                insertStatus = _db.StoreData("CouponCodeChangeStatus", sp);
                if (insertStatus != null && insertStatus.returnStatus != null)
                {
                    if (insertStatus.returnStatus == "Success")
                    {
                        responce.StatusCode = HttpStatusCode.OK;
                        responce.Message = insertStatus.returnMessage;
                    }
                    else
                    {
                        responce.StatusCode = HttpStatusCode.InternalServerError;
                        responce.Message = insertStatus.returnMessage;
                    }
                }

            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.Message;
            }
            return responce;
        }
        #endregion

        #region Coupon code Insert From Excel File
        public Responce ImportCouponCodeFromExcelToTable(string LocalSaveFilePath, int UserId)
        {
            DataTable dt = new DataTable();
            DataTable dtAll = new DataTable();
            Responce responce = new Responce();
            dbProxy _db = new dbProxy();
            try
            {
                var files = Directory.EnumerateFiles(LocalSaveFilePath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".csv") || s.EndsWith(".xlsx") || s.EndsWith(".xls"));
                if (files != null && files.Count() > 0)
                {

                    List<CreateCouponCodeModelTemp> TempList = new List<CreateCouponCodeModelTemp>();
                    foreach (var item in files)
                    {
                        string path1 = string.Format("{0}/{1}", LocalSaveFilePath, Path.GetFileName(item));
                        if (System.IO.File.Exists(item))
                        {
                            dt = ConvertCSVtoDataTable(item);
                            if (dt.Rows.Count > 0)
                            {
                                dt.Columns.Add("UploadStatus", typeof(String));
                                dt.Columns.Add("Reason", typeof(String));
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        string Reason = "";
                                        CreateCouponCodeModelTemp Temp = new CreateCouponCodeModelTemp();

                                        Temp.CouponCodeName = Convert.ToString(dt.Rows[i]["CouponCodeName"]);
                                        Temp.CouponType = Convert.ToString(dt.Rows[i]["CouponType"]);
                                        Temp.CouponUsagesType = Convert.ToString(dt.Rows[i]["CouponUsagesType"]);
                                        Temp.NoOfUsages = Convert.ToInt32(dt.Rows[i]["NoOfUsages"]);
                                        Temp.ValidityStartDate = Convert.ToDateTime(dt.Rows[i]["ValidityStartDate"]);
                                        Temp.ValidityEndDate = Convert.ToDateTime(dt.Rows[i]["ValidityEndDate"]);
                                        Temp.CouponSource = Convert.ToString(dt.Rows[i]["CouponSource"]);
                                        Temp.DiscountType = Convert.ToString(dt.Rows[i]["DiscountType"]);
                                        Temp.DiscountValue = Convert.ToInt32(dt.Rows[i]["DiscountValue"]);
                                        Temp.DomainId = Convert.ToString(dt.Rows[i]["DomainId"]);
                                        string SubscriptionBool = dt.Rows[i]["IsAppliedForSubscription"].ToString();
                                        if (!string.IsNullOrWhiteSpace(SubscriptionBool))
                                        {
                                            try
                                            {
                                                bool BoolValues = Boolean.Parse(SubscriptionBool);
                                                Temp.IsAppliedForSubscription = BoolValues;
                                            }
                                            catch (ArgumentException)
                                            {
                                                Reason += SubscriptionBool+" Is Applied For Subscription is Invalid Format.,";
                                            }
                                            catch (FormatException)
                                            {
                                                Reason += SubscriptionBool +" Is Applied For Subscription is Invalid Format.,";
                                            }
                                        }
                                       
                                        string AgeGroupBool = dt.Rows[i]["IsCouponAppliedForAgeGroup"].ToString();
                                        if (!string.IsNullOrWhiteSpace(AgeGroupBool))
                                        {
                                            try
                                            {
                                                bool BoolValues = Boolean.Parse(AgeGroupBool);
                                                Temp.IsCouponAppliedForAgeGroup = BoolValues;
                                            }
                                            catch (ArgumentException)
                                            {
                                                Reason += AgeGroupBool + " Is Applied For Age Group is Invalid Format.,";
                                            }
                                            catch (FormatException)
                                            {
                                                Reason += AgeGroupBool + " Is Applied For Age Group is Invalid Format.,";
                                            }
                                        }
                                        
                                        string MultipleUserTypeBool = dt.Rows[i]["IsAppliedForMultipleUserType"].ToString();
                                        if (!string.IsNullOrWhiteSpace(MultipleUserTypeBool))
                                        {
                                            try
                                            {
                                                bool BoolValues = Boolean.Parse(MultipleUserTypeBool);
                                                Temp.IsAppliedForMultipleUserType = BoolValues;
                                            }
                                            catch (ArgumentException)
                                            {
                                                Reason += MultipleUserTypeBool + " Is Applied For Multiple User Type is Invalid Format.,";
                                            }
                                            catch (FormatException)
                                            {
                                                Reason += MultipleUserTypeBool + " Is Applied For Multiple User Type is Invalid Format.,";
                                            }
                                        }
                                        
                                        Temp.UserType = Convert.ToString(dt.Rows[i]["UserType"]);
                                        Temp.SelectedAgeGroup = Convert.ToString(dt.Rows[i]["SelectedAgeGroup"]);
                                        Temp.SelectedSubscription = Convert.ToString(dt.Rows[i]["SelectedSubscription"]);
                                        Temp.UserId = UserId;
                                        List<CouponCode> CopuonAlreadyExistList = new List<CouponCode>();
                                        List<SetParameters> CouponExist = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value =Temp.CouponCodeName  },
                };
                                        CopuonAlreadyExistList = _db.GetDataMultiple("GetCouponCodeList", CopuonAlreadyExistList, CouponExist);
                                        if (CopuonAlreadyExistList != null && CopuonAlreadyExistList.Count() > 0)
                                        {
                                            Reason += "Coupon Code " + Temp.CouponCodeName + " Already Exist.,";
                                        }
                                        Reason += CheckRequiredValidation(Temp);
                                        Reason += checkValidCopuponCode(Temp);
                                        Reason = Reason.TrimEnd(',');
                                        if (String.IsNullOrEmpty(Reason))
                                        {
                                            Temp.Status = "Ok";
                                            dt.Rows[i]["UploadStatus"] = "Ok";
                                        }
                                        else
                                        {
                                            Temp.Status = "Fail";
                                            dt.Rows[i]["UploadStatus"] = "Fail";
                                        }
                                        dt.Rows[i]["Reason"] = Reason;
                                        Temp.Reason = Reason;
                                        TempList.Add(Temp);
                                    }
                                    dtAll.Merge(dt);
                                }
                            }
                            else
                            {
                                DeleteCouponCodeFromExcelFiles(LocalSaveFilePath);
                                responce.Result = null;
                                responce.Message = "Invalie Excel File Format Please Try again.";
                                responce.StatusCode = HttpStatusCode.InternalServerError;
                                return responce;
                            }
                        }
                    }

                    if (TempList != null && TempList.Any() && dtAll != null && dtAll.Rows.Count > 0)
                    {
                        Responce Save = new Responce();
                        //Save = InsertCouponCodeFromExcel(dtAll);
                        Save = InsertCouponCodeFromExcelWithList(TempList.Where(x => x.Status.Equals("Ok") && string.IsNullOrWhiteSpace(x.Reason)).ToList());
                        Save = InsertCouponCodeFromExcelTempWithList(TempList.Where(x => x.Status.Equals("Fail") && !string.IsNullOrWhiteSpace(x.Reason)).ToList());

                    }
                    DeleteCouponCodeFromExcelFiles(LocalSaveFilePath);
                    responce.Result = TempList;
                    responce.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    responce.Result = null;
                    responce.Message = "Excel File Not Found..";
                    responce.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                DeleteCouponCodeFromExcelFiles(LocalSaveFilePath);
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            finally
            {
                DeleteCouponCodeFromExcelFiles(LocalSaveFilePath);
            }
            return responce;
        }
        public static void DeleteCouponCodeFromExcelFiles(string LocalSaveFilePath)
        {
            try
            {
                var Excelfiles = Directory.EnumerateFiles(LocalSaveFilePath, "*.*", SearchOption.AllDirectories)
                      .Where(s => s.EndsWith(".csv") || s.EndsWith(".xlsx") || s.EndsWith(".xls"));
                if (Excelfiles != null && Excelfiles.Count() > 0)
                {
                    foreach (var item in Excelfiles)
                    {
                        if (item != null)
                        {
                            System.IO.File.Delete(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        private static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            FileInfo existingFile = new FileInfo(strFilePath);
            if (!existingFile.Extension.StartsWith(".csv"))
            {
                try
                {


                    using (ExcelPackage package = new ExcelPackage(existingFile))
                    {
                        //get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        int colCount = worksheet.Dimension.End.Column;  //get Column Count
                        int rowCount = worksheet.Dimension.End.Row;     //get row count

                        for (int col = 1; col <= colCount; col++)
                        {
                            //if (worksheet.Cells[1, col].Value != null && !string.IsNullOrWhiteSpace(worksheet.Cells[1, col].Value.ToString()))
                            dt.Columns.Add(worksheet.Cells[1, col].Value?.ToString().Trim());
                            //else
                            //    break;
                        }

                        for (int row = 1; row <= rowCount; row++)
                        {
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < colCount; i++)
                            {
                                string dta = worksheet.Cells[row + 1, i + 1].Value?.ToString().Trim();
                                double doubleValue;
                                bool IsDouble = double.TryParse(dta, out doubleValue);
                                if (IsDouble)
                                {
                                    if (i == 5 || i == 6)
                                    {
                                        if (i == 5)
                                        {

                                            DateTime conv = DateTime.FromOADate(doubleValue);
                                            dr[i] = new DateTime(conv.Year, conv.Month, conv.Day, 0, 0, 0);
                                        }
                                        if (i == 6)
                                        {
                                            DateTime conv = DateTime.FromOADate(doubleValue);
                                            dr[i] = new DateTime(conv.Year, conv.Month, conv.Day, 23, 59, 59);
                                        }
                                    }
                                    else
                                    {
                                        dr[i] = dta;
                                    }
                                }
                                else
                                {
                                    dr[i] = dta;
                                }

                            }
                            if (!AreAllColumnsEmpty(dr))
                            {
                                dt.Rows.Add(dr);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {

                using (StreamReader sr = new StreamReader(strFilePath))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }

                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        if (rows.Length > 1)
                        {
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                dr[i] = rows[i].Trim();
                            }
                            dt.Rows.Add(dr);
                        }
                    }

                }

            }
            return dt;
        }
        public static bool AreAllColumnsEmpty(DataRow dr)
        {
            if (dr == null)
            {
                return true;
            }
            else
            {
                foreach (var value in dr.ItemArray)
                {
                    if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private static string CheckRequiredValidation(CreateCouponCodeModelTemp CouponCode)
        {
            string Reason = string.Empty;
            try
            {

                if (String.IsNullOrWhiteSpace(CouponCode.CouponType))
                    Reason += "Coupon Type can not be blank.,";
                if (CouponCode.IsAppliedForMultipleUserType.HasValue && CouponCode.IsAppliedForMultipleUserType.Value)
                {
                    if (string.IsNullOrWhiteSpace(CouponCode.UserType))
                        Reason += "User Type can not be blank.,";
                }
                if (String.IsNullOrWhiteSpace(CouponCode.CouponUsagesType))
                    Reason += "Coupon Usages Type can not be blank.,";
                if (String.IsNullOrWhiteSpace(CouponCode.CouponCodeName))
                    Reason = "CouponCode Name cn not be blank.,";
                if (!CouponCode.NoOfUsages.HasValue)
                    Reason += "No Of Usages can not be blank.,";
                if (!CouponCode.ValidityStartDate.HasValue)
                    Reason += "Validity Start Date can not be blank.,";
                if (!CouponCode.ValidityEndDate.HasValue)
                    Reason += "Validity End Date can not be blank.,";
                if (String.IsNullOrWhiteSpace(CouponCode.DiscountType))
                    Reason += "Discount Type can not be blank.,";
                if (!CouponCode.DiscountValue.HasValue)
                    Reason += "Discount Value can not be blank.,";
                if (CouponCode.IsCouponAppliedForAgeGroup.HasValue && CouponCode.IsCouponAppliedForAgeGroup.Value)
                {
                    if (string.IsNullOrWhiteSpace(CouponCode.SelectedAgeGroup))
                        Reason += "Age Group can not be blank.,";
                }
                if (CouponCode.IsAppliedForSubscription.HasValue && CouponCode.IsAppliedForSubscription.Value)
                {
                    if (string.IsNullOrWhiteSpace(CouponCode.SelectedSubscription))
                        Reason += "Subscription can not be blank.,";
                }

            }
            catch (Exception ex)
            {
                Reason = ex.ToString();
            }
            return Reason;
        }

        private static string checkValidCopuponCode(CreateCouponCodeModelTemp Temp)
        {
            string Reason = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(Temp.CouponType))
                {
                    if (Temp.CouponType.Equals("0") || Convert.ToInt32(Temp.CouponType) <= 0)
                    {
                        Reason += "Invalid Coupon Type. Coupon Type must be greater than zero.,";
                    }
                }

                if (!string.IsNullOrWhiteSpace(Temp.CouponCodeName))
                {
                    if (!Regex.IsMatch(Temp.CouponCodeName, @"^[a-zA-Z0-9]+$"))
                    {
                        Reason = "Coupon Code must be in Alphabate characters and Number.,";
                    }
                }
                if (Temp.ValidityStartDate.HasValue)
                {
                    DateTime dDate;
                    if (DateTime.TryParse(Temp.ValidityStartDate.Value.ToString(), out dDate))
                    {
                        String.Format("{0:d/MM/yyyy}", dDate);

                        if (Temp.ValidityStartDate.Value.ToString() != "" && !(Temp.ValidityStartDate.Value >= DateTime.Today))
                        {
                            Reason += "Start Date should be earlier or equal To Today Date.,";
                        }
                    }
                    else
                    {
                        Reason += "Start Date Invalid Date Format.,";
                    }
                }
                if (Temp.ValidityEndDate.HasValue && Temp.ValidityStartDate.HasValue)
                {
                    DateTime dDate;
                    if (DateTime.TryParse(Temp.ValidityStartDate.Value.ToString(), out dDate))
                    {
                        if (Temp.ValidityEndDate.Value.ToString() != "" && Temp.ValidityStartDate.Value.ToString() != "" && !(Temp.ValidityEndDate.Value >= Temp.ValidityStartDate.Value))
                        {
                            Reason += "End date must be later than start date.,";
                        }
                    }
                    else
                    {
                        Reason += "End date invalid Date Format.,";
                    }
                }
                if (!string.IsNullOrWhiteSpace(Temp.CouponUsagesType))
                {
                    if (Temp.NoOfUsages.Value.ToString().Equals("0") || Temp.NoOfUsages.Value <= 0)
                    {
                        Reason += "Invalid No Of Usages.No Of Usages must be greater than zero.,";
                    }
                    if (!Temp.CouponUsagesType.ToString().Equals("0") || Convert.ToInt32(Temp.CouponUsagesType) > 0)
                    {
                        if (Temp.CouponUsagesType.Equals("1"))//single used
                        {

                            if (!Temp.NoOfUsages.Value.ToString().Equals("1"))
                            {
                                Reason += "Invalid No Of Usages.No Of Usages must be 1.,";
                            }
                        }
                        if (Temp.CouponUsagesType.Equals("2"))//multiple used
                        {
                            if (!Regex.IsMatch(Temp.NoOfUsages.Value.ToString(), @"^[0-9]{1,3}$"))
                            {
                                Reason += "Invalid No Of Usages.,";
                            }
                        }
                    }
                    else
                    {
                        Reason += "Invalid Coupon Usages Type.,";
                    }

                }
                if (!string.IsNullOrWhiteSpace(Temp.DiscountType))
                {
                    if (Temp.DiscountValue.Value.ToString().Equals("0") || Temp.DiscountValue.Value <= 0)//Percent used
                    {
                        Reason += "Invalid Discount Value.Discount Value must be greater than zero.,";
                    }
                    if (!Temp.DiscountType.ToString().Equals("0") || Convert.ToInt32(Temp.DiscountType)> 0)
                    {

                        if (Temp.DiscountType.Equals("1"))//Percent used
                        {
                            if (!Regex.IsMatch(Temp.DiscountValue.Value.ToString(), @"^[0-9]{1,2}$"))
                            {
                                Reason += "Invalid Discount Percent.Discount Percent must be less then 99.,";
                            }
                        }
                        if (Temp.DiscountType.Equals("2"))//Fixed used
                        {
                            if (!Regex.IsMatch(Temp.DiscountValue.Value.ToString(), @"^[0-9]{1,3}$"))
                            {
                                Reason += "Invalid Discount Value. Discount Value must be less then 999.,";
                            }
                        }
                    }
                    else
                    {
                        Reason += "Invalid Discount Type.,";
                    }

                }
                if (Temp.IsCouponAppliedForAgeGroup.HasValue && Temp.IsCouponAppliedForAgeGroup.Value && !string.IsNullOrWhiteSpace(Temp.SelectedAgeGroup))
                {
                    string[] ageGroup = Temp.SelectedAgeGroup.Split('|');
                    if (ageGroup.Length == 0)
                    {
                        Reason += "Please mention age group with comma seperated.,";
                    }
                    if (ageGroup.Length > 0)
                    {
                        try
                        {
                            foreach (var ages in ageGroup)
                            {

                                if (ages == null || string.IsNullOrWhiteSpace(ages) || !ages.All(char.IsDigit))
                                {

                                    Reason += ages + " Mention age group is not in correct format.,";
                                }
                                if (ages.ToString().Equals("0") || Convert.ToInt32(ages) <= 0)
                                {
                                    Reason += ages + " Mention age group is not in correct format. age group value must be greater than zero.,";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Reason += "Mention age group is not in correct format.,";
                        }
                    }
                }
                if (Temp.IsAppliedForSubscription.HasValue && Temp.IsAppliedForSubscription.Value && !string.IsNullOrWhiteSpace(Temp.SelectedSubscription))
                {
                    string[] Subscription = Temp.SelectedSubscription.Split('|');
                    if (Subscription.Length == 0)
                    {
                        Reason += "Please mention Subscription with comma seperated.,";
                    }
                    if (Subscription.Length > 0)
                    {
                        try
                        {
                            foreach (var sub in Subscription)
                            {
                                if (sub == null || !sub.All(char.IsDigit))
                                    Reason += sub + " Mention Subscription is not in correct format.,";
                                if (!string.IsNullOrWhiteSpace(sub) && sub.ToString().Equals("0") || Convert.ToInt32(sub) <= 0)
                                {
                                    Reason += sub + " Mention Subscription is not in correct format. Subscription value must be greater than zero.,";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Reason += "Mention Subscription is not in correct format.,";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Reason = ex.ToString();
            }
            return Reason;
        }
        private static Responce InsertCouponCodeFromExcel(DataTable registration)
        {
            Responce responce = new Responce();
            dbProxy _db = new dbProxy();
            GetStatus insertStatus = new GetStatus();
            DataSet ds = new DataSet();
            ds.Tables.Add(registration);
            string XmlParameter = ds.GetXml();

            List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@XMLParameter", Value = XmlParameter }
                    };

            insertStatus = _db.StoreData("Insert_Registration_WithExcel", sp);
            if (insertStatus.returnStatus == "Success")
            {
                responce.StatusCode = HttpStatusCode.OK;
                responce.Message = insertStatus.returnStatus;
            }
            return responce;
        }
        private Responce InsertCouponCodeFromExcelWithList(List<CreateCouponCodeModelTemp> Temp)
        {
            Responce responce = new Responce();

            try
            {
                dbProxy _db = new dbProxy();
                if (Temp != null && Temp.Count() > 0)
                {
                    string TransactionId = clsCommon.GenerateTransactionId();
                    foreach (var input in Temp)
                    {
                        input.TransactionId = TransactionId;
                        if (input.CouponCodeId == 0)
                        {
                            List<CouponCode> List = new List<CouponCode>();

                            List<SetParameters> CouponExist = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value =input.CouponCodeName  },
                };
                            List = _db.GetDataMultiple("GetCouponCodeList", List, CouponExist);
                            if (List == null || List.Count() == 0)
                            {
                                GetStatus insertStatus = new GetStatus();
                                List<SetParameters> sp = new List<SetParameters>()
            {
                new SetParameters{ ParameterName = "@QType", Value = "1" },
                 new SetParameters{ ParameterName = "@TransactionId", Value = input.TransactionId!=null?input.TransactionId:"" },
                new SetParameters{ ParameterName = "@CouponCodeName", Value = input.CouponCodeName },
                new SetParameters { ParameterName = "@CouponType", Value = input.CouponType },
                new SetParameters { ParameterName = "@CouponUsagesType", Value = input.CouponUsagesType },
                new SetParameters { ParameterName = "@NoOfUsages", Value = input.NoOfUsages.ToString() },
                new SetParameters { ParameterName = "@ValidityStartDate", Value = input.ValidityStartDate.ToString() },
                new SetParameters { ParameterName = "@ValidityEndDate", Value = input.ValidityEndDate.ToString()},
                new SetParameters { ParameterName = "@CouponSource", Value = input.CouponSource==null?"":input.CouponSource},
                new SetParameters { ParameterName = "@DiscountType", Value = input.DiscountType},
                new SetParameters { ParameterName = "@DiscountValue", Value = input.DiscountValue.ToString() },
                new SetParameters { ParameterName = "@IsAppliedForSubscription", Value = input.IsAppliedForSubscription.HasValue && input.IsAppliedForSubscription.Value?"1":"0" },
                new SetParameters { ParameterName = "@IsCouponAppliedForAgeGroup", Value = input.IsCouponAppliedForAgeGroup.HasValue && input.IsCouponAppliedForAgeGroup.Value?"1":"0"},
                new SetParameters { ParameterName = "@IsAppliedForMultipleUserType", Value = input.IsAppliedForMultipleUserType.HasValue &&input.IsAppliedForMultipleUserType.Value?"1":"0" },
                new SetParameters { ParameterName = "@UserType", Value = input.UserType},
                new SetParameters { ParameterName = "@UserId", Value = input.UserId.ToString()},
                new SetParameters { ParameterName = "@DomainId", Value = input.DomainId.ToString()}
            };

                                insertStatus = _db.StoreData("InsertCouponCode", sp);
                                if (insertStatus != null && insertStatus.returnStatus != null)
                                {
                                    if (insertStatus.returnStatus == "Success")
                                    {
                                        responce.Result = insertStatus.returnValue;
                                        if (insertStatus.returnValue > 0)
                                        {
                                            if (input.IsAppliedForMultipleUserType.HasValue && input.IsAppliedForMultipleUserType.Value)
                                                SaveCouponCodeUserTypeMaster(insertStatus.returnValue, input.UserType, input.UserId);

                                            if (input.IsCouponAppliedForAgeGroup.HasValue && input.IsCouponAppliedForAgeGroup.Value)
                                            {
                                                foreach (var item in input.SelectedAgeGroup.Split('|'))
                                                {
                                                    AgeGroupItemForCouponCode ageGroup = new AgeGroupItemForCouponCode()
                                                    {
                                                        AgeGroupId = Convert.ToInt32(item?.ToString()),
                                                    };
                                                    SaveCouponCodeAgeGroup(insertStatus.returnValue, ageGroup, input.UserId);
                                                }
                                            }
                                            if (input.IsAppliedForSubscription.HasValue && input.IsAppliedForSubscription.Value)
                                            {

                                                foreach (var item in input.SelectedSubscription.Split('|'))
                                                {
                                                    SubscriptionsItemForCouponCode subscriptions = new SubscriptionsItemForCouponCode()
                                                    {
                                                        Ranking = item,
                                                    };
                                                    SaveCouponCodeSubscription(insertStatus.returnValue, subscriptions, input.UserId);
                                                }
                                            }
                                        }
                                        responce.StatusCode = HttpStatusCode.OK;
                                        responce.Message = insertStatus.returnMessage;
                                    }
                                    else
                                    {
                                        responce.Result = insertStatus.returnMessage;
                                        responce.StatusCode = HttpStatusCode.InternalServerError;
                                    }
                                }
                            }
                            else
                            {
                                responce.Message = "Coupon Code " + input.CouponCodeName + " Already Exist.. ";
                                responce.StatusCode = HttpStatusCode.Ambiguous;
                                return responce;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        private Responce InsertCouponCodeFromExcelTempWithList(List<CreateCouponCodeModelTemp> Temp)
        {
            Responce responce = new Responce();

            try
            {
                dbProxy _db = new dbProxy();
                if (Temp != null && Temp.Count() > 0)
                {
                    GetStatus TruncateStatus = new GetStatus();
                    List<SetParameters> TruncateTemp = new List<SetParameters>()
                    {
                        new SetParameters{ParameterName="@QType",Value="1"}
                    };
                    TruncateStatus = _db.StoreData("TruncateCouponCode_Temp", TruncateTemp);
                    if (TruncateStatus != null && TruncateStatus.returnStatus != null)
                    {
                        foreach (var input in Temp)
                        {
                            if (input.CouponCodeId == 0)
                            {
                                GetStatus insertStatus = new GetStatus();
                                List<SetParameters> sp = new List<SetParameters>()
            {
                new SetParameters{ ParameterName = "@QType", Value = "1" },
                new SetParameters{ ParameterName = "@CouponCodeName", Value = input.CouponCodeName },
                new SetParameters { ParameterName = "@CouponType", Value = input.CouponType },
                new SetParameters { ParameterName = "@CouponUsagesType", Value = input.CouponUsagesType },
                new SetParameters { ParameterName = "@NoOfUsages", Value = input.NoOfUsages.ToString() },
                new SetParameters { ParameterName = "@ValidityStartDate", Value = input.ValidityStartDate.ToString() },
                new SetParameters { ParameterName = "@ValidityEndDate", Value = input.ValidityEndDate.ToString()},
                new SetParameters { ParameterName = "@CouponSource", Value = input.CouponSource==null?"":input.CouponSource},
                new SetParameters { ParameterName = "@DiscountType", Value = input.DiscountType},
                new SetParameters { ParameterName = "@DiscountValue", Value = input.DiscountValue.ToString() },
                new SetParameters { ParameterName = "@IsAppliedForSubscription", Value = input.IsAppliedForSubscription.HasValue && input.IsAppliedForSubscription.Value?"1":"0" },
                new SetParameters { ParameterName = "@IsCouponAppliedForAgeGroup", Value = input.IsCouponAppliedForAgeGroup.HasValue && input.IsCouponAppliedForAgeGroup.Value?"1":"0"},
                new SetParameters { ParameterName = "@IsAppliedForMultipleUserType", Value = input.IsAppliedForMultipleUserType.HasValue &&input.IsAppliedForMultipleUserType.Value?"1":"0" },
                new SetParameters { ParameterName = "@UserType", Value = input.UserType},
                new SetParameters { ParameterName = "@UserId", Value = input.UserId.ToString()},
                new SetParameters { ParameterName = "@UploadStatus", Value = input.Status},
                new SetParameters { ParameterName = "@Reason", Value = input.Reason.ToString()}
            };

                                insertStatus = _db.StoreData("InsertCouponCodeTemp", sp);
                                if (insertStatus != null && insertStatus.returnStatus != null)
                                {
                                    if (insertStatus.returnStatus == "Success")
                                    {
                                        responce.Result = insertStatus.returnValue;
                                        responce.StatusCode = HttpStatusCode.OK;
                                        responce.Message = insertStatus.returnMessage;
                                    }
                                    else
                                    {
                                        responce.Result = insertStatus.returnMessage;
                                        responce.StatusCode = HttpStatusCode.InternalServerError;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        #endregion

        #region Coupon Code Log
        public Responce CouponCodeLogList(FilterInput input)
        {
            Responce responce = new Responce();

            try
            {

                List<CouponCodeLog> List = new List<CouponCodeLog>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = input.QType },
                    new SetParameters{ ParameterName = "@Query", Value =input.Search==null?"":input.Search  },
                };
                List = _db.GetDataMultiple("GetCouponCodeLogList", List, sp);
                if (input.StartDate != null && input.StartDate.HasValue)
                {
                    input.StartDate = new DateTime(input.StartDate.Value.Year, input.StartDate.Value.Month, input.StartDate.Value.Day + 1, 0, 0, 0);
                    List = List.Where(x => Convert.ToDateTime(x.ValidityStartDate) >= input.StartDate.Value).ToList();
                }
                if (input.EndDate != null && input.EndDate.HasValue)
                {
                    input.EndDate = new DateTime(input.EndDate.Value.Year, input.EndDate.Value.Month, input.EndDate.Value.Day + 1, 23, 59, 59);
                    List = List.Where(x => Convert.ToDateTime(x.ValidityEndDate) <= input.EndDate.Value).ToList();
                }
                responce.TotalPage = (int)Math.Ceiling((double)List.Count() / input.itemsPerPage);
                input.Page = input.Page - 1;
                if (input.itemsPerPage != 0)
                {
                    List = List.Skip((input.itemsPerPage * input.Page)).Take(input.itemsPerPage).ToList();
                    responce.Result = List;
                }
                else
                    responce.Result = List;
                responce.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        #endregion
    }
}