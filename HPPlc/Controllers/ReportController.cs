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
using HPPlc.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Umbraco.Web.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
using Umbraco.Core.Models.PublishedContent;
using DotNetIntegrationKit;
using System.Net;
using HPPlc.Models.Mailer;
using HPPlc.Model;
using HPPlc.Models.ReportsHelper;
using HPPlc.Models.ImportExcelFiles;
using System.IO;
using Renci.SshNet;
using HPPlc.Models.Constant;

namespace HPPlc.Controllers
{
    public class ReportController : SurfaceController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;
        public ReportController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }

        public ReportController()
        {
        }
         
        [HttpGet]
        public ActionResult GetAllRegistrationList(string filter)
        {
            Responce responce = new Responce();
            try
            {

                List<clsRegistrationReport> List = new List<clsRegistrationReport>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "2" },
                    new SetParameters{ ParameterName = "@Query", Value = clsCommon.Encrypt(filter) },
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
					//              foreach (var item in List)
					//              {
					//if (!String.IsNullOrEmpty(item.u_name))
					//	item.u_name = clsCommon.Decrypt(item.u_name);
					//if (!String.IsNullOrEmpty(item.u_email))
					//	item.u_email = clsCommon.Decrypt(item.u_email);
					//if (!String.IsNullOrEmpty(item.u_whatsappno))
					//	item.u_whatsappno = (item.u_whatsappno != "" && item.u_whatsappno != null) ? clsCommon.Decrypt(item.u_whatsappno) : "";
					//item.u_whatsappno_prefix = item.u_whatsappno_prefix;
					//                  item.DOC = item.DOC;
					//                  item.IsActive = item.IsActive;
					//                  item.userUniqueId = item.userUniqueId;
					//              }
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
        public ActionResult GetAllSubscriptionList(string filter)
        {
            Responce responce = new Responce();
            try
            {

                List<clsSubscriptionReport> List = new List<clsSubscriptionReport>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value = clsCommon.Encrypt(filter) },
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
						//item.u_name = clsCommon.Decrypt(item.u_name);
						//                  item.u_email = clsCommon.Decrypt(item.u_email);
						//item.Ranking = item.Ranking;
      //                  item.SubscriptionName = item.SubscriptionName;
      //                  item.SubscriptionPrice = item.SubscriptionPrice;
      //                  item.SubscriptionDuration = item.SubscriptionDuration;
      //                  item.AgeGroup = item.AgeGroup;
      //                  item.SubscriptionStartDate = item.SubscriptionStartDate;
      //                  item.SubscriptionEndDate = item.SubscriptionEndDate;
      //                  item.DOC = item.DOC;
      //                  item.IsActive = item.IsActive;
      //                  item.PaymentStatus = item.PaymentStatus;
      //                  item.PaymentDate = item.PaymentDate;
      //                  item.PaymentId = item.PaymentId;
						//item.PaymentId = item.DiscountAmt;
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
        public ActionResult GetAllReferralDetailList(string filter)
        {
            Responce responce = new Responce();
            try
            {

                List<clsReferralDetailReport> List = new List<clsReferralDetailReport>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value = clsCommon.Encrypt(filter) },
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
						if(!String.IsNullOrEmpty(item.RefereeName))
							item.RefereeName = clsCommon.Decrypt(item.RefereeName);
						if (!String.IsNullOrEmpty(item.RefereeEmail))
							item.RefereeEmail = clsCommon.Decrypt(item.RefereeEmail);
						if (!String.IsNullOrEmpty(item.RefereeWNumber))
							item.RefereeWNumber = clsCommon.Decrypt(item.RefereeWNumber);
                        item.RefereeWPrefix = item.RefereeWPrefix;
                        item.DOC = item.DOC;
                        item.IsActive = item.IsActive;
                        item.ReferrerCode = item.ReferrerCode;
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
        public ActionResult GetAllReferralTransactionList(string filter)
        {
            Responce responce = new Responce();
            try
            {

                List<clsReferralDetailReport> List = new List<clsReferralDetailReport>();
                dbProxy _db = new dbProxy();

                List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" },
                    new SetParameters{ ParameterName = "@Query", Value = clsCommon.Encrypt(filter) },
                };
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
						item.u_whatsappno_prefix = item.u_whatsappno_prefix;
                        item.RewardReferralInMonths = item.RewardReferralInMonths;
                        item.RewardReferralInDays = item.RewardReferralInDays;
                        item.StartDate = item.StartDate;
                        item.EndDate = item.EndDate;
                        item.DOC = item.DOC;
                        item.IsActive = item.IsActive;
                        item.ReferrerCode = item.ReferrerCode;
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

        public ActionResult GPGReportExtract()
        {
            
            try
            {
                Responce subReport = new Responce();
                ReportsHelper reportsHelper = new ReportsHelper();
                subReport = reportsHelper.GetExtractReport();
                clsExpertHelper clsExpertHelper = new clsExpertHelper();
                
				//ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
				if (subReport != null && subReport.StatusCode == HttpStatusCode.OK)
                {
                    List<clsSubscriptionReportFoScheduler> List = new List<clsSubscriptionReportFoScheduler>();
                    List = subReport.Result as List<clsSubscriptionReportFoScheduler>;
                    byte[] bytes = clsExpertHelper.ListToExcelCSV(List, "ExtractReport");
                    MemoryStream stream = new MemoryStream(bytes);

                    try
                    {
                        var Excelfiles = Directory.EnumerateFiles(Server.MapPath("~\\ExcelFile\\SFMCExtractReport\\"), "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".csv") || s.EndsWith(".csv.gpg"));
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
                        string Bodys = "File not deleted \n Exception: " + ex.StackTrace + "\n Delivery date:" + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                        ReportStatusMail(ConfigurationManager.AppSettings["ExceptionEmailAddress"].ToString(), Bodys);

                        ApplicationError error = new ApplicationError();
                        error.PageName = "GPG Report";
                        error.MethodName = "file delete - Schedular";
                        error.ErrorMessage = ex.StackTrace;

                        HPPlc.Models.dbAccessClass.PostApplicationError(error);
                    }

                    // string FileName = DateTime.Now.ToString("dd-MM-yyyy") + "-ExtractReport.csv";
                    string FileName = "HP_PLC_IN_UserData_" + DateTime.Now.ToString("dd-MM-yyyy") + "_Reports";
                    String LocalDestinationFilename = Server.MapPath("~\\ExcelFile\\SFMCExtractReport\\").ToString() + FileName + ".csv";
                    //CSV saveon local folder
                    System.IO.File.WriteAllBytes(Server.MapPath("~\\ExcelFile\\SFMCExtractReport\\") + FileName + ".csv", bytes);

                    
                    try
                    {
                        string Host = ConfigurationManager.AppSettings["SFTSPHost"];
                        int Port = Convert.ToInt32(ConfigurationManager.AppSettings["SFTSPPort"]);
                        //int Port = 2222;
                        string Username = ConfigurationManager.AppSettings["SFTSPUser"];
                        string Password = ConfigurationManager.AppSettings["SFTSPPassword"];
                        String RemoteFileName = "/" + ConfigurationManager.AppSettings["SFTSPFolder"];

                        using (var sftp = new SftpClient(Host, Port, Username, Password))
                        {
                            sftp.Connect();
                            sftp.ChangeDirectory(RemoteFileName);
                            var listDirectory = sftp.ListDirectory(RemoteFileName);

                            using (var filestream = new FileStream(LocalDestinationFilename, FileMode.Open))
                            {
                                sftp.BufferSize = 4 * 1024;
                                sftp.UploadFile(filestream, Path.GetFileName(LocalDestinationFilename));
                            }

                            sftp.Disconnect();
                        }
                    }
                    catch (Exception ex)
                    {

                        string mailBody = "File not senton ftp \n <br> Exception: " + ex.StackTrace + "\n<br> Delivery date:" + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                        ReportStatusMail(ConfigurationManager.AppSettings["ExceptionEmailAddress"].ToString(), mailBody);

                        ApplicationError error = new ApplicationError();
                        error.PageName = "GPG Report";
                        error.MethodName = "ftp section - Schedular";
                        error.ErrorMessage = ex.StackTrace;

                        HPPlc.Models.dbAccessClass.PostApplicationError(error);
                    }

                    //Sendmail for reports
                    string Body = "Schedular GPG Reports has been sent! \n<br> Total count of reports items: " + List.Count + "\n<br> Delivery date:" + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                    ReportStatusMail("", Body);

                }
            }
            catch (Exception ex)
            {
                //Sendmail for reports
                string Body = "Email not sended! \n<br> Exception: " + ex.StackTrace + "\n<br> Delivery date:" + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                ReportStatusMail(ConfigurationManager.AppSettings["ExceptionEmailAddress"].ToString(), Body);

                //responce.StatusCode = HttpStatusCode.InternalServerError;
                //responce.Message = ex.ToString();
                ApplicationError error = new ApplicationError();
                error.PageName = "GPG Report";
                error.MethodName = "Main - Schedular";
                error.ErrorMessage = ex.StackTrace;

                HPPlc.Models.dbAccessClass.PostApplicationError(error);
            }

            return View();
        }

        public string ReportStatusMail(string emailto, string Body)
        {
            //ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
            SenderMailer mailer = new SenderMailer();
            MailContentModel mail = new MailContentModel();
           // mail = Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports);

            if (String.IsNullOrWhiteSpace(emailto))
            {
                emailto = "ashish.kumar@digitas.com";
            }

            if (mail != null)
            {
                mailer.SendMail(emailto, "Master Report", Body, null,null);
            }

            return "ok";
        }
    }
}