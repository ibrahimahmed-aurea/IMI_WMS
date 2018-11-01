namespace Cdc.MetaManager.GUI
{
    partial class ImportDialogForm
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbUniqueness = new System.Windows.Forms.TextBox();
            this.cbAddDrilldownViews = new System.Windows.Forms.CheckBox();
            this.previewCb = new System.Windows.Forms.CheckBox();
            this.beCbx = new System.Windows.Forms.ComboBox();
            this.beBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.moduleCbx = new System.Windows.Forms.ComboBox();
            this.moduleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.browseBtn = new System.Windows.Forms.Button();
            this.filenameTbx = new System.Windows.Forms.TextBox();
            this.applicationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.openXMLFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moduleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbUniqueness);
            this.groupBox1.Controls.Add(this.cbAddDrilldownViews);
            this.groupBox1.Controls.Add(this.previewCb);
            this.groupBox1.Controls.Add(this.beCbx);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.moduleCbx);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.browseBtn);
            this.groupBox1.Controls.Add(this.filenameTbx);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 228);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import ODR XML File";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(145, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(351, 55);
            this.label5.TabIndex = 11;
            this.label5.Text = "Only use this if you need to import the same dialog more then once. The prefix wi" +
                "ll be added to object names to create unique named objects to avoid conflicts in" +
                " the database.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Import Uniqueness Prefix:";
            // 
            // tbUniqueness
            // 
            this.tbUniqueness.Location = new System.Drawing.Point(148, 145);
            this.tbUniqueness.Name = "tbUniqueness";
            this.tbUniqueness.Size = new System.Drawing.Size(122, 20);
            this.tbUniqueness.TabIndex = 10;
            // 
            // cbAddDrilldownViews
            // 
            this.cbAddDrilldownViews.AutoSize = true;
            this.cbAddDrilldownViews.Checked = true;
            this.cbAddDrilldownViews.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAddDrilldownViews.Location = new System.Drawing.Point(148, 99);
            this.cbAddDrilldownViews.Name = "cbAddDrilldownViews";
            this.cbAddDrilldownViews.Size = new System.Drawing.Size(122, 17);
            this.cbAddDrilldownViews.TabIndex = 7;
            this.cbAddDrilldownViews.Text = "Add Drilldown Views";
            this.cbAddDrilldownViews.UseVisualStyleBackColor = true;
            // 
            // previewCb
            // 
            this.previewCb.AutoSize = true;
            this.previewCb.Location = new System.Drawing.Point(148, 122);
            this.previewCb.Name = "previewCb";
            this.previewCb.Size = new System.Drawing.Size(88, 17);
            this.previewCb.TabIndex = 8;
            this.previewCb.Text = "Preview Only";
            this.previewCb.UseVisualStyleBackColor = true;
            // 
            // beCbx
            // 
            this.beCbx.DataSource = this.beBindingSource;
            this.beCbx.DisplayMember = "Name";
            this.beCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.beCbx.FormattingEnabled = true;
            this.beCbx.Location = new System.Drawing.Point(148, 45);
            this.beCbx.Name = "beCbx";
            this.beCbx.Size = new System.Drawing.Size(348, 21);
            this.beCbx.TabIndex = 3;
            // 
            // beBindingSource
            // 
            this.beBindingSource.DataSource = typeof(Cdc.MetaManager.DataAccess.Domain.BusinessEntity);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Business Entity:";
            // 
            // moduleCbx
            // 
            this.moduleCbx.DataSource = this.moduleBindingSource;
            this.moduleCbx.DisplayMember = "Name";
            this.moduleCbx.FormattingEnabled = true;
            this.moduleCbx.Location = new System.Drawing.Point(148, 18);
            this.moduleCbx.Name = "moduleCbx";
            this.moduleCbx.Size = new System.Drawing.Size(348, 21);
            this.moduleCbx.TabIndex = 1;
            this.moduleCbx.TextChanged += new System.EventHandler(this.fieldTextChanged);
            // 
            // moduleBindingSource
            // 
            this.moduleBindingSource.DataSource = typeof(Cdc.MetaManager.DataAccess.Domain.Module);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Module:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "ODR XML File:";
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(400, 70);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(96, 23);
            this.browseBtn.TabIndex = 6;
            this.browseBtn.Text = "Browse...";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // filenameTbx
            // 
            this.filenameTbx.Location = new System.Drawing.Point(148, 72);
            this.filenameTbx.Name = "filenameTbx";
            this.filenameTbx.Size = new System.Drawing.Size(246, 20);
            this.filenameTbx.TabIndex = 5;
            this.filenameTbx.TextChanged += new System.EventHandler(this.fieldTextChanged);
            // 
            // applicationBindingSource
            // 
            this.applicationBindingSource.DataSource = typeof(Cdc.MetaManager.GUI.ApplicationWrapper);
            // 
            // openXMLFileDialog
            // 
            this.openXMLFileDialog.FileName = "openFileDialog1";
            this.openXMLFileDialog.Filter = "XML files|*.xml|All files|*.*";
            // 
            // okBtn
            // 
            this.okBtn.Enabled = false;
            this.okBtn.Location = new System.Drawing.Point(298, 5);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(96, 23);
            this.okBtn.TabIndex = 0;
            this.okBtn.Text = "Import";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(400, 5);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(96, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Close";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cancelBtn);
            this.panel1.Controls.Add(this.okBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 231);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(507, 33);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(507, 228);
            this.panel2.TabIndex = 0;
            // 
            // ImportDialogForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(513, 267);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportDialogForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Delphi Dialog";
            this.Load += new System.EventHandler(this.ImportDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.moduleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.TextBox filenameTbx;
        private System.Windows.Forms.OpenFileDialog openXMLFileDialog;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.ComboBox moduleCbx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource applicationBindingSource;
        private System.Windows.Forms.BindingSource moduleBindingSource;
        private System.Windows.Forms.ComboBox beCbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource beBindingSource;
        private System.Windows.Forms.CheckBox previewCb;
        private System.Windows.Forms.CheckBox cbAddDrilldownViews;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbUniqueness;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
    }
}