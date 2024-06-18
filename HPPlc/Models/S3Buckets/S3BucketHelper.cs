using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HPPlc.Models.S3Buckets
{
    public class S3BucketHelper
    {
        // Specify your bucket region (an example region is shown).  
        private static readonly string bucketName = ConfigurationManager.AppSettings["BucketFileSystem:BucketName"];
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;
        private static readonly string accesskey = ConfigurationManager.AppSettings["AWSAccessKey"];
        private static readonly string secretkey = ConfigurationManager.AppSettings["AWSSecretKey"];
        //private static readonly string folderName = "Invoice";//ConfigurationManager.AppSettings["BucketFileSystem:MediaPrefix"];
        private static readonly string BucketHostname = ConfigurationManager.AppSettings["BucketFileSystem:BucketHostname"];

        public Responce sendMyFileToS3Async(byte[] stream, string fileName, string folderName = "Invoice", string subFolderName = "")
        {
            Responce responce = new Responce();
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    responce.Message = "Please Enter File Name";
                    return responce;
                }

                if (stream != null && stream.Length > 0)
                {
                    fileName = folderName != "media"
                        ? folderName + "/" + fileName
                        : folderName + "/" + subFolderName + "/" + fileName;
                    var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);
                    TransferUtility utility = new TransferUtility(s3Client);
                    PutObjectRequest putRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = fileName,
                        InputStream = new MemoryStream(stream)
                    };
                    PutObjectResponse response1 = s3Client.PutObject(putRequest);
                    responce.StatusCode = System.Net.HttpStatusCode.OK;
                    responce.Result = folderName != "media"
                        ? BucketHostname + "/" + fileName
                        : "/" + fileName;//uploaded file url
                }
                else
                {
                    responce.StatusCode = System.Net.HttpStatusCode.NoContent;
                    responce.Message = "Please select input file";
                }
            }
            catch (Exception ex)
            {
                responce.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                responce.Message = ex.Message;
            }
            return responce;
        }
    }
}