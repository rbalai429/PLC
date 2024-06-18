using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HPPlc
{
    /// <summary>
    /// Summary description for ImageFileUpload
    /// </summary>
    public class ImageFileUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                if (context.Request.QueryString["upload"] != null)
                {
                    string _queryString = context.Request.QueryString["upload"].ToString();
                    string[] _GetName = _queryString.Split('.');
                    string _fileName = _GetName[0].ToString();

                    HttpFileCollection files = context.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fname;

                        fname = file.FileName;
                        string _getExt = Path.GetExtension(file.FileName);

                        fname = Path.Combine(context.Server.MapPath("/WebinarImage/"), _fileName.ToString() + _getExt.ToString());
                        file.SaveAs(fname);
                    }

                }
               
            }
            context.Response.ContentType = "text/plain";
        }
        //public void ProcessRequest(HttpContext context)
        //{
        //    context.Response.ContentType = "text/plain";
        //    context.Response.Write("Hello World");
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}