
namespace Cdc.MetaManager.GUI
{
    partial class GenerateReports
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvReports = new ListViewSort();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pProgress = new System.Windows.Forms.Panel();
            this.gbProgress = new System.Windows.Forms.GroupBox();
            this.btnStopGeneration = new System.Windows.Forms.Button();
            this.lblTimeElapsed = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowsePLSQLPackageFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseXMLSchemaFolder = new System.Windows.Forms.Button();
            this.tbXMLSchemaFolder = new System.Windows.Forms.TextBox();
            this.tbPLSQLPackageFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pProgress.SuspendLayout();
            this.gbProgress.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel6);
            this.groupBox3.Controls.Add(this.pProgress);
            this.groupBox3.Controls.Add(this.panel4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(609, 485);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // panel6
            // 
            this.panel6.AutoSize = true;
            this.panel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel6.Controls.Add(this.groupBox1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 122);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(603, 278);
            this.panel6.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvReports);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(603, 278);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generate Report Packages";
            // 
            // lvReports
            // 
            this.lvReports.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.lvReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvReports.FullRowSelect = true;
            this.lvReports.GridLines = true;
            this.lvReports.Location = new System.Drawing.Point(3, 16);
            this.lvReports.MultiSelect = false;
            this.lvReports.Name = "lvReports";
            this.lvReports.ShowItemToolTips = true;
            this.lvReports.Size = new System.Drawing.Size(597, 259);
            this.lvReports.TabIndex = 2;
            this.lvReports.UseCompatibleStateImageBehavior = false;
            this.lvReports.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 535;
            // 
            // pProgress
            // 
            this.pProgress.Controls.Add(this.gbProgress);
            this.pProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pProgress.Location = new System.Drawing.Point(3, 400);
            this.pProgress.Name = "pProgress";
            this.pProgress.Size = new System.Drawing.Size(603, 82);
            this.pProgress.TabIndex = 15;
            this.pProgress.Visible = false;
            // 
            // gbProgress
            // 
            this.gbProgress.Controls.Add(this.btnStopGeneration);
            this.gbProgress.Controls.Add(this.lblTimeElapsed);
            this.gbProgress.Controls.Add(this.label5);
            this.gbProgress.Controls.Add(this.lblProgressText);
            this.gbProgress.Controls.Add(this.progressBar);
            this.gbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbProgress.Location = new System.Drawing.Point(0, 0);
            this.gbProgress.Name = "gbProgress";
            this.gbProgress.Size = new System.Drawing.Size(603, 82);
            this.gbProgress.TabIndex = 0;
            this.gbProgress.TabStop = false;
            this.gbProgress.Text = "Generate Progress";
            // 
            // btnStopGeneration
            // 
            this.btnStopGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopGeneration.Location = new System.Drawing.Point(486, 48);
            this.btnStopGeneration.Name = "btnStopGeneration";
            this.btnStopGeneration.Size = new System.Drawing.Size(108, 23);
            this.btnStopGeneration.TabIndex = 9;
            this.btnStopGeneration.Text = "Stop Generation";
            this.btnStopGeneration.UseVisualStyleBackColor = true;
            this.btnStopGeneration.Click += new System.EventHandler(this.btnStopGeneration_Click);
            // 
            // lblTimeElapsed
            // 
            this.lblTimeElapsed.AutoSize = true;
            this.lblTimeElapsed.Location = new System.Drawing.Point(86, 63);
            this.lblTimeElapsed.Name = "lblTimeElapsed";
            this.lblTimeElapsed.Size = new System.Drawing.Size(0, 13);
            this.lblTimeElapsed.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Time Elapsed:";
            // 
            // lblProgressText
            // 
            this.lblProgressText.AutoSize = true;
            this.lblProgressText.Location = new System.Drawing.Point(6, 45);
            this.lblProgressText.Name = "lblProgressText";
            this.lblProgressText.Size = new System.Drawing.Size(69, 13);
            this.lblProgressText.TabIndex = 5;
            this.lblProgressText.Text = "ProgressText";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(9, 19);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(585, 23);
            this.progressBar.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.btnBrowsePLSQLPackageFolder);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.btnBrowseXMLSchemaFolder);
            this.panel4.Controls.Add(this.tbXMLSchemaFolder);
            this.panel4.Controls.Add(this.tbPLSQLPackageFolder);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(603, 106);
            this.panel4.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(136, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(360, 30);
            this.label2.TabIndex = 5;
            this.label2.Text = "The parent directory of the \"body\" and \"spec\" folders. If none of these directori" +
    "es exists as subdirectories to the selected folder, they will be created.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(136, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "The directory where the generated XML Schema files (.xsd) will end up.";
            // 
            // btnBrowsePLSQLPackageFolder
            // 
            this.btnBrowsePLSQLPackageFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowsePLSQLPackageFolder.Location = new System.Drawing.Point(502, 45);
            this.btnBrowsePLSQLPackageFolder.Name = "btnBrowsePLSQLPackageFolder";
            this.btnBrowsePLSQLPackageFolder.Size = new System.Drawing.Size(96, 23);
            this.btnBrowsePLSQLPackageFolder.TabIndex = 4;
            this.btnBrowsePLSQLPackageFolder.Text = "Browse...";
            this.btnBrowsePLSQLPackageFolder.UseVisualStyleBackColor = true;
            this.btnBrowsePLSQLPackageFolder.Click += new System.EventHandler(this.btnBrowsePLSQLPackageFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "XML Schema Folder:";
            // 
            // btnBrowseXMLSchemaFolder
            // 
            this.btnBrowseXMLSchemaFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseXMLSchemaFolder.Location = new System.Drawing.Point(502, 1);
            this.btnBrowseXMLSchemaFolder.Name = "btnBrowseXMLSchemaFolder";
            this.btnBrowseXMLSchemaFolder.Size = new System.Drawing.Size(96, 23);
            this.btnBrowseXMLSchemaFolder.TabIndex = 1;
            this.btnBrowseXMLSchemaFolder.Text = "Browse...";
            this.btnBrowseXMLSchemaFolder.UseVisualStyleBackColor = true;
            this.btnBrowseXMLSchemaFolder.Click += new System.EventHandler(this.btnBrowseXMLSchemaFolder_Click);
            // 
            // tbXMLSchemaFolder
            // 
            this.tbXMLSchemaFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbXMLSchemaFolder.Location = new System.Drawing.Point(139, 3);
            this.tbXMLSchemaFolder.Name = "tbXMLSchemaFolder";
            this.tbXMLSchemaFolder.Size = new System.Drawing.Size(357, 20);
            this.tbXMLSchemaFolder.TabIndex = 0;
            this.tbXMLSchemaFolder.TextChanged += new System.EventHandler(this.tbXMLSchemaFolder_TextChanged);
            // 
            // tbPLSQLPackageFolder
            // 
            this.tbPLSQLPackageFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPLSQLPackageFolder.Location = new System.Drawing.Point(139, 47);
            this.tbPLSQLPackageFolder.Name = "tbPLSQLPackageFolder";
            this.tbPLSQLPackageFolder.Size = new System.Drawing.Size(357, 20);
            this.tbPLSQLPackageFolder.TabIndex = 3;
            this.tbPLSQLPackageFolder.TextChanged += new System.EventHandler(this.tbPLSQLPackageFolder_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "PL/SQL Package Folder:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.okBtn);
            this.panel2.Controls.Add(this.cancelBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 485);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(609, 41);
            this.panel2.TabIndex = 14;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(405, 9);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(96, 23);
            this.okBtn.TabIndex = 0;
            this.okBtn.Text = "Generate";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(507, 9);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(96, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Close";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // GenerateReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 526);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerateReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report Generation";
            this.Load += new System.EventHandler(this.GenerateReports_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.pProgress.ResumeLayout(false);
            this.gbProgress.ResumeLayout(false);
            this.gbProgress.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button btnBrowsePLSQLPackageFolder;
        private System.Windows.Forms.Button btnBrowseXMLSchemaFolder;
        public System.Windows.Forms.TextBox tbPLSQLPackageFolder;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox tbXMLSchemaFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pProgress;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblProgressText;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox gbProgress;
        private ListViewSort lvReports;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.Label lblTimeElapsed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnStopGeneration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}