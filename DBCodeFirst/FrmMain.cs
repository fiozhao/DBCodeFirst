using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace DBCodeFirst
{
    public partial class FrmMain : Form
    {
        Generate gen = null;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            cbxDataBase.SelectedIndex = 3;

            this.dgvColumns.AutoGenerateColumns = false;
            //this.txtFilterTable.Focus();
            cbxDataBase.Focus();

            createCorrespondingXmlWhenNotExist();
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            List<ModelTable> listTables = getCheckedTableName();

            if (listTables.Count == 0)
            {
                MessageBox.Show("没勾选任何表名");
                return;
            }

            Enumeration.DataBaseType Type = (Enumeration.DataBaseType)cbxDataBase.SelectedIndex;
            Generate gen = new Generate(Type);
            string dbName = "";
            if (Type == Enumeration.DataBaseType.Oracle)
            {
                dbName = new OracleConnection(OracleHelper.connectStr).Database;
            }
            if (Type == Enumeration.DataBaseType.MySQL)
            {
                dbName = new MySqlConnection(MySqlHelper.connectStr).Database;
            }
            if (Type == Enumeration.DataBaseType.MSSQL)
            {
                dbName = new SqlConnection(MSSQLHelper.connectStr).Database;
            }
            if (Type == Enumeration.DataBaseType.PostgreSQL)
            {
                dbName = new SqlConnection(PgHelper.connectStr).Database;
            }
            else
            {
            }

            gen.GenerateDbContext(listTables, dbName);
            gen.GenerateModelEntities(listTables);
            foreach (ModelTable item in listTables)
            {
                gen.GenerateModel((Enumeration.DataBaseType)cbxDataBase.SelectedIndex, item);
                gen.GenerateMapping((Enumeration.DataBaseType)cbxDataBase.SelectedIndex, item);
                gen.GenerateDal((Enumeration.DataBaseType)cbxDataBase.SelectedIndex, item);
                gen.GenerateEs((Enumeration.DataBaseType)cbxDataBase.SelectedIndex, item);
            }

            //MessageBox.Show("生成文件成功!");
            System.Diagnostics.Process.Start("explorer.exe ", gen.gGeneFile);
        }

        /// <summary>
        /// 获取勾选的表信息
        /// </summary>
        /// <returns></returns>
        private List<ModelTable> getCheckedTableName()
        {
            List<ModelTable> list = new List<ModelTable>();
            ModelTable model;
            for (int i = 0; i < this.dgvTables.Rows.Count; i++)
            {
                if (this.dgvTables.Rows[i].Cells["ckbCheck"].Value != null && (bool)this.dgvTables.Rows[i].Cells["ckbCheck"].Value)
                {
                    model = new ModelTable();
                    model.Table_Name = this.dgvTables.Rows[i].Cells["table_name"].Value.ToString();
                    model.TabCamelName = this.dgvTables.Rows[i].Cells["table_camel_name"].Value.ToString();
                    model.PrimayKey = this.dgvTables.Rows[i].Cells["primay_key"].Value.ToString();

                    if (this.dgvTables.Rows[i].Cells["comments"].Value != null)
                    {
                        model.Comments = this.dgvTables.Rows[i].Cells["comments"].Value.ToString();
                    }
                    else
                    {
                        model.Comments = "";
                    }
                    list.Add(model);
                }
            }
            return list;
        }


        #region 获取列信息
        private void dgvTables_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                return;
            }
            //表中字段
            DataTable dtColumns = null;
            if (cbxDataBase.SelectedIndex == 0)
            {
                dtColumns = new OracleInfo().getColumnsByTableName(this.dgvTables.Rows[e.RowIndex].Cells["Table_Name"].Value.ToString());
            }
            else if (cbxDataBase.SelectedIndex == 1)
            {
                dtColumns = new MySQLInfo().getColumnsByTableName(this.dgvTables.Rows[e.RowIndex].Cells["Table_Name"].Value.ToString());
            }
            else if (cbxDataBase.SelectedIndex == 2)
            {
                dtColumns = new MSSQLInfo().getColumnsByTableName(this.dgvTables.Rows[e.RowIndex].Cells["Table_Name"].Value.ToString());
            }
            else if (cbxDataBase.SelectedIndex == 3)
            {
                dtColumns = new PgInfo().getColumnsByTableName(this.dgvTables.Rows[e.RowIndex].Cells["Table_Name"].Value.ToString());
            }
            else
            {

            }

            dtColumns.TableName = "Columns";
            createCorrespondingXmlWhenNotExist();
            
            //显示骆驼表示法列名
            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                string name = dtColumns.Rows[i]["Column_Name"].ToString();
                //if (dic.ContainsKey(name))
                //{
                //    dtColumns.Rows[i]["CamelName"] = dic[name];
                //}
                dtColumns.Rows[i]["Camel_Name"] = PublicHelper.GetCamelName(name);
            }
            dtColumns.AcceptChanges();

            this.dgvColumns.DataSource = dtColumns;
            //this.dgvColumns.AutoResizeColumns();
        }
        #endregion


        /// <summary>
        /// 创建存放原名称和骆驼法名称对应关系的XML文件
        /// </summary>
        private void createCorrespondingXmlWhenNotExist()
        {
            Enumeration.DataBaseType type = (Enumeration.DataBaseType)cbxDataBase.SelectedIndex;
            Generate gen = new Generate(type);

            if (!File.Exists(gen.correspondingXmlPath))
            {
                DataTable dtTableName = new DataTable();
                dtTableName.Columns.Add("OriName");
                dtTableName.Columns.Add("Camel_Name");
                dtTableName.TableName = "dt";
                dtTableName.Rows.Add(new string[] { "1", "1" });
                dtTableName.WriteXml(gen.correspondingXmlPath);
            }
        }

        private void ckbAllTable_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowTables_Click(object sender, EventArgs e)
        {
            DataTable tb = null;
            Enumeration.DataBaseType type = (Enumeration.DataBaseType)cbxDataBase.SelectedIndex;
            //Generate gen = new Generate(type);

            if (type == Enumeration.DataBaseType.Oracle)
            {
                tb = new OracleInfo().DBTables(txtFilterTable.Text);
            }
            else if (type == Enumeration.DataBaseType.MySQL)
            {
                tb = new MySQLInfo().DBTables(txtFilterTable.Text);
            }
            else if (type == Enumeration.DataBaseType.MSSQL)
            {
                tb = new MSSQLInfo().DBTables(txtFilterTable.Text);
            }
            else if (type == Enumeration.DataBaseType.PostgreSQL)
            {
                tb = new PgInfo().DBTables(txtFilterTable.Text);
            }
            else
            {
            }

            this.dgvTables.DataSource = tb;

            //CheckAll();
        }

        private void CheckAll()
        {
            for (int i = 0; i < this.dgvTables.Rows.Count; i++)
            {
                this.dgvTables.Rows[i].Cells["ckbCheck"].Value = this.ckbAllTable.Checked;
            }
        }

    }


}
