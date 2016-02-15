using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBCodeFirst
{
    class DataTypeConvert
    {
        /// <summary>
        /// 数据库类型到C#类型的转换(VS2008以上版本)
        /// </summary>
        /// <param name="dbtype">数据库中的类型</param>
        /// <returns></returns>
        public static string ConvertTypeVS2008(DataRow dtColumnsRow)
        {
            string tempType = dtColumnsRow["DATA_TYPE"].ToString();

            //var strLength = dtColumnsRow["DATA_SCALE"];
            //decimal strLength = (decimal?)dtColumnsRow["DATA_LENGTH"] ?? 0;
            //string tempPrecision = (int)(dtColumnsRow["DATA_PRECISION"] ?? 0);
            //string tempScale = (int)(dtColumnsRow["DATA_SCALE"] ?? 0);
            string sLength = dtColumnsRow["DATA_LENGTH"].ToString();
            decimal tempLength = decimal.Parse(sLength == "" ? "0" : sLength);
            //decimal tempLength = dtColumnsRow["DATA_LENGTH"] == System.DBNull.Value 
            //    ? 0:(decimal)(dtColumnsRow["DATA_LENGTH"]);
            string sPrecision = dtColumnsRow["DATA_PRECISION"].ToString();
            decimal tempPrecision = decimal.Parse(sPrecision == "" ? "0" : sPrecision);
            //decimal tempPrecision = dtColumnsRow["DATA_PRECISION"] == DBNull.Value 
            //    ? 0:(decimal)(dtColumnsRow["DATA_PRECISION"].ToString()) ;
            string sDATA_SCALE = dtColumnsRow["DATA_SCALE"].ToString();
            decimal tempScale = decimal.Parse(sDATA_SCALE == "" ? "0" : sDATA_SCALE);
            //decimal tempScale = dtColumnsRow["DATA_SCALE"] == DBNull.Value 
            //    ? 0 : (decimal)(dtColumnsRow["DATA_SCALE"]) ;
            string tempNullAble = dtColumnsRow["NULLABLE"].ToString();
            string tempDescription = dtColumnsRow["Comments"].ToString().Replace("\r\n", " ").Replace("\n", " ");


            string str = string.Empty;
            string type = tempType.ToUpper();
            string ret = "string";

            if (type == "TINYBLOB" || type == "BINARY" || type == "VARBINARY"
                || type == "TIMESTAMP" || type == "BFILE" || type == "BLOB"
                || type == "LONG RAW" || type == "RAW" || type == "MEDIUMBLOB"
                || type == "LONGBLOB")
            {
                //ret = tempNullAble == "N" ? "byte[]": "byte?[]";
                ret = "byte[]";
            }
            else if (type == "TEXT" || type == "TINYTEXT" || type == "MEDIUMTEXT"
                || type == "LONGTEXT" || type == "CHAR" || type == "CLOB"
                || type == "LONG" || type == "NCHAR" || type == "NCLOB"
                || type == "NVARCHAR2" || type == "ROWID" || type == "VARCHAR2"
                || type == "NVARCHAR" || type == "VARCHAR")
            {
                //ret = tempNullAble == "N" ? "string": "string?";类型“string”必须是不可以为 null 值的类型，才能用作泛型类型或方法“Nullable<T>”中的参数“T”
                ret = "string";
            }
            else if (type == "ENUM" || type == "SET")
            {
                ret = "string";
            }
            else if (type == "FLOAT" || type == "DOUBLE")
            {
                ret = tempNullAble == "N" ? "double" : "double?";
            }
            else if (type == "DECILMAL" || type == "DECIMAL")
            {
                ret = tempNullAble == "N" ? "decimal" : "decimal?";
            }
            else if (type == "NUMBER")
            {
                if (tempLength <= 22 && tempScale == 0) //无小数位数 
                {
                    //Toad 中的 INTEGER 数据类型
                    ret = tempNullAble == "N" ? "long" : "long?";
                }
                else
                {
                    ret = tempNullAble == "N" ? "decimal" : "decimal?";
                }
            }
            else if (type == "TINYINT")
            {
                if (tempPrecision == 3 && tempScale == 0) //Boolean
                {
                    ret = tempNullAble == "N" ? "bool" : "bool?";
                }
                else
                {
                    ret = tempNullAble == "N" ? "byte" : "byte?";
                }
            }
            else if (type == "INT" || type == "IPADDRESSTYPE" || type == "MEDIUMINT")
            {
                ret = tempNullAble == "N" ? "int" : "int?";
            }
            else if (type == "BIGINT")
            {
                ret = tempNullAble == "N" ? "long" : "long?";
            }
            else if (type == "SMALLINT" || type == "YEAR")
            {
                ret = tempNullAble == "N" ? "short" : "short?";
            }
            else if (type == "INTEGER") //Oracle 无这类型，实际可能为 NUMBER(38,0)
            {
                ret = tempNullAble == "N" ? "long" : "long?";
            }
            else if (type == "DATE")
            {
                ret = tempNullAble == "N" ? "DateTime" : "DateTime?";
            }
            else if (type == "DATETIME")
            {
                ret = tempNullAble == "N" ? "System.DateTime" : "System.DateTime?";
            }
            else if (type == "TIME")
            {
                ret = tempNullAble == "N" ? "System.TimeSpan" : "System.TimeSpan?";
            }
            else if (type == "BIT")
            {
                ret = tempNullAble == "N" ? "bool" : "bool?";
            }
            else if (type == "UNIQUEIDENTIFIER")
            {
                ret = tempNullAble == "N" ? "System.Guid" : "System.Guid?";
            }

            else if (type == "GEOMETRY" || type == "POINT" || type == "LINESTRING"
                || type == "POLYGON" || type == "MULTIPOINT" || type == "MULTILINESTRING"
                || type == "MULTIPOLYGON" || type == "GEOMETRYCOLLECTION")
            {
                ret = "string";
            }
            else
            {
                DialogResult dr;
                dr = MessageBox.Show("提示！", "未知类型：" + type, MessageBoxButtons.OK,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                ret = "string";
            }

            return ret;
        }

        /// <summary>
        /// 数据库类型到C#类型的转换(VS2005版本)
        /// </summary>
        /// <param name="dbtype">数据库中的类型</param>
        /// <returns></returns>
        public static string ConvertType2005(string dbtype, string tempLength, string tempScale)
        {

            string type = dbtype.ToUpper();
            string ret = "string";

            if (type == "NVARCHAR2" || type == "VARCHAR2" || type == "CHAR")
            {
                ret = "string";
            }
            else if (type == "NUMBER")
            {
                int length = Convert.ToInt32(tempLength);
                int scale = Convert.ToInt32(tempScale);
                if (scale > 0)
                {
                    ret = "decimal";//int 类型可空
                }
                else if (length <= 10)
                {
                    ret = "int";//int 类型可空
                }
                else
                {
                    ret = "long";
                }
            }
            else if (type == "DATE")
            {
                ret = "DateTime";
            }

            return ret;
        }


        /// <summary>
        /// 转换构造函数(VS2008以上版本)
        /// </summary>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        public static string ConvertConstruct2008(string dbtype)
        {
            string ret = "null";
            switch (dbtype.ToLower())
            {
                case "VARCHAR2":
                case "NVARCHAR2":
                    ret = "string.Empty";
                    break;
                case "int":
                case "smallint":
                case "tinyint":
                    ret = "null";  //int 类型也用string类型表示
                    break;
                case "bigint":
                    ret = "null";
                    break;
                case "smallmoney":
                case "decimal":
                case "numeric":
                    ret = "null";
                    break;
                case "float":
                    ret = "null";
                    break;
                case "money":
                    ret = "null";
                    break;
                case "DATE":
                    ret = "null";
                    break;
                case "bit":
                    ret = "null";
                    break;
                default:
                    ret = "null";
                    break;
            }
            return ret;
        }

        public static string ConvertFristLeeterToLower(string parStr)
        {
            string firstLowerLetter = parStr[0].ToString().ToLower();
            return firstLowerLetter + parStr.Substring(1, parStr.Length - 1);
        }

        public static string ConvertFristLeeterToUpper(string parStr)
        {
            string firstLowerLetter = parStr[0].ToString().ToUpper();
            return firstLowerLetter + parStr.Substring(1, parStr.Length - 1);
        }
    }
}
