namespace Cdc.MetaManager.GUI
{
    partial class GenerateCode
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
            this.GenerateProgressBar = new System.Windows.Forms.ProgressBar();
            this.Statuslabel = new System.Windows.Forms.Label();
            this.Generatebutton = new System.Windows.Forms.Button();
            this.TimeElapsedlabel = new System.Windows.Forms.Label();
            this.SelectAllModulescheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectAllServicescheckBox = new System.Windows.Forms.CheckBox();
            this.ServiceslistViewSort = new Cdc.MetaManager.GUI.ListViewSort();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.ModulesListViewSort = new Cdc.MetaManager.GUI.ListViewSort();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IgnoreCheckOutscheckBox = new System.Windows.Forms.CheckBox();
            this.Overall1label = new System.Windows.Forms.Label();
            this.Overall2label = new System.Windows.Forms.Label();
            this.Overall3label = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GenerateProgressBar
            // 
            this.GenerateProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateProgressBar.Location = new System.Drawing.Point(12, 515);
            this.GenerateProgressBar.Name = "GenerateProgressBar";
            this.GenerateProgressBar.Size = new System.Drawing.Size(755, 10);
            this.GenerateProgressBar.TabIndex = 1;
            // 
            // Statuslabel
            // 
            this.Statuslabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Statuslabel.Location = new System.Drawing.Point(12, 499);
            this.Statuslabel.Name = "Statuslabel";
            this.Statuslabel.Size = new System.Drawing.Size(755, 13);
            this.Statuslabel.TabIndex = 2;
            // 
            // Generatebutton
            // 
            this.Generatebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Generatebutton.Location = new System.Drawing.Point(692, 404);
            this.Generatebutton.Name = "Generatebutton";
            this.Generatebutton.Size = new System.Drawing.Size(75, 23);
            this.Generatebutton.TabIndex = 3;
            this.Generatebutton.Text = "Start";
            this.Generatebutton.UseVisualStyleBackColor = true;
            this.Generatebutton.Click += new System.EventHandler(this.Generatebutton_Click);
            // 
            // TimeElapsedlabel
            // 
            this.TimeElapsedlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TimeElapsedlabel.AutoSize = true;
            this.TimeElapsedlabel.Location = new System.Drawing.Point(90, 480);
            this.TimeElapsedlabel.Name = "TimeElapsedlabel";
            this.TimeElapsedlabel.Size = new System.Drawing.Size(49, 13);
            this.TimeElapsedlabel.TabIndex = 4;
            this.TimeElapsedlabel.Text = "00:00:00";
            // 
            // SelectAllModulescheckBox
            // 
            this.SelectAllModulescheckBox.AutoSize = true;
            this.SelectAllModulescheckBox.Location = new System.Drawing.Point(18, 32);
            this.SelectAllModulescheckBox.Name = "SelectAllModulescheckBox";
            this.SelectAllModulescheckBox.Size = new System.Drawing.Size(15, 14);
            this.SelectAllModulescheckBox.TabIndex = 4;
            this.SelectAllModulescheckBox.UseVisualStyleBackColor = true;
            this.SelectAllModulescheckBox.CheckedChanged += new System.EventHandler(this.SelectAllModulescheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Frontend";
            // 
            // SelectAllServicescheckBox
            // 
            this.SelectAllServicescheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectAllServicescheckBox.AutoSize = true;
            this.SelectAllServicescheckBox.Location = new System.Drawing.Point(399, 32);
            this.SelectAllServicescheckBox.Name = "SelectAllServicescheckBox";
            this.SelectAllServicescheckBox.Size = new System.Drawing.Size(15, 14);
            this.SelectAllServicescheckBox.TabIndex = 5;
            this.SelectAllServicescheckBox.UseVisualStyleBackColor = true;
            this.SelectAllServicescheckBox.CheckedChanged += new System.EventHandler(this.SelectAllServicescheckBox_CheckedChanged);
            // 
            // ServiceslistViewSort
            // 
            this.ServiceslistViewSort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ServiceslistViewSort.CheckBoxes = true;
            this.ServiceslistViewSort.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ServiceslistViewSort.FullRowSelect = true;
            this.ServiceslistViewSort.GridLines = true;
            this.ServiceslistViewSort.Location = new System.Drawing.Point(393, 25);
            this.ServiceslistViewSort.MultiSelect = false;
            this.ServiceslistViewSort.Name = "ServiceslistViewSort";
            this.ServiceslistViewSort.Size = new System.Drawing.Size(374, 373);
            this.ServiceslistViewSort.TabIndex = 4;
            this.ServiceslistViewSort.UseCompatibleStateImageBehavior = false;
            this.ServiceslistViewSort.View = System.Windows.Forms.View.Details;
            this.ServiceslistViewSort.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.ServiceslistViewSort_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "     Name";
            this.columnHeader1.Width = 350;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Backend";
            // 
            // ModulesListViewSort
            // 
            this.ModulesListViewSort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModulesListViewSort.CheckBoxes = true;
            this.ModulesListViewSort.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.ModulesListViewSort.FullRowSelect = true;
            this.ModulesListViewSort.GridLines = true;
            this.ModulesListViewSort.Location = new System.Drawing.Point(12, 25);
            this.ModulesListViewSort.MultiSelect = false;
            this.ModulesListViewSort.Name = "ModulesListViewSort";
            this.ModulesListViewSort.Size = new System.Drawing.Size(375, 373);
            this.ModulesListViewSort.TabIndex = 6;
            this.ModulesListViewSort.UseCompatibleStateImageBehavior = false;
            this.ModulesListViewSort.View = System.Windows.Forms.View.Details;
            this.ModulesListViewSort.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.ModulesListViewSort_ItemChecked);
            // 
            // chName
            // 
            this.chName.Text = "     Name";
            this.chName.Width = 350;
            // 
            // IgnoreCheckOutscheckBox
            // 
            this.IgnoreCheckOutscheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.IgnoreCheckOutscheckBox.AutoSize = true;
            this.IgnoreCheckOutscheckBox.Location = new System.Drawing.Point(15, 408);
            this.IgnoreCheckOutscheckBox.Name = "IgnoreCheckOutscheckBox";
            this.IgnoreCheckOutscheckBox.Size = new System.Drawing.Size(227, 17);
            this.IgnoreCheckOutscheckBox.TabIndex = 7;
            this.IgnoreCheckOutscheckBox.Text = "Ignore changes in my checked out objects";
            this.IgnoreCheckOutscheckBox.UseVisualStyleBackColor = true;
            this.IgnoreCheckOutscheckBox.CheckedChanged += new System.EventHandler(this.IgnoreCheckOutscheckBox_CheckedChanged);
            // 
            // Overall1label
            // 
            this.Overall1label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Overall1label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Overall1label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Overall1label.Location = new System.Drawing.Point(12, 445);
            this.Overall1label.Name = "Overall1label";
            this.Overall1label.Size = new System.Drawing.Size(252, 23);
            this.Overall1label.TabIndex = 8;
            this.Overall1label.Text = "Collecting Checkouts";
            this.Overall1label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Overall2label
            // 
            this.Overall2label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Overall2label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Overall2label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Overall2label.Location = new System.Drawing.Point(264, 445);
            this.Overall2label.Name = "Overall2label";
            this.Overall2label.Size = new System.Drawing.Size(251, 23);
            this.Overall2label.TabIndex = 9;
            this.Overall2label.Text = "Collecting Objects";
            this.Overall2label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Overall3label
            // 
            this.Overall3label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Overall3label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Overall3label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Overall3label.Location = new System.Drawing.Point(515, 445);
            this.Overall3label.Name = "Overall3label";
            this.Overall3label.Size = new System.Drawing.Size(252, 23);
            this.Overall3label.TabIndex = 10;
            this.Overall3label.Text = "Generating Code";
            this.Overall3label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 480);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Time Elapsed:";
            // 
            // GenerateCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 530);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SelectAllServicescheckBox);
            this.Controls.Add(this.SelectAllModulescheckBox);
            this.Controls.Add(this.Overall2label);
            this.Controls.Add(this.Overall3label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ServiceslistViewSort);
            this.Controls.Add(this.ModulesListViewSort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Overall1label);
            this.Controls.Add(this.IgnoreCheckOutscheckBox);
            this.Controls.Add(this.Generatebutton);
            this.Controls.Add(this.Statuslabel);
            this.Controls.Add(this.TimeElapsedlabel);
            this.Controls.Add(this.GenerateProgressBar);
            this.MaximumSize = new System.Drawing.Size(795, 2000);
            this.MinimumSize = new System.Drawing.Size(795, 174);
            this.Name = "GenerateCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Code";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenerateCode_FormClosing);
            this.Load += new System.EventHandler(this.GenerateCode_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar GenerateProgressBar;
        private System.Windows.Forms.Label Statuslabel;
        private System.Windows.Forms.Button Generatebutton;
        private System.Windows.Forms.Label TimeElapsedlabel;
        private System.Windows.Forms.Label label1;
        private ListViewSort ServiceslistViewSort;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox SelectAllModulescheckBox;
        private System.Windows.Forms.CheckBox SelectAllServicescheckBox;
        private ListViewSort ModulesListViewSort;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.CheckBox IgnoreCheckOutscheckBox;
        private System.Windows.Forms.Label Overall1label;
        private System.Windows.Forms.Label Overall2label;
        private System.Windows.Forms.Label Overall3label;
        private System.Windows.Forms.Label label6;
    }
}