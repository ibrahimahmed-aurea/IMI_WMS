namespace Cdc.MetaManager.GUI
{
    partial class ImportSelectedObjects
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
            this.panel7 = new System.Windows.Forms.Panel();
            this.ResultlistView = new ListViewSort();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.addFilesBtn = new System.Windows.Forms.Button();
            this.SelectAllcheckBox = new System.Windows.Forms.CheckBox();
            this.importBtn = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.clearListBtn = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.StatusprogressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.tbProgress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel9.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.ResultlistView);
            this.panel7.Controls.Add(this.panel1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 16);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(819, 244);
            this.panel7.TabIndex = 1;
            // 
            // ResultlistView
            // 
            this.ResultlistView.CheckBoxes = true;
            this.ResultlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5});
            this.ResultlistView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultlistView.FullRowSelect = true;
            this.ResultlistView.Location = new System.Drawing.Point(0, 44);
            this.ResultlistView.Name = "ResultlistView";
            this.ResultlistView.Size = new System.Drawing.Size(819, 200);
            this.ResultlistView.TabIndex = 2;
            this.ResultlistView.UseCompatibleStateImageBehavior = false;
            this.ResultlistView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Filepath";
            this.columnHeader5.Width = 653;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.addFilesBtn);
            this.panel1.Controls.Add(this.SelectAllcheckBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(819, 44);
            this.panel1.TabIndex = 1;
            // 
            // addFilesBtn
            // 
            this.addFilesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFilesBtn.Location = new System.Drawing.Point(703, 7);
            this.addFilesBtn.Name = "addFilesBtn";
            this.addFilesBtn.Size = new System.Drawing.Size(112, 28);
            this.addFilesBtn.TabIndex = 9;
            this.addFilesBtn.Text = "Add Files...";
            this.addFilesBtn.UseVisualStyleBackColor = true;
            this.addFilesBtn.Click += new System.EventHandler(this.addFilesBtn_Click);
            // 
            // SelectAllcheckBox
            // 
            this.SelectAllcheckBox.AutoSize = true;
            this.SelectAllcheckBox.Location = new System.Drawing.Point(9, 14);
            this.SelectAllcheckBox.Name = "SelectAllcheckBox";
            this.SelectAllcheckBox.Size = new System.Drawing.Size(69, 17);
            this.SelectAllcheckBox.TabIndex = 10;
            this.SelectAllcheckBox.Text = "Select all";
            this.SelectAllcheckBox.UseVisualStyleBackColor = true;
            this.SelectAllcheckBox.CheckedChanged += new System.EventHandler(this.SelectAllcheckBox_CheckedChanged);
            // 
            // importBtn
            // 
            this.importBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importBtn.Location = new System.Drawing.Point(568, 6);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(122, 28);
            this.importBtn.TabIndex = 0;
            this.importBtn.Text = "Import";
            this.importBtn.UseVisualStyleBackColor = true;
            this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.clearListBtn);
            this.panel6.Controls.Add(this.importBtn);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(3, 260);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(819, 52);
            this.panel6.TabIndex = 0;
            // 
            // clearListBtn
            // 
            this.clearListBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearListBtn.Location = new System.Drawing.Point(694, 6);
            this.clearListBtn.Name = "clearListBtn";
            this.clearListBtn.Size = new System.Drawing.Size(122, 28);
            this.clearListBtn.TabIndex = 1;
            this.clearListBtn.Text = "Clear List";
            this.clearListBtn.UseVisualStyleBackColor = true;
            this.clearListBtn.Click += new System.EventHandler(this.clearListBtn_Click);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.StatusprogressBar);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 16);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(819, 36);
            this.panel8.TabIndex = 5;
            // 
            // StatusprogressBar
            // 
            this.StatusprogressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusprogressBar.Location = new System.Drawing.Point(0, 3);
            this.StatusprogressBar.Name = "StatusprogressBar";
            this.StatusprogressBar.Size = new System.Drawing.Size(819, 25);
            this.StatusprogressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.StatusprogressBar.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel9);
            this.groupBox2.Controls.Add(this.panel8);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(825, 190);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Progress && Information";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.tbProgress);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 52);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(819, 135);
            this.panel9.TabIndex = 6;
            // 
            // tbProgress
            // 
            this.tbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbProgress.Location = new System.Drawing.Point(0, 0);
            this.tbProgress.Multiline = true;
            this.tbProgress.Name = "tbProgress";
            this.tbProgress.ReadOnly = true;
            this.tbProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbProgress.Size = new System.Drawing.Size(819, 135);
            this.tbProgress.TabIndex = 4;
            this.tbProgress.WordWrap = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel7);
            this.groupBox1.Controls.Add(this.panel6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(825, 315);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Objects To Import";
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
            this.splitContainer1.Size = new System.Drawing.Size(825, 509);
            this.splitContainer1.SplitterDistance = 315;
            this.splitContainer1.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.splitContainer1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(825, 509);
            this.panel5.TabIndex = 9;
            // 
            // ImportSelectedObjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 509);
            this.Controls.Add(this.panel5);
            this.Name = "ImportSelectedObjects";
            this.Text = "Import Objects";
            this.Load += new System.EventHandler(this.ImportSelectedObjects_Load);
            this.panel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel7;
        private ListViewSort ResultlistView;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox SelectAllcheckBox;
        private System.Windows.Forms.Button addFilesBtn;
        private System.Windows.Forms.Button importBtn;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ProgressBar StatusprogressBar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.TextBox tbProgress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button clearListBtn;
    }
}