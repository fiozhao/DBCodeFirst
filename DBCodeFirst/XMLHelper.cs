using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCodeFirst
{
    public static class XMLHelper
    {

        /// <summary>
        /// 读取XML文件，并以键值对的形式返回值
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> readXml(string XMLPath)
        {
            DataSet dsXml = new DataSet();
            dsXml.ReadXml(XMLPath);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < dsXml.Tables[0].Rows.Count; i++)
            {
                dic.Add(dsXml.Tables[0].Rows[i]["OriName"].ToString(),
                   PublicHelper.GetCamelName(dsXml.Tables[0].Rows[i]["Camel_Name"].ToString()));
            }
            return dic;
        }

        /// <summary>
        /// 读取XML文件，并以数据集的形式返回值
        /// </summary>
        /// <returns></returns>
        public static DataSet readXmlDs(string XMLPath)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(XMLPath);
            return ds;
        }
    }
}
