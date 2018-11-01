namespace Cdc.MetaManager.GUI
{
    partial class AddViewToViewNode
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbParentView = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbParentViewNode = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbIsCustomView = new System.Windows.Forms.CheckBox();
            this.btnSelectView = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbViewName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbViewTitle = new System.Windows.Forms.TextBox();
            this.btnCreateView = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(660, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 213);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 213);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(655, 5);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnOK);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 178);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(655, 35);
            this.panel4.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(327, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(408, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(655, 173);
            this.panel5.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbParentView);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbParentViewNode);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(655, 76);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parent ViewNode";
            // 
            // tbParentView
            // 
            this.tbParentView.Location = new System.Drawing.Point(134, 45);
            this.tbParentView.Name = "tbParentView";
            this.tbParentView.ReadOnly = true;
            this.tbParentView.Size = new System.Drawing.Size(349, 20);
            this.tbParentView.TabIndex = 11;
            this.tbParentView.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "ViewNode\'s View:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "ViewNode Id:";
            // 
            // tbParentViewNode
            // 
            this.tbParentViewNode.Location = new System.Drawing.Point(134, 19);
            this.tbParentViewNode.Name = "tbParentViewNode";
            this.tbParentViewNode.ReadOnly = true;
            this.tbParentViewNode.Size = new System.Drawing.Size(349, 20);
            this.tbParentViewNode.TabIndex = 9;
            this.tbParentViewNode.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 92);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(655, 5);
            this.panel6.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCreateView);
            this.groupBox1.Controls.Add(this.cbIsCustomView);
            this.groupBox1.Controls.Add(this.btnSelectView);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbViewName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbViewTitle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(655, 92);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View to Add";
            // 
            // cbIsCustomView
            // 
            this.cbIsCustomView.AutoCheck = false;
            this.cbIsCustomView.AutoSize = true;
            this.cbIsCustomView.Location = new System.Drawing.Point(134, 72);
            this.cbIsCustomView.Name = "cbIsCustomView";
            this.cbIsCustomView.Size = new System.Drawing.Size(87, 17);
            this.cbIsCustomView.TabIndex = 15;
            this.cbIsCustomView.TabStop = false;
            this.cbIsCustomView.Text = "Custom View";
            this.cbIsCustomView.UseVisualStyleBackColor = true;
            // 
            // btnSelectView
            // 
            this.btnSelectView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectView.Location = new System.Drawing.Point(505, 19);
            this.btnSelectView.Name = "btnSelectView";
            this.btnSelectView.Size = new System.Drawing.Size(110, 23);
            this.btnSelectView.TabIndex = 14;
            this.btnSelectView.Text = "Select Existing...";
            this.btnSelectView.UseVisualStyleBackColor = true;
            this.btnSelectView.Click += new System.EventHandler(this.btnSelectView_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name:";
            // 
            // tbViewName
            // 
            this.tbViewName.Location = new System.Drawing.Point(134, 19);
            this.tbViewName.Name = "tbViewName";
            this.tbViewName.ReadOnly = true;
            this.tbViewName.Size = new System.Drawing.Size(349, 20);
            this.tbViewName.TabIndex = 11;
            this.tbViewName.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Title:";
            // 
            // tbViewTitle
            // 
            this.tbViewTitle.Location = new System.Drawing.Point(134, 45);
            this.tbViewTitle.Name = "tbViewTitle";
            this.tbViewTitle.ReadOnly = true;
            this.tbViewTitle.Size = new System.Drawing.Size(349, 20);
            this.tbViewTitle.TabIndex = 13;
            this.tbViewTitle.TabStop = false;
            // 
            // btnCreateView
            // 
            this.btnCreateView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateView.Location = new System.Drawing.Point(505, 43);
            this.btnCreateView.Name = "btnCreateView";
            this.btnCreateView.Size = new System.Drawing.Size(110, 23);
            this.btnCreateView.TabIndex = 16;
            this.btnCreateView.Text = "Create New...";
            this.btnCreateView.UseVisualStyleBackColor = true;
            this.btnCreateView.Click += new System.EventHandler(this.btnCreateView_Click);
            // 
            // AddViewToViewNode
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(665, 213);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "AddViewToViewNode";
            this.Text = "Add Selected View to Dialog";
            this.Load += new System.EventHandler(this.AddViewToViewNode_Load);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox tbParentView;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbParentViewNode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbViewName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbViewTitle;
        private System.Windows.Forms.Button btnSelectView;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbIsCustomView;
        private System.Windows.Forms.Button btnCreateView;
    }
}