
namespace Cdc.MetaManager.GUI
{
    partial class SelectCustomDialog
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
            this.panel7 = new System.Windows.Forms.Panel();
            this.lvDialogs = new ListViewSort();
            this.chId = new System.Windows.Forms.ColumnHeader();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chTopic = new System.Windows.Forms.ColumnHeader();
            this.chDLLName = new System.Windows.Forms.ColumnHeader();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tbTopic = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbDLLName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnCreateCustomDialog = new System.Windows.Forms.Button();
            this.btnEditCustomDialog = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnDeleteCustomDialog = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel7);
            this.groupBox1.Controls.Add(this.panel6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(734, 406);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter Dialogs";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lvDialogs);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 58);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(728, 345);
            this.panel7.TabIndex = 8;
            // 
            // lvDialogs
            // 
            this.lvDialogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chId,
            this.chName,
            this.chTopic,
            this.chDLLName});
            this.lvDialogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDialogs.FullRowSelect = true;
            this.lvDialogs.HideSelection = false;
            this.lvDialogs.Location = new System.Drawing.Point(0, 0);
            this.lvDialogs.MultiSelect = false;
            this.lvDialogs.Name = "lvDialogs";
            this.lvDialogs.Size = new System.Drawing.Size(728, 345);
            this.lvDialogs.TabIndex = 0;
            this.lvDialogs.UseCompatibleStateImageBehavior = false;
            this.lvDialogs.View = System.Windows.Forms.View.Details;
            this.lvDialogs.DoubleClick += new System.EventHandler(this.lvDialogs_DoubleClick);
            this.lvDialogs.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvDialogs_ItemSelectionChanged);
            // 
            // chId
            // 
            this.chId.Text = "Id";
            this.chId.Width = 34;
            // 
            // chName
            // 
            this.chName.Text = "Dialog Name";
            this.chName.Width = 164;
            // 
            // chTopic
            // 
            this.chTopic.Text = "Topic";
            this.chTopic.Width = 257;
            // 
            // chDLLName
            // 
            this.chDLLName.Text = "DLL Name";
            this.chDLLName.Width = 248;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tbTopic);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.tbName);
            this.panel6.Controls.Add(this.tbDLLName);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 16);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(728, 42);
            this.panel6.TabIndex = 7;
            // 
            // tbTopic
            // 
            this.tbTopic.Location = new System.Drawing.Point(133, 16);
            this.tbTopic.Name = "tbTopic";
            this.tbTopic.Size = new System.Drawing.Size(138, 20);
            this.tbTopic.TabIndex = 6;
            this.tbTopic.TextChanged += new System.EventHandler(this.tbTopic_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(130, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Topic";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dialog Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(7, 16);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(120, 20);
            this.tbName.TabIndex = 2;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // tbDLLName
            // 
            this.tbDLLName.Location = new System.Drawing.Point(277, 16);
            this.tbDLLName.Name = "tbDLLName";
            this.tbDLLName.Size = new System.Drawing.Size(138, 20);
            this.tbDLLName.TabIndex = 4;
            this.tbDLLName.TextChanged += new System.EventHandler(this.tbDLLName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(274, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "DLL Name";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(644, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 450);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(739, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 450);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(734, 5);
            this.panel3.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnDeleteCustomDialog);
            this.panel4.Controls.Add(this.btnCreateCustomDialog);
            this.panel4.Controls.Add(this.btnEditCustomDialog);
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 411);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(734, 39);
            this.panel4.TabIndex = 7;
            // 
            // btnCreateCustomDialog
            // 
            this.btnCreateCustomDialog.Location = new System.Drawing.Point(7, 6);
            this.btnCreateCustomDialog.Name = "btnCreateCustomDialog";
            this.btnCreateCustomDialog.Size = new System.Drawing.Size(123, 23);
            this.btnCreateCustomDialog.TabIndex = 5;
            this.btnCreateCustomDialog.Text = "Create";
            this.btnCreateCustomDialog.UseVisualStyleBackColor = true;
            this.btnCreateCustomDialog.Click += new System.EventHandler(this.btnCreateCustomDialog_Click);
            // 
            // btnEditCustomDialog
            // 
            this.btnEditCustomDialog.Location = new System.Drawing.Point(265, 6);
            this.btnEditCustomDialog.Name = "btnEditCustomDialog";
            this.btnEditCustomDialog.Size = new System.Drawing.Size(123, 23);
            this.btnEditCustomDialog.TabIndex = 3;
            this.btnEditCustomDialog.Text = "Edit";
            this.btnEditCustomDialog.UseVisualStyleBackColor = true;
            this.btnEditCustomDialog.Click += new System.EventHandler(this.btnEditCustomDialog_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(734, 406);
            this.panel5.TabIndex = 8;
            // 
            // btnDeleteCustomDialog
            // 
            this.btnDeleteCustomDialog.Location = new System.Drawing.Point(136, 6);
            this.btnDeleteCustomDialog.Name = "btnDeleteCustomDialog";
            this.btnDeleteCustomDialog.Size = new System.Drawing.Size(123, 23);
            this.btnDeleteCustomDialog.TabIndex = 6;
            this.btnDeleteCustomDialog.Text = "Delete";
            this.btnDeleteCustomDialog.UseVisualStyleBackColor = true;
            this.btnDeleteCustomDialog.Click += new System.EventHandler(this.btnDeleteCustomDialog_Click);
            // 
            // SelectCustomDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(744, 450);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SelectCustomDialog";
            this.Text = "Custom Dialogs";
            this.Load += new System.EventHandler(this.SelectCustomDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private ListViewSort lvDialogs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbDLLName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader chId;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chDLLName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnEditCustomDialog;
        private System.Windows.Forms.Button btnCreateCustomDialog;
        private System.Windows.Forms.TextBox tbTopic;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader chTopic;
        private System.Windows.Forms.Button btnDeleteCustomDialog;
    }
}