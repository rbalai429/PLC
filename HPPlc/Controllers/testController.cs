using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace HPPlc.Controllers
{
    public class testController : SurfaceController
    {
        public int PdfPageCount()
        {
            string ppath = Server.MapPath("/ExcelFile/3-4-week1-maths-shapes-1.pdf");
            PdfReader pdfReader = new PdfReader(ppath);
            int numberOfPages = pdfReader.NumberOfPages;

            return numberOfPages;
        }
    }
}