
namespace Cdc.MetaManager.GUI
{
    partial class PropertyMapControl2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.sourceGrid = new System.Windows.Forms.PropertyGrid();
            this.pMulti = new System.Windows.Forms.Panel();
            this.btnMultiSetToChanged = new System.Windows.Forms.Button();
            this.btnMultiClearChangedList = new System.Windows.Forms.Button();
            this.btnSaveMulti = new System.Windows.Forms.Button();
            this.lblInformation = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.propertyListView = new Cdc.MetaManager.GUI.ListViewSort();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mapContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.defaultMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.requestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pMulti.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.mapContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.pMulti);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 285);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(848, 218);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.sourceGrid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(6, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(836, 158);
            this.panel2.TabIndex = 3;
            // 
            // sourceGrid
            // 
            this.sourceGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceGrid.HelpVisible = false;
            this.sourceGrid.Location = new System.Drawing.Point(0, 0);
            this.sourceGrid.Name = "sourceGrid";
            this.sourceGrid.Size = new System.Drawing.Size(836, 158);
            this.sourceGrid.TabIndex = 1;
            this.sourceGrid.ToolbarVisible = false;
            this.sourceGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.sourceGrid_PropertyValueChanged);
            this.sourceGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.sourceGrid_SelectedGridItemChanged);
            // 
            // pMulti
            // 
            this.pMulti.Controls.Add(this.btnMultiSetToChanged);
            this.pMulti.Controls.Add(this.btnMultiClearChangedList);
            this.pMulti.Controls.Add(this.btnSaveMulti);
            this.pMulti.Controls.Add(this.lblInformation);
            this.pMulti.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pMulti.Location = new System.Drawing.Point(6, 177);
            this.pMulti.Name = "pMulti";
            this.pMulti.Size = new System.Drawing.Size(836, 35);
            this.pMulti.TabIndex = 2;
            // 
            // btnMultiSetToChanged
            // 
            this.btnMultiSetToChanged.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMultiSetToChanged.Location = new System.Drawing.Point(648, 6);
            this.btnMultiSetToChanged.Name = "btnMultiSetToChanged";
            this.btnMultiSetToChanged.Size = new System.Drawing.Size(104, 23);
            this.btnMultiSetToChanged.TabIndex = 3;
            this.btnMultiSetToChanged.Text = "Set to Changed";
            this.btnMultiSetToChanged.UseVisualStyleBackColor = true;
            this.btnMultiSetToChanged.Visible = false;
            this.btnMultiSetToChanged.Click += new System.EventHandler(this.btnMultiSetToChanged_Click);
            // 
            // btnMultiClearChangedList
            // 
            this.btnMultiClearChangedList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMultiClearChangedList.Location = new System.Drawing.Point(533, 6);
            this.btnMultiClearChangedList.Name = "btnMultiClearChangedList";
            this.btnMultiClearChangedList.Size = new System.Drawing.Size(109, 23);
            this.btnMultiClearChangedList.TabIndex = 2;
            this.btnMultiClearChangedList.Text = "Clear Changed List";
            this.btnMultiClearChangedList.UseVisualStyleBackColor = true;
            this.btnMultiClearChangedList.Visible = false;
            this.btnMultiClearChangedList.Click += new System.EventHandler(this.btnMultiClearChangedList_Click);
            // 
            // btnSaveMulti
            // 
            this.btnSaveMulti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveMulti.Location = new System.Drawing.Point(758, 6);
            this.btnSaveMulti.Name = "btnSaveMulti";
            this.btnSaveMulti.Size = new System.Drawing.Size(75, 23);
            this.btnSaveMulti.TabIndex = 1;
            this.btnSaveMulti.Text = "Save";
            this.btnSaveMulti.UseVisualStyleBackColor = true;
            this.btnSaveMulti.Visible = false;
            this.btnSaveMulti.Click += new System.EventHandler(this.btnSaveMulti_Click);
            // 
            // lblInformation
            // 
            this.lblInformation.AutoSize = true;
            this.lblInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformation.ForeColor = System.Drawing.Color.Green;
            this.lblInformation.Location = new System.Drawing.Point(-2, 1);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(31, 13);
            this.lblInformation.TabIndex = 0;
            this.lblInformation.Text = "XXX";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.propertyListView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(848, 279);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Property Map (right click to map)";
            // 
            // propertyListView
            // 
            this.propertyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader7,
            this.columnHeader6,
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader5});
            this.propertyListView.ContextMenuStrip = this.mapContextMenu;
            this.propertyListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyListView.FullRowSelect = true;
            this.propertyListView.GridLines = true;
            this.propertyListView.HideSelection = false;
            this.propertyListView.Location = new System.Drawing.Point(6, 19);
            this.propertyListView.Name = "propertyListView";
            this.propertyListView.Size = new System.Drawing.Size(836, 254);
            this.propertyListView.TabIndex = 6;
            this.propertyListView.UseCompatibleStateImageBehavior = false;
            this.propertyListView.View = System.Windows.Forms.View.Details;
            this.propertyListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.propertyListView_ItemSelectionChanged);
            this.propertyListView.SelectedIndexChanged += new System.EventHandler(this.propertyListView_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Seq";
            this.columnHeader2.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Source Property";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Source Type";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Target Property";
            this.columnHeader6.Width = 150;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Target Type";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Default Value";
            this.columnHeader3.Width = 140;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Original Column";
            this.columnHeader5.Width = 120;
            // 
            // mapContextMenu
            // 
            this.mapContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultMenuItem,
            this.requestMenuItem,
            this.targetMenuItem});
            this.mapContextMenu.Name = "mapContextMenu";
            this.mapContextMenu.Size = new System.Drawing.Size(153, 92);
            this.mapContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.mapContextMenu_Opening);
            // 
            // defaultMenuItem
            // 
            this.defaultMenuItem.Name = "defaultMenuItem";
            this.defaultMenuItem.Size = new System.Drawing.Size(152, 22);
            this.defaultMenuItem.Text = "Default";
            // 
            // requestMenuItem
            // 
            this.requestMenuItem.Name = "requestMenuItem";
            this.requestMenuItem.Size = new System.Drawing.Size(152, 22);
            this.requestMenuItem.Text = "Request";
            // 
            // targetMenuItem
            // 
            this.targetMenuItem.Name = "targetMenuItem";
            this.targetMenuItem.Size = new System.Drawing.Size(152, 22);
            this.targetMenuItem.Text = "Target";
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 279);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(848, 6);
            this.splitter2.TabIndex = 14;
            this.splitter2.TabStop = false;
            // 
            // PropertyMapControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.groupBox1);
            this.Name = "PropertyMapControl2";
            this.Size = new System.Drawing.Size(848, 503);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pMulti.ResumeLayout(false);
            this.pMulti.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.mapContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PropertyGrid sourceGrid;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox groupBox2;
        private ListViewSort propertyListView;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ContextMenuStrip mapContextMenu;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripMenuItem defaultMenuItem;
        private System.Windows.Forms.ToolStripMenuItem requestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem targetMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pMulti;
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.Button btnSaveMulti;
        private System.Windows.Forms.Button btnMultiClearChangedList;
        private System.Windows.Forms.Button btnMultiSetToChanged;
    }
}
