using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.PDFGenerator
{
    public static class PDFGenerator
    {
        #region NReco.PdfGenerator
        public static byte[] GenerateRuntimePDF(string html, string Header = null)
        {
            try
            {
                HtmlToPdfConverter nRecohtmltoPdfObj = new HtmlToPdfConverter();
                if (Header != null)
                    nRecohtmltoPdfObj.PageHeaderHtml = Header;
                return nRecohtmltoPdfObj.GeneratePdf(html);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }
}