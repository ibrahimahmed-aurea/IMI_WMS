namespace Cdc.MetaManager.GUI
{
    partial class FindPropertyForm
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
            this.components = new System.ComponentModel.Container();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbSelect = new System.Windows.Forms.GroupBox();
            this.countLbl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.propertyListView = new ListViewSort();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbService = new System.Windows.Forms.GroupBox();
            this.mapTypeGb = new System.Windows.Forms.GroupBox();
            this.rbRequestMap = new System.Windows.Forms.RadioButton();
            this.rbResponseMap = new System.Windows.Forms.RadioButton();
            this.searchServiceBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.serviceCb = new System.Windows.Forms.ComboBox();
            this.serviceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.serviceMethodCb = new System.Windows.Forms.ComboBox();
            this.serviceMethodBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.gbNameTableColBE = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.beTbx = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.columnTbx = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableTbx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTbx = new System.Windows.Forms.TextBox();
            this.selectSearchfieldsGb = new System.Windows.Forms.GroupBox();
            this.rbService = new System.Windows.Forms.RadioButton();
            this.rbNameTableColBE = new System.Windows.Forms.RadioButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.propertyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.hintBtn = new System.Windows.Forms.Button();
            this.btnCreateProperty = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblInformation = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbSelect.SuspendLayout();
            this.gbService.SuspendLayout();
            this.mapTypeGb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.serviceMethodBindingSource)).BeginInit();
            this.gbNameTableColBE.SuspendLayout();
            this.selectSearchfieldsGb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyBindingSource)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.ContextMenuStrip = this.PropertyGridcontextMenuStrip;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(714, 199);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            this.propertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGrid_SelectedGridItemChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbSelect);
            this.panel1.Controls.Add(this.gbService);
            this.panel1.Controls.Add(this.gbNameTableColBE);
            this.panel1.Controls.Add(this.selectSearchfieldsGb);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 355);
            this.panel1.TabIndex = 0;
            // 
            // gbSelect
            // 
            this.gbSelect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbSelect.Controls.Add(this.countLbl);
            this.gbSelect.Controls.Add(this.label5);
            this.gbSelect.Controls.Add(this.propertyListView);
            this.gbSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSelect.Location = new System.Drawing.Point(0, 249);
            this.gbSelect.Name = "gbSelect";
            this.gbSelect.Padding = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.gbSelect.Size = new System.Drawing.Size(720, 106);
            this.gbSelect.TabIndex = 3;
            this.gbSelect.TabStop = false;
            this.gbSelect.Text = "Select Property";
            // 
            // countLbl
            // 
            this.countLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.countLbl.AutoSize = true;
            this.countLbl.Location = new System.Drawing.Point(49, 90);
            this.countLbl.Name = "countLbl";
            this.countLbl.Size = new System.Drawing.Size(13, 13);
            this.countLbl.TabIndex = 2;
            this.countLbl.Text = "0";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Count:";
            // 
            // propertyListView
            // 
            this.propertyListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader1});
            this.propertyListView.FullRowSelect = true;
            this.propertyListView.GridLines = true;
            this.propertyListView.HideSelection = false;
            this.propertyListView.Location = new System.Drawing.Point(3, 16);
            this.propertyListView.Margin = new System.Windows.Forms.Padding(0);
            this.propertyListView.Name = "propertyListView";
            this.propertyListView.Size = new System.Drawing.Size(711, 68);
            this.propertyListView.TabIndex = 0;
            this.propertyListView.UseCompatibleStateImageBehavior = false;
            this.propertyListView.View = System.Windows.Forms.View.Details;
            this.propertyListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.propertyListView_ItemSelectionChanged);
            this.propertyListView.SelectedIndexChanged += new System.EventHandler(this.propertyListView_SelectedIndexChanged);
            this.propertyListView.DoubleClick += new System.EventHandler(this.propertyListView_DoubleClick);
            this.propertyListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.propertyListView_KeyDown);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Id";
            this.columnHeader6.Width = 33;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Name";
            this.columnHeader7.Width = 147;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Type";
            this.columnHeader8.Width = 90;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Original Table";
            this.columnHeader9.Width = 109;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Original Column";
            this.columnHeader10.Width = 111;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Business Entity";
            this.columnHeader1.Width = 125;
            // 
            // gbService
            // 
            this.gbService.Controls.Add(this.mapTypeGb);
            this.gbService.Controls.Add(this.searchServiceBtn);
            this.gbService.Controls.Add(this.label7);
            this.gbService.Controls.Add(this.serviceCb);
            this.gbService.Controls.Add(this.serviceMethodCb);
            this.gbService.Controls.Add(this.label6);
            this.gbService.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbService.Location = new System.Drawing.Point(0, 165);
            this.gbService.Name = "gbService";
            this.gbService.Size = new System.Drawing.Size(720, 84);
            this.gbService.TabIndex = 2;
            this.gbService.TabStop = false;
            this.gbService.Text = "Search Fields";
            this.gbService.Visible = false;
            // 
            // mapTypeGb
            // 
            this.mapTypeGb.Controls.Add(this.rbRequestMap);
            this.mapTypeGb.Controls.Add(this.rbResponseMap);
            this.mapTypeGb.Location = new System.Drawing.Point(315, 10);
            this.mapTypeGb.Name = "mapTypeGb";
            this.mapTypeGb.Size = new System.Drawing.Size(213, 64);
            this.mapTypeGb.TabIndex = 4;
            this.mapTypeGb.TabStop = false;
            this.mapTypeGb.Text = "Map to Show";
            // 
            // rbRequestMap
            // 
            this.rbRequestMap.AutoSize = true;
            this.rbRequestMap.Location = new System.Drawing.Point(15, 41);
            this.rbRequestMap.Name = "rbRequestMap";
            this.rbRequestMap.Size = new System.Drawing.Size(65, 17);
            this.rbRequestMap.TabIndex = 1;
            this.rbRequestMap.Text = "Request";
            this.rbRequestMap.UseVisualStyleBackColor = true;
            this.rbRequestMap.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rbRequestMap_KeyDown);
            // 
            // rbResponseMap
            // 
            this.rbResponseMap.AutoSize = true;
            this.rbResponseMap.Checked = true;
            this.rbResponseMap.Location = new System.Drawing.Point(15, 19);
            this.rbResponseMap.Name = "rbResponseMap";
            this.rbResponseMap.Size = new System.Drawing.Size(73, 17);
            this.rbResponseMap.TabIndex = 0;
            this.rbResponseMap.TabStop = true;
            this.rbResponseMap.Text = "Response";
            this.rbResponseMap.UseVisualStyleBackColor = true;
            this.rbResponseMap.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rbResponseMap_KeyDown);
            // 
            // searchServiceBtn
            // 
            this.searchServiceBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.searchServiceBtn.Enabled = false;
            this.searchServiceBtn.Location = new System.Drawing.Point(617, 19);
            this.searchServiceBtn.Name = "searchServiceBtn";
            this.searchServiceBtn.Size = new System.Drawing.Size(96, 23);
            this.searchServiceBtn.TabIndex = 5;
            this.searchServiceBtn.Text = "Search";
            this.searchServiceBtn.UseVisualStyleBackColor = true;
            this.searchServiceBtn.Click += new System.EventHandler(this.searchServiceBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Service Method:";
            // 
            // serviceCb
            // 
            this.serviceCb.DataSource = this.serviceBindingSource;
            this.serviceCb.DisplayMember = "Name";
            this.serviceCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serviceCb.FormattingEnabled = true;
            this.serviceCb.Location = new System.Drawing.Point(97, 19);
            this.serviceCb.Name = "serviceCb";
            this.serviceCb.Size = new System.Drawing.Size(200, 21);
            this.serviceCb.TabIndex = 1;
            this.serviceCb.ValueMember = "Id";
            this.serviceCb.SelectedValueChanged += new System.EventHandler(this.serviceCb_SelectedValueChanged);
            this.serviceCb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.serviceCb_KeyDown);
            // 
            // serviceBindingSource
            // 
            this.serviceBindingSource.DataSource = typeof(Cdc.MetaManager.DataAccess.Domain.Service);
            // 
            // serviceMethodCb
            // 
            this.serviceMethodCb.DataSource = this.serviceMethodBindingSource;
            this.serviceMethodCb.DisplayMember = "Name";
            this.serviceMethodCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serviceMethodCb.FormattingEnabled = true;
            this.serviceMethodCb.Location = new System.Drawing.Point(97, 46);
            this.serviceMethodCb.Name = "serviceMethodCb";
            this.serviceMethodCb.Size = new System.Drawing.Size(200, 21);
            this.serviceMethodCb.TabIndex = 3;
            this.serviceMethodCb.ValueMember = "Id";
            this.serviceMethodCb.SelectedValueChanged += new System.EventHandler(this.serviceMethodCb_SelectedValueChanged);
            this.serviceMethodCb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.serviceMethodCb_KeyDown);
            // 
            // serviceMethodBindingSource
            // 
            this.serviceMethodBindingSource.DataSource = typeof(Cdc.MetaManager.DataAccess.Domain.ServiceMethod);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Service:";
            // 
            // gbNameTableColBE
            // 
            this.gbNameTableColBE.Controls.Add(this.label4);
            this.gbNameTableColBE.Controls.Add(this.beTbx);
            this.gbNameTableColBE.Controls.Add(this.searchBtn);
            this.gbNameTableColBE.Controls.Add(this.label3);
            this.gbNameTableColBE.Controls.Add(this.columnTbx);
            this.gbNameTableColBE.Controls.Add(this.label2);
            this.gbNameTableColBE.Controls.Add(this.tableTbx);
            this.gbNameTableColBE.Controls.Add(this.label1);
            this.gbNameTableColBE.Controls.Add(this.nameTbx);
            this.gbNameTableColBE.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbNameTableColBE.Location = new System.Drawing.Point(0, 63);
            this.gbNameTableColBE.Name = "gbNameTableColBE";
            this.gbNameTableColBE.Size = new System.Drawing.Size(720, 102);
            this.gbNameTableColBE.TabIndex = 1;
            this.gbNameTableColBE.TabStop = false;
            this.gbNameTableColBE.Text = "Search Fields";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(257, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Business Entity:";
            // 
            // beTbx
            // 
            this.beTbx.Location = new System.Drawing.Point(344, 19);
            this.beTbx.Name = "beTbx";
            this.beTbx.Size = new System.Drawing.Size(144, 20);
            this.beTbx.TabIndex = 7;
            this.beTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.beTbx_KeyDown);
            // 
            // searchBtn
            // 
            this.searchBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.searchBtn.Location = new System.Drawing.Point(617, 12);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(96, 23);
            this.searchBtn.TabIndex = 8;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Column:";
            // 
            // columnTbx
            // 
            this.columnTbx.Location = new System.Drawing.Point(98, 71);
            this.columnTbx.Name = "columnTbx";
            this.columnTbx.Size = new System.Drawing.Size(144, 20);
            this.columnTbx.TabIndex = 5;
            this.columnTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.columnTbx_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Table:";
            // 
            // tableTbx
            // 
            this.tableTbx.Location = new System.Drawing.Point(98, 45);
            this.tableTbx.Name = "tableTbx";
            this.tableTbx.Size = new System.Drawing.Size(144, 20);
            this.tableTbx.TabIndex = 3;
            this.tableTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tableTbx_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // nameTbx
            // 
            this.nameTbx.Location = new System.Drawing.Point(98, 19);
            this.nameTbx.Name = "nameTbx";
            this.nameTbx.Size = new System.Drawing.Size(144, 20);
            this.nameTbx.TabIndex = 1;
            this.nameTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameTbx_KeyDown);
            // 
            // selectSearchfieldsGb
            // 
            this.selectSearchfieldsGb.Controls.Add(this.rbService);
            this.selectSearchfieldsGb.Controls.Add(this.rbNameTableColBE);
            this.selectSearchfieldsGb.Dock = System.Windows.Forms.DockStyle.Top;
            this.selectSearchfieldsGb.Location = new System.Drawing.Point(0, 0);
            this.selectSearchfieldsGb.Name = "selectSearchfieldsGb";
            this.selectSearchfieldsGb.Size = new System.Drawing.Size(720, 63);
            this.selectSearchfieldsGb.TabIndex = 0;
            this.selectSearchfieldsGb.TabStop = false;
            this.selectSearchfieldsGb.Text = "Find By";
            // 
            // rbService
            // 
            this.rbService.AutoSize = true;
            this.rbService.Location = new System.Drawing.Point(13, 38);
            this.rbService.Name = "rbService";
            this.rbService.Size = new System.Drawing.Size(160, 17);
            this.rbService.TabIndex = 1;
            this.rbService.Text = "Service and Service Method";
            this.rbService.UseVisualStyleBackColor = true;
            this.rbService.CheckedChanged += new System.EventHandler(this.rbService_CheckedChanged);
            // 
            // rbNameTableColBE
            // 
            this.rbNameTableColBE.AutoSize = true;
            this.rbNameTableColBE.Checked = true;
            this.rbNameTableColBE.Location = new System.Drawing.Point(13, 15);
            this.rbNameTableColBE.Name = "rbNameTableColBE";
            this.rbNameTableColBE.Size = new System.Drawing.Size(180, 17);
            this.rbNameTableColBE.TabIndex = 0;
            this.rbNameTableColBE.TabStop = true;
            this.rbNameTableColBE.Text = "Name, Table,  Column and Entity";
            this.rbNameTableColBE.UseVisualStyleBackColor = true;
            this.rbNameTableColBE.CheckedChanged += new System.EventHandler(this.rbNameTableColBE_CheckedChanged);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(6, 361);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(720, 6);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // propertyBindingSource
            // 
            this.propertyBindingSource.DataSource = typeof(Cdc.MetaManager.DataAccess.Domain.Property);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.hintBtn);
            this.panel2.Controls.Add(this.btnCreateProperty);
            this.panel2.Controls.Add(this.okBtn);
            this.panel2.Controls.Add(this.cancelBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(6, 599);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(720, 36);
            this.panel2.TabIndex = 3;
            // 
            // hintBtn
            // 
            this.hintBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hintBtn.Enabled = false;
            this.hintBtn.Location = new System.Drawing.Point(463, 7);
            this.hintBtn.Name = "hintBtn";
            this.hintBtn.Size = new System.Drawing.Size(92, 23);
            this.hintBtn.TabIndex = 4;
            this.hintBtn.Text = "Assign Hint...";
            this.hintBtn.UseVisualStyleBackColor = true;
            this.hintBtn.Click += new System.EventHandler(this.hintBtn_Click);
            // 
            // btnCreateProperty
            // 
            this.btnCreateProperty.Location = new System.Drawing.Point(309, 7);
            this.btnCreateProperty.Name = "btnCreateProperty";
            this.btnCreateProperty.Size = new System.Drawing.Size(148, 23);
            this.btnCreateProperty.TabIndex = 3;
            this.btnCreateProperty.Text = "Create Custom Property...";
            this.btnCreateProperty.UseVisualStyleBackColor = true;
            this.btnCreateProperty.Click += new System.EventHandler(this.btnCreateProperty_Click);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(561, 7);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 0;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(642, 7);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(6, 367);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(720, 232);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.propertyGrid);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(714, 199);
            this.panel4.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblInformation);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 215);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(714, 14);
            this.panel3.TabIndex = 1;
            // 
            // lblInformation
            // 
            this.lblInformation.AutoSize = true;
            this.lblInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformation.ForeColor = System.Drawing.Color.Green;
            this.lblInformation.Location = new System.Drawing.Point(-3, 1);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(31, 13);
            this.lblInformation.TabIndex = 1;
            this.lblInformation.Text = "XXX";
            // 
            // FindPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(732, 641);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "FindPropertyForm";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Property";
            this.Load += new System.EventHandler(this.FindPropertyForm_Load);
            this.panel1.ResumeLayout(false);
            this.gbSelect.ResumeLayout(false);
            this.gbSelect.PerformLayout();
            this.gbService.ResumeLayout(false);
            this.gbService.PerformLayout();
            this.mapTypeGb.ResumeLayout(false);
            this.mapTypeGb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.serviceMethodBindingSource)).EndInit();
            this.gbNameTableColBE.ResumeLayout(false);
            this.gbNameTableColBE.PerformLayout();
            this.selectSearchfieldsGb.ResumeLayout(false);
            this.selectSearchfieldsGb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource propertyBindingSource;
        private System.Windows.Forms.GroupBox selectSearchfieldsGb;
        private System.Windows.Forms.RadioButton rbService;
        private System.Windows.Forms.RadioButton rbNameTableColBE;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.GroupBox gbService;
        private ListViewSort propertyListView;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.GroupBox gbNameTableColBE;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox beTbx;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox columnTbx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tableTbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTbx;
        private System.Windows.Forms.GroupBox gbSelect;
        private System.Windows.Forms.Button searchServiceBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox serviceCb;
        private System.Windows.Forms.ComboBox serviceMethodCb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource serviceBindingSource;
        private System.Windows.Forms.BindingSource serviceMethodBindingSource;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label countLbl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox mapTypeGb;
        private System.Windows.Forms.RadioButton rbRequestMap;
        private System.Windows.Forms.RadioButton rbResponseMap;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.Button btnCreateProperty;
        private System.Windows.Forms.Button hintBtn;
    }
}