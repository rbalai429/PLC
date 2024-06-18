using HPPlc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HPPlc.Model
{
    public class clsWorksheet
    {
        public string Insert_WorkSheet_Download_Print(string CultureInfo, string RefUserId, string Age, string WorkSheetId, string WorkshhetPDFUrl, string vFrom)
        {
            clsOnline_DataBaseHelper _objclsOnline_DataBaseHelper = new clsOnline_DataBaseHelper();

            try
            {
                string RowID = string.Empty;
                RowID = "0";
                DataTable DT = new DataTable();
                StoredProc strproc = null;
                string[] vParam = new string[6];
                vParam[0] = "@CultureInfo";
                vParam[1] = "@RefUserId";
                vParam[2] = "@Age";
                vParam[3] = "@WorkSheetId";
                vParam[4] = "@WorkshhetPDFUrl";
                vParam[5] = "@FromDestination";

                string[] vParamValue = new string[6];
                vParamValue[0] = CultureInfo;
                vParamValue[1] = RefUserId;
                vParamValue[2] = Age;
                vParamValue[3] = WorkSheetId;
                vParamValue[4] = WorkshhetPDFUrl;
                vParamValue[5] = vFrom;

                strproc = _objclsOnline_DataBaseHelper.ExecStoredProc("INSERT_DOWNLOAD_PRINT_USERDATA", vParam, vParamValue, "DataTable");

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
        public DataTable GET_EXPERT_TALK_DATA(string CultureInfo)
        {
            clsOnline_DataBaseHelper _objclsOnline_DataBaseHelper = new clsOnline_DataBaseHelper();

            try
            {
                DataTable DT = new DataTable();
                StoredProc strproc = null;
                string[] vParam = new string[1];
                vParam[0] = "@CultureInfo";

                string[] vParamValue = new string[1];
                vParamValue[0] = CultureInfo;

                strproc = _objclsOnline_DataBaseHelper.ExecStoredProc("Get_ExpertTalkContent", vParam, vParamValue, "DataTable");

                DT = strproc.DataTableObject;
                return DT;
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