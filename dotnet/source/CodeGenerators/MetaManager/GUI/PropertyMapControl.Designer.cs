
namespace Cdc.MetaManager.GUI
{
    partial class PropertyMapControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.renameBtn = new System.Windows.Forms.Button();
            this.mapNewBtn = new System.Windows.Forms.Button();
            this.mapExistingBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.detailGrid = new System.Windows.Forms.PropertyGrid();
            this.PropertyListcontextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mapExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyListView = new Cdc.MetaManager.GUI.ListViewSort();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.PropertyListcontextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.renameBtn);
            this.groupBox2.Controls.Add(this.mapNewBtn);
            this.groupBox2.Controls.Add(this.mapExistingBtn);
            this.groupBox2.Controls.Add(this.propertyListView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(789, 183);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameter Map";
            // 
            // renameBtn
            // 
            this.renameBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.renameBtn.Location = new System.Drawing.Point(676, 74);
            this.renameBtn.Name = "renameBtn";
            this.renameBtn.Size = new System.Drawing.Size(104, 23);
            this.renameBtn.TabIndex = 9;
            this.renameBtn.Text = "&Rename...";
            this.renameBtn.UseVisualStyleBackColor = true;
            this.renameBtn.Click += new System.EventHandler(this.renameBtn_Click);
            // 
            // mapNewBtn
            // 
            this.mapNewBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mapNewBtn.Location = new System.Drawing.Point(676, 45);
            this.mapNewBtn.Name = "mapNewBtn";
            this.mapNewBtn.Size = new System.Drawing.Size(104, 23);
            this.mapNewBtn.TabIndex = 8;
            this.mapNewBtn.Text = "Map &New...";
            this.mapNewBtn.UseVisualStyleBackColor = true;
            this.mapNewBtn.Click += new System.EventHandler(this.mapNewBtn_Click);
            // 
            // mapExistingBtn
            // 
            this.mapExistingBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mapExistingBtn.Location = new System.Drawing.Point(676, 16);
            this.mapExistingBtn.Name = "mapExistingBtn";
            this.mapExistingBtn.Size = new System.Drawing.Size(104, 23);
            this.mapExistingBtn.TabIndex = 7;
            this.mapExistingBtn.Text = "Map &Existing...";
            this.mapExistingBtn.UseVisualStyleBackColor = true;
            this.mapExistingBtn.Click += new System.EventHandler(this.mapExistingBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(789, 183);
            this.panel2.TabIndex = 13;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 183);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(789, 6);
            this.splitter2.TabIndex = 15;
            this.splitter2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 189);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(789, 312);
            this.panel1.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.detailGrid);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(789, 312);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // detailGrid
            // 
            this.detailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailGrid.HelpVisible = false;
            this.detailGrid.Location = new System.Drawing.Point(6, 19);
            this.detailGrid.Name = "detailGrid";
            this.detailGrid.Size = new System.Drawing.Size(777, 287);
            this.detailGrid.TabIndex = 1;
            // 
            // PropertyListcontextMenu
            // 
            this.PropertyListcontextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapExistingToolStripMenuItem,
            this.mapNewToolStripMenuItem,
            this.renameToolStripMenuItem});
            this.PropertyListcontextMenu.Name = "PropertyListcontextMenu";
            this.PropertyListcontextMenu.Size = new System.Drawing.Size(142, 70);
            this.PropertyListcontextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.PropertyListcontextMenu_Opening);
            // 
            // mapExistingToolStripMenuItem
            // 
            this.mapExistingToolStripMenuItem.Name = "mapExistingToolStripMenuItem";
            this.mapExistingToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.mapExistingToolStripMenuItem.Text = "Map Existing";
            this.mapExistingToolStripMenuItem.Click += new System.EventHandler(this.mapExistingBtn_Click);
            // 
            // mapNewToolStripMenuItem
            // 
            this.mapNewToolStripMenuItem.Name = "mapNewToolStripMenuItem";
            this.mapNewToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.mapNewToolStripMenuItem.Text = "Map New";
            this.mapNewToolStripMenuItem.Click += new System.EventHandler(this.mapNewBtn_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameBtn_Click);
            // 
            // propertyListView
            // 
            this.propertyListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader4,
            this.columnHeader7,
            this.columnHeader6,
            this.columnHeader1});
            this.propertyListView.ContextMenuStrip = this.PropertyListcontextMenu;
            this.propertyListView.FullRowSelect = true;
            this.propertyListView.GridLines = true;
            this.propertyListView.HideSelection = false;
            this.propertyListView.Location = new System.Drawing.Point(9, 16);
            this.propertyListView.MultiSelect = false;
            this.propertyListView.Name = "propertyListView";
            this.propertyListView.Size = new System.Drawing.Size(661, 164);
            this.propertyListView.TabIndex = 6;
            this.propertyListView.UseCompatibleStateImageBehavior = false;
            this.propertyListView.View = System.Windows.Forms.View.Details;
            this.propertyListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.propertyListView_ItemSelectionChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Direction";
            this.columnHeader5.Width = 67;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Source Property";
            this.columnHeader4.Width = 163;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Source Type";
            this.columnHeader7.Width = 122;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Target Property";
            this.columnHeader6.Width = 182;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Target Type";
            this.columnHeader1.Width = 115;
            // 
            // PropertyMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel2);
            this.Name = "PropertyMapControl";
            this.Size = new System.Drawing.Size(789, 501);
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.PropertyListcontextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button renameBtn;
        private System.Windows.Forms.Button mapNewBtn;
        private System.Windows.Forms.Button mapExistingBtn;
        private ListViewSort propertyListView;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PropertyGrid detailGrid;
        private System.Windows.Forms.ContextMenuStrip PropertyListcontextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapExistingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;

    }
}
