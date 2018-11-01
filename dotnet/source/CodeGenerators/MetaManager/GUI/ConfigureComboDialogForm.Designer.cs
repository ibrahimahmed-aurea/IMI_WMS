namespace Cdc.MetaManager.GUI
{
    partial class ConfigureComboDialogForm
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
            this.dialogTbx = new System.Windows.Forms.TextBox();
            this.dialogFindBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.KeycomboBox = new System.Windows.Forms.ComboBox();
            this.modifyViewMapBtn = new System.Windows.Forms.Button();
            this.leftServiceMethodMapBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dialogTbx
            // 
            this.dialogTbx.Location = new System.Drawing.Point(87, 29);
            this.dialogTbx.Name = "dialogTbx";
            this.dialogTbx.ReadOnly = true;
            this.dialogTbx.Size = new System.Drawing.Size(191, 20);
            this.dialogTbx.TabIndex = 0;
            // 
            // dialogFindBtn
            // 
            this.dialogFindBtn.Location = new System.Drawing.Point(284, 27);
            this.dialogFindBtn.Name = "dialogFindBtn";
            this.dialogFindBtn.Size = new System.Drawing.Size(135, 23);
            this.dialogFindBtn.TabIndex = 2;
            this.dialogFindBtn.Text = "Find...";
            this.dialogFindBtn.UseVisualStyleBackColor = true;
            this.dialogFindBtn.Click += new System.EventHandler(this.dialogFindBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.KeycomboBox);
            this.groupBox1.Controls.Add(this.modifyViewMapBtn);
            this.groupBox1.Controls.Add(this.leftServiceMethodMapBtn);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dialogFindBtn);
            this.groupBox1.Controls.Add(this.dialogTbx);
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 3, 3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.groupBox1.Size = new System.Drawing.Size(430, 137);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configure Combo Dialog";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Key Property:";
            // 
            // KeycomboBox
            // 
            this.KeycomboBox.FormattingEnabled = true;
            this.KeycomboBox.Location = new System.Drawing.Point(87, 55);
            this.KeycomboBox.Name = "KeycomboBox";
            this.KeycomboBox.Size = new System.Drawing.Size(191, 21);
            this.KeycomboBox.TabIndex = 24;
            // 
            // modifyViewMapBtn
            // 
            this.modifyViewMapBtn.Location = new System.Drawing.Point(284, 54);
            this.modifyViewMapBtn.Name = "modifyViewMapBtn";
            this.modifyViewMapBtn.Size = new System.Drawing.Size(135, 23);
            this.modifyViewMapBtn.TabIndex = 23;
            this.modifyViewMapBtn.Text = "Request Map...";
            this.modifyViewMapBtn.UseVisualStyleBackColor = true;
            this.modifyViewMapBtn.Click += new System.EventHandler(this.modifyViewMapBtn_Click);
            // 
            // leftServiceMethodMapBtn
            // 
            this.leftServiceMethodMapBtn.Location = new System.Drawing.Point(284, 83);
            this.leftServiceMethodMapBtn.Name = "leftServiceMethodMapBtn";
            this.leftServiceMethodMapBtn.Size = new System.Drawing.Size(135, 23);
            this.leftServiceMethodMapBtn.TabIndex = 22;
            this.leftServiceMethodMapBtn.Text = "Result Map...";
            this.leftServiceMethodMapBtn.UseVisualStyleBackColor = true;
            this.leftServiceMethodMapBtn.Click += new System.EventHandler(this.dialogMapBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Dialog:";
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(354, 158);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(91, 23);
            this.cancelBtn.TabIndex = 10;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(257, 158);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(91, 23);
            this.okBtn.TabIndex = 9;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // ConfigureComboDialogForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(455, 189);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureComboDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Combo Dialog";
            this.Load += new System.EventHandler(this.ConfigureComboDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox dialogTbx;
        private System.Windows.Forms.Button dialogFindBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button leftServiceMethodMapBtn;
        private System.Windows.Forms.Button modifyViewMapBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox KeycomboBox;
    }
}