namespace DBCodeFirst
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ColComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilterTable = new System.Windows.Forms.TextBox();
            this.ckbAllTable = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dgvColumns = new System.Windows.Forms.DataGridView();
            this.COLUMN_NAME2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATA_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATA_LENGTH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATA_PRECISION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATA_SCALE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NULLABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATA_DEFAULT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Camel_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTables = new System.Windows.Forms.DataGridView();
            this.ckbCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.table_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.table_camel_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.primay_key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbxDataBase = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnShowTables = new System.Windows.Forms.Button();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTables)).BeginInit();
            this.SuspendLayout();
            // 
            // ColComments
            // 
            this.ColComments.DataPropertyName = "comments";
            this.ColComments.HeaderText = "说明";
            this.ColComments.Name = "ColComments";
            this.ColComments.ReadOnly = true;
            this.ColComments.Width = 54;
            // 
            // Column_Name
            // 
            this.Column_Name.DataPropertyName = "Column_Name";
            this.Column_Name.HeaderText = "列名";
            this.Column_Name.Name = "Column_Name";
            this.Column_Name.ReadOnly = true;
            this.Column_Name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_Name.Width = 35;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(831, 12);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(82, 30);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "生成";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "过滤表名：";
            // 
            // txtFilterTable
            // 
            this.txtFilterTable.Location = new System.Drawing.Point(70, 14);
            this.txtFilterTable.Name = "txtFilterTable";
            this.txtFilterTable.Size = new System.Drawing.Size(209, 21);
            this.txtFilterTable.TabIndex = 0;
            // 
            // ckbAllTable
            // 
            this.ckbAllTable.AutoSize = true;
            this.ckbAllTable.Location = new System.Drawing.Point(646, 19);
            this.ckbAllTable.Name = "ckbAllTable";
            this.ckbAllTable.Size = new System.Drawing.Size(48, 16);
            this.ckbAllTable.TabIndex = 4;
            this.ckbAllTable.Text = "全选";
            this.ckbAllTable.UseVisualStyleBackColor = true;
            this.ckbAllTable.CheckedChanged += new System.EventHandler(this.ckbAllTable_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtFilterTable);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Location = new System.Drawing.Point(240, 2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(299, 40);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "数据表";
            // 
            // dgvColumns
            // 
            this.dgvColumns.AllowUserToAddRows = false;
            this.dgvColumns.AllowUserToDeleteRows = false;
            this.dgvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COLUMN_NAME2,
            this.DATA_TYPE,
            this.DATA_LENGTH,
            this.DATA_PRECISION,
            this.DATA_SCALE,
            this.NULLABLE,
            this.DATA_DEFAULT,
            this.Column4,
            this.Camel_Name});
            this.dgvColumns.Location = new System.Drawing.Point(0, 313);
            this.dgvColumns.Name = "dgvColumns";
            this.dgvColumns.ReadOnly = true;
            this.dgvColumns.RowHeadersVisible = false;
            this.dgvColumns.RowTemplate.Height = 23;
            this.dgvColumns.Size = new System.Drawing.Size(920, 241);
            this.dgvColumns.TabIndex = 8;
            // 
            // COLUMN_NAME2
            // 
            this.COLUMN_NAME2.DataPropertyName = "COLUMN_NAME";
            this.COLUMN_NAME2.HeaderText = "列名";
            this.COLUMN_NAME2.Name = "COLUMN_NAME2";
            this.COLUMN_NAME2.ReadOnly = true;
            this.COLUMN_NAME2.Width = 140;
            // 
            // DATA_TYPE
            // 
            this.DATA_TYPE.DataPropertyName = "DATA_TYPE";
            this.DATA_TYPE.HeaderText = "类型";
            this.DATA_TYPE.Name = "DATA_TYPE";
            this.DATA_TYPE.ReadOnly = true;
            this.DATA_TYPE.Width = 60;
            // 
            // DATA_LENGTH
            // 
            this.DATA_LENGTH.DataPropertyName = "DATA_LENGTH";
            this.DATA_LENGTH.HeaderText = "字节长度";
            this.DATA_LENGTH.Name = "DATA_LENGTH";
            this.DATA_LENGTH.ReadOnly = true;
            this.DATA_LENGTH.Width = 90;
            // 
            // DATA_PRECISION
            // 
            this.DATA_PRECISION.DataPropertyName = "DATA_PRECISION";
            this.DATA_PRECISION.HeaderText = "数字类型长度";
            this.DATA_PRECISION.Name = "DATA_PRECISION";
            this.DATA_PRECISION.ReadOnly = true;
            this.DATA_PRECISION.Width = 120;
            // 
            // DATA_SCALE
            // 
            this.DATA_SCALE.DataPropertyName = "DATA_SCALE";
            this.DATA_SCALE.HeaderText = "小数位数";
            this.DATA_SCALE.Name = "DATA_SCALE";
            this.DATA_SCALE.ReadOnly = true;
            this.DATA_SCALE.Width = 80;
            // 
            // NULLABLE
            // 
            this.NULLABLE.DataPropertyName = "NULLABLE";
            this.NULLABLE.HeaderText = "允空";
            this.NULLABLE.Name = "NULLABLE";
            this.NULLABLE.ReadOnly = true;
            this.NULLABLE.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NULLABLE.Width = 40;
            // 
            // DATA_DEFAULT
            // 
            this.DATA_DEFAULT.DataPropertyName = "DATA_DEFAULT";
            this.DATA_DEFAULT.HeaderText = "缺省值";
            this.DATA_DEFAULT.Name = "DATA_DEFAULT";
            this.DATA_DEFAULT.ReadOnly = true;
            this.DATA_DEFAULT.Width = 80;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "COMMENTS";
            this.Column4.HeaderText = "描述";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 200;
            // 
            // Camel_Name
            // 
            this.Camel_Name.DataPropertyName = "Camel_Name";
            this.Camel_Name.HeaderText = "驼峰命名";
            this.Camel_Name.Name = "Camel_Name";
            this.Camel_Name.ReadOnly = true;
            // 
            // dgvTables
            // 
            this.dgvTables.AllowUserToAddRows = false;
            this.dgvTables.AllowUserToResizeRows = false;
            this.dgvTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ckbCheck,
            this.table_name,
            this.table_camel_name,
            this.primay_key,
            this.comments});
            this.dgvTables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvTables.Location = new System.Drawing.Point(0, 50);
            this.dgvTables.Name = "dgvTables";
            this.dgvTables.RowHeadersVisible = false;
            this.dgvTables.RowTemplate.Height = 23;
            this.dgvTables.Size = new System.Drawing.Size(920, 257);
            this.dgvTables.TabIndex = 9;
            this.dgvTables.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTables_CellMouseClick);
            // 
            // ckbCheck
            // 
            this.ckbCheck.DataPropertyName = "ckbCheck";
            this.ckbCheck.FillWeight = 60.9137F;
            this.ckbCheck.HeaderText = "";
            this.ckbCheck.Name = "ckbCheck";
            this.ckbCheck.Width = 5;
            // 
            // table_name
            // 
            this.table_name.DataPropertyName = "Table_Name";
            this.table_name.FillWeight = 113.0288F;
            this.table_name.HeaderText = "原始表名";
            this.table_name.Name = "table_name";
            this.table_name.ReadOnly = true;
            this.table_name.Width = 78;
            // 
            // table_camel_name
            // 
            this.table_camel_name.DataPropertyName = "table_camel_name";
            this.table_camel_name.HeaderText = "驼峰命名";
            this.table_camel_name.Name = "table_camel_name";
            this.table_camel_name.Width = 78;
            // 
            // primay_key
            // 
            this.primay_key.DataPropertyName = "primay_key";
            this.primay_key.HeaderText = "有主键";
            this.primay_key.Name = "primay_key";
            this.primay_key.Width = 66;
            // 
            // comments
            // 
            this.comments.DataPropertyName = "comments";
            this.comments.FillWeight = 113.0288F;
            this.comments.HeaderText = "说明";
            this.comments.Name = "comments";
            this.comments.ReadOnly = true;
            this.comments.Width = 54;
            // 
            // cbxDataBase
            // 
            this.cbxDataBase.FormattingEnabled = true;
            this.cbxDataBase.Items.AddRange(new object[] {
            "Oracle",
            "MySQL",
            "MSSQL",
            "PostgreSQL"});
            this.cbxDataBase.Location = new System.Drawing.Point(88, 17);
            this.cbxDataBase.Name = "cbxDataBase";
            this.cbxDataBase.Size = new System.Drawing.Size(121, 20);
            this.cbxDataBase.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "数据库类型：";
            // 
            // btnShowTables
            // 
            this.btnShowTables.Location = new System.Drawing.Point(550, 14);
            this.btnShowTables.Name = "btnShowTables";
            this.btnShowTables.Size = new System.Drawing.Size(64, 28);
            this.btnShowTables.TabIndex = 12;
            this.btnShowTables.Text = "显示";
            this.btnShowTables.UseVisualStyleBackColor = true;
            this.btnShowTables.Click += new System.EventHandler(this.btnShowTables_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 562);
            this.Controls.Add(this.btnShowTables);
            this.Controls.Add(this.ckbAllTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxDataBase);
            this.Controls.Add(this.dgvTables);
            this.Controls.Add(this.dgvColumns);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主界面";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn ColComments;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Name;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilterTable;
        private System.Windows.Forms.CheckBox ckbAllTable;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView dgvColumns;
        private System.Windows.Forms.DataGridView dgvTables;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATA_LENGTH;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATA_PRECISION;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATA_SCALE;
        private System.Windows.Forms.DataGridViewTextBoxColumn NULLABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATA_DEFAULT;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Camel_Name;
        private System.Windows.Forms.ComboBox cbxDataBase;
        private System.Windows.Forms.Button btnShowTables;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn COLUMN_NAME2;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATA_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn primay_key;
        private System.Windows.Forms.DataGridViewTextBoxColumn comments;
        private System.Windows.Forms.DataGridViewTextBoxColumn table_camel_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn table_name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ckbCheck;
    }
}