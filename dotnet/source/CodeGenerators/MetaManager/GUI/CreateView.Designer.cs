namespace Cdc.MetaManager.GUI
{
    partial class CreateView
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
            this.Layout = new System.Windows.Forms.GroupBox();
            this.rbtnGroupbox = new System.Windows.Forms.RadioButton();
            this.rbtnTwoWayListBox = new System.Windows.Forms.RadioButton();
            this.rbtnGrid = new System.Windows.Forms.RadioButton();
            this.btnSelectServiceMethod = new System.Windows.Forms.Button();
            this.tbServiceMethod = new System.Windows.Forms.TextBox();
            this.cbBusinessEntity = new System.Windows.Forms.ComboBox();
            this.businessEntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.Layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.businessEntityBindingSource)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Layout);
            this.groupBox1.Controls.Add(this.btnSelectServiceMethod);
            this.groupBox1.Controls.Add(this.tbServiceMethod);
            this.groupBox1.Controls.Add(this.cbBusinessEntity);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbTitle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 238);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View";
            // 
            // Layout
            // 
            this.Layout.AutoSize = true;
            this.Layout.Controls.Add(this.rbtnGroupbox);
            this.Layout.Controls.Add(this.rbtnTwoWayListBox);
            this.Layout.Controls.Add(this.rbtnGrid);
            this.Layout.Location = new System.Drawing.Point(136, 98);
            this.Layout.Name = "Layout";
            this.Layout.Size = new System.Drawing.Size(229, 101);
            this.Layout.TabIndex = 18;
            this.Layout.TabStop = false;
            this.Layout.Text = "Layout";
            // 
            // rbtnGroupbox
            // 
            this.rbtnGroupbox.AutoSize = true;
            this.rbtnGroupbox.Checked = true;
            this.rbtnGroupbox.Location = new System.Drawing.Point(16, 19);
            this.rbtnGroupbox.Name = "rbtnGroupbox";
            this.rbtnGroupbox.Size = new System.Drawing.Size(71, 17);
            this.rbtnGroupbox.TabIndex = 16;
            this.rbtnGroupbox.TabStop = true;
            this.rbtnGroupbox.Text = "Groupbox";
            this.rbtnGroupbox.UseVisualStyleBackColor = true;
            // 
            // rbtnTwoWayListBox
            // 
            this.rbtnTwoWayListBox.AutoSize = true;
            this.rbtnTwoWayListBox.Location = new System.Drawing.Point(16, 65);
            this.rbtnTwoWayListBox.Name = "rbtnTwoWayListBox";
            this.rbtnTwoWayListBox.Size = new System.Drawing.Size(104, 17);
            this.rbtnTwoWayListBox.TabIndex = 17;
            this.rbtnTwoWayListBox.Text = "Two-way Listbox";
            this.rbtnTwoWayListBox.UseVisualStyleBackColor = true;
            // 
            // rbtnGrid
            // 
            this.rbtnGrid.AutoSize = true;
            this.rbtnGrid.Location = new System.Drawing.Point(16, 42);
            this.rbtnGrid.Name = "rbtnGrid";
            this.rbtnGrid.Size = new System.Drawing.Size(44, 17);
            this.rbtnGrid.TabIndex = 15;
            this.rbtnGrid.Text = "Grid";
            this.rbtnGrid.UseVisualStyleBackColor = true;
            // 
            // btnSelectServiceMethod
            // 
            this.btnSelectServiceMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectServiceMethod.Location = new System.Drawing.Point(504, 208);
            this.btnSelectServiceMethod.Name = "btnSelectServiceMethod";
            this.btnSelectServiceMethod.Size = new System.Drawing.Size(25, 22);
            this.btnSelectServiceMethod.TabIndex = 14;
            this.btnSelectServiceMethod.Text = "...";
            this.btnSelectServiceMethod.UseVisualStyleBackColor = true;
            this.btnSelectServiceMethod.Click += new System.EventHandler(this.btnSelectServiceMethod_Click);
            // 
            // tbServiceMethod
            // 
            this.tbServiceMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServiceMethod.Location = new System.Drawing.Point(136, 210);
            this.tbServiceMethod.Name = "tbServiceMethod";
            this.tbServiceMethod.ReadOnly = true;
            this.tbServiceMethod.Size = new System.Drawing.Size(362, 20);
            this.tbServiceMethod.TabIndex = 13;
            this.tbServiceMethod.TabStop = false;
            // 
            // cbBusinessEntity
            // 
            this.cbBusinessEntity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBusinessEntity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.cbBusinessEntity.DataSource = this.businessEntityBindingSource;
            this.cbBusinessEntity.DisplayMember = "Name";
            this.cbBusinessEntity.FormattingEnabled = true;
            this.cbBusinessEntity.Location = new System.Drawing.Point(136, 71);
            this.cbBusinessEntity.Name = "cbBusinessEntity";
            this.cbBusinessEntity.Size = new System.Drawing.Size(393, 21);
            this.cbBusinessEntity.TabIndex = 5;
            this.cbBusinessEntity.TextChanged += new System.EventHandler(this.cbBusinessEntity_TextChanged);
            // 
            // businessEntityBindingSource
            // 
            this.businessEntityBindingSource.DataSource = typeof(Cdc.MetaManager.DataAccess.Domain.BusinessEntity);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Business Entity:";
            // 
            // tbTitle
            // 
            this.tbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTitle.Location = new System.Drawing.Point(136, 45);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(393, 20);
            this.tbTitle.TabIndex = 3;
            this.tbTitle.TextChanged += new System.EventHandler(this.tbTitle_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Title:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(136, 19);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(393, 20);
            this.tbName.TabIndex = 1;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this.label4.Location = new System.Drawing.Point(9, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Service Method:";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnOK);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 240);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(536, 35);
            this.panel4.TabIndex = 11;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(373, 7);
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
            this.btnCancel.Location = new System.Drawing.Point(454, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CreateView
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(542, 278);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateView";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create View";
            this.Load += new System.EventHandler(this.CreateView_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Layout.ResumeLayout(false);
            this.Layout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.businessEntityBindingSource)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbBusinessEntity;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.BindingSource businessEntityBindingSource;
        private System.Windows.Forms.Button btnSelectServiceMethod;
        private System.Windows.Forms.TextBox tbServiceMethod;
        private System.Windows.Forms.RadioButton rbtnTwoWayListBox;
        private System.Windows.Forms.RadioButton rbtnGroupbox;
        private System.Windows.Forms.RadioButton rbtnGrid;
        new private System.Windows.Forms.GroupBox Layout;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}