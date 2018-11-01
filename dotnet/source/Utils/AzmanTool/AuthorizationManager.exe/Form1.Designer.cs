namespace AuthorizationManager
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ApplicationsComboBox = new System.Windows.Forms.ComboBox();
            this.RolesComboBox = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExpandAll = new System.Windows.Forms.Button();
            this.lblApplications = new System.Windows.Forms.Label();
            this.btnCreateRole = new System.Windows.Forms.Button();
            this.btnDeleteRole = new System.Windows.Forms.Button();
            this.btnAddWinUser = new System.Windows.Forms.Button();
            this.lblWinUsers = new System.Windows.Forms.Label();
            this.btnDelWinUser = new System.Windows.Forms.Button();
            this.RolegroupBox = new System.Windows.Forms.GroupBox();
            this.btnShowUserRoles = new System.Windows.Forms.Button();
            this.WinUserlistBox = new System.Windows.Forms.ListBox();
            this.AccessibilitygroupBox = new System.Windows.Forms.GroupBox();
            this.AllNodescheckBox = new System.Windows.Forms.CheckBox();
            this.ApplicationTreeView = new AuthorizationManager.Utilities.New_Winform_Components.NewTreeView();
            this.RolegroupBox.SuspendLayout();
            this.AccessibilitygroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ApplicationsComboBox
            // 
            this.ApplicationsComboBox.FormattingEnabled = true;
            this.ApplicationsComboBox.Location = new System.Drawing.Point(74, 6);
            this.ApplicationsComboBox.Name = "ApplicationsComboBox";
            this.ApplicationsComboBox.Size = new System.Drawing.Size(121, 21);
            this.ApplicationsComboBox.TabIndex = 2;
            this.ApplicationsComboBox.SelectedValueChanged += new System.EventHandler(this.ApplicationsComboBox_SelectedValueChanged);
            // 
            // RolesComboBox
            // 
            this.RolesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RolesComboBox.FormattingEnabled = true;
            this.RolesComboBox.Location = new System.Drawing.Point(6, 19);
            this.RolesComboBox.Name = "RolesComboBox";
            this.RolesComboBox.Size = new System.Drawing.Size(471, 21);
            this.RolesComboBox.TabIndex = 3;
            this.RolesComboBox.SelectedValueChanged += new System.EventHandler(this.RolesComboBox_SelectedValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(357, 632);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save Role Settings";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExpandAll
            // 
            this.btnExpandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpandAll.Location = new System.Drawing.Point(319, 19);
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.Size = new System.Drawing.Size(150, 23);
            this.btnExpandAll.TabIndex = 7;
            this.btnExpandAll.Text = "Expand/Collapse All";
            this.btnExpandAll.UseVisualStyleBackColor = true;
            this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
            // 
            // lblApplications
            // 
            this.lblApplications.AutoSize = true;
            this.lblApplications.Location = new System.Drawing.Point(9, 9);
            this.lblApplications.Name = "lblApplications";
            this.lblApplications.Size = new System.Drawing.Size(59, 13);
            this.lblApplications.TabIndex = 8;
            this.lblApplications.Text = "Application";
            // 
            // btnCreateRole
            // 
            this.btnCreateRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateRole.Location = new System.Drawing.Point(396, 46);
            this.btnCreateRole.Name = "btnCreateRole";
            this.btnCreateRole.Size = new System.Drawing.Size(81, 29);
            this.btnCreateRole.TabIndex = 10;
            this.btnCreateRole.Text = "New Role";
            this.btnCreateRole.UseVisualStyleBackColor = true;
            this.btnCreateRole.Click += new System.EventHandler(this.btnCreateRole_Click);
            // 
            // btnDeleteRole
            // 
            this.btnDeleteRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteRole.Location = new System.Drawing.Point(309, 46);
            this.btnDeleteRole.Name = "btnDeleteRole";
            this.btnDeleteRole.Size = new System.Drawing.Size(81, 29);
            this.btnDeleteRole.TabIndex = 11;
            this.btnDeleteRole.Text = "Delete Role";
            this.btnDeleteRole.UseVisualStyleBackColor = true;
            this.btnDeleteRole.Click += new System.EventHandler(this.btnDeleteRole_Click);
            // 
            // btnAddWinUser
            // 
            this.btnAddWinUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddWinUser.Location = new System.Drawing.Point(314, 623);
            this.btnAddWinUser.Name = "btnAddWinUser";
            this.btnAddWinUser.Size = new System.Drawing.Size(164, 29);
            this.btnAddWinUser.TabIndex = 12;
            this.btnAddWinUser.Text = "Connect Windows User to Role";
            this.btnAddWinUser.UseVisualStyleBackColor = true;
            this.btnAddWinUser.Click += new System.EventHandler(this.btnAddWinUser_Click);
            // 
            // lblWinUsers
            // 
            this.lblWinUsers.AutoSize = true;
            this.lblWinUsers.Location = new System.Drawing.Point(6, 70);
            this.lblWinUsers.Name = "lblWinUsers";
            this.lblWinUsers.Size = new System.Drawing.Size(81, 13);
            this.lblWinUsers.TabIndex = 14;
            this.lblWinUsers.Text = "Windows Users";
            // 
            // btnDelWinUser
            // 
            this.btnDelWinUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelWinUser.Location = new System.Drawing.Point(121, 623);
            this.btnDelWinUser.Name = "btnDelWinUser";
            this.btnDelWinUser.Size = new System.Drawing.Size(190, 29);
            this.btnDelWinUser.TabIndex = 15;
            this.btnDelWinUser.Text = "Disconnect Windows User from Role";
            this.btnDelWinUser.UseVisualStyleBackColor = true;
            this.btnDelWinUser.Click += new System.EventHandler(this.btnDelWinUser_Click);
            // 
            // RolegroupBox
            // 
            this.RolegroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.RolegroupBox.Controls.Add(this.btnShowUserRoles);
            this.RolegroupBox.Controls.Add(this.WinUserlistBox);
            this.RolegroupBox.Controls.Add(this.btnDeleteRole);
            this.RolegroupBox.Controls.Add(this.btnDelWinUser);
            this.RolegroupBox.Controls.Add(this.btnCreateRole);
            this.RolegroupBox.Controls.Add(this.RolesComboBox);
            this.RolegroupBox.Controls.Add(this.btnAddWinUser);
            this.RolegroupBox.Controls.Add(this.lblWinUsers);
            this.RolegroupBox.Location = new System.Drawing.Point(12, 33);
            this.RolegroupBox.Name = "RolegroupBox";
            this.RolegroupBox.Size = new System.Drawing.Size(483, 667);
            this.RolegroupBox.TabIndex = 16;
            this.RolegroupBox.TabStop = false;
            this.RolegroupBox.Text = "Role";
            // 
            // btnShowUserRoles
            // 
            this.btnShowUserRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowUserRoles.Location = new System.Drawing.Point(6, 623);
            this.btnShowUserRoles.Name = "btnShowUserRoles";
            this.btnShowUserRoles.Size = new System.Drawing.Size(112, 29);
            this.btnShowUserRoles.TabIndex = 16;
            this.btnShowUserRoles.Text = "Show Users\' Roles";
            this.btnShowUserRoles.UseVisualStyleBackColor = true;
            this.btnShowUserRoles.Click += new System.EventHandler(this.btnShowUserRoles_Click);
            // 
            // WinUserlistBox
            // 
            this.WinUserlistBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WinUserlistBox.FormattingEnabled = true;
            this.WinUserlistBox.Location = new System.Drawing.Point(6, 85);
            this.WinUserlistBox.Name = "WinUserlistBox";
            this.WinUserlistBox.Size = new System.Drawing.Size(471, 511);
            this.WinUserlistBox.TabIndex = 15;
            // 
            // AccessibilitygroupBox
            // 
            this.AccessibilitygroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccessibilitygroupBox.Controls.Add(this.AllNodescheckBox);
            this.AccessibilitygroupBox.Controls.Add(this.ApplicationTreeView);
            this.AccessibilitygroupBox.Controls.Add(this.btnSave);
            this.AccessibilitygroupBox.Controls.Add(this.btnExpandAll);
            this.AccessibilitygroupBox.Location = new System.Drawing.Point(503, 33);
            this.AccessibilitygroupBox.Name = "AccessibilitygroupBox";
            this.AccessibilitygroupBox.Size = new System.Drawing.Size(475, 668);
            this.AccessibilitygroupBox.TabIndex = 17;
            this.AccessibilitygroupBox.TabStop = false;
            this.AccessibilitygroupBox.Text = "Role Settings";
            // 
            // AllNodescheckBox
            // 
            this.AllNodescheckBox.AutoSize = true;
            this.AllNodescheckBox.Location = new System.Drawing.Point(6, 25);
            this.AllNodescheckBox.Name = "AllNodescheckBox";
            this.AllNodescheckBox.Size = new System.Drawing.Size(37, 17);
            this.AllNodescheckBox.TabIndex = 8;
            this.AllNodescheckBox.Text = "All";
            this.AllNodescheckBox.UseVisualStyleBackColor = true;
            this.AllNodescheckBox.CheckedChanged += new System.EventHandler(this.AllNodescheckBox_CheckedChanged);
            // 
            // ApplicationTreeView
            // 
            this.ApplicationTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplicationTreeView.CheckBoxes = true;
            this.ApplicationTreeView.Location = new System.Drawing.Point(6, 48);
            this.ApplicationTreeView.Name = "ApplicationTreeView";
            this.ApplicationTreeView.Size = new System.Drawing.Size(463, 578);
            this.ApplicationTreeView.TabIndex = 1;
            this.ApplicationTreeView.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCheck);
            this.ApplicationTreeView.Leave += new System.EventHandler(this.ApplicationTreeView_Leave);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 712);
            this.Controls.Add(this.AccessibilitygroupBox);
            this.Controls.Add(this.RolegroupBox);
            this.Controls.Add(this.lblApplications);
            this.Controls.Add(this.ApplicationsComboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "Form1";
            this.Text = "IMI Authorization Manager Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.RolegroupBox.ResumeLayout(false);
            this.RolegroupBox.PerformLayout();
            this.AccessibilitygroupBox.ResumeLayout(false);
            this.AccessibilitygroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ApplicationsComboBox;
        private System.Windows.Forms.ComboBox RolesComboBox;
        private Utilities.New_Winform_Components.NewTreeView ApplicationTreeView;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExpandAll;
        private System.Windows.Forms.Label lblApplications;
        private System.Windows.Forms.Button btnCreateRole;
        private System.Windows.Forms.Button btnDeleteRole;
        private System.Windows.Forms.Button btnAddWinUser;
        private System.Windows.Forms.Label lblWinUsers;
        private System.Windows.Forms.Button btnDelWinUser;
        private System.Windows.Forms.GroupBox RolegroupBox;
        private System.Windows.Forms.ListBox WinUserlistBox;
        private System.Windows.Forms.GroupBox AccessibilitygroupBox;
        private System.Windows.Forms.CheckBox AllNodescheckBox;
        private System.Windows.Forms.Button btnShowUserRoles;
    }
}

