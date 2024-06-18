using HPPlc.Models.FAQ;
using HPPlc.Models.ImportExcelFiles;
using HPPlc.Models.Mailer;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using Umbraco.Web;
using Timer = System.Timers.Timer;

namespace HPPlc.Models.Scheduler
{
    public class ScheduleViaStartUpFile
    {
        #region Import Excel Items
        private string FolderName = "/ExcelFile/";
        string Remote_FolderPath = ConfigurationManager.AppSettings["FolderName"].ToString();
        string Remote_BkupFolderPath = ConfigurationManager.AppSettings["BkupFolderName"].ToString();

        private string Host = ConfigurationManager.AppSettings["Host"].ToString();
        private string Username = ConfigurationManager.AppSettings["Username"].ToString();
        private string Password = ConfigurationManager.AppSettings["Password"].ToString();
        string LocalSaveFilePath = "";
        private static ImportExcelFilesHelper ImportExcelFilesHelper;
        private readonly HttpContext _httpContext;
        #endregion

        private IUmbracoContextFactory _context;
        public ScheduleViaStartUpFile(IUmbracoContextFactory context, HttpContext httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            ImportExcelFilesHelper = new ImportExcelFilesHelper(_context);
        }
        public void CallScheduler(string _LocalSaveFilePath)
        {
            //Shedule24Hours(_LocalSaveFilePath); //12PM one time call
            Every10MintusCall();// daily 6 PM to 6:10 PM call

            //Responce subReport = new Responce();
           // ReportsHelper.ReportsHelper reportsHelper = new ReportsHelper.ReportsHelper();
           // subReport = reportsHelper.GetExtractReport();
            //clsExpertHelper clsExpertHelper = new clsExpertHelper();
            //ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
            try
            {
                Responce subReport = new Responce();
                ReportsHelper.ReportsHelper reportsHelper = new ReportsHelper.ReportsHelper();
                subReport = reportsHelper.GetExtractReport();
                clsExpertHelper clsExpertHelper = new clsExpertHelper();
                ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
                if (subReport != null && subReport.StatusCode == HttpStatusCode.OK)
                {
                    List<clsSubscriptionReportFoScheduler> List = new List<clsSubscriptionReportFoScheduler>();
                    List= subReport.Result as List<clsSubscriptionReportFoScheduler>;
                    byte[] bytes = clsExpertHelper.ListToExcelCSV(List, "ExtractReport");
                    MemoryStream stream = new MemoryStream(bytes);
                    SenderMailer mailer = new SenderMailer();
                    string FileName = "";
                    string FileType = ConfigurationManager.AppSettings["FileType"];
                    if (FileType.Equals("CSV"))
                    {
                        FileName=DateTime.Now.ToString("dd-MM-yyyy") + "-ExtractReport.csv";
                        File.WriteAllBytes(_httpContext.Server.MapPath("~\\SFMCExtractReport\\") + FileName, bytes);
                    }
                    else
                    {
                        FileName = DateTime.Now.ToString("dd-MM-yyyy") + "-ExtractReport.csv.gpg";
                        File.WriteAllBytes(_httpContext.Server.MapPath("~\\SFMCExtractReport\\") + FileName, bytes);
                    }
                    //mailer.SendeMailWithAttchment(stream, FileName, "application/vnd.ms-excel", Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));
                    try
                    {

                        string SFTSPHost = ConfigurationManager.AppSettings["SFTSPHost"];
                        string SFTSPUser = ConfigurationManager.AppSettings["SFTSPUser"];
                        string SFTSPPassword = ConfigurationManager.AppSettings["SFTSPPassword"];
                        String RemoteFileName = "/" + ConfigurationManager.AppSettings["SFTSPFolder"];

                        String LocalDestinationFilename = _httpContext.Server.MapPath("~\\SFMCExtractReport\\").ToString() + FileName;

                        using (var sftp = new SftpClient(SFTSPHost,SFTSPUser, SFTSPPassword))
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
                    catch (Exception exp) { }
                }
            }
            catch (Exception ex)
            {
                //responce.StatusCode = HttpStatusCode.InternalServerError;
               // responce.Message = ex.ToString();
            }

        }
        #region Import Excel File 12 PM
        public void Shedule24Hours(string _LocalSaveFilePath)
        {
            LocalSaveFilePath = _LocalSaveFilePath;
            var curr = DateTime.Now;
            Timer scheduleTimer = new Timer();
            //scheduleTimer.Interval = 24 * 60 * 60 * 1000;//24 hours
            //scheduleTimer.Interval = 3 * 60 * 1000;//3 min
            //scheduleTimer.Interval = 5000;//5 seconds
            scheduleTimer.Interval = 30 * 60 * 1000;//30 min
            Timer t = new Timer(scheduleTimer.Interval); // 1 sec = 1000, 60 sec = 60000
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(CallFuncationEveryOne24Hours);
            t.Start();


        }
        public void CallFuncationEveryOne24Hours(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan start = new TimeSpan(0, 0, 0); //0 o'clock like 12 PM to 1 AM
            TimeSpan end = new TimeSpan(0, 59, 59);
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {

                Responce responce = new Responce();
                try
                {
                    using (var sftp = new SftpClient(Host, Username, Password))
                    {
                        sftp.Connect();
                        ImportExcelFilesHelper.DownloadDirectory(sftp, Remote_FolderPath, LocalSaveFilePath, Remote_BkupFolderPath);
                        sftp.Disconnect();
                        responce.Message = "File Import Successfully..!";
                        responce.StatusCode = HttpStatusCode.OK;
                    }


                }
                catch (Exception ex)
                {
                    responce.StatusCode = HttpStatusCode.InternalServerError;
                    responce.Message = ex.ToString();
                }

            }

        }
        #endregion

        #region  Call Every 10 Time Please Set up Our Time Here
        public void Every10MintusCall()
        {
            var curr = DateTime.Now;
            Timer scheduleTimer = new Timer();
            scheduleTimer.Interval = 10 * 60 * 1000;//10 min
            //scheduleTimer.Interval = 10 * 60 * 1;//10 min
            Timer t = new Timer(scheduleTimer.Interval); // 1 sec = 1000, 60 sec = 60000
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(CallFuncationEveryOne10Mintes);
            t.Start();
            ReportsHelper.ReportsHelper reportsHelper = new ReportsHelper.ReportsHelper();
            ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
            reportsHelper.SendEmailToRegistration(Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));


        }
        public void CallFuncationEveryOne10Mintes(object sender, System.Timers.ElapsedEventArgs e)
        {
            Responce responce = new Responce();
            TimeSpan now = DateTime.Now.TimeOfDay;
            #region Faq Send List to 6 PM to 6:10 PM
            TimeSpan FAQstart = new TimeSpan(19, 0, 0); //0 o'clock like 6 PM to 6:10 PM
            TimeSpan FAQend = new TimeSpan(19, 9, 0);
            if ((now > FAQstart) && (now < FAQend))
            {

                try
                {
                    ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
                    FAQHelper.SendEmailToFAQRequestHandler(Helper.GetMailContentFromCMS(Constant.ConstantUserType.FAQ));
                }
                catch (Exception ex)
                {
                    responce.StatusCode = HttpStatusCode.InternalServerError;
                    responce.Message = ex.ToString();
                }

            }
            #endregion

            #region Registration and subscription List Send To12 AM to 12:10 AM Mid Night
            TimeSpan Regstart = new TimeSpan(0, 0, 0); //0 o'clock like 12 AM to 12:10 AM
            TimeSpan Regend = new TimeSpan(1, 9, 0);
            if ((now > Regstart) && (now < Regend))
            {
                try
                {
                    ReportsHelper.ReportsHelper reportsHelper = new ReportsHelper.ReportsHelper();
                    ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
                    reportsHelper.SendEmailToRegistration(Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));
                    reportsHelper.SendEmailToSubscription(Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));

                }
                catch (Exception ex)
                {
                    responce.StatusCode = HttpStatusCode.InternalServerError;
                    responce.Message = ex.ToString();
                }

            }
            #endregion
            #region Faq Send List to 12 AM to 12:10 AM
            string SchedulerTimeForExtractReport = ConfigurationManager.AppSettings["SchedulerTimeForExtractReport"].ToString();
            string SchedulerDayForExtractReport = ConfigurationManager.AppSettings["SchedulerDayForExtractReport"].ToString();
            if (SchedulerTimeForExtractReport != null && !string.IsNullOrWhiteSpace(SchedulerTimeForExtractReport) && SchedulerDayForExtractReport != null && !string.IsNullOrWhiteSpace(SchedulerDayForExtractReport))
            {
                SchedulerDayForExtractReport = SchedulerDayForExtractReport.ToLower();
                if (SchedulerDayForExtractReport.Equals(DateTime.Now.Day.ToString().ToLower()))
                {
                    var SchedulerTimeForExtractReportSplit = SchedulerTimeForExtractReport.Split(':');
                    if (SchedulerTimeForExtractReportSplit != null)
                    {
                        TimeSpan substart = new TimeSpan(Convert.ToInt32(SchedulerTimeForExtractReportSplit[0].ToString()), Convert.ToInt32(SchedulerTimeForExtractReportSplit[1].ToString()), 0);
                        TimeSpan subend = new TimeSpan(Convert.ToInt32(SchedulerTimeForExtractReportSplit[0].ToString()), (Convert.ToInt32(SchedulerTimeForExtractReportSplit[1].ToString()) + 9), 0);
                        if ((now > substart) && (now < subend))
                        {

                            try
                            {
                                Responce subReport = new Responce();
                                ReportsHelper.ReportsHelper reportsHelper = new ReportsHelper.ReportsHelper();
                                subReport = reportsHelper.GetExtractReport();
                                clsExpertHelper clsExpertHelper = new clsExpertHelper();
                                ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
                                if (subReport != null && subReport.StatusCode == HttpStatusCode.OK)
                                {
                                    List<clsSubscriptionReportFoScheduler> List = new List<clsSubscriptionReportFoScheduler>();
                                    byte[] bytes = clsExpertHelper.ListToExcelCSV(List, "ExtractReport");
                                    MemoryStream stream = new MemoryStream(bytes);
                                    SenderMailer mailer = new SenderMailer();
                                    string FileName = DateTime.Now.ToString("dd-MM-yyyy") + "-ExtractReport.csv";
                                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~\\SFMCExtractReport\\")))
                                    {
                                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\SFMCExtractReport\\"));
                                    }
                                    File.WriteAllBytes(HttpContext.Current.Server.MapPath("~\\SFMCExtractReport\\") + FileName, bytes);
                                    //mailer.SendeMailWithAttchment(stream, FileName, "application/vnd.ms-excel", Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));
                                    try
                                    {

                                        string Host = ConfigurationManager.AppSettings["SFTPHost"];
                                        int Port = 2222;
                                        string Username = ConfigurationManager.AppSettings["SFTPUser"];
                                        string Password = ConfigurationManager.AppSettings["SFTPPassword"];
                                        String RemoteFileName = "/" + ConfigurationManager.AppSettings["SFTPFolder"];


                                        String LocalDestinationFilename = HttpContext.Current.Server.MapPath("~\\SFMCExtractReport\\").ToString() + FileName;

                                        using (var sftp = new SftpClient(Host, Username, Password))
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
                                    catch (Exception exp) { }
                                }
                            }
                            catch (Exception ex)
                            {
                                responce.StatusCode = HttpStatusCode.InternalServerError;
                                responce.Message = ex.ToString();
                            }

                        }
                    }
                }
            }
            #endregion

        }
        #endregion


    }
}