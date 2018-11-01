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
            this.components = new System.ComponentModel.Container();
            this.renderPanel = new Imi.Wms.Mobile.UI.Shared.RenderPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.inputPanel = new Microsoft.WindowsCE.Forms.InputPanel(this.components);
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.renderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderPanel
            // 
            this.renderPanel.Controls.Add(this.label1);
            this.renderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderPanel.Location = new System.Drawing.Point(0, 0);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(240, 268);
            this.renderPanel.Click += new System.EventHandler(this.renderPanel_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(0, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 27);
            this.label1.Text = "Waiting for application...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RenderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.renderPanel);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "RenderForm";
            this.Text = "IMI iWMS Thin Client";
            this.Load += new System.EventHandler(this.RenderForm_Load);
            this.Resize += new System.EventHandler(this.RenderForm_Resize);
            this.renderPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Imi.Wms.Mobile.UI.Shared.RenderPanel renderPanel;
        private System.Windows.Forms.Label label1;

        #endregion
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel;
        private System.Windows.Forms.MainMenu mainMenu;
    }
}