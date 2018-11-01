namespace Cdc.MetaManager.GUI
{
    partial class PropertyMapForm2
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
            this.autoMaptPropertiesBtn = new System.Windows.Forms.Button();
            this.syncBtn = new System.Windows.Forms.Button();
            this.removePropertyBtn = new System.Windows.Forms.Button();
            this.addPropertyBtn = new System.Windows.Forms.Button();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.propertyMapControl = new Cdc.MetaManager.GUI.PropertyMapControl2();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.autoMaptPropertiesBtn);
            this.panel1.Controls.Add(this.syncBtn);
            this.panel1.Controls.Add(this.removePropertyBtn);
            this.panel1.Controls.Add(this.addPropertyBtn);
            this.panel1.Controls.Add(this.refreshBtn);
            this.panel1.Controls.Add(this.okBtn);
            this.panel1.Controls.Add(this.cancelBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 549);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 36);
            this.panel1.TabIndex = 2;
            // 
            // autoMaptPropertiesBtn
            // 
            this.autoMaptPropertiesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoMaptPropertiesBtn.Location = new System.Drawing.Point(134, 6);
            this.autoMaptPropertiesBtn.Name = "autoMaptPropertiesBtn";
            this.autoMaptPropertiesBtn.Size = new System.Drawing.Size(114, 23);
            this.autoMaptPropertiesBtn.TabIndex = 10;
            this.autoMaptPropertiesBtn.Text = "Auto Map Properties";
            this.autoMaptPropertiesBtn.UseVisualStyleBackColor = true;
            this.autoMaptPropertiesBtn.Click += new System.EventHandler(this.autoMaptPropertiesBtn_Click);
            // 
            // syncBtn
            // 
            this.syncBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.syncBtn.Location = new System.Drawing.Point(494, 6);
            this.syncBtn.Name = "syncBtn";
            this.syncBtn.Size = new System.Drawing.Size(75, 23);
            this.syncBtn.TabIndex = 9;
            this.syncBtn.Text = "Synchronize";
            this.syncBtn.UseVisualStyleBackColor = true;
            this.syncBtn.Click += new System.EventHandler(this.syncBtn_Click);
            // 
            // removePropertyBtn
            // 
            this.removePropertyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removePropertyBtn.Location = new System.Drawing.Point(374, 6);
            this.removePropertyBtn.Name = "removePropertyBtn";
            this.removePropertyBtn.Size = new System.Drawing.Size(114, 23);
            this.removePropertyBtn.TabIndex = 8;
            this.removePropertyBtn.Text = "Remove Property";
            this.removePropertyBtn.UseVisualStyleBackColor = true;
            this.removePropertyBtn.Click += new System.EventHandler(this.removePropertyBtn_Click);
            // 
            // addPropertyBtn
            // 
            this.addPropertyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addPropertyBtn.Location = new System.Drawing.Point(254, 6);
            this.addPropertyBtn.Name = "addPropertyBtn";
            this.addPropertyBtn.Size = new System.Drawing.Size(114, 23);
            this.addPropertyBtn.TabIndex = 7;
            this.addPropertyBtn.Text = "Add Property...";
            this.addPropertyBtn.UseVisualStyleBackColor = true;
            this.addPropertyBtn.Click += new System.EventHandler(this.addPropertyBtn_Click);
            // 
            // refreshBtn
            // 
            this.refreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshBtn.Location = new System.Drawing.Point(575, 7);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(75, 23);
            this.refreshBtn.TabIndex = 6;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(656, 7);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 5;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(737, 7);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // propertyMapControl
            // 
            this.propertyMapControl.AllowNonUniquePropertyNames = false;
            this.propertyMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyMapControl.EnablePropertiesByDefault = false;
            this.propertyMapControl.IsEditable = false;
            this.propertyMapControl.Location = new System.Drawing.Point(0, 0);
            this.propertyMapControl.MultiSelMode = false;
            this.propertyMapControl.Name = "propertyMapControl";
            this.propertyMapControl.PropertyMap = null;
            this.propertyMapControl.RequestProperties = null;
            this.propertyMapControl.Size = new System.Drawing.Size(818, 549);
            this.propertyMapControl.SourceProperties = null;
            this.propertyMapControl.TabIndex = 0;
            this.propertyMapControl.TargetProperties = null;
            // 
            // PropertyMapForm2
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(818, 585);
            this.Controls.Add(this.propertyMapControl);
            this.Controls.Add(this.panel1);
            this.Name = "PropertyMapForm2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Property Mapper";
            this.Load += new System.EventHandler(this.PropertyMapForm2_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyMapControl2 propertyMapControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Button addPropertyBtn;
        private System.Windows.Forms.Button removePropertyBtn;
        private System.Windows.Forms.Button syncBtn;
        private System.Windows.Forms.Button autoMaptPropertiesBtn;
    }
}