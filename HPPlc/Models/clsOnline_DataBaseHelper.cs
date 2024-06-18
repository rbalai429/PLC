using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace HPPlc.Models
{

    public class clsOnline_DataBaseHelper
    {

        private byte[] key = { };
        private byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };

        private int mint_CommandTimeout = 30000;
        public enum ExpectedType
        {

            StringType = 0,
            NumberType = 1,
            DateType = 2,
            BooleanType = 3,
            ImageType = 4
        }
        public clsOnline_DataBaseHelper()
        {
            try
            {
               
            }
            catch
            {
                //throw new Exception("Error initializing data class." + Environment.NewLine + ex.Message);
                throw;
            }
        }

       

        public StoredProc ExecStoredProc(string sProcName, string ReturnType)
        {
            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            StoredProc sp = null;
            StoredProc objSP = null;
            DataTable DT = null;
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;
            
            using (mobj_SqlConnection)
            {
                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    sp = new StoredProc();
                    objSP = new StoredProc();
                    cmd = new SqlCommand(sProcName, mobj_SqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30000;
                    da = new SqlDataAdapter(sProcName, mobj_SqlConnection);
                    da.SelectCommand = cmd;

                    ReturnType = ReturnType.ToUpper();


                    if (ReturnType.Contains("DATAREADER"))
                    {
                        
                        sp.DataReaderObject = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        

                    }
                    else if (ReturnType.Contains("DATATABLE"))
                    {

                        da.Fill(sp.DataTableObject);

                        sp.DataTableObject = GenericReplace(sp.DataTableObject);

                    }
                    else if (ReturnType.Contains("DATASET"))
                    {
                        da.Fill(sp.DataSetObject);

                        for (int j = 0; j < sp.DataSetObject.Tables.Count; j++)
                        {

                            DT = new DataTable();

                            DT = sp.DataSetObject.Tables[j].Copy();

                            DT = GenericReplace(DT);

                            objSP.DataSetObject.Tables.Add(DT);

                        }

                        sp = objSP;
                    }
                    else if (ReturnType.Contains("SINGLERECORD"))
                    {
                        
                        sp.StringObject = cmd.ExecuteNonQuery().ToString();
                        

                    }
                    //Clean Up Command Object 
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }

                    //Clean Up DataAdapter Object 
                    if (da != null)
                    {
                        da.Dispose();
                    }
                    return sp;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (sp != null) sp = null;
                    if (objSP != null) objSP = null;
                    if (DT != null) DT = null;
                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }
        }


        public StoredProc ExecStoredProcTransaction(string sProcName, string[] sParamName, string[] sParamValue, string ReturnType)
        {
            
            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            StoredProc sp = null;
            StoredProc objSP = null;
            DataTable DT = null;
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;

            using (mobj_SqlConnection)
            {

                if (mobj_SqlConnection.State == ConnectionState.Closed)
                {
                    mobj_SqlConnection.Open();
                }

                SqlTransaction myTran = mobj_SqlConnection.BeginTransaction();

                try
                {
                    sp = new StoredProc();
                    objSP = new StoredProc();
                    cmd = new SqlCommand(sProcName, mobj_SqlConnection, myTran);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30000;
                    da = new SqlDataAdapter(sProcName, mobj_SqlConnection);
                    da.SelectCommand = cmd;

                    int i;

                    ReturnType = ReturnType.ToUpper();


                    for (i = 0; i < sParamName.Length; i++)
                    {
                        cmd.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                        //da.SelectCommand.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                    }
                    if (ReturnType.Contains("DATAREADER"))
                    {
                        //OpenConnection();
                        sp.DataReaderObject = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        //CloseConnection();//Manoj

                    }
                    else if (ReturnType.Contains("DATATABLE"))
                    {
                        da.Fill(sp.DataTableObject);

                        sp.DataTableObject = GenericReplace(sp.DataTableObject);
                    }
                    else if (ReturnType.Contains("DATASET"))
                    {
                        da.Fill(sp.DataSetObject);

                        for (int j = 0; j < sp.DataSetObject.Tables.Count; j++)
                        {
                            DT = new DataTable();

                            DT = sp.DataSetObject.Tables[j].Copy();

                            DT = GenericReplace(DT);

                            objSP.DataSetObject.Tables.Add(DT);

                        }

                        sp = objSP;
                    }
                    else if (ReturnType.Contains("SINGLERECORD"))
                    {
                        //OpenConnection();
                        sp.StringObject = cmd.ExecuteNonQuery().ToString();
                        //CloseConnection();//Manoj
                    }
                    //myTransaction.Commit();
                    myTran.Commit();
                    //CloseConnection();

                    //Clean Up Command Object 
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }

                    //Clean Up DataAdapter Object 
                    if (da != null)
                    {
                        da.Dispose();
                    }

                    return sp;
                }
                catch
                {
                    //myTransaction.
                    myTran.Rollback();
                    throw;
                }
                finally
                {
                    if (sp != null) sp = null;
                    if (objSP != null) objSP = null;
                    if (DT != null) DT = null;

                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }
        }


        public StoredProc ExecStoredProc(string sProcName, string[] sParamName, string[] sParamValue, string ReturnType)
        {
            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            StoredProc sp = null;
            StoredProc objSP = null;
            DataTable DT = null;
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;

            using (mobj_SqlConnection)
            {
                //SqlTransaction myTransaction = mobj_SqlConnection.BeginTransaction(); 
                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }


                    sp = new StoredProc();
                    objSP = new StoredProc();
                    cmd = new SqlCommand(sProcName, mobj_SqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000000;
                    da = new SqlDataAdapter(sProcName, mobj_SqlConnection);
                   
                    da.SelectCommand = cmd;
                    int i;

                    ReturnType = ReturnType.ToUpper();

                    for (i = 0; i < sParamName.Length; i++)
                    {

                        cmd.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                        //da.SelectCommand.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                    }
                    if (ReturnType.Contains("DATAREADER"))
                    {
                        //OpenConnection();//Manoj
                        sp.DataReaderObject = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        //CloseConnection();//Manoj

                    }
                    else if (ReturnType.Contains("DATATABLE"))
                    {
                        da.Fill(sp.DataTableObject);

                        sp.DataTableObject = GenericReplace(sp.DataTableObject);
                    }
                    else if (ReturnType.Contains("DATASET"))
                    {
                        da.Fill(sp.DataSetObject);



                        for (int j = 0; j < sp.DataSetObject.Tables.Count; j++)
                        {
                            DT = new DataTable();

                            DT = sp.DataSetObject.Tables[j].Copy();

                            DT = GenericReplace(DT);

                            objSP.DataSetObject.Tables.Add(DT);

                        }

                        sp = objSP;
                    }
                    else if (ReturnType.Contains("SINGLERECORD"))
                    {
                        //OpenConnection();//Manoj
                        sp.StringObject = cmd.ExecuteNonQuery().ToString();
                        //CloseConnection();//Manoj

                    }
                    //myTransaction.Commit();
                    //Clean Up Command Object 
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }

                    //Clean Up DataAdapter Object 
                    if (da != null)
                    {
                        da.Dispose();
                    }

                    return sp;
                }
                catch
                {
                    //myTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (sp != null) sp = null;
                    if (objSP != null) objSP = null;
                    if (DT != null) DT = null;

                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }
        }

        public StoredProc ExecStoredProc(string sProcName, string[] sParamName, string[] sParamValue, string ReturnType, SqlConnection cnn, SqlTransaction sqlTrans)
        {
            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            StoredProc sp = null;
            StoredProc objSP = null;
            DataTable DT = null;
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;
            using (mobj_SqlConnection)
            {
                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    sp = new StoredProc();
                    objSP = new StoredProc();
                    cmd = new SqlCommand(sProcName, cnn, sqlTrans);//Manoj
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30000;
                    da = new SqlDataAdapter(sProcName, cnn);
                    da.SelectCommand = cmd;
                    int i;

                    ReturnType = ReturnType.ToUpper();

                    for (i = 0; i < sParamName.Length; i++)
                    {
                        cmd.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                        //da.SelectCommand.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                    }
                    if (ReturnType.Contains("DATAREADER"))
                    {
                        //Manoj
                        sp.DataReaderObject = cmd.ExecuteReader();
                        //CloseConnection();//Manoj
                    }
                    else if (ReturnType.Contains("DATATABLE"))
                    {
                        da.Fill(sp.DataTableObject);

                        sp.DataTableObject = GenericReplace(sp.DataTableObject);

                    }
                    else if (ReturnType.Contains("DATASET"))
                    {
                        da.Fill(sp.DataSetObject);

                        objSP = new StoredProc();

                        for (int j = 0; j < sp.DataSetObject.Tables.Count; j++)
                        {
                            DT = new DataTable();

                            DT = sp.DataSetObject.Tables[j].Copy();

                            DT = GenericReplace(DT);

                            objSP.DataSetObject.Tables.Add(DT);

                        }

                        sp = objSP;

                    }
                    else if (ReturnType.Contains("SINGLERECORD"))
                    {
                        //OpenConnection();//Manoj
                        sp.StringObject = cmd.ExecuteNonQuery().ToString();
                        //CloseConnection();//Manoj
                    }

                    //Clean Up Command Object 
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }

                    //Clean Up DataAdapter Object 
                    if (da != null)
                    {
                        da.Dispose();
                    }

                    return sp;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (sp != null) sp = null;
                    if (objSP != null) objSP = null;
                    if (DT != null) DT = null;

                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                    cnn.Close();
                }
            }
        }

        public string ExecStoredProc(string sProcName, string[] sParamName, string[] sParamValue, string OutputParam_DataType, int size)
        {

            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            StoredProc sp = null;
            StoredProc objSP = null;
            DataTable DT = null;
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;

            using (mobj_SqlConnection)
            {
                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    sp = new StoredProc();
                    objSP = new StoredProc();
                    cmd = new SqlCommand(sProcName, mobj_SqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30000;
                    da = new SqlDataAdapter(sProcName, mobj_SqlConnection);
                    da.SelectCommand = cmd;
                    int i;

                    string result;

                    int len = 0;

                    len = sParamName.Length - 1;

                    for (i = 0; i < len; i++)
                    {
                        cmd.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                        //da.SelectCommand.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                    }

                    if (OutputParam_DataType.ToUpper() == "NVARCHAR")
                    {
                        cmd.Parameters.Add(sParamName[i].ToString(), SqlDbType.NVarChar, size).Direction = ParameterDirection.Output;
                    }
                    else if (OutputParam_DataType.ToUpper() == "VARCHAR")
                    {
                        cmd.Parameters.Add(sParamName[i].ToString(), SqlDbType.VarChar, size).Direction = ParameterDirection.Output;
                    }
                    else if (OutputParam_DataType.ToUpper() == "INT")
                    {
                        cmd.Parameters.Add(sParamName[i].ToString(), SqlDbType.Int).Direction = ParameterDirection.Output;
                    }

                    //OpenConnection();

                    cmd.ExecuteNonQuery().ToString();

                    result = (cmd.Parameters[sParamName[i].ToString()].Value.ToString());

                    //CloseConnection();//Manoj

                    //Clean Up Command Object 
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }

                    //Clean Up DataAdapter Object 
                    if (da != null)
                    {
                        da.Dispose();
                    }

                    return result;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (sp != null) sp = null;

                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }
        }

        public StoredProc ExecStoredProc(string sProcName, string[] sParamName, string[] sParamValue, string OutputParam_DataType, int size, string ReturnType)
        {

            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            StoredProc sp = null;
            StoredProc objSP = null;
            DataTable DT = null;
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;

            using (mobj_SqlConnection)
            {

                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    sp = new StoredProc();
                    objSP = new StoredProc();
                    cmd = new SqlCommand(sProcName, mobj_SqlConnection); //Manoj
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30000;
                    da = new SqlDataAdapter(sProcName, mobj_SqlConnection);
                    da.SelectCommand = cmd;
                    int i;

                    string result;

                    int len = 0;

                    len = sParamName.Length - 1;

                    for (i = 0; i < len; i++)
                    {
                        cmd.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                        //da.SelectCommand.Parameters.AddWithValue(sParamName[i].ToString(), sParamValue[i].ToString());
                    }

                    if (OutputParam_DataType.ToUpper() == "NVARCHAR")
                    {
                        cmd.Parameters.Add(sParamName[i].ToString(), SqlDbType.NVarChar, size).Direction = ParameterDirection.Output;
                    }
                    else if (OutputParam_DataType.ToUpper() == "VARCHAR")
                    {
                        cmd.Parameters.Add(sParamName[i].ToString(), SqlDbType.VarChar, size).Direction = ParameterDirection.Output;
                    }
                    else if (OutputParam_DataType.ToUpper() == "INT")
                    {
                        cmd.Parameters.Add(sParamName[i].ToString(), SqlDbType.Int).Direction = ParameterDirection.Output;
                    }

                    //cmd.ExecuteNonQuery().ToString();



                    if (ReturnType.Contains("DATAREADER"))
                    {
                        //OpenConnection();
                        sp.DataReaderObject = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        //CloseConnection();

                    }
                    else if (ReturnType.Contains("DATATABLE"))
                    {
                        da.Fill(sp.DataTableObject);
                        sp.DataTableObject = GenericReplace(sp.DataTableObject);

                    }
                    else if (ReturnType.Contains("DATASET"))
                    {
                        da.Fill(sp.DataSetObject);

                        objSP = new StoredProc();

                        for (int j = 0; j < sp.DataSetObject.Tables.Count; j++)
                        {
                            DT = new DataTable();

                            DT = sp.DataSetObject.Tables[j].Copy();

                            DT = GenericReplace(DT);

                            objSP.DataSetObject.Tables.Add(DT);

                        }

                        sp = objSP;

                    }
                    else if (ReturnType.Contains("SINGLERECORD"))
                    {
                        //OpenConnection();
                        sp.StringObject = cmd.ExecuteNonQuery().ToString();
                        //CloseConnection();//Manoj

                    }

                    result = (cmd.Parameters[sParamName[i].ToString()].Value.ToString());

                    if (result != "TRUE")
                    {
                        throw new Exception(result);
                    }


                    //Clean Up Command Object 
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }

                    //Clean Up DataAdapter Object 
                    if (da != null)
                    {
                        da.Dispose();
                    }


                    return sp;

                    //return result;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (sp != null) sp = null;
                    if (objSP != null) objSP = null;
                    if (DT != null) DT = null;
                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }
        }

        public int GetExecuteScalarByCommand(string Command)
        {
            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
          
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;

            using (mobj_SqlConnection)
            {
                object identity = 0;
                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    mobj_SqlCommand.CommandText = Command;
                    mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                    mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
                    mobj_SqlCommand.Connection = mobj_SqlConnection;
                    identity = mobj_SqlCommand.ExecuteScalar();
                    mobj_SqlConnection.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
                return Convert.ToInt32(identity);
            }
        }

        public void GetExecuteNonQueryByCommand(string Command)
        {
            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
          
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;

            using (mobj_SqlConnection)
            {
                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    mobj_SqlCommand.CommandText = Command;
                    mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                    mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                    mobj_SqlCommand.Connection = mobj_SqlConnection;
                    mobj_SqlCommand.ExecuteNonQuery();
                    mobj_SqlConnection.Close();


                }
                catch
                {

                    throw;
                }
                finally
                {
                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }
        }
        public DataSet GetDatasetByCommand(string Command)
        {
            SqlDataAdapter adpt = null;
            DataSet ds = null;
            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;
          
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;

            using (mobj_SqlConnection)
            {

                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    mobj_SqlCommand.CommandText = Command;
                    mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                    mobj_SqlCommand.CommandType = CommandType.StoredProcedure;


                    adpt = new SqlDataAdapter(mobj_SqlCommand);
                    ds = new DataSet();
                    adpt.Fill(ds);

                    //Clean Up DataAdapter Object 
                    if (adpt != null)
                    {
                        adpt.Dispose();
                    }


                    return ds;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (ds != null) ds = null;
                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }
        }
        public SqlDataReader GetReaderBySQL(string strSQL)
        {
            SqlCommand myCommand = null;

            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;

            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;
            using (mobj_SqlConnection)
            {

                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    myCommand = new SqlCommand(strSQL, mobj_SqlConnection);
                    mobj_SqlConnection.Close();
                    return myCommand.ExecuteReader();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }
        }
        public SqlDataReader GetReaderByCmd(string Command)
        {
            SqlDataReader objSqlDataReader = null;
            string mstr_ConnectionString;
            SqlConnection mobj_SqlConnection;
            SqlCommand mobj_SqlCommand;

            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
            mobj_SqlCommand = new SqlCommand();
            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
            mobj_SqlCommand.Connection = mobj_SqlConnection;
            using (mobj_SqlConnection)
            {
                try
                {
                    if (mobj_SqlConnection.State == ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Open();
                    }
                    mobj_SqlCommand.CommandText = Command;
                    mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
                    mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;

                    mobj_SqlCommand.Connection = mobj_SqlConnection;

                    objSqlDataReader = mobj_SqlCommand.ExecuteReader();
                    mobj_SqlConnection.Close();
                    return objSqlDataReader;
                }
                catch
                {

                    throw;
                }
                finally
                {
                    if (mobj_SqlConnection.State == ConnectionState.Open)
                    {
                        mobj_SqlConnection.Close();
                    }
                }
            }

        }

        //public void AddParameterToSQLCommand(string ParameterName, SqlDbType ParameterType)
        //{
        //    try
        //    {
        //        mobj_SqlCommand.Parameters.Add(new SqlParameter(ParameterName, ParameterType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public void AddParameterToSQLCommand(string ParameterName, SqlDbType ParameterType, int ParameterSize)
        //{
        //    try
        //    {
        //        mobj_SqlCommand.Parameters.Add(new SqlParameter(ParameterName, ParameterType, ParameterSize));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public void SetSQLCommandParameterValue(string ParameterName, object Value)
        //{
        //    try
        //    {
        //        mobj_SqlCommand.Parameters[ParameterName].Value = Value;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public void SQLBulkCopy(DataTable DT, string DestinationTableName, string[] SourceFields, string[] DestinatioFields)
        {
            SqlBulkCopy bulkCopy = null;
            
            string mstr_ConnectionString;
            mstr_ConnectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ToString();
            try
            {
                bulkCopy = new SqlBulkCopy(mstr_ConnectionString);

                bulkCopy.BulkCopyTimeout = 30000;
                for (int i = 0, j = 0; i < SourceFields.Length && j < DestinatioFields.Length; i++, j++)
                {
                    bulkCopy.ColumnMappings.Add(SourceFields[i], DestinatioFields[j]);
                }

                bulkCopy.DestinationTableName = DestinationTableName;
                bulkCopy.WriteToServer(DT);
            }
            catch
            {
                throw;
            }
        }

        protected DataTable GenericReplace(DataTable DT)
        {
            try
            {
                int vMaxRow = DT.Rows.Count;

                int vMaxColumn = DT.Columns.Count;

                for (int i = 0; i < vMaxRow; i++)
                {
                    for (int j = 0; j < vMaxColumn; j++)
                    {
                        string DataType = DT.Rows[i][j].GetType().ToString();

                        if (DataType.ToUpper() == "SYSTEM.STRING")
                        {
                            if (DT.Columns[j].ReadOnly == false)
                            {
                                if (DT.Columns[j].ColumnName.ToUpper().Contains("MOBILEEEEE") || DT.Columns[j].ColumnName.ToUpper().Contains("EMAILLLLL"))
                                {
                                    if (DT.Rows[i][j].ToString() != null && DT.Rows[i][j].ToString()!="")  
                                       DT.Rows[i][j] = Decrypt(ReverseConversion(DT.Rows[i][j].ToString()));
                                }
                                else
                                {
                                     if (DT.Rows[i][j].ToString() != null && DT.Rows[i][j].ToString() != "")  
                                      DT.Rows[i][j] = ReverseConversion(DT.Rows[i][j].ToString());
                                }
                            }

                        }
                    }
                }
                return DT;
            }
            catch
            {
                throw;
            }

        }

        public string ReverseConversion(string vSourceData)
        {
            //clsOnline_DataBaseHelper objDB = new clsOnline_DataBaseHelper();
            string vReversedData = string.Empty;

            try
            {
                vReversedData = ReverseConversion(vSourceData, true, false);
            }
            catch
            {
                throw;

            }
            return vReversedData;
        }

        public string ReverseConversion(string vSourceData, bool vHTMLAllowed, bool vScriptTagAllowed)
        {
            string vSRC = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(vSourceData))
                {

                    vSRC = vSourceData;
                    // vSRC = SourceData.Trim();
                    // Replace single quote 
                    vSRC = vSRC.Replace("&#59;", ";");            // This should be called first, before every replace.

                    vSRC = vSRC.Replace("''", "'");
                    vSRC = vSRC.Replace("&lt;", "<");
                    vSRC = vSRC.Replace("&gt;", ">");

                    vSRC = vSRC.Replace("&#173;", "--");
                    //

                    //SQL Related SQL Injection Cleaned
                    vSRC = Regex.Replace(vSRC, "&#101;xec", "exec", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#120;p_cmdshell", "xp_cmdshell", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#115;elect", "select", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#105;nsert", "insert", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#117;pdate", "update", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#100;elete", "delete", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#100;rop", "drop", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#116;runcate", "truncate", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#99;reate", "create", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#114;ename", "rename", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#97;lter", "alter", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#101;xists", "exists", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#114;estore", "restore", RegexOptions.IgnoreCase);
                    vSRC = Regex.Replace(vSRC, "&#115;p_", "sp_", RegexOptions.IgnoreCase);
                    //END

                    if (vHTMLAllowed == false)
                    {
                        vSRC = HttpContext.Current.Server.UrlEncode(vSRC);
                    }
                    else
                    {
                        if (vScriptTagAllowed == false)
                        {
                            // Replace SCRIPT Tag 
                            // Modified On : Dated 03_July_2011
                            // Description : Here we are replacing the  angle bracket (<) to &lt, so that while rendering on html
                            // page it is  treated like a normal text instead of HTML tag. 

                            vSRC = vSRC.Replace("<script", "&lt;script");
                            vSRC = vSRC.Replace("</script", "&lt;/script");

                            vSRC = vSRC.Replace("&lt;input", "<input");
                            vSRC = vSRC.Replace("&lt;/input", "</input");

                            vSRC = vSRC.Replace("&lt;form", "<form");
                            vSRC = vSRC.Replace("&lt;/form", "</form");

                            vSRC = vSRC.Replace("&lt;embed", "<embed");
                            vSRC = vSRC.Replace("&lt;/embed", "</embed");


                            vSRC = vSRC.Replace("&lt;textarea", "<textarea");
                            vSRC = vSRC.Replace("&lt;/textarea", "</textarea");

                            vSRC = vSRC.Replace("&lt;select", "<select");
                            vSRC = vSRC.Replace("&lt;/select", "</select");

                            vSRC = vSRC.Replace("&lt;img", "<img");
                            vSRC = vSRC.Replace("&lt;/img", "</img");

                        }
                    }
                }

            }
            catch
            {
                throw;
            }
            return vSRC.Trim();
        }

        /// <summary>
        /// To Decrypt string
        /// </summary>
        /// <param name="stringToDecrypt"></param>
        /// <param name="sEncryptionKey"></param>
        /// <returns>string</returns>
        /// <remarks></remarks>
        public string Decrypt(string stringToDecrypt)
        {
            stringToDecrypt = stringToDecrypt.Replace(" ", "+");
            string sEncryptionKey = "!*&@9876543210";
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];

            MemoryStream ms = null;
            Encoding encoding = null;
            try
            {
                ms = new MemoryStream();
                
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                //if (!stringToDecrypt.Equals(""))
                //{
                    cs.FlushFinalBlock();
                //}
                encoding = System.Text.Encoding.UTF8;
            }
            catch
            {
                throw;
            }

            return encoding.GetString(ms.ToArray());
        }

    }

    public class StoredProc
    {
        public SqlDataReader DataReaderObject = null;
        public DataTable DataTableObject = null;
        public DataSet DataSetObject = null;
        public string StringObject = string.Empty;

        public StoredProc()
        {
            DataReaderObject = null;
            DataTableObject = new DataTable();
            DataSetObject = new DataSet();
            StringObject = string.Empty;
        }
    }
}
