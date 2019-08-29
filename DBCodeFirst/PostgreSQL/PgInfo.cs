using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCodeFirst
{
    public class PgInfo
    {
        static NameValueCollection appSettings = System.Configuration.ConfigurationManager.AppSettings;
        NpgsqlConnection conn = new NpgsqlConnection(PgHelper.connectStr);
        String dbSchema = appSettings["dbSchema"];

        /// <summary>
        /// 数据库所有表名
        /// </summary>
        /// <param name="sFilterTable">过滤名称</param>
        /// <returns></returns>
        public DataTable DBTables(string sFilterTable)
        {
            string strSql = "select relname as table_name" 
                + ", cast(obj_description(relfilenode, 'pg_class') as varchar) as comments ,"
                + "'' table_camel_name, '' primay_key"
                + " from pg_class c where relnamespace IN(SELECT oid FROM pg_namespace"
                + " WHERE nspname = '" + dbSchema + "') and reltype > 0 and relname not like '%_copy%' order by table_name";
            if (sFilterTable.Trim() != string.Empty)
            {
                strSql = string.Format(strSql, "and LOWER(relname) like '" + sFilterTable.Trim().ToLower() + "%'");
            }
            else
            {
                strSql = string.Format(strSql, "");
            }
            //strSql = "SELECT * FROM \"ClientArticleConfig\"";
            DataTable dt = PgHelper.GetDataTable(strSql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string TableName = dt.Rows[i]["table_name"].ToString();
                dt.Rows[i]["table_camel_name"] = PublicHelper.GetCamelName(TableName);

                List<string> keyList = Generate.GetPrimayKeys(Enumeration.DataBaseType.PostgreSQL, TableName); //1 PostgreSQL
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
            string sqlstr = "SELECT t1.COLUMN_NAME,t1.DATA_TYPE,t1.CHARACTER_MAXIMUM_LENGTH as DATA_LENGTH,"
                + " t1.NUMERIC_PRECISION as DATA_PRECISION,t1.NUMERIC_SCALE as DATA_SCALE,"
                + " t1.IS_NULLABLE as NULLABLE,t1.COLUMN_DEFAULT as DATA_DEFAULT,t2.description as COMMENTS,'' Camel_Name"
                + " from information_schema.columns as t1"
                + " left join"
                + " (select a.attnum, a.attname, concat_ws('',t.typname,SUBSTRING(format_type(a.atttypid, a.atttypmod) from '\\(.*\\)')) as type,d.description"
                + "   from pg_class c, pg_attribute a,	pg_type t, pg_description d"
                + " where c.relname = '" + tableName + "' and relnamespace IN ( SELECT oid FROM pg_namespace WHERE nspname='" + dbSchema + "') and a.attnum > 0 and a.attrelid = c.oid and a.atttypid = t.oid and d.objoid = a.attrelid and d.objsubid = a.attnum"
                + " ) as t2 on t1.COLUMN_NAME = t2.attname"
                + " where t1.table_schema = '" + dbSchema + "' and t1.table_name = '" + tableName + "' AND t1.table_catalog = '" + conn.Database + "'";

            DataTable dt = PgHelper.GetDataTable(sqlstr);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string columnName = dt.Rows[i]["NULLABLE"].ToString();
                //如果字段名和表名相同,就在字段名后面加上字符"1"
                columnName = columnName == tableName ? columnName + "1" : columnName;

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
            string sqlstr = "SELECT t.TABLE_NAME, t.CONSTRAINT_TYPE, c.COLUMN_NAME, c.ORDINAL_POSITION"
                + " FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS t, INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS c "
                + "WHERE t.TABLE_NAME = c.TABLE_NAME AND t.CONSTRAINT_TYPE = 'PRIMARY KEY' AND c.TABLE_SCHEMA = '" + dbSchema + "' AND t.TABLE_NAME = '" + tableName
                + "' AND t.table_catalog = '" + conn.Database + "'";

            DataTable dtPrimaryKeys = PgHelper.GetDataTable(sqlstr);
            List<string> primayKeys = new List<string>();
            foreach (DataRow item in dtPrimaryKeys.Rows)
            {
                string columnName = item["column_name"].ToString();
                //如果字段名和表名相同,就在字段名后面加上字符"1"
                columnName = columnName == tableName ? columnName + "1" : columnName;

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
            //string strSql = "select TABLE_COMMENT from information_schema.tables where table_schema = '" + conn.Database + "' AND TABLE_NAME = '" + tableName + "'";
  
            string strSql = "select cast(obj_description(relfilenode, 'pg_class') as varchar) as TABLE_COMMENT"
                + " from pg_class"
                + " where relname in (select tablename from pg_tables where schemaname = '" + dbSchema + "' and position('_2' in tablename)= 0) and relname = '" + tableName + "';";

            string comments = PgHelper.ExecuteScalar(strSql) as string;
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

            DataTable dtPrimaryKeys1 = PgHelper.GetDataTable(sqlstr);
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
