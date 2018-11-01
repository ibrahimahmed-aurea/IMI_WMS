
namespace Cdc.MetaManager.GUI
{
    partial class GenerateBackendCodeForm
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
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lvServices = new ListViewSort();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.pProgress = new System.Windows.Forms.Panel();
            this.gbProgress = new System.Windows.Forms.GroupBox();
            this.btnStopGeneration = new System.Windows.Forms.Button();
            this.lblTimeElapsed = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbApp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.browseRefernceBtn = new System.Windows.Forms.Button();
            this.solutionNameTbx = new System.Windows.Forms.TextBox();
            this.browseSolutionBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.referencePathTbx = new System.Windows.Forms.TextBox();
            this.solutionPathTbx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pProgress.SuspendLayout();
            this.gbProgress.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel6);
            this.groupBox3.Controls.Add(this.pProgress);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(733, 481);
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
            this.panel6.Location = new System.Drawing.Point(3, 127);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(727, 269);
            this.panel6.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(727, 269);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Service Filter";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lvServices);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(721, 229);
            this.panel3.TabIndex = 2;
            // 
            // lvServices
            // 
            this.lvServices.CheckBoxes = true;
            this.lvServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.lvServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvServices.FullRowSelect = true;
            this.lvServices.GridLines = true;
            this.lvServices.Location = new System.Drawing.Point(0, 0);
            this.lvServices.MultiSelect = false;
            this.lvServices.Name = "lvServices";
            this.lvServices.Size = new System.Drawing.Size(721, 229);
            this.lvServices.TabIndex = 2;
            this.lvServices.UseCompatibleStateImageBehavior = false;
            this.lvServices.View = System.Windows.Forms.View.Details;
            this.lvServices.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvServices_ItemChecked);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 627;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cbSelectAll);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(721, 21);
            this.panel4.TabIndex = 1;
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(6, 3);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(15, 14);
            this.cbSelectAll.TabIndex = 1;
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // pProgress
            // 
            this.pProgress.Controls.Add(this.gbProgress);
            this.pProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pProgress.Location = new System.Drawing.Point(3, 396);
            this.pProgress.Name = "pProgress";
            this.pProgress.Size = new System.Drawing.Size(727, 82);
            this.pProgress.TabIndex = 16;
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
            this.gbProgress.Size = new System.Drawing.Size(727, 82);
            this.gbProgress.TabIndex = 0;
            this.gbProgress.TabStop = false;
            this.gbProgress.Text = "Generate Progress";
            // 
            // btnStopGeneration
            // 
            this.btnStopGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopGeneration.Location = new System.Drawing.Point(610, 48);
            this.btnStopGeneration.Name = "btnStopGeneration";
            this.btnStopGeneration.Size = new System.Drawing.Size(108, 23);
            this.btnStopGeneration.TabIndex = 8;
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
            this.progressBar.Size = new System.Drawing.Size(709, 23);
            this.progressBar.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbApp);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.browseRefernceBtn);
            this.panel1.Controls.Add(this.solutionNameTbx);
            this.panel1.Controls.Add(this.browseSolutionBtn);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.referencePathTbx);
            this.panel1.Controls.Add(this.solutionPathTbx);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(727, 111);
            this.panel1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Application:";
            // 
            // tbApp
            // 
            this.tbApp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbApp.Location = new System.Drawing.Point(133, 3);
            this.tbApp.Name = "tbApp";
            this.tbApp.ReadOnly = true;
            this.tbApp.Size = new System.Drawing.Size(590, 20);
            this.tbApp.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Solution Name:";
            // 
            // browseRefernceBtn
            // 
            this.browseRefernceBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseRefernceBtn.Location = new System.Drawing.Point(627, 79);
            this.browseRefernceBtn.Name = "browseRefernceBtn";
            this.browseRefernceBtn.Size = new System.Drawing.Size(96, 23);
            this.browseRefernceBtn.TabIndex = 11;
            this.browseRefernceBtn.Text = "Browse...";
            this.browseRefernceBtn.UseVisualStyleBackColor = true;
            this.browseRefernceBtn.Visible = false;
            this.browseRefernceBtn.Click += new System.EventHandler(this.browseRefernceBtn_Click);
            // 
            // solutionNameTbx
            // 
            this.solutionNameTbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.solutionNameTbx.Location = new System.Drawing.Point(133, 29);
            this.solutionNameTbx.Name = "solutionNameTbx";
            this.solutionNameTbx.ReadOnly = true;
            this.solutionNameTbx.Size = new System.Drawing.Size(590, 20);
            this.solutionNameTbx.TabIndex = 5;
            this.solutionNameTbx.Text = "Auto";
            // 
            // browseSolutionBtn
            // 
            this.browseSolutionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseSolutionBtn.Location = new System.Drawing.Point(627, 53);
            this.browseSolutionBtn.Name = "browseSolutionBtn";
            this.browseSolutionBtn.Size = new System.Drawing.Size(96, 23);
            this.browseSolutionBtn.TabIndex = 10;
            this.browseSolutionBtn.Text = "Browse...";
            this.browseSolutionBtn.UseVisualStyleBackColor = true;
            this.browseSolutionBtn.Visible = false;
            this.browseSolutionBtn.Click += new System.EventHandler(this.browseSolutionBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Solution Folder:";
            // 
            // referencePathTbx
            // 
            this.referencePathTbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.referencePathTbx.Location = new System.Drawing.Point(133, 81);
            this.referencePathTbx.Name = "referencePathTbx";
            this.referencePathTbx.ReadOnly = true;
            this.referencePathTbx.Size = new System.Drawing.Size(490, 20);
            this.referencePathTbx.TabIndex = 9;
            // 
            // solutionPathTbx
            // 
            this.solutionPathTbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.solutionPathTbx.Location = new System.Drawing.Point(133, 55);
            this.solutionPathTbx.Name = "solutionPathTbx";
            this.solutionPathTbx.ReadOnly = true;
            this.solutionPathTbx.Size = new System.Drawing.Size(490, 20);
            this.solutionPathTbx.TabIndex = 7;
            this.solutionPathTbx.TextChanged += new System.EventHandler(this.solutionPathTbx_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Reference Folder:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.okBtn);
            this.panel2.Controls.Add(this.cancelBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 481);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(733, 41);
            this.panel2.TabIndex = 14;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(529, 9);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(96, 23);
            this.okBtn.TabIndex = 3;
            this.okBtn.Text = "Generate";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(631, 9);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(96, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Close";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // GenerateBackendCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 522);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerateBackendCodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Start Backend Code Generation";
            this.Load += new System.EventHandler(this.GenerateCodeForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GenerateBackendCodeForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GenerateBackendCodeForm_KeyPress);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pProgress.ResumeLayout(false);
            this.gbProgress.ResumeLayout(false);
            this.gbProgress.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button browseRefernceBtn;
        private System.Windows.Forms.Button browseSolutionBtn;
        public System.Windows.Forms.TextBox referencePathTbx;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox solutionPathTbx;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox solutionNameTbx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.TextBox tbApp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pProgress;
        private System.Windows.Forms.GroupBox gbProgress;
        private System.Windows.Forms.Label lblTimeElapsed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblProgressText;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel3;
        private ListViewSort lvServices;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnStopGeneration;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.CheckBox cbSelectAll;
    }
}