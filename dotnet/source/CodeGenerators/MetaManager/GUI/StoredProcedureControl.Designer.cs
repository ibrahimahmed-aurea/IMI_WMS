namespace Cdc.MetaManager.GUI
{
    partial class StoredProcedureControl
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
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.lvFoundProcedures = new ListViewSort();
            this.chFileProcedureName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chParseStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chImportStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tbStoredProcText = new System.Windows.Forms.TextBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPackageName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbParseFileName = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fileBrowseDialog = new System.Windows.Forms.OpenFileDialog();
            this.panel5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(754, 567);
            this.panel5.TabIndex = 21;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(754, 567);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Import Package Spec";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel10);
            this.groupBox2.Controls.Add(this.panel9);
            this.groupBox2.Controls.Add(this.panel8);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(748, 478);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Content";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.splitContainer3);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(3, 46);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(742, 396);
            this.panel10.TabIndex = 4;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.lvFoundProcedures);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox7);
            this.splitContainer3.Size = new System.Drawing.Size(742, 396);
            this.splitContainer3.SplitterDistance = 367;
            this.splitContainer3.TabIndex = 1;
            // 
            // lvFoundProcedures
            // 
            this.lvFoundProcedures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFileProcedureName,
            this.chParseStatus,
            this.chImportStatus});
            this.lvFoundProcedures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFoundProcedures.FullRowSelect = true;
            this.lvFoundProcedures.HideSelection = false;
            this.lvFoundProcedures.Location = new System.Drawing.Point(0, 0);
            this.lvFoundProcedures.MultiSelect = false;
            this.lvFoundProcedures.Name = "lvFoundProcedures";
            this.lvFoundProcedures.Size = new System.Drawing.Size(367, 396);
            this.lvFoundProcedures.TabIndex = 0;
            this.lvFoundProcedures.UseCompatibleStateImageBehavior = false;
            this.lvFoundProcedures.View = System.Windows.Forms.View.Details;
            this.lvFoundProcedures.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvFoundProcedures_ItemSelectionChanged);
            // 
            // chFileProcedureName
            // 
            this.chFileProcedureName.Text = "Procedure Name";
            this.chFileProcedureName.Width = 174;
            // 
            // chParseStatus
            // 
            this.chParseStatus.Text = "Parse Status";
            this.chParseStatus.Width = 95;
            // 
            // chImportStatus
            // 
            this.chImportStatus.Text = "Import Status";
            this.chImportStatus.Width = 136;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tbStoredProcText);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox7.Location = new System.Drawing.Point(0, 0);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(371, 396);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Parsed Procedure";
            // 
            // tbStoredProcText
            // 
            this.tbStoredProcText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbStoredProcText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStoredProcText.Location = new System.Drawing.Point(3, 16);
            this.tbStoredProcText.Multiline = true;
            this.tbStoredProcText.Name = "tbStoredProcText";
            this.tbStoredProcText.ReadOnly = true;
            this.tbStoredProcText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbStoredProcText.Size = new System.Drawing.Size(365, 377);
            this.tbStoredProcText.TabIndex = 0;
            this.tbStoredProcText.TabStop = false;
            this.tbStoredProcText.WordWrap = false;
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(3, 442);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(742, 33);
            this.panel9.TabIndex = 3;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label1);
            this.panel8.Controls.Add(this.tbPackageName);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 16);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(742, 30);
            this.panel8.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Package:";
            // 
            // tbPackageName
            // 
            this.tbPackageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPackageName.Location = new System.Drawing.Point(60, 3);
            this.tbPackageName.Name = "tbPackageName";
            this.tbPackageName.ReadOnly = true;
            this.tbPackageName.Size = new System.Drawing.Size(679, 20);
            this.tbPackageName.TabIndex = 1;
            this.tbPackageName.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.tbParseFileName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(748, 70);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PLSQL Package Specification File Selection";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(6, 42);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(88, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbParseFileName
            // 
            this.tbParseFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbParseFileName.Location = new System.Drawing.Point(6, 19);
            this.tbParseFileName.Name = "tbParseFileName";
            this.tbParseFileName.Size = new System.Drawing.Size(736, 20);
            this.tbParseFileName.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(5, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(754, 5);
            this.panel4.TabIndex = 20;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 572);
            this.panel2.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(759, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 572);
            this.panel1.TabIndex = 17;
            // 
            // fileBrowseDialog
            // 
            this.fileBrowseDialog.Filter = "Spec Files (*.spec)|*.spec";
            // 
            // StoredProcedureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "StoredProcedureControl";
            this.Size = new System.Drawing.Size(764, 572);
            this.panel5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.OpenFileDialog fileBrowseDialog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private ListViewSort lvFoundProcedures;
        private System.Windows.Forms.ColumnHeader chFileProcedureName;
        private System.Windows.Forms.ColumnHeader chParseStatus;
        private System.Windows.Forms.ColumnHeader chImportStatus;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox tbStoredProcText;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPackageName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbParseFileName;
    }
}
