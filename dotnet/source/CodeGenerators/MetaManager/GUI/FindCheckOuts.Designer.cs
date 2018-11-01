namespace Cdc.MetaManager.GUI
{
    partial class FindCheckOuts
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
            this.panel5 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.resultListView = new Cdc.MetaManager.GUI.ListViewSort();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ObjectListcontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.jumpToObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareWithLatestVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectAllcheckBox = new System.Windows.Forms.CheckBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.Usernamelabel = new System.Windows.Forms.Label();
            this.checkoutComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.undoCheckoutButton = new System.Windows.Forms.Button();
            this.CheckinButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.progressTextBox = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.ObjectListcontextMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.splitContainer1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(702, 533);
            this.panel5.TabIndex = 8;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(702, 533);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel7);
            this.groupBox1.Controls.Add(this.panel6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(702, 330);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Find Checkouts";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.resultListView);
            this.panel7.Controls.Add(this.panel1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 16);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(696, 259);
            this.panel7.TabIndex = 1;
            // 
            // resultListView
            // 
            this.resultListView.CheckBoxes = true;
            this.resultListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader6});
            this.resultListView.ContextMenuStrip = this.ObjectListcontextMenuStrip;
            this.resultListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultListView.FullRowSelect = true;
            this.resultListView.Location = new System.Drawing.Point(0, 68);
            this.resultListView.MultiSelect = false;
            this.resultListView.Name = "resultListView";
            this.resultListView.Size = new System.Drawing.Size(696, 191);
            this.resultListView.TabIndex = 2;
            this.resultListView.UseCompatibleStateImageBehavior = false;
            this.resultListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Application";
            this.columnHeader5.Width = 94;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "  Type";
            this.columnHeader4.Width = 210;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 237;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Checked out by";
            this.columnHeader3.Width = 93;
            // 
            // ObjectListcontextMenuStrip
            // 
            this.ObjectListcontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToObjectToolStripMenuItem,
            this.compareWithLatestVersionToolStripMenuItem});
            this.ObjectListcontextMenuStrip.Name = "ObjectListcontextMenuStrip";
            this.ObjectListcontextMenuStrip.Size = new System.Drawing.Size(248, 48);
            // 
            // jumpToObjectToolStripMenuItem
            // 
            this.jumpToObjectToolStripMenuItem.Name = "jumpToObjectToolStripMenuItem";
            this.jumpToObjectToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.jumpToObjectToolStripMenuItem.Text = "Jump to object...";
            this.jumpToObjectToolStripMenuItem.Click += new System.EventHandler(this.jumpToObjectToolStripMenuItem_Click);
            // 
            // compareWithLatestVersionToolStripMenuItem
            // 
            this.compareWithLatestVersionToolStripMenuItem.Name = "compareWithLatestVersionToolStripMenuItem";
            this.compareWithLatestVersionToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.compareWithLatestVersionToolStripMenuItem.Text = "Compare with previous version...";
            this.compareWithLatestVersionToolStripMenuItem.Click += new System.EventHandler(this.compareWithLatestVersionToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.selectAllcheckBox);
            this.panel1.Controls.Add(this.searchButton);
            this.panel1.Controls.Add(this.UserNameTextBox);
            this.panel1.Controls.Add(this.Usernamelabel);
            this.panel1.Controls.Add(this.checkoutComboBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(696, 68);
            this.panel1.TabIndex = 1;
            // 
            // selectAllcheckBox
            // 
            this.selectAllcheckBox.AutoSize = true;
            this.selectAllcheckBox.Location = new System.Drawing.Point(6, 48);
            this.selectAllcheckBox.Name = "selectAllcheckBox";
            this.selectAllcheckBox.Size = new System.Drawing.Size(69, 17);
            this.selectAllcheckBox.TabIndex = 10;
            this.selectAllcheckBox.Text = "Select all";
            this.selectAllcheckBox.UseVisualStyleBackColor = true;
            this.selectAllcheckBox.Click += new System.EventHandler(this.selectAllCheckBox_CheckedChanged);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(612, 8);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 9;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Location = new System.Drawing.Point(272, 15);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(102, 20);
            this.UserNameTextBox.TabIndex = 8;
            this.UserNameTextBox.Visible = false;
            // 
            // Usernamelabel
            // 
            this.Usernamelabel.AutoSize = true;
            this.Usernamelabel.Location = new System.Drawing.Point(211, 18);
            this.Usernamelabel.Name = "Usernamelabel";
            this.Usernamelabel.Size = new System.Drawing.Size(55, 13);
            this.Usernamelabel.TabIndex = 7;
            this.Usernamelabel.Text = "Username";
            this.Usernamelabel.Visible = false;
            // 
            // checkoutComboBox
            // 
            this.checkoutComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.checkoutComboBox.FormattingEnabled = true;
            this.checkoutComboBox.Items.AddRange(new object[] {
            "My",
            "All",
            "Users"});
            this.checkoutComboBox.Location = new System.Drawing.Point(84, 15);
            this.checkoutComboBox.Name = "checkoutComboBox";
            this.checkoutComboBox.Size = new System.Drawing.Size(121, 21);
            this.checkoutComboBox.TabIndex = 6;
            this.checkoutComboBox.SelectedIndexChanged += new System.EventHandler(this.checkOutscomboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Checkouts";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.undoCheckoutButton);
            this.panel6.Controls.Add(this.CheckinButton);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(3, 275);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(696, 52);
            this.panel6.TabIndex = 0;
            // 
            // undoCheckoutButton
            // 
            this.undoCheckoutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.undoCheckoutButton.Location = new System.Drawing.Point(612, 6);
            this.undoCheckoutButton.Name = "undoCheckoutButton";
            this.undoCheckoutButton.Size = new System.Drawing.Size(75, 23);
            this.undoCheckoutButton.TabIndex = 1;
            this.undoCheckoutButton.Text = "Undo Checkout";
            this.undoCheckoutButton.UseVisualStyleBackColor = true;
            this.undoCheckoutButton.Click += new System.EventHandler(this.undoCheckoutButton_Click);
            // 
            // CheckinButton
            // 
            this.CheckinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckinButton.Location = new System.Drawing.Point(531, 6);
            this.CheckinButton.Name = "CheckinButton";
            this.CheckinButton.Size = new System.Drawing.Size(75, 23);
            this.CheckinButton.TabIndex = 0;
            this.CheckinButton.Text = "Check In";
            this.CheckinButton.UseVisualStyleBackColor = true;
            this.CheckinButton.Click += new System.EventHandler(this.checkinButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel9);
            this.groupBox2.Controls.Add(this.panel8);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(702, 199);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Progress";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.progressTextBox);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 46);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(696, 150);
            this.panel9.TabIndex = 6;
            // 
            // progressTextBox
            // 
            this.progressTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressTextBox.Location = new System.Drawing.Point(0, 0);
            this.progressTextBox.Multiline = true;
            this.progressTextBox.Name = "progressTextBox";
            this.progressTextBox.ReadOnly = true;
            this.progressTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.progressTextBox.Size = new System.Drawing.Size(696, 150);
            this.progressTextBox.TabIndex = 4;
            this.progressTextBox.WordWrap = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.progressBar);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 16);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(696, 30);
            this.panel8.TabIndex = 5;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(0, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(696, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 5;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "New";
            // 
            // FindCheckOuts
            // 
            this.AcceptButton = this.searchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 533);
            this.Controls.Add(this.panel5);
            this.Name = "FindCheckOuts";
            this.Text = "Find Checkouts";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel5.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ObjectListcontextMenuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button CheckinButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.TextBox progressTextBox;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel1;
        private ListViewSort resultListView;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label Usernamelabel;
        private System.Windows.Forms.ComboBox checkoutComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox selectAllcheckBox;
        private System.Windows.Forms.Button undoCheckoutButton;
        private System.Windows.Forms.ContextMenuStrip ObjectListcontextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem jumpToObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareWithLatestVersionToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader6;

    }
}