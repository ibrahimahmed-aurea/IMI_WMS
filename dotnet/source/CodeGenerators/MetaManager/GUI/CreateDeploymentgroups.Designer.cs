namespace Cdc.MetaManager.GUI
{
    partial class CreateDeploymentgroups
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
            this.allAppListView = new System.Windows.Forms.ListView();
            this.AppName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Frontend = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Backend = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FrontendApptextBox = new System.Windows.Forms.TextBox();
            this.BackendApptextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DeploymentgroupslistView = new System.Windows.Forms.ListView();
            this.deploygrpName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.DeployGrpNametextBox = new System.Windows.Forms.TextBox();
            this.Savebutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // allAppListView
            // 
            this.allAppListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AppName,
            this.Frontend,
            this.Backend});
            this.allAppListView.FullRowSelect = true;
            this.allAppListView.Location = new System.Drawing.Point(12, 12);
            this.allAppListView.Name = "allAppListView";
            this.allAppListView.Size = new System.Drawing.Size(308, 263);
            this.allAppListView.TabIndex = 0;
            this.allAppListView.UseCompatibleStateImageBehavior = false;
            this.allAppListView.View = System.Windows.Forms.View.Details;
            this.allAppListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.allAppListView_ItemDrag);
            // 
            // AppName
            // 
            this.AppName.Text = "Name";
            this.AppName.Width = 164;
            // 
            // Frontend
            // 
            this.Frontend.Text = "Frontend";
            // 
            // Backend
            // 
            this.Backend.Text = "Backend";
            // 
            // FrontendApptextBox
            // 
            this.FrontendApptextBox.AllowDrop = true;
            this.FrontendApptextBox.Location = new System.Drawing.Point(326, 101);
            this.FrontendApptextBox.Name = "FrontendApptextBox";
            this.FrontendApptextBox.Size = new System.Drawing.Size(207, 20);
            this.FrontendApptextBox.TabIndex = 1;
            this.FrontendApptextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrontendApptextBox_DragDrop);
            this.FrontendApptextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrontendApptextBox_DragEnter);
            // 
            // BackendApptextBox
            // 
            this.BackendApptextBox.AllowDrop = true;
            this.BackendApptextBox.Location = new System.Drawing.Point(326, 140);
            this.BackendApptextBox.Name = "BackendApptextBox";
            this.BackendApptextBox.Size = new System.Drawing.Size(207, 20);
            this.BackendApptextBox.TabIndex = 2;
            this.BackendApptextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.BackendApptextBox_DragDrop);
            this.BackendApptextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.BackendApptextBox_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(326, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Frontend Application";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Backend Application";
            // 
            // DeploymentgroupslistView
            // 
            this.DeploymentgroupslistView.AllowDrop = true;
            this.DeploymentgroupslistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.deploygrpName,
            this.columnHeader1,
            this.columnHeader2});
            this.DeploymentgroupslistView.Location = new System.Drawing.Point(538, 12);
            this.DeploymentgroupslistView.Name = "DeploymentgroupslistView";
            this.DeploymentgroupslistView.Size = new System.Drawing.Size(478, 263);
            this.DeploymentgroupslistView.TabIndex = 5;
            this.DeploymentgroupslistView.UseCompatibleStateImageBehavior = false;
            this.DeploymentgroupslistView.View = System.Windows.Forms.View.Details;
            this.DeploymentgroupslistView.DragDrop += new System.Windows.Forms.DragEventHandler(this.DeploymentgroupslistView_DragDrop);
            this.DeploymentgroupslistView.DragEnter += new System.Windows.Forms.DragEventHandler(this.DeploymentgroupslistView_DragEnter);
            // 
            // deploygrpName
            // 
            this.deploygrpName.Text = "Name";
            this.deploygrpName.Width = 168;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Frontend Application";
            this.columnHeader1.Width = 145;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Backend Application";
            this.columnHeader2.Width = 133;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(327, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Deployment Group Name";
            // 
            // DeployGrpNametextBox
            // 
            this.DeployGrpNametextBox.Location = new System.Drawing.Point(327, 179);
            this.DeployGrpNametextBox.Name = "DeployGrpNametextBox";
            this.DeployGrpNametextBox.Size = new System.Drawing.Size(207, 20);
            this.DeployGrpNametextBox.TabIndex = 6;
            this.DeployGrpNametextBox.TextChanged += new System.EventHandler(this.DeployGrpNametextBox_TextChanged);
            this.DeployGrpNametextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DeployGrpNametextBox_MouseDown);
            // 
            // Savebutton
            // 
            this.Savebutton.Location = new System.Drawing.Point(941, 283);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(75, 23);
            this.Savebutton.TabIndex = 8;
            this.Savebutton.Text = "OK";
            this.Savebutton.UseVisualStyleBackColor = true;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // CreateDeploymentgroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 318);
            this.Controls.Add(this.Savebutton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DeployGrpNametextBox);
            this.Controls.Add(this.DeploymentgroupslistView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BackendApptextBox);
            this.Controls.Add(this.FrontendApptextBox);
            this.Controls.Add(this.allAppListView);
            this.Name = "CreateDeploymentgroups";
            this.Text = "CreateDeploymentgroups";
            this.Load += new System.EventHandler(this.CreateDeploymentgroups_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView allAppListView;
        private System.Windows.Forms.ColumnHeader AppName;
        private System.Windows.Forms.ColumnHeader Frontend;
        private System.Windows.Forms.ColumnHeader Backend;
        private System.Windows.Forms.TextBox FrontendApptextBox;
        private System.Windows.Forms.TextBox BackendApptextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView DeploymentgroupslistView;
        private System.Windows.Forms.ColumnHeader deploygrpName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DeployGrpNametextBox;
        private System.Windows.Forms.Button Savebutton;
    }
}