using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Umbraco.Web;
using Timer = System.Timers.Timer;
namespace HPPlc.Models.ImportExcelFiles
{
    public class ImportExcelViaStartUpFile
    {
        private string FolderName = "/ExcelFile/";

        string Remote_FolderPath = ConfigurationManager.AppSettings["FolderName"].ToString();
        string Remote_BkupFolderPath = ConfigurationManager.AppSettings["BkupFolderName"].ToString();

        private string Host = ConfigurationManager.AppSettings["Host"].ToString();
        private string Username = ConfigurationManager.AppSettings["Username"].ToString();
        private string Password = ConfigurationManager.AppSettings["Password"].ToString();
        string LocalSaveFilePath = "";
        private static ImportExcelFilesHelper ImportExcelFilesHelper;
        private IUmbracoContextFactory _context;
        public ImportExcelViaStartUpFile(IUmbracoContextFactory context)
        {
            _context = context;
            ImportExcelFilesHelper = new ImportExcelFilesHelper(_context);
        }
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
            var today = DateTime.Today; //0:59:59 o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {

                Responce responce = new Responce();
                try
                {
                    bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(FolderName));
                    if (!exists)
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(FolderName));

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
    }
}