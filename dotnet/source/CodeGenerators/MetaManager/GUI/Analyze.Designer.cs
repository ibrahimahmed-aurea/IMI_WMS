namespace Cdc.MetaManager.GUI
{
    partial class Analyze
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbProgress = new System.Windows.Forms.GroupBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCheckStoredProcedures = new System.Windows.Forms.CheckBox();
            this.tbDatabaseConnection = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCheckSQLQueries = new System.Windows.Forms.CheckBox();
            this.btnChangeDir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSpecFilePath = new System.Windows.Forms.TextBox();
            this.gbChecks = new System.Windows.Forms.GroupBox();
            this.cbCheckAllDialogs = new System.Windows.Forms.CheckBox();
            this.cbCheckAllMaps = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblTimeElapsed = new System.Windows.Forms.Label();
            this.panel4.SuspendLayout();
            this.gbProgress.SuspendLayout();
            this.gbChecks.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(674, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 311);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 311);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(669, 5);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnStart);
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 277);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(669, 34);
            this.panel4.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(481, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(97, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(585, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gbProgress
            // 
            this.gbProgress.Controls.Add(this.lblPass);
            this.gbProgress.Controls.Add(this.progressBar);
            this.gbProgress.Controls.Add(this.lblProgressText);
            this.gbProgress.Controls.Add(this.label5);
            this.gbProgress.Controls.Add(this.lblTimeElapsed);
            this.gbProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbProgress.Location = new System.Drawing.Point(0, 167);
            this.gbProgress.Name = "gbProgress";
            this.gbProgress.Size = new System.Drawing.Size(669, 105);
            this.gbProgress.TabIndex = 1;
            this.gbProgress.TabStop = false;
            this.gbProgress.Text = "Check Progress";
            this.gbProgress.Visible = false;
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(8, 16);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(51, 13);
            this.lblPass.TabIndex = 9;
            this.lblPass.Text = "PassText";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(11, 32);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(651, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 4;
            // 
            // lblProgressText
            // 
            this.lblProgressText.AutoSize = true;
            this.lblProgressText.Location = new System.Drawing.Point(8, 58);
            this.lblProgressText.Name = "lblProgressText";
            this.lblProgressText.Size = new System.Drawing.Size(69, 13);
            this.lblProgressText.TabIndex = 5;
            this.lblProgressText.Text = "ProgressText";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Time Elapsed:";
            // 
            // cbCheckStoredProcedures
            // 
            this.cbCheckStoredProcedures.AutoSize = true;
            this.cbCheckStoredProcedures.Location = new System.Drawing.Point(11, 19);
            this.cbCheckStoredProcedures.Name = "cbCheckStoredProcedures";
            this.cbCheckStoredProcedures.Size = new System.Drawing.Size(310, 17);
            this.cbCheckStoredProcedures.TabIndex = 0;
            this.cbCheckStoredProcedures.Text = "Check Stored Procedures against Specifications (.spec) files";
            this.cbCheckStoredProcedures.UseVisualStyleBackColor = true;
            this.cbCheckStoredProcedures.CheckedChanged += new System.EventHandler(this.cbCheckStoredProcedures_CheckedChanged);
            // 
            // tbDatabaseConnection
            // 
            this.tbDatabaseConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDatabaseConnection.Location = new System.Drawing.Point(147, 91);
            this.tbDatabaseConnection.Name = "tbDatabaseConnection";
            this.tbDatabaseConnection.ReadOnly = true;
            this.tbDatabaseConnection.Size = new System.Drawing.Size(517, 20);
            this.tbDatabaseConnection.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Database Connection:";
            // 
            // cbCheckSQLQueries
            // 
            this.cbCheckSQLQueries.AutoSize = true;
            this.cbCheckSQLQueries.Location = new System.Drawing.Point(11, 68);
            this.cbCheckSQLQueries.Name = "cbCheckSQLQueries";
            this.cbCheckSQLQueries.Size = new System.Drawing.Size(204, 17);
            this.cbCheckSQLQueries.TabIndex = 5;
            this.cbCheckSQLQueries.Text = "Check SQL Queries against database";
            this.cbCheckSQLQueries.UseVisualStyleBackColor = true;
            this.cbCheckSQLQueries.CheckedChanged += new System.EventHandler(this.cbCheckSQLQueries_CheckedChanged);
            // 
            // btnChangeDir
            // 
            this.btnChangeDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeDir.Location = new System.Drawing.Point(591, 40);
            this.btnChangeDir.Name = "btnChangeDir";
            this.btnChangeDir.Size = new System.Drawing.Size(75, 23);
            this.btnChangeDir.TabIndex = 3;
            this.btnChangeDir.Text = "Change...";
            this.btnChangeDir.UseVisualStyleBackColor = true;
            this.btnChangeDir.Click += new System.EventHandler(this.btnChangeDir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path to .spec files:";
            // 
            // tbSpecFilePath
            // 
            this.tbSpecFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSpecFilePath.Location = new System.Drawing.Point(147, 42);
            this.tbSpecFilePath.Name = "tbSpecFilePath";
            this.tbSpecFilePath.ReadOnly = true;
            this.tbSpecFilePath.Size = new System.Drawing.Size(441, 20);
            this.tbSpecFilePath.TabIndex = 1;
            // 
            // gbChecks
            // 
            this.gbChecks.AutoSize = true;
            this.gbChecks.Controls.Add(this.cbCheckAllDialogs);
            this.gbChecks.Controls.Add(this.cbCheckAllMaps);
            this.gbChecks.Controls.Add(this.cbCheckStoredProcedures);
            this.gbChecks.Controls.Add(this.tbDatabaseConnection);
            this.gbChecks.Controls.Add(this.label2);
            this.gbChecks.Controls.Add(this.cbCheckSQLQueries);
            this.gbChecks.Controls.Add(this.btnChangeDir);
            this.gbChecks.Controls.Add(this.label1);
            this.gbChecks.Controls.Add(this.tbSpecFilePath);
            this.gbChecks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbChecks.Location = new System.Drawing.Point(0, 0);
            this.gbChecks.Name = "gbChecks";
            this.gbChecks.Size = new System.Drawing.Size(669, 167);
            this.gbChecks.TabIndex = 1;
            this.gbChecks.TabStop = false;
            this.gbChecks.Text = "Checks";
            // 
            // cbCheckAllDialogs
            // 
            this.cbCheckAllDialogs.AutoSize = true;
            this.cbCheckAllDialogs.Location = new System.Drawing.Point(11, 140);
            this.cbCheckAllDialogs.Name = "cbCheckAllDialogs";
            this.cbCheckAllDialogs.Size = new System.Drawing.Size(108, 17);
            this.cbCheckAllDialogs.TabIndex = 9;
            this.cbCheckAllDialogs.Text = "Check all Dialogs";
            this.cbCheckAllDialogs.UseVisualStyleBackColor = true;
            this.cbCheckAllDialogs.CheckedChanged += new System.EventHandler(this.cbCheckAllDialogs_CheckedChanged);
            // 
            // cbCheckAllMaps
            // 
            this.cbCheckAllMaps.AutoSize = true;
            this.cbCheckAllMaps.Location = new System.Drawing.Point(11, 117);
            this.cbCheckAllMaps.Name = "cbCheckAllMaps";
            this.cbCheckAllMaps.Size = new System.Drawing.Size(99, 17);
            this.cbCheckAllMaps.TabIndex = 8;
            this.cbCheckAllMaps.Text = "Check all Maps";
            this.cbCheckAllMaps.UseVisualStyleBackColor = true;
            this.cbCheckAllMaps.CheckedChanged += new System.EventHandler(this.cbCheckAllMaps_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.AutoSize = true;
            this.panel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel5.Controls.Add(this.gbChecks);
            this.panel5.Controls.Add(this.gbProgress);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(669, 272);
            this.panel5.TabIndex = 3;
            // 
            // lblTimeElapsed
            // 
            this.lblTimeElapsed.AutoSize = true;
            this.lblTimeElapsed.Location = new System.Drawing.Point(88, 76);
            this.lblTimeElapsed.Name = "lblTimeElapsed";
            this.lblTimeElapsed.Size = new System.Drawing.Size(51, 13);
            this.lblTimeElapsed.TabIndex = 7;
            this.lblTimeElapsed.Text = "TimeText";
            // 
            // Analyze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(679, 311);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Analyze";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Analyze";
            this.Load += new System.EventHandler(this.CheckBackend_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CheckBackend_FormClosing);
            this.panel4.ResumeLayout(false);
            this.gbProgress.ResumeLayout(false);
            this.gbProgress.PerformLayout();
            this.gbChecks.ResumeLayout(false);
            this.gbChecks.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbProgress;
        private System.Windows.Forms.CheckBox cbCheckSQLQueries;
        private System.Windows.Forms.Button btnChangeDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSpecFilePath;
        private System.Windows.Forms.CheckBox cbCheckStoredProcedures;
        private System.Windows.Forms.GroupBox gbChecks;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblProgressText;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbDatabaseConnection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox cbCheckAllMaps;
        private System.Windows.Forms.CheckBox cbCheckAllDialogs;
        private System.Windows.Forms.Label lblTimeElapsed;
    }
}