using HPPlc.Models;
using HPPlc.Models.ImportExcelFiles;

using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.WebApi;
namespace HPPlc.Controllers.ImportExcel
{
    public class ImportExcelFileAuthorizeApiController : UmbracoAuthorizedApiController
    {

        private string FolderName = "/ExcelFile/";
        private string LocalSaveFilePath = HttpContext.Current.Server.MapPath("/ExcelFile/").ToString();
        string Remote_FolderPath = ConfigurationManager.AppSettings["FolderName"].ToString();
        string Remote_BkupFolderPath = ConfigurationManager.AppSettings["BkupFolderName"].ToString();
        
        
        private string Host = ConfigurationManager.AppSettings["SFTPHost"].ToString();
        private string Username = ConfigurationManager.AppSettings["Username"].ToString();
        private string Password = ConfigurationManager.AppSettings["Password"].ToString();
        private readonly ImportExcelFilesHelper importExcelFilesHelper;
        public ImportExcelFileAuthorizeApiController(IUmbracoContextFactory context)
        {
            importExcelFilesHelper = new ImportExcelFilesHelper(context);
        }

        #region Import Excel file from sftp start
        [System.Web.Http.HttpGet]
        public async Task<Responce> ImportExcelFiles()
        {
            Responce responce = new Responce();
            try
            {
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(FolderName));
                if (!exists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(FolderName));
                await Task.Run(() =>
                {

                    Task.Factory.StartNew(() =>
                    {
                        using (var sftp = new SftpClient(Host, Username, Password))
                        {
                            sftp.Connect();
                            importExcelFilesHelper.DownloadDirectory(sftp, Remote_FolderPath, LocalSaveFilePath, Remote_BkupFolderPath);
                            sftp.Disconnect();
                            responce.Message = "File Import Successfully..!";
                            responce.StatusCode = HttpStatusCode.OK;
                        }
                    }).Wait();
                });

            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            return responce;
        }

        [System.Web.Http.HttpGet]
        public async Task<Responce> ImportExcelFilesLocal()
        {
            Responce responce = new Responce();
            try
            {
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(FolderName));
                if (!exists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(FolderName));
                await Task.Run(() =>
                {
                    
                    Task.Factory.StartNew(() =>
                    {
                        using (var sftp = new SftpClient(Host, Username, Password))
                        {
                            sftp.Connect();
                            importExcelFilesHelper.DownloadDirectory(sftp, Remote_FolderPath, LocalSaveFilePath, Remote_BkupFolderPath);
                            sftp.Disconnect();
                            responce.Message = "File Import Successfully..!";
                            responce.StatusCode = HttpStatusCode.OK;
                        }
                    }).Wait();
                });

            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            return responce;
        }
        [System.Web.Http.HttpPost]
        public Responce PostSaveFile()
        {
            Responce responce = new Responce();
            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0 ?
       HttpContext.Current.Request.Files[0] : null;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

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
            }
            catch (Exception ex)
            {
                responce.Message = ex.Message;
                responce.StatusCode = HttpStatusCode.InternalServerError;
            }
            return responce;
        }
        #endregion Import Excel file from sftp end

        #region Get Excel data from folder start
        [System.Web.Http.HttpGet]
        public async Task<Responce> ImportExcelToTableAsync()
        {
            Responce responce = new Responce();
            try
            {
                await Task.Run(() =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        responce = ImportExcelFilesHelper.ImportExcelToTable(LocalSaveFilePath);
                    }).Wait();
                });
            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            return responce;
        }


        [System.Web.Http.HttpGet]
        public Responce GetRegister_TempList()
        {
            Responce responce = new Responce();
            try
            {
                int QType = 1;
                dbProxy _db = new dbProxy();
                GetStatus insertStatus = new GetStatus();
                List<RegisterTemp> registerTemps = new List<RegisterTemp>();
                List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@QType", Value = QType.ToString()},
                    };
                registerTemps = _db.GetDataMultiple("GetRegistration_Temp", registerTemps, sp);
                if (registerTemps != null && registerTemps.Count > 0)
                {
                    foreach (var item in registerTemps)
                    {
						if(!String.IsNullOrEmpty(item.Name))
							item.Name = clsCommon.Decrypt(item.Name);
						if (!String.IsNullOrEmpty(item.Email))
							item.Email = clsCommon.Decrypt(item.Email);
						if (!String.IsNullOrEmpty(item.WhatsAppNo))
							item.WhatsAppNo = clsCommon.Decrypt(item.WhatsAppNo);
                    }
                }
                responce.Result = registerTemps;
                responce.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            return responce;
        }

        [System.Web.Http.HttpGet]
        public Responce InsertRegisterBackUpData()
        {
            Responce responce = new Responce();
            try
            {
                int QType = 1;
                dbProxy _db = new dbProxy();
                List<RegisterTemp> registerTemps = new List<RegisterTemp>();
                List<SetParameters> sp = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@QType", Value =QType.ToString()}
                    };
                registerTemps = _db.GetDataMultiple("GetRegistration_Temp", registerTemps, sp);


                GetStatus insertStatus = new GetStatus();
                foreach (RegisterTemp registration in registerTemps)
                {
                    List<SetParameters> spinsert = new List<SetParameters>()
                    {
                        new SetParameters { ParameterName = "@QType", Value =QType.ToString()},
                        new SetParameters { ParameterName = "@User_Id", Value = registration.UserId.ToString() },
                        new SetParameters { ParameterName = "@name", Value = clsCommon.Encrypt(registration.Name) },
                        new SetParameters { ParameterName = "@email", Value = clsCommon.Encrypt(registration.Email) },
                        new SetParameters { ParameterName = "@whatsappno_prefix", Value = registration.WhatsAppNoPrefix },
                        new SetParameters { ParameterName = "@whatsappno", Value = clsCommon.Encrypt(registration.WhatsAppNo) },
                        new SetParameters { ParameterName = "@age_group", Value = registration.SelectedAgeGroup },
                        new SetParameters { ParameterName = "@ComWithEmail", Value = registration.ComWithEmail },
                        new SetParameters { ParameterName = "@ComWithWhatsApp", Value = registration.ComWithWhatsApp == null ? "" : registration.ComWithWhatsApp },
                        new SetParameters { ParameterName = "@ComWithPhone", Value = registration.ComWithPhone == null ? "" : registration.ComWithPhone },
                        new SetParameters { ParameterName = "@CheckedTAndC", Value = registration.termsChecked == null ? "No" : "Yes" },
                        new SetParameters { ParameterName = "@GenereatedReferralCode", Value =registration.ReferralCode },
                         new SetParameters { ParameterName = "@EncPassword", Value =registration.EncPassword },
                         new SetParameters { ParameterName = "@FromTemp", Value ="1" },
                    new SetParameters { ParameterName = "@Reason", Value =registration.Reason },

                    };

                    insertStatus = _db.StoreData("Insert_Registration_BackUp", spinsert);
                }

                if (insertStatus.returnStatus == "Success")
                {
                    responce.StatusCode = HttpStatusCode.OK;
                    responce.Message = insertStatus.returnStatus;
                }

            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.ToString();
            }
            return responce;
        }
        #endregion Get Excel data from folder end

        
       
        #region Create Zip File Start
        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage CreateZip()
        //{
        //    Responce responce = new Responce();
        //    HttpResponseMessage result = null;
        //    bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(FolderName));
        //    if (exists)
        //    {
        //        responce = importExcelFilesHelper.CreateZipForExcelFile(LocalSaveFilePath);
        //        if (responce.StatusCode == HttpStatusCode.OK)
        //        {

        //            byte[] bytes = (byte[])responce.Result;


        //            result = Request.CreateResponse(HttpStatusCode.OK);
        //            result.Content = new ByteArrayContent(bytes);
        //            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //            result.Content.Headers.ContentDisposition.FileName = DateTime.Now.ToString() + ".zip";
        //        }
        //        else
        //        {
        //            string yourJson = "No File Found";
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");
        //            return response;

        //        }
        //    }
        //    return result;

        //}


        #endregion Create Zip File End
    }

}