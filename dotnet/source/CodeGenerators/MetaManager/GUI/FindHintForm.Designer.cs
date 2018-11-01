
namespace Cdc.MetaManager.GUI
{
    partial class FindHintForm
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
            this.gbSelect = new System.Windows.Forms.GroupBox();
            this.hintListView = new ListViewSort();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbNameTableColBE = new System.Windows.Forms.GroupBox();
            this.clearBtn = new System.Windows.Forms.Button();
            this.idTbx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.searchBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.columnTbx = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableTbx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textTbx = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.createBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.fullTextTbx = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.gbSelect.SuspendLayout();
            this.gbNameTableColBE.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbSelect);
            this.panel1.Controls.Add(this.gbNameTableColBE);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(695, 355);
            this.panel1.TabIndex = 0;
            // 
            // gbSelect
            // 
            this.gbSelect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbSelect.Controls.Add(this.hintListView);
            this.gbSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSelect.Location = new System.Drawing.Point(0, 131);
            this.gbSelect.Name = "gbSelect";
            this.gbSelect.Padding = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.gbSelect.Size = new System.Drawing.Size(695, 224);
            this.gbSelect.TabIndex = 3;
            this.gbSelect.TabStop = false;
            this.gbSelect.Text = "Result (max 1000 items)";
            this.gbSelect.Enter += new System.EventHandler(this.gbSelect_Enter);
            // 
            // hintListView
            // 
            this.hintListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader9,
            this.columnHeader10});
            this.hintListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hintListView.FullRowSelect = true;
            this.hintListView.GridLines = true;
            this.hintListView.HideSelection = false;
            this.hintListView.Location = new System.Drawing.Point(3, 16);
            this.hintListView.Margin = new System.Windows.Forms.Padding(0);
            this.hintListView.MultiSelect = false;
            this.hintListView.Name = "hintListView";
            this.hintListView.Size = new System.Drawing.Size(689, 196);
            this.hintListView.TabIndex = 0;
            this.hintListView.UseCompatibleStateImageBehavior = false;
            this.hintListView.View = System.Windows.Forms.View.Details;
            this.hintListView.SelectedIndexChanged += new System.EventHandler(this.propertyListView_SelectedIndexChanged);
            this.hintListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.propertyListView_KeyDown);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Id";
            this.columnHeader6.Width = 51;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Text";
            this.columnHeader7.Width = 242;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Table";
            this.columnHeader9.Width = 109;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Column";
            this.columnHeader10.Width = 111;
            // 
            // gbNameTableColBE
            // 
            this.gbNameTableColBE.Controls.Add(this.clearBtn);
            this.gbNameTableColBE.Controls.Add(this.idTbx);
            this.gbNameTableColBE.Controls.Add(this.label4);
            this.gbNameTableColBE.Controls.Add(this.searchBtn);
            this.gbNameTableColBE.Controls.Add(this.label3);
            this.gbNameTableColBE.Controls.Add(this.columnTbx);
            this.gbNameTableColBE.Controls.Add(this.label2);
            this.gbNameTableColBE.Controls.Add(this.tableTbx);
            this.gbNameTableColBE.Controls.Add(this.label1);
            this.gbNameTableColBE.Controls.Add(this.textTbx);
            this.gbNameTableColBE.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbNameTableColBE.Location = new System.Drawing.Point(0, 0);
            this.gbNameTableColBE.Name = "gbNameTableColBE";
            this.gbNameTableColBE.Size = new System.Drawing.Size(695, 131);
            this.gbNameTableColBE.TabIndex = 1;
            this.gbNameTableColBE.TabStop = false;
            this.gbNameTableColBE.Text = "Search Fields";
            // 
            // clearBtn
            // 
            this.clearBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.clearBtn.Location = new System.Drawing.Point(593, 44);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(96, 23);
            this.clearBtn.TabIndex = 5;
            this.clearBtn.Text = "Clear";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // idTbx
            // 
            this.idTbx.Location = new System.Drawing.Point(88, 21);
            this.idTbx.Name = "idTbx";
            this.idTbx.Size = new System.Drawing.Size(238, 20);
            this.idTbx.TabIndex = 0;
            this.idTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.idTbx_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Id:";
            // 
            // searchBtn
            // 
            this.searchBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.searchBtn.Location = new System.Drawing.Point(593, 15);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(96, 23);
            this.searchBtn.TabIndex = 4;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Column:";
            // 
            // columnTbx
            // 
            this.columnTbx.Location = new System.Drawing.Point(88, 73);
            this.columnTbx.Name = "columnTbx";
            this.columnTbx.Size = new System.Drawing.Size(144, 20);
            this.columnTbx.TabIndex = 2;
            this.columnTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.columnTbx_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Table:";
            // 
            // tableTbx
            // 
            this.tableTbx.Location = new System.Drawing.Point(88, 47);
            this.tableTbx.Name = "tableTbx";
            this.tableTbx.Size = new System.Drawing.Size(144, 20);
            this.tableTbx.TabIndex = 1;
            this.tableTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tableTbx_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text:";
            // 
            // textTbx
            // 
            this.textTbx.Location = new System.Drawing.Point(88, 99);
            this.textTbx.Name = "textTbx";
            this.textTbx.Size = new System.Drawing.Size(441, 20);
            this.textTbx.TabIndex = 3;
            this.textTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textTbx_KeyDown);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(6, 361);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(695, 6);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.deleteBtn);
            this.panel2.Controls.Add(this.createBtn);
            this.panel2.Controls.Add(this.okBtn);
            this.panel2.Controls.Add(this.cancelBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(6, 488);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(695, 44);
            this.panel2.TabIndex = 3;
            // 
            // deleteBtn
            // 
            this.deleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteBtn.Enabled = false;
            this.deleteBtn.Location = new System.Drawing.Point(112, 11);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(96, 23);
            this.deleteBtn.TabIndex = 8;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.UseVisualStyleBackColor = true;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // createBtn
            // 
            this.createBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createBtn.Location = new System.Drawing.Point(10, 11);
            this.createBtn.Name = "createBtn";
            this.createBtn.Size = new System.Drawing.Size(96, 23);
            this.createBtn.TabIndex = 7;
            this.createBtn.Text = "Create...";
            this.createBtn.UseVisualStyleBackColor = true;
            this.createBtn.Click += new System.EventHandler(this.createBtn_Click);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(494, 11);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(96, 23);
            this.okBtn.TabIndex = 10;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(596, 11);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(96, 23);
            this.cancelBtn.TabIndex = 11;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(6, 367);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(695, 121);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Text";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.fullTextTbx);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(689, 102);
            this.panel4.TabIndex = 2;
            // 
            // fullTextTbx
            // 
            this.fullTextTbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fullTextTbx.Location = new System.Drawing.Point(0, 0);
            this.fullTextTbx.Multiline = true;
            this.fullTextTbx.Name = "fullTextTbx";
            this.fullTextTbx.Size = new System.Drawing.Size(689, 102);
            this.fullTextTbx.TabIndex = 6;
            this.fullTextTbx.TextChanged += new System.EventHandler(this.fullTextTbx_TextChanged);
            this.fullTextTbx.Leave += new System.EventHandler(this.fullTextTbx_Leave);
            // 
            // FindHintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(707, 538);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "FindHintForm";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hints";
            this.Load += new System.EventHandler(this.FindPropertyForm_Load);
            this.panel1.ResumeLayout(false);
            this.gbSelect.ResumeLayout(false);
            this.gbNameTableColBE.ResumeLayout(false);
            this.gbNameTableColBE.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private ListViewSort hintListView;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.GroupBox gbNameTableColBE;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox columnTbx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tableTbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textTbx;
        private System.Windows.Forms.GroupBox gbSelect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox fullTextTbx;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button createBtn;
        private System.Windows.Forms.TextBox idTbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button clearBtn;
    }
}