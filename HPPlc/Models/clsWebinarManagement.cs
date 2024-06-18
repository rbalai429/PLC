using HPPlc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HPPlc.Model
{
    public class clsWebinarManagement
    {
        public int id { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string SubCategory { get; set; }
        public string AgeGroupe { get; set; }
        public string SubscriptionType { get; set; }
        public string MeetingDate { get; set; }
        public string MeetingTitle { get; set; }
        public string MeetingUrl { get; set; }
        public string MeetingAgenda { get; set; }
        public string MeetingDuration { get; set; }
        public string AuthorName { get; set; }
        public string ImageFile { get; set; }
        public string Insert_WebinarDetails(string vType, string vLanguage, string vCategory, string vSubCategory
            , string vAgeGroupe, string vSubscriptionType, string vMeetingDate, string vMeetingTitle, string vMeetingUrl, string vMeetingAgenda, string vMeetingDuration, string vThumnailImage, string vAuthorName, string vWebinarId)
        {
            clsOnline_DataBaseHelper _objclsOnline_DataBaseHelper = new clsOnline_DataBaseHelper();

            try
            {
                string RowID = string.Empty;
                RowID = "0";
                DataTable DT = new DataTable();
                StoredProc strproc = null;
                string[] vParam = new string[14];

                vParam[0] = "@QType";
                vParam[1] = "@WebinarId";
                vParam[2] = "@Language";
                vParam[3] = "@Category";
                vParam[4] = "@SubCategory";
                vParam[5] = "@AgeGroupe";
                vParam[6] = "@SubscriptionType";
                vParam[7] = "@MeetingDate";
                vParam[8] = "@MeetingTitle";
                vParam[9] = "@MeetingUrl";
                vParam[10] = "@MeetingDuration";
                vParam[11] = "@MeetingAgenda";
                vParam[12] = "@ImageFile";
                vParam[13] = "@AuthorName";

                string[] vParamValue = new string[14];
                vParamValue[0] = vType;
                vParamValue[1] = vWebinarId;
                vParamValue[2] = vLanguage;
                vParamValue[3] = vCategory;
                vParamValue[4] = vSubCategory;
                vParamValue[5] = vAgeGroupe;
                vParamValue[6] = vSubscriptionType;
                vParamValue[7] = vMeetingDate;
                vParamValue[8] = vMeetingTitle;
                vParamValue[9] = vMeetingUrl;
                vParamValue[10] = vMeetingDuration;
                vParamValue[11] = vMeetingAgenda;
                vParamValue[12] = vThumnailImage;
                vParamValue[13] = vAuthorName;

                strproc = _objclsOnline_DataBaseHelper.ExecStoredProc("INSETR_WEBINAR_DETAILS", vParam, vParamValue, "DataTable");

                DT = strproc.DataTableObject;
                if (DT != null)
                    if (DT.Rows.Count > 0)
                        RowID = DT.Rows[0][0].ToString();

                if (Convert.ToInt32(vWebinarId) > 0)
                {

                }

                return RowID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objclsOnline_DataBaseHelper = null;
            }

        }

        public string Insert_WebinarLogDetails(string vWebinarId, string vLanguage, string vCategory, string vSubCategory
            , string vAgeGroupe, string vSubscriptionType, string vMeetingDate, string vMeetingTitle, string vMeetingUrl, string vMeetingAgenda, string vMeetingDuration, string vThumnailImage, string vAuthorName,string vUpdatedBy)
        {
            clsOnline_DataBaseHelper _objclsOnline_DataBaseHelper = new clsOnline_DataBaseHelper();

            try
            {
                string RowID = string.Empty;
                RowID = "0";
                DataTable DT = new DataTable();
                StoredProc strproc = null;
                string[] vParam = new string[14];

                vParam[0] = "@WebinarId";
                vParam[1] = "@Language";
                vParam[2] = "@Category";
                vParam[3] = "@SubCategory";
                vParam[4] = "@AgeGroupe";
                vParam[5] = "@SubscriptionType";
                vParam[6] = "@MeetingDate";
                vParam[7] = "@MeetingTitle";
                vParam[8] = "@MeetingUrl";
                vParam[9] = "@MeetingDuration";
                vParam[10] = "@MeetingAgenda";
                vParam[11] = "@ImageFile";
                vParam[12] = "@AuthorName";
                vParam[13] = "@UpdatedBy";

                string[] vParamValue = new string[14];
                vParamValue[0] = vWebinarId;
                vParamValue[1] = vLanguage;
                vParamValue[2] = vCategory;
                vParamValue[3] = vSubCategory;
                vParamValue[4] = vAgeGroupe;
                vParamValue[5] = vSubscriptionType;
                vParamValue[6] = vMeetingDate;
                vParamValue[7] = vMeetingTitle;
                vParamValue[8] = vMeetingUrl;
                vParamValue[9] = vMeetingDuration;
                vParamValue[10] = vMeetingAgenda;
                vParamValue[11] = vThumnailImage;
                vParamValue[12] = vAuthorName;
                vParamValue[13] = vUpdatedBy;

                strproc = _objclsOnline_DataBaseHelper.ExecStoredProc("INSERT_WEBINAR_LOG_DETAILS", vParam, vParamValue, "DataTable");

                DT = strproc.DataTableObject;
                if (DT != null)
                    if (DT.Rows.Count > 0)
                        RowID = DT.Rows[0][0].ToString();

                return RowID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objclsOnline_DataBaseHelper = null;
            }

        }

    }
}