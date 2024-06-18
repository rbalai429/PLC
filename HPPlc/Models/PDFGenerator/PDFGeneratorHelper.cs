using HPPlc.Models.S3Buckets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HPPlc.Models.PDFGenerator
{
    public static class PDFGeneratorHelper
    {
        public static Responce GetInvoicePDf(string html)
        {
            Responce responce = new Responce();
            try
            {
                responce.Result= PDFGenerator.GenerateRuntimePDF(html);
                if (responce.Result != null)
                {
                    byte[] respbytes = responce.Result as byte[];
                    var fileName = DateTime.Now.Ticks.ToString();
                    S3BucketHelper s3BucketHelper = new S3BucketHelper();
                    responce = s3BucketHelper.sendMyFileToS3Async(respbytes, fileName + ".pdf");
                }
                //string FolderName ="/InvoicePDF/";
                //bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(FolderName));
                //if (!exists)
                //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(FolderName));
                //string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath(FolderName));
                //foreach (string file in files)
                //{
                //    File.Delete(file);
                //}
                //using (MemoryStream ms = new MemoryStream())
                //{
                //   File.WriteAllBytes(Path.Combine(HttpContext.Current.Server.MapPath(FolderName), fileName + ".pdf"), respbytes);
                //}
                return responce;
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