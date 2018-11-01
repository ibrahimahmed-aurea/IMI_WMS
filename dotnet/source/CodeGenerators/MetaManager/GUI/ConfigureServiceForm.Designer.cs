namespace Cdc.MetaManager.GUI
{
    partial class ConfigureServiceForm
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
            this.clearBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClearDisplayProp2 = new System.Windows.Forms.Button();
            this.displayProperty2Cbx = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.displayProperty1Cbx = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.keyPropertyCbx = new System.Windows.Forms.ComboBox();
            this.fintBtn = new System.Windows.Forms.Button();
            this.serviceMethodTbx = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.clearBtn);
            this.panel1.Controls.Add(this.cancelBtn);
            this.panel1.Controls.Add(this.okBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 149);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(401, 35);
            this.panel1.TabIndex = 5;
            // 
            // clearBtn
            // 
            this.clearBtn.Location = new System.Drawing.Point(8, 6);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(90, 23);
            this.clearBtn.TabIndex = 3;
            this.clearBtn.Text = "Clear All Values";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(318, 6);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(237, 6);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClearDisplayProp2);
            this.groupBox1.Controls.Add(this.displayProperty2Cbx);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.displayProperty1Cbx);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.keyPropertyCbx);
            this.groupBox1.Controls.Add(this.fintBtn);
            this.groupBox1.Controls.Add(this.serviceMethodTbx);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 3, 3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.groupBox1.Size = new System.Drawing.Size(388, 141);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configure Service Method";
            // 
            // btnClearDisplayProp2
            // 
            this.btnClearDisplayProp2.Location = new System.Drawing.Point(330, 107);
            this.btnClearDisplayProp2.Name = "btnClearDisplayProp2";
            this.btnClearDisplayProp2.Size = new System.Drawing.Size(51, 23);
            this.btnClearDisplayProp2.TabIndex = 5;
            this.btnClearDisplayProp2.Text = "Clear";
            this.btnClearDisplayProp2.UseVisualStyleBackColor = true;
            this.btnClearDisplayProp2.Click += new System.EventHandler(this.btnClearDisplayProp2_Click);
            // 
            // displayProperty2Cbx
            // 
            this.displayProperty2Cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.displayProperty2Cbx.FormattingEnabled = true;
            this.displayProperty2Cbx.Location = new System.Drawing.Point(112, 109);
            this.displayProperty2Cbx.Name = "displayProperty2Cbx";
            this.displayProperty2Cbx.Size = new System.Drawing.Size(213, 21);
            this.displayProperty2Cbx.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Display Property 2:";
            // 
            // displayProperty1Cbx
            // 
            this.displayProperty1Cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.displayProperty1Cbx.FormattingEnabled = true;
            this.displayProperty1Cbx.Location = new System.Drawing.Point(112, 82);
            this.displayProperty1Cbx.Name = "displayProperty1Cbx";
            this.displayProperty1Cbx.Size = new System.Drawing.Size(213, 21);
            this.displayProperty1Cbx.TabIndex = 3;
            this.displayProperty1Cbx.SelectedIndexChanged += new System.EventHandler(this.displayProperty1Cbx_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Display Property 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Key Property:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Service Method:";
            // 
            // keyPropertyCbx
            // 
            this.keyPropertyCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyPropertyCbx.FormattingEnabled = true;
            this.keyPropertyCbx.Location = new System.Drawing.Point(112, 55);
            this.keyPropertyCbx.Name = "keyPropertyCbx";
            this.keyPropertyCbx.Size = new System.Drawing.Size(213, 21);
            this.keyPropertyCbx.TabIndex = 2;
            this.keyPropertyCbx.SelectedIndexChanged += new System.EventHandler(this.keyPropertyCbx_SelectedIndexChanged);
            // 
            // fintBtn
            // 
            this.fintBtn.Location = new System.Drawing.Point(331, 26);
            this.fintBtn.Name = "fintBtn";
            this.fintBtn.Size = new System.Drawing.Size(29, 23);
            this.fintBtn.TabIndex = 1;
            this.fintBtn.Text = "...";
            this.fintBtn.UseVisualStyleBackColor = true;
            this.fintBtn.Click += new System.EventHandler(this.fintBtn_Click);
            // 
            // serviceMethodTbx
            // 
            this.serviceMethodTbx.Location = new System.Drawing.Point(112, 28);
            this.serviceMethodTbx.Name = "serviceMethodTbx";
            this.serviceMethodTbx.ReadOnly = true;
            this.serviceMethodTbx.Size = new System.Drawing.Size(213, 20);
            this.serviceMethodTbx.TabIndex = 0;
            // 
            // ConfigureServiceForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(401, 184);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureServiceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Service Method";
            this.Load += new System.EventHandler(this.ConfigureServiceForm_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button fintBtn;
        private System.Windows.Forms.TextBox serviceMethodTbx;
        private System.Windows.Forms.ComboBox displayProperty2Cbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox displayProperty1Cbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox keyPropertyCbx;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.Button btnClearDisplayProp2;
    }
}