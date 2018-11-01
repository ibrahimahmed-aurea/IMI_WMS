namespace Cdc.MetaManager.GUI
{
    partial class CreateEditActionWizard
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.targetPage = new System.Windows.Forms.TabPage();
            this.ObjectPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.targetQueryBtn = new System.Windows.Forms.RadioButton();
            this.targetProcedureBtn = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tbFormatText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.entityCbx = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.actionNameTb = new System.Windows.Forms.TextBox();
            this.queryPage = new System.Windows.Forms.TabPage();
            this.queryControl = new Cdc.MetaManager.GUI.QueryControl();
            this.procedurePage = new System.Windows.Forms.TabPage();
            this.storedProcedureControl = new Cdc.MetaManager.GUI.StoredProcedureControl();
            this.paramPage = new System.Windows.Forms.TabPage();
            this.propertyMapControl = new Cdc.MetaManager.GUI.PropertyMapControl();
            this.advancedPage = new System.Windows.Forms.TabPage();
            this.ServiceMethodgroupBox = new System.Windows.Forms.GroupBox();
            this.cbCreateInService = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbNewServiceMethod = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.CommitTransactionCb = new System.Windows.Forms.CheckBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.MessageHandlingCb = new System.Windows.Forms.CheckBox();
            this.MultiSelectCb = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.RowTrackingIdTbx = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.finishBtn = new System.Windows.Forms.Button();
            this.nextBtn = new System.Windows.Forms.Button();
            this.prevBtn = new System.Windows.Forms.Button();
            this.fileBrowseDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl.SuspendLayout();
            this.targetPage.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.queryPage.SuspendLayout();
            this.procedurePage.SuspendLayout();
            this.paramPage.SuspendLayout();
            this.advancedPage.SuspendLayout();
            this.ServiceMethodgroupBox.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.targetPage);
            this.tabControl.Controls.Add(this.queryPage);
            this.tabControl.Controls.Add(this.procedurePage);
            this.tabControl.Controls.Add(this.paramPage);
            this.tabControl.Controls.Add(this.advancedPage);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(975, 586);
            this.tabControl.TabIndex = 9;
            // 
            // targetPage
            // 
            this.targetPage.Controls.Add(this.ObjectPropertyGrid);
            this.targetPage.Controls.Add(this.groupBox6);
            this.targetPage.Controls.Add(this.groupBox5);
            this.targetPage.Location = new System.Drawing.Point(4, 22);
            this.targetPage.Name = "targetPage";
            this.targetPage.Padding = new System.Windows.Forms.Padding(3);
            this.targetPage.Size = new System.Drawing.Size(967, 560);
            this.targetPage.TabIndex = 0;
            this.targetPage.Text = "Action";
            this.targetPage.UseVisualStyleBackColor = true;
            this.targetPage.ParentChanged += new System.EventHandler(this.targetPage_ParentChanged);
            // 
            // ObjectPropertyGrid
            // 
            this.ObjectPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectPropertyGrid.ContextMenuStrip = this.PropertyGridcontextMenuStrip;
            this.ObjectPropertyGrid.Location = new System.Drawing.Point(8, 160);
            this.ObjectPropertyGrid.Name = "ObjectPropertyGrid";
            this.ObjectPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.ObjectPropertyGrid.Size = new System.Drawing.Size(953, 394);
            this.ObjectPropertyGrid.TabIndex = 8;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.targetQueryBtn);
            this.groupBox6.Controls.Add(this.targetProcedureBtn);
            this.groupBox6.Location = new System.Drawing.Point(778, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(183, 145);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Type";
            // 
            // targetQueryBtn
            // 
            this.targetQueryBtn.AutoSize = true;
            this.targetQueryBtn.Location = new System.Drawing.Point(6, 42);
            this.targetQueryBtn.Name = "targetQueryBtn";
            this.targetQueryBtn.Size = new System.Drawing.Size(77, 17);
            this.targetQueryBtn.TabIndex = 1;
            this.targetQueryBtn.TabStop = true;
            this.targetQueryBtn.Text = "SQL Query";
            this.targetQueryBtn.UseVisualStyleBackColor = true;
            this.targetQueryBtn.Click += new System.EventHandler(this.targetQueryBtn_Click_1);
            // 
            // targetProcedureBtn
            // 
            this.targetProcedureBtn.AutoSize = true;
            this.targetProcedureBtn.Location = new System.Drawing.Point(6, 19);
            this.targetProcedureBtn.Name = "targetProcedureBtn";
            this.targetProcedureBtn.Size = new System.Drawing.Size(108, 17);
            this.targetProcedureBtn.TabIndex = 0;
            this.targetProcedureBtn.TabStop = true;
            this.targetProcedureBtn.Text = "Stored Procedure";
            this.targetProcedureBtn.UseVisualStyleBackColor = true;
            this.targetProcedureBtn.Click += new System.EventHandler(this.targetProcedureBtn_Click_1);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.tbFormatText);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.entityCbx);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.actionNameTb);
            this.groupBox5.Location = new System.Drawing.Point(8, 6);
            this.groupBox5.MinimumSize = new System.Drawing.Size(764, 146);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(764, 146);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "General";
            // 
            // tbFormatText
            // 
            this.tbFormatText.BackColor = System.Drawing.Color.White;
            this.tbFormatText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFormatText.Location = new System.Drawing.Point(164, 77);
            this.tbFormatText.Multiline = true;
            this.tbFormatText.Name = "tbFormatText";
            this.tbFormatText.ReadOnly = true;
            this.tbFormatText.Size = new System.Drawing.Size(290, 49);
            this.tbFormatText.TabIndex = 5;
            this.tbFormatText.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(116, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Format:";
            // 
            // entityCbx
            // 
            this.entityCbx.DisplayMember = "Name";
            this.entityCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.entityCbx.FormattingEnabled = true;
            this.entityCbx.Location = new System.Drawing.Point(119, 24);
            this.entityCbx.Name = "entityCbx";
            this.entityCbx.Size = new System.Drawing.Size(335, 21);
            this.entityCbx.TabIndex = 1;
            this.entityCbx.SelectedIndexChanged += new System.EventHandler(this.entityCbx_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Business Entity:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Name:";
            // 
            // actionNameTb
            // 
            this.actionNameTb.Location = new System.Drawing.Point(119, 51);
            this.actionNameTb.Name = "actionNameTb";
            this.actionNameTb.Size = new System.Drawing.Size(335, 20);
            this.actionNameTb.TabIndex = 3;
            this.actionNameTb.TextChanged += new System.EventHandler(this.actionNameTb_TextChanged);
            // 
            // queryPage
            // 
            this.queryPage.Controls.Add(this.queryControl);
            this.queryPage.Location = new System.Drawing.Point(4, 22);
            this.queryPage.Name = "queryPage";
            this.queryPage.Padding = new System.Windows.Forms.Padding(3);
            this.queryPage.Size = new System.Drawing.Size(967, 560);
            this.queryPage.TabIndex = 4;
            this.queryPage.Text = "Query";
            this.queryPage.UseVisualStyleBackColor = true;
            this.queryPage.ParentChanged += new System.EventHandler(this.queryPage_ParentChanged);
            // 
            // queryControl
            // 
            this.queryControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryControl.Location = new System.Drawing.Point(3, 3);
            this.queryControl.Name = "queryControl";
            this.queryControl.Query = null;
            this.queryControl.Size = new System.Drawing.Size(961, 554);
            this.queryControl.TabIndex = 0;
            // 
            // procedurePage
            // 
            this.procedurePage.Controls.Add(this.storedProcedureControl);
            this.procedurePage.Location = new System.Drawing.Point(4, 22);
            this.procedurePage.Name = "procedurePage";
            this.procedurePage.Padding = new System.Windows.Forms.Padding(3);
            this.procedurePage.Size = new System.Drawing.Size(967, 560);
            this.procedurePage.TabIndex = 1;
            this.procedurePage.Text = "Stored Procedure";
            this.procedurePage.UseVisualStyleBackColor = true;
            this.procedurePage.ParentChanged += new System.EventHandler(this.procedurePage_ParentChanged);
            // 
            // storedProcedureControl
            // 
            this.storedProcedureControl.BackendApplication = null;
            this.storedProcedureControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.storedProcedureControl.Location = new System.Drawing.Point(3, 3);
            this.storedProcedureControl.Name = "storedProcedureControl";
            this.storedProcedureControl.SelectStoredProcedure = null;
            this.storedProcedureControl.Size = new System.Drawing.Size(961, 554);
            this.storedProcedureControl.TabIndex = 22;
            // 
            // paramPage
            // 
            this.paramPage.Controls.Add(this.propertyMapControl);
            this.paramPage.Location = new System.Drawing.Point(4, 22);
            this.paramPage.Name = "paramPage";
            this.paramPage.Padding = new System.Windows.Forms.Padding(3);
            this.paramPage.Size = new System.Drawing.Size(967, 560);
            this.paramPage.TabIndex = 5;
            this.paramPage.Text = "Parameter Map";
            this.paramPage.UseVisualStyleBackColor = true;
            this.paramPage.ParentChanged += new System.EventHandler(this.paramPage_ParentChanged);
            // 
            // propertyMapControl
            // 
            this.propertyMapControl.BackendApplication = null;
            this.propertyMapControl.BusinessEntity = null;
            this.propertyMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyMapControl.IsEditable = false;
            this.propertyMapControl.Location = new System.Drawing.Point(3, 3);
            this.propertyMapControl.MappableProperties = null;
            this.propertyMapControl.Name = "propertyMapControl";
            this.propertyMapControl.RequestMap = null;
            this.propertyMapControl.ResponseMap = null;
            this.propertyMapControl.Size = new System.Drawing.Size(961, 554);
            this.propertyMapControl.TabIndex = 0;
            // 
            // advancedPage
            // 
            this.advancedPage.Controls.Add(this.ServiceMethodgroupBox);
            this.advancedPage.Controls.Add(this.groupBox9);
            this.advancedPage.Location = new System.Drawing.Point(4, 22);
            this.advancedPage.Name = "advancedPage";
            this.advancedPage.Padding = new System.Windows.Forms.Padding(3);
            this.advancedPage.Size = new System.Drawing.Size(967, 560);
            this.advancedPage.TabIndex = 3;
            this.advancedPage.Text = "Advanced Settings";
            this.advancedPage.UseVisualStyleBackColor = true;
            this.advancedPage.ParentChanged += new System.EventHandler(this.advancedPage_ParentChanged);
            // 
            // ServiceMethodgroupBox
            // 
            this.ServiceMethodgroupBox.Controls.Add(this.cbCreateInService);
            this.ServiceMethodgroupBox.Controls.Add(this.label1);
            this.ServiceMethodgroupBox.Controls.Add(this.cbNewServiceMethod);
            this.ServiceMethodgroupBox.Location = new System.Drawing.Point(8, 295);
            this.ServiceMethodgroupBox.Name = "ServiceMethodgroupBox";
            this.ServiceMethodgroupBox.Size = new System.Drawing.Size(515, 83);
            this.ServiceMethodgroupBox.TabIndex = 4;
            this.ServiceMethodgroupBox.TabStop = false;
            this.ServiceMethodgroupBox.Text = "Service Method";
            // 
            // cbCreateInService
            // 
            this.cbCreateInService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCreateInService.FormattingEnabled = true;
            this.cbCreateInService.Location = new System.Drawing.Point(103, 49);
            this.cbCreateInService.Name = "cbCreateInService";
            this.cbCreateInService.Size = new System.Drawing.Size(401, 21);
            this.cbCreateInService.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Create in Service:";
            // 
            // cbNewServiceMethod
            // 
            this.cbNewServiceMethod.AutoSize = true;
            this.cbNewServiceMethod.Checked = true;
            this.cbNewServiceMethod.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNewServiceMethod.Location = new System.Drawing.Point(9, 19);
            this.cbNewServiceMethod.Name = "cbNewServiceMethod";
            this.cbNewServiceMethod.Size = new System.Drawing.Size(234, 17);
            this.cbNewServiceMethod.TabIndex = 0;
            this.cbNewServiceMethod.Text = "Create a new Service Method for this Action";
            this.cbNewServiceMethod.UseVisualStyleBackColor = true;
            this.cbNewServiceMethod.CheckedChanged += new System.EventHandler(this.CreateServiceMethodcheckBox_CheckedChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.richTextBox3);
            this.groupBox9.Controls.Add(this.CommitTransactionCb);
            this.groupBox9.Controls.Add(this.richTextBox2);
            this.groupBox9.Controls.Add(this.MessageHandlingCb);
            this.groupBox9.Controls.Add(this.MultiSelectCb);
            this.groupBox9.Controls.Add(this.label11);
            this.groupBox9.Controls.Add(this.label10);
            this.groupBox9.Controls.Add(this.label9);
            this.groupBox9.Controls.Add(this.label7);
            this.groupBox9.Controls.Add(this.label8);
            this.groupBox9.Controls.Add(this.RowTrackingIdTbx);
            this.groupBox9.Location = new System.Drawing.Point(8, 6);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(515, 283);
            this.groupBox9.TabIndex = 3;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Advanced Action Settings";
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox3.Enabled = false;
            this.richTextBox3.ForeColor = System.Drawing.Color.Blue;
            this.richTextBox3.Location = new System.Drawing.Point(144, 189);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(360, 69);
            this.richTextBox3.TabIndex = 10;
            this.richTextBox3.Text = "This only applies to Actions connected to a stored procedure that has a Ref Curso" +
    "r as a parameter.\n\nAll other stored procedures will be commited as usuall no mat" +
    "ter what this flag is set to!";
            // 
            // CommitTransactionCb
            // 
            this.CommitTransactionCb.AutoSize = true;
            this.CommitTransactionCb.Location = new System.Drawing.Point(119, 170);
            this.CommitTransactionCb.Name = "CommitTransactionCb";
            this.CommitTransactionCb.Size = new System.Drawing.Size(178, 17);
            this.CommitTransactionCb.TabIndex = 9;
            this.CommitTransactionCb.Text = "Commit Transaction (Ref Cursor)";
            this.CommitTransactionCb.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Enabled = false;
            this.richTextBox2.ForeColor = System.Drawing.Color.Blue;
            this.richTextBox2.Location = new System.Drawing.Point(144, 152);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(360, 16);
            this.richTextBox2.TabIndex = 8;
            this.richTextBox2.Text = "Set this to handle Error, Warning and Information messages.";
            // 
            // MessageHandlingCb
            // 
            this.MessageHandlingCb.AutoSize = true;
            this.MessageHandlingCb.Location = new System.Drawing.Point(119, 135);
            this.MessageHandlingCb.Name = "MessageHandlingCb";
            this.MessageHandlingCb.Size = new System.Drawing.Size(156, 17);
            this.MessageHandlingCb.TabIndex = 7;
            this.MessageHandlingCb.Text = "Message Handling Enabled";
            this.MessageHandlingCb.UseVisualStyleBackColor = true;
            // 
            // MultiSelectCb
            // 
            this.MultiSelectCb.AutoSize = true;
            this.MultiSelectCb.Location = new System.Drawing.Point(119, 114);
            this.MultiSelectCb.Name = "MultiSelectCb";
            this.MultiSelectCb.Size = new System.Drawing.Size(81, 17);
            this.MultiSelectCb.TabIndex = 6;
            this.MultiSelectCb.Text = "Multi Select";
            this.MultiSelectCb.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(116, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(367, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "and stored procedure that create new records (typcially used in New diaslog)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(116, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(370, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Enter the name or the main table for the query or the \"New\" stored procedure";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(116, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(249, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "There is no overhead for defining it, only extra code";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(116, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(388, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Only needed in queries that are used in a grid View that has a Create/New dialog";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Row Tracking Id:";
            // 
            // RowTrackingIdTbx
            // 
            this.RowTrackingIdTbx.Location = new System.Drawing.Point(119, 19);
            this.RowTrackingIdTbx.Name = "RowTrackingIdTbx";
            this.RowTrackingIdTbx.Size = new System.Drawing.Size(192, 20);
            this.RowTrackingIdTbx.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.finishBtn);
            this.panel1.Controls.Add(this.nextBtn);
            this.panel1.Controls.Add(this.prevBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 592);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(975, 47);
            this.panel1.TabIndex = 10;
            // 
            // finishBtn
            // 
            this.finishBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.finishBtn.Enabled = false;
            this.finishBtn.Location = new System.Drawing.Point(872, 15);
            this.finishBtn.Name = "finishBtn";
            this.finishBtn.Size = new System.Drawing.Size(84, 23);
            this.finishBtn.TabIndex = 2;
            this.finishBtn.Text = "Finish";
            this.finishBtn.UseVisualStyleBackColor = true;
            this.finishBtn.Click += new System.EventHandler(this.finishBtn_Click);
            // 
            // nextBtn
            // 
            this.nextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nextBtn.Location = new System.Drawing.Point(782, 15);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(84, 23);
            this.nextBtn.TabIndex = 1;
            this.nextBtn.Text = "Next";
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // prevBtn
            // 
            this.prevBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prevBtn.Location = new System.Drawing.Point(692, 15);
            this.prevBtn.Name = "prevBtn";
            this.prevBtn.Size = new System.Drawing.Size(84, 23);
            this.prevBtn.TabIndex = 0;
            this.prevBtn.Text = "Prev";
            this.prevBtn.UseVisualStyleBackColor = true;
            this.prevBtn.Click += new System.EventHandler(this.prevBtn_Click);
            // 
            // fileBrowseDialog
            // 
            this.fileBrowseDialog.Filter = "Spec Files (*.spec)|*.spec";
            // 
            // CreateEditActionWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 639);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl);
            this.Name = "CreateEditActionWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddActionWizard";
            this.Load += new System.EventHandler(this.AddActionWizard_Load);
            this.tabControl.ResumeLayout(false);
            this.targetPage.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.queryPage.ResumeLayout(false);
            this.procedurePage.ResumeLayout(false);
            this.paramPage.ResumeLayout(false);
            this.advancedPage.ResumeLayout(false);
            this.ServiceMethodgroupBox.ResumeLayout(false);
            this.ServiceMethodgroupBox.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage targetPage;
        private System.Windows.Forms.TabPage queryPage;        
        private System.Windows.Forms.TabPage advancedPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button finishBtn;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.Button prevBtn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton targetQueryBtn;
        private System.Windows.Forms.RadioButton targetProcedureBtn;
        private System.Windows.Forms.TextBox tbFormatText;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox entityCbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox actionNameTb;
        private System.Windows.Forms.TabPage paramPage;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.CheckBox CommitTransactionCb;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.CheckBox MessageHandlingCb;
        private System.Windows.Forms.CheckBox MultiSelectCb;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox RowTrackingIdTbx;
        private System.Windows.Forms.OpenFileDialog fileBrowseDialog;
        private System.Windows.Forms.TabPage procedurePage;
        private QueryControl queryControl;
        private PropertyMapControl propertyMapControl;
        private StoredProcedureControl storedProcedureControl;
        private System.Windows.Forms.GroupBox ServiceMethodgroupBox;
        private System.Windows.Forms.ComboBox cbCreateInService;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbNewServiceMethod;
        private System.Windows.Forms.PropertyGrid ObjectPropertyGrid;
        //private PropertyMapControl propertyMapControl1;

    }
}