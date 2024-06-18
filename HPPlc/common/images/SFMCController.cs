using HP_COMM.Models;
using HPCommunity.Models;
using LINQtoCSV;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Web.Mvc;
using DidiSoft.Pgp;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;

namespace HP_COMM.Controllers
{
    public class SFMCController : SurfaceController
    {
        public ActionResult DownloadSFMCData()//string url
        {
            ////Generate a public/private key pair.  
            //RSA rsa = RSA.Create();
            ////Save the public key information to an RSAParameters structure.  
            //RSAParameters rsaKeyInfo = rsa.ExportParameters(false);

            //string dataFile = Server.MapPath("~/SFMCExtractReport/HP_CG_IN_20220301.csv");
            //string publicKeyFile = Server.MapPath("~/Key/IN_Digitas_PublicKey.asc");
            //string encryptedFile = Server.MapPath("~/SFMCExtractReport/HP_CG_IN_20220301.csv.pgp");
            //bool asciiArmour = false;
            //PGPLibAsync pgp = new PGPLibAsync();
            //pgp.EncryptFileAsync(dataFile, publicKeyFile, encryptedFile, asciiArmour);


            //string dataFile = Server.MapPath("~/SFMCExtractReport/HP_PLC_IN_UserData_20220419.csv");
            //string publicKeyFile = Server.MapPath("~/Key/IN_Digitas_PublicKey.asc");
            //string encryptedFile = Server.MapPath("~/SFMCExtractReport/HP_PLC_IN_UserData_20220419.csv.gpg");
            //bool asciiArmour = false;
            //PGPLibAsync pgp = new PGPLibAsync();
            //pgp.EncryptFileAsync(dataFile, publicKeyFile, encryptedFile, asciiArmour);

            DataTable DT = new DataTable();
            clsSFMC _objclsSFMC = new clsSFMC();
            if (Request.QueryString["url"] != null && Request.QueryString["url"].ToString() == "KK")
            {
                DT = _objclsSFMC.Get_SFMC_information();
                if (DT != null && DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        string uName = DT.Rows[i]["u_name"].ToString();
                        string EmailAddress = DT.Rows[i]["EmailAddress"].ToString();
                        //string u_mobileno = DT.Rows[i]["u_mobileno"].ToString();
                        if (uName != "")
                            uName = clsCommon.Decrypt(uName);
                        if (EmailAddress != "")
                            EmailAddress = clsCommon.Decrypt(EmailAddress);
                        //if (u_mobileno != "")
                        //    u_mobileno = clsCommon.Decrypt(u_mobileno);
                        DT.Rows[i]["u_name"] = uName;
                        DT.Rows[i]["EmailAddress"] = EmailAddress;
                        //DT.Rows[i]["u_mobileno"] = u_mobileno;
                    }
                }
                DT.AcceptChanges();
                if (DT != null && DT.Rows.Count > 0)
                    DTToExcelKK(DT);
            }
            if (Request.QueryString["url"] != null && Request.QueryString["url"].ToString() == "TFN")
            {
                DT = _objclsSFMC.Get_SFMC_information();
                if (DT != null && DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        string uName = DT.Rows[i]["u_name"].ToString();
                        string EmailAddress = DT.Rows[i]["EmailAddress"].ToString();
                        //string u_mobileno = DT.Rows[i]["u_mobileno"].ToString();
                        if (uName != "")
                            uName = clsCommon.Decrypt(uName);
                        if (EmailAddress != "")
                            EmailAddress = clsCommon.Decrypt(EmailAddress);
                        //if (u_mobileno != "")
                        //    u_mobileno = clsCommon.Decrypt(u_mobileno);
                        DT.Rows[i]["u_name"] = uName;
                        DT.Rows[i]["EmailAddress"] = EmailAddress;
                        //DT.Rows[i]["u_mobileno"] = u_mobileno;
                    }
                }
                DT.AcceptChanges();
                if (DT != null && DT.Rows.Count > 0)
                    DTToExcelTFN(DT);
            }
            else
                return View();
            return View();
        }


        public void DTToExcelKK(DataTable dt)
        {
            string csv = string.Empty;
            foreach (DataColumn column in dt.Columns)
            {
                csv += column.ColumnName + ',';
            }
            csv += "\r\n";
            foreach (System.Data.DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            string FileName = "HP_CG_IN_" + DateTime.Now.ToString("yyyyMMdd");



            System.IO.File.WriteAllText(Server.MapPath("~/SFMCExtractReport/") + FileName + ".csv", csv);

            List<DMSReportEntity> dmsdatalist = new List<DMSReportEntity>();
            dmsdatalist = clsCommon.ConvertDataTable<DMSReportEntity>(dt);

            CsvFileDescription csvFileDescription = new CsvFileDescription
            {
                SeparatorChar = '|',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true
            };
            CsvContext csvContext = new CsvContext();
            byte[] file = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                {
                    csvContext.Write<DMSReportEntity>(dmsdatalist, streamWriter, csvFileDescription);
                    streamWriter.Flush();
                    file = memoryStream.ToArray();
                }
            }

            //string dataFile = Server.MapPath("~/SFMCExtractReport/" + FileName + ".csv");
            //string publicKeyFile = Server.MapPath("~/Key/IN_Digitas_PublicKey.asc");
            //string encryptedFile = Server.MapPath("~/SFMCExtractReport/" + FileName + ".csv.gpg");

            //bool asciiArmour = false;
            //PGPLibAsync pgp = new PGPLibAsync();
            //pgp.EncryptFileAsync(dataFile, publicKeyFile, encryptedFile, asciiArmour);

            string filename = FileName + ".csv";
            // string filename = FileName + ".csv.gpg";
            //try
            //{

            //    // KK Thrusday 11
            //    string Host = "a1.usa.hp.com";
            //    int Port = 2222;
            //    string Username = "ind_ols";
            //    string Password = "Tea$UsQ6";
            //    String RemoteFileName = "/Creators_Garage";


            //    String LocalDestinationFilename = Server.MapPath("~\\SFMCExtractReport\\").ToString() + filename;

            //    if (!System.IO.File.Exists(LocalDestinationFilename))
            //    {
            //        System.IO.File.WriteAllBytes(LocalDestinationFilename, file);
            //    }
            //    using (var sftp = new SftpClient(Host, Port, Username, Password))
            //    {
            //        sftp.Connect();
            //        sftp.ChangeDirectory(RemoteFileName);
            //        var listDirectory = sftp.ListDirectory(RemoteFileName);

            //        using (var filestream = new FileStream(LocalDestinationFilename, FileMode.Open))
            //        {
            //            sftp.BufferSize = 4 * 1024;
            //            sftp.UploadFile(filestream, Path.GetFileName(LocalDestinationFilename));
            //        }

            //        sftp.Disconnect();
            //    }
            //}
            //catch (Exception exp) { }

        }
        public byte[] DTToExcelKK1(DataTable dt)
        {
            byte[] file = null;
            string csv = string.Empty;
            foreach (DataColumn column in dt.Columns)
            {
                csv += column.ColumnName + ',';
            }
            csv += "\r\n";
            foreach (System.Data.DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            string FileName = "HP_CG_IN_" + DateTime.Now.ToString("yyyyMMdd");



            System.IO.File.WriteAllText(Server.MapPath("~/SFMCExtractReport/") + FileName + ".csv", csv);

            List<DMSReportEntity> dmsdatalist = new List<DMSReportEntity>();
            dmsdatalist = clsCommon.ConvertDataTable<DMSReportEntity>(dt);
            file = ListToExcel(dmsdatalist, "Password");
            //CsvFileDescription csvFileDescription = new CsvFileDescription
            //{
            //    SeparatorChar = '|',
            //    FirstLineHasColumnNames = true,
            //    EnforceCsvColumnAttribute = true
            //};
            //CsvContext csvContext = new CsvContext();
            //var keyGenerator = new Rfc2898DeriveBytes("TEST", 8);
            //var rijndael = Rijndael.Create();

            //// BlockSize, KeySize in bit --> divide by 8  
            //rijndael.IV = keyGenerator.GetBytes(rijndael.BlockSize / 8);
            //rijndael.Key = keyGenerator.GetBytes(rijndael.KeySize / 8);
            //using (MemoryStream memoryStream = new MemoryStream())
            //{

            //    memoryStream.Write(keyGenerator.Salt, 0, 8);
            //    using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
            //    {
            //        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
            //        {
            //            csvContext.Write<DMSReportEntity>(dmsdatalist, streamWriter, csvFileDescription);
            //            streamWriter.Flush();
            //            //file = memoryStream.ToArray();
            //            streamWriter.Dispose();
            //        }
                    
            //    }
            //}

            //string dataFile = Server.MapPath("~/SFMCExtractReport/" + FileName + ".csv");
            //string publicKeyFile = Server.MapPath("~/Key/IN_Digitas_PublicKey.asc");
            //string encryptedFile = Server.MapPath("~/SFMCExtractReport/" + FileName + ".csv.gpg");

            //bool asciiArmour = false;
            //PGPLibAsync pgp = new PGPLibAsync();
            //pgp.EncryptFileAsync(dataFile, publicKeyFile, encryptedFile, asciiArmour);

            string filename = FileName + ".csv";
            // string filename = FileName + ".csv.gpg";
            //try
            //{

            //    // KK Thrusday 11
            //    string Host = "a1.usa.hp.com";
            //    int Port = 2222;
            //    string Username = "ind_ols";
            //    string Password = "Tea$UsQ6";
            //    String RemoteFileName = "/Creators_Garage";


            //    String LocalDestinationFilename = Server.MapPath("~\\SFMCExtractReport\\").ToString() + filename;

            //    if (!System.IO.File.Exists(LocalDestinationFilename))
            //    {
            //        System.IO.File.WriteAllBytes(LocalDestinationFilename, file);
            //    }
            //    using (var sftp = new SftpClient(Host, Port, Username, Password))
            //    {
            //        sftp.Connect();
            //        sftp.ChangeDirectory(RemoteFileName);
            //        var listDirectory = sftp.ListDirectory(RemoteFileName);

            //        using (var filestream = new FileStream(LocalDestinationFilename, FileMode.Open))
            //        {
            //            sftp.BufferSize = 4 * 1024;
            //            sftp.UploadFile(filestream, Path.GetFileName(LocalDestinationFilename));
            //        }

            //        sftp.Disconnect();
            //    }
            //}
            //catch (Exception exp) { }
            return file;
        }
        public void DTToExcelTFN(DataTable dt)
        {
            //string csv = string.Empty;
            //foreach (DataColumn column in dt.Columns)
            //{
            //    csv += column.ColumnName + ',';
            //}
            //csv += "\r\n";
            //foreach (System.Data.DataRow row in dt.Rows)
            //{
            //    foreach (DataColumn column in dt.Columns)
            //    {
            //        //Add the Data rows.
            //        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
            //    }

            //    //Add new line.
            //    csv += "\r\n";
            //}

            string FileName = "HP_CG_IN_" + DateTime.Now.ToString("yyyyMMdd");



            //System.IO.File.WriteAllText(Server.MapPath("~/SFMCExtractReport/") + FileName + ".csv", csv);

            List<DMSReportEntity> dmsdatalist = new List<DMSReportEntity>();
            dmsdatalist = clsCommon.ConvertDataTable<DMSReportEntity>(dt);

            CsvFileDescription csvFileDescription = new CsvFileDescription
            {
                SeparatorChar = '|',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true
            };
            CsvContext csvContext = new CsvContext();
            byte[] file = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                {
                    csvContext.Write<DMSReportEntity>(dmsdatalist, streamWriter, csvFileDescription);
                    streamWriter.Flush();
                    file = memoryStream.ToArray();
                }
            }

            string dataFile = Server.MapPath("~/SFMCExtractReport/" + FileName + ".csv");
            string publicKeyFile = Server.MapPath("~/Key/IN_Digitas_PublicKey.asc");
            string encryptedFile = Server.MapPath("~/SFMCExtractReport/" + FileName + ".csv.gpg");
            bool asciiArmour = false;
            PGPLibAsync pgp = new PGPLibAsync();
            pgp.EncryptFileAsync(dataFile, publicKeyFile, encryptedFile, asciiArmour);


            //string filename = FileName + ".csv";
            string filename = FileName + ".csv.gpg";
            //string filenameEnc = FileName + "enc.csv";
            try
            {

                // KK Thrusday 11

                // Tifny Wednesday 11
                //string Host = "ftp.s4.exacttarget.com";
                //int Port = 22;
                //string Username = "1306115";
                //string Password = "3Hha2M0j";
                //String RemoteFileName = "/Import/IN_creator_garage_digitas/FY22Q1";
                //String LocalDestinationFilename = Server.MapPath("~\\SFMCExtractReport\\").ToString() + filename;


                string Host = "ftp.s4.exacttarget.com";
                int Port = 22;
                string Username = "1306115_IIS";
                string Password = "ZeIq!3@bAasP173!";
                String RemoteFileName = "/CG/Production";
                String LocalDestinationFilename = Server.MapPath("~\\SFMCExtractReport\\").ToString() + filename;



                if (!System.IO.File.Exists(LocalDestinationFilename))
                {
                    System.IO.File.WriteAllBytes(LocalDestinationFilename, file);
                }
                using (var sftp = new SftpClient(Host, Port, Username, Password))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(RemoteFileName);
                    var listDirectory = sftp.ListDirectory(RemoteFileName);

                    using (var filestream = new FileStream(LocalDestinationFilename, FileMode.Open))
                    {
                        sftp.BufferSize = 4 * 1024;
                        //sftp.UploadFile(filestream, Path.GetFileName(LocalDestinationFilename));
                    }

                    sftp.Disconnect();
                }
            }
            catch (Exception exp) { }

        }
        public string DecryptSt()//string url
        {
            DataTable DT = new DataTable();
            clsSFMC _objclsSFMC = new clsSFMC();
            if (Request.QueryString["ST"] != null)
            {
                string st = clsCommon.Encrypt(Request.QueryString["ST"].ToString());
                HttpContext.Response.Write(st);
            }
            if (Request.QueryString["ET"] != null)
            {
                string check = Request.QueryString["ET"].ToString();
                check = check.Replace(" ", "+");
                string st = clsCommon.Decrypt(check);
                HttpContext.Response.Write(st);
            }
            return "";
        }
        //public void UploadEncrytped(DataTable dt)
        //{
        //    string csv = string.Empty;
        //    foreach (DataColumn column in dt.Columns)
        //    {
        //        csv += column.ColumnName + ',';
        //    }
        //    csv += "\r\n";
        //    foreach (System.Data.DataRow row in dt.Rows)
        //    {
        //        foreach (DataColumn column in dt.Columns)
        //        {
        //            //Add the Data rows.
        //            csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
        //        }

        //        //Add new line.
        //        csv += "\r\n";
        //    }

        //    string FileName = "HP_CG_IN_" + DateTime.Now.ToString("yyyyMMdd");

        //    System.IO.File.WriteAllText(Server.MapPath("~/SFMCExtractReport/") + FileName + ".csv", csv);

        //    List<DMSReportEntity> dmsdatalist = new List<DMSReportEntity>();
        //    dmsdatalist = clsCommon.ConvertDataTable<DMSReportEntity>(dt);

        //    CsvFileDescription csvFileDescription = new CsvFileDescription
        //    {
        //        SeparatorChar = '|',
        //        FirstLineHasColumnNames = true,
        //        EnforceCsvColumnAttribute = true
        //    };
        //    CsvContext csvContext = new CsvContext();
        //    byte[] file = null;
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        using (StreamWriter streamWriter = new StreamWriter(memoryStream))
        //        {
        //            csvContext.Write<DMSReportEntity>(dmsdatalist, streamWriter, csvFileDescription);
        //            streamWriter.Flush();
        //            file = memoryStream.ToArray();
        //        }
        //    }
        //    string filename = FileName + ".csv";
        //    string filenameEnc = FileName + "enc.csv.gpg";
        //    clsCommon.EncryptFile(Server.MapPath("~/SFMCExtractReport/") + FileName + ".csv", Server.MapPath("~/SFMCExtractReport/") + filenameEnc);

        //}

        [System.Web.Http.HttpGet]
        public FileResult ExportCSV()
        {
            try
            {
                Responce responce = new Responce();
                byte[] data = null;
                DataTable DT = new DataTable();
                clsSFMC _objclsSFMC = new clsSFMC();

                DT = _objclsSFMC.Get_SFMC_information();
                if (DT != null && DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        string uName = DT.Rows[i]["u_name"].ToString();
                        string EmailAddress = DT.Rows[i]["EmailAddress"].ToString();
                        //string u_mobileno = DT.Rows[i]["u_mobileno"].ToString();
                        if (uName != "")
                            uName = clsCommon.Decrypt(uName);
                        if (EmailAddress != "")
                            EmailAddress = clsCommon.Decrypt(EmailAddress);
                        //if (u_mobileno != "")
                        //    u_mobileno = clsCommon.Decrypt(u_mobileno);
                        DT.Rows[i]["u_name"] = uName;
                        DT.Rows[i]["EmailAddress"] = EmailAddress;
                        //DT.Rows[i]["u_mobileno"] = u_mobileno;
                    }
                }
                DT.AcceptChanges();
                data = DTToExcelKK1(DT);
                return File(data, "application/octet-stream", Path.GetFileName(DateTime.Now.ToString("dd-MM-yyyy") + "-PasswordProtected.xlsx"));

            }
            catch (Exception ex)
            {

            }
            return null;

        }
        public byte[] ListToExcel<T>(List<T> query, string WorksheetName)
        {
            Responce responce = new Responce();
            try
            {
                //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    //Create the worksheet
                    //pck.Encryption.IsEncrypted = true;
                    //pck.Encryption.Password = "T";
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(WorksheetName);

                    //get our column headings
                    var t = typeof(T);
                    var Headings = t.GetProperties();
                    for (int i = 0; i < Headings.Count(); i++)
                    {

                        ws.Cells[1, i + 1].Value = Headings[i].Name;
                        if (Headings[i].PropertyType == typeof(DateTime) || Headings[i].PropertyType == typeof(DateTime?))
                        {
                            ws.Cells[1, i + 1].Style.Numberformat.Format = "MMMM, dd  yyyy HH:mm:ss";
                            ws.Cells[1, i + 1].AutoFitColumns();
                            ;
                            for (int j = 2; j < query.Count() + 2; j++)
                            {
                                ws.Cells[j, i + 1].Style.Numberformat.Format = "MMMM, dd  yyyy HH:mm:ss";
                                ws.Cells[j, i + 1].AutoFitColumns();
                            }
                        }
                    }

                    //populate our Data
                    if (query.Count() > 0)
                    {
                        ws.Cells["A2"].LoadFromCollection(query);
                        ws.Cells.AutoFitColumns();
                    }
                    ws.Protection.SetPassword("T");
                    //Format the header
                    using (ExcelRange rng = ws.Cells["A1:BZ1"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                        rng.Style.Font.Color.SetColor(Color.White);
                    }
                    //pck.Encryption.Password="T";
                    //Write it back to the client
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        pck.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        return memoryStream.ToArray();
                    }
                }

            }
            catch (Exception ex)
            {
                responce.Result = null;
                responce.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return null;

        }
        
    }
}

public class DMSReportEntity
{
    [CsvColumn(Name = "userid", FieldIndex = 1)]
    public int userid
    {
        get; set;
    }

    [CsvColumn(Name = "u_name", FieldIndex = 2)]
    public string u_name
    {
        get; set;
    }

    [CsvColumn(Name = "EmailAddress", FieldIndex = 3)]
    public string EmailAddress
    {
        get; set;
    }

    [CsvColumn(Name = "u_mobileno_prefix", FieldIndex = 4)]
    public string u_mobileno_prefix
    {
        get; set;
    }

    [CsvColumn(Name = "u_mobileno", FieldIndex = 5)]
    public string u_mobileno
    {
        get; set;
    }

    [CsvColumn(Name = "EmailConsent", FieldIndex = 6)]
    public string EmailConsent
    {
        get; set;
    }

    [CsvColumn(Name = "PhoneConsent", FieldIndex = 7)]
    public string PhoneConsent
    {
        get; set;
    }

    [CsvColumn(Name = "CheckedTnC", FieldIndex = 8)]
    public string CheckedTnC
    {
        get; set;
    }

    [CsvColumn(Name = "login_date", FieldIndex = 9)]
    public string login_date
    {
        get; set;
    }
    [CsvColumn(Name = "video_view", FieldIndex = 10)]
    public string video_view
    {
        get; set;
    }
}
