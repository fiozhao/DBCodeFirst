using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DBCodeFirst
{
    public class OracleInfo
    {

        /// <summary>
        /// 数据库所有表名
        /// </summary>
        /// <param name="sFilterTable">过滤名称</param>
        /// <returns></returns>
        public DataTable DBTables(string sFilterTable)
        {
            string strSql = "SELECT ut.table_name, utc.comments, '' table_camel_name, '' primay_key FROM user_tables ut INNER JOIN user_tab_comments utc ON ut.table_name=utc.table_name {0} ORDER BY ut.table_name";
            if (sFilterTable.Trim() != string.Empty)
            {
                strSql = string.Format(strSql, "WHERE LOWER(ut.table_name) LIKE '" + sFilterTable.Trim().ToLower() + "%'");
            }
            else
            {
                strSql = string.Format(strSql, "");
            }
            DataTable dt = OracleHelper.GetDataTable(strSql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string TableName = dt.Rows[i]["table_name"].ToString();
                dt.Rows[i]["table_camel_name"] = PublicHelper.GetCamelName(TableName);

                List<string> keyList = Generate.GetPrimayKeys(0, TableName); //0 Oracle
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
        /// 获取表的说明
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string TableComments(string tableName)
        {
            string strSql = string.Format("SELECT utc.comments FROM user_tab_comments utc where utc.table_name = '{0}'", tableName);
            string comments = OracleHelper.ExecuteScalar(strSql) as string;
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
            string sqlstr = string.Format(@"select A.COLUMN_NAME from user_tab_columns a where A.TABLE_NAME='{0}' and A.NULLABLE ='N'", tableName);
            DataTable dtPrimaryKeys1 = OracleHelper.GetDataTable(sqlstr);
            foreach (DataRow item in dtPrimaryKeys1.Rows)
            {
                string columnName = item["column_name"].ToString();
                columnName = columnName == tableName ? columnName + "1" : columnName;//如果字段名和表明相同,就在字段名后面加上字符"1"
                primayKeys.Add(columnName);
            }
            return primayKeys;
        }

        /// <summary>
        /// 根据表名对应的列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable getColumnsByTableName(string tableName)
        {
            DataTable dtColumns = OracleHelper.GetDataTable("SELECT utc.COLUMN_NAME,utc.DATA_TYPE,utc.DATA_LENGTH,"
                + "utc.DATA_PRECISION, utc.DATA_SCALE,"
                + "utc.NULLABLE, utc.DATA_DEFAULT,ucc.comments,'' Camel_Name FROM user_tab_columns utc "
                + "INNER JOIN user_col_comments ucc ON utc.table_name=ucc.table_name AND utc.column_name=ucc.column_name "
                + "WHERE UPPER(utc.table_name)='" + tableName + "'");
            return dtColumns;
        }

        /// <summary>
        /// 获取表的主键字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<string> GetPrimayKeys(string tableName)
        {
            //string sqlstr = string.Format(@"SELECT * FROM user_cons_columns ucc WHERE UPPER(ucc.table_name)='{0}' AND UPPER(ucc.constraint_name)=CONCAT('PK_','{1}')", tableName, tableName);
            string sqlstr = string.Format(@"select a.column_name 
                                              from user_cons_columns a, user_constraints b 
                                              where a.constraint_name = b.constraint_name 
                                              and b.constraint_type = 'P' 
                                              and a.table_name = '{0}'", tableName);
            DataTable dtPrimaryKeys = OracleHelper.GetDataTable(sqlstr);
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

    }
}
