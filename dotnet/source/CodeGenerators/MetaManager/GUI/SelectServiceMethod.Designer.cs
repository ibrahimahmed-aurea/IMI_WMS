
namespace Cdc.MetaManager.GUI
{
    partial class SelectServiceMethod
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbViewName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.tbQuery = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbServiceMethod = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbService = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lvServiceMethods = new Cdc.MetaManager.GUI.ListViewSort();
            this.chId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chService = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chServiceMethod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chQuery = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkDisableTooltip = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbViewName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(613, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View Information";
            // 
            // tbViewName
            // 
            this.tbViewName.Location = new System.Drawing.Point(9, 32);
            this.tbViewName.Name = "tbViewName";
            this.tbViewName.ReadOnly = true;
            this.tbViewName.Size = new System.Drawing.Size(250, 20);
            this.tbViewName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFind);
            this.groupBox2.Controls.Add(this.tbQuery);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbServiceMethod);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbService);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lvServiceMethods);
            this.groupBox2.Location = new System.Drawing.Point(12, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(613, 283);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Service Methods";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(532, 30);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // tbQuery
            // 
            this.tbQuery.Location = new System.Drawing.Point(308, 32);
            this.tbQuery.Name = "tbQuery";
            this.tbQuery.Size = new System.Drawing.Size(218, 20);
            this.tbQuery.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(305, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Query / Ref Cursor Proc";
            // 
            // tbServiceMethod
            // 
            this.tbServiceMethod.Location = new System.Drawing.Point(137, 32);
            this.tbServiceMethod.Name = "tbServiceMethod";
            this.tbServiceMethod.Size = new System.Drawing.Size(165, 20);
            this.tbServiceMethod.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(134, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Service Method";
            // 
            // tbService
            // 
            this.tbService.Location = new System.Drawing.Point(9, 32);
            this.tbService.Name = "tbService";
            this.tbService.Size = new System.Drawing.Size(122, 20);
            this.tbService.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Service";
            // 
            // lvServiceMethods
            // 
            this.lvServiceMethods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chId,
            this.chService,
            this.chServiceMethod,
            this.chQuery});
            this.lvServiceMethods.FullRowSelect = true;
            this.lvServiceMethods.HideSelection = false;
            this.lvServiceMethods.Location = new System.Drawing.Point(9, 58);
            this.lvServiceMethods.MultiSelect = false;
            this.lvServiceMethods.Name = "lvServiceMethods";
            this.lvServiceMethods.Size = new System.Drawing.Size(598, 217);
            this.lvServiceMethods.TabIndex = 0;
            this.lvServiceMethods.UseCompatibleStateImageBehavior = false;
            this.lvServiceMethods.View = System.Windows.Forms.View.Details;
            this.lvServiceMethods.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.lvServiceMethods_ItemMouseHover);
            this.lvServiceMethods.SelectedIndexChanged += new System.EventHandler(this.lvServiceMethods_SelectedIndexChanged);
            // 
            // chId
            // 
            this.chId.Text = "Id";
            this.chId.Width = 45;
            // 
            // chService
            // 
            this.chService.Text = "Service";
            this.chService.Width = 127;
            // 
            // chServiceMethod
            // 
            this.chServiceMethod.Text = "Service Method";
            this.chServiceMethod.Width = 190;
            // 
            // chQuery
            // 
            this.chQuery.Text = "Query / Ref Cursor Proc";
            this.chQuery.Width = 217;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(544, 375);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(463, 375);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkDisableTooltip
            // 
            this.chkDisableTooltip.AutoSize = true;
            this.chkDisableTooltip.Checked = true;
            this.chkDisableTooltip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisableTooltip.Location = new System.Drawing.Point(21, 378);
            this.chkDisableTooltip.Name = "chkDisableTooltip";
            this.chkDisableTooltip.Size = new System.Drawing.Size(96, 17);
            this.chkDisableTooltip.TabIndex = 4;
            this.chkDisableTooltip.Text = "Disable Tooltip";
            this.chkDisableTooltip.UseVisualStyleBackColor = true;
            this.chkDisableTooltip.CheckedChanged += new System.EventHandler(this.chkDisableTooltip_CheckedChanged);
            // 
            // SelectServiceMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 407);
            this.Controls.Add(this.chkDisableTooltip);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectServiceMethod";
            this.Text = "Select Service Method";
            this.Load += new System.EventHandler(this.SelectServiceMethod_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private ListViewSort lvServiceMethods;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox tbViewName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader chService;
        private System.Windows.Forms.ColumnHeader chServiceMethod;
        private System.Windows.Forms.ColumnHeader chQuery;
        private System.Windows.Forms.TextBox tbQuery;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbServiceMethod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbService;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader chId;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.CheckBox chkDisableTooltip;
    }
}