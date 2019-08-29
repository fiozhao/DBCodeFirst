using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCodeFirst
{
    public class MySQLInfo
    {
        MySqlConnection conn = new MySqlConnection(MySqlHelper.connectStr);

        /// <summary>
        /// 数据库所有表名
        /// </summary>
        /// <param name="sFilterTable">过滤名称</param>
        /// <returns></returns>
        public DataTable DBTables(string sFilterTable)
        {
            string strSql = "select table_name, TABLE_COMMENT as comments, '' table_camel_name, '' primay_key  from information_schema.tables where table_schema = '" + conn.Database + "'";
            if (sFilterTable.Trim() != string.Empty)
            {
                strSql = string.Format(strSql, "WHERE LOWER(TABLE_NAME) LIKE '" + sFilterTable.Trim().ToLower() + "%'");
            }
            else
            {
                strSql = string.Format(strSql, "");
            }
            DataTable dt = MySqlHelper.GetDataTable(strSql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string TableName = dt.Rows[i]["table_name"].ToString();
                dt.Rows[i]["table_camel_name"] = PublicHelper.GetCamelName(TableName);

                List<string> keyList = Generate.GetPrimayKeys(Enumeration.DataBaseType.MySQL, TableName); //1 MySQL
                if (keyList.Count == 0)
                {
                    //dt.Rows[i]["primay_key"] = "N";
                    dt.Rows[i]["primay_key"] = "";
                }
                else
                {
                    //dt.Rows[i]["primay_key"] = "Y";
                    dt.Rows[i]["primay_key"] = keyList[0].ToString();
                }
            }

            return dt;
        }

        /// <summary>
        /// 根据表名对应的列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable getColumnsByTableName(string tableName)
        {            
            string sqlstr = "SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH as DATA_LENGTH,"
                + "NUMERIC_PRECISION as DATA_PRECISION,NUMERIC_SCALE as DATA_SCALE,"
                + "IS_NULLABLE as NULLABLE,COLUMN_DEFAULT as DATA_DEFAULT,COLUMN_COMMENT as COMMENTS,'' Camel_Name "
                + "from information_schema.columns "
                + "where table_schema='" + conn.Database + "' and table_name='" + tableName + "'";

            DataTable dt = MySqlHelper.GetDataTable(sqlstr);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string columnName = dt.Rows[i]["NULLABLE"].ToString();
                columnName = columnName == tableName ? columnName + "1" : columnName;//如果字段名和表明相同,就在字段名后面加上字符"1"

                if (columnName == "YES")
                {
                    dt.Rows[i]["NULLABLE"] = "Y";
                }
                if (columnName == "NO")
                {
                    dt.Rows[i]["NULLABLE"] = "N";
                }
            }

            return dt;
        }

        /// <summary>
        /// 获取表的主键字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<string> GetPrimayKeys(string tableName)
        {
            string sqlstr = "SELECT t.TABLE_NAME, t.CONSTRAINT_TYPE, c.COLUMN_NAME, c.ORDINAL_POSITION FROM "
                + "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS t, INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS c "
                + "WHERE t.TABLE_NAME = c.TABLE_NAME "
                + "AND t.CONSTRAINT_TYPE = 'PRIMARY KEY' AND t.TABLE_SCHEMA = '" + conn.Database 
                + "' AND c.TABLE_SCHEMA = '" + conn.Database + "' AND t.TABLE_NAME = '" + tableName + "'";
            DataTable dtPrimaryKeys = MySqlHelper.GetDataTable(sqlstr);
            List<string> primayKeys = new List<string>();
            foreach (DataRow item in dtPrimaryKeys.Rows)
            {
                string columnName = item["column_name"].ToString();
                columnName = columnName == tableName ? columnName + "1" : columnName;//如果字段名和表明相同,就在字段名后面加上字符"1"

                if (!primayKeys.Contains(columnName))
                {
                    primayKeys.Add(columnName);
                }
            }
            return primayKeys;
        }

        /// <summary>
        /// 获取表的说明
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string TableComments(string tableName)
        {
            string strSql = "select TABLE_COMMENT from information_schema.tables where table_schema = '" + conn.Database + "' AND TABLE_NAME = '" + tableName + "'";
            string comments = MySqlHelper.ExecuteScalar(strSql) as string;
            return comments == null ? "" : comments;
        }


        /// <summary>
        /// 查找不为空的列
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<string> GetNotNullColumns(string tableName)
        {
            List<string> primayKeys = new List<string>();

            string sqlstr = "SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH as DATA_LENGTH,"
                + "NUMERIC_PRECISION as DATA_PRECISION,NUMERIC_SCALE as DATA_SCALE,"
                + "IS_NULLABLE as NULLABLE,COLUMN_DEFAULT as DATA_DEFAULT,COLUMN_COMMENT as COMMENTS,'' Camel_Name from information_schema.columns "
                + "where table_schema='" + conn.Database + "' and table_name='" + tableName + "' and IS_NULLABLE = 'NO'";

            DataTable dtPrimaryKeys1 = MySqlHelper.GetDataTable(sqlstr);
            foreach (DataRow item in dtPrimaryKeys1.Rows)
            {
                string columnName = item["column_name"].ToString();
                columnName = columnName == tableName ? columnName + "1" : columnName;//如果字段名和表明相同,就在字段名后面加上字符"1"
                primayKeys.Add(columnName);
            }
            return primayKeys;
        }



    }
}
