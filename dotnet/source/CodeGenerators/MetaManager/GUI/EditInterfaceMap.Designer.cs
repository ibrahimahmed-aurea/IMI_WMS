
namespace Cdc.MetaManager.GUI
{
    partial class EditInterfaceMap
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tcMaps = new System.Windows.Forms.TabControl();
            this.tpRequest = new System.Windows.Forms.TabPage();
            this.lvRequest = new ListViewSort();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.requestContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiRequestEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRequestAddCustomField = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRequestRemoveCustomField = new System.Windows.Forms.ToolStripMenuItem();
            this.tpResponse = new System.Windows.Forms.TabPage();
            this.lvResponse = new ListViewSort();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.responseContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiResponseEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCustomField = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiResponseAddCustomField = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiResponseRemoveCustomField = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiResponseMapFromRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tcMaps.SuspendLayout();
            this.tpRequest.SuspendLayout();
            this.requestContextMenu.SuspendLayout();
            this.tpResponse.SuspendLayout();
            this.responseContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 440);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(805, 36);
            this.panel1.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(645, 7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(726, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tcMaps);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(805, 437);
            this.panel2.TabIndex = 1;
            // 
            // tcMaps
            // 
            this.tcMaps.Controls.Add(this.tpRequest);
            this.tcMaps.Controls.Add(this.tpResponse);
            this.tcMaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMaps.Location = new System.Drawing.Point(0, 0);
            this.tcMaps.Name = "tcMaps";
            this.tcMaps.SelectedIndex = 0;
            this.tcMaps.Size = new System.Drawing.Size(805, 437);
            this.tcMaps.TabIndex = 0;
            // 
            // tpRequest
            // 
            this.tpRequest.Controls.Add(this.lvRequest);
            this.tpRequest.Location = new System.Drawing.Point(4, 22);
            this.tpRequest.Name = "tpRequest";
            this.tpRequest.Padding = new System.Windows.Forms.Padding(3);
            this.tpRequest.Size = new System.Drawing.Size(797, 411);
            this.tpRequest.TabIndex = 0;
            this.tpRequest.Text = "Request";
            this.tpRequest.UseVisualStyleBackColor = true;
            // 
            // lvRequest
            // 
            this.lvRequest.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader16});
            this.lvRequest.ContextMenuStrip = this.requestContextMenu;
            this.lvRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRequest.FullRowSelect = true;
            this.lvRequest.GridLines = true;
            this.lvRequest.HideSelection = false;
            this.lvRequest.Location = new System.Drawing.Point(3, 3);
            this.lvRequest.Name = "lvRequest";
            this.lvRequest.Size = new System.Drawing.Size(791, 405);
            this.lvRequest.TabIndex = 0;
            this.lvRequest.UseCompatibleStateImageBehavior = false;
            this.lvRequest.View = System.Windows.Forms.View.Details;
            this.lvRequest.DoubleClick += new System.EventHandler(this.lvRequest_DoubleClick);
            this.lvRequest.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvRequest_KeyUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Seq";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 201;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Datatype";
            this.columnHeader4.Width = 93;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Default Value";
            this.columnHeader5.Width = 114;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Searchable";
            this.columnHeader6.Width = 80;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Custom";
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Db Reference";
            this.columnHeader16.Width = 96;
            // 
            // requestContextMenu
            // 
            this.requestContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRequestEdit,
            this.toolStripMenuItem1});
            this.requestContextMenu.Name = "requestContextMenu";
            this.requestContextMenu.Size = new System.Drawing.Size(145, 48);
            this.requestContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.requestContextMenu_Opening);
            // 
            // tsmiRequestEdit
            // 
            this.tsmiRequestEdit.Name = "tsmiRequestEdit";
            this.tsmiRequestEdit.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.tsmiRequestEdit.Size = new System.Drawing.Size(144, 22);
            this.tsmiRequestEdit.Text = "Edit";
            this.tsmiRequestEdit.Click += new System.EventHandler(this.tsmiRequestEdit_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRequestAddCustomField,
            this.tsmiRequestRemoveCustomField});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItem1.Text = "Custom Field";
            // 
            // tsmiRequestAddCustomField
            // 
            this.tsmiRequestAddCustomField.Name = "tsmiRequestAddCustomField";
            this.tsmiRequestAddCustomField.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.tsmiRequestAddCustomField.Size = new System.Drawing.Size(141, 22);
            this.tsmiRequestAddCustomField.Text = "Add";
            this.tsmiRequestAddCustomField.Click += new System.EventHandler(this.tsmiRequestAddCustomField_Click);
            // 
            // tsmiRequestRemoveCustomField
            // 
            this.tsmiRequestRemoveCustomField.Name = "tsmiRequestRemoveCustomField";
            this.tsmiRequestRemoveCustomField.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.tsmiRequestRemoveCustomField.Size = new System.Drawing.Size(141, 22);
            this.tsmiRequestRemoveCustomField.Text = "Remove";
            this.tsmiRequestRemoveCustomField.Click += new System.EventHandler(this.tsmiRequestRemoveCustomField_Click);
            // 
            // tpResponse
            // 
            this.tpResponse.Controls.Add(this.lvResponse);
            this.tpResponse.Location = new System.Drawing.Point(4, 22);
            this.tpResponse.Name = "tpResponse";
            this.tpResponse.Padding = new System.Windows.Forms.Padding(3);
            this.tpResponse.Size = new System.Drawing.Size(797, 411);
            this.tpResponse.TabIndex = 1;
            this.tpResponse.Text = "Response";
            this.tpResponse.UseVisualStyleBackColor = true;
            // 
            // lvResponse
            // 
            this.lvResponse.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader15,
            this.columnHeader14,
            this.columnHeader17});
            this.lvResponse.ContextMenuStrip = this.responseContextMenu;
            this.lvResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvResponse.FullRowSelect = true;
            this.lvResponse.GridLines = true;
            this.lvResponse.Location = new System.Drawing.Point(3, 3);
            this.lvResponse.Name = "lvResponse";
            this.lvResponse.Size = new System.Drawing.Size(791, 405);
            this.lvResponse.TabIndex = 1;
            this.lvResponse.UseCompatibleStateImageBehavior = false;
            this.lvResponse.View = System.Windows.Forms.View.Details;
            this.lvResponse.DoubleClick += new System.EventHandler(this.lvResponse_DoubleClick);
            this.lvResponse.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvResponse_KeyUp);
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Id";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Seq";
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Name";
            this.columnHeader10.Width = 190;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Datatype";
            this.columnHeader11.Width = 93;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Default Value";
            this.columnHeader12.Width = 114;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Display Format";
            this.columnHeader13.Width = 107;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Custom";
            this.columnHeader15.Width = 52;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Request Field";
            this.columnHeader14.Width = 91;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Db Reference";
            // 
            // responseContextMenu
            // 
            this.responseContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiResponseEdit,
            this.tsmiCustomField,
            this.tsmiResponseMapFromRequest});
            this.responseContextMenu.Name = "requestContextMenu";
            this.responseContextMenu.Size = new System.Drawing.Size(201, 70);
            this.responseContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.responseContextMenu_Opening);
            // 
            // tsmiResponseEdit
            // 
            this.tsmiResponseEdit.Name = "tsmiResponseEdit";
            this.tsmiResponseEdit.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.tsmiResponseEdit.Size = new System.Drawing.Size(200, 22);
            this.tsmiResponseEdit.Text = "Edit";
            this.tsmiResponseEdit.Click += new System.EventHandler(this.tsmiResponseEdit_Click);
            // 
            // tsmiCustomField
            // 
            this.tsmiCustomField.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiResponseAddCustomField,
            this.tsmiResponseRemoveCustomField});
            this.tsmiCustomField.Name = "tsmiCustomField";
            this.tsmiCustomField.Size = new System.Drawing.Size(200, 22);
            this.tsmiCustomField.Text = "Custom Field";
            // 
            // tsmiResponseAddCustomField
            // 
            this.tsmiResponseAddCustomField.Name = "tsmiResponseAddCustomField";
            this.tsmiResponseAddCustomField.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.tsmiResponseAddCustomField.Size = new System.Drawing.Size(141, 22);
            this.tsmiResponseAddCustomField.Text = "Add";
            this.tsmiResponseAddCustomField.Click += new System.EventHandler(this.tsmiResponseAddCustomField_Click);
            // 
            // tsmiResponseRemoveCustomField
            // 
            this.tsmiResponseRemoveCustomField.Name = "tsmiResponseRemoveCustomField";
            this.tsmiResponseRemoveCustomField.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.tsmiResponseRemoveCustomField.Size = new System.Drawing.Size(141, 22);
            this.tsmiResponseRemoveCustomField.Text = "Remove";
            this.tsmiResponseRemoveCustomField.Click += new System.EventHandler(this.tsmiResponseRemoveCustomField_Click);
            // 
            // tsmiResponseMapFromRequest
            // 
            this.tsmiResponseMapFromRequest.Name = "tsmiResponseMapFromRequest";
            this.tsmiResponseMapFromRequest.Size = new System.Drawing.Size(200, 22);
            this.tsmiResponseMapFromRequest.Text = "Map from Request Field";
            // 
            // EditInterfaceMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(811, 479);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "EditInterfaceMap";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Edit Interface";
            this.Load += new System.EventHandler(this.EditViewInterfaceMap_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tcMaps.ResumeLayout(false);
            this.tpRequest.ResumeLayout(false);
            this.requestContextMenu.ResumeLayout(false);
            this.tpResponse.ResumeLayout(false);
            this.responseContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tcMaps;
        private System.Windows.Forms.TabPage tpRequest;
        private System.Windows.Forms.TabPage tpResponse;
        private ListViewSort lvRequest;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private ListViewSort lvResponse;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ContextMenuStrip requestContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiRequestAddCustomField;
        private System.Windows.Forms.ToolStripMenuItem tsmiRequestRemoveCustomField;
        private System.Windows.Forms.ToolStripMenuItem tsmiRequestEdit;
        private System.Windows.Forms.ContextMenuStrip responseContextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiResponseEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomField;
        private System.Windows.Forms.ToolStripMenuItem tsmiResponseAddCustomField;
        private System.Windows.Forms.ToolStripMenuItem tsmiResponseRemoveCustomField;
        private System.Windows.Forms.ToolStripMenuItem tsmiResponseMapFromRequest;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader17;

    }
}