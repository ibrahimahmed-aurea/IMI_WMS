namespace Cdc.MetaManager.GUI.Loader
{
    partial class SelectInstance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectInstance));
            this.InstanceslistBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartInstancebutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InstanceslistBox
            // 
            this.InstanceslistBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstanceslistBox.FormattingEnabled = true;
            this.InstanceslistBox.Location = new System.Drawing.Point(12, 25);
            this.InstanceslistBox.Name = "InstanceslistBox";
            this.InstanceslistBox.Size = new System.Drawing.Size(402, 199);
            this.InstanceslistBox.TabIndex = 0;
            this.InstanceslistBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InstanceslistBox_KeyDown);
            this.InstanceslistBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InstanceslistBox_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available Instances";
            // 
            // StartInstancebutton
            // 
            this.StartInstancebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartInstancebutton.Location = new System.Drawing.Point(319, 235);
            this.StartInstancebutton.Name = "StartInstancebutton";
            this.StartInstancebutton.Size = new System.Drawing.Size(95, 23);
            this.StartInstancebutton.TabIndex = 2;
            this.StartInstancebutton.Text = "Start Instance";
            this.StartInstancebutton.UseVisualStyleBackColor = true;
            this.StartInstancebutton.Click += new System.EventHandler(this.StartInstancebutton_Click);
            // 
            // SelectInstance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 270);
            this.Controls.Add(this.StartInstancebutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InstanceslistBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectInstance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Instance";
            this.Load += new System.EventHandler(this.SelectInstance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox InstanceslistBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartInstancebutton;
    }
}

