namespace Cdc.MetaManager.GUI
{
    partial class CreatePropertyForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbDateTime = new System.Windows.Forms.RadioButton();
            this.rbBoolean = new System.Windows.Forms.RadioButton();
            this.rbDecimal = new System.Windows.Forms.RadioButton();
            this.rbInt = new System.Windows.Forms.RadioButton();
            this.rbDouble = new System.Windows.Forms.RadioButton();
            this.rbString = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rbInt64 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Location = new System.Drawing.Point(9, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 237);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Property";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbInt64);
            this.groupBox2.Controls.Add(this.rbDateTime);
            this.groupBox2.Controls.Add(this.rbBoolean);
            this.groupBox2.Controls.Add(this.rbDecimal);
            this.groupBox2.Controls.Add(this.rbInt);
            this.groupBox2.Controls.Add(this.rbDouble);
            this.groupBox2.Controls.Add(this.rbString);
            this.groupBox2.Location = new System.Drawing.Point(15, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(370, 179);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CLR Type";
            // 
            // rbDateTime
            // 
            this.rbDateTime.AutoSize = true;
            this.rbDateTime.Location = new System.Drawing.Point(9, 154);
            this.rbDateTime.Name = "rbDateTime";
            this.rbDateTime.Size = new System.Drawing.Size(108, 17);
            this.rbDateTime.TabIndex = 5;
            this.rbDateTime.TabStop = true;
            this.rbDateTime.Text = "System.DateTime";
            this.rbDateTime.UseVisualStyleBackColor = true;
            // 
            // rbBoolean
            // 
            this.rbBoolean.AutoSize = true;
            this.rbBoolean.Location = new System.Drawing.Point(9, 132);
            this.rbBoolean.Name = "rbBoolean";
            this.rbBoolean.Size = new System.Drawing.Size(101, 17);
            this.rbBoolean.TabIndex = 4;
            this.rbBoolean.TabStop = true;
            this.rbBoolean.Text = "System.Boolean";
            this.rbBoolean.UseVisualStyleBackColor = true;
            // 
            // rbDecimal
            // 
            this.rbDecimal.AutoSize = true;
            this.rbDecimal.Location = new System.Drawing.Point(9, 109);
            this.rbDecimal.Name = "rbDecimal";
            this.rbDecimal.Size = new System.Drawing.Size(100, 17);
            this.rbDecimal.TabIndex = 3;
            this.rbDecimal.TabStop = true;
            this.rbDecimal.Text = "System.Decimal";
            this.rbDecimal.UseVisualStyleBackColor = true;
            // 
            // rbInt
            // 
            this.rbInt.AutoSize = true;
            this.rbInt.Location = new System.Drawing.Point(9, 40);
            this.rbInt.Name = "rbInt";
            this.rbInt.Size = new System.Drawing.Size(86, 17);
            this.rbInt.TabIndex = 2;
            this.rbInt.TabStop = true;
            this.rbInt.Text = "System.Int32";
            this.rbInt.UseVisualStyleBackColor = true;
            // 
            // rbDouble
            // 
            this.rbDouble.AutoSize = true;
            this.rbDouble.Location = new System.Drawing.Point(9, 86);
            this.rbDouble.Name = "rbDouble";
            this.rbDouble.Size = new System.Drawing.Size(96, 17);
            this.rbDouble.TabIndex = 1;
            this.rbDouble.TabStop = true;
            this.rbDouble.Text = "System.Double";
            this.rbDouble.UseVisualStyleBackColor = true;
            // 
            // rbString
            // 
            this.rbString.AutoSize = true;
            this.rbString.Location = new System.Drawing.Point(9, 17);
            this.rbString.Name = "rbString";
            this.rbString.Size = new System.Drawing.Size(89, 17);
            this.rbString.TabIndex = 0;
            this.rbString.TabStop = true;
            this.rbString.Text = "System.String";
            this.rbString.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(115, 19);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(270, 20);
            this.tbName.TabIndex = 10;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(242, 255);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(79, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(327, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rbInt64
            // 
            this.rbInt64.AutoSize = true;
            this.rbInt64.Location = new System.Drawing.Point(9, 63);
            this.rbInt64.Name = "rbInt64";
            this.rbInt64.Size = new System.Drawing.Size(86, 17);
            this.rbInt64.TabIndex = 6;
            this.rbInt64.TabStop = true;
            this.rbInt64.Text = "System.Int64";
            this.rbInt64.UseVisualStyleBackColor = true;
            // 
            // CreatePropertyForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(415, 287);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreatePropertyForm";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Property";
            this.Load += new System.EventHandler(this.CreatePropertyForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbBoolean;
        private System.Windows.Forms.RadioButton rbDecimal;
        private System.Windows.Forms.RadioButton rbInt;
        private System.Windows.Forms.RadioButton rbDouble;
        private System.Windows.Forms.RadioButton rbString;
        private System.Windows.Forms.RadioButton rbDateTime;
        private System.Windows.Forms.RadioButton rbInt64;
    }
}