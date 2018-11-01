namespace Cdc.MetaManager.GUI
{
    partial class SelectDisplayFormats
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectDisplayFormats));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.gbTestDisplayFormat = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDisplayFormat = new System.Windows.Forms.TextBox();
            this.llblHelpFormatStrings = new System.Windows.Forms.LinkLabel();
            this.tbTestData = new System.Windows.Forms.TextBox();
            this.lblDataType = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.lvDisplayFormats = new ListViewSort();
            this.chDisplayFormat = new System.Windows.Forms.ColumnHeader();
            this.chExample = new System.Windows.Forms.ColumnHeader();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.gbTestDisplayFormat.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(490, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 490);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 490);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(485, 5);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnOK);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 455);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(485, 35);
            this.panel4.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(322, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(403, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Controls.Add(this.gbTestDisplayFormat);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(485, 450);
            this.panel5.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel7);
            this.groupBox1.Controls.Add(this.panel6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(485, 275);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Existing used Display Formats";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lvDisplayFormats);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 16);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(479, 233);
            this.panel7.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(3, 249);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(479, 23);
            this.panel6.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.ForestGreen;
            this.label4.Location = new System.Drawing.Point(1, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(461, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Double Click the item in list to copy the Display Format to the testsection below" +
                ".";
            // 
            // gbTestDisplayFormat
            // 
            this.gbTestDisplayFormat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbTestDisplayFormat.Controls.Add(this.panel9);
            this.gbTestDisplayFormat.Controls.Add(this.panel8);
            this.gbTestDisplayFormat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbTestDisplayFormat.Location = new System.Drawing.Point(0, 275);
            this.gbTestDisplayFormat.Name = "gbTestDisplayFormat";
            this.gbTestDisplayFormat.Size = new System.Drawing.Size(485, 175);
            this.gbTestDisplayFormat.TabIndex = 1;
            this.gbTestDisplayFormat.TabStop = false;
            this.gbTestDisplayFormat.Text = "Test Display Format";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.richTextBox1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(479, 60);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Date Time special Display Format";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 16);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(473, 41);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label1);
            this.panel8.Controls.Add(this.tbDisplayFormat);
            this.panel8.Controls.Add(this.llblHelpFormatStrings);
            this.panel8.Controls.Add(this.tbTestData);
            this.panel8.Controls.Add(this.lblDataType);
            this.panel8.Controls.Add(this.label2);
            this.panel8.Controls.Add(this.tbOutput);
            this.panel8.Controls.Add(this.btnGenerate);
            this.panel8.Controls.Add(this.label3);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 16);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(479, 96);
            this.panel8.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Display Format:";
            // 
            // tbDisplayFormat
            // 
            this.tbDisplayFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDisplayFormat.Location = new System.Drawing.Point(89, 3);
            this.tbDisplayFormat.Name = "tbDisplayFormat";
            this.tbDisplayFormat.Size = new System.Drawing.Size(386, 20);
            this.tbDisplayFormat.TabIndex = 0;
            this.tbDisplayFormat.TextChanged += new System.EventHandler(this.tbDisplayFormat_TextChanged);
            // 
            // llblHelpFormatStrings
            // 
            this.llblHelpFormatStrings.AutoSize = true;
            this.llblHelpFormatStrings.Location = new System.Drawing.Point(86, 26);
            this.llblHelpFormatStrings.Name = "llblHelpFormatStrings";
            this.llblHelpFormatStrings.Size = new System.Drawing.Size(71, 13);
            this.llblHelpFormatStrings.TabIndex = 8;
            this.llblHelpFormatStrings.TabStop = true;
            this.llblHelpFormatStrings.Text = "[PlaceHolder]";
            this.llblHelpFormatStrings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblHelpFormatStrings_LinkClicked);
            // 
            // tbTestData
            // 
            this.tbTestData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTestData.Location = new System.Drawing.Point(89, 44);
            this.tbTestData.Name = "tbTestData";
            this.tbTestData.Size = new System.Drawing.Size(237, 20);
            this.tbTestData.TabIndex = 2;
            this.tbTestData.TextChanged += new System.EventHandler(this.tbTestData_TextChanged);
            // 
            // lblDataType
            // 
            this.lblDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataType.Location = new System.Drawing.Point(331, 47);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(63, 16);
            this.lblDataType.TabIndex = 7;
            this.lblDataType.Text = "[int]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Test Data:";
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.Location = new System.Drawing.Point(89, 70);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.Size = new System.Drawing.Size(386, 20);
            this.tbOutput.TabIndex = 6;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(401, 42);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output:";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.groupBox3);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 112);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(479, 60);
            this.panel9.TabIndex = 12;
            // 
            // lvDisplayFormats
            // 
            this.lvDisplayFormats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDisplayFormat,
            this.chExample});
            this.lvDisplayFormats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDisplayFormats.FullRowSelect = true;
            this.lvDisplayFormats.HideSelection = false;
            this.lvDisplayFormats.Location = new System.Drawing.Point(0, 0);
            this.lvDisplayFormats.MultiSelect = false;
            this.lvDisplayFormats.Name = "lvDisplayFormats";
            this.lvDisplayFormats.Size = new System.Drawing.Size(479, 233);
            this.lvDisplayFormats.TabIndex = 0;
            this.lvDisplayFormats.UseCompatibleStateImageBehavior = false;
            this.lvDisplayFormats.View = System.Windows.Forms.View.Details;
            this.lvDisplayFormats.DoubleClick += new System.EventHandler(this.lvDisplayFormats_DoubleClick);
            // 
            // chDisplayFormat
            // 
            this.chDisplayFormat.Text = "Display Format";
            this.chDisplayFormat.Width = 216;
            // 
            // chExample
            // 
            this.chExample.Text = "Example";
            this.chExample.Width = 214;
            // 
            // SelectDisplayFormats
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(495, 490);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SelectDisplayFormats";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Display Format";
            this.Load += new System.EventHandler(this.SelectDisplayFormats_Load);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.gbTestDisplayFormat.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel6;
        private ListViewSort lvDisplayFormats;
        private System.Windows.Forms.GroupBox gbTestDisplayFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTestData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDisplayFormat;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ColumnHeader chDisplayFormat;
        private System.Windows.Forms.ColumnHeader chExample;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.LinkLabel llblHelpFormatStrings;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel9;
    }
}