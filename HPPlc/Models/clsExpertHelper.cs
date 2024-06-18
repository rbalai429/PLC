using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace HPPlc.Models
{
    public class clsExpertHelper
    {
        public GetStatus insertExpertData()
        {
            dbProxy _db = new dbProxy();
            GetStatus insertStatus = new GetStatus();
            try
            {
                List<SetParameters> spinsert = new List<SetParameters>()
                             {
                                new SetParameters{ ParameterName = "@QType", Value = "1" },
                                new SetParameters{ ParameterName = "@UserId", Value =SessionManagement.GetCurrentSession<int>(SessionType.UserId).ToString() },

                                new SetParameters{ ParameterName = "@MettingId", Value =SessionManagement.GetCurrentSession<int>(SessionType.ExpertTalkId).ToString() },
                                new SetParameters{ ParameterName = "@MettingName", Value =SessionManagement.GetCurrentSession<string>(SessionType.MettingName).ToString() }, new SetParameters{ ParameterName = "@MeetingDate", Value =SessionManagement.GetCurrentSession<DateTime>(SessionType.MeetingDate).ToString() },
                              };

                insertStatus = _db.StoreData("insertExpertTalkHistory", spinsert);
                SessionManagement.DeleteFromSession(SessionType.ExpertTalkUrl);
                SessionManagement.DeleteFromSession(SessionType.MettingName);
                SessionManagement.DeleteFromSession(SessionType.ExpertTalkId);
            }
            catch (Exception ex)
            {

                throw;
            }

            return insertStatus;
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

                    //Format the header
                    using (ExcelRange rng = ws.Cells["A1:BZ1"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                        rng.Style.Font.Color.SetColor(Color.White);
                    }
                    //pck.Encryption.Password="TEST";
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
        public byte[] ListToExcelCSV<T>(List<T> query, string WorksheetName)
        {
            Responce responce = new Responce();
            try
            {
                //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                StringBuilder sb = new StringBuilder();

                var t = typeof(T);
                var Headings = t.GetProperties();
                for (int i = 0; i < Headings.Count(); i++)
                {
                    //Append data with separator.
                    sb.Append(Headings[i].Name + ',');
                }
                sb.Append("\r\n");
                if (query.Count() > 0)
                {
                    List<clsSubscriptionReportFoScheduler> list = new List<clsSubscriptionReportFoScheduler>();
                    list = query as List<clsSubscriptionReportFoScheduler>;

                    foreach (clsSubscriptionReportFoScheduler item in list)
                    {
                        sb.Append(item.UserUniqueId + ',');
                        sb.Append(item.Existing_New + ',');
                        sb.Append(item.DataSource + ',');
                        if (!String.IsNullOrWhiteSpace(item.u_name))
                            sb.Append(item.u_name = clsCommon.Decrypt(item.u_name) + ',');
                        else
                            sb.Append(',');

                        if (!String.IsNullOrWhiteSpace(item.u_email))
                            sb.Append(item.u_email = clsCommon.Decrypt(item.u_email) + ',');
                        else
                            sb.Append(',');

                        sb.Append(item.u_whatsappno_prefix + ',');
                        if (!String.IsNullOrWhiteSpace(item.u_whatsappno))
                            sb.Append(item.u_whatsappno = clsCommon.Decrypt(item.u_whatsappno) + ',');
                        else
                            sb.Append(',');

                        //sb.Append(item.u_name + ',');
                        //sb.Append(item.u_email + ',');
                       
                        //sb.Append(item.u_whatsappno + ',');
                        sb.Append(item.ComWithEmail + ',');
                        sb.Append(item.ComWithWhatsApp + ',');
                        sb.Append(item.ComWithPhone + ',');
                        sb.Append(item.CheckedTAndC + ',');
                        sb.Append(item.AgeGroup + ',');
                        sb.Append(item.Subscription_Plan_Opted + ',');
                        sb.Append(item.Plan_Amount.ToString() + ',');
                        sb.Append(item.Existing_User_Discount.ToString() + ',');
                        sb.Append(item.Coupon_Redeemed_Amount_Item.ToString() + ',');
                        sb.Append(item.Coupon_Name_Item + ',');
                        sb.Append(item.Coupon_Redeemed_Amount_Tran.ToString() + ',');
                        sb.Append(item.Coupon_Name_Tran + ',');
                        sb.Append(item.Source + ',');
                        sb.Append(item.Amount_Received.ToString() + ',');
                        sb.Append(item.RegistrationdatetimeStamp + ',');
                        sb.Append(item.DOC + ',');
                        sb.Append(item.ReferralCode + ',');
                        sb.Append(item.Email_Consent_UTC_ts + ',');
                        sb.Append(item.WhatsApp_Consent_UTC_ts + ',');
                        sb.Append(item.Phone_Consent_UTC_ts + ',');
                        sb.Append("\r\n");
                    }


                }

                //Append new line character.
                sb.Append("\r\n");



                return Encoding.UTF8.GetBytes(sb.ToString());

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