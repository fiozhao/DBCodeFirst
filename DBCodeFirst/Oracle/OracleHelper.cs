using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;

namespace DBCodeFirst
{
	public class OracleHelper
	{
        public static string connectStr = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;

        public OracleHelper()
        {
            //connectStr = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;
        }

        public OracleHelper(string connectonString)
        {
            connectStr = ConfigurationManager.ConnectionStrings[connectonString].ConnectionString;
        }

        public static DataTable GetDataTable(string sqlStr)
		{
			DataTable dataTable = new DataTable();
			OracleConnection oracleConnection  = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandText = sqlStr;
				oracleCommand.CommandType = CommandType.Text;
				new OracleDataAdapter
				{
					SelectCommand = oracleCommand
				}.Fill(dataTable);
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataTable;
		}

		public static DataTable GetDataTable(string sqlStr, List<OracleParameter> parListOracleParameters)
		{
			DataTable dataTable = new DataTable();
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				new OracleDataAdapter
				{
					SelectCommand = oracleCommand
				}.Fill(dataTable);
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataTable;
		}

		public static DataTable GetDataTable(string sqlStr, List<OracleParameter> parListOracleParameters, CommandType parCommandType)
		{
			DataTable dataTable = new DataTable();
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandType = parCommandType;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				new OracleDataAdapter
				{
					SelectCommand = oracleCommand
				}.Fill(dataTable);
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataTable;
		}

		public static DataSet GetDataSet(string sqlStr)
		{
			DataSet dataSet = new DataSet();
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandText = sqlStr;
				oracleCommand.CommandType = CommandType.Text;
				new OracleDataAdapter
				{
					SelectCommand = oracleCommand
				}.Fill(dataSet);
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataSet;
		}

		public static DataSet GetDataSet(string sqlStr, List<OracleParameter> parListOracleParameters)
		{
			DataSet dataSet = new DataSet();
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				new OracleDataAdapter
				{
					SelectCommand = oracleCommand
				}.Fill(dataSet);
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataSet;
		}

		public static DataSet GetDataSet(string sqlStr, List<OracleParameter> parListOracleParameters, CommandType parCommandType)
		{
			DataSet dataSet = new DataSet();
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandType = parCommandType;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				new OracleDataAdapter
				{
					SelectCommand = oracleCommand
				}.Fill(dataSet);
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataSet;
		}

		public static DataSet GetDataTablesArray(List<string> sqlStrArray)
		{
			DataSet dataSet = new DataSet();
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandType = CommandType.Text;
				OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
				oracleDataAdapter.SelectCommand = oracleCommand;
				for (int i = 0; i < sqlStrArray.Count; i++)
				{
					oracleCommand.CommandText = sqlStrArray[i];
					DataTable dataTable = new DataTable();
					oracleDataAdapter.Fill(dataTable);
					dataTable.TableName = "DataTable" + Convert.ToString(i + 1);
					dataSet.Tables.Add(dataTable);
				}
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataSet;
		}

		public static DataSet GetDataTablesArray(List<string> sqlStrArray, List<List<OracleParameter>> parListParameters)
		{
			DataSet dataSet = new DataSet();
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
				oracleDataAdapter.SelectCommand = oracleCommand;
				for (int i = 0; i < sqlStrArray.Count; i++)
				{
					oracleCommand.CommandText = sqlStrArray[i];
					for (int j = 0; j < parListParameters[i].Count; j++)
					{
						oracleCommand.Parameters.Add(parListParameters[i][j]);
					}
					DataTable dataTable = new DataTable();
					oracleDataAdapter.Fill(dataTable);
					oracleCommand.Parameters.Clear();
					dataTable.TableName = "DataTable" + Convert.ToString(i + 1);
					dataSet.Tables.Add(dataTable);
				}
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataSet;
		}

		public static DataSet GetDataTablesArray(List<string> sqlStrArray, List<List<OracleParameter>> parListParameters, CommandType parCommandType)
		{
			DataSet dataSet = new DataSet();
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandType = parCommandType;
				OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
				oracleDataAdapter.SelectCommand = oracleCommand;
				for (int i = 0; i < sqlStrArray.Count; i++)
				{
					oracleCommand.CommandText = sqlStrArray[i];
					for (int j = 0; j < parListParameters[i].Count; j++)
					{
						oracleCommand.Parameters.Add(parListParameters[i][j]);
					}
					DataTable dataTable = new DataTable();
					oracleDataAdapter.Fill(dataTable);
					oracleCommand.Parameters.Clear();
					dataTable.TableName = "DataTable" + Convert.ToString(i + 1);
					dataSet.Tables.Add(dataTable);
				}
				oracleCommand.Dispose();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleConnection.Close();
				throw ex;
			}
			return dataSet;
		}

		public static int ExecuteSql(string sqlStr)
		{
			int result = 0;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				oracleCommand.CommandText = sqlStr;
				oracleCommand.CommandType = CommandType.Text;
				result = oracleCommand.ExecuteNonQuery();
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return result;
		}

		public static int ExecuteSql(string sqlStr, List<OracleParameter> parListOracleParameters)
		{
			int result = 0;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				result = oracleCommand.ExecuteNonQuery();
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return result;
		}

		public static int ExecuteSql(string sqlStr, List<OracleParameter> parListOracleParameters, CommandType parCommandType)
		{
			int result = 0;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				oracleCommand.CommandType = parCommandType;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				result = oracleCommand.ExecuteNonQuery();
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return result;
		}

		public static int ExecuteSqlArray(List<string> sqlStr)
		{
			int result = 0;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				oracleCommand.CommandType = CommandType.Text;
				for (int i = 0; i < sqlStr.Count; i++)
				{
					oracleCommand.CommandText = sqlStr[i];
					result = oracleCommand.ExecuteNonQuery();
				}
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return result;
		}

		public static int ExecuteSqlArray(List<string> sqlStr, List<List<OracleParameter>> parListParameters)
		{
			int num = 0;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				for (int i = 0; i < sqlStr.Count; i++)
				{
					oracleCommand.CommandText = sqlStr[i];
					for (int j = 0; j < parListParameters[i].Count; j++)
					{
						oracleCommand.Parameters.Add(parListParameters[i][j]);
					}
					int num2 = oracleCommand.ExecuteNonQuery();
					num += num2;
					oracleCommand.Parameters.Clear();
				}
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return num;
		}

		public static int ExecuteSqlArray(List<string> sqlStr, List<List<OracleParameter>> parListParameters, CommandType parCommandType)
		{
			int num = 0;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				oracleCommand.CommandType = parCommandType;
				for (int i = 0; i < sqlStr.Count; i++)
				{
					oracleCommand.CommandText = sqlStr[i];
					for (int j = 0; j < parListParameters[i].Count; j++)
					{
						oracleCommand.Parameters.Add(parListParameters[i][j]);
					}
					int num2 = oracleCommand.ExecuteNonQuery();
					num += num2;
					oracleCommand.Parameters.Clear();
				}
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return num;
		}

		public static object ExecuteScalar(string sqlStr)
		{
			object result = null;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				oracleCommand.CommandText = sqlStr;
				oracleCommand.CommandType = CommandType.Text;
				result = oracleCommand.ExecuteScalar();
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return result;
		}

		public static object ExecuteScalar(string sqlStr, CommandType parCommandType)
		{
			object result = null;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				oracleCommand.CommandText = sqlStr;
				oracleCommand.CommandType = parCommandType;
				result = oracleCommand.ExecuteScalar();
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return result;
		}

		public static object ExecuteScalar(string sqlStr, CommandType parCommandType, List<OracleParameter> parListOracleParameters)
		{
			object result = null;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			OracleTransaction oracleTransaction = null;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				oracleTransaction = oracleConnection.BeginTransaction();
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.Transaction = oracleTransaction;
				oracleCommand.CommandType = parCommandType;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				result = oracleCommand.ExecuteScalar();
				oracleCommand.Dispose();
				oracleTransaction.Commit();
				oracleConnection.Close();
			}
			catch (Exception ex)
			{
				oracleTransaction.Rollback();
				oracleConnection.Close();
				throw ex;
			}
			return result;
		}

		public static OracleDataReader ExecuteReader(string sqlStr)
		{
			OracleDataReader oracleDataReader = null;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandText = sqlStr;
				oracleCommand.CommandType = CommandType.Text;
				oracleDataReader = oracleCommand.ExecuteReader(CommandBehavior.CloseConnection);
				oracleCommand.Dispose();
			}
			catch (Exception ex)
			{
				oracleDataReader.Close();
				oracleConnection.Close();
				throw ex;
			}
			return oracleDataReader;
		}

		public static OracleDataReader ExecuteReader(string sqlStr, List<OracleParameter> parListOracleParameters)
		{
			OracleDataReader oracleDataReader = null;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				oracleDataReader = oracleCommand.ExecuteReader(CommandBehavior.CloseConnection);
				oracleCommand.Dispose();
			}
			catch (Exception ex)
			{
				oracleDataReader.Close();
				oracleConnection.Close();
				throw ex;
			}
			return oracleDataReader;
		}

		public static OracleDataReader ExecuteReader(string sqlStr, List<OracleParameter> parListOracleParameters, CommandType parCommandType)
		{
			OracleDataReader oracleDataReader = null;
			OracleConnection oracleConnection = new OracleConnection();
			oracleConnection.ConnectionString = OracleHelper.connectStr;
			try
			{
				if (oracleConnection.State != ConnectionState.Open)
				{
					oracleConnection.Open();
				}
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandType = parCommandType;
				oracleCommand.CommandText = sqlStr;
				for (int i = 0; i < parListOracleParameters.Count; i++)
				{
					oracleCommand.Parameters.Add(parListOracleParameters[i]);
				}
				oracleDataReader = oracleCommand.ExecuteReader(CommandBehavior.CloseConnection);
				oracleCommand.Dispose();
			}
			catch (Exception ex)
			{
				oracleDataReader.Close();
				oracleConnection.Close();
				throw ex;
			}
			return oracleDataReader;
		}
	}
}
