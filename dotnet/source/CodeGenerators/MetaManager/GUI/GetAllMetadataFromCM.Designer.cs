namespace Cdc.MetaManager.GUI
{
    partial class GetAllMetadataFromCM
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
            this.Startbutton = new System.Windows.Forms.Button();
            this.Closebutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.StatusprogressBar = new System.Windows.Forms.ProgressBar();
            this.Statuslabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.browseFolderBtn = new System.Windows.Forms.Button();
            this.repositoryPathTbx = new System.Windows.Forms.TextBox();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.ExcludeZeroFilescheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Startbutton
            // 
            this.Startbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Startbutton.Location = new System.Drawing.Point(432, 95);
            this.Startbutton.Name = "Startbutton";
            this.Startbutton.Size = new System.Drawing.Size(113, 23);
            this.Startbutton.TabIndex = 8;
            this.Startbutton.Text = "Start";
            this.Startbutton.UseVisualStyleBackColor = true;
            this.Startbutton.Click += new System.EventHandler(this.Startbutton_Click);
            // 
            // Closebutton
            // 
            this.Closebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Closebutton.Location = new System.Drawing.Point(551, 95);
            this.Closebutton.Name = "Closebutton";
            this.Closebutton.Size = new System.Drawing.Size(107, 23);
            this.Closebutton.TabIndex = 7;
            this.Closebutton.Text = "Close";
            this.Closebutton.UseVisualStyleBackColor = true;
            this.Closebutton.Click += new System.EventHandler(this.Closebutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Status :";
            // 
            // StatusprogressBar
            // 
            this.StatusprogressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusprogressBar.Location = new System.Drawing.Point(16, 70);
            this.StatusprogressBar.Name = "StatusprogressBar";
            this.StatusprogressBar.Size = new System.Drawing.Size(642, 19);
            this.StatusprogressBar.TabIndex = 5;
            // 
            // Statuslabel
            // 
            this.Statuslabel.AutoSize = true;
            this.Statuslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Statuslabel.Location = new System.Drawing.Point(88, 43);
            this.Statuslabel.Name = "Statuslabel";
            this.Statuslabel.Size = new System.Drawing.Size(0, 24);
            this.Statuslabel.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Repository Root Folder:";
            // 
            // browseFolderBtn
            // 
            this.browseFolderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseFolderBtn.Location = new System.Drawing.Point(563, 4);
            this.browseFolderBtn.Name = "browseFolderBtn";
            this.browseFolderBtn.Size = new System.Drawing.Size(95, 23);
            this.browseFolderBtn.TabIndex = 12;
            this.browseFolderBtn.Text = "Browse...";
            this.browseFolderBtn.UseVisualStyleBackColor = true;
            this.browseFolderBtn.Click += new System.EventHandler(this.browseFolderBtn_Click);
            // 
            // repositoryPathTbx
            // 
            this.repositoryPathTbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.repositoryPathTbx.Location = new System.Drawing.Point(136, 6);
            this.repositoryPathTbx.Name = "repositoryPathTbx";
            this.repositoryPathTbx.ReadOnly = true;
            this.repositoryPathTbx.Size = new System.Drawing.Size(421, 20);
            this.repositoryPathTbx.TabIndex = 11;
            this.repositoryPathTbx.TextChanged += new System.EventHandler(this.repositoryPathTbx_TextChanged);
            // 
            // ExcludeZeroFilescheckBox
            // 
            this.ExcludeZeroFilescheckBox.AutoSize = true;
            this.ExcludeZeroFilescheckBox.Location = new System.Drawing.Point(16, 99);
            this.ExcludeZeroFilescheckBox.Name = "ExcludeZeroFilescheckBox";
            this.ExcludeZeroFilescheckBox.Size = new System.Drawing.Size(151, 17);
            this.ExcludeZeroFilescheckBox.TabIndex = 13;
            this.ExcludeZeroFilescheckBox.Text = "Exclude files with zero size";
            this.ExcludeZeroFilescheckBox.UseVisualStyleBackColor = true;
            // 
            // GetAllMetadataFromCM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 130);
            this.Controls.Add(this.ExcludeZeroFilescheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.browseFolderBtn);
            this.Controls.Add(this.repositoryPathTbx);
            this.Controls.Add(this.Statuslabel);
            this.Controls.Add(this.Startbutton);
            this.Controls.Add(this.Closebutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StatusprogressBar);
            this.MinimumSize = new System.Drawing.Size(683, 168);
            this.Name = "GetAllMetadataFromCM";
            this.Text = "Get All Metadata From CM";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Startbutton;
        private System.Windows.Forms.Button Closebutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar StatusprogressBar;
        private System.Windows.Forms.Label Statuslabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button browseFolderBtn;
        public System.Windows.Forms.TextBox repositoryPathTbx;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.CheckBox ExcludeZeroFilescheckBox;
    }
}