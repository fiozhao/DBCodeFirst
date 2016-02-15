using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCodeFirst
{
    public class MSSQLInfo
    {
        SqlConnection conn = new SqlConnection(SqlHelper.connectStr);

        /// <summary>
        /// 数据库所有表名
        /// </summary>
        /// <param name="sFilterTable">过滤名称</param>
        /// <returns></returns>
        public DataTable DBTables(string sFilterTable)
        {
            //string strSql = "SELECT Name as TABLE_NAME, TABLE_COMMENT as comments, NAMEtemp,* FROM " + conn.Database +  "..SysObjects Where XType = 'U' ORDER BY Name";
            //string strSql = "select table_name, TABLE_COMMENT as comments, '' table_camel_name, '' primay_key  from information_schema.tables where table_schema = '" + conn.Database + "'";
            string strSql = "select a.name AS TABLE_NAME,  isnull(g.[value], '') AS comments, '' table_camel_name, '' primay_key from  sys.tables a left join sys.extended_properties g on (a.object_id = g.major_id AND g.minor_id = 0)";
            if (sFilterTable.Trim() != string.Empty)
            {
                strSql = string.Format(strSql, "WHERE LOWER(TABLE_NAME) LIKE '" + sFilterTable.Trim().ToLower() + "%'");
            }
            else
            {
                strSql = string.Format(strSql, "");
            }
            DataTable dt = SqlHelper.GetDataTable(strSql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string TableName = dt.Rows[i]["table_name"].ToString();
                dt.Rows[i]["table_camel_name"] = PublicHelper.GetCamelName(TableName);

                List<string> keyList = Generate.SelectGetPrimayKeys(Enumeration.DataBaseType.MSSQL, TableName); // MSSQL
                if (keyList.Count == 0)
                {
                    dt.Rows[i]["primay_key"] = "N";
                }
                else
                {
                    dt.Rows[i]["primay_key"] = "Y";
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
            //DataTable dtColumns = SqlHelper.GetDataTable("SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH as DATA_LENGTH,"
            //    + "NUMERIC_PRECISION as DATA_PRECISION,NUMERIC_SCALE as DATA_SCALE,"
            //    + "IS_NULLABLE as NULLABLE,COLUMN_DEFAULT as DATA_DEFAULT,COLUMN_COMMENT as COMMENTS,'' Camel_Name "
            //    + "from information_schema.columns "
            //    + "where table_schema='" + conn.Database + "' and table_name='" + tableName + "'");

            string SQL = "SELECT COLUMN_NAME = a.name, "
                + "DATA_TYPE = b.name, "
                + "DATA_LENGTH = a.length, "
                + "DATA_PRECISION = COLUMNPROPERTY(a.id, a.name, 'PRECISION'), "
                + "DATA_SCALE = isnull(COLUMNPROPERTY(a.id, a.name, 'Scale'), 0), "
                + "NULLABLE = case when a.isnullable = 1 then 'Y'else 'N' end, "
                + "DATA_DEFAULT = isnull(e.text, ''), "
                + "COMMENTS = isnull(g.[value], ''), "
                + "'' Camel_Name "
                + "FROM syscolumns a left join systypes b on a.xusertype = b.xusertype "
                + "inner join sysobjects d on a.id = d.id  and d.xtype = 'U' and d.name <> 'dtproperties' "
                + "left join syscomments e on a.cdefault = e.id "
                + "left join sys.extended_properties g on a.id = G.major_id and a.colid = g.minor_id "
                + "left join sys.extended_properties f on d.id = f.major_id and f.minor_id = 0 "
                + "where d.[name] = '" + tableName + "' "
                + "order by a.id,a.colorder ";

            DataTable dtColumns = SqlHelper.GetDataTable(SQL);
            return dtColumns;
        }

        /// <summary>
        /// 获取表的主键字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<string> GetPrimayKeys(string tableName)
        {
            //string sqlstr = "SELECT t.TABLE_NAME, t.CONSTRAINT_TYPE, c.COLUMN_NAME, c.ORDINAL_POSITION FROM "
            //    + "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS t, INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS c "
            //    + "WHERE t.TABLE_NAME = c.TABLE_NAME "
            //    + "AND t.CONSTRAINT_TYPE = 'PRIMARY KEY' AND t.TABLE_SCHEMA = '" + conn.Database
            //    + "' AND c.TABLE_SCHEMA = '" + conn.Database + "' AND t.TABLE_NAME = '" + tableName + "'";

            string sqlstr = "select b.column_name "
                + "from information_schema.table_constraints a "
                + "inner join information_schema.constraint_column_usage b "
                + "on a.constraint_name = b.constraint_name "
                + "where a.constraint_type = 'PRIMARY KEY' and a.table_name = '"+ tableName + "' ";

            DataTable dtPrimaryKeys = SqlHelper.GetDataTable(sqlstr);
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
            //string strSql = "select TABLE_COMMENT from information_schema.tables where table_schema = '" + conn.Database + "' AND TABLE_NAME = '" + tableName + "'";
            string strSql = "SELECT TABLE_COMMENT = isnull(f.value, '') FROM sysobjects d "
                 + "left join sys.extended_properties f on d.id = f.major_id and f.minor_id = 0 "
                 + "where d.name = '" + tableName + "' and d.xtype = 'U' and d.name <> 'dtproperties' ";

            string comments = SqlHelper.ExecuteScalar(strSql) as string;
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

            string sqlstr = "SELECT COLUMN_NAME = a.name, "
                       + "DATA_TYPE = b.name, "
                       + "DATA_LENGTH = a.length, "
                       + "DATA_PRECISION = COLUMNPROPERTY(a.id, a.name, 'PRECISION'), "
                       + "DATA_SCALE = isnull(COLUMNPROPERTY(a.id, a.name, 'Scale'), 0), "
                       + "NULLABLE = case when a.isnullable = 1 then 'Y'else 'N' end, "
                       + "DATA_DEFAULT = isnull(e.text, ''), "
                       + "COMMENTS = isnull(g.[value], '') "
                       + "FROM syscolumns a left join systypes b on a.xusertype = b.xusertype "
                       + "inner join sysobjects d on a.id = d.id  and d.xtype = 'U' and d.name <> 'dtproperties' "
                       + "left join syscomments e on a.cdefault = e.id "
                       + "left join sys.extended_properties g on a.id = G.major_id and a.colid = g.minor_id "
                       + "left join sys.extended_properties f on d.id = f.major_id and f.minor_id = 0 "
                       + "where d.[name] = '" + tableName + "' and a.isnullable = 0 "
                       + "order by a.id,a.colorder ";

            DataTable dtPrimaryKeys1 = SqlHelper.GetDataTable(sqlstr);
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
