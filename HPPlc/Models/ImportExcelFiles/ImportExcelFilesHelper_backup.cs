using HPPlc.Models.Mailer;
using OfficeOpenXml;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Web;

namespace HPPlc.Models.ImportExcelFiles
{
    public class ImportExcelFilesHelper_backup
    {
        private  readonly IUmbracoContextFactory _context;
       public static RegistrationMailerModel RegistrationMailer;
        public ImportExcelFilesHelper_backup(IUmbracoContextFactory context)
        {
            _context = context;
            RegistrationMailer = new RegistrationMailerModel();
            using (var cref = _context.EnsureUmbracoContext())
            {
                var cache = cref.UmbracoContext.Content;
                var node = cache.GetById(Constant.Constant.RegistrationMailer) ;
                if (node != null)
                {
                    var Mail = node as Umbraco.Web.PublishedModels.RegistrationMailer;
                    RegistrationMailer.HeaderBanner = Mail.HeaderBanner.Url().ToString();
                    RegistrationMailer.Title = Mail.Title.ToString();
                    RegistrationMailer.BodyContent = Mail.BodyContent;
                    RegistrationMailer.VerifyButton = Mail.VerifyButton.Name.ToString();
                    RegistrationMailer.FooterText = Mail.FooterText;
                    RegistrationMailer.UnsubscribeButton = Mail.UnsubscribeButton.Name.ToString();
                }
            }
        }
        public void DownloadDirectory(SftpClient client, string source, string destination, string remote_back_folder)
        {
            try
            {
                var files = client.ListDirectory(source);
                int Downlaodcount = 0;
                foreach (var file in files)
                {
                    if (!file.IsDirectory && !file.IsSymbolicLink)
                    {
                        DownloadFile(client, file, destination);
                        //save file in local folder
                        UploadFile(client, file, remote_back_folder, destination);
                        //move file in sftp server source to back up folder
                        DeleteFile(client, file, source);
                        //after move file in back up folder delete file in original path
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Bitmap bmp = new Bitmap(10, 10);
                            bmp.Save(ms, ImageFormat.Bmp);

                            ms.Seek(0, SeekOrigin.Begin);

                            client.UploadFile(ms, "File.bmp");
                            //send email for single file is after save in project folder--ExcelFile
                            SenderMailer sender = new SenderMailer();
                            MailContentModel mailer = GetSMTPForAdmin();
                            if (mailer != null && mailer.EmailTo != null)
                            {
                                sender.SendeMailWithAttchment("text", ms, file.Name, "application/vnd.ms-excel", mailer);
                            }
                        }


                    }
                    else if (file.IsSymbolicLink)
                    {
                        Console.WriteLine("Ignoring symbolic link {0}", file.FullName);
                    }
                    else if (file.Name != "." && file.Name != "..")
                    {
                        var dir = Directory.CreateDirectory(Path.Combine(destination, file.Name));
                        DownloadDirectory(client, file.FullName, dir.FullName, remote_back_folder);
                    }

                    if (Downlaodcount >= 2)
                    {
                        break;
                    }
                    Downlaodcount++;

                }
                //send email for single file is after save in project folder--ExcelFile
                ImportExcelToTable(destination);
                //send email for zip file to admin after all project is done..
                CreateZipForExcelFile(destination);


            }
            catch (Exception ex)
            {

            }

        }
        private static void DownloadFile(SftpClient client, SftpFile file, string directory)
        {
            try
            {
                if (File.Exists(directory + "/" + file.Name))
                {
                    File.Delete(directory + "/" + file.Name);
                }

                using (Stream fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
                {
                    client.DownloadFile(file.FullName, fileStream);

                }

            }
            catch (Exception ex)
            {

            }

        }
        private static void UploadFile(SftpClient client, SftpFile file, string remote_back_folder, string localpath)
        {
            try
            {
                if (client.Exists(remote_back_folder + "/" + file.Name))
                {
                    client.DeleteFile(Path.Combine(remote_back_folder, file.Name));
                }

                using (Stream fileStream = File.OpenRead(Path.Combine(localpath, file.Name)))
                {
                    client.UploadFile(fileStream, Path.Combine(remote_back_folder, file.Name));
                }

            }
            catch (Exception ex)
            {
            }

        }
        private static void DeleteFile(SftpClient client, SftpFile file, string Remote_FolderPath)
        {
            try
            {
                if (client.Exists(Remote_FolderPath + "/" + file.Name))
                {
                    client.DeleteFile(Path.Combine(Remote_FolderPath, file.Name));
                }

            }
            catch (Exception ex)
            {

            }

        }
        public static Responce ImportExcelToTable(string LocalSaveFilePath)
        {
            DataTable dt = new DataTable();
            DataTable dtAll = new DataTable();
            Responce responce = new Responce();
            try
            {
                var files = Directory.EnumerateFiles(LocalSaveFilePath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".csv") || s.EndsWith(".xlsx") || s.EndsWith(".xls"));
                if (files != null && files.Count() > 0)
                {
                    string Reason = "";
                    List<RegisterTemp> myProfileList = new List<RegisterTemp>();
                    foreach (var item in files)
                    {
                        string path1 = string.Format("{0}/{1}", LocalSaveFilePath, Path.GetFileName(item));
                        if (File.Exists(item))
                        {
                            dt = ConvertCSVtoDataTable(item);
                            dt.Columns.Add("Status", typeof(String));
                            dt.Columns.Add("Reason", typeof(String));
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    RegisterTemp myProfile = new RegisterTemp();

                                    myProfile.Name = Convert.ToString(dt.Rows[i]["u_name"]);
                                    myProfile.Email = Convert.ToString(dt.Rows[i]["u_email"]).ToLower();
                                    myProfile.WhatsAppNoPrefix = Convert.ToString(dt.Rows[i]["u_whatsappno_prefix"]);
                                    myProfile.WhatsAppNo = Convert.ToString(dt.Rows[i]["u_whatsappno"]);
                                    myProfile.SelectedAgeGroup = Convert.ToString(dt.Rows[i]["age_group"]);
                                    myProfile.ComWithEmail = Convert.ToString(dt.Rows[i]["ComWithEmail"]);
                                    myProfile.ComWithWhatsApp = Convert.ToString(dt.Rows[i]["ComWithWhatsApp"]);
                                    myProfile.ComWithPhone = Convert.ToString(dt.Rows[i]["ComWithPhone"]);

                                    Reason = CheckRequiredValidation(myProfile);
                                    Reason += checkValidRegisterDetails(myProfile);
                                    Reason = Reason.TrimEnd(',');

                                    if (String.IsNullOrEmpty(Reason))
                                        dt.Rows[i]["Status"] = "Ok";
                                    else
                                        dt.Rows[i]["Status"] = "Fail";

                                    dt.Rows[i]["Reason"] = Reason;
                                    if (!String.IsNullOrEmpty(myProfile.Name))
                                        dt.Rows[i]["u_name"] = clsCommon.Encrypt(myProfile.Name);
                                    if (!String.IsNullOrEmpty(myProfile.Email))
                                        dt.Rows[i]["u_email"] = clsCommon.Encrypt(myProfile.Email);
                                    if (!String.IsNullOrEmpty(myProfile.WhatsAppNo))
                                        dt.Rows[i]["u_whatsappno"] = clsCommon.Encrypt(myProfile.WhatsAppNo);

                                    myProfileList.Add(myProfile);
                                }

                                dtAll.Merge(dt);
                            }
                        }
                    }

                    if (myProfileList != null && myProfileList.Any() && dtAll != null && dtAll.Rows.Count > 0)
                    {
                        InsertRegisterTempData(dtAll);
                    }

                    responce.Result = myProfileList;
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
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            return responce;
        }
        private static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            FileInfo existingFile = new FileInfo(strFilePath);
            if (!existingFile.Extension.StartsWith(".csv"))
            {
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    //get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;     //get row count

                    for (int col = 1; col <= colCount; col++)
                    {
                        dt.Columns.Add(worksheet.Cells[1, col].Value?.ToString().Trim());
                    }

                    for (int row = 1; row <= rowCount; row++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < colCount; i++)
                        {
                            string dta = worksheet.Cells[row + 1, i + 1].Value?.ToString().Trim();
                            dr[i] = dta;
                        }
                        if (!AreAllColumnsEmpty(dr))
                        {
                            dt.Rows.Add(dr);
                        }
                    }

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
        private  static Responce InsertRegisterTempData(DataTable registration)
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
                SendEmailToSaveFromExcelFile();
            }
            return responce;
        }
        private static  void SendEmailToSaveFromExcelFile()
        {
            List<RegisterTemp> List = new List<RegisterTemp>();
            dbProxy _db = new dbProxy();

            List<SetParameters> sp = new List<SetParameters>()
                {
                    new SetParameters{ ParameterName = "@QType", Value = "1" }

                };
            List = _db.GetDataMultiple("GetAllRegistration_Temp", List, sp);
            if (List != null && List.Count > 0)
            {

                foreach (var item in List)
                {
                    if (!string.IsNullOrWhiteSpace(item.Mode) && item.Mode.Equals("Excel"))
                    {
                        string emailLink = string.Empty;
                        string encryptUserId = string.Empty;
                        string siteURL = string.Empty;
                        if (!string.IsNullOrEmpty(item.UserId.ToString()))
                            encryptUserId = clsCommon.encrypto(item.UserId.ToString());
                        siteURL = ConfigurationManager.AppSettings["SiteUrl"].ToString();
                        emailLink = siteURL + "my-account/set-password?id=" + encryptUserId;
                        HtmlRenderHelper.HtmlRenderHelper renderHelper = new HtmlRenderHelper.HtmlRenderHelper();

                        RegistrationMailer.Name = clsCommon.Decrypt(item.Name);
                        RegistrationMailer.Link = emailLink;

                        RegistrationMailer.ViewName = "/Views/mailer/registrationMailerForExcel.cshtml";
                        var responce = renderHelper.GetRegistartionHtml(RegistrationMailer);
                        if (responce != null && responce.StatusCode == HttpStatusCode.OK)
                        {
                            SenderMailer mailsend = new SenderMailer();
                            mailsend.SendUserValidateEmailerContent("HP Print Learn Center Registration Link", clsCommon.Decrypt(item.Email), "", responce.Result.ToString());
                        }
                        
                    }
                }
            }
        }
        private static string CheckRequiredValidation(RegisterTemp registration)
        {
            string Reason = string.Empty;
            try
            {
                if (String.IsNullOrWhiteSpace(registration.Email))
                    Reason = "Email Id cn not be blank.,";
                else if (String.IsNullOrWhiteSpace(registration.WhatsAppNo))
                    Reason += "Mobile no can not be blank.,";
                else if (String.IsNullOrWhiteSpace(registration.SelectedAgeGroup))
                    Reason += "Age group can not be blank.,";
                else if (String.IsNullOrWhiteSpace(registration.ComWithEmail))
                    Reason += "Mail communication require.,";
                else if (String.IsNullOrWhiteSpace(registration.ComWithWhatsApp))
                    Reason += "WhatsApp communication require.,";
                else if (String.IsNullOrWhiteSpace(registration.ComWithPhone))
                    Reason += "Mobile communication require.,";
            }
            catch (Exception ex)
            {
                Reason = ex.ToString();
            }
            return Reason;
        }

        private static string checkValidRegisterDetails(RegisterTemp registration)
        {
            string Reason = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(registration.Name))
                {
                    if (!Regex.IsMatch(registration.Name, @"^[a-zA-Z ]+$"))
                    {
                        Reason = "Name must be in only Alphabate characters.,";
                    }
                }
                if (!string.IsNullOrWhiteSpace(registration.Email))
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(registration.Email);
                    if (!match.Success)
                        Reason += "Please enter correct format for email id.,";
                    //var addr = new System.Net.Mail.MailAddress(registration.Email);
                    //if (addr.Address != registration.Email)
                    //{
                    //    Reason += "Please enter correct format for email id.,";
                    //}
                }
                if (!string.IsNullOrWhiteSpace(registration.WhatsAppNo))
                {
                    if (Regex.Match(registration.WhatsAppNo, @"^(\+[0-9]{9})$").Success)
                    {
                        Reason += "Please enter valid  WhatsApp no.,";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(registration.SelectedAgeGroup))
                {
                    string[] ageGroup = registration.SelectedAgeGroup.Split('|');
                    if (ageGroup.Length == 0)
                    {
                        Reason += "Please mention age group with comma seperated,";
                    }
                    else if (ageGroup.Length > 0)
                    {
                        try
                        {
                            foreach (var ages in ageGroup)
                            {
                                if (ages.Length >= 3 && ages.Contains('-'))
                                {
                                    if ((int.Parse(ages.Split('-').First()).GetType() != typeof(int) || int.Parse(ages.Split('-').Last()).GetType() != typeof(int)))
                                    {
                                        Reason += "Mention age group is not in correct format,";
                                    }
                                }
                                else
                                {
                                    Reason += "Mention age group is not in correct format,";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Reason += "Mention age group is not in correct format,";
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

        public Responce CreateZipForExcelFile(string LocalSaveFilePath)
        {
            Responce responce = new Responce();
            try
            {
                string path = LocalSaveFilePath;
                DirectoryInfo dir = new DirectoryInfo(path);
                if (dir != null && dir.GetFiles() != null && dir.GetFiles().Count() > 0)
                {
                    byte[] fileBytes = null;
                    string FileName = string.Empty;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                        {
                            foreach (FileInfo flInfo in dir.GetFiles())
                            {
                                ZipArchiveEntry zipItem = zip.CreateEntry(flInfo.Name);
                                FileName = zipItem.FullName;
                                using (MemoryStream originalFileMemoryStream = new MemoryStream(File.ReadAllBytes(LocalSaveFilePath + flInfo.Name)))
                                {
                                    using (Stream entryStream = zipItem.Open())
                                    {
                                        originalFileMemoryStream.CopyTo(entryStream);
                                    }
                                }
                                flInfo.Delete();
                            }
                        }
                        fileBytes = memoryStream.ToArray();
                        SenderMailer sender = new SenderMailer();
                        MailContentModel mailer = GetSMTPForAdmin();
                        MemoryStream attachmentStream = new MemoryStream(memoryStream.ToArray());
                        if (mailer != null && mailer.EmailTo != null)
                        {
                            sender.SendeMailWithAttchment("text", attachmentStream, DateTime.Now.Ticks.ToString() + ".zip", "application/zip", mailer);
                        }
                    }
                    responce.Result = fileBytes;
                    responce.StatusCode = HttpStatusCode.OK;

                }
                else
                {

                    responce.Result = null;
                    responce.Message = "Excel File Folder Is Empty";
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
        private MailContentModel GetSMTPForUser()
        {
            MailContentModel mail = new MailContentModel();
            using (var cref = _context.EnsureUmbracoContext())
            {
                var cache = cref.UmbracoContext.Content;
                var node = cache.GetById(Constant.Constant.SMTFForUser);
                if (node != null)
                {
                    mail.Name = node.Name;
                    mail.Subject = node.Value("subject").ToString();
                    mail.EmailTo = node.Value("emailTo").ToString();
                    mail.EmailBcc = node.Value("emailBcc") as IEnumerable<string>;
                    mail.EmailCC = node.Value("emailCC") as IEnumerable<string>;
                }
                return mail;
            }


        }
        private MailContentModel GetSMTPForAdmin()
        {
            MailContentModel mail = new MailContentModel();
            using (var cref = _context.EnsureUmbracoContext())
            {
                var cache = cref.UmbracoContext.Content;
                var node = cache.GetById(Constant.Constant.SMTFForAdmin);
                if (node != null)
                {
                    mail.Name = node.Name;
                    mail.Subject = node.Value("subject").ToString();
                    mail.EmailTo = node.Value("emailTo").ToString();
                    mail.EmailBcc = node.Value("emailBcc") as IEnumerable<string>;
                    mail.EmailCC = node.Value("emailCC") as IEnumerable<string>;
                }
                return mail;
            }

        }
    }
}