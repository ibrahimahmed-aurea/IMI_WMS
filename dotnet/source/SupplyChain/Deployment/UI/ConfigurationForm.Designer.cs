namespace Imi.SupplyChain.Deployment.UI
{
    partial class ConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.folderBrowse = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbWebserverPort = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lblIISVersion = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbInternetGuestAccount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbVirtualDirectoryPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbVirtualDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbWebserverName = new System.Windows.Forms.TextBox();
            this.btnCertificateFileBrowse = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.tbCertificateFile = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStageBrowse = new System.Windows.Forms.Button();
            this.tbStageArea = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pfxFileBrowse = new System.Windows.Forms.OpenFileDialog();
            this.cbAskForPassword = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnOk);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(6, 398);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(665, 29);
            this.panel4.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(502, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(583, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // folderBrowse
            // 
            this.folderBrowse.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbWebserverPort);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.lblIISVersion);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.tbInternetGuestAccount);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbVirtualDirectoryPath);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbVirtualDirectory);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbWebserverName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(665, 270);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Deployment Website";
            // 
            // tbWebserverPort
            // 
            this.tbWebserverPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWebserverPort.Location = new System.Drawing.Point(134, 49);
            this.tbWebserverPort.Name = "tbWebserverPort";
            this.tbWebserverPort.Size = new System.Drawing.Size(56, 20);
            this.tbWebserverPort.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 52);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(84, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "Webserver Port:";
            // 
            // lblIISVersion
            // 
            this.lblIISVersion.AutoSize = true;
            this.lblIISVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIISVersion.Location = new System.Drawing.Point(235, 246);
            this.lblIISVersion.Name = "lblIISVersion";
            this.lblIISVersion.Size = new System.Drawing.Size(70, 13);
            this.lblIISVersion.TabIndex = 16;
            this.lblIISVersion.Text = "<IIS Version>";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(131, 246);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Current IIS Version:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(199, 232);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "IUSR";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(199, 216);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(234, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "<Webserver Name>\\IUSR_<Webserver Name>";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(131, 232);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "IIS 7+:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(131, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "IIS 5.1-6.0:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(580, 148);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbInternetGuestAccount
            // 
            this.tbInternetGuestAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInternetGuestAccount.Location = new System.Drawing.Point(134, 193);
            this.tbInternetGuestAccount.Name = "tbInternetGuestAccount";
            this.tbInternetGuestAccount.Size = new System.Drawing.Size(440, 20);
            this.tbInternetGuestAccount.TabIndex = 8;
            this.tbInternetGuestAccount.TextChanged += new System.EventHandler(this.tbInternetGuestAccount_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Internet Guest Account:";
            // 
            // tbVirtualDirectoryPath
            // 
            this.tbVirtualDirectoryPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVirtualDirectoryPath.Location = new System.Drawing.Point(133, 150);
            this.tbVirtualDirectoryPath.Name = "tbVirtualDirectoryPath";
            this.tbVirtualDirectoryPath.Size = new System.Drawing.Size(440, 20);
            this.tbVirtualDirectoryPath.TabIndex = 6;
            this.tbVirtualDirectoryPath.TextChanged += new System.EventHandler(this.tbVirtualDirectoryPath_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Path to Virtual Directory:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(131, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(344, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "http://<Webserver Name>:<Webserver Port>/<Main Virtual Directory>/";
            // 
            // tbVirtualDirectory
            // 
            this.tbVirtualDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVirtualDirectory.Location = new System.Drawing.Point(134, 100);
            this.tbVirtualDirectory.Name = "tbVirtualDirectory";
            this.tbVirtualDirectory.Size = new System.Drawing.Size(439, 20);
            this.tbVirtualDirectory.TabIndex = 5;
            this.tbVirtualDirectory.TextChanged += new System.EventHandler(this.tbVirtualDirectory_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Main Virtual Directory:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(131, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(227, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "http://<Webserver Name>:<Webserver Port>/";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Webserver Name:";
            // 
            // tbWebserverName
            // 
            this.tbWebserverName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWebserverName.Location = new System.Drawing.Point(134, 19);
            this.tbWebserverName.Name = "tbWebserverName";
            this.tbWebserverName.Size = new System.Drawing.Size(521, 20);
            this.tbWebserverName.TabIndex = 3;
            this.tbWebserverName.TextChanged += new System.EventHandler(this.tbWebserverName_TextChanged);
            // 
            // btnCertificateFileBrowse
            // 
            this.btnCertificateFileBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCertificateFileBrowse.Location = new System.Drawing.Point(580, 17);
            this.btnCertificateFileBrowse.Name = "btnCertificateFileBrowse";
            this.btnCertificateFileBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCertificateFileBrowse.TabIndex = 10;
            this.btnCertificateFileBrowse.Text = "Browse...";
            this.btnCertificateFileBrowse.UseVisualStyleBackColor = true;
            this.btnCertificateFileBrowse.Click += new System.EventHandler(this.btnCertificateFileBrowse_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Certificate File (.pfx):";
            // 
            // tbCertificateFile
            // 
            this.tbCertificateFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCertificateFile.Location = new System.Drawing.Point(134, 19);
            this.tbCertificateFile.Name = "tbCertificateFile";
            this.tbCertificateFile.Size = new System.Drawing.Size(440, 20);
            this.tbCertificateFile.TabIndex = 9;
            this.tbCertificateFile.TextChanged += new System.EventHandler(this.tbCertificateFile_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnStageBrowse);
            this.groupBox2.Controls.Add(this.tbStageArea);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(665, 54);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Staging Area";
            // 
            // btnStageBrowse
            // 
            this.btnStageBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStageBrowse.Location = new System.Drawing.Point(580, 19);
            this.btnStageBrowse.Name = "btnStageBrowse";
            this.btnStageBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnStageBrowse.TabIndex = 2;
            this.btnStageBrowse.Text = "Browse...";
            this.btnStageBrowse.UseVisualStyleBackColor = true;
            this.btnStageBrowse.Click += new System.EventHandler(this.btnStageBrowse_Click);
            // 
            // tbStageArea
            // 
            this.tbStageArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStageArea.Location = new System.Drawing.Point(134, 21);
            this.tbStageArea.Name = "tbStageArea";
            this.tbStageArea.Size = new System.Drawing.Size(439, 20);
            this.tbStageArea.TabIndex = 1;
            this.tbStageArea.TextChanged += new System.EventHandler(this.tbStageArea_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Path:";
            // 
            // pfxFileBrowse
            // 
            this.pfxFileBrowse.DefaultExt = "pfx";
            this.pfxFileBrowse.Filter = "Certificate files (.pfx)|*.pfx";
            // 
            // cbAskForPassword
            // 
            this.cbAskForPassword.AutoSize = true;
            this.cbAskForPassword.Location = new System.Drawing.Point(134, 46);
            this.cbAskForPassword.Name = "cbAskForPassword";
            this.cbAskForPassword.Size = new System.Drawing.Size(158, 17);
            this.cbAskForPassword.TabIndex = 11;
            this.cbAskForPassword.Text = "Ask for Certificate Password";
            this.cbAskForPassword.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbAskForPassword);
            this.groupBox5.Controls.Add(this.tbCertificateFile);
            this.groupBox5.Controls.Add(this.btnCertificateFileBrowse);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(6, 330);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(665, 68);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Deployment Certificate";
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // ConfigurationForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(677, 433);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationForm";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IMI Supply Chain Deployment Manager Configuration";
            this.Load += new System.EventHandler(this.Configuration_Load);
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbInternetGuestAccount;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbVirtualDirectoryPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbVirtualDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbWebserverName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStageBrowse;
        private System.Windows.Forms.TextBox tbStageArea;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnCertificateFileBrowse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbCertificateFile;
        private System.Windows.Forms.OpenFileDialog pfxFileBrowse;
        private System.Windows.Forms.CheckBox cbAskForPassword;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblIISVersion;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbWebserverPort;
        private System.Windows.Forms.Label label14;
    }
}