using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using HPPlc.Models.Mailer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace HPPlc.Models.FAQ
{
    public static class FAQHelper
    {
        public static Responce SaveRequest(FAQRequestModel fAQRequest)
        {
            Responce responce = new Responce();
            try
            {
                if (fAQRequest.SelectDate == null || string.IsNullOrWhiteSpace(fAQRequest.SelectDate))
                    fAQRequest.SelectDate = DateTime.Now.ToString("MMM dd,yyyy");
                if (fAQRequest.SelectTime == null || string.IsNullOrWhiteSpace(fAQRequest.SelectTime))
                    fAQRequest.SelectTime = DateTime.Now.ToString("HH:mm:ss:tt");
                int QType = 1;
                dbProxy _db = new dbProxy();
                GetStatus status = new GetStatus();
				string Remarks = String.Empty;
				string Consent = String.Empty;
				if (!String.IsNullOrEmpty(fAQRequest?.Remark))
					Remarks = fAQRequest?.Remark;
				if (!String.IsNullOrEmpty(fAQRequest?.Consent))
					Consent = fAQRequest?.Consent;
				List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@QType", Value =QType.ToString()},
                        new SetParameters { ParameterName = "@FullName", Value =fAQRequest.FullName},
                        new SetParameters { ParameterName = "@Mobile", Value =fAQRequest.Mobile},
                        new SetParameters { ParameterName = "@SelectDate", Value =fAQRequest.SelectDate},
                        new SetParameters { ParameterName = "@SelectTime", Value =fAQRequest.SelectTime},
                        new SetParameters { ParameterName = "@Remark",Value= Remarks},
                        new SetParameters { ParameterName = "@consent",Value=Consent}
                    };
                status = _db.StoreData("InsertFAQRequest", sp);
                if (status != null && status.returnStatus == "Success")
                {
                    responce.Result = status.returnValue;
                    responce.Message = status.returnMessage;
                    responce.StatusCode = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                responce.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                responce.Message = ex.Message;
            }
            return responce;
        }
        

        public static Responce AllRequestList()
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
                responce.Result = fAQRequestsList;
                responce.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        
        public static void SendEmailToFAQRequestHandler(MailContentModel mailContent)
        {
            try
            {
                if (mailContent != null && mailContent.EmailTo != null)
                {
                    Responce responce = new Responce();
                    responce = AllRequestList();
                    List<FAQRequestModel> List = new List<FAQRequestModel>();
                    clsExpertHelper clsExpertHelper = new clsExpertHelper();
                    List = responce.Result as List<FAQRequestModel>;
                    byte[] bytes = clsExpertHelper.ListToExcel(List, "FAQRequest");
                    MemoryStream stream = new MemoryStream(bytes);
                    SenderMailer mailer = new SenderMailer();
                    string FileName = DateTime.Now.ToString("dd-MM-yyyy") + "-FAQReuestList.xlsx";
                    mailer.SendeMailWithAttchment(stream, FileName, "application/vnd.ms-excel", mailContent);
                }

            }
            catch (Exception ex)
            {

            }
        }
        public static Responce GetHolidayList()
        {
            Responce responce = new Responce();
            try
            {
                List<string> Time = new List<string>();
                dbProxy _db = new dbProxy();
                List<string> fAQRequestsList = new List<string>();
                List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@QType", Value ="1"}
                    };
                fAQRequestsList = _db.GetDataMultiple("GetHolidayListAndTimeCount", fAQRequestsList, sp);
                if (fAQRequestsList != null && fAQRequestsList.Count() > 0)
                {
                    responce.Result = fAQRequestsList.ToList();
                    responce.StatusCode = System.Net.HttpStatusCode.OK;
                }

            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        public static Responce GetTimeList(string SelectedDate)
        {
            Responce responce = new Responce();
            try
            {
                List<TimeList> Time = new List<TimeList>();
                var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
                var FAQ = helper.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
                                                .Where(x => x.ContentType.Alias == "fAQ")?.OfType<Umbraco.Web.PublishedModels.FAQ>().FirstOrDefault();
                ;
                if (FAQ != null)
                {
                    var GetTimeList = FAQ.TimeList.Where(x => x.IsActive);
                    if (GetTimeList != null && GetTimeList.Count() > 0)
                    {
                        foreach (var item in GetTimeList)
                        {
                            Time.Add(new TimeList()
                            {
                                Title = item.Title,
                                Value = item.Value,
                                IsActive = item.IsActive,
                                MaxCount = item.MaxCount
                            });
                        }


                        if (string.IsNullOrWhiteSpace(SelectedDate))
                        {
                            responce.Result = Time;
                            responce.StatusCode = System.Net.HttpStatusCode.OK;
                            return responce;
                        }
                        else
                        {
                            List<string> DbTimeList = new List<string>();
                            dbProxy _db = new dbProxy();
                            List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@QType", Value ="2"},
                        new SetParameters { ParameterName = "@SelectedDate", Value =SelectedDate}
                    };
                            DbTimeList = _db.GetDataMultiple("GetHolidayListAndTimeCount", DbTimeList, sp);
                            if (DbTimeList != null && DbTimeList.Count() > 0)
                            {
                                List<TimeList> NewTime = new List<TimeList>();
                                foreach (var item in Time.ToList())
                                {
                                    if (DbTimeList.Where(x => x.Equals(item.Value)).Count() < item.MaxCount)
                                    {
                                        NewTime.Add(new TimeList()
                                        {
                                            Title = item.Title,
                                            Value = item.Value,
                                            IsActive = item.IsActive,
                                            MaxCount = item.MaxCount
                                        });
                                    }
                                }
                                responce.Result = NewTime.ToList();
                                responce.StatusCode = System.Net.HttpStatusCode.OK;
                            }
                            else
                            {
                                responce.Result = Time;
                                responce.StatusCode = System.Net.HttpStatusCode.OK;
                                return responce;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return responce;
        }
    }
}