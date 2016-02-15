using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Collections;

namespace DBCodeFirst
{
    /// <summary>
    /// 基于MySQL的数据层基类
    /// </summary>
    /// <remarks>
    /// 参考于MS Petshop 4.0
    /// </remarks>
    public abstract class MySqlHelper
    {

        //public MySqlHelper()
        //{

        //}

        public static string connectStr = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ToString();
        // 用于缓存参数的HASH表
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        public MySqlHelper(string connectonString)
        {
            connectStr = ConfigurationManager.ConnectionStrings[connectonString].ConnectionString;
        }

        public static DataTable GetDataTable(string sqlStr)
        {
            DataTable dataTable = new DataTable();
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = MySqlHelper.connectStr;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                new MySqlDataAdapter
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
        /// <param name="conn">MySqlConnection对象</param>
        /// <param name="trans">MySqlTransaction对象，可为null</param>
        /// <param name="cmd">MySqlCommand对象</param>
        /// <param name="cmdType">CommandType，存储过程或命令行</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">MySqlCommand参数数组，可为null</param>
        private static void PrepareCommand(MySqlConnection conn, MySqlTransaction trans, MySqlCommand cmd, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
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
                foreach (MySqlParameter parm in cmdParms)
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
        /// <param name="cmdParms">MySqlCommand参数数组</param>
        /// <returns>返回受引响的记录行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {

            MySqlCommand cmd = new MySqlCommand();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
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
        /// <param name="cmdParms">MySqlCommand参数数组</param>
        /// <returns>返回受引响的记录行数</returns>
        public static int ExecuteNonQuery(MySqlConnection conn, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();

            PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);

            int val = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            return val;

        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="trans">MySqlTransaction对象</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">MySqlCommand参数数组</param>
        /// <returns>返回受引响的记录行数</returns>
        public static int ExecuteNonQuery(MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)

        {
            MySqlCommand cmd = new MySqlCommand();

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
        /// <param name="cmdParms">MySqlCommand参数数组</param>
        /// <returns>返回Object对象</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
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
        /// <param name="cmdParms">MySqlCommand参数数组</param>
        /// <returns>返回Object对象</returns>
        public static object ExecuteScalar(MySqlConnection conn, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }


        public static object ExecuteScalar(string sqlStr)
        {
            object result = null;
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = MySqlHelper.connectStr;
            MySqlTransaction tran = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                tran = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand();
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
        /// 执行命令或存储过程，返回MySqlDataReader对象
        /// 注意MySqlDataReader对象使用完后必须Close以释放MySqlConnection资源
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="cmdParms">MySqlCommand参数数组</param>
        /// <returns></returns>
        public static MySqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)

        {

            MySqlCommand cmd = new MySqlCommand();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try

            {

                PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);

                MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

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
        /// <param name="cmdParms">MySqlCommand参数数组(可为null值)</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection conn = new MySqlConnection(connectionString))

            {

                PrepareCommand(conn, null, cmd, cmdType, cmdText, cmdParms);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

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
        public static DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            //创建一个MySqlCommand对象
            MySqlCommand cmd = new MySqlCommand();
            //创建一个MySqlConnection对象
            MySqlConnection conn = new MySqlConnection(connectionString);

            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，

            try
            {
                //调用 PrepareCommand 方法，对 MySqlCommand 对象设置参数
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                //调用 MySqlCommand  的 ExecuteReader 方法
                MySqlDataAdapter adapter = new MySqlDataAdapter();
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
        public static void CacheParameters(string cacheKey, params MySqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 找回缓存参数集合
        /// </summary>
        /// <param name="cacheKey">用于找回参数的关键字</param>
        /// <returns>缓存的参数集合</returns>
        public static MySqlParameter[] GetCachedParameters(string cacheKey)
        {
            MySqlParameter[] cachedParms = (MySqlParameter[])parmCache[cacheKey];
            if (cachedParms == null)
                return null;

            MySqlParameter[] clonedParms = new MySqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (MySqlParameter)((ICloneable)cachedParms[i]).Clone();

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
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
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
                foreach (MySqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

    }//end class

} 


