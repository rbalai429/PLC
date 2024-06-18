using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HPPlc.Models
{
	public class dbProxy
	{
		string conn = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString.ToString();

		public GetStatus StoreData(string procName, List<SetParameters> parameter)
		{
			DynamicParameters dbparameters = new DynamicParameters();
			if (parameter != null && parameter.Count > 0)
			{
				foreach (var param in parameter)
				{
					dbparameters.Add(param.ParameterName, param.Value.ToString());
				}
			}

			GetStatus status = new GetStatus();
			using (var connection = new SqlConnection(conn))
			{
				status = connection.Query<GetStatus>(procName, dbparameters
							, commandType: CommandType.StoredProcedure).SingleOrDefault();

				connection.Close();
			}

			return status;
		}

		public async Task<GetStatus> StoreDataAsync(string procName, List<SetParameters> parameter)
		{
			DynamicParameters dbparameters = new DynamicParameters();
			if (parameter != null && parameter.Count > 0)
			{
				foreach (var param in parameter)
				{
					dbparameters.Add(param.ParameterName, param.Value.ToString());
				}
			}

			GetStatus status = new GetStatus();
			using (var connection = new SqlConnection(conn))
			{
				status = await connection.QueryFirstOrDefaultAsync<GetStatus>(procName, dbparameters
							, commandType: CommandType.StoredProcedure);

				connection.Close();
			}

			return status;
		}
		public T GetData<T>(string procName, T variable, List<SetParameters> parameter)
		{
			DynamicParameters dbparameters = new DynamicParameters();
			if (parameter != null && parameter.Count > 0)
			{
				foreach (var param in parameter)
				{
					dbparameters.Add(param.ParameterName, param.Value.ToString());
				}
			}
			using (var connection = new SqlConnection(conn))
			{
				variable = connection.Query<T>(procName, dbparameters,
							commandType: CommandType.StoredProcedure).SingleOrDefault();

				connection.Close();
			}

			return variable;
		}

		public List<T> GetDataMultiple<T>(string procName, List<T> variable, List<SetParameters> parameter)
		{
			DynamicParameters dbparameters = new DynamicParameters();
			if (parameter != null && parameter.Count > 0)
			{
				foreach (var param in parameter)
				{
					dbparameters.Add(param.ParameterName, param.Value.ToString());
				}
			}
			using (var connection = new SqlConnection(conn))
			{
				variable = connection.Query<T>(procName, dbparameters,
							commandType: CommandType.StoredProcedure).ToList();

				connection.Close();
			}

			return variable;
		}

		public DataTable ExecStoredProc(string sProcName, SqlParameter[] sParam, string ReturnType)
		{
			SqlConnection mobj_SqlConnection;
			SqlCommand mobj_SqlCommand;
			SqlCommand cmd = null;
			SqlDataAdapter da = null;
			DataTable DT = new DataTable();

			mobj_SqlConnection = new SqlConnection(conn);
			mobj_SqlCommand = new SqlCommand();
			mobj_SqlCommand.CommandTimeout = 0x2710;
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

					cmd = new SqlCommand(sProcName, mobj_SqlConnection);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandTimeout = 30000;
					da = new SqlDataAdapter(sProcName, mobj_SqlConnection);
					da.SelectCommand = cmd;
					ReturnType = ReturnType.ToUpper();
					for (int i = 0; i < sParam.Length; i++)
					{
						cmd.Parameters.AddWithValue(sParam[i].ParameterName.ToString(), sParam[i].Value.ToString());
					}
					if (ReturnType.Contains("DATATABLE"))
					{
						da.Fill(DT);
					}
					if (cmd != null)
					{
						cmd.Dispose();
					}
					if (da != null)
					{
						da.Dispose();
					}
					return DT;
				}
				catch (Exception ex)
				{
					//myTransaction.Rollback();
					throw ex;
				}
				finally
				{

					if (DT != null) DT = null;
					if (mobj_SqlConnection.State == ConnectionState.Open)
					{
						mobj_SqlConnection.Dispose();
					}
				}
			}
		}
	}
}