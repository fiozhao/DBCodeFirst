using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCodeFirst
{
    /// <summary>
    /// Object对象操作类
    /// </summary>
    public class Comm_Object
    {
        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        static public bool ObjectIsNull(Object obj)
        {
            //如果对象引用为null 或者 对象值为null 或者对象值为空
            if (obj == null || obj == System.DBNull.Value || obj.ToString().Equals("") || obj.ToString() == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
