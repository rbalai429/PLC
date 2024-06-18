using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class ExportToExcel
	{
        public void DownloadExcelClosedXML(string fileName,DataTable dt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(dt);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet";
                   // HttpContext.Current.Response.ContentType = "application/vnd.xls";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename="+ fileName + ".xlsx");
                    HttpContext.Current.Response.BinaryWrite(content);
                    //HttpContext.Current.Response.Close();
                    //HttpContext.Current.Response.End();
                    HttpContext.Current.Response.Flush();
                }
            }
        }
    }
}