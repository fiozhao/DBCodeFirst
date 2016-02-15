using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCodeFirst
{

    public class ModelTable
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Table_Name { get; set; }

        /// <summary>
        /// 骆驼法表名
        /// </summary>
        public string TabCamelName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 是否有主键
        /// </summary>
        public string PrimayKey { get; set; }
    }
}
