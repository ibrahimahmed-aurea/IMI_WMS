using Imi.Wms.Mobile.UI.Shared;
namespace Imi.Wms.Mobile.UI
{
    partial class RenderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenderForm));
            this.Titlelabel = new System.Windows.Forms.Label();
            this.renderPanel = new Imi.Wms.Mobile.UI.Shared.RenderPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.renderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Titlelabel
            // 
            this.Titlelabel.AutoSize = true;
            this.Titlelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Titlelabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Titlelabel.Location = new System.Drawing.Point(12, 9);
            this.Titlelabel.Name = "Titlelabel";
            this.Titlelabel.Size = new System.Drawing.Size(0, 20);
            this.Titlelabel.TabIndex = 7;
            this.Titlelabel.Visible = false;
            // 
            // renderPanel
            // 
            this.renderPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.renderPanel.AutoScaleFactor = new System.Drawing.SizeF(1F, 1F);
            this.renderPanel.Controls.Add(this.label1);
            this.renderPanel.Location = new System.Drawing.Point(-1, -1);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.NativeDriver = null;
            this.renderPanel.Size = new System.Drawing.Size(245, 243);
            this.renderPanel.TabIndex = 6;
            this.renderPanel.SizeChanged += new System.EventHandler(this.renderPanel_SizeChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 243);
            this.label1.TabIndex = 0;
            this.label1.Text = "Waiting for application....";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RenderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(244, 242);
            this.Controls.Add(this.Titlelabel);
            this.Controls.Add(this.renderPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenderForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IMI iWMS Thin Client";
            this.Load += new System.EventHandler(this.RenderForm_Load);
            this.renderPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Titlelabel;
        private RenderPanel renderPanel;

    }
}