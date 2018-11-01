namespace Cdc.MetaManager.GUI
{
    partial class ConnectViewToService
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbModules = new System.Windows.Forms.ComboBox();
            this.moduleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnShowDialog = new System.Windows.Forms.Button();
            this.cbDialogs = new System.Windows.Forms.ComboBox();
            this.dialogBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lvViews = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moduleBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dialogBindingSource)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbModules);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Module";
            // 
            // cbModules
            // 
            this.cbModules.DataSource = this.moduleBindingSource;
            this.cbModules.DisplayMember = "Name";
            this.cbModules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModules.FormattingEnabled = true;
            this.cbModules.Location = new System.Drawing.Point(6, 19);
            this.cbModules.Name = "cbModules";
            this.cbModules.Size = new System.Drawing.Size(294, 21);
            this.cbModules.TabIndex = 0;
            this.cbModules.ValueMember = "Id";
            this.cbModules.SelectedValueChanged += new System.EventHandler(this.cbModules_SelectedValueChanged);
            // 
            // moduleBindingSource
            // 
            this.moduleBindingSource.DataSource = typeof(Cdc.MetaManager.DataAccess.Domain.Module);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnShowDialog);
            this.groupBox2.Controls.Add(this.cbDialogs);
            this.groupBox2.Location = new System.Drawing.Point(12, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(580, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dialog";
            // 
            // btnShowDialog
            // 
            this.btnShowDialog.Location = new System.Drawing.Point(306, 17);
            this.btnShowDialog.Name = "btnShowDialog";
            this.btnShowDialog.Size = new System.Drawing.Size(99, 23);
            this.btnShowDialog.TabIndex = 1;
            this.btnShowDialog.Text = "Show Dialog";
            this.btnShowDialog.UseVisualStyleBackColor = true;
            this.btnShowDialog.Click += new System.EventHandler(this.btnShowDialog_Click);
            // 
            // cbDialogs
            // 
            this.cbDialogs.DataSource = this.dialogBindingSource;
            this.cbDialogs.DisplayMember = "Name";
            this.cbDialogs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDialogs.FormattingEnabled = true;
            this.cbDialogs.Location = new System.Drawing.Point(6, 19);
            this.cbDialogs.Name = "cbDialogs";
            this.cbDialogs.Size = new System.Drawing.Size(294, 21);
            this.cbDialogs.TabIndex = 0;
            this.cbDialogs.ValueMember = "Id";
            this.cbDialogs.SelectedValueChanged += new System.EventHandler(this.cbDialogs_SelectedValueChanged);
            // 
            // dialogBindingSource
            // 
            this.dialogBindingSource.DataSource = typeof(Cdc.MetaManager.DataAccess.Domain.Dialog);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lvViews);
            this.groupBox3.Location = new System.Drawing.Point(12, 130);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(580, 299);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Views";
            // 
            // lvViews
            // 
            this.lvViews.Location = new System.Drawing.Point(6, 19);
            this.lvViews.Name = "lvViews";
            this.lvViews.Size = new System.Drawing.Size(568, 216);
            this.lvViews.TabIndex = 0;
            this.lvViews.UseCompatibleStateImageBehavior = false;
            // 
            // ConnectViewToService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 476);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ConnectViewToService";
            this.Text = "ConnectViewToService";
            this.Load += new System.EventHandler(this.ConnectViewToService_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.moduleBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dialogBindingSource)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbModules;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnShowDialog;
        private System.Windows.Forms.ComboBox cbDialogs;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lvViews;
        private System.Windows.Forms.BindingSource moduleBindingSource;
        private System.Windows.Forms.BindingSource dialogBindingSource;
    }
}