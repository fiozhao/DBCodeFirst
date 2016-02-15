using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCodeFirst
{
    static class PublicHelper
    {

        /// <summary>
        /// 将字段编程驼峰命名法形式
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetCamelName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                return null;
            }
            if (columnName.IndexOf("_") > 0 | columnName.IndexOf(" ") > 0)
            {

                string[] arr = columnName.ToLower().Split('_');
                string newColumnName = string.Empty;
                foreach (var item in arr)
                {
                    string dx = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item);
                    newColumnName += dx;
                }
                return newColumnName;
            }
            else {
                return columnName;
            }

        }


        /// <summary>
        /// 将dtColumns列中列名由大写转化为骆驼表示法
        /// </summary>
        /// <param name="dtColumns"></param>
        /// <returns>所有列名都有对应的骆驼表示法返回true,否则返回false</returns>
        public static bool getColCamelName(DataTable dtColumns, Dictionary<string, string> dic)
        {
            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                string _colName = dtColumns.Rows[i]["Column_Name"].ToString();
                if (dic.ContainsKey(_colName))
                {
                    dtColumns.Rows[i]["Column_Name"] = dic[_colName];
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
