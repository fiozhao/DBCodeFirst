using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Npgsql.Data;
//using Npgsql.Data.NpgsqlClient;

using System.Data;
using System.Configuration;
using System.Collections;
using Npgsql;

namespace DBCodeFirst
{
    /// <summary>
    /// 基于Npgsql的数据层基类
    /// </summary>
    /// <remarks>
    /// 参考于MS Petshop 4.0
    /// </remarks>
    public abstract class PgHelper
    {

        //public NpgsqlHelper()
        //{

        //}

        public static string connectStr = ConfigurationManager.ConnectionStrings["PgConnectionString"].ToString();
        //public static string ConnectionString = ConfigurationManager.ConnectionStrings["PgConnectionString"].ToString();
        // 用于缓存参数的HASH表
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        public PgHelper(string connectonString)
        {
            connectStr = ConfigurationManager.ConnectionStrings[connectonString].ConnectionString;
        }

        public static DataTable GetDataTable(string sqlStr)
        {
            DataTable dataTable = new DataTable();
            NpgsqlConnection conn = new NpgsqlConnection();
            conn.ConnectionString = PgHelper.connectStr;
                        
            //NpgsqlCommand cmd = new NpgsqlCommand();
            //cmd.Connection = conn;
            //cmd.CommandText = sqlStr;
            //cmd.CommandType = CommandType.Text;
            //new NpgsqlDataAdapter
            //{
            //    SelectCommand = cmd
            //}.Fill(dataTable);

            //return dataTable;

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                new NpgsqlDataAdapter
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

        #region PrepareCommand

        /// <summary>
        /// Command预处理
        /// </summary>
        /// <param name="conn">NpgsqlConnection对象</param>
        /// <param name="trans">NpgsqlTransaction对象，可为null</param>
        /// <param name="cmd">NpgsqlCommand对象</param>
        /// <param name="cmdType">CommandType，存储过程或命令行</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">NpgsqlCommand参数数组，可为null</param>
        private static void PrepareCommand(NpgsqlConnection conn, NpgsqlTransaction trans, NpgsqlCommand cmd, CommandType cmdType, string cmdText, NpgsqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (NpgsqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }

        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">NpgsqlCommand参数数组</param>
        /// <returns>返回受引响的记录行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] cmdParms)
        {

            NpgsqlCommand cmd = new NpgsqlCommand();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="conn">Connection对象</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">NpgsqlCommand参数数组</param>
        /// <returns>返回受引响的记录行数</returns>
        public static int ExecuteNonQuery(NpgsqlConnection conn, CommandType cmdType, string cmdText, params NpgsqlParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();

            PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);

            int val = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            return val;

        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="trans">NpgsqlTransaction对象</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">NpgsqlCommand参数数组</param>
        /// <returns>返回受引响的记录行数</returns>
        public static int ExecuteNonQuery(NpgsqlTransaction trans, CommandType cmdType, string cmdText, params NpgsqlParameter[] cmdParms)

        {
            NpgsqlCommand cmd = new NpgsqlCommand();

            PrepareCommand(trans.Connection, trans, cmd, cmdType, cmdText, cmdParms);

            int val = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            return val;

        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行命令，返回第一行第一列的值
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">NpgsqlCommand参数数组</param>
        /// <returns>返回Object对象</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                PrepareCommand(connection, null, cmd, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行命令，返回第一行第一列的值
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">NpgsqlCommand参数数组</param>
        /// <returns>返回Object对象</returns>
        public static object ExecuteScalar(NpgsqlConnection conn, CommandType cmdType, string cmdText, params NpgsqlParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }


        public static object ExecuteScalar(string sqlStr)
        {
            object result = null;
            NpgsqlConnection conn = new NpgsqlConnection();
            conn.ConnectionString = PgHelper.connectStr;
            NpgsqlTransaction tran = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                tran = conn.BeginTransaction();
                NpgsqlCommand cmd = new NpgsqlCommand();
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

        #endregion

        #region ExecuteReader

        /// <summary>
        /// 执行命令或存储过程，返回NpgsqlDataReader对象
        /// 注意NpgsqlDataReader对象使用完后必须Close以释放NpgsqlConnection资源
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">NpgsqlCommand参数数组</param>
        /// <returns></returns>
        public static NpgsqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] cmdParms)

        {

            NpgsqlCommand cmd = new NpgsqlCommand();

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            try

            {

                PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);

                NpgsqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                cmd.Parameters.Clear();

                return dr;

            }

            catch

            {

                conn.Close();

                throw;

            }

        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行命令或存储过程，返回DataSet对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程或SQL语句)</para>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">NpgsqlCommand参数数组(可为null值)</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))

            {

                PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                conn.Close();

                cmd.Parameters.Clear();

                return ds;

            }

        }

        #endregion

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] commandParameters)
        {
            //创建一个NpgsqlCommand对象
            NpgsqlCommand cmd = new NpgsqlCommand();
            //创建一个NpgsqlConnection对象
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，

            try
            {
                //调用 PrepareCommand 方法，对 NpgsqlCommand 对象设置参数
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                //调用 NpgsqlCommand  的 ExecuteReader 方法
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();

                adapter.Fill(ds);
                //清除参数
                cmd.Parameters.Clear();
                conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 将参数集合添加到缓存
        /// </summary>
        /// <param name="cacheKey">添加到缓存的变量</param>
        /// <param name="commandParameters">一个将要添加到缓存的sql参数集合</param>
        public static void CacheParameters(string cacheKey, params NpgsqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 找回缓存参数集合
        /// </summary>
        /// <param name="cacheKey">用于找回参数的关键字</param>
        /// <returns>缓存的参数集合</returns>
        public static NpgsqlParameter[] GetCachedParameters(string cacheKey)
        {
            NpgsqlParameter[] cachedParms = (NpgsqlParameter[])parmCache[cacheKey];
            if (cachedParms == null)
                return null;

            NpgsqlParameter[] clonedParms = new NpgsqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (NpgsqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 准备执行一个命令
        /// </summary>
        /// <param name="cmd">sql命令</param>
        /// <param name="conn">OleDb连接</param>
        /// <param name="trans">OleDb事务</param>
        /// <param name="cmdType">命令类型例如存储过程或者文本</param>
        /// <param name="cmdText">命令文本,例如:Select * from Products</param>
        /// <param name="cmdParms">执行命令的参数</param>
        private static void PrepareCommand(NpgsqlCommand cmd, NpgsqlConnection conn, NpgsqlTransaction trans, CommandType cmdType, string cmdText, NpgsqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (NpgsqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }



        /// <summary>  
        /// 执行SQL语句  
        /// </summary>  
        /// <param name="sql">SQL</param>  
        /// <returns>成功返回大于0的数字</returns>  
        public static int ExecuteSQL(string sql)
        {
            int num2 = -1;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectStr))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        num2 = command.ExecuteNonQuery();
                    }
                    catch (NpgsqlException exception)
                    {
                        throw new Exception(exception.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return num2;
        }

        //带参数的执行查询，不返回结果，返回影响行数
        //执行SQL语句并返回受影响的行数
        public static int ExecuteNonQuery(string sql, params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectStr))
            {
                conn.Open();
                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    //foreach (SqlParameter param in parameters)
                    //{
                    //    cmd.Parameters.Add(param);
                    //}
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        //执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        public static object ExecuteScalar(string sql, params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectStr))
            {
                conn.Open();
                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();

                }
            }
        }




        //查询并返回结果集DataTable,一般只用来执行查询结果比较少的sql。
        public static DataTable ExecuteDataTable(string sql, params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectStr))
            {
                conn.Open();
                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    return dataset.Tables[0];
                }
            }

            //查询较大的数据用 DateRead()，但应尽可能用分页数据，仍然用datatable更好。
        }

    }//end class

}


