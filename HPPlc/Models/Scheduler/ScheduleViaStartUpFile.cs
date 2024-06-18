using HPPlc.Controllers;
using HPPlc.Models.FAQ;
using HPPlc.Models.ImportExcelFiles;
using HPPlc.Models.Mailer;
using LINQtoCSV;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Umbraco.Core.Services.Implement;
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
			//try
			//{
			//    Responce subReport = new Responce();
			//    ReportsHelper.ReportsHelper reportsHelper = new ReportsHelper.ReportsHelper();
			//    subReport = reportsHelper.GetExtractReport();
			//    clsExpertHelper clsExpertHelper = new clsExpertHelper();
			//    ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
			//    if (subReport != null && subReport.StatusCode == HttpStatusCode.OK)
			//    {
			//        List<clsSubscriptionReportFoScheduler> List = new List<clsSubscriptionReportFoScheduler>();
			//        List= subReport.Result as List<clsSubscriptionReportFoScheduler>;
			//        byte[] bytes = clsExpertHelper.ListToExcelCSV(List, "ExtractReport");
			//        MemoryStream stream = new MemoryStream(bytes);
			//        SenderMailer mailer = new SenderMailer();
			//        string FileName = "";
			//        string FileType = ConfigurationManager.AppSettings["FileType"];
			//        if (FileType.Equals("CSV"))
			//        {
			//            FileName=DateTime.Now.ToString("dd-MM-yyyy") + "-ExtractReport.csv";
			//            File.WriteAllBytes(_httpContext.Server.MapPath("~\\SFMCExtractReport\\") + FileName, bytes);
			//        }
			//        else
			//        {
			//            FileName = DateTime.Now.ToString("dd-MM-yyyy") + "-ExtractReport.csv.gpg";
			//            File.WriteAllBytes(_httpContext.Server.MapPath("~\\SFMCExtractReport\\") + FileName, bytes);
			//        }
			//        //mailer.SendeMailWithAttchment(stream, FileName, "application/vnd.ms-excel", Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));
			//        try
			//        {

			//            string SFTSPHost = ConfigurationManager.AppSettings["SFTSPHost"];
			//            string SFTSPUser = ConfigurationManager.AppSettings["SFTSPUser"];
			//            string SFTSPPassword = ConfigurationManager.AppSettings["SFTSPPassword"];
			//            String RemoteFileName = "/" + ConfigurationManager.AppSettings["SFTSPFolder"];

			//            String LocalDestinationFilename = _httpContext.Server.MapPath("~\\SFMCExtractReport\\").ToString() + FileName;

			//            using (var sftp = new SftpClient(SFTSPHost,SFTSPUser, SFTSPPassword))
			//            {
			//                sftp.Connect();
			//                sftp.ChangeDirectory(RemoteFileName);
			//                var listDirectory = sftp.ListDirectory(RemoteFileName);

			//                using (var filestream = new FileStream(LocalDestinationFilename, FileMode.Open))
			//                {
			//                    sftp.BufferSize = 4 * 1024;
			//                    sftp.UploadFile(filestream, Path.GetFileName(LocalDestinationFilename));
			//                }

			//                sftp.Disconnect();
			//            }
			//        }
			//        catch (Exception exp) { }
			//    }
			//}
			//catch (Exception ex)
			//{
			//    //responce.StatusCode = HttpStatusCode.InternalServerError;
			//   // responce.Message = ex.ToString();
			//}

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
			int SchedulerIntervalInMin = int.Parse(ConfigurationManager.AppSettings["SchedulerIntervalInMin"]);
			var curr = DateTime.Now;
			Timer scheduleTimer = new Timer();
			scheduleTimer.Interval = SchedulerIntervalInMin * 60 * 1000;//10 min
																		//scheduleTimer.Interval = 10 * 60 * 1;//10 min
			Timer t = new Timer(scheduleTimer.Interval); // 1 sec = 1000, 60 sec = 60000
			t.AutoReset = true;
			t.Elapsed += new System.Timers.ElapsedEventHandler(CallFuncationEveryOne10Mintes);
			t.Start();
			ReportsHelper.ReportsHelper reportsHelper = new ReportsHelper.ReportsHelper();
			ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
			// reportsHelper.SendEmailToRegistration(Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));


		}
		public void CallFuncationEveryOne10Mintes(object sender, System.Timers.ElapsedEventArgs e)
		{
			Responce responce = new Responce();
			TimeSpan now = DateTime.Now.TimeOfDay;
			//#region Faq Send List to 6 PM to 6:10 PM
			//TimeSpan FAQstart = new TimeSpan(19, 0, 0); //0 o'clock like 6 PM to 6:10 PM
			//TimeSpan FAQend = new TimeSpan(19, 9, 0);
			//if ((now > FAQstart) && (now < FAQend))
			//{

			//    try
			//    {
			//        ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
			//        FAQHelper.SendEmailToFAQRequestHandler(Helper.GetMailContentFromCMS(Constant.ConstantUserType.FAQ));
			//    }
			//    catch (Exception ex)
			//    {
			//        responce.StatusCode = HttpStatusCode.InternalServerError;
			//        responce.Message = ex.ToString();
			//    }

			//}
			//#endregion

			//#region Registration and subscription List Send To12 AM to 12:10 AM Mid Night
			//TimeSpan Regstart = new TimeSpan(0, 0, 0); //0 o'clock like 12 AM to 12:10 AM
			//TimeSpan Regend = new TimeSpan(1, 9, 0);
			//if ((now > Regstart) && (now < Regend))
			//{
			//    try
			//    {
			//        ReportsHelper.ReportsHelper reportsHelper = new ReportsHelper.ReportsHelper();
			//        ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
			//        reportsHelper.SendEmailToRegistration(Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));
			//        reportsHelper.SendEmailToSubscription(Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));

			//    }
			//    catch (Exception ex)
			//    {
			//        responce.StatusCode = HttpStatusCode.InternalServerError;
			//        responce.Message = ex.ToString();
			//    }

			//}
			//#endregion
			#region Reports Send List to 12 AM to 12:10 AM
			string SchedulerTimeForExtractReport = ConfigurationManager.AppSettings["SchedulerTimeForExtractReport"].ToString();
			string SchedulerDayForExtractReport = ConfigurationManager.AppSettings["SchedulerDayForExtractReport"].ToString();
			if (SchedulerTimeForExtractReport != null && !string.IsNullOrWhiteSpace(SchedulerTimeForExtractReport) && SchedulerDayForExtractReport != null && !string.IsNullOrWhiteSpace(SchedulerDayForExtractReport))
			{
				SchedulerDayForExtractReport = SchedulerDayForExtractReport.ToLower();
				if (SchedulerDayForExtractReport.Equals(DateTime.Now.DayOfWeek.ToString().ToLower()))
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
									List = subReport.Result as List<clsSubscriptionReportFoScheduler>;
									byte[] bytes = clsExpertHelper.ListToExcelCSV(List, "ExtractReport");
									MemoryStream stream = new MemoryStream(bytes);

									try
									{
										var Excelfiles = Directory.EnumerateFiles(_httpContext.Server.MapPath("~\\SFMCExtractReport\\"), "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".csv") || s.EndsWith(".csv.gpg"));
										if (Excelfiles != null && Excelfiles.Count() > 0)
										{
											foreach (var item in Excelfiles)
											{
												if (item != null)
												{
													File.Delete(item);
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
									String LocalDestinationFilename = _httpContext.Server.MapPath("~\\SFMCExtractReport\\").ToString() + FileName + ".csv";
									//CSV saveon local folder
									File.WriteAllBytes(_httpContext.Server.MapPath("~\\SFMCExtractReport\\") + FileName + ".csv", bytes);

									//CsvFileDescription csvFileDescription = new CsvFileDescription
									//{
									//    SeparatorChar = '|',
									//    FirstLineHasColumnNames = true,
									//    EnforceCsvColumnAttribute = true
									//};
									//CsvContext csvContext = new CsvContext();
									//byte[] file = null;
									//using (MemoryStream memoryStream = new MemoryStream())
									//{
									//    using (StreamWriter streamWriter = new StreamWriter(memoryStream))
									//    {
									//        csvContext.Write<clsSubscriptionReportFoScheduler>(List, streamWriter, csvFileDescription);
									//        streamWriter.Flush();
									//        file = memoryStream.ToArray();
									//    }
									//}


									//if (!System.IO.File.Exists(LocalDestinationFilename))
									//{
									//    System.IO.File.WriteAllBytes(LocalDestinationFilename, file);
									//}
									//File.WriteAllBytes(_httpContext.Server.MapPath("~\\SFMCExtractReport\\") + FileName + ".csv.gpg", bytes);


									//mailer.SendeMailWithAttchment(stream, FileName, "application/vnd.ms-excel", Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports));

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

										string mailBody = "File not senton ftp \n Exception: " + ex.StackTrace + "\n Delivery date:" + DateTime.Now.ToString("dddd, dd MMMM yyyy");
										ReportStatusMail(ConfigurationManager.AppSettings["ExceptionEmailAddress"].ToString(), mailBody);

										ApplicationError error = new ApplicationError();
										error.PageName = "GPG Report";
										error.MethodName = "ftp section - Schedular";
										error.ErrorMessage = ex.StackTrace;

										HPPlc.Models.dbAccessClass.PostApplicationError(error);
									}

									//Sendmail for reports
									string Body = "Email has been sended! \n Total count of reports items: " + List.Count + "\n Delivery date:" + DateTime.Now.ToString("dddd, dd MMMM yyyy");
									ReportStatusMail("", Body);

								}
							}
							catch (Exception ex)
							{
								//Sendmail for reports
								string Body = "Email not sended! \n Exception: " + ex.StackTrace + "\n Delivery date:" + DateTime.Now.ToString("dddd, dd MMMM yyyy");
								ReportStatusMail(ConfigurationManager.AppSettings["ExceptionEmailAddress"].ToString(), Body);

								//responce.StatusCode = HttpStatusCode.InternalServerError;
								//responce.Message = ex.ToString();
								ApplicationError error = new ApplicationError();
								error.PageName = "GPG Report";
								error.MethodName = "Main - Schedular";
								error.ErrorMessage = ex.StackTrace;

								HPPlc.Models.dbAccessClass.PostApplicationError(error);
							}

						}
					}
				}
			}
			#endregion

		}
		#endregion

		public string ReportStatusMail(string emailto, string Body)
		{
			ImportExcelFilesHelper Helper = new ImportExcelFilesHelper(_context);
			SenderMailer mailer = new SenderMailer();
			MailContentModel mail = new MailContentModel();
			mail = Helper.GetMailContentFromCMS(Constant.ConstantUserType.Reports);

			if (String.IsNullOrWhiteSpace(emailto))
			{
				emailto = mail.EmailTo;
			}

			if (mail != null)
			{
				mailer.SendMail(emailto, mail.Subject, Body, mail.EmailCC, mail.EmailBcc);
			}

			return "ok";
		}

		
	}
}