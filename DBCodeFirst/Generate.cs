﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBCodeFirst
{
    public class Generate
    {
        static NameValueCollection appSettings = System.Configuration.ConfigurationManager.AppSettings;
        public string gGeneFile = Application.StartupPath + "\\GeneFile\\";
        public string correspondingXmlPath = Application.StartupPath + "\\Xml\\Corresponding.xml";
        public String dbSchema = appSettings["dbSchema"];
        //public string Schema;
        public string NameSpaceModel;
        public string NameSapceMaping;
        //OracleInfo info = new OracleInfo();
        Dictionary<string, string> dic = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Generate(Enumeration.DataBaseType dbtype)
        {
            NameValueCollection appSettings = System.Configuration.ConfigurationManager.AppSettings;
            if (dbtype == Enumeration.DataBaseType.Oracle)
            {
                //Schema = appSettings["Schema"];
                dbSchema = new OracleConnection(OracleHelper.connectStr).Database;
            }
            if (dbtype == Enumeration.DataBaseType.MySQL)
            {
                dbSchema = new MySqlConnection(MySqlHelper.connectStr).Database;
            }
            if (dbtype == Enumeration.DataBaseType.MSSQL)
            {
                dbSchema = "SCHEMA";
            }
            //if (dbtype == Enumeration.DataBaseType.PostgreSQL)
            //{
            //}
            NameSpaceModel = appSettings["NameSpaceModel"];
            NameSapceMaping = appSettings["NameSapceMaping"];

            dic = XMLHelper.readXml(correspondingXmlPath);
        }

        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="dbTableName">数据库中的表名称</param>
        /// <param name="tableCamelName">参与生成的表名称(可能是数据表名称也可能是表名称的骆驼表示法</param>
        public void GenerateModel(Enumeration.DataBaseType DBType, ModelTable mt)
        {
            StringBuilder sbTemp = new StringBuilder();
            DataTable dtColumns = SelectColumnsByTableName(DBType, mt.Table_Name);

            string colName = string.Empty;

            #region 生成命名空间和类
            sbTemp.Append("using System;"); //引入命名空间
            sbTemp.Append("\r\nusing Nest;");
            sbTemp.Append("\r\nusing System.ComponentModel;");
            sbTemp.Append("\r\nusing System.Collections.Generic;");
            sbTemp.Append("\r\nusing System.ComponentModel.DataAnnotations;");
            sbTemp.Append("\r\nusing System.ComponentModel.DataAnnotations.Schema;");
            sbTemp.Append("\r\n");
            sbTemp.Append("\r\n").Append("namespace ").Append(NameSpaceModel);    //命名空间
            sbTemp.Append("\r\n").Append("{");
            sbTemp.Append("\r\n").Append("\t/// <summary>");
            sbTemp.Append("\r\n").Append("\t///").Append(GetTableComments(DBType, mt.Table_Name)
                .Replace("\r\n", " ")
                .Replace("\n", " "));
            sbTemp.Append("\r\n").Append("\t/// </summary>");
            sbTemp.Append("\r\n").Append("\t[Serializable]");
            sbTemp.Append("\r\n").Append("\t[Table(\"" + mt.Table_Name + "\")]");
            sbTemp.Append("\r\n").Append("\t[ElasticsearchType(Name = \"" + mt.Table_Name + "\")]");
            sbTemp.Append("\r\n").Append("\tpublic partial class ")
                .Append(Words.ToSingular(Words.reWriteWord(mt.TabCamelName)));
            sbTemp.Append("\r\n").Append("\t{");    //类
            #endregion

            #region 生成属性
            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                //string tempTableName = dtColumns.Rows[i]["TABLE_NAME"].ToString();
                string tempColumnName = dtColumns.Rows[i]["COLUMN_NAME"].ToString();
                tempColumnName = tempColumnName == mt.Table_Name ? tempColumnName + "1" : tempColumnName;//如果字段名和表明相同,就在字段名后面加上字符"1"
                colName = tempColumnName;

                string tempType = dtColumns.Rows[i]["DATA_TYPE"].ToString();
                string tempLength = dtColumns.Rows[i]["DATA_LENGTH"].ToString();
                string tempPrecision = dtColumns.Rows[i]["DATA_PRECISION"].ToString();
                string tempScale = dtColumns.Rows[i]["DATA_SCALE"].ToString();
                string tempNullAble = dtColumns.Rows[i]["NULLABLE"].ToString();
                string tempDescription = dtColumns.Rows[i]["COMMENTS"].ToString().Replace("\r\n", " ").Replace("\n", " ");

                //if (string.IsNullOrWhiteSpace(tempScale))
                //{
                //    tempScale = "0";
                //}

                //属性
                sbTemp.Append("\r\n");
                sbTemp.Append("\r\n\t\t/// <summary>");
                sbTemp.Append("\r\n\t\t///").Append(tempDescription);
                sbTemp.Append("\r\n\t\t/// </summary>");
                //[Nest.String(Index = FieldIndexOption.NotAnalyzed)]
                //[Nest.String(Analyzer = "standard")]

                sbTemp.Append("\r\n\t\t[Display(Name = \"").Append(PublicHelper.GetCamelName(colName)).Append("\")]");
                if (tempNullAble == "N" || tempNullAble == "NO")
                {
                    sbTemp.Append("\r\n\t\t[Required]");
                }
                string ColType = DataTypeConvert.ConvertTypeVS2008(dtColumns.Rows[i]);
                if (ColType == "string")
                {
                    if (tempLength == "")
                    {
                        if (tempType == "text" || tempType == "character varying")
                        {
                            sbTemp.Append("\r\n\t\t[StringLength(").Append("8000").Append(")]");
                        }
                        else
                        {
                            MessageBox.Show("提示！", "未知类型：" + tempType, MessageBoxButtons.OK,
         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {
                        sbTemp.Append("\r\n\t\t[StringLength(").Append(tempLength).Append(")]");
                    }
                }
                //sbTemp.Append("\r\n\t\t[Column(TypeName = \"").Append(ColType).Append("\")]");

                //是否主键
                if (mt.PrimayKey.Equals(colName))
                {
                    sbTemp.Append("\r\n\t\t[Key]");
                    sbTemp.Append("\r\n\t\t[Keyword]");
                }

                sbTemp.Append("\r\n\t\tpublic ")
                                .Append(ColType)
                                .Append(" ")
                                .Append(Words.reWriteWord(colName))
                                .Append(" { get; set; }");

            }
            sbTemp.Append("\r\n\t}");
            sbTemp.Append("\r\n}");
            #endregion

            // 生成cs文件
            generateFile("Models", Words.ToSingular(mt.TabCamelName), sbTemp);
        }


        /// <summary>
        /// 生成Mapping
        /// </summary>
        /// <param name="dbTableName">数据库中的表名称</param>
        public void GenerateMapping(Enumeration.DataBaseType DBType, ModelTable mt)
        {
            String dbTableName = mt.Table_Name;
            StringBuilder sbTemp = new StringBuilder();
            DataTable dtColumns = SelectColumnsByTableName(DBType, dbTableName);

            string colName = string.Empty;

            #region 生成命名空间和类
            sbTemp.Append("using System.ComponentModel.DataAnnotations.Schema;");
            sbTemp.Append("\r\n");
            sbTemp.Append("using System.Data.Entity.ModelConfiguration;");
            sbTemp.Append("\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("namespace ").Append(NameSapceMaping);    //命名空间
            sbTemp.Append("\r\n");
            sbTemp.Append("{");
            sbTemp.Append("\r\n");
            sbTemp.Append("\tpublic class ").Append(Words.ToSingular(dbTableName))
                .Append("Map : EntityTypeConfiguration<").Append(Words.ToSingular(dbTableName)).Append(">"); ;
            sbTemp.Append("\r\n");
            sbTemp.Append("\t{");    //类
            #endregion

            #region 生成构造函数
            sbTemp.Append("\r\n");
            //sbTemp.Append("\t\t/// <summary>");
            //sbTemp.Append("\r\n");
            //sbTemp.Append("\t\t///").Append("构造函数");
            //sbTemp.Append("\r\n");
            //sbTemp.Append("\t\t/// </summary>");
            //sbTemp.Append("\r\n");
            sbTemp.Append("\t\tpublic ").Append(Words.ToSingular(dbTableName)).Append("Map").Append("()");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\t{");
            sbTemp.Append("\r\n");
            sbTemp.Append("\r\n");

            #region 主键
            sbTemp.Append("\t\t\t// Primary Key");
            sbTemp.Append("\r\n");
            List<string> keyList = GetPrimayKeys(DBType, dbTableName);

            List<string> keyList2 = keyList;


            if (keyList.Count == 0)
            {
                keyList = GetNotNullColumns(DBType, dbTableName);
                sbTemp.Append("\t\t\t//缺少主键!");
                //MessageBox.Show(tableName + "缺少主键!");
                //return;
            }
            else if (keyList.Count == 1)
            {
                sbTemp.Append("\t\t\tthis.HasKey(t => t.").Append(keyList[0]).Append(");");
            }
            else
            {
                sbTemp.Append("\t\t\tthis.HasKey(t => new {")
                    .Append("t.").Append(string.Join(",t.", keyList.ToArray())).Append("});");

            }
            #endregion

            sbTemp.Append("\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\t\t// Properties");
            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                string tempColumnName = dtColumns.Rows[i]["COLUMN_NAME"].ToString();
                tempColumnName = tempColumnName == dbTableName ? tempColumnName + "1" : tempColumnName;//如果字段名和表明相同,就在字段名后面加上字符"1"
                colName = tempColumnName;
                string tempType = dtColumns.Rows[i]["DATA_TYPE"].ToString().ToUpper();
                string strNullAble = dtColumns.Rows[i]["NULLABLE"].ToString();
                string sLength = dtColumns.Rows[i]["DATA_LENGTH"].ToString();
                int maxLength = int.Parse(sLength == "" ? "0" : sLength);

                StringBuilder sb = new StringBuilder();

                if (keyList2.Contains(colName))
                {
                    //产生问题:The changes to the database were committed successfully, but an error occurred while updating the object context 
                    //sb.Append("\r\n");
                    //sb.Append("\t\t\t.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)");
                }

                if (strNullAble == "N" || strNullAble == "NO")
                {
                    sb.Append("\r\n");
                    sb.Append("\t\t\t.IsRequired()");
                }

                if (tempType == "MEDIUMTEXT" || tempType == "TEXT" || tempType == "VARCHAR" //MySQL
                        || tempType == "CHAR" || tempType == "CLOB" || tempType == "LONG"   //Oracle
                        || tempType == "NCHAR" || tempType == "NCLOB" || tempType == "NVARCHAR2"
                        || tempType == "ROWID" || tempType == "VARCHAR2")
                {
                    if (tempType == "CHAR" || tempType == "NCHAR")
                    {
                        sb.Append("\r\n");
                        sb.Append("\t\t\t.IsFixedLength()");
                    }

                    if (maxLength > 0)
                    {
                        sb.Append("\r\n");
                        sb.Append("\t\t\t.HasMaxLength(").Append(maxLength).Append(")");
                    }
                }

                if (sb.Length > 0)
                {
                    sbTemp.Append("\r\n");
                    sbTemp.Append("\t\t\tthis.Property(t => t.").Append(Words.reWriteWord(colName)).Append(")");
                    sbTemp.Append(sb);
                    sbTemp.Append(";");
                    sbTemp.Append("\r\n");
                }
            }
            sbTemp.Append("\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\t\t// Table & Column Mappings");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\t\tthis.ToTable(\"")
                .Append(Words.reWriteWord(dbTableName)).Append("\", \"" + dbSchema + "\");");

            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                string tempColumnName = dtColumns.Rows[i]["COLUMN_NAME"].ToString();
                colName = tempColumnName;
                tempColumnName = tempColumnName == dbTableName ? tempColumnName + "1" : tempColumnName;//如果字段名和表明相同,就在字段名后面加上字符"1"
                sbTemp.Append("\r\n");
                sbTemp.Append("\t\t\tthis.Property(t => t.")
                    .Append(Words.reWriteWord(tempColumnName))
                    .Append(").HasColumnName(\"")
                    .Append(colName)
                    .Append("\");");
            }
            sbTemp.Append("\r\n\t\t}");
            sbTemp.Append("\r\n\t}");
            sbTemp.Append("\r\n}");

            #endregion

            // 生成cs文件
            generateFile("Models\\Mapping", Words.ToSingular(dbTableName) + "Map", sbTemp);
        }

        /// <summary>
        /// 生成dbContext
        /// </summary>
        /// <param name="pTables"></param>
        /// <param name="DBName"></param>
        public void GenerateDbContext(List<ModelTable> pTables, string DBName)
        {
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append("using Microsoft.EntityFrameworkCore;\r\n");
            sbTemp.Append("using SysConfig;\r\n");
            //sbTemp.Append("using System.Data.Common;\r\n");
            //sbTemp.Append("using System.Data.Entity;\r\n");
            //sbTemp.Append("using System.Data.Entity.Core.Objects;\r\n");
            //sbTemp.Append("using System.Data.Entity.Infrastructure;\r\n");
            //sbTemp.Append("using System.Data.Entity.Infrastructure.Interception;\r\n");

            //sbTemp.Append("using " + NameSpaceModel + ".Mapping;\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("namespace ").Append(NameSpaceModel).Append("\r\n");
            sbTemp.Append("{\r\n");
            //sbTemp.Append("\t[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]\r\n");

            //sbTemp.Append("\tpublic partial class " + DBName + "Context : DbContext\r\n");
            sbTemp.Append("\tpublic partial class " + "dbContext : DbContext\r\n");
            sbTemp.Append("\t{\r\n");
            sbTemp.Append("\t\t#region 构造函数\r\n");

            //sbTemp.Append("\t\tstatic " + DBName + "Context()\r\n");
            sbTemp.Append("\t\tpublic " + "dbContext()\r\n");
            sbTemp.Append("\t\t{\r\n");
            //sbTemp.Append("\t\t\tDatabase.SetInitializer<" + DBName + "Context>(null);\r\n");
            //sbTemp.Append("#if DEBUG\r\n");
            //sbTemp.Append("\t\t\tDbInterception.Add(new EFIntercepterLogging());\r\n");
            //sbTemp.Append("#endif\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\r\n");

            sbTemp.Append("\t\tpublic dbContext(DbContextOptions<dbContext> options): base(options)\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t}\r\n");


            //sbTemp.Append("\t\tpublic " + DBName + "Context() : base(\"Name=" + DBName + "Context\")\r\n");
            //sbTemp.Append("\t\t{\r\n");
            //sbTemp.Append("\t\t}\r\n");
            //sbTemp.Append("\r\n");
            //sbTemp.Append("\t\tpublic " + DBName + "Context(ObjectContext objectContext, bool dbContextOwnsObjectContext)\r\n");
            //sbTemp.Append("\t\t: base(objectContext, dbContextOwnsObjectContext)\r\n");
            //sbTemp.Append("\t\t{\r\n");
            //sbTemp.Append("\t\t}\r\n");
            //sbTemp.Append("\r\n");
            //sbTemp.Append("\t\tpublic " + DBName + "Context(string nameOrConnectionString)\r\n");
            //sbTemp.Append("\t\t: base(nameOrConnectionString)\r\n");
            //sbTemp.Append("\t\t{\r\n");
            //sbTemp.Append("\t\t}\r\n");
            //sbTemp.Append("\r\n");
            //sbTemp.Append("\t\tpublic " + DBName + "Context(DbConnection dbc, bool b) : base(dbc, b)\r\n");
            //sbTemp.Append("\t\t{\r\n");
            //sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\t#endregion\r\n");
            sbTemp.Append("\t\t#region DbSet\r\n");

            foreach (ModelTable item in pTables)
            {
                //if (item.PrimayKey == "Y")
                if (item.PrimayKey != "")
                {
                    sbTemp.Append("\t\tpublic DbSet<" + Words.ToSingular(item.TabCamelName) + "> " + Words.ToPlural(item.TabCamelName) + " { get; set; }\r\n");
                }
                //if (item.PrimayKey == "N")
                if (item.PrimayKey == "")
                {
                    sbTemp.Append("\t\t//无主键public DbSet<" + Words.ToSingular(item.TabCamelName) + "> " + Words.ToPlural(item.TabCamelName) + " { get; set; }\r\n");
                }
            }

            sbTemp.Append("\t\t#endregion\r\n");
            sbTemp.Append("\r\n");

            sbTemp.Append("\t\tprotected override void OnModelCreating(ModelBuilder modelBuilder)\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\tbase.OnModelCreating(modelBuilder);\r\n");
            sbTemp.Append("\t\t//         Database.SetInitializer<agescommContext> (null);\r\n");
            sbTemp.Append("\t\t////不使用复数\r\n");
            sbTemp.Append("\t\t//modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();\r\n");

            //foreach (ModelTable item in pTables)
            //{
            //    //if (item.PrimayKey == "Y")
            //    if (item.PrimayKey != "")
            //    {
            //        sbTemp.Append("\t\t\tmodelBuilder.Configurations.Add(new " + Words.ToSingular(item.Table_Name) + "Map());\r\n");
            //    }
            //    //if (item.PrimayKey == "N")
            //    if (item.PrimayKey == "")
            //    {
            //        sbTemp.Append("\t\t\t//无主键modelBuilder.Configurations.Add(new " + Words.ToSingular(item.Table_Name) + "Map());\r\n");
            //    }
            //}
            sbTemp.Append("\t\t}\r\n");


            sbTemp.Append("\t\tprotected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\tbase.OnConfiguring(optionsBuilder);\r\n");

            sbTemp.Append("\t\tstring connStr = ConfigurationManager.AppSettings[\"dbContext\"];\r\n");
            sbTemp.Append("\t\tstring dbType = ConfigurationManager.AppSettings[\"dbType\"].ToUpper();\r\n");

            //connStr = "Server=192.168.0.170;Port=3307;User Id=agescomm;Password=agescomm;Database=agescomm";
            //Console.WriteLine(connStr);
            sbTemp.Append("\t\tif (dbType == \"POSTGRESQL\")\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t\toptionsBuilder.UseNpgsql(connStr);\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\t\tif (dbType == \"MYSQL\")\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t\toptionsBuilder.UseMySQL(connStr);\r\n");
            sbTemp.Append("\t\t}\r\n");

            //optionsBuilder.UseMyCat("server=192.168.0.129;database=blog;uid=test;pwd=test") // MyCAT连接字符串
            // .UseDataNode("192.168.0.129", "blog_1", "root", "123456") // 数据节点连接信息
            // .UseDataNode("192.168.0.129", "blog_2", "root", "123456")
            sbTemp.Append("\t\t}\r\n");



            sbTemp.Append("\t}\r\n");
            sbTemp.Append("}\r\n");

            //generateFile("Models", DBName + "Context", sbTemp);
            generateFile("Models", "dbContext", sbTemp);

        }

        /// <summary>
        /// 生成EmodelEntities
        /// </summary>
        /// <param name="pTables"></param>
        public void GenerateModelEntities(List<ModelTable> pTables)
        {
            StringBuilder sbTemp = new StringBuilder();

            sbTemp.Append("using System;\r\n");
            sbTemp.Append("using System.Collections.Generic;\r\n");
            sbTemp.Append("using System.Linq;\r\n");
            sbTemp.Append("using System.Text;\r\n");
            sbTemp.Append("using System.Threading.Tasks;\r\n");
            sbTemp.Append("using System.Data.Entity.ModelConfiguration.Conventions;\r\n");
            sbTemp.Append("using System.Reflection.Emit;\r\n");
            sbTemp.Append("using System.Data.Entity.Core.Objects;\r\n");
            sbTemp.Append("using System.Data.Entity;\r\n");
            sbTemp.Append("using System.Data.Common;\r\n");
            sbTemp.Append("using AutoTest.Mapping;\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("namespace ").Append(NameSpaceModel).Append("\r\n");
            sbTemp.Append("{\r\n");
            sbTemp.Append("\t[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]\r\n");

            sbTemp.Append("\tpublic partial class MySqlModelEntities : DbContext\r\n");
            sbTemp.Append("\t{\r\n");
            sbTemp.Append("\t\t#region 构造函数\r\n");

            sbTemp.Append("\t\tstatic MySqlModelEntities()\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\tDatabase.SetInitializer<MySqlModelEntities>(null);\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\tpublic MySqlModelEntities(ObjectContext objectContext, bool dbContextOwnsObjectContext)\r\n");
            sbTemp.Append("\t\t: base(objectContext, dbContextOwnsObjectContext)\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\tpublic MySqlModelEntities(string nameOrConnectionString)\r\n");
            sbTemp.Append("\t\t: base(nameOrConnectionString)\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\tpublic MySqlModelEntities() : base(\"Name =MySqlConnection\")\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\tpublic MySqlModelEntities(DbConnection dbc, bool b) : base(dbc, b)\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\t#endregion\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\t#region DbSet\r\n");

            foreach (ModelTable item in pTables)
            {
                sbTemp.Append("\t\tpublic DbSet<" + item.Table_Name + "> " + item.Table_Name + "s" + " { get; set; }\r\n");
            }

            sbTemp.Append("\t\t#endregion\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\tprotected override void OnModelCreating(DbModelBuilder modelBuilder)\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t// 移除EF的表名公约  \r\n");
            sbTemp.Append("\t\t\tmodelBuilder.Conventions.Remove<PluralizingTableNameConvention>();\r\n");
            sbTemp.Append("\t\t\tforeach (var item in _MappingInfos)\r\n");
            sbTemp.Append("\t\t\t{\r\n");
            sbTemp.Append("\t\t\t\tdynamic map = GetInstance(item);\r\n");
            sbTemp.Append("\t\t\t\tmodelBuilder.Configurations.Add(map);\r\n");
            sbTemp.Append("\t\t\t}\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\t\tprivate static IEnumerable<Type> _mapping;\r\n");
            sbTemp.Append("\t\tprotected static IEnumerable<Type> _MappingInfos\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t\tget\r\n");
            sbTemp.Append("\t\t\t\t{\r\n");
            sbTemp.Append("\t\t\t\tif (_mapping == null)\r\n");
            sbTemp.Append("\t\t\t\t{\r\n");
            sbTemp.Append("\t\t\t\t\t//_mapping = typeof(fpagentMap).Assembly.GetTypes().Where(m => m.Name.EndsWith(\"Map\", true, null) && m.IsPublic && m.Namespace == "
                + NameSapceMaping + " && !m.IsGenericType).ToArray();\r\n");
            sbTemp.Append("\t\t\t\t}\r\n");
            sbTemp.Append("\t\t\t\treturn _mapping;\r\n");
            sbTemp.Append("\t\t\t}\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\t\t");
            sbTemp.Append("\t\t/// <summary>\r\n");
            sbTemp.Append("\t\t/// 创建对象，非反射\r\n");
            sbTemp.Append("\t\t/// </summary>\r\n");
            sbTemp.Append("\t\t/// <param name=\"type\"></param>\r\n");
            sbTemp.Append("\t\t/// <returns></returns>\r\n");
            sbTemp.Append("\t\tpublic static Object GetInstance(Type type)\r\n");
            sbTemp.Append("\t\t{\r\n");
            sbTemp.Append("\t\t\t///通过Emit动态生成创建对象委托减少反射带来的性能损耗\r\n");
            sbTemp.Append("\t\t\tDynamicMethod dm = new DynamicMethod(\"CreateInstanceDelete\", type, null, typeof(MySqlModelEntities).Module);\r\n");
            sbTemp.Append("\t\t\tILGenerator ilGenerator = dm.GetILGenerator();\r\n");
            sbTemp.Append("\t\t\tilGenerator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));\r\n");
            sbTemp.Append("\t\t\tilGenerator.Emit(OpCodes.Ret);\r\n");
            sbTemp.Append("\t\t\tvar create = dm.CreateDelegate(typeof(Func<object>)) as Func<object>;\r\n");
            sbTemp.Append("\t\t\treturn create();\r\n");
            sbTemp.Append("\t\t}\r\n");
            sbTemp.Append("\t}\r\n");
            sbTemp.Append("}\r\n");

            generateFile("Models", "MySqlModelEntities", sbTemp);

        }


        /// <summary>
        /// 生成DAL
        /// </summary>
        /// <param name="dbTableName">数据库中的表名称</param>
        /// <param name="tableCamelName">参与生成的表名称(可能是数据表名称也可能是表名称的骆驼表示法</param>
        public void GenerateDal(Enumeration.DataBaseType DBType, ModelTable mt)
        {
            StringBuilder sbTemp = new StringBuilder();
            DataTable dtColumns = SelectColumnsByTableName(DBType, mt.Table_Name);

            string colName = string.Empty;

            #region 内容
            sbTemp.Append("using DBModels;\r\n");
            sbTemp.Append("using Models;\r\n");
            sbTemp.Append("using System.Collections.Generic;\r\n");
            sbTemp.Append("using System.Linq;\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("namespace DAL\r\n");
            sbTemp.Append("{\r\n");
            sbTemp.Append("    public class " + mt.TabCamelName + "DAL : Base<" + mt.TabCamelName + ">\r\n");
            sbTemp.Append("    {\r\n");
            sbTemp.Append("        /// <summary>\r\n");
            sbTemp.Append("        /// 获取分页列表(模糊搜索)\r\n");
            sbTemp.Append("        /// </summary>\r\n");
            sbTemp.Append("        /// <param name=\"PageIndex\">页码</param>\r\n");
            sbTemp.Append("        /// <param name=\"PageSize\">页容量</param>\r\n");
            sbTemp.Append("        /// <returns></returns>\r\n");
            sbTemp.Append("        public PageModel<" + mt.TabCamelName + "> GetList(int PageIndex, int PageSize, string search)\r\n");
            sbTemp.Append("        {\r\n");
            sbTemp.Append("            using (dbContext db = new dbContext())\r\n");
            sbTemp.Append("            {\r\n");
            sbTemp.Append("                PageModel<" + mt.TabCamelName + "> model = new PageModel<" + mt.TabCamelName + ">();\r\n");
            sbTemp.Append("                var linq = db." + mt.TabCamelName + "s;\r\n");
            sbTemp.Append("                //if (CommHelper.CurrentUser != null && !CommHelper.CurrentUser.LoginName.ToLower().Contains(\"admin\"))\r\n");
            sbTemp.Append("                //{\r\n");
            sbTemp.Append("                //    linq = linq.Where(o => o.Creator == CommHelper.CurrentUser.ID);\r\n");
            sbTemp.Append("                //}\r\n");
            sbTemp.Append("                //if (!string.IsNullOrEmpty(search))\r\n");
            sbTemp.Append("                //{\r\n");
            sbTemp.Append("                //    linq = linq.Where(t => t.Keyword != null && t.Keyword.Contains(search));\r\n");
            sbTemp.Append("                //}\r\n");
            sbTemp.Append("                model.Total = linq.Count();\r\n");
            sbTemp.Append("                model.Data = linq.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();\r\n");
            sbTemp.Append("                return model;\r\n");
            sbTemp.Append("            }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        /// <summary>\r\n");
            sbTemp.Append("        /// 获取分页列表(模糊搜索) 查询状态 0未开始，1成功，2失败\r\n");
            sbTemp.Append("        /// </summary>\r\n");
            sbTemp.Append("        /// <param name=\"PageIndex\"></param>\r\n");
            sbTemp.Append("        /// <param name=\"PageSize\"></param>\r\n");
            sbTemp.Append("        /// <param name=\"search\">关键字</param>\r\n");
            sbTemp.Append("        /// <param name=\"state\">状态（0未开始，1成功，2失败）</param>\r\n");
            sbTemp.Append("        /// <returns></returns>\r\n");
            sbTemp.Append("        public PageModel<" + mt.TabCamelName + "> GetList(int PageIndex, int PageSize, string search,int state)\r\n");
            sbTemp.Append("        {\r\n");
            sbTemp.Append("            using (dbContext db = new dbContext())\r\n");
            sbTemp.Append("            {\r\n");
            sbTemp.Append("                PageModel<" + mt.TabCamelName + "> model = new PageModel<" + mt.TabCamelName + ">();\r\n");
            sbTemp.Append("                IQueryable<" + mt.TabCamelName + "> linq= null;\r\n");
            sbTemp.Append("                if (state==0)\r\n");
            sbTemp.Append("                {\r\n");
            sbTemp.Append("                    //只查询未开始的\r\n");
            sbTemp.Append("                     linq = db." + mt.TabCamelName + "s;\r\n");
            sbTemp.Append("                }\r\n");
            sbTemp.Append("                else\r\n");
            sbTemp.Append("                {\r\n");
            sbTemp.Append("                    linq = db." + mt.TabCamelName + "s;\r\n");
            sbTemp.Append("                }\r\n");
            sbTemp.Append("                //if (!string.IsNullOrEmpty(search))\r\n");
            sbTemp.Append("                //{\r\n");
            sbTemp.Append("                //    linq = linq.Where(t => t.Keyword != null && t.Keyword.Contains(search));\r\n");
            sbTemp.Append("                //}\r\n");
            sbTemp.Append("                model.Total = linq.Count();\r\n");
            sbTemp.Append("                //model.Data = linq.OrderByDescending(t => t.ModifyTime).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();\r\n");
            sbTemp.Append("                model.Data = linq.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();\r\n");
            sbTemp.Append("                return model;\r\n");
            sbTemp.Append("            }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        /// <summary>\r\n");
            sbTemp.Append("        /// 判断是否存在\r\n");
            sbTemp.Append("        /// </summary>\r\n");
            sbTemp.Append("        /// <param name=\"ClientID\">用户ID</param>\r\n");
            sbTemp.Append("        /// <param name=\"Keyword\">产品参数</param>\r\n");
            sbTemp.Append("        /// <param name=\"Name\">微博名称</param>\r\n");
            sbTemp.Append("        /// <param name=\"Enterprise\">企业名</param>\r\n");
            sbTemp.Append("        /// <returns></returns>\r\n");
            sbTemp.Append("        public bool IsExist(string ClientID, string Keyword, string Name, string Enterprise)\r\n");
            sbTemp.Append("        {\r\n");
            sbTemp.Append("            bool result = false;\r\n");
            sbTemp.Append("            using (dbContext db = new dbContext())\r\n");
            sbTemp.Append("            {\r\n");
            sbTemp.Append("                //var model = db." + mt.TabCamelName + "s.Where(t => t.DeleteTime == null && t.ClientID == ClientID && t.Keyword == Keyword && t.Name == Name && t.EnterpriseName == Enterprise).FirstOrDefault();\r\n");
            sbTemp.Append("                var model = db." + mt.TabCamelName + "s.FirstOrDefault();\r\n");
            sbTemp.Append("                if (model != null)\r\n");
            sbTemp.Append("                {\r\n");
            sbTemp.Append("                    result = true;\r\n");
            sbTemp.Append("                }\r\n");
            sbTemp.Append("                return result;\r\n");
            sbTemp.Append("            }\r\n");
            sbTemp.Append("        }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        /// <summary>\r\n");
            sbTemp.Append("        /// 获取全量分页列表(模糊搜索)\r\n");
            sbTemp.Append("        /// </summary>\r\n");
            sbTemp.Append("        /// <param name=\"PageIndex\"></param>\r\n");
            sbTemp.Append("        /// <param name=\"PageSize\"></param>\r\n");
            sbTemp.Append("        /// <returns></returns>\r\n");
            sbTemp.Append("        public PageModel<" + mt.TabCamelName + "> GetFullList(int PageIndex, int PageSize, string search, int fullstate = 0)\r\n");
            sbTemp.Append("        {\r\n");
            sbTemp.Append("            using (dbContext db = new dbContext())\r\n");
            sbTemp.Append("            {\r\n");
            sbTemp.Append("                PageModel<" + mt.TabCamelName + "> model = new PageModel<" + mt.TabCamelName + ">();\r\n");
            sbTemp.Append("                // t.IsFull = 2为开启全量抓取\r\n");
            sbTemp.Append("                var linq = db." + mt.TabCamelName + "s;\r\n");
            sbTemp.Append("                //if (!string.IsNullOrEmpty(search))\r\n");
            sbTemp.Append("                //{\r\n");
            sbTemp.Append("                //    linq = linq.Where(t => t.Keyword != null && t.Keyword.Contains(search));\r\n");
            sbTemp.Append("                //}\r\n");
            sbTemp.Append("                model.Total = linq.Count();\r\n");
            sbTemp.Append("                //model.Data = linq.OrderByDescending(t => t.ModifyTime).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();\r\n");
            sbTemp.Append("                model.Data = linq.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();\r\n");
            sbTemp.Append("                return model;\r\n");
            sbTemp.Append("            }\r\n");
            sbTemp.Append("        }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        /// <summary>\r\n");
            sbTemp.Append("        /// 获取分页列表(模糊搜索)\r\n");
            sbTemp.Append("        /// </summary>\r\n");
            sbTemp.Append("        /// <param name=\"PageIndex\"></param>\r\n");
            sbTemp.Append("        /// <param name=\"PageSize\"></param>\r\n");
            sbTemp.Append("        /// <param name=\"ClientID\">用户ID</param>\r\n");
            sbTemp.Append("        /// <returns></returns>\r\n");
            sbTemp.Append("        public PageModel<" + mt.TabCamelName + "> GetList(int PageIndex, int PageSize, string ClientID, string search)\r\n");
            sbTemp.Append("        {\r\n");
            sbTemp.Append("            using (dbContext db = new dbContext())\r\n");
            sbTemp.Append("            {\r\n");
            sbTemp.Append("                PageModel<" + mt.TabCamelName + "> model = new PageModel<" + mt.TabCamelName + ">();\r\n");
            sbTemp.Append("                var linq = db." + mt.TabCamelName + "s;\r\n");
            sbTemp.Append("                //if (!string.IsNullOrEmpty(search))\r\n");
            sbTemp.Append("                //{\r\n");
            sbTemp.Append("                //    linq = linq.Where(t => t.Keyword != null && t.Keyword.Contains(search));\r\n");
            sbTemp.Append("                //}\r\n");
            sbTemp.Append("                model.Total = linq.Count();\r\n");
            sbTemp.Append("                model.Data = linq.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();\r\n");
            sbTemp.Append("                return model;\r\n");
            sbTemp.Append("            }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        public int GetTotalCount()\r\n");
            sbTemp.Append("        {\r\n");
            sbTemp.Append("            using (dbContext db = new dbContext())\r\n");
            sbTemp.Append("            {\r\n");
            sbTemp.Append("                return db." + mt.TabCamelName + "s.Count();\r\n");
            sbTemp.Append("            }\r\n");
            sbTemp.Append("        }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("        /// <summary>\r\n");
            sbTemp.Append("        /// 分页获取list数据\r\n");
            sbTemp.Append("        /// </summary>\r\n");
            sbTemp.Append("        /// <param name=\"pageIndex\"></param>\r\n");
            sbTemp.Append("        /// <param name=\"pageSize\"></param>\r\n");
            sbTemp.Append("        /// <returns></returns>\r\n");
            sbTemp.Append("        public List<" + mt.TabCamelName + "> GetListByPage(int pageIndex, int pageSize)\r\n");
            sbTemp.Append("        {\r\n");
            sbTemp.Append("            using (dbContext db = new dbContext())\r\n");
            sbTemp.Append("            {\r\n");
            sbTemp.Append("                //return db." + mt.TabCamelName + "s.OrderBy(o => o.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();\r\n");
            sbTemp.Append("                return db." + mt.TabCamelName + "s.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();\r\n");
            sbTemp.Append("            }\r\n");
            sbTemp.Append("        }\r\n");
            sbTemp.Append("    }\r\n");
            sbTemp.Append("}\r\n");

            #endregion

            // 生成cs文件
            generateFile("DAL", Words.ToSingular(mt.TabCamelName + "DAL"), sbTemp);
        }


        /// <summary>
        /// 生成DAL
        /// </summary>
        /// <param name="dbTableName">数据库中的表名称</param>
        /// <param name="tableCamelName">参与生成的表名称(可能是数据表名称也可能是表名称的骆驼表示法</param>
        public void GenerateEs(Enumeration.DataBaseType DBType, ModelTable mt)
        {
            StringBuilder sbTemp = new StringBuilder();
            DataTable dtColumns = SelectColumnsByTableName(DBType, mt.Table_Name);

            string colName = string.Empty;

            #region 内容

            sbTemp.Append("using DBModels;\r\n");
            sbTemp.Append("using System;\r\n");
            sbTemp.Append("using System.Collections.Generic;\r\n");
            sbTemp.Append("using System.Linq;\r\n");
            sbTemp.Append("using System.Text;\r\n");
            sbTemp.Append("using System.Threading.Tasks;\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("namespace EsDal\r\n");
            sbTemp.Append("{\r\n");
            sbTemp.Append("    public class " + mt.TabCamelName + "ES : Base.EsBase\r\n");
            sbTemp.Append("    {\r\n");
            sbTemp.Append("        /// <summary>\r\n");
            sbTemp.Append("        /// 根据关键字和数据更新时间删除数据\r\n");
            sbTemp.Append("        /// </summary>\r\n");
            sbTemp.Append("        /// <param name=\"keyword\">关键字</param>\r\n");
            sbTemp.Append("        /// <returns></returns>\r\n");
            sbTemp.Append("        public bool DeleteByKeyword(string keyword)\r\n");
            sbTemp.Append("        {\r\n");
            sbTemp.Append("            var queryString = \"iD: (\\\"\" + keyword + \"\\\")\";\r\n");
            sbTemp.Append("            var response = client.DeleteByQuery<" + mt.TabCamelName + ">(p => p.Query(q => q.QueryString(o => o.Query(queryString))));\r\n");
            sbTemp.Append("            return response.Total > 0;\r\n");
            sbTemp.Append("        }\r\n");
            sbTemp.Append("\r\n");
            sbTemp.Append("    }\r\n");
            sbTemp.Append("}\r\n");

            #endregion

            // 生成cs文件
            generateFile("ES", Words.ToSingular(mt.TabCamelName + "ES"), sbTemp);
        }


        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="path">类型 分为model,bll,dal</param>
        /// <param name="pfileName"></param>
        public void generateFile(string path, string pfileName, StringBuilder sbTemp)
        {
            string direcPath = gGeneFile;
            if (!Directory.Exists(direcPath))
            {
                Directory.CreateDirectory(direcPath);
            }

            string filePath = "";
            if (path == "")
            {
                filePath = direcPath + "\\" + pfileName + ".cs";
            }
            else
            {
                if (!Directory.Exists(direcPath + path))
                {
                    Directory.CreateDirectory(direcPath + path);
                }
                filePath = direcPath + path + "\\" + pfileName + ".cs";
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (FileStream fs = File.Open(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(sbTemp.ToString());
                    //sw.Encoding = Encoding.UTF8;
                    sw.Flush();
                }
            }
        }


        /// <summary>
        /// 选择数据库
        /// </summary>
        /// <param name="DBType"></param>
        /// <param name="dbTableName"></param>
        /// <returns></returns>
        public static DataTable SelectColumnsByTableName(Enumeration.DataBaseType DBType, string dbTableName)
        {
            DataTable dtColumns = null;
            if (DBType == Enumeration.DataBaseType.Oracle)
            {
                dtColumns = new OracleInfo().getColumnsByTableName(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.MySQL)
            {
                dtColumns = new MySQLInfo().getColumnsByTableName(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.MSSQL)
            {
                dtColumns = new MSSQLInfo().getColumnsByTableName(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.PostgreSQL)
            {
                dtColumns = new PgInfo().getColumnsByTableName(dbTableName);
            }
            else
            {

            }

            return dtColumns;
        }

        /// <summary>
        /// 选获取表的主键
        /// </summary>
        /// <param name="DBType"></param>
        /// <param name="dbTableName"></param>
        /// <returns></returns>
        public static List<string> GetPrimayKeys(Enumeration.DataBaseType DBType, string dbTableName)
        {
            List<string> keyList = null;
            if (DBType == Enumeration.DataBaseType.Oracle)
            {
                keyList = new OracleInfo().GetPrimayKeys(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.MySQL)
            {
                keyList = new MySQLInfo().GetPrimayKeys(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.MSSQL)
            {
                keyList = new MSSQLInfo().GetPrimayKeys(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.PostgreSQL)
            {
                keyList = new PgInfo().GetPrimayKeys(dbTableName);
            }
            else
            {

            }

            for (int i = 0; i < keyList.Count(); i++)
            {
                keyList[i] = Words.reWriteWord(keyList[i]);
            }

            return keyList;
        }

        public static List<string> GetNotNullColumns(Enumeration.DataBaseType DBType, string dbTableName)
        {
            List<string> keyList = null;
            if (DBType == Enumeration.DataBaseType.Oracle)
            {
                keyList = new OracleInfo().GetNotNullColumns(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.MySQL)
            {
                keyList = new MySQLInfo().GetNotNullColumns(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.MSSQL)
            {
                keyList = new MSSQLInfo().GetNotNullColumns(dbTableName);
            }
            else
            {

            }

            return keyList;
        }

        /// <summary>
        /// 获取表的注释
        /// </summary>
        /// <param name="DBType"></param>
        /// <param name="dbTableName"></param>
        /// <returns></returns>
        public static string GetTableComments(Enumeration.DataBaseType DBType, string dbTableName)
        {
            string comment = null;
            if (DBType == Enumeration.DataBaseType.Oracle)
            {
                comment = new OracleInfo().TableComments(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.MySQL)
            {
                comment = new MySQLInfo().TableComments(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.MSSQL)
            {
                comment = new MSSQLInfo().TableComments(dbTableName);
            }
            else if (DBType == Enumeration.DataBaseType.PostgreSQL)
            {
                comment = new PgInfo().TableComments(dbTableName);
            }
            else
            {

            }

            return comment;
        }


    }
}
