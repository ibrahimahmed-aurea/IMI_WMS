namespace Cdc.MetaManager.GUI
{
    partial class EditInterfaceProperty
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbDefaultNone = new System.Windows.Forms.RadioButton();
            this.tbId = new System.Windows.Forms.TextBox();
            this.tbDefaultValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDefaultSession = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.rbDefaultText = new System.Windows.Forms.RadioButton();
            this.rbDefaultSession = new System.Windows.Forms.RadioButton();
            this.cbSearchable = new System.Windows.Forms.CheckBox();
            this.tbDisplayFormat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowseDisplayFormat = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 202);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(463, 32);
            this.panel1.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(304, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(385, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(463, 199);
            this.panel2.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbDisplayFormat);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnBrowseDisplayFormat);
            this.groupBox2.Controls.Add(this.cbSearchable);
            this.groupBox2.Controls.Add(this.rbDefaultNone);
            this.groupBox2.Controls.Add(this.tbId);
            this.groupBox2.Controls.Add(this.tbDefaultValue);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbDefaultSession);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbName);
            this.groupBox2.Controls.Add(this.rbDefaultText);
            this.groupBox2.Controls.Add(this.rbDefaultSession);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(463, 199);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Property";
            // 
            // rbDefaultNone
            // 
            this.rbDefaultNone.AutoSize = true;
            this.rbDefaultNone.Checked = true;
            this.rbDefaultNone.Location = new System.Drawing.Point(96, 73);
            this.rbDefaultNone.Name = "rbDefaultNone";
            this.rbDefaultNone.Size = new System.Drawing.Size(51, 17);
            this.rbDefaultNone.TabIndex = 5;
            this.rbDefaultNone.TabStop = true;
            this.rbDefaultNone.Text = "None";
            this.rbDefaultNone.UseVisualStyleBackColor = true;
            // 
            // tbId
            // 
            this.tbId.Location = new System.Drawing.Point(96, 19);
            this.tbId.Name = "tbId";
            this.tbId.ReadOnly = true;
            this.tbId.Size = new System.Drawing.Size(245, 20);
            this.tbId.TabIndex = 1;
            this.tbId.TabStop = false;
            // 
            // tbDefaultValue
            // 
            this.tbDefaultValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDefaultValue.Enabled = false;
            this.tbDefaultValue.Location = new System.Drawing.Point(202, 96);
            this.tbDefaultValue.Name = "tbDefaultValue";
            this.tbDefaultValue.ReadOnly = true;
            this.tbDefaultValue.Size = new System.Drawing.Size(255, 20);
            this.tbDefaultValue.TabIndex = 7;
            this.tbDefaultValue.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Id:";
            // 
            // cbDefaultSession
            // 
            this.cbDefaultSession.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDefaultSession.DisplayMember = "Name";
            this.cbDefaultSession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultSession.Enabled = false;
            this.cbDefaultSession.FormattingEnabled = true;
            this.cbDefaultSession.Location = new System.Drawing.Point(202, 118);
            this.cbDefaultSession.Name = "cbDefaultSession";
            this.cbDefaultSession.Size = new System.Drawing.Size(255, 21);
            this.cbDefaultSession.TabIndex = 9;
            this.cbDefaultSession.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Default Value:";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(96, 45);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(361, 20);
            this.tbName.TabIndex = 3;
            this.tbName.TabStop = false;
            // 
            // rbDefaultText
            // 
            this.rbDefaultText.AutoSize = true;
            this.rbDefaultText.Location = new System.Drawing.Point(96, 96);
            this.rbDefaultText.Name = "rbDefaultText";
            this.rbDefaultText.Size = new System.Drawing.Size(52, 17);
            this.rbDefaultText.TabIndex = 6;
            this.rbDefaultText.Text = "Value";
            this.rbDefaultText.UseVisualStyleBackColor = true;
            this.rbDefaultText.CheckedChanged += new System.EventHandler(this.rbDefaultText_CheckedChanged);
            // 
            // rbDefaultSession
            // 
            this.rbDefaultSession.AutoSize = true;
            this.rbDefaultSession.Location = new System.Drawing.Point(96, 119);
            this.rbDefaultSession.Name = "rbDefaultSession";
            this.rbDefaultSession.Size = new System.Drawing.Size(103, 17);
            this.rbDefaultSession.TabIndex = 8;
            this.rbDefaultSession.Text = "Session Variable";
            this.rbDefaultSession.UseVisualStyleBackColor = true;
            this.rbDefaultSession.CheckedChanged += new System.EventHandler(this.rbDefaultSession_CheckedChanged);
            // 
            // cbSearchable
            // 
            this.cbSearchable.AutoSize = true;
            this.cbSearchable.Location = new System.Drawing.Point(13, 171);
            this.cbSearchable.Name = "cbSearchable";
            this.cbSearchable.Size = new System.Drawing.Size(128, 17);
            this.cbSearchable.TabIndex = 11;
            this.cbSearchable.Text = "Show in search panel";
            this.cbSearchable.UseVisualStyleBackColor = true;
            // 
            // tbDisplayFormat
            // 
            this.tbDisplayFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDisplayFormat.Location = new System.Drawing.Point(96, 145);
            this.tbDisplayFormat.Name = "tbDisplayFormat";
            this.tbDisplayFormat.Size = new System.Drawing.Size(326, 20);
            this.tbDisplayFormat.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Display Format:";
            // 
            // btnBrowseDisplayFormat
            // 
            this.btnBrowseDisplayFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDisplayFormat.Location = new System.Drawing.Point(428, 143);
            this.btnBrowseDisplayFormat.Name = "btnBrowseDisplayFormat";
            this.btnBrowseDisplayFormat.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseDisplayFormat.TabIndex = 14;
            this.btnBrowseDisplayFormat.Text = "...";
            this.btnBrowseDisplayFormat.UseVisualStyleBackColor = true;
            this.btnBrowseDisplayFormat.Click += new System.EventHandler(this.btnBrowseDisplayFormat_Click);
            // 
            // EditInterfaceProperty
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(469, 237);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditInterfaceProperty";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Property";
            this.Load += new System.EventHandler(this.EditInterfaceProperty_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDefaultValue;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDefaultSession;
        private System.Windows.Forms.RadioButton rbDefaultText;
        private System.Windows.Forms.RadioButton rbDefaultSession;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbDefaultNone;
        private System.Windows.Forms.TextBox tbDisplayFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseDisplayFormat;
        private System.Windows.Forms.CheckBox cbSearchable;
    }
}