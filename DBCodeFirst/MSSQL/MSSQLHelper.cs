using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCodeFirst
{
    public sealed partial class SqlHelper
    {
        /// <summary>
        /// 批量操作每批次记录数
        /// </summary>
        public static int BatchSize = 2000;

        /// <summary>
        /// 超时时间
        /// </summary>
        public static int CommandTimeOut = 600;

        //private string connectionString;

        public SqlHelper()
        {

        }

        /// <summary>
        ///初始化SqlHelper实例
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        //public SqlHelper(string connectionString)
        //{
        //    this.connectStr = connectionString;
        //}

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        //public string connectStr
        //{
        //    get { return System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString; }
        //    set { connectionString = value; }
        //}
        public static string connectStr = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ToString();

        public static DataTable GetDataTable(string sqlStr)
        {
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = SqlHelper.connectStr;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                new SqlDataAdapter
                {
                    SelectCommand = cmd
                }.Fill(dataTable);
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
            return dataTable;
        }


        public static object ExecuteScalar(string sqlStr)
        {
            object result = null;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = SqlHelper.connectStr;
            SqlTransaction tran = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                tran = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;
                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                result = cmd.ExecuteScalar();
                cmd.Dispose();
                tran.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                conn.Close();
                throw ex;
            }
            return result;
        }


        #region 实例方法

        #region ExecuteNonQuery

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonQuery(string commandText, params SqlParameter[] parms)
        {
            return ExecuteNonQuery(connectStr, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteNonQuery(connectStr, commandType, commandText, parms);
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public T ExecuteScalar<T>(string commandText, params SqlParameter[] parms)
        {
            return ExecuteScalar<T>(connectStr, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public object ExecuteScalar(string commandText, params SqlParameter[] parms)
        {
            return ExecuteScalar(connectStr, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public object ExecuteScalar(CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteScalar(connectStr, commandType, commandText, parms);
        }

        #endregion ExecuteScalar

        #region ExecuteDataReader

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        private SqlDataReader ExecuteDataReader(string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataReader(connectStr, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        private SqlDataReader ExecuteDataReader(CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataReader(connectStr, commandType, commandText, parms);
        }
        #endregion

        #region ExecuteDataRow

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行</returns>
        public DataRow ExecuteDataRow(string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataRow(connectStr, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行</returns>
        public DataRow ExecuteDataRow(CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataRow(connectStr, commandType, commandText, parms);
        }

        #endregion ExecuteDataRow

        #region ExecuteDataTable

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public DataTable ExecuteDataTable(string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataTable(connectStr, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(connectStr, commandType, commandText, parms).Tables[0];
        }

        #endregion ExecuteDataTable

        #region ExecuteDataSet

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public DataSet ExecuteDataSet(string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(connectStr, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(connectStr, commandType, commandText, parms);
        }

        #endregion ExecuteDataSet

        #region 批量操作

        /// <summary>
        /// 使用SqlDataAdapter批量更新数据
        /// </summary>
        /// <param name="table">数据表</param>
        public void BatchUpdate(DataTable table)
        {
            BatchUpdate(connectStr, table);
        }

        ///// <summary>
        /////大批量数据插入,返回成功插入行数
        ///// </summary>
        ///// <param name="table">数据表</param>
        ///// <returns>返回成功插入行数</returns>
        //public int BulkInsert(DataTable table)
        //{
        //    return BulkInsert(ConnectionString, table);
        //}

        #endregion 批量操作

        #endregion 实例方法

        #region 静态方法

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] parms)
        {
            if (connection.State != ConnectionState.Open) connection.Open();

            command.Connection = connection;
            command.CommandTimeout = CommandTimeOut;
            // 设置命令文本(存储过程名或SQL语句)
            command.CommandText = commandText;
            // 分配事务
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            // 设置命令类型.
            command.CommandType = commandType;
            if (parms != null && parms.Length > 0)
            {
                //预处理SqlParameter参数数组，将为NULL的参数赋值为DBNull.Value;
                foreach (SqlParameter parameter in parms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                }
                command.Parameters.AddRange(parms);
            }
        }

        #region ExecuteNonQuery

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, string commandText, params SqlParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteNonQuery(connection, CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteNonQuery(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteNonQuery(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteNonQuery(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        private static int ExecuteNonQuery(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            int retval = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return retval;
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static T ExecuteScalar<T>(string connectionString, string commandText, params SqlParameter[] parms)
        {
            object result = ExecuteScalar(connectionString, commandText, parms);
            if (result != null)
            {
                return (T)Convert.ChangeType(result, typeof(T)); ;
            }
            return default(T);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string connectionString, string commandText, params SqlParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteScalar(connection, CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteScalar(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteScalar(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteScalar(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        private static object ExecuteScalar(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            object retval = command.ExecuteScalar();
            command.Parameters.Clear();
            return retval;
        }

        #endregion ExecuteScalar

        #region ExecuteDataReader

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        private static SqlDataReader ExecuteDataReader(string connectionString, string commandText, params SqlParameter[] parms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return ExecuteDataReader(connection, null, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        private static SqlDataReader ExecuteDataReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return ExecuteDataReader(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        private static SqlDataReader ExecuteDataReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataReader(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        private static SqlDataReader ExecuteDataReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataReader(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        private static SqlDataReader ExecuteDataReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        #endregion

        #region ExecuteDataRow

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>,返回结果集中的第一行</returns>
        public static DataRow ExecuteDataRow(string connectionString, string commandText, params SqlParameter[] parms)
        {
            DataTable dt = ExecuteDataTable(connectionString, CommandType.Text, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>,返回结果集中的第一行</returns>
        public static DataRow ExecuteDataRow(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            DataTable dt = ExecuteDataTable(connectionString, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>,返回结果集中的第一行</returns>
        public static DataRow ExecuteDataRow(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            DataTable dt = ExecuteDataTable(connection, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>,返回结果集中的第一行</returns>
        public static DataRow ExecuteDataRow(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            DataTable dt = ExecuteDataTable(transaction, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        #endregion ExecuteDataRow

        #region ExecuteDataTable

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteDataTable(string connectionString, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(connectionString, CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(connectionString, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteDataTable(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(connection, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteDataTable(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(transaction, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="tableName">数据表名称</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteEmptyDataTable(string connectionString, string tableName)
        {
            return ExecuteDataSet(connectionString, CommandType.Text, string.Format("select * from {0} where 1=-1", tableName)).Tables[0];
        }

        #endregion ExecuteDataTable

        #region ExecuteDataSet

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(string connectionString, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(connectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteDataSet(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            return ExecuteDataSet(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        private static DataSet ExecuteDataSet(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms)
        {
            SqlCommand command = new SqlCommand();

            PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (commandText.IndexOf("@") > 0)
            {
                commandText = commandText.ToLower();
                int index = commandText.IndexOf("where ");
                if (index < 0)
                {
                    index = commandText.IndexOf("\nwhere");
                }
                if (index > 0)
                {
                    ds.ExtendedProperties.Add("SQL", commandText.Substring(0, index - 1));  //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
                }
                else
                {
                    ds.ExtendedProperties.Add("SQL", commandText);  //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
                }
            }
            else
            {
                ds.ExtendedProperties.Add("SQL", commandText);  //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
            }

            foreach (DataTable dt in ds.Tables)
            {
                dt.ExtendedProperties.Add("SQL", ds.ExtendedProperties["SQL"]);
            }

            command.Parameters.Clear();
            return ds;
        }

        #endregion ExecuteDataSet

        #region 批量操作

        /// <summary>
        ///使用SqlDataAdapter批量更新数据
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        public static void BatchUpdate(string connectionString, DataTable table)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = connection.CreateCommand();
            command.CommandTimeout = CommandTimeOut;
            command.CommandType = CommandType.Text;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            SqlCommandBuilder commandBulider = new SqlCommandBuilder(adapter);
            commandBulider.ConflictOption = ConflictOption.OverwriteChanges;

            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                //设置批量更新的每次处理条数
                adapter.UpdateBatchSize = BatchSize;
                //设置事物
                adapter.SelectCommand.Transaction = transaction;

                if (table.ExtendedProperties["SQL"] != null)
                {
                    adapter.SelectCommand.CommandText = table.ExtendedProperties["SQL"].ToString();
                }
                adapter.Update(table);
                transaction.Commit();/////提交事务
            }
            catch (SqlException ex)
            {
                if (transaction != null) transaction.Rollback();
                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        ///// <summary>
        /////大批量数据插入,返回成功插入行数
        ///// </summary>
        ///// <param name="connectionString">数据库连接字符串</param>
        ///// <param name="table">数据表</param>
        ///// <returns>返回成功插入行数</returns>
        //public static int BulkInsert(string connectionString, DataTable table)
        //{
        //    if (string.IsNullOrEmpty(table.TableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
        //    if (table.Rows.Count == 0) return 0;
        //    int insertCount = 0;
        //    string tmpPath = Path.GetTempFileName();
        //    string csv = DataTableToCsv(table);
        //    File.WriteAllText(tmpPath, csv);
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        SqlTransaction tran = null;
        //        try
        //        {
        //            conn.Open();
        //            tran = conn.BeginTransaction();
        //            SqlBulkLoader bulk = new SqlBulkLoader(conn)
        //            {
        //                FieldTerminator = ",",
        //                FieldQuotationCharacter = '"',
        //                EscapeCharacter = '"',
        //                LineTerminator = "\r\n",
        //                FileName = tmpPath,
        //                NumberOfLinesToSkip = 0,
        //                TableName = table.TableName,
        //            };
        //            bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToArray());
        //            insertCount = bulk.Load();
        //            tran.Commit();
        //        }
        //        catch (SqlException ex)
        //        {
        //            if (tran != null) tran.Rollback();
        //            throw ex;
        //        }
        //    }
        //    File.Delete(tmpPath);
        //    return insertCount;
        //}

        /// <summary>
        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion 批量操作

        #endregion 静态方法
    }
}
