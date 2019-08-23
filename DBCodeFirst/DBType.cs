using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCodeFirst
{

    public class Enumeration
    {
        public enum DataBaseType
        {
            Oracle = 0,
            MySQL = 1,
            MSSQL = 2,
            PostgreSQL = 3
        }

        enum Days { Sat, Sun, Mon, Tue, Wed, Thu, Fri };

    }
}
